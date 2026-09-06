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
using Architecture.WinApp.CommissionAgent.FromWise_Helper_Methods;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.ModernHistories;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Cmagt.Approvals;

public class frmApprovalCommissionTradeBill : Form
{
	private WaitForm wf = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGridDbCall = new DataTable();

	private DataTable dtCustomerDetail = new DataTable();

	private int currentPage = 1;

	private int pageSize = 20;

	private int totalRecords;

	public int DocumentTypeId;

	public int RecId;

	public int DefaultDaysToLessFromHistoryFromDate;

	public bool flagUnApprove;

	private bool FormInitialized;

	private bool AscOrDesc = true;

	private bool SelectAll;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

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

	private ToolStripButton btnApproved;

	internal UltraCombo CmbItemNameFilter;

	private Label label1;

	private FlowLayoutPanel FlpSideBar;

	internal UltraCombo CmbSupplierNameFilter;

	private Label label7;

	internal UltraCombo CmbTradingAccountFilter;

	private Label label14;

	private Panel TotalRecordPanel;

	private Label label5;

	private Label TotalHeaderRecords;

	private ToolStripButton BtnShowHideFilter;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private Panel panel3;

	private Label labelFormHeading;

	private TextBox txtApprovalRemarks;

	private Label label2;

	private Panel PanelMainSelectedDocData;

	private Button BtnLoadNextRecord;

	private Button BtnLoadPreviousRecord;

	private Label txtDocDate;

	private Label label3;

	private Label txtDocNo;

	private Label label6;

	private Label txtDeliveryTermPurchase;

	internal GridEX grdCustomerDetail;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel PanelMainSelectedNetPreviousDocData;

	private Button BtnShowPreviousRecord;

	private Button BtnShowNextRecord;

	private Label label19;

	private Button BtnApproveThis;

	private Label label11;

	private Label txtBuyerName;

	private Label txtSupplierName;

	private Label label16;

	private Label txtRemarks;

	private ToolStripButton btnSelectAllCards;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private Button BtnGoToFirst;

	private Button BtnGoToLast;

	private PictureBox BtnSorting;

	private Label label4;

	internal UltraCombo CmbParentCategoryFilter;

	private Panel PanelOtherGridInfo;

	internal UltraCombo CmbBuyerNameFilter;

	private Label label8;

	private Panel PanelHeaderInfo;

	private Label label56;

	private Label label9;

	private Label label15;

	private Label label20;

	private Label label22;

	private GroupBox GbWbComparison;

	private Label label62;

	private Label label82;

	private Label label83;

	private Label label80;

	private Label label84;

	private Label label85;

	private Label label86;

	private GroupBox GbPurchaseSide;

	private Label lblTradingAccount;

	private Label label37;

	private Label label39;

	private Label txtBillAmountPurchase;

	private GroupBox groupBox1;

	private Label label23;

	private Label label26;

	private Label txtBillAmountSale;

	private Label txtDeliveryTermSale;

	private Label lblFreightAccountPurchase;

	private Label lblFreightRemarksPurchase;

	private Label lblTotalFreightPurchase;

	private Label lblFreightAddLessPurchase;

	private Label lblFreightAmountPurchase;

	private Label label12;

	private Label label17;

	private Label lblSaleFreightRemarks;

	private Label label21;

	private Label lblSaleFreightAc;

	private Label lblSaleNetFreight;

	private Label label32;

	private Label lblSaleFreightAddLessAmount;

	private Label lblSaleFreightAmount;

	private Label label35;

	private Label label36;

	private Label label38;

	private Label lblPlAmount;

	private Label label52;

	private Label lblBillWeightGrn;

	private Label lblSupplierWeight;

	private Label lblSupplierSecondWeight;

	private Label lblSupplierFirstWeight;

	private Label lblBillWeightDiff;

	private Label lblNetWeightDifference;

	private Label lblSecondWtDifference;

	private Label lblFirstWtDifference;

	private Label lblBillWeightGdn;

	private Label lblBuyerWeight;

	private Label lblBuyerSecondWeight;

	private Label lblBuyerFirstWeight;

	private Label label18;

	private Label lblPurchaseCommissionDetail;

	private Label lblPurchasePayment;

	private Label label33;

	private Label lblSalePaymentDetail;

	private Label label24;

	private Label lblSaleCommissionDetail;

	private Label label28;

	private Label txtDiffAvgNetRate;

	private Label label25;

	private Label txtPurchaseAvgNetRate;

	private Label txtSaleAvgNetRate;

	private Label lblPurchseTaxAccount;

	private Label label34;

	private Label lblWhtPurchaseAccount;

	private Label label40;

	private Label lblWhtPurchasePercent;

	private Label lblWhtPurchaseAmount;

	private Label label42;

	private Label label43;

	private Label lblWhtSalePercent;

	private Label lblWhtSaleAmount;

	private Label label44;

	private Label label45;

	private Label lblWHTSaleAccount;

	private Label label47;

	private Label lblTaxSaleAccount;

	private Label label49;

	public frmApprovalCommissionTradeBill(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		TradeBill_Helper.InitializeDetailtable(dtCustomerDetail);
		InitializeComponentMethod();
	}

	private void frmApprovalCommissionTradeBill_Load(object sender, EventArgs e)
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
			if (!base.IsDisposed)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					var result = t.Result;
					ctrlGrdBar3.mGridSaveLayouts.Visible = false;
					ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
					if (flagUnApprove)
					{
						labelFormHeading.Text = "Pending 'Commission Agent Trade Bill' For UnApproval";
						btnApproved.Text = "UnApprove";
					}
					else
					{
						labelFormHeading.Text = "Pending 'Commission Agent Trade Bill' For Approval";
						btnApproved.Text = "Approve";
					}
					if (DocumentTypeId > 0)
					{
						GenerateCardInSideBarLayout(AscOrDesc);
					}
					if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
					{
						GRNfromdate.Value = Convert.ToDateTime(RequestedFromDate);
						GRNToDate.Value = Convert.ToDateTime(RequestedToDate);
					}
					else
					{
						GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
					}
					HistoryComboBind(result.HistoryComboData);
					FormInitialized = true;
					GRNfromdate.Focus();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return InvCommAgentTradeBill.GetDataForDropDownFromCommOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId
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
			DataTable dtTa = dtCust.Clone();
			DataTable dtSP = dtCust.Clone();
			DataTable dtItem = dtCust.Clone();
			DataTable dtInv = dtCust.Clone();
			foreach (DataRow dr in dt.Rows)
			{
				switch (Conversion.ToString(dr["Activity"]))
				{
				case "TradingAccount":
					dtTa.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "Customer":
					dtCust.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "Supplier":
					dtSP.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "Item":
					dtItem.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "ParentCategory":
					dtInv.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbTradingAccountFilter, dtTa, "Id", "Name", "Trading Account", AllColumns: true, Conversion.ToInt(CmbTradingAccountFilter.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBuyerNameFilter, dtCust, "Id", "Name", "Buyer Name", AllColumns: true, Conversion.ToInt(CmbTradingAccountFilter.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierNameFilter, dtSP, "Id", "Name", "Supplier Name", AllColumns: true, Conversion.ToInt(CmbSupplierNameFilter.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbParentCategoryFilter, dtInv, "Id", "Name", "Parent Category", AllColumns: true, Conversion.ToInt(CmbParentCategoryFilter.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemNameFilter, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemNameFilter.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			((Control)(object)CmbTradingAccountFilter).Text = string.Empty;
			((Control)(object)CmbSupplierNameFilter).Text = string.Empty;
			((Control)(object)CmbParentCategoryFilter).Text = string.Empty;
			((Control)(object)CmbItemNameFilter).Text = string.Empty;
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
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		try
		{
			if (Reset)
			{
				FromReset();
			}
			dtGridDbCall.Clear();
			DateTime from;
			DateTime to;
			if (!Reset)
			{
				from = ((!Conversion.CheckDateTimeNull(RequestedFromDate)) ? RequestedFromDate : Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period));
				to = ((!Conversion.CheckDateTimeNull(RequestedToDate)) ? RequestedToDate : Conversion.ToDateTime((object)GRNToDate.Value));
			}
			else
			{
				from = Conversion.ToDateTime((object)GRNfromdate.Value);
				to = Conversion.ToDateTime((object)GRNToDate.Value);
			}
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.BranchesId = UserAccount.BranchesId;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.DocumentTypeId = DocumentTypeId;
			grn.FromDate = ((!Reset) ? from : Conversion.ToDateTime((object)GRNfromdate.Value));
			grn.ToDate = ((!Reset) ? to : Conversion.ToDateTime((object)GRNToDate.Value));
			grn.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text.Trim());
			grn.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text.Trim());
			grn.AccountId = Conversion.ToInt(CmbTradingAccountFilter.Value);
			grn.SupplierId = Conversion.ToInt(CmbSupplierNameFilter.Value);
			grn.BuyerId = Conversion.ToInt(CmbBuyerNameFilter.Value);
			grn.ParentCategoryId = Conversion.ToInt(CmbParentCategoryFilter.Value);
			grn.ItemId = Conversion.ToInt(CmbItemNameFilter.Value);
			grn.ApprovedStatus = flagUnApprove;
			dtGridDbCall = InvCommAgentTradeBill.DataForApproval(grn);
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
			SelectAll = false;
			btnSelectAllCards.Text = (SelectAll ? "Un-Select All" : "Select All");
			FlpSideBar.SuspendLayout();
			FlpSideBar.Controls.Clear();
			FlpSideBar.AutoScroll = true;
			IEnumerable<DataRow> distinctRows = from r in dtGridDbCall.AsEnumerable()
				group r by r.Field<int>("Id") into g
				select g.First();
			distinctRows = (AscOrDesc ? distinctRows.OrderBy((DataRow r) => r.Field<int>("DocNo")) : distinctRows.OrderByDescending((DataRow r) => r.Field<int>("DocNo")));
			List<DataRow> distinctList = distinctRows.ToList();
			if (totalRecords == 0)
			{
				totalRecords = distinctList.Count;
			}
			int skip = (currentPage - 1) * pageSize;
			foreach (DataRow row in distinctList.Skip(skip).Take(pageSize))
			{
				int docId = Conversion.ToInt(row["Id"]);
				Conversion.ToString(row["DocNo"]);
				Conversion.ToString(row["EntryUserName"]);
				Conversion.ToString(row["ModifyUserName"]);
				string TradingGlAccount = Conversion.ToString(row["TradingGlAccount"]);
				DateTime docDate = Conversion.ToDateTime(row["DocDate"]);
				DateTime entryDate = Conversion.ToDateTime(row["EntryDate"]);
				DateTime modifyDate = Conversion.ToDateTime(row["ModifyDate"]);
				Conversion.ToInt(row["NoOfAttachments"]);
				ApprovalSideBarMainCard card = new ApprovalSideBarMainCard
				{
					DocumentId = docId,
					Width = 350,
					Height = 210
				};
				card.chkSelectionWithMainText.Text = TradingGlAccount;
				card.FlpMain.SuspendLayout();
				card.FlpMain.Controls.Clear();
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Supplier Name:"),
					Col2 = new CardCellModel
					{
						Text = Conversion.ToString(row["SupplierName"]),
						ColumnSpan = 3,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.Blue,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Buyer Name:"),
					Col2 = new CardCellModel
					{
						Text = Conversion.ToString(row["BuyerName"]),
						ColumnSpan = 3,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.Red,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Deliver To:"),
					Col2 = new CardCellModel
					{
						Text = Conversion.ToString(row["GdnDeliverToPartyName"]),
						ColumnSpan = 3,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.IndianRed,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Doc No:"),
					Col2 = ApprovalFormHelper.DefaultLinkModel(Conversion.ToString(row["DocNo"]), delegate
					{
						CrystalReportPrint_Helper.CommissionAgentBillIncomeSlip_1056A(docId);
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
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultButtonModel("Approve This ✔", Color.DarkGreen, delegate
					{
						ProcessApproval(new List<ApprovalSideBarMainCard> { card });
					}, 2),
					Col3 = ApprovalFormHelper.DefaultButtonModel(string.Format("Attachments ({0})", Conversion.ToInt(row["NoOfAttachments"])), Color.DarkSlateBlue, delegate
					{
						CommonServices.GetNoofAttachmentsByRefDocumentTypeID(docId, DocumentTypeId);
					}, 2),
					RowHeight = 25
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
			InvCommAgentTradeBill Or = InvCommAgentTradeBill.GetByID(RecId);
			if (Or == null || Or.InvCommAgentTradeBillDetailslist == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			txtDocDate.Text = Conversion.ToDateTime((object)Or.DocDate).ToShortDateString();
			txtDocNo.Text = Conversion.ToString((object)Or.DocNo);
			lblTradingAccount.Text = Conversion.ToString((object)Or.TradingGlAccount);
			txtSupplierName.Text = Conversion.ToString((object)Or.SupplierName);
			txtDeliveryTermPurchase.Text = Conversion.ToString((object)Or.DeliveryTerm);
			txtBillAmountPurchase.Text = Conversion.ToDecimal((object)Or.PurchaseBillAmount).ToString(clsGlobalVariables.stringFormatsingle);
			lblSupplierFirstWeight.Text = Conversion.ToDecimal((object)Or.SupplierFirstWeight).ToString("#,##0.###");
			lblSupplierSecondWeight.Text = Conversion.ToDecimal((object)Or.SupplierSecondWeight).ToString("#,##0.###");
			lblSupplierWeight.Text = Conversion.ToDecimal((object)Or.SupplierNetWeight).ToString("#,##0.###");
			lblBillWeightGrn.Text = Conversion.ToDecimal((object)Or.SupplierBillWeight).ToString("#,##0.###");
			txtBuyerName.Text = Conversion.ToString((object)Or.CustomerName);
			txtDeliveryTermSale.Text = Conversion.ToString((object)Or.DeliveryTermSale);
			txtBillAmountSale.Text = Conversion.ToDecimal((object)Or.SaleBillAmount).ToString(clsGlobalVariables.stringFormatsingle);
			lblBuyerFirstWeight.Text = Conversion.ToDecimal((object)Or.BuyerFirstWeight).ToString("#,##0.###");
			lblBuyerSecondWeight.Text = Conversion.ToDecimal((object)Or.BuyerNetWeight).ToString("#,##0.###");
			lblBuyerWeight.Text = Conversion.ToDecimal((object)Or.BuyerNetWeight).ToString("#,##0.###");
			lblBillWeightGdn.Text = Conversion.ToDecimal((object)Or.BuyerBillWeight).ToString("#,##0.###");
			lblPlAmount.Text = Conversion.ToDecimal((object)Or.PLAmount).ToString(clsGlobalVariables.stringFormatsingle);
			txtRemarks.Text = Conversion.ToString((object)Or.RemarksHeader);
			lblPurchasePayment.Text = Conversion.ToString((object)Or.PurchasePaymentDetailDescription);
			lblSalePaymentDetail.Text = Conversion.ToString((object)Or.SalePaymentDetailDescription);
			lblPurchseTaxAccount.Text = Conversion.ToString((object)Or.PurchaseTaxAccount);
			lblTaxSaleAccount.Text = Conversion.ToString((object)Or.SaleTaxAccount);
			lblPurchaseCommissionDetail.Text = Conversion.ToString((object)Or.PurchaseCommisionDetailDescription);
			lblSaleCommissionDetail.Text = Conversion.ToString((object)Or.SaleCommisionDetailDescription);
			dtCustomerDetail.Rows.Clear();
			TradeBill_Helper.FillDetailtableFromListCommonForReadById(Or, dtCustomerDetail);
			grdCustomerDetail.DataSource = dtCustomerDetail;
			grdCustomerDetail.RetrieveStructure();
			grdSettingSettings();
			if (Or.CommisionAgentBillFreightDetailList != null && Or.CommisionAgentBillFreightDetailList.Count > 0)
			{
				CommisionAgentBillFreightDetail purchase = Or.CommisionAgentBillFreightDetailList.FirstOrDefault((CommisionAgentBillFreightDetail x) => x.EntrySideId == 1);
				if (purchase != null)
				{
					lblFreightAmountPurchase.Text = purchase.Amount.ToString("#,##0.###");
					lblFreightAddLessPurchase.Text = purchase.AddLessAmount.ToString("#,##0.###");
					lblTotalFreightPurchase.Text = purchase.NetAmount.ToString("#,##0.###");
					lblFreightRemarksPurchase.Text = purchase.Remarks ?? "";
					lblFreightAccountPurchase.Text = purchase.AccountBusinessName;
				}
				CommisionAgentBillFreightDetail sale2 = Or.CommisionAgentBillFreightDetailList.FirstOrDefault((CommisionAgentBillFreightDetail x) => x.EntrySideId == 2);
				if (sale2 != null)
				{
					lblSaleFreightAmount.Text = sale2.Amount.ToString("#,##0.###");
					lblSaleFreightAddLessAmount.Text = sale2.AddLessAmount.ToString("#,##0.###");
					lblSaleNetFreight.Text = sale2.NetAmount.ToString("#,##0.###");
					lblSaleFreightRemarks.Text = sale2.Remarks ?? "";
					lblSaleFreightAc.Text = sale2.AccountBusinessName;
				}
			}
			if (Or.CommisionAgentBillTaxDetailList != null && Or.CommisionAgentBillTaxDetailList.Any())
			{
				List<CommisionAgentBillTaxDetail> purchaseTaxes = Or.CommisionAgentBillTaxDetailList.Where((CommisionAgentBillTaxDetail x) => x.EntrySideId == 1).ToList();
				if (purchaseTaxes.Any())
				{
					CommisionAgentBillTaxDetail purchase2 = purchaseTaxes.FirstOrDefault();
					if (purchase2 != null)
					{
						lblWhtPurchasePercent.Text = purchase2.TaxPercantage.ToString("#,##0.###");
						lblWhtPurchaseAmount.Text = purchase2.TaxAmount.ToString("#,##0.###");
						lblWhtPurchaseAccount.Text = purchase2.TaxAccountTitle;
						lblWhtPurchaseAccount.Tag = purchase2.TaxAccountId;
					}
				}
				List<CommisionAgentBillTaxDetail> saleTaxes = Or.CommisionAgentBillTaxDetailList.Where((CommisionAgentBillTaxDetail x) => x.EntrySideId == 2).ToList();
				if (saleTaxes.Any())
				{
					CommisionAgentBillTaxDetail sale = saleTaxes.FirstOrDefault();
					if (sale != null)
					{
						lblWhtSalePercent.Text = sale.TaxPercantage.ToString("#,##0.###");
						lblWhtSaleAmount.Text = sale.TaxAmount.ToString("#,##0.###");
						lblWHTSaleAccount.Text = sale.TaxAccountTitle;
						lblWHTSaleAccount.TabIndex = sale.TaxAccountId;
					}
				}
			}
			CalculateHeaderRates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateHeaderRates()
	{
		try
		{
			double PurchaseAvgRate = GridEX_Helper.GetColumnAvg(grdCustomerDetail, "PurchaseNetRate");
			double SaleAvgRate = GridEX_Helper.GetColumnAvg(grdCustomerDetail, "SaleNetRate");
			txtSaleAvgNetRate.Text = SaleAvgRate.ToString("#,##0.###");
			txtPurchaseAvgNetRate.Text = PurchaseAvgRate.ToString("#,##0.###");
			FormHelper.SetDifferenceForLable(txtPurchaseAvgNetRate, txtSaleAvgNetRate, txtDiffAvgNetRate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingSettings()
	{
		try
		{
			TradeBill_Helper.DetailGridCommonSetting(grdCustomerDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			lblTradingAccount.Text = "None";
			lblPlAmount.Text = "0";
			lblFreightAccountPurchase.Text = "None";
			lblFreightAddLessPurchase.Text = "0";
			lblFreightAmountPurchase.Text = "0";
			lblTotalFreightPurchase.Text = "0";
			lblFreightRemarksPurchase.Text = "None";
			lblPurchseTaxAccount.Text = "None";
			lblWhtPurchaseAccount.Text = "None";
			lblWhtPurchaseAmount.Text = "0";
			lblWhtPurchasePercent.Text = "0";
			lblSaleFreightAc.Text = "None";
			lblSaleFreightAddLessAmount.Text = "0";
			lblSaleFreightAmount.Text = "0";
			lblSaleNetFreight.Text = "0";
			lblSaleFreightRemarks.Text = "None";
			lblTaxSaleAccount.Text = "None";
			lblWHTSaleAccount.Text = "None";
			lblWhtSaleAmount.Text = "0";
			lblWhtSalePercent.Text = "0";
			lblSupplierFirstWeight.Text = "0";
			lblBuyerFirstWeight.Text = "0";
			lblSupplierSecondWeight.Text = "0";
			lblBuyerSecondWeight.Text = "0";
			lblSupplierWeight.Text = "0";
			lblBuyerWeight.Text = "0";
			lblBillWeightGrn.Text = "0";
			lblBillWeightGdn.Text = "0";
			txtPurchaseAvgNetRate.Text = "0";
			txtSaleAvgNetRate.Text = "0";
			txtDiffAvgNetRate.Text = "0";
			txtSupplierName.Text = "None";
			txtBuyerName.Text = "None";
			txtDeliveryTermPurchase.Text = "None";
			txtRemarks.Text = "None";
			lblPurchasePayment.Text = "None";
			lblSalePaymentDetail.Text = "None";
			lblSaleCommissionDetail.Text = "None";
			lblPurchaseCommissionDetail.Text = "None";
			dtCustomerDetail.Clear();
			grdCustomerDetail.DataSource = null;
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
				return;
			}
			FormHelper.CanOpenForm("frmCommissionAgentTradeBillAgainstGdn", delegate
			{
				frmCommissionAgentTradeBillAgainstGdn frm = new frmCommissionAgentTradeBillAgainstGdn(UserAccount);
				frm.BringToFront();
				frm.Show();
				frm.FormInitializedAction += delegate
				{
					frm.ReadById(RecId);
				};
				frm.BringToFront();
				bool formClosed = false;
				frm.FormClosed += delegate
				{
					formClosed = true;
				};
				frm.Show();
				while (!formClosed)
				{
					Application.DoEvents();
				}
			}, UserAccount);
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
				where Conversion.ToInt(row["docNo"]) == nextDocNo
				select row).FirstOrDefault();
			if (nextRecord != null)
			{
				int nextDocId = Conversion.ToInt(nextRecord["purchaseOrderMasterId"]);
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
					where Conversion.ToInt(row["docNo"]) == PreviousDocNo
					select row).FirstOrDefault();
				if (previousRecord != null)
				{
					int PreviousDocId = Conversion.ToInt(previousRecord["purchaseOrderMasterId"]);
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

	private void btnApproved_Click(object sender, EventArgs e)
	{
		try
		{
			List<ApprovalSideBarMainCard> checkedCards = (from c in FlpSideBar.Controls.OfType<ApprovalSideBarMainCard>()
				where c.chkSelectionWithMainText.Checked
				select c).ToList();
			if (!checkedCards.Any())
			{
				MessageBox.Show("Please select at least one record first", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				ProcessApproval(checkedCards);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ProcessApproval(List<ApprovalSideBarMainCard> selectedCards)
	{
		string action = (flagUnApprove ? "UnApproved" : "Approved");
		_ = flagUnApprove;
		try
		{
			ApprovalList card = ApprovalFormHelper.ProcessApproval(selectedCards, UserAccount, flagUnApprove, DocumentTypeId, txtApprovalRemarks, "Commission Agent Trade Bill");
			if (card != null)
			{
				InvCommAgentTradeBill.UpdateApprovalStatus(card);
				MessageBox.Show("Record(s) " + action + " successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				MainGridDbCall();
				GenerateCardInSideBarLayout(AscOrDesc);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnApproveThis_Click(object sender, EventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		string action = (flagUnApprove ? "UnApprove" : "Approve");
		string reqType = (flagUnApprove ? "UP" : "AP");
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("No record selected to approve.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			ApprovalList approvalWrapper = new ApprovalList
			{
				AllApprovalLists = new List<ApprovalList>()
			};
			string remarks = txtApprovalRemarks?.Text?.Trim();
			if (string.IsNullOrWhiteSpace(remarks))
			{
				txtApprovalRemarks.Focus();
				MessageBox.Show("Approval remarks are required for: " + txtDocNo.Text, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			approvalWrapper.AllApprovalLists.Add(new ApprovalList
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				Id = RecId,
				PostDate = DateTime.Now,
				EntryUser = UserAccount.ID,
				ReqType = reqType,
				Comments = remarks
			});
			InvCommAgentTradeBill.UpdateApprovalStatus(approvalWrapper);
			MessageBox.Show("Record(s) " + action + "d successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			MainGridDbCall();
			GenerateCardInSideBarLayout(AscOrDesc);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while " + action + " records: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSelectAllCards_Click(object sender, EventArgs e)
	{
		try
		{
			if (FlpSideBar.Controls.Count == 0)
			{
				string Text = (SelectAll ? "Select" : "Un-Select");
				MessageBox.Show("No cards available to " + Text + ".", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			SelectAll = !SelectAll;
			foreach (ApprovalSideBarMainCard item in FlpSideBar.Controls.OfType<ApprovalSideBarMainCard>())
			{
				item.chkSelectionWithMainText.Checked = SelectAll;
			}
			btnSelectAllCards.Text = (SelectAll ? "Un-Select All" : "Select All");
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
			if (e.Control && e.KeyCode == Keys.A)
			{
				btnApproved_Click(null, null);
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

	private void txtGrnNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrnNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void lblSupplierFirstWeight_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifferenceForLable(lblSupplierFirstWeight, lblBuyerFirstWeight, lblFirstWtDifference);
	}

	private void lblBuyerFirstWeight_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifferenceForLable(lblSupplierFirstWeight, lblBuyerFirstWeight, lblFirstWtDifference);
	}

	private void lblSupplierSecondWeight_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifferenceForLable(lblSupplierSecondWeight, lblBuyerSecondWeight, lblSecondWtDifference);
	}

	private void lblBuyerSecondWeight_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifferenceForLable(lblSupplierSecondWeight, lblBuyerSecondWeight, lblSecondWtDifference);
	}

	private void lblSupplierWeight_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifferenceForLable(lblSupplierWeight, lblBuyerWeight, lblNetWeightDifference);
	}

	private void lblBuyerWeight_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifferenceForLable(lblSupplierWeight, lblBuyerWeight, lblNetWeightDifference);
	}

	private void lblBillWeightGrn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifferenceForLable(lblBillWeightGrn, lblBillWeightGdn, lblBillWeightDiff);
	}

	private void lblBillWeightGdn_TextChanged(object sender, EventArgs e)
	{
		FormHelper.SetDifferenceForLable(lblBillWeightGrn, lblBillWeightGdn, lblBillWeightDiff);
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
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		GridEXLayout grdCustomerDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Cmagt.Approvals.frmApprovalCommissionTradeBill));
		this.panel1 = new System.Windows.Forms.Panel();
		this.FlpSideBar = new System.Windows.Forms.FlowLayoutPanel();
		this.TotalRecordPanel = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.TotalHeaderRecords = new System.Windows.Forms.Label();
		this.BtnGoToFirst = new System.Windows.Forms.Button();
		this.BtnGoToLast = new System.Windows.Forms.Button();
		this.BtnLoadPreviousRecord = new System.Windows.Forms.Button();
		this.BtnLoadNextRecord = new System.Windows.Forms.Button();
		this.GBFilters = new System.Windows.Forms.GroupBox();
		this.BtnSorting = new System.Windows.Forms.PictureBox();
		this.CmbSupplierNameFilter = new UltraCombo();
		this.CmbBuyerNameFilter = new UltraCombo();
		this.CmbTradingAccountFilter = new UltraCombo();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.CmbParentCategoryFilter = new UltraCombo();
		this.CmbItemNameFilter = new UltraCombo();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnApproved = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShowHideFilter = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSelectAllCards = new System.Windows.Forms.ToolStripButton();
		this.panel3 = new System.Windows.Forms.Panel();
		this.labelFormHeading = new System.Windows.Forms.Label();
		this.txtApprovalRemarks = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.PanelMainSelectedDocData = new System.Windows.Forms.Panel();
		this.grdCustomerDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label19 = new System.Windows.Forms.Label();
		this.PanelHeaderInfo = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.lblSaleFreightRemarks = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtBuyerName = new System.Windows.Forms.Label();
		this.lblSaleFreightAc = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.lblSaleNetFreight = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.lblSaleFreightAddLessAmount = new System.Windows.Forms.Label();
		this.txtBillAmountSale = new System.Windows.Forms.Label();
		this.lblSaleFreightAmount = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtDeliveryTermSale = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.lblTradingAccount = new System.Windows.Forms.Label();
		this.GbWbComparison = new System.Windows.Forms.GroupBox();
		this.lblBillWeightDiff = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.lblNetWeightDifference = new System.Windows.Forms.Label();
		this.label83 = new System.Windows.Forms.Label();
		this.lblBillWeightGrn = new System.Windows.Forms.Label();
		this.lblSecondWtDifference = new System.Windows.Forms.Label();
		this.lblSupplierWeight = new System.Windows.Forms.Label();
		this.lblFirstWtDifference = new System.Windows.Forms.Label();
		this.lblSupplierSecondWeight = new System.Windows.Forms.Label();
		this.lblSupplierFirstWeight = new System.Windows.Forms.Label();
		this.lblBillWeightGdn = new System.Windows.Forms.Label();
		this.label80 = new System.Windows.Forms.Label();
		this.lblBuyerWeight = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.label85 = new System.Windows.Forms.Label();
		this.lblBuyerSecondWeight = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.lblBuyerFirstWeight = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.GbPurchaseSide = new System.Windows.Forms.GroupBox();
		this.label37 = new System.Windows.Forms.Label();
		this.txtBillAmountPurchase = new System.Windows.Forms.Label();
		this.lblFreightRemarksPurchase = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.lblFreightAccountPurchase = new System.Windows.Forms.Label();
		this.lblTotalFreightPurchase = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.lblFreightAddLessPurchase = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.lblFreightAmountPurchase = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtSupplierName = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.txtDeliveryTermPurchase = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.Label();
		this.PanelOtherGridInfo = new System.Windows.Forms.Panel();
		this.lblSaleCommissionDetail = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.lblSalePaymentDetail = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.lblPurchasePayment = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.lblPurchaseCommissionDetail = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.PanelMainSelectedNetPreviousDocData = new System.Windows.Forms.Panel();
		this.lblPlAmount = new System.Windows.Forms.Label();
		this.BtnShowPreviousRecord = new System.Windows.Forms.Button();
		this.label52 = new System.Windows.Forms.Label();
		this.BtnApproveThis = new System.Windows.Forms.Button();
		this.BtnShowNextRecord = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtDiffAvgNetRate = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtPurchaseAvgNetRate = new System.Windows.Forms.Label();
		this.txtSaleAvgNetRate = new System.Windows.Forms.Label();
		this.lblPurchseTaxAccount = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.lblWhtPurchaseAccount = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.lblWhtPurchasePercent = new System.Windows.Forms.Label();
		this.lblWhtPurchaseAmount = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.lblWhtSalePercent = new System.Windows.Forms.Label();
		this.lblWhtSaleAmount = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.lblWHTSaleAccount = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.lblTaxSaleAccount = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.TotalRecordPanel.SuspendLayout();
		this.GBFilters.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.BtnSorting).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierNameFilter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameFilter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTradingAccountFilter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategoryFilter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameFilter).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel3.SuspendLayout();
		this.PanelMainSelectedDocData.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.PanelHeaderInfo.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.GbWbComparison.SuspendLayout();
		this.GbPurchaseSide.SuspendLayout();
		this.PanelOtherGridInfo.SuspendLayout();
		this.PanelMainSelectedNetPreviousDocData.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.FlpSideBar);
		this.panel1.Controls.Add(this.TotalRecordPanel);
		this.panel1.Controls.Add(this.GBFilters);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 24);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(380, 687);
		this.panel1.TabIndex = 0;
		this.FlpSideBar.Dock = System.Windows.Forms.DockStyle.Fill;
		this.FlpSideBar.Location = new System.Drawing.Point(0, 235);
		this.FlpSideBar.Name = "FlpSideBar";
		this.FlpSideBar.Size = new System.Drawing.Size(380, 416);
		this.FlpSideBar.TabIndex = 1;
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
		this.label5.Location = new System.Drawing.Point(94, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(112, 20);
		this.label5.TabIndex = 1;
		this.label5.Text = "Total Records: ";
		this.TotalHeaderRecords.AutoSize = true;
		this.TotalHeaderRecords.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalHeaderRecords.Location = new System.Drawing.Point(205, 6);
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
		this.BtnGoToFirst.Location = new System.Drawing.Point(38, 1);
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
		this.BtnGoToLast.Location = new System.Drawing.Point(297, 1);
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
		this.GBFilters.BackColor = System.Drawing.Color.White;
		this.GBFilters.Controls.Add(this.BtnSorting);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierNameFilter);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerNameFilter);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter);
		this.GBFilters.Controls.Add(this.txtDocNoFrom);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategoryFilter);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameFilter);
		this.GBFilters.Controls.Add(this.GRNfromdate);
		this.GBFilters.Controls.Add(this.txtDocNoTo);
		this.GBFilters.Controls.Add(this.GRNToDate);
		this.GBFilters.Controls.Add(this.label8);
		this.GBFilters.Controls.Add(this.label7);
		this.GBFilters.Controls.Add(this.label14);
		this.GBFilters.Controls.Add(this.label4);
		this.GBFilters.Controls.Add(this.btnshow);
		this.GBFilters.Controls.Add(this.label10);
		this.GBFilters.Controls.Add(this.label1);
		this.GBFilters.Controls.Add(this.label27);
		this.GBFilters.Dock = System.Windows.Forms.DockStyle.Top;
		this.GBFilters.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.GBFilters.ForeColor = System.Drawing.Color.Maroon;
		this.GBFilters.Location = new System.Drawing.Point(0, 28);
		this.GBFilters.Name = "GBFilters";
		this.GBFilters.Size = new System.Drawing.Size(380, 207);
		this.GBFilters.TabIndex = 0;
		this.GBFilters.TabStop = false;
		this.GBFilters.Text = "Filters";
		this.BtnSorting.Image = Architecture.WinApp.Properties.Resources.SortDesc;
		this.BtnSorting.Location = new System.Drawing.Point(327, 170);
		this.BtnSorting.Name = "BtnSorting";
		this.BtnSorting.Size = new System.Drawing.Size(33, 32);
		this.BtnSorting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.BtnSorting.TabIndex = 189;
		this.BtnSorting.TabStop = false;
		this.BtnSorting.Click += new System.EventHandler(BtnSorting_Click);
		this.CmbSupplierNameFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierNameFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierNameFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierNameFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierNameFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierNameFilter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierNameFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierNameFilter).Location = new System.Drawing.Point(98, 88);
		((System.Windows.Forms.Control)(object)this.CmbSupplierNameFilter).Name = "CmbSupplierNameFilter";
		((System.Windows.Forms.Control)(object)this.CmbSupplierNameFilter).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierNameFilter).TabIndex = 5;
		this.CmbBuyerNameFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerNameFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerNameFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerNameFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerNameFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameFilter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerNameFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameFilter).Location = new System.Drawing.Point(98, 116);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameFilter).Name = "CmbBuyerNameFilter";
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameFilter).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameFilter).TabIndex = 4;
		this.CmbTradingAccountFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbTradingAccountFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTradingAccountFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTradingAccountFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTradingAccountFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTradingAccountFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter).Location = new System.Drawing.Point(98, 60);
		((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter).Name = "CmbTradingAccountFilter";
		((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter).TabIndex = 4;
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(98, 35);
		this.txtDocNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.ShortcutsEnabled = false;
		this.txtDocNoFrom.Size = new System.Drawing.Size(130, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDocNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoFrom_KeyPress);
		this.CmbParentCategoryFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategoryFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategoryFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategoryFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategoryFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryFilter).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCategoryFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryFilter).Location = new System.Drawing.Point(98, 144);
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryFilter).Name = "CmbParentCategoryFilter";
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryFilter).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryFilter).TabIndex = 6;
		this.CmbItemNameFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameFilter).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameFilter).Location = new System.Drawing.Point(98, 173);
		((System.Windows.Forms.Control)(object)this.CmbItemNameFilter).Name = "CmbItemNameFilter";
		((System.Windows.Forms.Control)(object)this.CmbItemNameFilter).Size = new System.Drawing.Size(169, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameFilter).TabIndex = 6;
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(98, 12);
		this.GRNfromdate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(130, 22);
		this.GRNfromdate.TabIndex = 0;
		this.txtDocNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(230, 35);
		this.txtDocNoTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.ShortcutsEnabled = false;
		this.txtDocNoTo.Size = new System.Drawing.Size(130, 23);
		this.txtDocNoTo.TabIndex = 3;
		this.txtDocNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDocNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoTo_KeyPress);
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(230, 12);
		this.GRNToDate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(130, 22);
		this.GRNToDate.TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(5, 122);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 15);
		this.label8.TabIndex = 636;
		this.label8.Text = "Buyer Name";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(5, 94);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 635;
		this.label7.Text = "Supplier Name";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(5, 66);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(95, 15);
		this.label14.TabIndex = 636;
		this.label14.Text = "Trading Account";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(5, 150);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(91, 15);
		this.label4.TabIndex = 184;
		this.label4.Text = "Parent Category";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(269, 173);
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
		this.label10.Size = new System.Drawing.Size(88, 15);
		this.label10.TabIndex = 144;
		this.label10.Text = "From && To Doc";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(5, 179);
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
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "From && To Date";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(380, 28);
		this.panel2.TabIndex = 1;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnNew, this.toolStripSeparator5, this.btnRefresh, this.toolStripSeparator4, this.btnApproved, this.toolStripSeparator3, this.btnShortCutKeys, this.toolStripSeparator2, this.BtnShowHideFilter, this.toolStripSeparator1,
			this.btnSelectAllCards
		});
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
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
		this.btnApproved.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApproved.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(146, 24);
		this.btnApproved.Text = "Approve Selected";
		this.btnApproved.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.btnApproved.ToolTipText = "Click to Approve Selected Records";
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
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
		this.btnSelectAllCards.Checked = true;
		this.btnSelectAllCards.CheckState = System.Windows.Forms.CheckState.Checked;
		this.btnSelectAllCards.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSelectAllCards.Image = Architecture.WinApp.Properties.Resources.list;
		this.btnSelectAllCards.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSelectAllCards.Name = "btnSelectAllCards";
		this.btnSelectAllCards.Size = new System.Drawing.Size(92, 24);
		this.btnSelectAllCards.Text = "Select All";
		this.btnSelectAllCards.Click += new System.EventHandler(btnSelectAllCards_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.labelFormHeading);
		this.panel3.Controls.Add(this.txtApprovalRemarks);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1547, 24);
		this.panel3.TabIndex = 52;
		this.labelFormHeading.AutoSize = true;
		this.labelFormHeading.BackColor = System.Drawing.Color.Teal;
		this.labelFormHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelFormHeading.ForeColor = System.Drawing.Color.White;
		this.labelFormHeading.Location = new System.Drawing.Point(3, 2);
		this.labelFormHeading.Name = "labelFormHeading";
		this.labelFormHeading.Size = new System.Drawing.Size(454, 19);
		this.labelFormHeading.TabIndex = 4;
		this.labelFormHeading.Text = "Pending 'Commission Agent Trade Bill' For UnApproval";
		this.txtApprovalRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovalRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovalRemarks.Location = new System.Drawing.Point(585, 1);
		this.txtApprovalRemarks.Margin = new System.Windows.Forms.Padding(4);
		this.txtApprovalRemarks.Name = "txtApprovalRemarks";
		this.txtApprovalRemarks.ShortcutsEnabled = false;
		this.txtApprovalRemarks.Size = new System.Drawing.Size(508, 23);
		this.txtApprovalRemarks.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(458, 4);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(120, 17);
		this.label2.TabIndex = 121;
		this.label2.Text = "Approval Remarks";
		this.PanelMainSelectedDocData.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerDetail);
		this.PanelMainSelectedDocData.Controls.Add(this.panel14);
		this.PanelMainSelectedDocData.Controls.Add(this.PanelHeaderInfo);
		this.PanelMainSelectedDocData.Controls.Add(this.PanelOtherGridInfo);
		this.PanelMainSelectedDocData.Controls.Add(this.PanelMainSelectedNetPreviousDocData);
		this.PanelMainSelectedDocData.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainSelectedDocData.Location = new System.Drawing.Point(380, 24);
		this.PanelMainSelectedDocData.Name = "PanelMainSelectedDocData";
		this.PanelMainSelectedDocData.Size = new System.Drawing.Size(1167, 687);
		this.PanelMainSelectedDocData.TabIndex = 53;
		this.grdCustomerDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdCustomerDetail).BackColor = System.Drawing.Color.White;
		grdCustomerDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerDetail.DesignTimeLayout = grdCustomerDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Location = new System.Drawing.Point(0, 318);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Name = "grdCustomerDetail";
		((ScrollableBase)this.grdCustomerDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Size = new System.Drawing.Size(1167, 229);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).TabIndex = 2;
		this.grdCustomerDetail.TotalRow = (InheritableBoolean)1;
		this.grdCustomerDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerDetail).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Controls.Add(this.label19);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 297);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1167, 21);
		this.panel14.TabIndex = 133;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1126, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdCustomerDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar3.TabIndex = 127;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(5, 2);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(74, 16);
		this.label19.TabIndex = 126;
		this.label19.Text = "Detail Info\t\r\n";
		this.PanelHeaderInfo.BackColor = System.Drawing.Color.White;
		this.PanelHeaderInfo.Controls.Add(this.groupBox1);
		this.PanelHeaderInfo.Controls.Add(this.label11);
		this.PanelHeaderInfo.Controls.Add(this.lblTradingAccount);
		this.PanelHeaderInfo.Controls.Add(this.GbWbComparison);
		this.PanelHeaderInfo.Controls.Add(this.label16);
		this.PanelHeaderInfo.Controls.Add(this.GbPurchaseSide);
		this.PanelHeaderInfo.Controls.Add(this.txtRemarks);
		this.PanelHeaderInfo.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHeaderInfo.Location = new System.Drawing.Point(0, 34);
		this.PanelHeaderInfo.Name = "PanelHeaderInfo";
		this.PanelHeaderInfo.Size = new System.Drawing.Size(1167, 263);
		this.PanelHeaderInfo.TabIndex = 135;
		this.groupBox1.Controls.Add(this.lblWhtSalePercent);
		this.groupBox1.Controls.Add(this.lblSaleFreightRemarks);
		this.groupBox1.Controls.Add(this.lblWhtSaleAmount);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.lblWHTSaleAccount);
		this.groupBox1.Controls.Add(this.txtBuyerName);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.lblSaleFreightAc);
		this.groupBox1.Controls.Add(this.lblTaxSaleAccount);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.lblSaleNetFreight);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.lblSaleFreightAddLessAmount);
		this.groupBox1.Controls.Add(this.txtBillAmountSale);
		this.groupBox1.Controls.Add(this.lblSaleFreightAmount);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.txtDeliveryTermSale);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.ForeColor = System.Drawing.Color.Red;
		this.groupBox1.Location = new System.Drawing.Point(794, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(362, 230);
		this.groupBox1.TabIndex = 6;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Sale Side";
		this.lblSaleFreightRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblSaleFreightRemarks.ForeColor = System.Drawing.Color.Black;
		this.lblSaleFreightRemarks.Location = new System.Drawing.Point(130, 127);
		this.lblSaleFreightRemarks.Name = "lblSaleFreightRemarks";
		this.lblSaleFreightRemarks.Size = new System.Drawing.Size(229, 20);
		this.lblSaleFreightRemarks.TabIndex = 684;
		this.lblSaleFreightRemarks.Text = "None";
		this.lblSaleFreightRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Teal;
		this.label17.Location = new System.Drawing.Point(5, 28);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(97, 15);
		this.label17.TabIndex = 679;
		this.label17.Text = "Customer Name";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Teal;
		this.label21.Location = new System.Drawing.Point(5, 130);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(99, 15);
		this.label21.TabIndex = 688;
		this.label21.Text = "Freight Remarks";
		this.txtBuyerName.AutoEllipsis = true;
		this.txtBuyerName.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBuyerName.ForeColor = System.Drawing.Color.Black;
		this.txtBuyerName.Location = new System.Drawing.Point(131, 12);
		this.txtBuyerName.Name = "txtBuyerName";
		this.txtBuyerName.Size = new System.Drawing.Size(230, 47);
		this.txtBuyerName.TabIndex = 184;
		this.txtBuyerName.Text = "None";
		this.txtBuyerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblSaleFreightAc.AutoEllipsis = true;
		this.lblSaleFreightAc.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSaleFreightAc.ForeColor = System.Drawing.Color.Black;
		this.lblSaleFreightAc.Location = new System.Drawing.Point(130, 74);
		this.lblSaleFreightAc.Name = "lblSaleFreightAc";
		this.lblSaleFreightAc.Size = new System.Drawing.Size(230, 20);
		this.lblSaleFreightAc.TabIndex = 680;
		this.lblSaleFreightAc.Text = "None";
		this.lblSaleFreightAc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.Teal;
		this.label23.Location = new System.Drawing.Point(5, 201);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(93, 20);
		this.label23.TabIndex = 498;
		this.label23.Text = "Bill Amount";
		this.lblSaleNetFreight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblSaleNetFreight.ForeColor = System.Drawing.Color.Black;
		this.lblSaleNetFreight.Location = new System.Drawing.Point(130, 110);
		this.lblSaleNetFreight.Name = "lblSaleNetFreight";
		this.lblSaleNetFreight.Size = new System.Drawing.Size(97, 20);
		this.lblSaleNetFreight.TabIndex = 683;
		this.lblSaleNetFreight.Text = "None";
		this.lblSaleNetFreight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Teal;
		this.label32.Location = new System.Drawing.Point(5, 113);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(71, 15);
		this.label32.TabIndex = 686;
		this.label32.Text = "Net Freight";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Teal;
		this.label26.Location = new System.Drawing.Point(5, 59);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(86, 15);
		this.label26.TabIndex = 498;
		this.label26.Text = "Delivery Term";
		this.lblSaleFreightAddLessAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblSaleFreightAddLessAmount.ForeColor = System.Drawing.Color.Black;
		this.lblSaleFreightAddLessAmount.Location = new System.Drawing.Point(293, 93);
		this.lblSaleFreightAddLessAmount.Name = "lblSaleFreightAddLessAmount";
		this.lblSaleFreightAddLessAmount.Size = new System.Drawing.Size(62, 20);
		this.lblSaleFreightAddLessAmount.TabIndex = 682;
		this.lblSaleFreightAddLessAmount.Text = "None";
		this.lblSaleFreightAddLessAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtBillAmountSale.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.txtBillAmountSale.ForeColor = System.Drawing.Color.Black;
		this.txtBillAmountSale.Location = new System.Drawing.Point(130, 201);
		this.txtBillAmountSale.Name = "txtBillAmountSale";
		this.txtBillAmountSale.Size = new System.Drawing.Size(130, 20);
		this.txtBillAmountSale.TabIndex = 184;
		this.txtBillAmountSale.Text = "0";
		this.txtBillAmountSale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblSaleFreightAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblSaleFreightAmount.ForeColor = System.Drawing.Color.Black;
		this.lblSaleFreightAmount.Location = new System.Drawing.Point(130, 93);
		this.lblSaleFreightAmount.Name = "lblSaleFreightAmount";
		this.lblSaleFreightAmount.Size = new System.Drawing.Size(97, 20);
		this.lblSaleFreightAmount.TabIndex = 681;
		this.lblSaleFreightAmount.Text = "None";
		this.lblSaleFreightAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Teal;
		this.label35.Location = new System.Drawing.Point(224, 96);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(63, 15);
		this.label35.TabIndex = 685;
		this.label35.Text = "Add / Less";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Teal;
		this.label36.Location = new System.Drawing.Point(5, 96);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(95, 15);
		this.label36.TabIndex = 687;
		this.label36.Text = "Freight Amount";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Teal;
		this.label38.Location = new System.Drawing.Point(5, 77);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(119, 15);
		this.label38.TabIndex = 679;
		this.label38.Text = "Freight Account (Cr)";
		this.txtDeliveryTermSale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDeliveryTermSale.ForeColor = System.Drawing.Color.Black;
		this.txtDeliveryTermSale.Location = new System.Drawing.Point(131, 56);
		this.txtDeliveryTermSale.Name = "txtDeliveryTermSale";
		this.txtDeliveryTermSale.Size = new System.Drawing.Size(198, 20);
		this.txtDeliveryTermSale.TabIndex = 184;
		this.txtDeliveryTermSale.Text = "None";
		this.txtDeliveryTermSale.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.Teal;
		this.label11.Location = new System.Drawing.Point(527, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(129, 19);
		this.label11.TabIndex = 8;
		this.label11.Text = "Trading Account :";
		this.lblTradingAccount.AutoEllipsis = true;
		this.lblTradingAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTradingAccount.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.lblTradingAccount.Location = new System.Drawing.Point(368, 26);
		this.lblTradingAccount.Name = "lblTradingAccount";
		this.lblTradingAccount.Size = new System.Drawing.Size(419, 25);
		this.lblTradingAccount.TabIndex = 184;
		this.lblTradingAccount.Text = "None";
		this.lblTradingAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.GbWbComparison.Controls.Add(this.txtDiffAvgNetRate);
		this.GbWbComparison.Controls.Add(this.label25);
		this.GbWbComparison.Controls.Add(this.txtPurchaseAvgNetRate);
		this.GbWbComparison.Controls.Add(this.txtSaleAvgNetRate);
		this.GbWbComparison.Controls.Add(this.lblBillWeightDiff);
		this.GbWbComparison.Controls.Add(this.label62);
		this.GbWbComparison.Controls.Add(this.label82);
		this.GbWbComparison.Controls.Add(this.lblNetWeightDifference);
		this.GbWbComparison.Controls.Add(this.label83);
		this.GbWbComparison.Controls.Add(this.lblBillWeightGrn);
		this.GbWbComparison.Controls.Add(this.lblSecondWtDifference);
		this.GbWbComparison.Controls.Add(this.lblSupplierWeight);
		this.GbWbComparison.Controls.Add(this.lblFirstWtDifference);
		this.GbWbComparison.Controls.Add(this.lblSupplierSecondWeight);
		this.GbWbComparison.Controls.Add(this.lblSupplierFirstWeight);
		this.GbWbComparison.Controls.Add(this.lblBillWeightGdn);
		this.GbWbComparison.Controls.Add(this.label80);
		this.GbWbComparison.Controls.Add(this.lblBuyerWeight);
		this.GbWbComparison.Controls.Add(this.label84);
		this.GbWbComparison.Controls.Add(this.label85);
		this.GbWbComparison.Controls.Add(this.lblBuyerSecondWeight);
		this.GbWbComparison.Controls.Add(this.label86);
		this.GbWbComparison.Controls.Add(this.lblBuyerFirstWeight);
		this.GbWbComparison.Font = new System.Drawing.Font("Tahoma", 8.5f, System.Drawing.FontStyle.Bold);
		this.GbWbComparison.ForeColor = System.Drawing.Color.Maroon;
		this.GbWbComparison.Location = new System.Drawing.Point(368, 53);
		this.GbWbComparison.Name = "GbWbComparison";
		this.GbWbComparison.Size = new System.Drawing.Size(421, 135);
		this.GbWbComparison.TabIndex = 3;
		this.GbWbComparison.TabStop = false;
		this.GbWbComparison.Text = "Weigh-Bridge Weight Comparison";
		this.lblBillWeightDiff.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBillWeightDiff.ForeColor = System.Drawing.Color.Black;
		this.lblBillWeightDiff.Location = new System.Drawing.Point(318, 95);
		this.lblBillWeightDiff.Name = "lblBillWeightDiff";
		this.lblBillWeightDiff.Size = new System.Drawing.Size(100, 20);
		this.lblBillWeightDiff.TabIndex = 690;
		this.lblBillWeightDiff.Text = "0";
		this.lblBillWeightDiff.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.Black;
		this.label62.Location = new System.Drawing.Point(5, 97);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(68, 15);
		this.label62.TabIndex = 675;
		this.label62.Text = "Bill Weight";
		this.label82.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label82.ForeColor = System.Drawing.Color.Black;
		this.label82.Location = new System.Drawing.Point(198, 14);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(110, 18);
		this.label82.TabIndex = 270;
		this.label82.Text = "Buyer";
		this.label82.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblNetWeightDifference.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNetWeightDifference.ForeColor = System.Drawing.Color.Black;
		this.lblNetWeightDifference.Location = new System.Drawing.Point(318, 74);
		this.lblNetWeightDifference.Name = "lblNetWeightDifference";
		this.lblNetWeightDifference.Size = new System.Drawing.Size(100, 20);
		this.lblNetWeightDifference.TabIndex = 689;
		this.lblNetWeightDifference.Text = "0";
		this.lblNetWeightDifference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label83.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label83.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label83.ForeColor = System.Drawing.Color.Black;
		this.label83.Location = new System.Drawing.Point(86, 14);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(110, 18);
		this.label83.TabIndex = 271;
		this.label83.Text = "Supplier";
		this.label83.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblBillWeightGrn.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBillWeightGrn.ForeColor = System.Drawing.Color.Black;
		this.lblBillWeightGrn.Location = new System.Drawing.Point(95, 95);
		this.lblBillWeightGrn.Name = "lblBillWeightGrn";
		this.lblBillWeightGrn.Size = new System.Drawing.Size(100, 20);
		this.lblBillWeightGrn.TabIndex = 682;
		this.lblBillWeightGrn.Text = "0";
		this.lblBillWeightGrn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblBillWeightGrn.TextChanged += new System.EventHandler(lblBillWeightGrn_TextChanged);
		this.lblSecondWtDifference.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSecondWtDifference.ForeColor = System.Drawing.Color.Black;
		this.lblSecondWtDifference.Location = new System.Drawing.Point(318, 53);
		this.lblSecondWtDifference.Name = "lblSecondWtDifference";
		this.lblSecondWtDifference.Size = new System.Drawing.Size(100, 20);
		this.lblSecondWtDifference.TabIndex = 688;
		this.lblSecondWtDifference.Text = "0";
		this.lblSecondWtDifference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSupplierWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSupplierWeight.ForeColor = System.Drawing.Color.Black;
		this.lblSupplierWeight.Location = new System.Drawing.Point(95, 74);
		this.lblSupplierWeight.Name = "lblSupplierWeight";
		this.lblSupplierWeight.Size = new System.Drawing.Size(100, 20);
		this.lblSupplierWeight.TabIndex = 681;
		this.lblSupplierWeight.Text = "0";
		this.lblSupplierWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSupplierWeight.TextChanged += new System.EventHandler(lblSupplierWeight_TextChanged);
		this.lblFirstWtDifference.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFirstWtDifference.ForeColor = System.Drawing.Color.Black;
		this.lblFirstWtDifference.Location = new System.Drawing.Point(318, 32);
		this.lblFirstWtDifference.Name = "lblFirstWtDifference";
		this.lblFirstWtDifference.Size = new System.Drawing.Size(100, 20);
		this.lblFirstWtDifference.TabIndex = 687;
		this.lblFirstWtDifference.Text = "0";
		this.lblFirstWtDifference.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSupplierSecondWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSupplierSecondWeight.ForeColor = System.Drawing.Color.Black;
		this.lblSupplierSecondWeight.Location = new System.Drawing.Point(95, 53);
		this.lblSupplierSecondWeight.Name = "lblSupplierSecondWeight";
		this.lblSupplierSecondWeight.Size = new System.Drawing.Size(100, 20);
		this.lblSupplierSecondWeight.TabIndex = 680;
		this.lblSupplierSecondWeight.Text = "0";
		this.lblSupplierSecondWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSupplierSecondWeight.TextChanged += new System.EventHandler(lblSupplierSecondWeight_TextChanged);
		this.lblSupplierFirstWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSupplierFirstWeight.ForeColor = System.Drawing.Color.Black;
		this.lblSupplierFirstWeight.Location = new System.Drawing.Point(95, 32);
		this.lblSupplierFirstWeight.Name = "lblSupplierFirstWeight";
		this.lblSupplierFirstWeight.Size = new System.Drawing.Size(100, 20);
		this.lblSupplierFirstWeight.TabIndex = 679;
		this.lblSupplierFirstWeight.Text = "0";
		this.lblSupplierFirstWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblSupplierFirstWeight.TextChanged += new System.EventHandler(lblSupplierFirstWeight_TextChanged);
		this.lblBillWeightGdn.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBillWeightGdn.ForeColor = System.Drawing.Color.Black;
		this.lblBillWeightGdn.Location = new System.Drawing.Point(209, 95);
		this.lblBillWeightGdn.Name = "lblBillWeightGdn";
		this.lblBillWeightGdn.Size = new System.Drawing.Size(100, 20);
		this.lblBillWeightGdn.TabIndex = 686;
		this.lblBillWeightGdn.Text = "0";
		this.lblBillWeightGdn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblBillWeightGdn.TextChanged += new System.EventHandler(lblBillWeightGdn_TextChanged);
		this.label80.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label80.ForeColor = System.Drawing.Color.Black;
		this.label80.Location = new System.Drawing.Point(309, 14);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(110, 18);
		this.label80.TabIndex = 269;
		this.label80.Text = "Differences";
		this.label80.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblBuyerWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBuyerWeight.ForeColor = System.Drawing.Color.Black;
		this.lblBuyerWeight.Location = new System.Drawing.Point(209, 74);
		this.lblBuyerWeight.Name = "lblBuyerWeight";
		this.lblBuyerWeight.Size = new System.Drawing.Size(100, 20);
		this.lblBuyerWeight.TabIndex = 685;
		this.lblBuyerWeight.Text = "0";
		this.lblBuyerWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblBuyerWeight.TextChanged += new System.EventHandler(lblBuyerWeight_TextChanged);
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25f, System.Drawing.FontStyle.Bold);
		this.label84.ForeColor = System.Drawing.Color.Black;
		this.label84.Location = new System.Drawing.Point(4, 55);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(81, 17);
		this.label84.TabIndex = 268;
		this.label84.Text = "Tare Weight";
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25f, System.Drawing.FontStyle.Bold);
		this.label85.ForeColor = System.Drawing.Color.Black;
		this.label85.Location = new System.Drawing.Point(4, 34);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(85, 17);
		this.label85.TabIndex = 267;
		this.label85.Text = "Load Weight";
		this.lblBuyerSecondWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBuyerSecondWeight.ForeColor = System.Drawing.Color.Black;
		this.lblBuyerSecondWeight.Location = new System.Drawing.Point(209, 53);
		this.lblBuyerSecondWeight.Name = "lblBuyerSecondWeight";
		this.lblBuyerSecondWeight.Size = new System.Drawing.Size(100, 20);
		this.lblBuyerSecondWeight.TabIndex = 684;
		this.lblBuyerSecondWeight.Text = "0";
		this.lblBuyerSecondWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblBuyerSecondWeight.TextChanged += new System.EventHandler(lblBuyerSecondWeight_TextChanged);
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25f, System.Drawing.FontStyle.Bold);
		this.label86.ForeColor = System.Drawing.Color.Black;
		this.label86.Location = new System.Drawing.Point(4, 76);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(78, 17);
		this.label86.TabIndex = 266;
		this.label86.Text = "Net Weight";
		this.lblBuyerFirstWeight.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBuyerFirstWeight.ForeColor = System.Drawing.Color.Black;
		this.lblBuyerFirstWeight.Location = new System.Drawing.Point(209, 32);
		this.lblBuyerFirstWeight.Name = "lblBuyerFirstWeight";
		this.lblBuyerFirstWeight.Size = new System.Drawing.Size(100, 20);
		this.lblBuyerFirstWeight.TabIndex = 683;
		this.lblBuyerFirstWeight.Text = "0";
		this.lblBuyerFirstWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblBuyerFirstWeight.TextChanged += new System.EventHandler(lblBuyerFirstWeight_TextChanged);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Teal;
		this.label16.Location = new System.Drawing.Point(5, 237);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(92, 19);
		this.label16.TabIndex = 8;
		this.label16.Text = "Bill Remarks";
		this.GbPurchaseSide.Controls.Add(this.lblWhtPurchasePercent);
		this.GbPurchaseSide.Controls.Add(this.lblWhtPurchaseAmount);
		this.GbPurchaseSide.Controls.Add(this.label42);
		this.GbPurchaseSide.Controls.Add(this.label43);
		this.GbPurchaseSide.Controls.Add(this.lblWhtPurchaseAccount);
		this.GbPurchaseSide.Controls.Add(this.label40);
		this.GbPurchaseSide.Controls.Add(this.lblPurchseTaxAccount);
		this.GbPurchaseSide.Controls.Add(this.label34);
		this.GbPurchaseSide.Controls.Add(this.label37);
		this.GbPurchaseSide.Controls.Add(this.txtBillAmountPurchase);
		this.GbPurchaseSide.Controls.Add(this.lblFreightRemarksPurchase);
		this.GbPurchaseSide.Controls.Add(this.label56);
		this.GbPurchaseSide.Controls.Add(this.lblFreightAccountPurchase);
		this.GbPurchaseSide.Controls.Add(this.lblTotalFreightPurchase);
		this.GbPurchaseSide.Controls.Add(this.label9);
		this.GbPurchaseSide.Controls.Add(this.lblFreightAddLessPurchase);
		this.GbPurchaseSide.Controls.Add(this.label12);
		this.GbPurchaseSide.Controls.Add(this.lblFreightAmountPurchase);
		this.GbPurchaseSide.Controls.Add(this.label15);
		this.GbPurchaseSide.Controls.Add(this.label22);
		this.GbPurchaseSide.Controls.Add(this.label20);
		this.GbPurchaseSide.Controls.Add(this.txtSupplierName);
		this.GbPurchaseSide.Controls.Add(this.label39);
		this.GbPurchaseSide.Controls.Add(this.txtDeliveryTermPurchase);
		this.GbPurchaseSide.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.GbPurchaseSide.ForeColor = System.Drawing.Color.Blue;
		this.GbPurchaseSide.Location = new System.Drawing.Point(1, 2);
		this.GbPurchaseSide.Name = "GbPurchaseSide";
		this.GbPurchaseSide.Size = new System.Drawing.Size(362, 230);
		this.GbPurchaseSide.TabIndex = 1;
		this.GbPurchaseSide.TabStop = false;
		this.GbPurchaseSide.Text = "Purchase Side";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
		this.label37.ForeColor = System.Drawing.Color.Teal;
		this.label37.Location = new System.Drawing.Point(5, 201);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(93, 20);
		this.label37.TabIndex = 498;
		this.label37.Text = "Bill Amount";
		this.txtBillAmountPurchase.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.txtBillAmountPurchase.ForeColor = System.Drawing.Color.Black;
		this.txtBillAmountPurchase.Location = new System.Drawing.Point(127, 201);
		this.txtBillAmountPurchase.Name = "txtBillAmountPurchase";
		this.txtBillAmountPurchase.Size = new System.Drawing.Size(130, 20);
		this.txtBillAmountPurchase.TabIndex = 184;
		this.txtBillAmountPurchase.Text = "0";
		this.txtBillAmountPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblFreightRemarksPurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFreightRemarksPurchase.ForeColor = System.Drawing.Color.Black;
		this.lblFreightRemarksPurchase.Location = new System.Drawing.Point(127, 127);
		this.lblFreightRemarksPurchase.Name = "lblFreightRemarksPurchase";
		this.lblFreightRemarksPurchase.Size = new System.Drawing.Size(229, 20);
		this.lblFreightRemarksPurchase.TabIndex = 502;
		this.lblFreightRemarksPurchase.Text = "None";
		this.lblFreightRemarksPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.Teal;
		this.label56.Location = new System.Drawing.Point(5, 130);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(99, 15);
		this.label56.TabIndex = 678;
		this.label56.Text = "Freight Remarks";
		this.lblFreightAccountPurchase.AutoEllipsis = true;
		this.lblFreightAccountPurchase.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFreightAccountPurchase.ForeColor = System.Drawing.Color.Black;
		this.lblFreightAccountPurchase.Location = new System.Drawing.Point(127, 75);
		this.lblFreightAccountPurchase.Name = "lblFreightAccountPurchase";
		this.lblFreightAccountPurchase.Size = new System.Drawing.Size(230, 18);
		this.lblFreightAccountPurchase.TabIndex = 499;
		this.lblFreightAccountPurchase.Text = "None";
		this.lblFreightAccountPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblTotalFreightPurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblTotalFreightPurchase.ForeColor = System.Drawing.Color.Black;
		this.lblTotalFreightPurchase.Location = new System.Drawing.Point(127, 110);
		this.lblTotalFreightPurchase.Name = "lblTotalFreightPurchase";
		this.lblTotalFreightPurchase.Size = new System.Drawing.Size(100, 20);
		this.lblTotalFreightPurchase.TabIndex = 501;
		this.lblTotalFreightPurchase.Text = "None";
		this.lblTotalFreightPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Teal;
		this.label9.Location = new System.Drawing.Point(5, 113);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(71, 15);
		this.label9.TabIndex = 676;
		this.label9.Text = "Net Freight";
		this.lblFreightAddLessPurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFreightAddLessPurchase.ForeColor = System.Drawing.Color.Black;
		this.lblFreightAddLessPurchase.Location = new System.Drawing.Point(293, 93);
		this.lblFreightAddLessPurchase.Name = "lblFreightAddLessPurchase";
		this.lblFreightAddLessPurchase.Size = new System.Drawing.Size(64, 20);
		this.lblFreightAddLessPurchase.TabIndex = 500;
		this.lblFreightAddLessPurchase.Text = "None";
		this.lblFreightAddLessPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Teal;
		this.label12.Location = new System.Drawing.Point(5, 28);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(89, 15);
		this.label12.TabIndex = 499;
		this.label12.Text = "Supplier Name";
		this.lblFreightAmountPurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFreightAmountPurchase.ForeColor = System.Drawing.Color.Black;
		this.lblFreightAmountPurchase.Location = new System.Drawing.Point(127, 93);
		this.lblFreightAmountPurchase.Name = "lblFreightAmountPurchase";
		this.lblFreightAmountPurchase.Size = new System.Drawing.Size(97, 20);
		this.lblFreightAmountPurchase.TabIndex = 499;
		this.lblFreightAmountPurchase.Text = "None";
		this.lblFreightAmountPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Teal;
		this.label15.Location = new System.Drawing.Point(224, 96);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(63, 15);
		this.label15.TabIndex = 675;
		this.label15.Text = "Add / Less";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Teal;
		this.label22.Location = new System.Drawing.Point(5, 96);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(95, 15);
		this.label22.TabIndex = 677;
		this.label22.Text = "Freight Amount";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Teal;
		this.label20.Location = new System.Drawing.Point(5, 77);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(121, 15);
		this.label20.TabIndex = 471;
		this.label20.Text = "Freight Account (Dr)";
		this.txtSupplierName.AutoEllipsis = true;
		this.txtSupplierName.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierName.ForeColor = System.Drawing.Color.Black;
		this.txtSupplierName.Location = new System.Drawing.Point(127, 11);
		this.txtSupplierName.Name = "txtSupplierName";
		this.txtSupplierName.Size = new System.Drawing.Size(230, 48);
		this.txtSupplierName.TabIndex = 184;
		this.txtSupplierName.Text = "None";
		this.txtSupplierName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Teal;
		this.label39.Location = new System.Drawing.Point(5, 59);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(86, 15);
		this.label39.TabIndex = 498;
		this.label39.Text = "Delivery Term";
		this.txtDeliveryTermPurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDeliveryTermPurchase.ForeColor = System.Drawing.Color.Black;
		this.txtDeliveryTermPurchase.Location = new System.Drawing.Point(127, 56);
		this.txtDeliveryTermPurchase.Name = "txtDeliveryTermPurchase";
		this.txtDeliveryTermPurchase.Size = new System.Drawing.Size(219, 20);
		this.txtDeliveryTermPurchase.TabIndex = 184;
		this.txtDeliveryTermPurchase.Text = "None";
		this.txtDeliveryTermPurchase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtRemarks.Location = new System.Drawing.Point(131, 234);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(1026, 25);
		this.txtRemarks.TabIndex = 184;
		this.txtRemarks.Text = "None";
		this.txtRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.PanelOtherGridInfo.BackColor = System.Drawing.Color.White;
		this.PanelOtherGridInfo.Controls.Add(this.lblSaleCommissionDetail);
		this.PanelOtherGridInfo.Controls.Add(this.label28);
		this.PanelOtherGridInfo.Controls.Add(this.lblSalePaymentDetail);
		this.PanelOtherGridInfo.Controls.Add(this.label24);
		this.PanelOtherGridInfo.Controls.Add(this.lblPurchasePayment);
		this.PanelOtherGridInfo.Controls.Add(this.label33);
		this.PanelOtherGridInfo.Controls.Add(this.lblPurchaseCommissionDetail);
		this.PanelOtherGridInfo.Controls.Add(this.label18);
		this.PanelOtherGridInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.PanelOtherGridInfo.Location = new System.Drawing.Point(0, 547);
		this.PanelOtherGridInfo.Name = "PanelOtherGridInfo";
		this.PanelOtherGridInfo.Size = new System.Drawing.Size(1167, 140);
		this.PanelOtherGridInfo.TabIndex = 134;
		this.lblSaleCommissionDetail.AutoEllipsis = true;
		this.lblSaleCommissionDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblSaleCommissionDetail.Location = new System.Drawing.Point(140, 68);
		this.lblSaleCommissionDetail.Name = "lblSaleCommissionDetail";
		this.lblSaleCommissionDetail.Size = new System.Drawing.Size(1020, 23);
		this.lblSaleCommissionDetail.TabIndex = 506;
		this.lblSaleCommissionDetail.Text = "None";
		this.lblSaleCommissionDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Teal;
		this.label28.Location = new System.Drawing.Point(5, 72);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(102, 15);
		this.label28.TabIndex = 505;
		this.label28.Text = "Sale Commission:";
		this.lblSalePaymentDetail.AutoEllipsis = true;
		this.lblSalePaymentDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblSalePaymentDetail.Location = new System.Drawing.Point(140, 26);
		this.lblSalePaymentDetail.Name = "lblSalePaymentDetail";
		this.lblSalePaymentDetail.Size = new System.Drawing.Size(1020, 23);
		this.lblSalePaymentDetail.TabIndex = 504;
		this.lblSalePaymentDetail.Text = "None";
		this.lblSalePaymentDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Teal;
		this.label24.Location = new System.Drawing.Point(5, 30);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(85, 15);
		this.label24.TabIndex = 503;
		this.label24.Text = "Sale Payment:";
		this.lblPurchasePayment.AutoEllipsis = true;
		this.lblPurchasePayment.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblPurchasePayment.Location = new System.Drawing.Point(140, 5);
		this.lblPurchasePayment.Name = "lblPurchasePayment";
		this.lblPurchasePayment.Size = new System.Drawing.Size(1020, 23);
		this.lblPurchasePayment.TabIndex = 502;
		this.lblPurchasePayment.Text = "None";
		this.lblPurchasePayment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Teal;
		this.label33.Location = new System.Drawing.Point(5, 9);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(112, 15);
		this.label33.TabIndex = 501;
		this.label33.Text = "Purchase Payment:";
		this.lblPurchaseCommissionDetail.AutoEllipsis = true;
		this.lblPurchaseCommissionDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblPurchaseCommissionDetail.Location = new System.Drawing.Point(140, 48);
		this.lblPurchaseCommissionDetail.Name = "lblPurchaseCommissionDetail";
		this.lblPurchaseCommissionDetail.Size = new System.Drawing.Size(1020, 23);
		this.lblPurchaseCommissionDetail.TabIndex = 500;
		this.lblPurchaseCommissionDetail.Text = "None";
		this.lblPurchaseCommissionDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Teal;
		this.label18.Location = new System.Drawing.Point(5, 52);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(129, 15);
		this.label18.TabIndex = 499;
		this.label18.Text = "Purchase Commission:";
		this.PanelMainSelectedNetPreviousDocData.BackColor = System.Drawing.Color.White;
		this.PanelMainSelectedNetPreviousDocData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.lblPlAmount);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnShowPreviousRecord);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.label52);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnApproveThis);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnShowNextRecord);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.label6);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.txtDocNo);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.txtDocDate);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.label3);
		this.PanelMainSelectedNetPreviousDocData.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelMainSelectedNetPreviousDocData.Location = new System.Drawing.Point(0, 0);
		this.PanelMainSelectedNetPreviousDocData.Name = "PanelMainSelectedNetPreviousDocData";
		this.PanelMainSelectedNetPreviousDocData.Size = new System.Drawing.Size(1167, 34);
		this.PanelMainSelectedNetPreviousDocData.TabIndex = 0;
		this.lblPlAmount.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.lblPlAmount.Location = new System.Drawing.Point(922, 5);
		this.lblPlAmount.Name = "lblPlAmount";
		this.lblPlAmount.Size = new System.Drawing.Size(128, 20);
		this.lblPlAmount.TabIndex = 679;
		this.lblPlAmount.Text = "0";
		this.lblPlAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(726, 6);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(94, 18);
		this.label52.TabIndex = 499;
		this.label52.Text = "Profit/Loss";
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
		this.BtnApproveThis.Size = new System.Drawing.Size(129, 25);
		this.BtnApproveThis.TabIndex = 0;
		this.BtnApproveThis.Text = "Approve This";
		this.BtnApproveThis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnApproveThis.UseVisualStyleBackColor = false;
		this.BtnApproveThis.Click += new System.EventHandler(BtnApproveThis_Click);
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
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Teal;
		this.label6.Location = new System.Drawing.Point(286, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 26);
		this.label6.TabIndex = 6;
		this.label6.Text = "Doc #";
		this.txtDocNo.AutoSize = true;
		this.txtDocNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.txtDocNo.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.ForeColor = System.Drawing.Color.Blue;
		this.txtDocNo.Location = new System.Drawing.Point(343, 3);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(23, 26);
		this.txtDocNo.TabIndex = 7;
		this.txtDocNo.Text = "0";
		this.txtDocNo.Click += new System.EventHandler(txtDocNo_Click);
		this.txtDocDate.AutoSize = true;
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.ForeColor = System.Drawing.Color.Black;
		this.txtDocDate.Location = new System.Drawing.Point(573, 6);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(46, 19);
		this.txtDocDate.TabIndex = 9;
		this.txtDocDate.Text = "None";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(528, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(49, 19);
		this.label3.TabIndex = 8;
		this.label3.Text = "Date: ";
		this.txtDiffAvgNetRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiffAvgNetRate.ForeColor = System.Drawing.Color.Black;
		this.txtDiffAvgNetRate.Location = new System.Drawing.Point(318, 112);
		this.txtDiffAvgNetRate.Name = "txtDiffAvgNetRate";
		this.txtDiffAvgNetRate.Size = new System.Drawing.Size(100, 20);
		this.txtDiffAvgNetRate.TabIndex = 694;
		this.txtDiffAvgNetRate.Text = "0";
		this.txtDiffAvgNetRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Black;
		this.label25.Location = new System.Drawing.Point(5, 114);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(82, 15);
		this.label25.TabIndex = 691;
		this.label25.Text = "Avg Net Rate";
		this.txtPurchaseAvgNetRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseAvgNetRate.ForeColor = System.Drawing.Color.Black;
		this.txtPurchaseAvgNetRate.Location = new System.Drawing.Point(95, 112);
		this.txtPurchaseAvgNetRate.Name = "txtPurchaseAvgNetRate";
		this.txtPurchaseAvgNetRate.Size = new System.Drawing.Size(100, 20);
		this.txtPurchaseAvgNetRate.TabIndex = 692;
		this.txtPurchaseAvgNetRate.Text = "0";
		this.txtPurchaseAvgNetRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtSaleAvgNetRate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleAvgNetRate.ForeColor = System.Drawing.Color.Black;
		this.txtSaleAvgNetRate.Location = new System.Drawing.Point(209, 112);
		this.txtSaleAvgNetRate.Name = "txtSaleAvgNetRate";
		this.txtSaleAvgNetRate.Size = new System.Drawing.Size(100, 20);
		this.txtSaleAvgNetRate.TabIndex = 693;
		this.txtSaleAvgNetRate.Text = "0";
		this.txtSaleAvgNetRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.lblPurchseTaxAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblPurchseTaxAccount.ForeColor = System.Drawing.Color.Black;
		this.lblPurchseTaxAccount.Location = new System.Drawing.Point(127, 145);
		this.lblPurchseTaxAccount.Name = "lblPurchseTaxAccount";
		this.lblPurchseTaxAccount.Size = new System.Drawing.Size(229, 20);
		this.lblPurchseTaxAccount.TabIndex = 679;
		this.lblPurchseTaxAccount.Text = "None";
		this.lblPurchseTaxAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Teal;
		this.label34.Location = new System.Drawing.Point(5, 148);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(75, 15);
		this.label34.TabIndex = 680;
		this.label34.Text = "Tax Account";
		this.lblWhtPurchaseAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblWhtPurchaseAccount.ForeColor = System.Drawing.Color.Black;
		this.lblWhtPurchaseAccount.Location = new System.Drawing.Point(127, 163);
		this.lblWhtPurchaseAccount.Name = "lblWhtPurchaseAccount";
		this.lblWhtPurchaseAccount.Size = new System.Drawing.Size(229, 20);
		this.lblWhtPurchaseAccount.TabIndex = 681;
		this.lblWhtPurchaseAccount.Text = "None";
		this.lblWhtPurchaseAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Teal;
		this.label40.Location = new System.Drawing.Point(5, 166);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(84, 15);
		this.label40.TabIndex = 682;
		this.label40.Text = "WHT Account";
		this.lblWhtPurchasePercent.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblWhtPurchasePercent.ForeColor = System.Drawing.Color.Black;
		this.lblWhtPurchasePercent.Location = new System.Drawing.Point(293, 181);
		this.lblWhtPurchasePercent.Name = "lblWhtPurchasePercent";
		this.lblWhtPurchasePercent.Size = new System.Drawing.Size(64, 20);
		this.lblWhtPurchasePercent.TabIndex = 684;
		this.lblWhtPurchasePercent.Text = "None";
		this.lblWhtPurchasePercent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblWhtPurchaseAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblWhtPurchaseAmount.ForeColor = System.Drawing.Color.Black;
		this.lblWhtPurchaseAmount.Location = new System.Drawing.Point(127, 181);
		this.lblWhtPurchaseAmount.Name = "lblWhtPurchaseAmount";
		this.lblWhtPurchaseAmount.Size = new System.Drawing.Size(97, 20);
		this.lblWhtPurchaseAmount.TabIndex = 683;
		this.lblWhtPurchaseAmount.Text = "None";
		this.lblWhtPurchaseAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Teal;
		this.label42.Location = new System.Drawing.Point(224, 184);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(73, 15);
		this.label42.TabIndex = 685;
		this.label42.Text = "Tax Percent";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Teal;
		this.label43.Location = new System.Drawing.Point(5, 184);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(83, 15);
		this.label43.TabIndex = 686;
		this.label43.Text = "WHT Amount";
		this.lblWhtSalePercent.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblWhtSalePercent.ForeColor = System.Drawing.Color.Black;
		this.lblWhtSalePercent.Location = new System.Drawing.Point(297, 181);
		this.lblWhtSalePercent.Name = "lblWhtSalePercent";
		this.lblWhtSalePercent.Size = new System.Drawing.Size(64, 20);
		this.lblWhtSalePercent.TabIndex = 692;
		this.lblWhtSalePercent.Text = "None";
		this.lblWhtSalePercent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblWhtSaleAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblWhtSaleAmount.ForeColor = System.Drawing.Color.Black;
		this.lblWhtSaleAmount.Location = new System.Drawing.Point(131, 181);
		this.lblWhtSaleAmount.Name = "lblWhtSaleAmount";
		this.lblWhtSaleAmount.Size = new System.Drawing.Size(97, 20);
		this.lblWhtSaleAmount.TabIndex = 691;
		this.lblWhtSaleAmount.Text = "None";
		this.lblWhtSaleAmount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.Teal;
		this.label44.Location = new System.Drawing.Point(228, 184);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(73, 15);
		this.label44.TabIndex = 693;
		this.label44.Text = "Tax Percent";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Teal;
		this.label45.Location = new System.Drawing.Point(5, 184);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(83, 15);
		this.label45.TabIndex = 694;
		this.label45.Text = "WHT Amount";
		this.lblWHTSaleAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblWHTSaleAccount.ForeColor = System.Drawing.Color.Black;
		this.lblWHTSaleAccount.Location = new System.Drawing.Point(131, 163);
		this.lblWHTSaleAccount.Name = "lblWHTSaleAccount";
		this.lblWHTSaleAccount.Size = new System.Drawing.Size(229, 20);
		this.lblWHTSaleAccount.TabIndex = 689;
		this.lblWHTSaleAccount.Text = "None";
		this.lblWHTSaleAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Teal;
		this.label47.Location = new System.Drawing.Point(5, 166);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(84, 15);
		this.label47.TabIndex = 690;
		this.label47.Text = "WHT Account";
		this.lblTaxSaleAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblTaxSaleAccount.ForeColor = System.Drawing.Color.Black;
		this.lblTaxSaleAccount.Location = new System.Drawing.Point(131, 145);
		this.lblTaxSaleAccount.Name = "lblTaxSaleAccount";
		this.lblTaxSaleAccount.Size = new System.Drawing.Size(229, 20);
		this.lblTaxSaleAccount.TabIndex = 687;
		this.lblTaxSaleAccount.Text = "None";
		this.lblTaxSaleAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Teal;
		this.label49.Location = new System.Drawing.Point(5, 148);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(75, 15);
		this.label49.TabIndex = 688;
		this.label49.Text = "Tax Account";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1547, 711);
		base.Controls.Add(this.PanelMainSelectedDocData);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmApprovalCommissionTradeBill";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmApprovalCommissionTradeBill_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.TotalRecordPanel.ResumeLayout(false);
		this.TotalRecordPanel.PerformLayout();
		this.GBFilters.ResumeLayout(false);
		this.GBFilters.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.BtnSorting).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierNameFilter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameFilter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTradingAccountFilter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategoryFilter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameFilter).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.PanelMainSelectedDocData.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.PanelHeaderInfo.ResumeLayout(false);
		this.PanelHeaderInfo.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.GbWbComparison.ResumeLayout(false);
		this.GbWbComparison.PerformLayout();
		this.GbPurchaseSide.ResumeLayout(false);
		this.GbPurchaseSide.PerformLayout();
		this.PanelOtherGridInfo.ResumeLayout(false);
		this.PanelOtherGridInfo.PerformLayout();
		this.PanelMainSelectedNetPreviousDocData.ResumeLayout(false);
		this.PanelMainSelectedNetPreviousDocData.PerformLayout();
		base.ResumeLayout(false);
	}
}
