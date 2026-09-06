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
using Architecture.WinApp.Service.lgstcm;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.lgstcm.Approvals;

public class frmApprovalLogisticAgreement : Form
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

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private bool FormInitialized;

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

	internal UltraCombo CmbServiceTypeHistory;

	private Label label4;

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

	private Label txtServiceType;

	private Label label13;

	private Label txtExchangeRate;

	private Label label29;

	private Label txtTcyCurrency;

	private Label label30;

	internal GridEX grdCustomerDetail;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel PanelMainSelectedNetPreviousDocData;

	private Button BtnShowPreviousRecord;

	private Button BtnShowNextRecord;

	private Label label19;

	private Label txtServiceProvider;

	private Label txtBrokerAgentName;

	private Button BtnApproveThis;

	private Label label16;

	private Label txtRemarks;

	private Label label11;

	private Label label9;

	private Label label12;

	private Label txtEffectiveDateTo;

	private Label txtEffectiveDateFrom;

	public frmApprovalLogisticAgreement(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void frmApprovalLogisticAgreement_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(1100, 740);
	}

	private void InitializeComponentCustom()
	{
		try
		{
			dtCustomerDetail.Columns.Add("Id", typeof(int));
			dtCustomerDetail.Columns.Add("PartyRefDocNo", typeof(string));
			dtCustomerDetail.Columns.Add("PartyRefDocDate", typeof(DateTime));
			dtCustomerDetail.Columns.Add("ItemId", typeof(int));
			dtCustomerDetail.Columns.Add("ItemCode");
			dtCustomerDetail.Columns.Add("ItemName");
			dtCustomerDetail.Columns.Add("TransactionCurrencyId", typeof(int));
			dtCustomerDetail.Columns.Add("TransactionCurrency", typeof(string));
			dtCustomerDetail.Columns.Add("ServiceRate", typeof(decimal));
			dtCustomerDetail.Columns.Add("RateBaseId", typeof(int));
			dtCustomerDetail.Columns.Add("RateBase", typeof(string));
			dtCustomerDetail.Columns.Add("RateBaseEquivalent", typeof(decimal));
			dtCustomerDetail.Columns.Add("TcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("LcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("Remarks");
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
						labelFormHeading.Text = "Pending 'Logistics Agreement' For UnApproval";
						btnApproved.Text = "UnApprove";
					}
					else
					{
						labelFormHeading.Text = "Pending 'Logistics Agreement' For Approval";
						btnApproved.Text = "Approve";
					}
					if (DocumentTypeId > 0)
					{
						GenerateCardInSideBarLayout();
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
			return AgreementHeader.GetDataForDropDownFromAgreementHeader(new ReportsParameters
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
			DataTable dtST = dtCust.Clone();
			foreach (DataRow dr in dt.Rows)
			{
				switch (Conversion.ToString(dr["Activity"]))
				{
				case "BrokerAgent":
					dtCust.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "ServiceProvider":
					dtSP.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "ServiceItem":
					dtItem.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "ServiceType":
					dtST.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBrokerAgentName, dtCust, "Id", "Name", "Broker Agent Name", AllColumns: true, Conversion.ToInt(CmbBrokerAgentName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbServiceProvider, dtSP, "Id", "Name", "Service Provider", AllColumns: true, Conversion.ToInt(CmbServiceProvider.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbServiceTypeHistory, dtST, "Id", "Name", "Service Type", AllColumns: true, Conversion.ToInt(CmbServiceTypeHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			((Control)(object)CmbServiceTypeHistory).Text = string.Empty;
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
			GenerateCardInSideBarLayout();
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
			grn.FromDate = from;
			grn.ToDate = to;
			grn.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text.Trim());
			grn.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text.Trim());
			grn.BrokerAgentId = Conversion.ToInt(CmbBrokerAgentName.Value);
			grn.ServiceProviderId = Conversion.ToInt(CmbServiceProvider.Value);
			grn.ItemId = Conversion.ToInt(CmbItemName.Value);
			grn.TypeId = Conversion.ToInt(CmbServiceTypeHistory.Value);
			grn.IsApproved = flagUnApprove;
			dtGridDbCall = AgreementHeader.AgreementHeader_DataForApproval(grn);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCardInSideBarLayout()
	{
		try
		{
			FlpSideBar.SuspendLayout();
			FlpSideBar.Controls.Clear();
			FlpSideBar.AutoScroll = true;
			List<DataRow> distinctRows = (from r in dtGridDbCall.AsEnumerable()
				group r by r.Field<int>("AgreementHeaderId") into g
				select g.First()).ToList();
			if (totalRecords == 0)
			{
				totalRecords = distinctRows.Count;
			}
			int skip = (currentPage - 1) * pageSize;
			List<DataRow> pageRecords = (from r in distinctRows.AsEnumerable().Skip(skip).Take(pageSize)
				group r by r.Field<int>("AgreementHeaderId") into g
				select g.First()).ToList();
			foreach (DataRow row in pageRecords)
			{
				int docId = Conversion.ToInt(row["AgreementHeaderId"]);
				string docNo = Conversion.ToString(row["documentNo"]);
				Conversion.ToString(row["ServiceType"]);
				string entryUser = Conversion.ToString(row["EntryUserName"]);
				string modifyUser = Conversion.ToString(row["ModifyUserName"]);
				Conversion.ToString(row["ServiceProvider"]);
				string BillToPartyName = Conversion.ToString(row["BrokerAgentName"]);
				int NoOfAttachments = Conversion.ToInt(row["NoOfAttachments"]);
				bool num = Conversion.ToBool(row["isActive"]);
				DateTime docDate = Conversion.ToDateTime(row["documentDate"]);
				DateTime entryDate = Conversion.ToDateTime(row["EntryDate"]);
				DateTime modifyDate = Conversion.ToDateTime(row["ModifyDate"]);
				ApprovalSideBarMainCard card = new ApprovalSideBarMainCard
				{
					DocumentId = docId
				};
				card.chkSelectionWithMainText.Text = BillToPartyName;
				card.FlpMain.SuspendLayout();
				card.FlpMain.Controls.Clear();
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = new CardCellModel
					{
						Text = "Doc No:",
						ForeColor = Color.DarkCyan,
						Font = new Font("Segoe UI", 9f),
						Padding = new Padding(0)
					},
					Col2 = new CardCellModel
					{
						Text = docNo,
						CellType = CardCellType.Link,
						Bold = false,
						Font = new Font("Segoe UI", 10f),
						ForeColor = Color.RoyalBlue,
						OnClick = delegate
						{
							CrystalReportPrint_Helper.PurchaseOrderHeader_Slip_1303(docId);
						},
						Padding = new Padding(0)
					},
					Col3 = new CardCellModel
					{
						Text = "Doc Date:",
						ForeColor = Color.DarkCyan,
						Font = new Font("Segoe UI", 9f),
						Padding = new Padding(0)
					},
					Col4 = new CardCellModel
					{
						Text = docDate.ToString("dd-MMM-yyyy"),
						Font = new Font("Segoe UI", 9f),
						ForeColor = Color.Black,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = new CardCellModel
					{
						Text = "Entry User:",
						ForeColor = Color.DarkCyan,
						Font = new Font("Segoe UI", 9f),
						Padding = new Padding(0)
					},
					Col2 = new CardCellModel
					{
						Text = entryUser,
						CellType = CardCellType.Label,
						Bold = false,
						Font = new Font("Segoe UI", 9f),
						ForeColor = Color.Black,
						Padding = new Padding(0)
					},
					Col3 = new CardCellModel
					{
						Text = "Entry Date:",
						ForeColor = Color.DarkCyan,
						Font = new Font("Segoe UI", 9f),
						Padding = new Padding(0)
					},
					Col4 = new CardCellModel
					{
						Text = entryDate.ToString("dd-MMM-yyyy"),
						Font = new Font("Segoe UI", 9f),
						ForeColor = Color.Black,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = new CardCellModel
					{
						Text = "Modify User:",
						ForeColor = Color.DarkCyan,
						Font = new Font("Segoe UI", 9f),
						Padding = new Padding(0)
					},
					Col2 = new CardCellModel
					{
						Text = modifyUser,
						CellType = CardCellType.Label,
						Bold = false,
						Font = new Font("Segoe UI", 9f),
						ForeColor = Color.Black,
						Padding = new Padding(0)
					},
					Col3 = new CardCellModel
					{
						Text = "Modify Date:",
						ForeColor = Color.DarkCyan,
						Font = new Font("Segoe UI", 9f),
						Padding = new Padding(0)
					},
					Col4 = new CardCellModel
					{
						Text = (Conversion.CheckDateTimeNull(modifyDate) ? "" : modifyDate.ToString("dd-MMM-yyyy")),
						Font = new Font("Segoe UI", 9f),
						ForeColor = Color.Black,
						Padding = new Padding(0)
					},
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = new CardCellModel
					{
						Text = "Approve This   ✔",
						CellType = CardCellType.Button,
						Bold = true,
						ForeColor = Color.DarkGreen,
						Height = 28,
						ColumnSpan = 2,
						TextAlign = ContentAlignment.MiddleCenter,
						OnClick = delegate
						{
							ProcessApproval(new List<ApprovalSideBarMainCard> { card });
						},
						Padding = new Padding(0, 0, 1, 0)
					},
					Col3 = new CardCellModel
					{
						Text = $"Attachments ({NoOfAttachments})",
						CellType = CardCellType.Button,
						Bold = true,
						ForeColor = Color.DarkSlateBlue,
						Height = 28,
						ColumnSpan = 2,
						TextAlign = ContentAlignment.MiddleCenter,
						OnClick = delegate
						{
							CommonServices.GetNoofAttachmentsByRefDocumentTypeID(docId, DocumentTypeId);
						},
						Padding = new Padding(1, 0, 1, 0)
					},
					RowHeight = 25
				});
				ApprovalFormHelper.SetupCardTableLayout(card.FlpMain, 76, 100, 76, 80);
				card.FlpMain.ResumeLayout();
				card.UserControlClicked += HeaderCard_Click;
				card.Width = 350;
				card.Height = 140;
				FlpSideBar.Controls.Add(card);
				if (!num)
				{
					FlpSideBar.BackColor = Color.Gainsboro;
				}
				else
				{
					FlpSideBar.BackColor = Color.White;
				}
			}
			TotalHeaderRecords.Text = $"{(currentPage - 1) * pageSize + pageRecords.Count} / {totalRecords}";
			BtnLoadPreviousRecord.Enabled = currentPage > 1;
			BtnLoadNextRecord.Enabled = currentPage * pageSize < totalRecords;
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
			GenerateCardInSideBarLayout();
		}
	}

	private void BtnLoadNextRecord_Click(object sender, EventArgs e)
	{
		if (currentPage * pageSize < totalRecords)
		{
			currentPage++;
			GenerateCardInSideBarLayout();
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

	public void ReadById(int Id)
	{
		try
		{
			FromReset();
			RecId = Id;
			AgreementHeader Or = AgreementHeader.ReadById(RecId);
			if (Or == null || Or.AgreementDetailList == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			txtDocDate.Text = Or.DocumentDate.ToString("d");
			txtDocNo.Text = Conversion.ToString((object)Or.DocumentNo);
			txtBrokerAgentName.Text = Conversion.ToString((object)Or.BrokerAgentName);
			txtServiceProvider.Text = Conversion.ToString((object)Or.ServiceProviderName);
			txtServiceType.Text = Conversion.ToString((object)Or.ServiceType);
			txtRemarks.Text = Conversion.ToString((object)Or.RemarksHeader);
			txtTcyCurrency.Text = Conversion.ToString((object)Or.TransactionCurrencyCode);
			txtExchangeRate.Text = Or.transactionExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			txtEffectiveDateFrom.Text = Conversion.ToString((object)Or.effectiveDateFrom);
			txtEffectiveDateTo.Text = Conversion.ToString((object)Or.effectiveDateTo);
			if (!Or.isActive)
			{
				PanelHeaderInfo.BackColor = Color.Gainsboro;
			}
			else
			{
				PanelHeaderInfo.BackColor = Color.White;
			}
			dtCustomerDetail.Rows.Clear();
			FillDetailFromListCommonForReadById(Or, dtCustomerDetail);
			grdCustomerDetail.DataSource = dtCustomerDetail;
			grdCustomerDetail.RetrieveStructure();
			grdCustomerDetailSetting();
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
			txtBrokerAgentName.Text = "None";
			txtServiceProvider.Text = "None";
			txtServiceType.Text = "None";
			txtRemarks.Text = "None";
			txtTcyCurrency.Text = "None";
			txtExchangeRate.Text = "0";
			txtEffectiveDateFrom.Text = "None";
			txtEffectiveDateTo.Text = "None";
			dtCustomerDetail.Clear();
			grdCustomerDetail.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetailSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ItemId", "TransactionCurrencyId", "RateBaseId", "ItemCode", "RateBaseEquivalent" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdCustomerDetail, 2, 3, hiddenColumns);
			grdCustomerDetail.RootTable.Columns["PartyRefDocNo"].Width = 120;
			grdCustomerDetail.RootTable.Columns["PartyRefDocDate"].Width = 100;
			grdCustomerDetail.RootTable.Columns["ItemName"].Width = 160;
			grdCustomerDetail.RootTable.Columns["ItemCode"].Width = 100;
			grdCustomerDetail.RootTable.Columns["TransactionCurrency"].Width = 80;
			grdCustomerDetail.RootTable.Columns["ServiceRate"].Width = 80;
			grdCustomerDetail.RootTable.Columns["RateBase"].Width = 120;
			grdCustomerDetail.RootTable.Columns["RateBaseEquivalent"].Width = 90;
			grdCustomerDetail.RootTable.Columns["TcyAmount"].Width = 90;
			grdCustomerDetail.RootTable.Columns["LcyAmount"].Width = 90;
			grdCustomerDetail.RootTable.Columns["Remarks"].Width = 180;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(AgreementHeader invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.AgreementDetailList : null) == null || invGrn.AgreementDetailList.Count == 0)
		{
			return;
		}
		foreach (AgreementDetail detail in invGrn.AgreementDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.agreementDetailId;
			dr["PartyRefDocNo"] = detail.partyRefDocNo;
			dr["PartyRefDocDate"] = detail.partyRefDocDate;
			dr["ItemId"] = detail.serviceItemId;
			dr["ItemCode"] = detail.ItemCode;
			dr["ItemName"] = detail.ItemName;
			dr["TransactionCurrencyId"] = detail.transactionCurrencyId;
			dr["TransactionCurrency"] = detail.TransactionCurrencyCode;
			dr["ServiceRate"] = detail.serviceRate;
			dr["RateBaseId"] = detail.serviceRateBaseUomId;
			dr["RateBase"] = detail.RateBaseUom;
			dr["RateBaseEquivalent"] = detail.RateBaseUomEquivalent;
			dr["TcyAmount"] = detail.tcyAmount;
			dr["LcyAmount"] = detail.lcyAmount;
			dr["Remarks"] = detail.Remarks;
			dt.Rows.Add(dr);
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
			FormHelper.CanOpenForm("frmLogisticAgreement", delegate
			{
				frmLogisticAgreement frmLogisticAgreement = new frmLogisticAgreement(UserAccount);
				frmLogisticAgreement.Show();
				frmLogisticAgreement.ReadById(RecId);
				frmLogisticAgreement.BringToFront();
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
				where Conversion.ToInt(row["documentNo"]) == nextDocNo
				select row).FirstOrDefault();
			if (nextRecord != null)
			{
				int nextDocId = Conversion.ToInt(nextRecord["AgreementHeaderId"]);
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
					where Conversion.ToInt(row["documentNo"]) == PreviousDocNo
					select row).FirstOrDefault();
				if (previousRecord != null)
				{
					int PreviousDocId = Conversion.ToInt(previousRecord["AgreementHeaderId"]);
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
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		string action = (flagUnApprove ? "UnApprove" : "Approve");
		string reqType = (flagUnApprove ? "UP" : "AP");
		try
		{
			if (!selectedCards.Any())
			{
				MessageBox.Show("Please select at least one record to " + action + ".", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			string remarks = txtApprovalRemarks?.Text?.Trim();
			if (string.IsNullOrWhiteSpace(remarks))
			{
				txtApprovalRemarks.Focus();
				MessageBox.Show("Approval remarks are required...", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				if (MessageBox.Show("Are you sure to " + action + " selected 'Logistics Agreement'(s)?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ApprovalList approvalWrapper = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				foreach (ApprovalSideBarMainCard card in selectedCards)
				{
					approvalWrapper.AllApprovalLists.Add(new ApprovalList
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = DocumentTypeId,
						Id = card.DocumentId,
						PostDate = DateTime.Now,
						EntryUser = UserAccount.ID,
						ReqType = reqType,
						Comments = remarks
					});
				}
				AgreementHeader.UpdateApprovalStatus(approvalWrapper);
				MessageBox.Show("Record(s) " + action + "d successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				MainGridDbCall();
				GenerateCardInSideBarLayout();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while " + action + " records: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				MessageBox.Show("Approval remarks are required for: " + txtServiceType.Text, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
			AgreementHeader.UpdateApprovalStatus(approvalWrapper);
			MessageBox.Show("Record(s) " + action + "d successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			MainGridDbCall();
			GenerateCardInSideBarLayout();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error while " + action + " records: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			throw new Exception(ex.Message);
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
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		GridEXLayout grdCustomerDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.lgstcm.Approvals.frmApprovalLogisticAgreement));
		this.panel1 = new System.Windows.Forms.Panel();
		this.FlpSideBar = new System.Windows.Forms.FlowLayoutPanel();
		this.TotalRecordPanel = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.TotalHeaderRecords = new System.Windows.Forms.Label();
		this.BtnLoadPreviousRecord = new System.Windows.Forms.Button();
		this.BtnLoadNextRecord = new System.Windows.Forms.Button();
		this.GBFilters = new System.Windows.Forms.GroupBox();
		this.CmbServiceProvider = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbBrokerAgentName = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbServiceTypeHistory = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnApproved = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShowHideFilter = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
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
		this.label9 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtEffectiveDateTo = new System.Windows.Forms.Label();
		this.txtEffectiveDateFrom = new System.Windows.Forms.Label();
		this.txtTcyCurrency = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtServiceProvider = new System.Windows.Forms.Label();
		this.txtBrokerAgentName = new System.Windows.Forms.Label();
		this.txtServiceType = new System.Windows.Forms.Label();
		this.PanelMainSelectedNetPreviousDocData = new System.Windows.Forms.Panel();
		this.BtnApproveThis = new System.Windows.Forms.Button();
		this.BtnShowPreviousRecord = new System.Windows.Forms.Button();
		this.BtnShowNextRecord = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.TotalRecordPanel.SuspendLayout();
		this.GBFilters.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgentName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel3.SuspendLayout();
		this.PanelMainSelectedDocData.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.PanelHeaderInfo.SuspendLayout();
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
		this.FlpSideBar.Location = new System.Drawing.Point(0, 203);
		this.FlpSideBar.Name = "FlpSideBar";
		this.FlpSideBar.Size = new System.Drawing.Size(380, 457);
		this.FlpSideBar.TabIndex = 4;
		this.TotalRecordPanel.BackColor = System.Drawing.Color.White;
		this.TotalRecordPanel.Controls.Add(this.label5);
		this.TotalRecordPanel.Controls.Add(this.TotalHeaderRecords);
		this.TotalRecordPanel.Controls.Add(this.BtnLoadPreviousRecord);
		this.TotalRecordPanel.Controls.Add(this.BtnLoadNextRecord);
		this.TotalRecordPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.TotalRecordPanel.Location = new System.Drawing.Point(0, 660);
		this.TotalRecordPanel.Name = "TotalRecordPanel";
		this.TotalRecordPanel.Size = new System.Drawing.Size(380, 27);
		this.TotalRecordPanel.TabIndex = 8;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(96, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(95, 17);
		this.label5.TabIndex = 1;
		this.label5.Text = "Total Records: ";
		this.TotalHeaderRecords.AutoSize = true;
		this.TotalHeaderRecords.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalHeaderRecords.Location = new System.Drawing.Point(188, 5);
		this.TotalHeaderRecords.Name = "TotalHeaderRecords";
		this.TotalHeaderRecords.Size = new System.Drawing.Size(15, 17);
		this.TotalHeaderRecords.TabIndex = 2;
		this.TotalHeaderRecords.Text = "0";
		this.BtnLoadPreviousRecord.BackColor = System.Drawing.Color.Transparent;
		this.BtnLoadPreviousRecord.FlatAppearance.BorderSize = 0;
		this.BtnLoadPreviousRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadPreviousRecord.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadPreviousRecord.ForeColor = System.Drawing.Color.White;
		this.BtnLoadPreviousRecord.Image = Architecture.WinApp.Properties.Resources.arrow_left_line;
		this.BtnLoadPreviousRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnLoadPreviousRecord.Location = new System.Drawing.Point(7, 2);
		this.BtnLoadPreviousRecord.Margin = new System.Windows.Forms.Padding(2);
		this.BtnLoadPreviousRecord.Name = "BtnLoadPreviousRecord";
		this.BtnLoadPreviousRecord.Size = new System.Drawing.Size(27, 21);
		this.BtnLoadPreviousRecord.TabIndex = 8;
		this.BtnLoadPreviousRecord.UseVisualStyleBackColor = false;
		this.BtnLoadPreviousRecord.Click += new System.EventHandler(BtnLoadPreviousRecord_Click);
		this.BtnLoadNextRecord.BackColor = System.Drawing.Color.Transparent;
		this.BtnLoadNextRecord.FlatAppearance.BorderSize = 0;
		this.BtnLoadNextRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadNextRecord.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadNextRecord.ForeColor = System.Drawing.Color.White;
		this.BtnLoadNextRecord.Image = Architecture.WinApp.Properties.Resources.arrow_right_line1;
		this.BtnLoadNextRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnLoadNextRecord.Location = new System.Drawing.Point(334, 2);
		this.BtnLoadNextRecord.Margin = new System.Windows.Forms.Padding(2);
		this.BtnLoadNextRecord.Name = "BtnLoadNextRecord";
		this.BtnLoadNextRecord.Size = new System.Drawing.Size(27, 21);
		this.BtnLoadNextRecord.TabIndex = 8;
		this.BtnLoadNextRecord.UseVisualStyleBackColor = false;
		this.BtnLoadNextRecord.Click += new System.EventHandler(BtnLoadNextRecord_Click);
		this.GBFilters.BackColor = System.Drawing.Color.White;
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceProvider);
		this.GBFilters.Controls.Add(this.label7);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName);
		this.GBFilters.Controls.Add(this.label14);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory);
		this.GBFilters.Controls.Add(this.label4);
		this.GBFilters.Controls.Add(this.txtDocNoFrom);
		this.GBFilters.Controls.Add(this.btnshow);
		this.GBFilters.Controls.Add(this.label10);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.GBFilters.Controls.Add(this.label1);
		this.GBFilters.Controls.Add(this.GRNfromdate);
		this.GBFilters.Controls.Add(this.label27);
		this.GBFilters.Controls.Add(this.txtDocNoTo);
		this.GBFilters.Controls.Add(this.GRNToDate);
		this.GBFilters.Dock = System.Windows.Forms.DockStyle.Top;
		this.GBFilters.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.GBFilters.Location = new System.Drawing.Point(0, 28);
		this.GBFilters.Name = "GBFilters";
		this.GBFilters.Size = new System.Drawing.Size(380, 175);
		this.GBFilters.TabIndex = 0;
		this.GBFilters.TabStop = false;
		this.GBFilters.Text = "Filters";
		this.CmbServiceProvider.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceProvider.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceProvider.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceProvider.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance117;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceProvider.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Location = new System.Drawing.Point(66, 88);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Name = "CmbServiceProvider";
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Size = new System.Drawing.Size(293, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).TabIndex = 633;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 90);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(51, 30);
		this.label7.TabIndex = 635;
		this.label7.Text = "Service \r\nProvider";
		this.CmbBrokerAgentName.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokerAgentName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokerAgentName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokerAgentName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokerAgentName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Location = new System.Drawing.Point(66, 60);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Name = "CmbBrokerAgentName";
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Size = new System.Drawing.Size(293, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).TabIndex = 632;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(5, 56);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(52, 30);
		this.label14.TabIndex = 636;
		this.label14.Text = "Broker / \r\nAgent";
		this.CmbServiceTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance146;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance142).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Location = new System.Drawing.Point(66, 144);
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Name = "CmbServiceTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceTypeHistory).TabIndex = 181;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 142);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(45, 30);
		this.label4.TabIndex = 184;
		this.label4.Text = "Service\r\nType";
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(66, 35);
		this.txtDocNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.ShortcutsEnabled = false;
		this.txtDocNoFrom.Size = new System.Drawing.Size(146, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDocNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoFrom_KeyPress);
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(304, 146);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 23);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 39);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(60, 15);
		this.label10.TabIndex = 144;
		this.label10.Text = "From Doc";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(66, 116);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(293, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 7;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 122);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(32, 15);
		this.label1.TabIndex = 121;
		this.label1.Text = "Item";
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(66, 12);
		this.GRNfromdate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(146, 22);
		this.GRNfromdate.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 16);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "From Date";
		this.txtDocNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(213, 35);
		this.txtDocNoTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.ShortcutsEnabled = false;
		this.txtDocNoTo.Size = new System.Drawing.Size(146, 23);
		this.txtDocNoTo.TabIndex = 3;
		this.txtDocNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDocNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrnNoTo_KeyPress);
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(213, 12);
		this.GRNToDate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(146, 22);
		this.GRNToDate.TabIndex = 1;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(380, 28);
		this.panel2.TabIndex = 1;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.btnRefresh, this.btnApproved, this.btnShortCutKeys, this.toolStripSeparator2, this.BtnShowHideFilter, this.toolStripSeparator1 });
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
		this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(24, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.ToolTipText = "Click To Refresh Filters";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnApproved.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnApproved.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApproved.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(24, 24);
		this.btnApproved.Text = "Approve";
		this.btnApproved.ToolTipText = "Click to Approve Selected Records";
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
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
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.labelFormHeading);
		this.panel3.Controls.Add(this.txtApprovalRemarks);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1084, 24);
		this.panel3.TabIndex = 52;
		this.labelFormHeading.AutoSize = true;
		this.labelFormHeading.BackColor = System.Drawing.Color.Teal;
		this.labelFormHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelFormHeading.ForeColor = System.Drawing.Color.White;
		this.labelFormHeading.Location = new System.Drawing.Point(3, 2);
		this.labelFormHeading.Name = "labelFormHeading";
		this.labelFormHeading.Size = new System.Drawing.Size(377, 19);
		this.labelFormHeading.TabIndex = 4;
		this.labelFormHeading.Text = "Pending 'Logistic Agreement' For UnApproval";
		this.txtApprovalRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovalRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovalRemarks.Location = new System.Drawing.Point(572, 1);
		this.txtApprovalRemarks.Margin = new System.Windows.Forms.Padding(4);
		this.txtApprovalRemarks.Name = "txtApprovalRemarks";
		this.txtApprovalRemarks.ShortcutsEnabled = false;
		this.txtApprovalRemarks.Size = new System.Drawing.Size(510, 23);
		this.txtApprovalRemarks.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label2.Location = new System.Drawing.Point(445, 4);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(120, 17);
		this.label2.TabIndex = 121;
		this.label2.Text = "Approval Remarks";
		this.PanelMainSelectedDocData.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerDetail);
		this.PanelMainSelectedDocData.Controls.Add(this.panel14);
		this.PanelMainSelectedDocData.Controls.Add(this.PanelHeaderInfo);
		this.PanelMainSelectedDocData.Controls.Add(this.PanelMainSelectedNetPreviousDocData);
		this.PanelMainSelectedDocData.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainSelectedDocData.Location = new System.Drawing.Point(380, 24);
		this.PanelMainSelectedDocData.Name = "PanelMainSelectedDocData";
		this.PanelMainSelectedDocData.Size = new System.Drawing.Size(704, 687);
		this.PanelMainSelectedDocData.TabIndex = 53;
		this.grdCustomerDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdCustomerDetail).BackColor = System.Drawing.Color.White;
		grdCustomerDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerDetail.DesignTimeLayout = grdCustomerDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Location = new System.Drawing.Point(0, 202);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Name = "grdCustomerDetail";
		((ScrollableBase)this.grdCustomerDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Size = new System.Drawing.Size(704, 485);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).TabIndex = 132;
		this.grdCustomerDetail.TotalRow = (InheritableBoolean)1;
		this.grdCustomerDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerDetail).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Controls.Add(this.label19);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 181);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(704, 21);
		this.panel14.TabIndex = 133;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(658, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdCustomerDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 20);
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
		this.PanelHeaderInfo.Controls.Add(this.label9);
		this.PanelHeaderInfo.Controls.Add(this.label12);
		this.PanelHeaderInfo.Controls.Add(this.label11);
		this.PanelHeaderInfo.Controls.Add(this.label16);
		this.PanelHeaderInfo.Controls.Add(this.txtRemarks);
		this.PanelHeaderInfo.Controls.Add(this.txtExchangeRate);
		this.PanelHeaderInfo.Controls.Add(this.label29);
		this.PanelHeaderInfo.Controls.Add(this.txtEffectiveDateTo);
		this.PanelHeaderInfo.Controls.Add(this.txtEffectiveDateFrom);
		this.PanelHeaderInfo.Controls.Add(this.txtTcyCurrency);
		this.PanelHeaderInfo.Controls.Add(this.label30);
		this.PanelHeaderInfo.Controls.Add(this.label13);
		this.PanelHeaderInfo.Controls.Add(this.txtServiceProvider);
		this.PanelHeaderInfo.Controls.Add(this.txtBrokerAgentName);
		this.PanelHeaderInfo.Controls.Add(this.txtServiceType);
		this.PanelHeaderInfo.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHeaderInfo.Location = new System.Drawing.Point(0, 31);
		this.PanelHeaderInfo.Name = "PanelHeaderInfo";
		this.PanelHeaderInfo.Size = new System.Drawing.Size(704, 150);
		this.PanelHeaderInfo.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.Teal;
		this.label9.Location = new System.Drawing.Point(424, 97);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(104, 15);
		this.label9.TabIndex = 194;
		this.label9.Text = "Effective Date To";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.Teal;
		this.label12.Location = new System.Drawing.Point(424, 76);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(120, 15);
		this.label12.TabIndex = 195;
		this.label12.Text = "Effective Date From";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 11f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.Teal;
		this.label11.Location = new System.Drawing.Point(6, 40);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(109, 36);
		this.label11.TabIndex = 191;
		this.label11.Text = "Broker Agent /\r\nService Provider";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Teal;
		this.label16.Location = new System.Drawing.Point(6, 119);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(67, 19);
		this.label16.TabIndex = 189;
		this.label16.Text = "Remarks";
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtRemarks.Location = new System.Drawing.Point(121, 118);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(591, 20);
		this.txtRemarks.TabIndex = 190;
		this.txtRemarks.Text = "None";
		this.txtRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtExchangeRate.Location = new System.Drawing.Point(544, 55);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(160, 15);
		this.txtExchangeRate.TabIndex = 187;
		this.txtExchangeRate.Text = "0";
		this.txtExchangeRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label29.ForeColor = System.Drawing.Color.Teal;
		this.label29.Location = new System.Drawing.Point(424, 54);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(89, 15);
		this.label29.TabIndex = 187;
		this.label29.Text = "Exchange Rate";
		this.txtEffectiveDateTo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtEffectiveDateTo.Location = new System.Drawing.Point(544, 97);
		this.txtEffectiveDateTo.Name = "txtEffectiveDateTo";
		this.txtEffectiveDateTo.Size = new System.Drawing.Size(160, 15);
		this.txtEffectiveDateTo.TabIndex = 188;
		this.txtEffectiveDateTo.Text = "None";
		this.txtEffectiveDateFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtEffectiveDateFrom.Location = new System.Drawing.Point(544, 76);
		this.txtEffectiveDateFrom.Name = "txtEffectiveDateFrom";
		this.txtEffectiveDateFrom.Size = new System.Drawing.Size(160, 15);
		this.txtEffectiveDateFrom.TabIndex = 188;
		this.txtEffectiveDateFrom.Text = "None";
		this.txtTcyCurrency.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtTcyCurrency.Location = new System.Drawing.Point(544, 34);
		this.txtTcyCurrency.Name = "txtTcyCurrency";
		this.txtTcyCurrency.Size = new System.Drawing.Size(160, 15);
		this.txtTcyCurrency.TabIndex = 188;
		this.txtTcyCurrency.Text = "None";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label30.ForeColor = System.Drawing.Color.Teal;
		this.label30.Location = new System.Drawing.Point(424, 34);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(78, 15);
		this.label30.TabIndex = 188;
		this.label30.Text = "Tcy Currency";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Teal;
		this.label13.Location = new System.Drawing.Point(6, 87);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(103, 19);
		this.label13.TabIndex = 8;
		this.label13.Text = "Service Type :";
		this.txtServiceProvider.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtServiceProvider.Location = new System.Drawing.Point(121, 86);
		this.txtServiceProvider.Name = "txtServiceProvider";
		this.txtServiceProvider.Size = new System.Drawing.Size(285, 20);
		this.txtServiceProvider.TabIndex = 184;
		this.txtServiceProvider.Text = "None";
		this.txtServiceProvider.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtBrokerAgentName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokerAgentName.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtBrokerAgentName.Location = new System.Drawing.Point(121, 4);
		this.txtBrokerAgentName.Name = "txtBrokerAgentName";
		this.txtBrokerAgentName.Size = new System.Drawing.Size(506, 25);
		this.txtBrokerAgentName.TabIndex = 184;
		this.txtBrokerAgentName.Text = "None";
		this.txtBrokerAgentName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtServiceType.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtServiceType.Location = new System.Drawing.Point(121, 48);
		this.txtServiceType.Name = "txtServiceType";
		this.txtServiceType.Size = new System.Drawing.Size(285, 20);
		this.txtServiceType.TabIndex = 184;
		this.txtServiceType.Text = "None";
		this.txtServiceType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.PanelMainSelectedNetPreviousDocData.BackColor = System.Drawing.Color.White;
		this.PanelMainSelectedNetPreviousDocData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnApproveThis);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnShowPreviousRecord);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnShowNextRecord);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.label6);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.txtDocNo);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.txtDocDate);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.label3);
		this.PanelMainSelectedNetPreviousDocData.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelMainSelectedNetPreviousDocData.Location = new System.Drawing.Point(0, 0);
		this.PanelMainSelectedNetPreviousDocData.Name = "PanelMainSelectedNetPreviousDocData";
		this.PanelMainSelectedNetPreviousDocData.Size = new System.Drawing.Size(704, 31);
		this.PanelMainSelectedNetPreviousDocData.TabIndex = 136;
		this.BtnApproveThis.BackColor = System.Drawing.Color.SeaGreen;
		this.BtnApproveThis.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.BtnApproveThis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnApproveThis.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnApproveThis.ForeColor = System.Drawing.Color.White;
		this.BtnApproveThis.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnApproveThis.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnApproveThis.Location = new System.Drawing.Point(79, 2);
		this.BtnApproveThis.Margin = new System.Windows.Forms.Padding(2);
		this.BtnApproveThis.Name = "BtnApproveThis";
		this.BtnApproveThis.Size = new System.Drawing.Size(129, 25);
		this.BtnApproveThis.TabIndex = 10;
		this.BtnApproveThis.Text = "Approve This";
		this.BtnApproveThis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnApproveThis.UseVisualStyleBackColor = false;
		this.BtnApproveThis.Click += new System.EventHandler(BtnApproveThis_Click);
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
		this.label6.Location = new System.Drawing.Point(438, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 26);
		this.label6.TabIndex = 6;
		this.label6.Text = "Doc #";
		this.txtDocNo.AutoSize = true;
		this.txtDocNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.txtDocNo.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.ForeColor = System.Drawing.Color.Blue;
		this.txtDocNo.Location = new System.Drawing.Point(502, 3);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(23, 26);
		this.txtDocNo.TabIndex = 7;
		this.txtDocNo.Text = "0";
		this.txtDocNo.Click += new System.EventHandler(txtDocNo_Click);
		this.txtDocDate.AutoSize = true;
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.ForeColor = System.Drawing.Color.Black;
		this.txtDocDate.Location = new System.Drawing.Point(612, 6);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(46, 19);
		this.txtDocDate.TabIndex = 9;
		this.txtDocDate.Text = "None";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(567, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(49, 19);
		this.label3.TabIndex = 8;
		this.label3.Text = "Date: ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 711);
		base.Controls.Add(this.PanelMainSelectedDocData);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmApprovalLogisticAgreement";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmApprovalLogisticAgreement_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.TotalRecordPanel.ResumeLayout(false);
		this.TotalRecordPanel.PerformLayout();
		this.GBFilters.ResumeLayout(false);
		this.GBFilters.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgentName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceTypeHistory).EndInit();
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
		this.PanelMainSelectedNetPreviousDocData.ResumeLayout(false);
		this.PanelMainSelectedNetPreviousDocData.PerformLayout();
		base.ResumeLayout(false);
	}
}
