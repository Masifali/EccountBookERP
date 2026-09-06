using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Infragistics.Shared;
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
namespace Architecture.WinApp.Purchase
{

public class frmLoadGRN : Form
{
	private InvfrmPurchaseInvoice PI;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtGRNS = new DataTable();

	public DataTable dtBranch = new DataTable();

	public int DocumentTypeId = 46;

	private bool BranchImplemented;

	private bool AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt;

	private IContainer components;

	private Panel panel1;

	private Panel panelheading;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private Button btnReset;

	internal GridEX GridDetail;

	private RadioButton rdMarketGrn;

	private RadioButton rdRegularGrn;

	private Panel panel4;

	internal UltraCombo cmbBranchName;

	private Label label2;

	public frmLoadGRN(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmLoadGRN_Load_1(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PurchaseInvoiceBranchWise"));
			AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt"));
			rdRegularGrn.Checked = true;
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			dtGRNS.Columns.Add("Id");
			dtGRNS.Columns.Add("GrnType");
			if (dtBranch.Columns.Count == 0)
			{
				dtBranch.Columns.Add("BranchId", typeof(int));
				dtBranch.Columns.Add("BranchName", typeof(string));
			}
			BranchesFill();
			PendingGrnLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)cmbBranchName).Text);
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				dtBranch = InvGrn.GetBranchesAllocatedToUserFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, DocumentTypeId);
			}
			if (dtBranch.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtBranch, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
			UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj).Key = "Selected";
			((HeaderBase)obj.Header).Caption = string.Empty;
			obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj.DataType = typeof(bool);
			((HeaderBase)obj.Header).VisiblePosition = 0;
			cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			cmbBranchName.LimitToList.ToString();
			((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			if (string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)cmbBranchName).Text)))
			{
				if (!string.IsNullOrEmpty(BranchName))
				{
					((Control)(object)cmbBranchName).Text = BranchName;
				}
				else
				{
					((Control)(object)cmbBranchName).Text = Conversion.ToString(dtBranch.Rows[0]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdRegularGrn_Click(object sender, EventArgs e)
	{
		PendingGrnLoad();
	}

	private void rdMarketGrn_Click(object sender, EventArgs e)
	{
		PendingMarketGrnLoad();
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			if (rdRegularGrn.Checked)
			{
				PendingGrnLoad();
			}
			else
			{
				PendingMarketGrnLoad();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void PendingGrnLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.FromDate = FromDate.Value;
			grn.ToDate = Todate.Value;
			grn.DocumentTypeId = DocumentTypeId;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				grn.BranchesIds = BranchIds;
				DataTable lst = InvPurchaseInvoice.PendingGrnforLoadingInvoice(grn);
				if (lst.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("BranchName");
					dt.Columns.Add("DocDate", typeof(DateTime));
					dt.Columns.Add("DocNo", typeof(int));
					dt.Columns.Add("SupplierCustomerId");
					dt.Columns.Add("SupplierCustomer");
					dt.Columns.Add("DeliveryTerm");
					dt.Columns.Add("GpNO", typeof(int));
					dt.Columns.Add("GpDate", typeof(DateTime));
					dt.Columns.Add("BiltyNo");
					dt.Columns.Add("VehicleNo");
					dt.Columns.Add("PurchaseOrder");
					dt.Columns.Add("PurchaseOrderId");
					dt.Columns.Add("PurchaseAgainst");
					dt.Columns.Add("RefDocumentTypeId");
					dt.Columns.Add("GrnStatusId");
					dt.Columns.Add("GrnStatus");
					for (int i = 0; i < lst.Rows.Count; i++)
					{
						dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["BranchName"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToString("dd-MMM.yy"), lst.Rows[i]["DocNo"], lst.Rows[i]["SupplierCustomerId"], lst.Rows[i]["SupplierCustomer"], lst.Rows[i]["DeliveryTerm"], lst.Rows[i]["GpNO"], Conversion.ToDateTime(lst.Rows[i]["GpDate"]).ToShortDateString(), lst.Rows[i]["BiltyNo"], lst.Rows[i]["VehicleNo"], lst.Rows[i]["PurchaseOrder"], lst.Rows[i]["PurchaseOrderId"], lst.Rows[i]["PurchaseAgainst"], lst.Rows[i]["RefDocumentTypeId"], lst.Rows[i]["GrnStatusId"], lst.Rows[i]["GrnStatus"]);
					}
					grd.DataSource = dt;
					grd.RetrieveStructure();
					grdSettingsForGrnPending();
				}
				else
				{
					grd.ClearStructure();
					GridDetail.ClearStructure();
				}
				ctrlGrdBar1_Load(null, null);
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingMarketGrnLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			DataTable lst = InvPurchaseInvoice.GetPendingMarketGrnForPurchaseInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = FromDate.Value,
				ToDate = Todate.Value,
				DocumentTypeId = 46
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocDate");
				dt.Columns.Add("DocNo");
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("SupplierCustomer");
				dt.Columns.Add("GpNO");
				dt.Columns.Add("BiltyNo");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("ItemQty");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PurchaseOrder");
				dt.Columns.Add("PurchaseOrderId");
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["Id"], Conversion.ToDateTime(lst.Rows[i]["DocDate"]).ToString("dd-MMM.yy"), lst.Rows[i]["DocNo"], lst.Rows[i]["SupplierCustomerId"], lst.Rows[i]["SupplierCustomer"], lst.Rows[i]["GpNO"], lst.Rows[i]["BiltyNo"], lst.Rows[i]["VehicleNo"], lst.Rows[i]["ItemQty"], lst.Rows[i]["ItemName"], lst.Rows[i]["PurchaseOrder"], lst.Rows[i]["PurchaseOrderId"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettingsForGrnPending();
			}
			else
			{
				grd.ClearStructure();
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdSettingsForGrnPending()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "GrnStatusId", "RefDocumentTypeId", "SupplierCustomerId", "PurchaseOrderId" };
			if (DocumentTypeId == 137 || DocumentTypeId == 169)
			{
				hiddenColumns.AddRange(new string[3] { "PurchaseOrder", "PurchaseAgainst", "GpNO" });
			}
			if (DocumentTypeId == 165)
			{
				hiddenColumns.AddRange(new string[2] { "PurchaseOrder", "PurchaseAgainst" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns);
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["SupplierCustomer"].DefaultFilterRowComparison = (FilterConditionOperator)7;
			grd.RootTable.Columns["SupplierCustomer"].Width = 250;
			GridEX_Helper.AddSelectButton(grd, "Select", 30, 0);
			grd.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "GrnId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"EbUnit",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(GridDetail, 2, 3, hiddenColumns, editableColumns);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				InvGrn invGrn = InvGrn.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("Id");
				dthistorydetail.Columns.Add("GrnId");
				dthistorydetail.Columns.Add("Order");
				dthistorydetail.Columns.Add("Item");
				dthistorydetail.Columns.Add("CropYear");
				dthistorydetail.Columns.Add("Job");
				dthistorydetail.Columns.Add("PackingType");
				dthistorydetail.Columns.Add("UOM");
				dthistorydetail.Columns.Add("Qty", typeof(double));
				dthistorydetail.Columns.Add("GrossWight", typeof(double));
				dthistorydetail.Columns.Add("EbUnit", typeof(double));
				dthistorydetail.Columns.Add("EbTotal", typeof(double));
				dthistorydetail.Columns.Add("EbPurAgainstWeight", typeof(double));
				dthistorydetail.Columns.Add("WtCut", typeof(double));
				dthistorydetail.Columns.Add("WtCutTotal", typeof(double));
				dthistorydetail.Columns.Add("AddLesswt", typeof(double));
				dthistorydetail.Columns.Add("NetWeight", typeof(double));
				dthistorydetail.Columns.Add("StockWeight", typeof(double));
				dthistorydetail.Columns.Add("WareHouse");
				dthistorydetail.Columns.Add("LabNo");
				dthistorydetail.Columns.Add("City");
				dthistorydetail.Clear();
				for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGrn.invGrnDetail[i].Id, invGrn.invGrnDetail[i].InvGrnId, invGrn.invGrnDetail[i].PurchaseOrder, invGrn.invGrnDetail[i].Item, invGrn.invGrnDetail[i].CropYear, invGrn.invGrnDetail[i].JobLot, invGrn.invGrnDetail[i].PackingType, invGrn.invGrnDetail[i].UOM, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].EBWPerUnit, invGrn.invGrnDetail[i].EBWTotal, invGrn.invGrnDetail[i].EbPurAgainstWeight, invGrn.invGrnDetail[i].WtCut, invGrn.invGrnDetail[i].WtCutTotal, invGrn.invGrnDetail[i].AdLsWeight, invGrn.invGrnDetail[i].NetBillWeight, invGrn.invGrnDetail[i].StockWeight, invGrn.invGrnDetail[i].WarehouseId, invGrn.invGrnDetail[i].LabReportRef, invGrn.invGrnDetail[i].AreaCity);
				}
				GridDetail.DataSource = dthistorydetail;
				GridDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtGRNS = null;
		Hide();
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		FromDate.Focus();
		FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
		grd.DataSource = null;
		GridDetail.DataSource = null;
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
			MessageBox.Show(ex.Message);
		}
	}

	private void frmLoadGRN_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbBranchName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnReset_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
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
			int GrnType = 1;
			if (rdMarketGrn.Checked)
			{
				GrnType = 7;
			}
			if (Conversion.ToInt((object)grd.GetCheckedRows().Count().ToString()) > 0)
			{
				int supplierCustomer = 0;
				int OrderId = 0;
				int sID = 0;
				int sOrderId = 1;
				int sDocId = 0;
				int RefDocumentTypeId = 0;
				int RefId = 0;
				int DocId = 0;
				string DeliveryTerm = "";
				dtGRNS.Rows.Clear();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					supplierCustomer = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					OrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
					RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					DocId = Conversion.ToInt(r.Cells["Id"].Value);
					string D_Term = "";
					if (RefDocumentTypeId != 46)
					{
						D_Term = Conversion.ToString(r.Cells["DeliveryTerm"].Value);
					}
					if (Conversion.ToInt(r.Cells["GrnStatusId"].Value) == 1)
					{
						string Msg = "Approval is required because Bill weight exceeds the stock weight OR Empty Bags Deduction equal to zero";
						if (AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt)
						{
							Msg = "Approval is required because \nBill weight exceeds Balance Order Weight \n                           OR\nBill weight exceeds stock Weight \n                           OR\nEmpty Bags Deduction equal to zero";
						}
						MessageBox.Show(Msg, "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						return;
					}
					if (supplierCustomer != 0)
					{
						if (sID == 0)
						{
							sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						}
						if (RefId == 0)
						{
							RefId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
						}
						if (sOrderId == 1)
						{
							sOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
						}
						if (sDocId == 0)
						{
							sDocId = Conversion.ToInt(r.Cells["Id"].Value);
						}
						if (DeliveryTerm == "")
						{
							DeliveryTerm = D_Term;
						}
						if (DocumentTypeId == 166 && DocId != sDocId)
						{
							dtGRNS.Rows.Clear();
							MessageBox.Show("only single grn select");
							return;
						}
						if (sID != supplierCustomer)
						{
							dtGRNS.Rows.Clear();
							MessageBox.Show("Sorry the Grn should be of same Supplier");
							return;
						}
						if (RefId != RefDocumentTypeId)
						{
							dtGRNS.Rows.Clear();
							MessageBox.Show("Sorry the Grn should be of same PurchaseAgainst type");
							return;
						}
						if (RefDocumentTypeId != 46 && DeliveryTerm != D_Term)
						{
							dtGRNS.Rows.Clear();
							MessageBox.Show("Sorry the Grn should be of same Delivery Term");
							return;
						}
						if (sOrderId != OrderId)
						{
							dtGRNS.Rows.Clear();
							MessageBox.Show("Sorry the Grn Not should be of same PurchaseOrder");
							return;
						}
						if (r.Cells["PurchaseAgainst"].Value.ToString() == "PurchaseOrder")
						{
							GrnType = 1;
						}
						else if (r.Cells["PurchaseAgainst"].Value.ToString() == "Market Purchase")
						{
							GrnType = 2;
						}
						else if (r.Cells["PurchaseAgainst"].Value.ToString() == "Gate Purchase")
						{
							GrnType = 3;
						}
						else if (r.Cells["PurchaseAgainst"].Value.ToString() == "Purchase From Party Processing")
						{
							GrnType = 4;
						}
						dtGRNS.Rows.Add(r.Cells["Id"].Value.ToString(), GrnType.ToString());
						sID = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						sOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
						RefId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
						sDocId = Conversion.ToInt(r.Cells["Id"].Value);
					}
				}
				Hide();
			}
			else
			{
				MessageBox.Show("Check the row first");
			}
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
			dt.Rows.Add("Ctrl+E & ESC", "For Close");
			dt.Rows.Add("Ctrl+S", "For Show Records");
			dt.Rows.Add("Ctrl+N", "For Reset");
			dt.Rows.Add("Ctrl+L", "To Load Records");
			dt.Rows.Add("Ctrl+F5", "For Focus on From Date");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Branch");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panelheading = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.rdMarketGrn = new System.Windows.Forms.RadioButton();
		this.rdRegularGrn = new System.Windows.Forms.RadioButton();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.cmbBranchName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panelheading.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panelheading);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 26);
		this.panel1.TabIndex = 0;
		this.panelheading.BackColor = System.Drawing.Color.Teal;
		this.panelheading.Controls.Add(this.label23);
		this.panelheading.Controls.Add(this.ctrlGrdBar1);
		this.panelheading.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelheading.Location = new System.Drawing.Point(0, 0);
		this.panelheading.Name = "panelheading";
		this.panelheading.Size = new System.Drawing.Size(964, 28);
		this.panelheading.TabIndex = 29;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(89, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "GRN Load";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(923, 3);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 103);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(964, 261);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.rdMarketGrn);
		this.panel2.Controls.Add(this.rdRegularGrn);
		this.panel2.Controls.Add(this.btnReset);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel2.Location = new System.Drawing.Point(0, 26);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 52);
		this.panel2.TabIndex = 30;
		this.rdMarketGrn.AutoSize = true;
		this.rdMarketGrn.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdMarketGrn.Location = new System.Drawing.Point(658, 28);
		this.rdMarketGrn.Name = "rdMarketGrn";
		this.rdMarketGrn.Size = new System.Drawing.Size(84, 19);
		this.rdMarketGrn.TabIndex = 3;
		this.rdMarketGrn.TabStop = true;
		this.rdMarketGrn.Text = "Market Grn";
		this.rdMarketGrn.UseVisualStyleBackColor = true;
		this.rdMarketGrn.Visible = false;
		this.rdMarketGrn.Click += new System.EventHandler(rdMarketGrn_Click);
		this.rdRegularGrn.AutoSize = true;
		this.rdRegularGrn.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdRegularGrn.Location = new System.Drawing.Point(658, 6);
		this.rdRegularGrn.Name = "rdRegularGrn";
		this.rdRegularGrn.Size = new System.Drawing.Size(87, 19);
		this.rdRegularGrn.TabIndex = 13;
		this.rdRegularGrn.TabStop = true;
		this.rdRegularGrn.Text = "Regular Grn";
		this.rdRegularGrn.UseVisualStyleBackColor = true;
		this.rdRegularGrn.Visible = false;
		this.rdRegularGrn.Click += new System.EventHandler(rdRegularGrn_Click);
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReset.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(594, 22);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(56, 25);
		this.btnReset.TabIndex = 7;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(533, 22);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(61, 25);
		this.btnLoadOnInvoice.TabIndex = 6;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(471, 22);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(62, 25);
		this.btngrnlod.TabIndex = 5;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.Todate.CustomFormat = "dd-MMM-yyyy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(340, 23);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(128, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yyyy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(209, 23);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(128, 23);
		this.FromDate.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(340, 5);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(209, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 78);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 25);
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
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 389);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(964, 172);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 3;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel4.Location = new System.Drawing.Point(0, 364);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 25);
		this.panel4.TabIndex = 32;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(3, 21);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(204, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 148;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(79, 15);
		this.label2.TabIndex = 147;
		this.label2.Text = "Branch Name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 561);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmLoadGRN";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmLoadGRN_Load_1);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLoadGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panelheading.ResumeLayout(false);
		this.panelheading.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		base.ResumeLayout(false);
	}
}

}
