using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.ApprovalDashboard
{

public class CommercialInvoiceApproval : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtHistoryDbData = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool RateAddLessOnCommercialInvoice;

	public bool ForSpecialApproval;

	private bool FormInitialized;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private IContainer components;

	private Panel panel4;

	private Panel panel5;

	private Panel panel1;

	private Panel panel7;

	private ToolStrip toolStrip3;

	internal UltraCombo cmbFCurrencr;

	internal UltraCombo cmbPaymentTerm;

	internal UltraCombo combranches;

	internal UltraCombo comproject;

	internal UltraCombo ultraCombo3;

	internal UltraCombo CmbLCContractor;

	internal UltraCombo ultraCombo2;

	internal UltraCombo ultraCombo1;

	internal UltraCombo cmbCarierType;

	internal UltraCombo cmbExporterBank;

	internal UltraCombo cmbImporterBank;

	internal UltraCombo cmbEFormNo;

	internal UltraCombo cmbdeliveryterm;

	private ToolStrip miniToolStrip;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton toolStripButton4;

	private ToolStripButton toolStripButton5;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip2;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX gridDetailHistory;

	private Label label69;

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

	private Label label93;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel2;

	private Panel panel3;

	private Panel panel6;

	private Panel panel9;

	private Panel panel10;

	private Panel panel8;

	private ToolStripButton btnApproved;

	public CommercialInvoiceApproval(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(() => new
		{
			CombosData = ComboDbCall()
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
				RateAddLessOnCommercialInvoice = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("RateAddLessOnCommercialInvoice"));
				var result = t.Result;
				ComboBind(result.CombosData);
				FormInitialized = true;
				if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
				{
					FromDateHistory.Value = Convert.ToDateTime(RequestedFromDate);
					ToDateHistory.Value = Convert.ToDateTime(RequestedToDate);
					HistoryGridDbCall();
				}
				else
				{
					FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					ToDateHistory.Value = DateTime.Now;
					HistoryGridDbCall(Filters: false);
				}
				HistoryGridBind();
				FromDateHistory.Checked = false;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable ComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.GetDataForDropDownFromExportInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "Customer"
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ComboBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dt, "Id", "name", "Party", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			DataGridHistory.DataSource = null;
			gridDetailHistory.DataSource = null;
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
			ComboBind(ComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ExImCommercialInvoice_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridDbCall();
			HistoryGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridDbCall(bool Filters = true)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistoryDbData.Clear();
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.Ids = "204";
			if (Filters)
			{
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
			}
			Obj.IsApproved = false;
			if (ForSpecialApproval)
			{
				Obj.SkipZero = 1;
			}
			dtHistoryDbData = ExImInvoice.FormHistory(Obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridBind()
	{
		try
		{
			if (dtHistoryDbData.Rows.Count > 0)
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
				foreach (DataRow dr in dtHistoryDbData.Rows)
				{
					dtHistoryGrid.Rows.Add(dr["Id"], dr["InvoiceNo"], dr["DocCode"], Conversion.ToDateTime(dr["DocDate"]).ToString("dd-MMM-yyyy"), dr["Customer"], dr["NoOfContainers"], dr["GrossWeight"], dr["NetWeight"], dr["FCurrencyAmount"], dr["AddLessAmount"], dr["TotalAmount"], dr["LoadingPort"], dr["DestinationPort"], dr["EntryUserName"], dr["EntryDate"], dr["ModifyUserName"], dr["ModifyDate"], dr["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistoryGrid;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.DataSource = null;
				gridDetailHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 3, 3);
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
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
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			GridEX_Helper.AddSelectButton(DataGridHistory, "Select", 30, 0);
			GridEX_Helper.AddButton(DataGridHistory, "Approve", "Approve", 70, 1);
			GridEX_Helper.AddButton(DataGridHistory, "Slip", "521-Slip", 70, 2);
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 204);
			}
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
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Slip")
				{
					CommonServices.CommercialInvoiceSlip521(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Approve")
				{
					ApprovedRecord(Conversion.ToInt(r.Cells["Id"].Value));
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
				GetDetailByHeaderId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailByHeaderId(int Id)
	{
		try
		{
			ExImInvoice PIH = ExImInvoice.GetByID(Id);
			if (PIH == null || PIH.ExImInvoiceDetail.Count == 0)
			{
				gridDetailHistory.DataSource = null;
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("ContractNo");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("PackType");
			dt.Columns.Add("CropYear");
			dt.Columns.Add("JobLot");
			dt.Columns.Add("Qty/M.Ton", typeof(double));
			dt.Columns.Add("PackSize");
			dt.Columns.Add("NoOfBags", typeof(double));
			dt.Columns.Add("RateWithoutAddLess", typeof(double));
			dt.Columns.Add("RateAddLess", typeof(double));
			dt.Columns.Add("Cost/M.Ton", typeof(double));
			dt.Columns.Add("CostUom");
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("HSCode");
			dt.Columns.Add("ItemDetail");
			dt.Columns.Add("FarmingNTrade");
			dt.Columns.Add("PackingDetail");
			for (int i = 0; i < PIH.ExImInvoiceDetail.Count; i++)
			{
				dt.Rows.Add(PIH.ExImInvoiceDetail[i].ContractNoShipmentWise, PIH.ExImInvoiceDetail[i].ItemName, PIH.ExImInvoiceDetail[i].PackMaterilaType, PIH.ExImInvoiceDetail[i].CropYear, PIH.ExImInvoiceDetail[i].JobLotDescription, PIH.ExImInvoiceDetail[i].NetWeight / 1000.0, PIH.ExImInvoiceDetail[i].OuterUOM, PIH.ExImInvoiceDetail[i].OuterQty, (PIH.ExImInvoiceDetail[i].RateWithoutAddLess > 0.0) ? PIH.ExImInvoiceDetail[i].RateWithoutAddLess : PIH.ExImInvoiceDetail[i].RatePrice, PIH.ExImInvoiceDetail[i].RateAddLess, PIH.ExImInvoiceDetail[i].RatePrice, PIH.ExImInvoiceDetail[i].RateUOM, PIH.ExImInvoiceDetail[i].FcAmount, PIH.ExImInvoiceDetail[i].HsCode, PIH.ExImInvoiceDetail[i].ItemDescriptionManual, PIH.ExImInvoiceDetail[i].FarmingNTrade, PIH.ExImInvoiceDetail[i].OuterPackDescription);
			}
			gridDetailHistory.DataSource = dt;
			gridDetailHistory.RetrieveStructure();
			GridEX_Helper.GridWrappingAndColumnSettings(gridDetailHistory, 3, 3);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Approve Commercial Invoice's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					ApprovalList detail = new ApprovalList();
					detail.EntryUser = UserAccount.ID;
					detail.Id = Conversion.ToInt(item.Cells["Id"].Value);
					detail.ActionTypeId = ForSpecialApproval;
					obj.AllApprovalLists.Add(detail);
				}
				ExImInvoice.ApprovedRecordByExImInvoiceId(obj);
				MessageBox.Show("Record Approved Successfully");
				HistoryGridDbCall();
				HistoryGridBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApprovedRecord(int RecId)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve ?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.Id = RecId;
				item.EntryUser = UserAccount.ID;
				item.ActionTypeId = ForSpecialApproval;
				val.AllApprovalLists.Add(item);
				ExImInvoice.ApprovedRecordByExImInvoiceId(val);
				MessageBox.Show("Record Approved Successfully");
				HistoryGridDbCall();
				HistoryGridBind();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.Message);
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
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
					((Control)(object)gridDetailHistory).Focus();
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
		}
		catch (Exception)
		{
			throw;
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
			dt.Rows.Add("Ctrl+S", "For Show Data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grids");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On From Date");
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
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Approve")
				{
					ApprovedRecord(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Slip")
				{
					CommonServices.CommercialInvoiceSlip521(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Attachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 204);
				}
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
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Expected O, but got Unknown
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Expected O, but got Unknown
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Expected O, but got Unknown
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Expected O, but got Unknown
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Expected O, but got Unknown
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Expected O, but got Unknown
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Expected O, but got Unknown
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Expected O, but got Unknown
		//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Expected O, but got Unknown
		//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout gridDetailHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
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
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance165 = new Appearance();
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
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
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
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.CommercialInvoiceApproval));
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.gridDetailHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label93 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label92 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
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
		this.panel2 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnApproved = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbFCurrencr = new UltraCombo();
		this.cmbPaymentTerm = new UltraCombo();
		this.combranches = new UltraCombo();
		this.comproject = new UltraCombo();
		this.ultraCombo3 = new UltraCombo();
		this.CmbLCContractor = new UltraCombo();
		this.ultraCombo2 = new UltraCombo();
		this.ultraCombo1 = new UltraCombo();
		this.cmbCarierType = new UltraCombo();
		this.cmbExporterBank = new UltraCombo();
		this.cmbImporterBank = new UltraCombo();
		this.cmbEFormNo = new UltraCombo();
		this.cmbdeliveryterm = new UltraCombo();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1184, 624);
		this.panel4.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel6);
		this.panel7.Controls.Add(this.panel3);
		this.panel7.Controls.Add(this.panel2);
		this.panel7.Controls.Add(this.toolStrip3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1184, 624);
		this.panel7.TabIndex = 0;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 149);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1184, 222);
		this.panel8.TabIndex = 56;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1184, 222);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetailHistory);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(0, 371);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1184, 253);
		this.panel9.TabIndex = 57;
		this.gridDetailHistory.AllowEdit = (InheritableBoolean)2;
		gridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridDetailHistory.DesignTimeLayout = gridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridDetailHistory.DynamicFiltering = true;
		this.gridDetailHistory.FilterMode = (FilterMode)2;
		this.gridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Name = "gridDetailHistory";
		((ScrollableBase)this.gridDetailHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetailHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.gridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Size = new System.Drawing.Size(1184, 227);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).TabIndex = 1;
		this.gridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.gridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label93);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1184, 26);
		this.panel10.TabIndex = 0;
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label93.ForeColor = System.Drawing.Color.White;
		this.label93.Location = new System.Drawing.Point(2, 4);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(234, 17);
		this.label93.TabIndex = 1;
		this.label93.Text = "Detail of the Selected above Row";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label92);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 122);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1184, 27);
		this.panel6.TabIndex = 55;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1142, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(3, 4);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(142, 19);
		this.label92.TabIndex = 1;
		this.label92.Text = "Filtered Records";
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 51);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1184, 71);
		this.panel3.TabIndex = 54;
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.rdmodifydate);
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
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(932, 71);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
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
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(373, 20);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(58, 15);
		this.label20.TabIndex = 87;
		this.label20.Text = "Customer";
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
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance264).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance265).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance265).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance266).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance266;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance267).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance267).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance268;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance273).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance277;
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
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label69);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1184, 24);
		this.panel2.TabIndex = 53;
		this.label69.AutoSize = true;
		this.label69.BackColor = System.Drawing.Color.Teal;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(2, 2);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(205, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Export Invoice Approval";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.btnRefreshHistory, this.btnApproved });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1184, 27);
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
		this.btnApproved.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnApproved.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(93, 24);
		this.btnApproved.Text = "Approved";
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 624);
		this.panel1.TabIndex = 1;
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance262).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance262).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance262).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance263;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance270).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance270).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance271;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance250).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance251).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbFCurrencr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Location = new System.Drawing.Point(385, 129);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Name = "cmbFCurrencr";
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).TabIndex = 12;
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(111, 210);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 7;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance124;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance129).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1307, 237);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance252).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1404, 237);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.ultraCombo3.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo3.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo3.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo3.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance258;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance259).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance260;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo3.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Location = new System.Drawing.Point(111, 91);
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Name = "ultraCombo3";
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo3).TabIndex = 3;
		this.CmbLCContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbLCContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLCContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLCContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance151;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance208).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.CmbLCContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Location = new System.Drawing.Point(111, 120);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Name = "CmbLCContractor";
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).TabIndex = 4;
		this.ultraCombo2.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo2.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance172).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Location = new System.Drawing.Point(385, 37);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Name = "ultraCombo2";
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).TabIndex = 9;
		this.ultraCombo1.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance215;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance220).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance177).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(385, 67);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 10;
		this.cmbCarierType.AllowNull = (DefaultableBoolean)1;
		this.cmbCarierType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCarierType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCarierType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance224;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance229).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbCarierType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Location = new System.Drawing.Point(385, 96);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Name = "cmbCarierType";
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).TabIndex = 11;
		this.cmbExporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbExporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance236;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbExporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Location = new System.Drawing.Point(111, 180);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Name = "cmbExporterBank";
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).TabIndex = 6;
		this.cmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance239;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance242).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance244).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Location = new System.Drawing.Point(111, 151);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Name = "cmbImporterBank";
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).TabIndex = 5;
		this.cmbEFormNo.AllowNull = (DefaultableBoolean)1;
		this.cmbEFormNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEFormNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEFormNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance182;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance187).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance188).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbEFormNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Location = new System.Drawing.Point(662, 164);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Name = "cmbEFormNo";
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Size = new System.Drawing.Size(164, 22);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).TabIndex = 0;
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance199).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance200).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(385, 6);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 8;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 624);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "CommercialInvoiceApproval";
		this.Text = "Eccount Book ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExImCommercialInvoice_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ImProformaInvoice_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
