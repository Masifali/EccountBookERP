using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.CommissionAgent;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.CommissionAgent;
using Architecture.WinApp.CommissionAgent.FromWise_Helper_Methods;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.ModernHistories;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Cmagt.Approvals
{

public class frmApprovalPurchaseOrderCmagt : Form
{
	private WaitForm wf = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGridDbCall = new DataTable();

	private DataTable dtCustomerDetail = new DataTable();

	private DataTable dtSaleOrderMap = new DataTable();

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

	internal UltraCombo CmbItemName;

	private Label label1;

	private FlowLayoutPanel FlpSideBar;

	internal UltraCombo CmbBillToParty;

	private Label label7;

	internal UltraCombo CmbCommissionAgentName;

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

	private Panel PanelHeaderInfo;

	private Label txtDocDate;

	private Label label3;

	private Label txtDocNo;

	private Label label6;

	private Label txtDeliveryTerm;

	private Label label13;

	internal GridEX grdCustomerDetail;

	private Panel panel14;

	private Panel PanelMainSelectedNetPreviousDocData;

	private Button BtnShowPreviousRecord;

	private Button BtnShowNextRecord;

	private Label label19;

	private Button BtnApproveThis;

	private Label label11;

	private Label txtCommissionAgentName;

	private Label txtBillToPartyName;

	private Label label16;

	private Label txtRemarks;

	private Label label18;

	private ToolStripButton btnSelectAllCards;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private Button BtnGoToFirst;

	private Button BtnGoToLast;

	private PictureBox BtnSorting;

	private Label txtExpiryDate;

	private Label label17;

	private Label label4;

	internal UltraCombo CmbParentCategory;

	private Label txtDeliveryDays;

	private Label txtDeliveryStartDate;

	private Label label21;

	private Label label12;

	private GroupBox groupBox3;

	private Label label26;

	private Label label48;

	private Label label46;

	private Label label42;

	private Label label41;

	private CheckBox chkOtherExpenseAllowed;

	private CheckBox chkWithHoldingTaxApplied;

	private Label txtCommissionType;

	private GroupBox groupBox1;

	private Label txtBrokeryAccount;

	private Label label24;

	private Label label25;

	private Label label30;

	private Label txtBrokeryAmount;

	private Label txtBrokeryRate;

	private Label txtBrokeryUom;

	private Label txtBrokeryType;

	private Label label35;

	private Label label36;

	private Label txtCommissionAcount;

	private Label txtCommissionAmount;

	private Label txtCommissionRate;

	private Label txtCommissionUom;

	private Panel panel16;

	private Label label60;

	internal GridEX grdSaleOrderMapping;

	public frmApprovalPurchaseOrderCmagt(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		PurchaseOrderCmagt_Helper.InitializeDetailtable(dtCustomerDetail);
		PurchaseOrderCmagt_Helper.InitializeSaleOrderMapDetailtable(dtSaleOrderMap);
		InitializeComponentMethod();
	}

	private void frmApprovalPurchaseOrderCmagt_Load(object sender, EventArgs e)
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
					if (flagUnApprove)
					{
						labelFormHeading.Text = "Pending 'PurchaseOrder' For UnApproval";
						btnApproved.Text = "UnApprove";
					}
					else
					{
						labelFormHeading.Text = "Pending 'PurchaseOrder' For Approval";
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
			return purchaseOrderMaster.GetDataForDropDown(new ReportsParameters
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
			DataTable dtSP = dtCust.Clone();
			DataTable dtItem = dtCust.Clone();
			DataTable dtInv = dtCust.Clone();
			foreach (DataRow dr in dt.Rows)
			{
				switch (Conversion.ToString(dr["Activity"]))
				{
				case "CommissionAgent":
					dtCust.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "SupplierName":
					dtSP.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "Item":
					dtItem.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "ParentCategories":
					dtInv.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCommissionAgentName, dtCust, "Id", "Name", "Commission Agent Name", AllColumns: true, Conversion.ToInt(CmbCommissionAgentName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBillToParty, dtSP, "Id", "Name", "Bill To Party", AllColumns: true, Conversion.ToInt(CmbBillToParty.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbParentCategory, dtInv, "Id", "Name", "Parent Category", AllColumns: true, Conversion.ToInt(CmbParentCategory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			((Control)(object)CmbCommissionAgentName).Text = string.Empty;
			((Control)(object)CmbBillToParty).Text = string.Empty;
			((Control)(object)CmbParentCategory).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
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
			grn.CommissionAgentId = Conversion.ToInt(CmbCommissionAgentName.Value);
			grn.BillToPartyId = Conversion.ToInt(CmbBillToParty.Value);
			grn.ParentCategoryId = Conversion.ToInt(CmbParentCategory.Value);
			grn.ItemId = Conversion.ToInt(CmbItemName.Value);
			grn.ApprovedStatus = flagUnApprove;
			dtGridDbCall = purchaseOrderMaster.PurchaseOrder_DataForApproval(grn);
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
				group r by r.Field<int>("purchaseOrderMasterId") into g
				select g.First();
			distinctRows = (AscOrDesc ? distinctRows.OrderBy((DataRow r) => r.Field<int>("docNo")) : distinctRows.OrderByDescending((DataRow r) => r.Field<int>("docNo")));
			List<DataRow> distinctList = distinctRows.ToList();
			if (totalRecords == 0)
			{
				totalRecords = distinctList.Count;
			}
			int skip = (currentPage - 1) * pageSize;
			foreach (DataRow row in distinctList.Skip(skip).Take(pageSize))
			{
				int docId = Conversion.ToInt(row["purchaseOrderMasterId"]);
				Conversion.ToString(row["docNo"]);
				Conversion.ToString(row["EntryUserName"]);
				Conversion.ToString(row["ModifyUserName"]);
				string BrokerOrServiceProviderName = Conversion.ToString(row["CommissionAgentName"]);
				string BillToPartyName = Conversion.ToString(row["SupplierName"]);
				DateTime docDate = Conversion.ToDateTime(row["docDate"]);
				DateTime entryDate = Conversion.ToDateTime(row["EntryDate"]);
				DateTime modifyDate = Conversion.ToDateTime(row["ModifyDate"]);
				Conversion.ToInt(row["NoOfAttachments"]);
				string buyerNames = Conversion.ToString(row["BuyerNamesFromMapping"]);
				ApprovalSideBarMainCard card = new ApprovalSideBarMainCard
				{
					DocumentId = docId,
					Width = 350,
					Height = 140
				};
				card.chkSelectionWithMainText.Text = BillToPartyName;
				card.FlpMain.SuspendLayout();
				card.FlpMain.Controls.Clear();
				int cardHeight = 10;
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Com Agent:"),
					Col2 = new CardCellModel
					{
						Text = BrokerOrServiceProviderName,
						ColumnSpan = 3,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.DarkRed,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
				IEnumerable<string> buyers = Enumerable.Empty<string>();
				if (!string.IsNullOrWhiteSpace(buyerNames))
				{
					buyers = (from x in buyerNames.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries)
						select x.Trim() into x
						where !string.IsNullOrWhiteSpace(x)
						select x).Distinct();
				}
				if (!buyers.Any())
				{
					buyers = new string[1] { "None" };
				}
				foreach (string buyerName in buyers)
				{
					ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
					{
						Col1 = ApprovalFormHelper.DefaultLabelModel("Buyer Name:"),
						Col2 = new CardCellModel
						{
							Text = buyerName,
							ColumnSpan = 3,
							Font = new Font("Segoe UI", 9f, FontStyle.Bold),
							ForeColor = Color.Blue,
							Padding = Padding.Empty
						},
						RowHeight = 20
					});
					cardHeight += 27;
				}
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Deliver To:"),
					Col2 = new CardCellModel
					{
						Text = Conversion.ToString(row["DeliveryToPartyName"]),
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
					Col2 = ApprovalFormHelper.DefaultLinkModel(Conversion.ToString(row["docNo"]), delegate
					{
						CrystalReportPrint_Helper.PurchaseOrderSlip_1052(docId);
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
				cardHeight += 180;
				card.Height = cardHeight;
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
			purchaseOrderMaster Or = purchaseOrderMaster.ReadById(RecId);
			if (Or == null || Or.purchaseOrderDetailList == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			txtDocDate.Text = Or.docDate.ToString("d");
			txtDocNo.Text = Conversion.ToString((object)Or.docNo);
			txtBillToPartyName.Text = Conversion.ToString((object)Or.SupplierName);
			txtCommissionAgentName.Text = Conversion.ToString((object)Or.CommissionAgentName);
			txtDeliveryTerm.Text = Conversion.ToString((object)Or.DeliveryTerm);
			txtDeliveryStartDate.Text = Or.deliveryStartDate.ToString("d");
			txtDeliveryDays.Text = Conversion.ToString((object)Or.deliveryDays);
			txtExpiryDate.Text = Or.deliveryStartDate.AddDays(Conversion.ToInt((object)txtDeliveryDays.Text)).ToString("d");
			List<purchaseOrderCommissionDetail> purchaseOrderCommissionDetailList = Or.PurchaseOrderCommissionDetailList;
			if (purchaseOrderCommissionDetailList != null && purchaseOrderCommissionDetailList.Count > 0)
			{
				purchaseOrderCommissionDetail commission = Or.PurchaseOrderCommissionDetailList.FirstOrDefault((purchaseOrderCommissionDetail x) => x.agentTypeId == 1);
				purchaseOrderCommissionDetail brokery = Or.PurchaseOrderCommissionDetailList.FirstOrDefault((purchaseOrderCommissionDetail x) => x.agentTypeId == 2);
				if (commission != null)
				{
					txtCommissionAcount.Text = Conversion.ToString((object)commission.CommissionAgentName);
					txtCommissionType.Text = Conversion.ToString((object)commission.CommissionType);
					txtCommissionUom.Text = commission.RateUom.ToString("#,##.###");
					txtCommissionRate.Text = commission.commissionRate.ToString("#,##.###");
					txtCommissionAmount.Text = commission.commissionAmount.ToString("#,##.###");
				}
				if (brokery != null)
				{
					txtBrokeryAccount.Text = Conversion.ToString((object)brokery.CommissionAgentName);
					txtBrokeryType.Text = Conversion.ToString((object)brokery.CommissionType);
					txtBrokeryUom.Text = brokery.RateUom.ToString("#,##.###");
					txtBrokeryRate.Text = brokery.commissionRate.ToString("#,##.###");
					txtBrokeryAmount.Text = brokery.commissionAmount.ToString("#,##.###");
				}
			}
			txtRemarks.Text = Conversion.ToString((object)Or.remarksHeader);
			chkOtherExpenseAllowed.Checked = Or.isSupplierOtherChargesAllowed;
			chkWithHoldingTaxApplied.Checked = Or.isWhtApplied;
			dtCustomerDetail.Rows.Clear();
			PurchaseOrderCmagt_Helper.FillDetailFromListCommonForReadById(Or, dtCustomerDetail);
			grdCustomerDetail.DataSource = dtCustomerDetail;
			grdCustomerDetail.RetrieveStructure();
			grdSettingSettings();
			dtSaleOrderMap.Rows.Clear();
			for (int i = 0; i < Or.purchaseOrderSaleOrderMappingList.Count; i++)
			{
				dtSaleOrderMap.Rows.Add(Or.purchaseOrderSaleOrderMappingList[i].purchaseOrderSaleOrderMappingId, Or.purchaseOrderSaleOrderMappingList[i].saleOrderMasterId, Or.purchaseOrderSaleOrderMappingList[i].saleOrderDetailId, Or.purchaseOrderSaleOrderMappingList[i].SaleOrderNo, Or.purchaseOrderSaleOrderMappingList[i].buyerId, Or.purchaseOrderSaleOrderMappingList[i].BuyerName, Or.purchaseOrderSaleOrderMappingList[i].itemId, Or.purchaseOrderSaleOrderMappingList[i].ItemName, Or.purchaseOrderSaleOrderMappingList[i].SaleOrderWeight, Or.purchaseOrderSaleOrderMappingList[i].UsedSaleOrderWeightInPo, Or.purchaseOrderSaleOrderMappingList[i].SaleOrderBalWeight, Or.purchaseOrderSaleOrderMappingList[i].itemNetWeight);
			}
			if (dtSaleOrderMap.Columns.Count > 0)
			{
				grdSaleOrderMapping.DataSource = dtSaleOrderMap;
				grdSaleOrderMapping.RetrieveStructure();
				grdSaleOrderMappingSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleOrderMappingSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SoId", "SoDetailId", "BuyerId", "ItemId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"CurrentAllocationWeight",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdSaleOrderMapping, 2, 3, hiddenColumns, editableColumns);
			grdSaleOrderMapping.RootTable.Columns["SaleOrderNo"].Width = Constants.InventoryConstants.DocNoConstant + 15;
			grdSaleOrderMapping.RootTable.Columns["BuyerName"].Width = Constants.InventoryConstants.SupplierName;
			grdSaleOrderMapping.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdSaleOrderMapping.RootTable.Columns["ItemWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdSaleOrderMapping.RootTable.Columns["BookedWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdSaleOrderMapping.RootTable.Columns["BalanceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdSaleOrderMapping.RootTable.Columns["CurrentAllocationWeight"].Width = Constants.InventoryConstants.WeightConstant + 20;
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
			PurchaseOrderCmagt_Helper.DetailGridCommonSetting(grdCustomerDetail);
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
			txtBillToPartyName.Text = "None";
			txtCommissionAgentName.Text = "None";
			txtDeliveryTerm.Text = "None";
			txtDeliveryStartDate.Text = "None";
			txtDeliveryDays.Text = "0";
			txtExpiryDate.Text = "None";
			txtCommissionAcount.Text = "None";
			txtCommissionType.Text = "None";
			txtCommissionUom.Text = "None";
			txtCommissionRate.Text = "0";
			txtCommissionAmount.Text = "0";
			txtBrokeryAccount.Text = "None";
			txtBrokeryType.Text = "None";
			txtBrokeryUom.Text = "None";
			txtBrokeryRate.Text = "0";
			txtBrokeryAmount.Text = "0";
			txtRemarks.Text = "None";
			chkOtherExpenseAllowed.Checked = false;
			chkWithHoldingTaxApplied.Checked = false;
			dtCustomerDetail.Clear();
			dtSaleOrderMap.Clear();
			grdCustomerDetail.DataSource = null;
			grdSaleOrderMapping.DataSource = null;
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
			FormHelper.CanOpenForm("frmPurchaseOrderCmagt", delegate
			{
				frmPurchaseOrderCmagt frmPurchaseOrderCmagt = new frmPurchaseOrderCmagt(UserAccount);
				frmPurchaseOrderCmagt.Show();
				frmPurchaseOrderCmagt.ReadById(RecId);
				frmPurchaseOrderCmagt.BringToFront();
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
			ApprovalList card = ApprovalFormHelper.ProcessApproval(selectedCards, UserAccount, flagUnApprove, DocumentTypeId, txtApprovalRemarks, "Purchase Order");
			if (card != null)
			{
				purchaseOrderMaster.UpdateApprovalStatus(card);
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
			purchaseOrderMaster.UpdateApprovalStatus(approvalWrapper);
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
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Expected O, but got Unknown
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Expected O, but got Unknown
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance9 = new Appearance();
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
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		GridEXLayout grdCustomerDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSaleOrderMapping_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Cmagt.Approvals.frmApprovalPurchaseOrderCmagt));
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
		this.CmbBillToParty = new UltraCombo();
		this.CmbCommissionAgentName = new UltraCombo();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.CmbParentCategory = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
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
		this.label19 = new System.Windows.Forms.Label();
		this.PanelHeaderInfo = new System.Windows.Forms.Panel();
		this.txtCommissionAgentName = new System.Windows.Forms.Label();
		this.txtBillToPartyName = new System.Windows.Forms.Label();
		this.txtExpiryDate = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtBrokeryAccount = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.txtBrokeryAmount = new System.Windows.Forms.Label();
		this.txtBrokeryRate = new System.Windows.Forms.Label();
		this.txtBrokeryUom = new System.Windows.Forms.Label();
		this.txtBrokeryType = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtCommissionAcount = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.txtCommissionAmount = new System.Windows.Forms.Label();
		this.txtCommissionRate = new System.Windows.Forms.Label();
		this.txtCommissionUom = new System.Windows.Forms.Label();
		this.txtCommissionType = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtDeliveryDays = new System.Windows.Forms.Label();
		this.txtDeliveryStartDate = new System.Windows.Forms.Label();
		this.txtDeliveryTerm = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.Label();
		this.PanelMainSelectedNetPreviousDocData = new System.Windows.Forms.Panel();
		this.chkOtherExpenseAllowed = new System.Windows.Forms.CheckBox();
		this.BtnShowPreviousRecord = new System.Windows.Forms.Button();
		this.BtnApproveThis = new System.Windows.Forms.Button();
		this.chkWithHoldingTaxApplied = new System.Windows.Forms.CheckBox();
		this.BtnShowNextRecord = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.grdSaleOrderMapping = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label60 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.TotalRecordPanel.SuspendLayout();
		this.GBFilters.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.BtnSorting).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel3.SuspendLayout();
		this.PanelMainSelectedDocData.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.PanelHeaderInfo.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.PanelMainSelectedNetPreviousDocData.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrderMapping).BeginInit();
		this.panel16.SuspendLayout();
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
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillToParty);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgentName);
		this.GBFilters.Controls.Add(this.txtDocNoFrom);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.GBFilters.Controls.Add(this.GRNfromdate);
		this.GBFilters.Controls.Add(this.txtDocNoTo);
		this.GBFilters.Controls.Add(this.GRNToDate);
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
		this.BtnSorting.Location = new System.Drawing.Point(324, 171);
		this.BtnSorting.Name = "BtnSorting";
		this.BtnSorting.Size = new System.Drawing.Size(33, 32);
		this.BtnSorting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.BtnSorting.TabIndex = 189;
		this.BtnSorting.TabStop = false;
		this.BtnSorting.Click += new System.EventHandler(BtnSorting_Click);
		this.CmbBillToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbBillToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance26;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance31).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Location = new System.Drawing.Point(95, 88);
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Name = "CmbBillToParty";
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillToParty).TabIndex = 5;
		this.CmbCommissionAgentName.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgentName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgentName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgentName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgentName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgentName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentName).Location = new System.Drawing.Point(95, 60);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentName).Name = "CmbCommissionAgentName";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentName).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentName).TabIndex = 4;
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(95, 35);
		this.txtDocNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.ShortcutsEnabled = false;
		this.txtDocNoFrom.Size = new System.Drawing.Size(130, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDocNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoFrom_KeyPress);
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(95, 116);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 6;
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(95, 144);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 6;
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(95, 12);
		this.GRNfromdate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(130, 22);
		this.GRNfromdate.TabIndex = 0;
		this.txtDocNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(227, 35);
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
		this.GRNToDate.Location = new System.Drawing.Point(227, 12);
		this.GRNToDate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(130, 22);
		this.GRNToDate.TabIndex = 1;
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
		this.label14.Location = new System.Drawing.Point(5, 58);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(73, 30);
		this.label14.TabIndex = 636;
		this.label14.Text = "Commission\r\nAgent";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(5, 122);
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
		this.btnshow.Location = new System.Drawing.Point(266, 174);
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
		this.label10.Size = new System.Drawing.Size(60, 15);
		this.label10.TabIndex = 144;
		this.label10.Text = "From Doc";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(5, 150);
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
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "From Date";
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
		this.panel3.Size = new System.Drawing.Size(1184, 24);
		this.panel3.TabIndex = 52;
		this.labelFormHeading.AutoSize = true;
		this.labelFormHeading.BackColor = System.Drawing.Color.Teal;
		this.labelFormHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelFormHeading.ForeColor = System.Drawing.Color.White;
		this.labelFormHeading.Location = new System.Drawing.Point(3, 2);
		this.labelFormHeading.Name = "labelFormHeading";
		this.labelFormHeading.Size = new System.Drawing.Size(344, 19);
		this.labelFormHeading.TabIndex = 4;
		this.labelFormHeading.Text = "Pending 'Purchase Order' For UnApproval";
		this.txtApprovalRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovalRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovalRemarks.Location = new System.Drawing.Point(571, 1);
		this.txtApprovalRemarks.Margin = new System.Windows.Forms.Padding(4);
		this.txtApprovalRemarks.Name = "txtApprovalRemarks";
		this.txtApprovalRemarks.ShortcutsEnabled = false;
		this.txtApprovalRemarks.Size = new System.Drawing.Size(508, 23);
		this.txtApprovalRemarks.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(444, 4);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(120, 17);
		this.label2.TabIndex = 121;
		this.label2.Text = "Approval Remarks";
		this.PanelMainSelectedDocData.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerDetail);
		this.PanelMainSelectedDocData.Controls.Add(this.panel16);
		this.PanelMainSelectedDocData.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleOrderMapping);
		this.PanelMainSelectedDocData.Controls.Add(this.panel14);
		this.PanelMainSelectedDocData.Controls.Add(this.PanelHeaderInfo);
		this.PanelMainSelectedDocData.Controls.Add(this.PanelMainSelectedNetPreviousDocData);
		this.PanelMainSelectedDocData.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainSelectedDocData.Location = new System.Drawing.Point(380, 24);
		this.PanelMainSelectedDocData.Name = "PanelMainSelectedDocData";
		this.PanelMainSelectedDocData.Size = new System.Drawing.Size(804, 687);
		this.PanelMainSelectedDocData.TabIndex = 53;
		this.grdCustomerDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdCustomerDetail).BackColor = System.Drawing.Color.White;
		grdCustomerDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerDetail.DesignTimeLayout = grdCustomerDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Location = new System.Drawing.Point(0, 256);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Name = "grdCustomerDetail";
		((ScrollableBase)this.grdCustomerDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Size = new System.Drawing.Size(804, 236);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).TabIndex = 2;
		this.grdCustomerDetail.TotalRow = (InheritableBoolean)1;
		this.grdCustomerDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerDetail).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label19);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 235);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(804, 21);
		this.panel14.TabIndex = 133;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(5, 2);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(74, 16);
		this.label19.TabIndex = 126;
		this.label19.Text = "Detail Info\t\r\n";
		this.PanelHeaderInfo.BackColor = System.Drawing.Color.White;
		this.PanelHeaderInfo.Controls.Add(this.txtCommissionAgentName);
		this.PanelHeaderInfo.Controls.Add(this.txtBillToPartyName);
		this.PanelHeaderInfo.Controls.Add(this.txtExpiryDate);
		this.PanelHeaderInfo.Controls.Add(this.label17);
		this.PanelHeaderInfo.Controls.Add(this.groupBox1);
		this.PanelHeaderInfo.Controls.Add(this.groupBox3);
		this.PanelHeaderInfo.Controls.Add(this.txtDeliveryDays);
		this.PanelHeaderInfo.Controls.Add(this.txtDeliveryStartDate);
		this.PanelHeaderInfo.Controls.Add(this.txtDeliveryTerm);
		this.PanelHeaderInfo.Controls.Add(this.label11);
		this.PanelHeaderInfo.Controls.Add(this.label16);
		this.PanelHeaderInfo.Controls.Add(this.label21);
		this.PanelHeaderInfo.Controls.Add(this.label12);
		this.PanelHeaderInfo.Controls.Add(this.label18);
		this.PanelHeaderInfo.Controls.Add(this.label13);
		this.PanelHeaderInfo.Controls.Add(this.txtRemarks);
		this.PanelHeaderInfo.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHeaderInfo.Location = new System.Drawing.Point(0, 34);
		this.PanelHeaderInfo.Name = "PanelHeaderInfo";
		this.PanelHeaderInfo.Size = new System.Drawing.Size(804, 201);
		this.PanelHeaderInfo.TabIndex = 1;
		this.txtCommissionAgentName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionAgentName.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtCommissionAgentName.Location = new System.Drawing.Point(173, 32);
		this.txtCommissionAgentName.Name = "txtCommissionAgentName";
		this.txtCommissionAgentName.Size = new System.Drawing.Size(590, 25);
		this.txtCommissionAgentName.TabIndex = 184;
		this.txtCommissionAgentName.Text = "None";
		this.txtCommissionAgentName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtBillToPartyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillToPartyName.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtBillToPartyName.Location = new System.Drawing.Point(173, 5);
		this.txtBillToPartyName.Name = "txtBillToPartyName";
		this.txtBillToPartyName.Size = new System.Drawing.Size(590, 25);
		this.txtBillToPartyName.TabIndex = 184;
		this.txtBillToPartyName.Text = "None";
		this.txtBillToPartyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtExpiryDate.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.txtExpiryDate.ForeColor = System.Drawing.Color.Red;
		this.txtExpiryDate.Location = new System.Drawing.Point(92, 142);
		this.txtExpiryDate.Name = "txtExpiryDate";
		this.txtExpiryDate.Size = new System.Drawing.Size(130, 25);
		this.txtExpiryDate.TabIndex = 188;
		this.txtExpiryDate.Text = "None";
		this.txtExpiryDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label17.ForeColor = System.Drawing.Color.Teal;
		this.label17.Location = new System.Drawing.Point(5, 147);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(71, 15);
		this.label17.TabIndex = 188;
		this.label17.Text = "Expiry Date";
		this.groupBox1.Controls.Add(this.txtBrokeryAccount);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.txtBrokeryAmount);
		this.groupBox1.Controls.Add(this.txtBrokeryRate);
		this.groupBox1.Controls.Add(this.txtBrokeryUom);
		this.groupBox1.Controls.Add(this.txtBrokeryType);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.ForeColor = System.Drawing.Color.Teal;
		this.groupBox1.Location = new System.Drawing.Point(494, 67);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(269, 100);
		this.groupBox1.TabIndex = 189;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Brokery";
		this.txtBrokeryAccount.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.txtBrokeryAccount.ForeColor = System.Drawing.Color.Black;
		this.txtBrokeryAccount.Location = new System.Drawing.Point(36, 14);
		this.txtBrokeryAccount.Name = "txtBrokeryAccount";
		this.txtBrokeryAccount.Size = new System.Drawing.Size(231, 25);
		this.txtBrokeryAccount.TabIndex = 188;
		this.txtBrokeryAccount.Text = "None";
		this.txtBrokeryAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(3, 19);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(26, 15);
		this.label24.TabIndex = 33;
		this.label24.Text = "A/c";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(3, 62);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(33, 30);
		this.label25.TabIndex = 30;
		this.label25.Text = "Rate\r\nUom";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(124, 45);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(30, 15);
		this.label30.TabIndex = 35;
		this.label30.Text = "Rate";
		this.txtBrokeryAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtBrokeryAmount.ForeColor = System.Drawing.Color.Black;
		this.txtBrokeryAmount.Location = new System.Drawing.Point(177, 67);
		this.txtBrokeryAmount.Name = "txtBrokeryAmount";
		this.txtBrokeryAmount.Size = new System.Drawing.Size(90, 20);
		this.txtBrokeryAmount.TabIndex = 184;
		this.txtBrokeryAmount.Text = "0";
		this.txtBrokeryAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtBrokeryRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtBrokeryRate.ForeColor = System.Drawing.Color.Black;
		this.txtBrokeryRate.Location = new System.Drawing.Point(177, 42);
		this.txtBrokeryRate.Name = "txtBrokeryRate";
		this.txtBrokeryRate.Size = new System.Drawing.Size(90, 20);
		this.txtBrokeryRate.TabIndex = 184;
		this.txtBrokeryRate.Text = "0";
		this.txtBrokeryRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtBrokeryUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtBrokeryUom.ForeColor = System.Drawing.Color.Black;
		this.txtBrokeryUom.Location = new System.Drawing.Point(37, 67);
		this.txtBrokeryUom.Name = "txtBrokeryUom";
		this.txtBrokeryUom.Size = new System.Drawing.Size(85, 20);
		this.txtBrokeryUom.TabIndex = 184;
		this.txtBrokeryUom.Text = "None";
		this.txtBrokeryUom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtBrokeryType.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtBrokeryType.ForeColor = System.Drawing.Color.Black;
		this.txtBrokeryType.Location = new System.Drawing.Point(37, 42);
		this.txtBrokeryType.Name = "txtBrokeryType";
		this.txtBrokeryType.Size = new System.Drawing.Size(85, 20);
		this.txtBrokeryType.TabIndex = 184;
		this.txtBrokeryType.Text = "None";
		this.txtBrokeryType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(3, 45);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(32, 15);
		this.label35.TabIndex = 31;
		this.label35.Text = "Type";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(124, 70);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(51, 15);
		this.label36.TabIndex = 37;
		this.label36.Text = "Amount";
		this.groupBox3.Controls.Add(this.txtCommissionAcount);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.label48);
		this.groupBox3.Controls.Add(this.label46);
		this.groupBox3.Controls.Add(this.txtCommissionAmount);
		this.groupBox3.Controls.Add(this.txtCommissionRate);
		this.groupBox3.Controls.Add(this.txtCommissionUom);
		this.groupBox3.Controls.Add(this.txtCommissionType);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox3.ForeColor = System.Drawing.Color.Teal;
		this.groupBox3.Location = new System.Drawing.Point(224, 67);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(270, 100);
		this.groupBox3.TabIndex = 189;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Commission";
		this.txtCommissionAcount.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.txtCommissionAcount.ForeColor = System.Drawing.Color.Black;
		this.txtCommissionAcount.Location = new System.Drawing.Point(36, 14);
		this.txtCommissionAcount.Name = "txtCommissionAcount";
		this.txtCommissionAcount.Size = new System.Drawing.Size(231, 25);
		this.txtCommissionAcount.TabIndex = 188;
		this.txtCommissionAcount.Text = "None";
		this.txtCommissionAcount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(3, 19);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(26, 15);
		this.label26.TabIndex = 33;
		this.label26.Text = "A/c";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(3, 62);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(33, 30);
		this.label48.TabIndex = 30;
		this.label48.Text = "Rate\r\nUom";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(124, 45);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(30, 15);
		this.label46.TabIndex = 35;
		this.label46.Text = "Rate";
		this.txtCommissionAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtCommissionAmount.ForeColor = System.Drawing.Color.Black;
		this.txtCommissionAmount.Location = new System.Drawing.Point(177, 67);
		this.txtCommissionAmount.Name = "txtCommissionAmount";
		this.txtCommissionAmount.Size = new System.Drawing.Size(90, 20);
		this.txtCommissionAmount.TabIndex = 184;
		this.txtCommissionAmount.Text = "0";
		this.txtCommissionAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtCommissionRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtCommissionRate.ForeColor = System.Drawing.Color.Black;
		this.txtCommissionRate.Location = new System.Drawing.Point(177, 42);
		this.txtCommissionRate.Name = "txtCommissionRate";
		this.txtCommissionRate.Size = new System.Drawing.Size(90, 20);
		this.txtCommissionRate.TabIndex = 184;
		this.txtCommissionRate.Text = "0";
		this.txtCommissionRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtCommissionUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtCommissionUom.ForeColor = System.Drawing.Color.Black;
		this.txtCommissionUom.Location = new System.Drawing.Point(37, 67);
		this.txtCommissionUom.Name = "txtCommissionUom";
		this.txtCommissionUom.Size = new System.Drawing.Size(85, 20);
		this.txtCommissionUom.TabIndex = 184;
		this.txtCommissionUom.Text = "None";
		this.txtCommissionUom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtCommissionType.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtCommissionType.ForeColor = System.Drawing.Color.Black;
		this.txtCommissionType.Location = new System.Drawing.Point(37, 42);
		this.txtCommissionType.Name = "txtCommissionType";
		this.txtCommissionType.Size = new System.Drawing.Size(85, 20);
		this.txtCommissionType.TabIndex = 184;
		this.txtCommissionType.Text = "None";
		this.txtCommissionType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(3, 45);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(32, 15);
		this.label42.TabIndex = 31;
		this.label42.Text = "Type";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(124, 70);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(51, 15);
		this.label41.TabIndex = 37;
		this.label41.Text = "Amount";
		this.txtDeliveryDays.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDeliveryDays.Location = new System.Drawing.Point(92, 119);
		this.txtDeliveryDays.Name = "txtDeliveryDays";
		this.txtDeliveryDays.Size = new System.Drawing.Size(130, 20);
		this.txtDeliveryDays.TabIndex = 184;
		this.txtDeliveryDays.Text = "None";
		this.txtDeliveryDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtDeliveryStartDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDeliveryStartDate.Location = new System.Drawing.Point(92, 97);
		this.txtDeliveryStartDate.Name = "txtDeliveryStartDate";
		this.txtDeliveryStartDate.Size = new System.Drawing.Size(130, 20);
		this.txtDeliveryStartDate.TabIndex = 184;
		this.txtDeliveryStartDate.Text = "None";
		this.txtDeliveryStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(92, 75);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.Size = new System.Drawing.Size(130, 20);
		this.txtDeliveryTerm.TabIndex = 184;
		this.txtDeliveryTerm.Text = "None";
		this.txtDeliveryTerm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.Teal;
		this.label11.Location = new System.Drawing.Point(4, 34);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(133, 19);
		this.label11.TabIndex = 8;
		this.label11.Text = "Commision Agent ";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Teal;
		this.label16.Location = new System.Drawing.Point(5, 171);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(67, 19);
		this.label16.TabIndex = 8;
		this.label16.Text = "Remarks";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label21.ForeColor = System.Drawing.Color.Teal;
		this.label21.Location = new System.Drawing.Point(5, 125);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(83, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Delivery Days";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.Teal;
		this.label12.Location = new System.Drawing.Point(5, 92);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(65, 30);
		this.label12.TabIndex = 8;
		this.label12.Text = "Delivery\r\nStart Date";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Teal;
		this.label18.Location = new System.Drawing.Point(4, 8);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(170, 19);
		this.label18.TabIndex = 8;
		this.label18.Text = "Supplier (Bill To Party) :";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.Teal;
		this.label13.Location = new System.Drawing.Point(5, 77);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(86, 15);
		this.label13.TabIndex = 8;
		this.label13.Text = "Delivery Term";
		this.txtRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtRemarks.Location = new System.Drawing.Point(92, 169);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(671, 25);
		this.txtRemarks.TabIndex = 184;
		this.txtRemarks.Text = "None";
		this.txtRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.PanelMainSelectedNetPreviousDocData.BackColor = System.Drawing.Color.White;
		this.PanelMainSelectedNetPreviousDocData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.chkOtherExpenseAllowed);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnShowPreviousRecord);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnApproveThis);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.chkWithHoldingTaxApplied);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnShowNextRecord);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.label6);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.txtDocNo);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.txtDocDate);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.label3);
		this.PanelMainSelectedNetPreviousDocData.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelMainSelectedNetPreviousDocData.Location = new System.Drawing.Point(0, 0);
		this.PanelMainSelectedNetPreviousDocData.Name = "PanelMainSelectedNetPreviousDocData";
		this.PanelMainSelectedNetPreviousDocData.Size = new System.Drawing.Size(804, 34);
		this.PanelMainSelectedNetPreviousDocData.TabIndex = 0;
		this.chkOtherExpenseAllowed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkOtherExpenseAllowed.AutoSize = true;
		this.chkOtherExpenseAllowed.Enabled = false;
		this.chkOtherExpenseAllowed.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.chkOtherExpenseAllowed.ForeColor = System.Drawing.Color.Teal;
		this.chkOtherExpenseAllowed.Location = new System.Drawing.Point(314, 16);
		this.chkOtherExpenseAllowed.Name = "chkOtherExpenseAllowed";
		this.chkOtherExpenseAllowed.Size = new System.Drawing.Size(176, 17);
		this.chkOtherExpenseAllowed.TabIndex = 192;
		this.chkOtherExpenseAllowed.Text = "Other Expense Allowed";
		this.chkOtherExpenseAllowed.UseVisualStyleBackColor = true;
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
		this.BtnApproveThis.Size = new System.Drawing.Size(129, 25);
		this.BtnApproveThis.TabIndex = 0;
		this.BtnApproveThis.Text = "Approve This";
		this.BtnApproveThis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnApproveThis.UseVisualStyleBackColor = false;
		this.BtnApproveThis.Click += new System.EventHandler(BtnApproveThis_Click);
		this.chkWithHoldingTaxApplied.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkWithHoldingTaxApplied.AutoSize = true;
		this.chkWithHoldingTaxApplied.Enabled = false;
		this.chkWithHoldingTaxApplied.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.chkWithHoldingTaxApplied.ForeColor = System.Drawing.Color.Teal;
		this.chkWithHoldingTaxApplied.Location = new System.Drawing.Point(314, 1);
		this.chkWithHoldingTaxApplied.Name = "chkWithHoldingTaxApplied";
		this.chkWithHoldingTaxApplied.Size = new System.Drawing.Size(189, 17);
		this.chkWithHoldingTaxApplied.TabIndex = 191;
		this.chkWithHoldingTaxApplied.Text = "With Holding Tax Applied";
		this.chkWithHoldingTaxApplied.UseVisualStyleBackColor = true;
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
		this.label6.Location = new System.Drawing.Point(519, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 26);
		this.label6.TabIndex = 6;
		this.label6.Text = "Doc #";
		this.txtDocNo.AutoSize = true;
		this.txtDocNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.txtDocNo.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.ForeColor = System.Drawing.Color.Blue;
		this.txtDocNo.Location = new System.Drawing.Point(576, 3);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(23, 26);
		this.txtDocNo.TabIndex = 7;
		this.txtDocNo.Text = "0";
		this.txtDocNo.Click += new System.EventHandler(txtDocNo_Click);
		this.txtDocDate.AutoSize = true;
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.ForeColor = System.Drawing.Color.Black;
		this.txtDocDate.Location = new System.Drawing.Point(698, 6);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(46, 19);
		this.txtDocDate.TabIndex = 9;
		this.txtDocDate.Text = "None";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(653, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(49, 19);
		this.label3.TabIndex = 8;
		this.label3.Text = "Date: ";
		this.grdSaleOrderMapping.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdSaleOrderMapping).BackColor = System.Drawing.Color.White;
		this.grdSaleOrderMapping.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdSaleOrderMapping_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSaleOrderMapping.DesignTimeLayout = grdSaleOrderMapping_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMapping).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdSaleOrderMapping.FilterMode = (FilterMode)2;
		this.grdSaleOrderMapping.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMapping).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleOrderMapping.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMapping).Location = new System.Drawing.Point(0, 515);
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMapping).Name = "grdSaleOrderMapping";
		((ScrollableBase)this.grdSaleOrderMapping).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMapping).Size = new System.Drawing.Size(804, 172);
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMapping).TabIndex = 138;
		this.grdSaleOrderMapping.TotalRow = (InheritableBoolean)1;
		this.grdSaleOrderMapping.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleOrderMapping).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label60);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel16.Location = new System.Drawing.Point(0, 492);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(804, 23);
		this.panel16.TabIndex = 139;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Yellow;
		this.label60.Location = new System.Drawing.Point(4, 4);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(117, 15);
		this.label60.TabIndex = 239;
		this.label60.Text = "Sale Order Mapping";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 711);
		base.Controls.Add(this.PanelMainSelectedDocData);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmApprovalPurchaseOrderCmagt";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmApprovalPurchaseOrderCmagt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.TotalRecordPanel.ResumeLayout(false);
		this.TotalRecordPanel.PerformLayout();
		this.GBFilters.ResumeLayout(false);
		this.GBFilters.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.BtnSorting).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
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
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.PanelMainSelectedNetPreviousDocData.ResumeLayout(false);
		this.PanelMainSelectedNetPreviousDocData.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrderMapping).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
