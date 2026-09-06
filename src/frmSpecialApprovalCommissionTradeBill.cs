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
using Architecture.WinApp.CommissionAgent.FromWise_Helper_Methods;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.ModernHistories;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
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

public class frmSpecialApprovalCommissionTradeBill : Form
{
	private WaitForm wf = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGridDbCall = new DataTable();

	private DataTable dtCustomerDetail = new DataTable();

	private int currentPage = 1;

	private int pageSize = 20;

	private int totalRecords;

	public int DocumentTypeId;

	public int SpecialApproval_ReasonId;

	public int BranchesId;

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

	private Button BtnLoadNextRecord;

	private Button BtnLoadPreviousRecord;

	private ToolStripButton btnSelectAllCards;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private Button BtnGoToFirst;

	private Button BtnGoToLast;

	private PictureBox BtnSorting;

	private Label label4;

	internal UltraCombo CmbParentCategoryFilter;

	internal UltraCombo CmbBuyerNameFilter;

	private Label label8;

	public frmSpecialApprovalCommissionTradeBill(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		TradeBill_Helper.InitializeDetailtable(dtCustomerDetail);
		InitializeComponentMethod();
	}

	private void frmSpecialApprovalCommissionTradeBill_Load(object sender, EventArgs e)
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
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		try
		{
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
			grn.BranchesId = ((BranchesId > 0) ? BranchesId : UserAccount.BranchesId);
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
			grn.SpecialApprovalDocs = 1;
			grn.ReasonId = SpecialApproval_ReasonId;
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
				Conversion.ToString(row["TradingGlAccount"]);
				string ReasonRemarks = Conversion.ToString(row["ReasonRemarks"]);
				DateTime docDate = Conversion.ToDateTime(row["DocDate"]);
				DateTime entryDate = Conversion.ToDateTime(row["EntryDate"]);
				DateTime modifyDate = Conversion.ToDateTime(row["ModifyDate"]);
				Conversion.ToInt(row["NoOfAttachments"]);
				ApprovalSideBarMainCard card = new ApprovalSideBarMainCard
				{
					DocumentId = docId,
					Width = 350,
					Height = 280
				};
				card.chkSelectionWithMainText.Text = ReasonRemarks;
				card.FlpMain.SuspendLayout();
				card.FlpMain.Controls.Clear();
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Tading Account:"),
					Col2 = new CardCellModel
					{
						Text = Conversion.ToString(row["TradingGlAccount"]),
						ColumnSpan = 3,
						Font = new Font("Segoe UI", 9f),
						ForeColor = Color.DarkRed,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
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
					Col1 = new CardCellModel
					{
						Text = "Supplier Weight",
						ColumnSpan = 2,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.Teal,
						Padding = new Padding(0)
					},
					Col2 = new CardCellModel
					{
						Text = Conversion.ToString(row["SupplierBillWeight"]),
						ColumnSpan = 2,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.Blue,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = new CardCellModel
					{
						Text = "Buyer Weight",
						ColumnSpan = 2,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.Teal,
						Padding = new Padding(0)
					},
					Col2 = new CardCellModel
					{
						Text = Conversion.ToString(row["BuyerBillWeight"]),
						ColumnSpan = 2,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.Red,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = new CardCellModel
					{
						Text = "Difference Weight",
						ColumnSpan = 2,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.Teal,
						Padding = new Padding(0)
					},
					Col2 = new CardCellModel
					{
						Text = Conversion.ToString((object)(Conversion.ToDouble(row["SupplierBillWeight"]) - Conversion.ToDouble(row["BuyerBillWeight"]))),
						ColumnSpan = 2,
						Font = new Font("Segoe UI", 9f, FontStyle.Bold),
						ForeColor = Color.DarkRed,
						Padding = new Padding(0)
					},
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

	private void PrepareSidebar()
	{
		SelectAll = false;
		btnSelectAllCards.Text = (SelectAll ? "Un-Select All" : "Select All");
		FlpSideBar.SuspendLayout();
		FlpSideBar.Controls.Clear();
		FlpSideBar.AutoScroll = true;
		FlpSideBar.WrapContents = true;
		FlpSideBar.FlowDirection = FlowDirection.LeftToRight;
	}

	private void UpdateFooter()
	{
		TotalHeaderRecords.Text = $"{Math.Min(currentPage * pageSize, totalRecords)} / {totalRecords}";
		BtnLoadPreviousRecord.Enabled = currentPage > 1;
		BtnLoadNextRecord.Enabled = currentPage * pageSize < totalRecords;
		BtnGoToFirst.Enabled = currentPage > 1;
		BtnGoToLast.Enabled = currentPage * pageSize < totalRecords;
	}

	private void BuildCardUI(ApprovalSideBarMainCard card, DataRow row)
	{
		card.FlpMain.SuspendLayout();
		card.FlpMain.Controls.Clear();
		Font fontNormal = new Font("Segoe UI", 9f);
		Font fontBold = new Font("Segoe UI", 9f, FontStyle.Bold);
		Font fontHeader = new Font("Segoe UI", 10f, FontStyle.Bold);
		double supplierWeight = Conversion.ToDouble(row["SupplierBillWeight"]);
		double buyerWeight = Conversion.ToDouble(row["BuyerBillWeight"]);
		double difference = supplierWeight - buyerWeight;
		Color differenceColor = ((difference < 0.0) ? Color.DarkRed : Color.DarkGreen);
		card.Width = 370;
		card.Height = 285;
		card.Margin = new Padding(8);
		card.BackColor = Color.White;
		card.BorderStyle = BorderStyle.FixedSingle;
		card.chkSelectionWithMainText.Text = Conversion.ToString(row["ReasonRemarks"]);
		card.chkSelectionWithMainText.Font = fontHeader;
		ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
		{
			Col1 = new CardCellModel
			{
				Text = "\ud83c\udfe2 Trading",
				Font = fontBold,
				ForeColor = Color.Teal
			},
			Col2 = new CardCellModel
			{
				Text = Conversion.ToString(row["TradingGlAccount"]),
				ColumnSpan = 3,
				Font = fontBold,
				ForeColor = Color.DarkRed
			},
			RowHeight = 24
		});
		ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
		{
			Col1 = new CardCellModel
			{
				Text = "\ud83d\ude9b Supplier",
				Font = fontBold,
				ForeColor = Color.Teal
			},
			Col2 = new CardCellModel
			{
				Text = Conversion.ToString(row["SupplierName"]),
				ColumnSpan = 3,
				Font = fontBold,
				ForeColor = Color.Blue
			},
			RowHeight = 24
		});
		ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
		{
			Col1 = new CardCellModel
			{
				Text = "\ud83d\uded2 Buyer",
				Font = fontBold,
				ForeColor = Color.Teal
			},
			Col2 = new CardCellModel
			{
				Text = Conversion.ToString(row["BuyerName"]),
				ColumnSpan = 3,
				Font = fontBold,
				ForeColor = Color.DarkRed
			},
			RowHeight = 24
		});
		card.FlpMain.Controls.Add(new Panel
		{
			Height = 1,
			Width = 330,
			BackColor = Color.LightGray,
			Margin = new Padding(3, 5, 3, 5)
		});
		ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
		{
			Col1 = new CardCellModel
			{
				Text = "Supplier Weight",
				ColumnSpan = 2,
				Font = fontBold,
				ForeColor = Color.Teal
			},
			Col2 = new CardCellModel
			{
				Text = supplierWeight.ToString("N0"),
				ColumnSpan = 2,
				Font = fontBold,
				ForeColor = Color.Blue
			},
			RowHeight = 24
		});
		ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
		{
			Col1 = new CardCellModel
			{
				Text = "Buyer Weight",
				ColumnSpan = 2,
				Font = fontBold,
				ForeColor = Color.Teal
			},
			Col2 = new CardCellModel
			{
				Text = buyerWeight.ToString("N0"),
				ColumnSpan = 2,
				Font = fontBold,
				ForeColor = Color.DarkRed
			},
			RowHeight = 24
		});
		ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
		{
			Col1 = new CardCellModel
			{
				Text = "⚖ Difference",
				ColumnSpan = 2,
				Font = fontBold,
				ForeColor = Color.Teal
			},
			Col2 = new CardCellModel
			{
				Text = difference.ToString("N0"),
				ColumnSpan = 2,
				Font = new Font("Segoe UI", 10f, FontStyle.Bold),
				ForeColor = differenceColor
			},
			RowHeight = 26
		});
		if (Math.Abs(difference) >= 500.0)
		{
			ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
			{
				Col1 = new CardCellModel
				{
					Text = "\ud83d\udd34 CRITICAL DIFFERENCE",
					ColumnSpan = 4,
					Font = fontBold,
					ForeColor = Color.White,
					BackColor = Color.DarkRed,
					Padding = new Padding(4)
				},
				RowHeight = 28
			});
			card.Height += 40;
		}
		card.FlpMain.Controls.Add(new Panel
		{
			Height = 1,
			Width = 330,
			BackColor = Color.LightGray,
			Margin = new Padding(3, 5, 3, 5)
		});
		ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
		{
			Col1 = new CardCellModel
			{
				Text = "Entry",
				Font = fontBold,
				ForeColor = Color.Gray
			},
			Col2 = new CardCellModel
			{
				Text = Conversion.ToString(row["EntryUserName"]) + " | " + string.Format("{0:dd-MMM-yyyy}", Conversion.ToDateTime(row["EntryDate"])),
				ColumnSpan = 3,
				Font = fontNormal,
				ForeColor = Color.Black
			},
			RowHeight = 22
		});
		string modifyText = "";
		if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row["ModifyDate"])))
		{
			modifyText = Conversion.ToString(row["ModifyUserName"]) + " | " + string.Format("{0:dd-MMM-yyyy}", Conversion.ToDateTime(row["ModifyDate"]));
		}
		ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
		{
			Col1 = new CardCellModel
			{
				Text = "Modify",
				Font = fontBold,
				ForeColor = Color.Gray
			},
			Col2 = new CardCellModel
			{
				Text = modifyText,
				ColumnSpan = 3,
				Font = fontNormal,
				ForeColor = Color.Black
			},
			RowHeight = 22
		});
		int docId = Conversion.ToInt(row["Id"]);
		ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
		{
			Col1 = ApprovalFormHelper.DefaultButtonModel("✔ Approve", Color.DarkGreen, delegate
			{
				ProcessApproval(new List<ApprovalSideBarMainCard> { card });
			}, 2),
			Col3 = ApprovalFormHelper.DefaultButtonModel(string.Format("\ud83d\udcce Attachments ({0})", Conversion.ToInt(row["NoOfAttachments"])), Color.DarkSlateBlue, delegate
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(docId, DocumentTypeId);
			}, 2),
			RowHeight = 34
		});
		ApprovalFormHelper.SetupCardTableLayout(card.FlpMain, 95, 120, 70, 70);
		card.FlpMain.ResumeLayout();
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
			ApprovalSideBarMainCard mainCard = sender as ApprovalSideBarMainCard;
			if (mainCard == null && sender is Control ctrl)
			{
				_ = ctrl.Parent;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			ApprovalList card = ApprovalFormHelper.ProcessApproval(selectedCards, UserAccount, flagUnApprove, DocumentTypeId, txtApprovalRemarks, "Commission Agent Trade Bill", SpecialApproval_ReasonId);
			if (card != null)
			{
				InvCommAgentTradeBill.UpdateSpecialApprovalStatus(card);
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
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Cmagt.Approvals.frmSpecialApprovalCommissionTradeBill));
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
		base.SuspendLayout();
		this.panel1.Controls.Add(this.FlpSideBar);
		this.panel1.Controls.Add(this.TotalRecordPanel);
		this.panel1.Controls.Add(this.GBFilters);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 24);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(984, 687);
		this.panel1.TabIndex = 0;
		this.FlpSideBar.Dock = System.Windows.Forms.DockStyle.Fill;
		this.FlpSideBar.Location = new System.Drawing.Point(0, 124);
		this.FlpSideBar.Name = "FlpSideBar";
		this.FlpSideBar.Size = new System.Drawing.Size(984, 527);
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
		this.TotalRecordPanel.Size = new System.Drawing.Size(984, 36);
		this.TotalRecordPanel.TabIndex = 2;
		this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(425, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(112, 20);
		this.label5.TabIndex = 1;
		this.label5.Text = "Total Records: ";
		this.TotalHeaderRecords.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
		this.TotalHeaderRecords.AutoSize = true;
		this.TotalHeaderRecords.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalHeaderRecords.Location = new System.Drawing.Point(536, 6);
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
		this.BtnGoToLast.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnGoToLast.BackColor = System.Drawing.Color.Transparent;
		this.BtnGoToLast.FlatAppearance.BorderSize = 0;
		this.BtnGoToLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnGoToLast.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnGoToLast.ForeColor = System.Drawing.Color.White;
		this.BtnGoToLast.Image = Architecture.WinApp.Properties.Resources.download_removebg_previewd;
		this.BtnGoToLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnGoToLast.Location = new System.Drawing.Point(900, 1);
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
		this.BtnLoadNextRecord.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnLoadNextRecord.BackColor = System.Drawing.Color.Transparent;
		this.BtnLoadNextRecord.FlatAppearance.BorderSize = 0;
		this.BtnLoadNextRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadNextRecord.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadNextRecord.ForeColor = System.Drawing.Color.White;
		this.BtnLoadNextRecord.Image = Architecture.WinApp.Properties.Resources.arrow_right_line1;
		this.BtnLoadNextRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnLoadNextRecord.Location = new System.Drawing.Point(940, 1);
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
		this.GBFilters.Size = new System.Drawing.Size(984, 96);
		this.GBFilters.TabIndex = 0;
		this.GBFilters.TabStop = false;
		this.GBFilters.Text = "Filters";
		this.BtnSorting.Image = Architecture.WinApp.Properties.Resources.SortDesc;
		this.BtnSorting.Location = new System.Drawing.Point(944, 37);
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
		((System.Windows.Forms.Control)(object)this.CmbSupplierNameFilter).Location = new System.Drawing.Point(459, 10);
		((System.Windows.Forms.Control)(object)this.CmbSupplierNameFilter).Name = "CmbSupplierNameFilter";
		((System.Windows.Forms.Control)(object)this.CmbSupplierNameFilter).Size = new System.Drawing.Size(230, 26);
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
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameFilter).Location = new System.Drawing.Point(459, 37);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameFilter).Name = "CmbBuyerNameFilter";
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameFilter).Size = new System.Drawing.Size(230, 26);
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
		((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter).Location = new System.Drawing.Point(98, 64);
		((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter).Name = "CmbTradingAccountFilter";
		((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbTradingAccountFilter).TabIndex = 4;
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(98, 39);
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
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryFilter).Location = new System.Drawing.Point(459, 64);
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryFilter).Name = "CmbParentCategoryFilter";
		((System.Windows.Forms.Control)(object)this.CmbParentCategoryFilter).Size = new System.Drawing.Size(230, 26);
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
		((System.Windows.Forms.Control)(object)this.CmbItemNameFilter).Location = new System.Drawing.Point(757, 10);
		((System.Windows.Forms.Control)(object)this.CmbItemNameFilter).Name = "CmbItemNameFilter";
		((System.Windows.Forms.Control)(object)this.CmbItemNameFilter).Size = new System.Drawing.Size(220, 26);
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
		this.txtDocNoTo.Location = new System.Drawing.Point(230, 39);
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
		this.label8.Location = new System.Drawing.Point(366, 43);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 15);
		this.label8.TabIndex = 636;
		this.label8.Text = "Buyer Name";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(366, 16);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 635;
		this.label7.Text = "Supplier Name";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(5, 70);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(95, 15);
		this.label14.TabIndex = 636;
		this.label14.Text = "Trading Account";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(366, 70);
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
		this.btnshow.Location = new System.Drawing.Point(886, 40);
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
		this.label10.Location = new System.Drawing.Point(5, 43);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(88, 15);
		this.label10.TabIndex = 144;
		this.label10.Text = "From && To Doc";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(692, 16);
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
		this.panel2.Size = new System.Drawing.Size(984, 28);
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
		this.toolStrip3.Size = new System.Drawing.Size(984, 27);
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
		this.panel3.Size = new System.Drawing.Size(984, 24);
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 711);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmSpecialApprovalCommissionTradeBill";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmSpecialApprovalCommissionTradeBill_Load);
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
		base.ResumeLayout(false);
	}
}

}
