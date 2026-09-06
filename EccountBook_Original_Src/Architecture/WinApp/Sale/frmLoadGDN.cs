using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Purchase;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Sale;

public class frmLoadGDN : Form
{
	private InvfrmPurchaseInvoice PI;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtGDNS = new DataTable();

	public int DocumentTypeId;

	public string DocumentTypeIds = "";

	public bool BranchImplemented;

	public DataTable dtBranch = new DataTable();

	public bool IsStockReservedPerParty;

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button BtnShow;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label2;

	private Label label3;

	internal GridEX GridDetail;

	private Panel panel4;

	private Label label1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label4;

	internal UltraCombo CmbBranch;

	public frmLoadGDN(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmLoadGRN_Load(object sender, EventArgs e)
	{
		FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
		dtGDNS.Columns.Add("Id");
		dtBranch.Columns.Add("BranchId", typeof(int));
		dtBranch.Columns.Add("BranchName");
		BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SaleInvoiceBranchWise"));
		IsStockReservedPerParty = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsStockReservedPerParty"));
		BranchesFill();
		PendingGdnLoad();
	}

	private void BranchesFill()
	{
		try
		{
			Conversion.ToString((object)((Control)(object)CmbBranch).Text);
			dtBranch.Clear();
			((UltraGridBase)CmbBranch).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				int DocTypeId = 0;
				string DocTypeIds = "";
				DocTypeId = ((InvfrmInvSaleInvoiceDirect.FlagLoader == "SD") ? 124 : ((DocumentTypeId != 170) ? 86 : 170));
				if (!string.IsNullOrEmpty(DocumentTypeIds))
				{
					DocTypeIds = DocumentTypeIds;
					DocTypeId = 0;
				}
				dtBranch = InvGdn.GetBranchesAllocatedToUserFromGdn(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, DocTypeId, DocTypeIds);
			}
			if (dtBranch.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtBranch, CmbBranch, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
			UltraGridColumn obj = ((UltraGridBase)CmbBranch).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj).Key = "Selected";
			((HeaderBase)obj.Header).Caption = string.Empty;
			obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj.DataType = typeof(bool);
			((HeaderBase)obj.Header).VisiblePosition = 0;
			CmbBranch.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbBranch.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbBranch.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbBranch.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			((UltraGridBase)CmbBranch).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			CmbBranch.LimitToList.ToString();
			((Control)(object)CmbBranch).Text = UserAccount.BranchName.ToString();
			RowEnumerator enumerator = ((UltraGridBase)CmbBranch).Rows.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Cells["Selected"].Value = true;
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtGDNS.Rows.Add(0);
		Hide();
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			PendingGdnLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PendingGdnLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters gdn = new ReportsParameters();
			gdn.OrganizationId = UserAccount.OrganizationId;
			gdn.CompanyId = UserAccount.CompanyId;
			if (InvfrmInvSaleInvoiceDirect.FlagLoader == "SD")
			{
				gdn.DocumentTypeId = 124;
			}
			else if (DocumentTypeId == 170)
			{
				gdn.DocumentTypeId = 170;
			}
			else
			{
				gdn.DocumentTypeId = 86;
			}
			if (!string.IsNullOrEmpty(DocumentTypeIds))
			{
				gdn.DocumentTypeIds = DocumentTypeIds;
				gdn.DocumentTypeId = 0;
			}
			gdn.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			gdn.ToDate = Conversion.ToDateTime((object)Todate.Value);
			gdn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranch).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranch).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				gdn.BranchesIds = BranchIds;
				DataTable lst = InvSaleInvoice.PendingGdnforLoadingInvoiceRice(gdn);
				if (lst.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("OtherCategoryId", typeof(int));
					dt.Columns.Add("CategoryII", typeof(string));
					dt.Columns.Add("DocumentTypeId", typeof(int));
					dt.Columns.Add("DocumentType");
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("CostCenterId", typeof(int));
					dt.Columns.Add("CostCenter", typeof(string));
					dt.Columns.Add("BranchName");
					dt.Columns.Add("SupplierCustomerId");
					dt.Columns.Add("CustomerName");
					dt.Columns.Add("GpNO", typeof(int));
					dt.Columns.Add("BiltyNo");
					dt.Columns.Add("VehicleNo");
					dt.Columns.Add("ItemQty", typeof(double));
					dt.Columns.Add("OutwardGatePassId", typeof(int));
					dt.Columns.Add("WbIds", typeof(string));
					dt.Columns.Add("TicketNos", typeof(string));
					dt.Columns.Add("OrderNo", typeof(int));
					dt.Columns.Add("IsStockReserved", typeof(string));
					for (int i = 0; i < lst.Rows.Count; i++)
					{
						dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["OtherCategoryId"], lst.Rows[i]["OtherCategory"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentType"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToString("dd-MMM-yy"), lst.Rows[i]["DocNo"], lst.Rows[i]["CostCenterId"], lst.Rows[i]["CostCenterName"], lst.Rows[i]["BranchName"], lst.Rows[i]["SupplierCustomerId"], lst.Rows[i]["SupplierCustomer"], lst.Rows[i]["GpNO"], lst.Rows[i]["BiltyNo"], lst.Rows[i]["VehicleNo"], lst.Rows[i]["ItemQty"], lst.Rows[i]["OutwardGatePassId"], lst.Rows[i]["WbIds"], lst.Rows[i]["TicketNos"], lst.Rows[i]["OrderNo"], Conversion.ToBool(lst.Rows[i]["IsStockReserved"]) ? "Reserved" : "Not Reserved");
					}
					grd.DataSource = dt;
					grd.RetrieveStructure();
					grdSettingsForGrnPending();
				}
				else
				{
					grd.DataSource = null;
					GridDetail.DataSource = null;
				}
				return;
			}
			CmbBranch.Focus();
			throw new Exception("Select branch first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdSettingsForGrnPending()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["OutwardGatePassId"].Visible = false;
			grd.RootTable.Columns["WbIds"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["CostCenterId"].Visible = false;
			grd.RootTable.Columns["OtherCategoryId"].Visible = false;
			grd.RootTable.Columns["IsStockReserved"].Visible = IsStockReservedPerParty;
			grd.RootTable.Columns["BranchName"].Visible = !BranchImplemented;
			grd.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["GpNO"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["TicketNos"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,#";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 30;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				InvGdn invGdn = InvGdn.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("OrderId");
				dthistorydetail.Columns.Add("Order");
				dthistorydetail.Columns.Add("Item");
				dthistorydetail.Columns.Add("CropYear");
				dthistorydetail.Columns.Add("JobLot");
				dthistorydetail.Columns.Add("PackingType");
				dthistorydetail.Columns.Add("UOM");
				dthistorydetail.Columns.Add("Qty", typeof(double));
				dthistorydetail.Columns.Add("GrossWight", typeof(double));
				dthistorydetail.Columns.Add("EbUnit", typeof(double));
				dthistorydetail.Columns.Add("EbTotal", typeof(double));
				dthistorydetail.Columns.Add("WtCut", typeof(double));
				dthistorydetail.Columns.Add("WtCutTotal", typeof(double));
				dthistorydetail.Columns.Add("AddLesswt", typeof(double));
				dthistorydetail.Columns.Add("NetWeight", typeof(double));
				dthistorydetail.Columns.Add("StockWeight", typeof(double));
				dthistorydetail.Columns.Add("WareHouse");
				dthistorydetail.Columns.Add("LabNo");
				dthistorydetail.Columns.Add("City");
				dthistorydetail.Clear();
				for (int i = 0; i < invGdn.invGdnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGdn.invGdnDetail[i].SaleOrderId, invGdn.invGdnDetail[i].SaleOrderNo, invGdn.invGdnDetail[i].Item, invGdn.invGdnDetail[i].CropYear, invGdn.invGdnDetail[i].JobLot, invGdn.invGdnDetail[i].PackingType, invGdn.invGdnDetail[i].UOMCode, invGdn.invGdnDetail[i].ItemQty, invGdn.invGdnDetail[i].GrossWeight, invGdn.invGdnDetail[i].EBWPerUnit, invGdn.invGdnDetail[i].EBWTotal, invGdn.invGdnDetail[i].WtCut, invGdn.invGdnDetail[i].WtCutTotal, invGdn.invGdnDetail[i].AdLsWeight, invGdn.invGdnDetail[i].NetBillWeight, invGdn.invGdnDetail[i].StockWeight, invGdn.invGdnDetail[i].WareHouseCode, invGdn.invGdnDetail[i].LabReportRef, invGdn.invGdnDetail[i].AreaCity);
				}
				GridDetail.DataSource = dthistorydetail;
				GridDetail.RetrieveStructure();
				GridDetailSetting();
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
			GridDetail.RootTable.Columns["OrderId"].Visible = false;
			GridDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["EbUnit"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["GrossWight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["GrossWight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["GrossWight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["EbTotal"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["WtCut"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["WtCut"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["WtCutTotal"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["AddLesswt"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["AddLesswt"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(GridDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			int GdnId = Conversion.ToInt(currentRow.Cells["Id"].Value);
			int GpId = Conversion.ToInt(currentRow.Cells["OutwardGatePassId"].Value);
			string WbIds = Conversion.ToString(currentRow.Cells["WbIds"].Value);
			if (e.Column.Key == "DocNo")
			{
				CommonServices.InvGdnSlip260(GdnId);
			}
			else if (e.Column.Key == "GpNO" && GpId > 0)
			{
				CommonServices.GatePassOutwardSlipandRegister(GpId);
			}
			else if (e.Column.Key == "TicketNos" && WbIds != "0")
			{
				DataTable dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Ids = WbIds
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "280-InvRptWeighBridgeSlip.rpt");
			}
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnLoad(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)grd.GetCheckedRows().Count().ToString()) > 0)
			{
				int OrderNo = 0;
				int sID = 0;
				int DocID = 0;
				int CostCentrId = 0;
				string IsReserved = "";
				dtGDNS.Rows.Clear();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					int supplierCustomer = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					if (supplierCustomer != 0)
					{
						if (sID == 0)
						{
							sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						}
						if (sID != supplierCustomer)
						{
							dtGDNS.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Gdn's are not of same Customer");
							return;
						}
					}
					int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
					if (DocumentTypeId != 0)
					{
						if (DocID == 0)
						{
							DocID = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
						}
						if (DocID != DocumentTypeId)
						{
							dtGDNS.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Gdn's are not of same DocumentType");
							return;
						}
					}
					int CurrOrderNo = Conversion.ToInt(r.Cells["OrderNo"].Value);
					if (CurrOrderNo != 0)
					{
						if (OrderNo == 0)
						{
							OrderNo = Conversion.ToInt(r.Cells["OrderNo"].Value);
						}
						if (OrderNo != CurrOrderNo)
						{
							dtGDNS.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Gdn's are not of same Orders");
							return;
						}
					}
					int costCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
					if (costCenterId != 0)
					{
						if (CostCentrId == 0)
						{
							CostCentrId = costCenterId;
						}
						if (CostCentrId != costCenterId)
						{
							dtGDNS.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Gdn's are not of same Cost Center");
							return;
						}
					}
					if (IsStockReservedPerParty)
					{
						string reserved = Conversion.ToString(r.Cells["IsStockReserved"].Value);
						if (reserved != "")
						{
							if (IsReserved == "")
							{
								IsReserved = reserved;
							}
							if (reserved != IsReserved)
							{
								dtGDNS.Rows.Clear();
								MessageBox.Show("Sorry!. The Selected Gdn's are not of same Reserved Status");
								return;
							}
						}
					}
					dtGDNS.Rows.Add(r.Cells["Id"].Value.ToString());
					sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					DocID = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
					OrderNo = Conversion.ToInt(r.Cells["OrderNo"].Value);
				}
				Hide();
				return;
			}
			throw new Exception("Check the row first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E & ESC", "For Close");
			dt.Rows.Add("Ctrl+S", "For Search");
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Down", "To focus on main grid");
			dt.Rows.Add("Ctrl+Up", "To focus on From date");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any grid To Call Function's On Button Or Link");
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
		MakeShortCutKeys();
	}

	private void frmLoadGDN_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				dtGDNS = null;
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				BtnShow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				BtnLoad(null, null);
			}
			else if ((!e.Control || e.KeyCode != Keys.N) && (!e.Control || e.KeyCode != Keys.R))
			{
				if (e.Control && e.KeyCode == Keys.F5)
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.frmLoadGDN));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.BtnShow = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel4.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1037, 51);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1037, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Visible = false;
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Visible = false;
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortcutKeys.Image");
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
		this.label23.Location = new System.Drawing.Point(1, 28);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(211, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending GDN For Invoice";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(995, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 116);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1037, 241);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.BtnShow);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 51);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1037, 40);
		this.panel2.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(12, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(55, 15);
		this.label4.TabIndex = 18;
		this.label4.Text = "Branches";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(77, 5);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(232, 29);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 17;
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(575, 8);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(135, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(385, 8);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(135, 23);
		this.FromDate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(521, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(317, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(786, 7);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(58, 25);
		this.btnLoadOnInvoice.TabIndex = 3;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(BtnLoad);
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(716, 7);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(64, 25);
		this.BtnShow.TabIndex = 2;
		this.BtnShow.Text = "Search";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 91);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1037, 25);
		this.panel3.TabIndex = 31;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		this.GridDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxVisible = false;
		this.GridDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 379);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(1037, 297);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 2;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel4.Location = new System.Drawing.Point(0, 357);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1037, 22);
		this.panel4.TabIndex = 32;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(215, 14);
		this.label1.TabIndex = 24;
		this.label1.Text = "Detail of the Above Selected Row";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1037, 676);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Name = "frmLoadGDN";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmLoadGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLoadGDN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		base.ResumeLayout(false);
	}
}
