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
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Service.lgstcm;

public class frmLoadLogisticAgreementForPO : Form
{
	private UserAccount UserAccount = new UserAccount();

	public DataTable dtLoader = new DataTable();

	private DataTable dtHistory = new DataTable();

	public string GdnDetailIds = string.Empty;

	private bool FormInitialized;

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private Panel panel3;

	internal GridEX grd;

	private DateTimePicker ToDate;

	private DateTimePicker FromDate;

	private Label label2;

	private Label label3;

	private TextBox txtDocNoTo;

	private Label label1;

	private TextBox txtDocNoFrom;

	private Label label4;

	internal UltraCombo CmbBrokerAgent;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label6;

	internal UltraCombo CmbItemName;

	private ToolStripButton BtnLoadSelectedRows;

	internal UltraCombo CmbServiceProvider;

	internal UltraCombo CmbServiceType;

	private Label label8;

	private Panel panel4;

	internal GridEX grdDetail;

	private Label label7;

	private Label label5;

	public frmLoadLogisticAgreementForPO(UserAccount user)
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
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 650);
			FromDate.Value = DateTime.Now.AddDays(-7.0);
			FromDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void InitializeComponentMethod()
	{
		try
		{
			CombosFill((await Task.Run(delegate
			{
				DataTable comboData = ComboDbCall();
				PendingDataDbCall();
				return new
				{
					ComboData = comboData
				};
			})).ComboData);
			GrdDataBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error occurred during database call: " + ex.Message);
		}
	}

	private DataTable ComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		try
		{
			return AgreementHeader.GetDataForDropDownFromAgreementHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesIds = Conversion.ToString((object)UserAccount.BranchesId),
				DocumentTypeIds = "1301"
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CombosFill(DataTable dtcombo)
	{
		try
		{
			DataTable dtST = new DataTable();
			dtST.Columns.Add("Id", typeof(int));
			dtST.Columns.Add("Name");
			DataTable dtSP = dtST.Clone();
			DataTable dtBA = dtST.Clone();
			DataTable dtItem = dtST.Clone();
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ServiceType")
				{
					dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ServiceProvider")
				{
					dtSP.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BrokerAgent")
				{
					dtBA.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ServiceItem")
				{
					dtItem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbServiceType, dtST, "Id", "Name", "Service Type", AllColumns: true, Conversion.ToInt(CmbServiceType.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbServiceProvider, dtSP, "Id", "Name", "Service Provider", AllColumns: true, Conversion.ToInt(CmbServiceProvider.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemName.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBrokerAgent, dtBA, "Id", "Name", "Broker Agent", AllColumns: true, Conversion.ToInt(CmbBrokerAgent.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingDataDbCall();
			GrdDataBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingDataDbCall()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.BranchesId = UserAccount.BranchesId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.DocumentTypeId = 1301;
			Obj.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			Obj.ToDate = Conversion.ToDateTime((object)ToDate.Value);
			Obj.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text);
			Obj.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text);
			Obj.TypeId = Conversion.ToInt(CmbServiceType.Value);
			Obj.BrokerAgentId = Conversion.ToInt(CmbBrokerAgent.Value);
			Obj.ServiceProviderId = Conversion.ToInt(CmbServiceProvider.Value);
			Obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			dtHistory = AgreementHeader.Agreement_DataForPurchaseOrder(Obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdDataBind()
	{
		try
		{
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("RecordNo", typeof(int));
				dt.Columns.Add("AgreementHeaderId", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocumentNo", typeof(int));
				dt.Columns.Add("DocumentDate", typeof(DateTime));
				dt.Columns.Add("ServiceTypeId", typeof(int));
				dt.Columns.Add("ServiceType", typeof(string));
				dt.Columns.Add("ServiceProviderId", typeof(int));
				dt.Columns.Add("ServiceProviderName", typeof(string));
				dt.Columns.Add("EffectiveDateFrom", typeof(DateTime));
				dt.Columns.Add("EffectiveDateTo", typeof(DateTime));
				dt.Columns.Add("IsActive", typeof(bool));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				dt.Columns.Add("IsApproved", typeof(bool));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovalUserName", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				HashSet<int> addedRecordNos = new HashSet<int>();
				foreach (DataRow r in dtHistory.Rows)
				{
					int Id = Conversion.ToInt(r["AgreementHeaderId"]);
					if (addedRecordNos.Add(Id))
					{
						dt.Rows.Add(r["RecordNo"], r["AgreementHeaderId"], r["DocumentTypeId"], r["DocumentNo"], r["DocumentDate"], r["ServiceTypeId"], r["ServiceType"], r["ServiceProviderId"], r["ServiceProviderName"], r["EffectiveDateFrom"], r["EffectiveDateTo"], r["IsActive"], r["EntryUserName"], r["EntryDate"], r["ModifyDate"], r["ModifyUserName"], r["IsApproved"], r["ApprovedDate"], r["ApprovalUserName"], r["RemarksHeader"], r["NoOfAttachments"]);
					}
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CheckedAllMainRows()
	{
		try
		{
			if (string.IsNullOrEmpty(GdnDetailIds))
			{
				return;
			}
			int result;
			HashSet<int> allowedDetailIds = new HashSet<int>((from s in GdnDetailIds.Trim(',').Split(',')
				where int.TryParse(s, out result)
				select s).Select(int.Parse));
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow row in rows)
			{
				int currentId = Conversion.ToInt(row.Cells["DetailId"].Value);
				if (allowedDetailIds.Contains(currentId))
				{
					row.BeginEdit();
					row.IsChecked = Convert.ToBoolean(value: true);
					row.EndEdit();
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSettings()
	{
		try
		{
			string[] hiddenColumns = new string[8] { "RecordNo", "AgreementHeaderId", "DocumentTypeId", "ServiceTypeId", "ServiceProviderId", "ServiceItemId", "DetailId", "FcyAmount" };
			CommonServices.GridWrappingAndColumnSettings(grd, 3, 3, null, hiddenColumns);
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["DocumentNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			grd.RootTable.Columns["DocumentDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["EffectiveDateFrom"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["EffectiveDateTo"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["ServiceType"].Width = Constants.InventoryConstants.DocumentType;
			grd.RootTable.Columns["ServiceProviderName"].Width = 180;
			grd.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grd.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["ApprovalUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grd.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			GridEX_Helper.AddSelectButton(grd, "Select", 30, 0);
			grd.FrozenColumns = 1;
			CheckedAllMainRows();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1301);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			DetailGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (item.Cells.Count <= 0)
				{
					return;
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("AgreementHeaderId", typeof(int));
				dt.Columns.Add("DetailId", typeof(int));
				dt.Columns.Add("PartyRefDocNo", typeof(string));
				dt.Columns.Add("PartyRefDocDate", typeof(DateTime));
				dt.Columns.Add("ServiceItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("TcyCode", typeof(string));
				dt.Columns.Add("TcyExchangeRate", typeof(decimal));
				dt.Columns.Add("ServiceRate", typeof(decimal));
				dt.Columns.Add("RateBaseUom", typeof(string));
				dt.Columns.Add("TcyAmount", typeof(decimal));
				dt.Columns.Add("LcyAmount", typeof(decimal));
				dt.Columns.Add("FcyAmount", typeof(decimal));
				dt.Columns.Add("Remarks", typeof(string));
				foreach (DataRow r2 in from r in dtHistory.AsEnumerable()
					where r.Field<int>("AgreementHeaderId") == Conversion.ToInt(item.Cells["AgreementHeaderId"].Value)
					select r)
				{
					DataRow newRow = dt.NewRow();
					newRow["AgreementHeaderId"] = r2["AgreementHeaderId"];
					newRow["DetailId"] = r2["DetailId"];
					newRow["PartyRefDocNo"] = r2["PartyRefDocNo"];
					newRow["PartyRefDocDate"] = r2["PartyRefDocDate"];
					newRow["ServiceItemId"] = r2["ServiceItemId"];
					newRow["ItemName"] = r2["ItemName"];
					newRow["ItemCode"] = r2["ItemCode"];
					newRow["TcyCode"] = r2["TransactionCurrencyCode"];
					newRow["TcyExchangeRate"] = r2["TransactionExchangeRate"];
					newRow["ServiceRate"] = r2["ServiceRate"];
					newRow["RateBaseUom"] = r2["RateBaseUom"];
					newRow["TcyAmount"] = r2["TcyAmount"];
					newRow["LcyAmount"] = r2["LcyAmount"];
					newRow["FcyAmount"] = r2["FcyAmount"];
					newRow["Remarks"] = r2["Remarks"];
					dt.Rows.Add(newRow);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "ServiceItemId", "DetailId", "AgreementHeaderId", "FcyAmount" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 3, 3, hiddenColumns);
			grdDetail.RootTable.Columns["PartyRefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdDetail.RootTable.Columns["PartyRefDocDate"].Width = Constants.InventoryConstants.DateConstant;
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemName", Constants.InventoryConstants.ItemName);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemCode", 60);
			GridEX_Helper.SetColumnWidth(grdDetail, "RateBaseUom", Constants.InventoryConstants.UomConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "TcyCode", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "TcyExchangeRate", Constants.InventoryConstants.ExRateConstant + 15);
			GridEX_Helper.SetColumnWidth(grdDetail, "ServiceRate", Constants.InventoryConstants.ExRateConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "TcyAmount", Constants.InventoryConstants.AmountConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "LcyAmount", Constants.InventoryConstants.AmountConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "Remarks", Constants.InventoryConstants.RemarksConstants);
			grdDetail.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			txtDocNoFrom.Clear();
			txtDocNoTo.Clear();
			((Control)(object)CmbBrokerAgent).Text = string.Empty;
			PendingDataDbCall();
			GrdDataBind();
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
			CombosFill(ComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			if (checkedRows.Count() == 0)
			{
				MessageBox.Show("Check the row first");
				return;
			}
			dtLoader = dtHistory.Clone();
			int baseId = Conversion.ToInt(checkedRows[0].Cells["AgreementHeaderId"].Value);
			List<int> detailIds = new List<int>();
			GridEXRow[] array = checkedRows;
			foreach (GridEXRow row in array)
			{
				if (Conversion.ToInt(row.Cells["AgreementHeaderId"].Value) != baseId)
				{
					MessageBox.Show("Sorry! You can Only Check rows which have the same DocNo");
					return;
				}
				int detailId = Conversion.ToInt(row.Cells["AgreementHeaderId"].Value);
				detailIds.Add(detailId);
			}
			if (detailIds.Count > 0)
			{
				List<DataRow> rows = (from r in dtHistory.AsEnumerable()
					where detailIds.Contains(r.Field<int>("AgreementHeaderId"))
					select r).ToList();
				if (rows.Any())
				{
					dtLoader = rows.CopyToDataTable();
				}
			}
			Hide();
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
			dt.Rows.Add("Ctrl+S", "For Search");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void frmLoadLogisticAgreementForPO_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				dtLoader = null;
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnReset_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDocNoTo_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance145 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
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
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
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
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.lgstcm.frmLoadLogisticAgreementForPO));
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadSelectedRows = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbServiceProvider = new UltraCombo();
		this.CmbServiceType = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.CmbBrokerAgent = new UltraCombo();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(973, 50);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripSeparator6, this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.BtnLoadSelectedRows, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(973, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnReset_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.BtnLoadSelectedRows.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadSelectedRows.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadSelectedRows.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadSelectedRows.Name = "BtnLoadSelectedRows";
		this.BtnLoadSelectedRows.Size = new System.Drawing.Size(58, 22);
		this.BtnLoadSelectedRows.Text = "Load";
		this.BtnLoadSelectedRows.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 27);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(399, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending Logistic Agreement For Purchase Order";
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 133);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(973, 206);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceProvider);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceType);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokerAgent);
		this.panel2.Controls.Add(this.txtDocNoTo);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.txtDocNoFrom);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.ToDate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 50);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(973, 61);
		this.panel2.TabIndex = 0;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(356, 29);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(92, 30);
		this.label7.TabIndex = 188;
		this.label7.Text = "Broker Agent / \r\nService Provider";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(356, 7);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(84, 19);
		this.label5.TabIndex = 187;
		this.label5.Text = "Bill To Party";
		this.CmbServiceProvider.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceProvider.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceProvider.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceProvider.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance146;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceProvider.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Location = new System.Drawing.Point(450, 31);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Name = "CmbServiceProvider";
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).TabIndex = 5;
		this.CmbServiceType.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Location = new System.Drawing.Point(750, 31);
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Name = "CmbServiceType";
		((System.Windows.Forms.Control)(object)this.CmbServiceType).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceType).TabIndex = 7;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(674, 37);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 185;
		this.label8.Text = "Service Type";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(674, 9);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 36;
		this.label6.Text = "Item Name";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance122).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(750, 3);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 6;
		this.CmbBrokerAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokerAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokerAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokerAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokerAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokerAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgent).Location = new System.Drawing.Point(450, 3);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgent).Name = "CmbBrokerAgent";
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgent).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgent).TabIndex = 4;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(275, 33);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.Size = new System.Drawing.Size(80, 23);
		this.txtDocNoTo.TabIndex = 3;
		this.txtDocNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoTo_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(199, 37);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 15);
		this.label1.TabIndex = 34;
		this.label1.Text = "Doc No To";
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(275, 5);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(80, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoFrom_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(199, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 32;
		this.label4.Text = "Doc No From";
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(70, 33);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(125, 22);
		this.ToDate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(70, 5);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(125, 22);
		this.FromDate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 37);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(910, 31);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(60, 26);
		this.btngrnlod.TabIndex = 8;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 111);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(973, 22);
		this.panel3.TabIndex = 31;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel4.Location = new System.Drawing.Point(0, 339);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(973, 23);
		this.panel4.TabIndex = 37;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 362);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(973, 249);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 36;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(973, 611);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmLoadLogisticAgreementForPO";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLoadLogisticAgreementForPO_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		base.ResumeLayout(false);
	}
}
