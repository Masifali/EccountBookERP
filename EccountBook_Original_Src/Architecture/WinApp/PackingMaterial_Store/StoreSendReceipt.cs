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
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PackingMaterial_Store;

public class StoreSendReceipt : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private Attachment AT = new Attachment();

	private DataTable dtDetail = new DataTable();

	private DataTable dtItem = new DataTable();

	private int RecId;

	private int[] AryColWidth;

	private LoaderForPackingMaterialAndConsumeableStoreItemSendReceipts Loader;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnattachment;

	private ToolStripButton btnPrint;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private CheckBox ChkPrint;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	internal UltraCombo CmbSenderWareHouse;

	private Label label4;

	internal UltraCombo CmbReceiverWareHouse;

	private Label label3;

	private Panel panel1;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton BtnLoader;

	private ToolStrip toolStrip2;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label2;

	internal GridEX GrdHistory;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label6;

	internal GridEX GrdDetail;

	private ToolStripButton btnGenerateItemNameFromCurrentPosition;

	internal GridEX Grd;

	private Panel panel8;

	private GroupBox groupBox3;

	private Button btnshow;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label7;

	private Label label16;

	internal UltraCombo CmbSenderWarehouseHistory;

	internal UltraCombo CmbReceiverWarehouseHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	public StoreSendReceipt(UserAccount PUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = PUser;
	}

	private void StoreSendReceipt_Load(object sender, EventArgs e)
	{
		try
		{
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			base.WindowState = FormWindowState.Maximized;
			GenerateCode();
			ItemFill();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("RefDocNoId", typeof(int));
			dtDetail.Columns.Add("RefDocDetailId", typeof(int));
			dtDetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtDetail.Columns.Add("RecordNo", typeof(int));
			dtDetail.Columns.Add("ItemId");
			dtDetail.Columns.Add("ItemConditionId", typeof(int));
			dtDetail.Columns.Add("PackingTypeId");
			dtDetail.Columns.Add("PackingType");
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("BalQty", typeof(double));
			dtDetail.Columns.Add("AvgRate", typeof(double));
			dtDetail.Columns.Add("Amount", typeof(double));
			Grd.DataSource = dtDetail;
			Grd.RetrieveStructure();
			GrdSettings();
			DocDate.Tag = DocDate.Value;
			SenderWareHouse();
			ReceiverWareHouse();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			FromDateHistory.Value = DateTime.Now.AddDays(-7.0);
			ToDateHistory.Value = DateTime.Now;
			ComboBindForHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int Code = InvStoreSendReceipt.GenerateCode(new InvStoreSendReceipt
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				DocumentTypeId = 177,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
				return;
			}
			throw new Exception("DocNo Not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SenderWareHouse()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalWarehousesWithBranches.Where((GlobalWarehousesWithBranch r) => r.IsActive && r.WareHouseTypeId == 2).ToList(), (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			InfragisticsHelper.BindAndRetainSelection(CmbSenderWareHouse, dt, "Id", "Description", "Sender Warehouse", AllColumns: true, Conversion.ToInt(CmbSenderWareHouse.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ReceiverWareHouse()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalWarehousesWithBranches.Where((GlobalWarehousesWithBranch r) => r.IsActive && (r.WareHouseTypeId == 2 || r.WareHouseTypeId == 3)).ToList(), (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			InfragisticsHelper.BindAndRetainSelection(CmbReceiverWareHouse, dt, "Id", "Description", "Receiver Warehouse", AllColumns: true, Conversion.ToInt(CmbReceiverWareHouse.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbReceiverWareHouse).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			dtItem.Rows.Clear();
			dtItem = CommonServices.GetPackingMaterialItemsAllocateToFlow();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ComboBindForHistory()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			DataTable dtWarehouse = new DataTable();
			dtWarehouse.Columns.Add("Id", typeof(int));
			dtWarehouse.Columns.Add("ReferenceName");
			DataTable dtReceiverWare = dtWarehouse.Clone();
			DataTable dt = InvStoreSendReceipt.GetDataForDropDown(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "SenderWarehouse")
					{
						dtWarehouse.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ReceiverWarehouse")
					{
						dtReceiverWare.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtWarehouse, CmbSenderWarehouseHistory, "Id", "ReferenceName", "Sender Warehouse", ZeroIndex: false);
			DDL.BindDDL(dtReceiverWare, CmbReceiverWarehouseHistory, "Id", "ReferenceName", "Receiver Warehouse", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridComboBind()
	{
		GridEX_Helper.ComboBind(Grd.RootTable.Columns["ItemId"], dtItem, "ItemId", "ItemName", "Item Name", Constants.InventoryConstants.ItemName + 20);
		DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList(), (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
		GridEX_Helper.ComboBind(Grd.RootTable.Columns["ItemConditionId"], dt, "Id", "Description", "Item Condition", Constants.InventoryConstants.StringConstant);
	}

	private void GrdSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "RefDocNoId", "RefDocDetailId", "RefDocumentTypeId", "PackingTypeId", "BalQty" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"ItemQty",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(Grd, 2, 3, hiddenColumns, editableColumns);
			GridComboBind();
			Grd.RootTable.Columns["RecordNo"].Width = Constants.InventoryConstants.DocNoConstant;
			Grd.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			Grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			Grd.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			Grd.RootTable.Columns["AvgRate"].Width = Constants.InventoryConstants.ExRateConstant;
			Grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			GridEX_Helper.AddButton(Grd, "Delete", "X", 20);
			GridEX_Helper.AddButton(Grd, "Add", "+", 20);
			ctrlGrdBar1_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)Grd).Name);
			if (fs != null)
			{
				Grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistory).Name);
			if (fs != null)
			{
				GrdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdDetail).Name);
			if (fs != null)
			{
				GrdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoader_Click(object sender, EventArgs e)
	{
		try
		{
			Loader = new LoaderForPackingMaterialAndConsumeableStoreItemSendReceipts(UserAccount);
			Loader.ShowDialog();
			Loader.BringToFront();
			LoadDetailForGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDetailForGrid()
	{
		try
		{
			DataTable dt = Loader.DtLoader;
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			if (((UltraGridBase)CmbSenderWareHouse).ActiveRow != null && Grd.GetRows().Count() > 0 && Conversion.ToInt(CmbSenderWareHouse.Value) != Conversion.ToInt(dt.Rows[0]["SenderWarehouseId"]))
			{
				throw new Exception("You Can't Load Data Of Different Warehouse");
			}
			CmbSenderWareHouse.Value = Conversion.ToInt(dt.Rows[0]["SenderWarehouseId"]);
			bool flag = false;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				int flagMaxRowNoInDetail = 0;
				if (Grd.GetRows().Length != 0)
				{
					flagMaxRowNoInDetail = Conversion.ToInt((object)Grd.GetRows().Max((GridEXRow o) => Conversion.ToInt(o.Cells["RecordNo"].Value)));
					GridEXRow[] rows = Grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["RefDocDetailId"].Value) == Conversion.ToInt(dt.Rows[i]["RefDocSubIdNo"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtDetail.Rows.Add(0, dt.Rows[i]["RefDocIdNo"], dt.Rows[i]["RefDocSubIdNo"], dt.Rows[i]["RefDocumentTypeId"], (flagMaxRowNoInDetail > 0) ? (flagMaxRowNoInDetail + 1) : (i + 1), 0, 0, dt.Rows[i]["PackingTypeId"], dt.Rows[i]["PackingType"], dt.Rows[i]["BalQty"], dt.Rows[i]["BalQty"], 0, 0);
				}
				flag = false;
			}
			Grd.DataSource = dtDetail;
			Grd.RetrieveStructure();
			GrdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			RecId = 0;
			txtdocno.Clear();
			((Control)(object)CmbSenderWareHouse).Text = string.Empty;
			GenerateCode();
			dtDetail.Rows.Clear();
			Grd.DataSource = dtDetail;
			btnsave.Visible = true;
			btnupdate.Visible = false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "WarehousesWithBranches", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			ItemFill();
			SenderWareHouse();
			ReceiverWareHouse();
			GridComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (Conversion.ToInt((object)txtdocno.Text.Trim()) == 0)
		{
			MessageBox.Show("Doc No is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSenderWareHouse).ActiveRow == null)
		{
			MessageBox.Show("Sender Warehouse is Required");
			CmbSenderWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbReceiverWareHouse).ActiveRow == null)
		{
			MessageBox.Show("Receiver Warehouse is Required");
			CmbReceiverWareHouse.Focus();
			return false;
		}
		return true;
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvStoreSendReceipt obj = new InvStoreSendReceipt();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.InvStoreSendReceiptId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 177;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ID;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ApprovedUserId = UserAccount.ID;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.DocDate = DocDate.Value;
			obj.SenderWarehouseId = Conversion.ToInt(CmbSenderWareHouse.Value);
			obj.ReceiverWarehouseId = Conversion.ToInt(CmbReceiverWareHouse.Value);
			obj.ScreenName = Conversion.ToString((object)base.Name);
			obj.invStoreSendReceiptDetailsList = new List<InvStoreSendReceiptDetail>();
			if (Grd.RowCount > 0)
			{
				GridEXRow[] rows = Grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvStoreSendReceiptDetail Sd = new InvStoreSendReceiptDetail();
					Sd.InvStoreSendReceiptDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					Sd.RefDocNoId = Conversion.ToInt(r.Cells["RefDocNoId"].Value);
					Sd.RefDocDetailId = Conversion.ToInt(r.Cells["RefDocDetailId"].Value);
					Sd.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					Sd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (Sd.ItemId == 0)
					{
						throw new Exception("Item Required In Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
					Sd.ItemName = Conversion.ToString((object)r.Cells["ItemId"].Text);
					Sd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
					if (Sd.ItemConditionId == 0)
					{
						throw new Exception("Item Condition Required In Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
					Sd.ItemCondition = Conversion.ToString((object)r.Cells["ItemConditionId"].Text);
					Sd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					if (Sd.InvPackingTypeId == 0)
					{
						throw new Exception("Packing Type Required In Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
					Sd.QtyIn = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					if (Sd.QtyIn == 0m)
					{
						throw new Exception("Item Qty Required In Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
					Sd.ItemRate = Conversion.ToDecimal(r.Cells["AvgRate"].Value);
					if (Sd.ItemRate == 0m)
					{
						throw new Exception("Avg Rate Required In Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
					Sd.ItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					if (Sd.ItemAmount == 0m)
					{
						throw new Exception("Amount Required In Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
					Sd.SortNo = r.RowIndex + 1;
					DataTable dt2 = (DataTable)Grd.DataSource;
					if (dt2.AsEnumerable().Count((DataRow row) => row.Field<int>("RecordNo") == Conversion.ToInt(r.Cells["RecordNo"].Value)) > 1)
					{
						double BalQty = Conversion.ToDouble(r.Cells["BalQty"].Value);
						if ((from row in dt2.AsEnumerable()
							where row.Field<int>("RecordNo") == Conversion.ToInt(r.Cells["RecordNo"].Value)
							select row).Sum((DataRow row) => row.Field<double>("ItemQty")) > BalQty)
						{
							throw new Exception("Total Qty of Record#" + r.Cells["RecordNo"].Value?.ToString() + " Can't Be Greater Than Balance Qty Which Is " + BalQty);
						}
					}
					DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
					if (dt.Rows.Count > 0)
					{
						DataRow[] dr = dt.Select("Equivalent='" + 1 + "'");
						if (dr.Length == 0)
						{
							MessageBox.Show("RateUom With Equivalent [1] is not define For Item : " + Conversion.ToString((object)r.Cells["ItemName"].Text));
							return;
						}
						Sd.ItemUomId = Conversion.ToInt(dr[0].ItemArray[0]);
					}
					obj.invStoreSendReceiptDetailsList.Add(Sd);
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				obj.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvStoreSendReceipt>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = InvStoreSendReceipt.Save(obj);
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully [" + obj.DocNo + "]");
				}
				else
				{
					MessageBox.Show("Record Update Successfully [" + obj.DocNo + "]");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 177, base.Name);
				}
				Reset();
				if (ChkPrint.Checked)
				{
					GeneratePrint(success);
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				DocDate.Focus();
			}
			else
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridHistoryBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 177;
			if (FromDateHistory.Checked)
			{
				obj.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				obj.ToDate = ToDateHistory.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.FromWarehouseId = Conversion.ToInt(CmbSenderWarehouseHistory.Value);
			obj.ToWarehouseId = Conversion.ToInt(CmbReceiverWarehouseHistory.Value);
			DataTable dt = InvStoreSendReceipt.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("DocNo", typeof(int));
				dtHistory.Columns.Add("DocDate", typeof(DateTime));
				dtHistory.Columns.Add("SenderWarehouse");
				dtHistory.Columns.Add("ReceiverWarehouse");
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("EntryUser");
				dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser");
				dtHistory.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dt.Rows[i]["InvStoreSendReceiptId"], dt.Rows[i]["DocNo"], dt.Rows[i]["DocDate"], dt.Rows[i]["SenderWareHouse"], dt.Rows[i]["ReceiverWareHouse"], dt.Rows[i]["EntryDate"], dt.Rows[i]["EntryUser"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["ModifyUser"], dt.Rows[i]["NoOfAttachments"]);
				}
				GrdHistory.DataSource = dtHistory;
				GrdHistory.RetrieveStructure();
				GridHistorySetting();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridHistorySetting()
	{
		try
		{
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistory, 2, 3);
			GrdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			GrdHistory.RootTable.Columns["SenderWarehouse"].Width = Constants.InventoryConstants.WareHouseName + 20;
			GrdHistory.RootTable.Columns["ReceiverWarehouse"].Width = Constants.InventoryConstants.WareHouseName + 20;
			GrdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GrdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			GrdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GrdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			GrdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			GrdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GridEX_Helper.AddButton(GrdHistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(GrdHistory, "Print", "Print", 40, 1);
			GrdHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = Grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				decimal BalQty = Conversion.ToDecimal(r.Cells["BalQty"].Value);
				decimal enteredQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
				decimal Rate = default(decimal);
				if (e.Column.Key == "ItemId" || e.Column.Key == "ItemConditionId")
				{
					DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(ItemId, DocDate.Value, ItemConditionId, RecId, (RecId > 0) ? 177 : 0);
					if (dtRate.Rows.Count > 0)
					{
						Rate = Math.Round(Conversion.ToDecimal(dtRate.Rows[0]["AvgRate"]), 3);
					}
					r.Cells["AvgRate"].Value = Rate;
				}
				else if (e.Column.Key == "ItemQty" && btnsave.Visible && btnsave.Enabled)
				{
					int recordNo = Conversion.ToInt(r.Cells["RecordNo"].Value);
					DataRow dr = dtDetail.Rows[r.RowIndex];
					decimal otherRowsQty = (from row in dtDetail.AsEnumerable()
						where row.Field<int>("RecordNo") == recordNo && row != dr
						select row).Sum((DataRow row) => Conversion.ToDecimal(row["ItemQty"]));
					decimal maxAllowed = BalQty - otherRowsQty;
					if (enteredQty <= 0m)
					{
						r.Cells["ItemQty"].Value = maxAllowed;
						enteredQty = maxAllowed;
						MessageBox.Show($"Qty must be greater than 0. Reset to max allowed: {maxAllowed}", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else if (enteredQty > maxAllowed)
					{
						r.Cells["ItemQty"].Value = maxAllowed;
						enteredQty = maxAllowed;
						MessageBox.Show($"Qty exceeds allowed balance for RecordNo {recordNo}.\n" + $"Other rows already use {otherRowsQty}, max allowed is {maxAllowed}.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
				decimal num = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
				decimal AvgRate = Conversion.ToDecimal(r.Cells["AvgRate"].Value);
				decimal Amount = num * AvgRate;
				r.Cells["Amount"].Value = Amount;
			}
			Grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = Grd.CurrentRow;
			if (e.Column.Key == "Delete" && item != null && (int)item.RowType == 0)
			{
				item.Delete();
			}
			if (e.Column.Key == "Add")
			{
				DataRow dr = dtDetail.Rows[item.RowIndex];
				double BalQty = Conversion.ToDouble(dr["BalQty"]);
				double ItemQty = Conversion.ToDouble(dr["ItemQty"]);
				double QtyTobeAdded = 0.0;
				if (BalQty == ItemQty)
				{
					MessageBox.Show("Current Row can't be breakable. First Break The Qty of Row", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (dtDetail.AsEnumerable().Count((DataRow row) => row.Field<int>("RecordNo") == Conversion.ToInt(dr["RecordNo"])) == 1)
				{
					QtyTobeAdded = BalQty - ItemQty;
				}
				else
				{
					double totalQty = (from row in dtDetail.AsEnumerable()
						where row.Field<int>("RecordNo") == Conversion.ToInt(dr["RecordNo"])
						select row).Sum((DataRow row) => row.Field<double>("ItemQty"));
					if (totalQty > BalQty)
					{
						throw new Exception("Total Qty Of Record#" + dr["RecordNo"]?.ToString() + " Can't Be Greater Than Balance Qty Which Is " + BalQty);
					}
					QtyTobeAdded = BalQty - totalQty;
					if (QtyTobeAdded == 0.0)
					{
						MessageBox.Show("Current Row can't be breakable. First Break The Qty of any Row of Record No" + dr["RecordNo"], "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				dtDetail.Rows.Add(dr.ItemArray);
				Grd.DataSource = dtDetail;
				dtDetail.Rows[dtDetail.Rows.Count - 1]["ItemQty"] = QtyTobeAdded;
				if (btnupdate.Visible && btnupdate.Enabled)
				{
					dtDetail.Rows[dtDetail.Rows.Count - 1]["Id"] = 0;
				}
			}
			if (Grd.GetRows().Length == 0)
			{
				((Control)(object)CmbSenderWareHouse).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			RecId = Id;
			InvStoreSendReceipt obj = InvStoreSendReceipt.GetByID(Id);
			txtdocno.Text = obj.DocNo.ToString();
			DocDate.Value = obj.DocDate;
			DocDate.Tag = obj.DocDate;
			CmbSenderWareHouse.Value = obj.SenderWarehouseId;
			CmbReceiverWareHouse.Value = obj.ReceiverWarehouseId;
			dtDetail.Rows.Clear();
			Dictionary<int, int> recordNoMap = new Dictionary<int, int>();
			int recordCounter = 0;
			foreach (InvStoreSendReceiptDetail invStoreSendReceiptDetails in obj.invStoreSendReceiptDetailsList)
			{
				int detailId = invStoreSendReceiptDetails.RefDocDetailId;
				if (!recordNoMap.ContainsKey(detailId))
				{
					recordCounter = (recordNoMap[detailId] = recordCounter + 1);
				}
			}
			for (int i = 0; i < obj.invStoreSendReceiptDetailsList.Count; i++)
			{
				dtDetail.Rows.Add(obj.invStoreSendReceiptDetailsList[i].InvStoreSendReceiptDetailId, obj.invStoreSendReceiptDetailsList[i].RefDocNoId, obj.invStoreSendReceiptDetailsList[i].RefDocDetailId, obj.invStoreSendReceiptDetailsList[i].RefDocumentTypeId, recordNoMap[obj.invStoreSendReceiptDetailsList[i].RefDocDetailId], obj.invStoreSendReceiptDetailsList[i].ItemId, obj.invStoreSendReceiptDetailsList[i].ItemConditionId, obj.invStoreSendReceiptDetailsList[i].InvPackingTypeId, obj.invStoreSendReceiptDetailsList[i].PackingType, obj.invStoreSendReceiptDetailsList[i].QtyIn, obj.invStoreSendReceiptDetailsList[i].QtyIn, obj.invStoreSendReceiptDetailsList[i].ItemRate, obj.invStoreSendReceiptDetailsList[i].ItemAmount);
			}
			btnsave.Visible = false;
			btnupdate.Visible = true;
			Grd.DataSource = dtDetail;
			Grd.RetrieveStructure();
			GrdSettings();
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			tabControl1.SelectedIndex = 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(id);
				}
				else if (e.Column.Key == "Print")
				{
					GeneratePrint(id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 177);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow Row = GrdHistory.CurrentRow;
			if (Row != null && (int)Row.RowType == 0 && Row.Cells.Count > 0)
			{
				BindDetailOfHeaderId(Conversion.ToInt(Row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDetailOfHeaderId(int Id)
	{
		try
		{
			InvStoreSendReceipt obj = InvStoreSendReceipt.GetByID(Id);
			if (obj.invStoreSendReceiptDetailsList != null && obj.invStoreSendReceiptDetailsList.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemName");
				dt.Columns.Add("ItemCondition");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("AvgRate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				for (int i = 0; i < obj.invStoreSendReceiptDetailsList.Count; i++)
				{
					dt.Rows.Add(obj.invStoreSendReceiptDetailsList[i].ItemName, obj.invStoreSendReceiptDetailsList[i].ItemCondition, obj.invStoreSendReceiptDetailsList[i].PackingType, obj.invStoreSendReceiptDetailsList[i].QtyIn, obj.invStoreSendReceiptDetailsList[i].ItemRate, obj.invStoreSendReceiptDetailsList[i].ItemAmount);
				}
				GrdDetail.DataSource = dt;
				GrdDetail.RetrieveStructure();
				GridEX_Helper.GridWrappingAndColumnSettings(GrdDetail, 2, 3);
				GrdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
				GrdDetail.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
				GrdDetail.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
				GrdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
				GrdDetail.RootTable.Columns["AvgRate"].Width = Constants.InventoryConstants.ExRateConstant;
				GrdDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
				ctrlGrdBar3_Load(null, null);
			}
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
			if (RecId > 0)
			{
				GeneratePrint(RecId);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratePrint(int PrintId)
	{
		try
		{
			CommonServices.StoreSendReceipt_Slip465(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StoreSendReceipt_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnGenerateItemNameFromCurrentPosition_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = Grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int CurrRowItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				GridEXRow[] rows = Grd.GetRows();
				foreach (GridEXRow row in rows)
				{
					row.BeginEdit();
					if (row.RowIndex > item.RowIndex && Conversion.ToInt(row.Cells["ItemId"].Value) == 0)
					{
						row.Cells["ItemId"].Value = CurrRowItemId;
					}
					double ItemQty = Conversion.ToDouble(row.Cells["ItemQty"].Value);
					int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
					int ItemConditionId = Conversion.ToInt(row.Cells["ItemConditionId"].Value);
					double rate = 0.0;
					DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(itemId, Conversion.ToDateTime((object)DocDate.Value), ItemConditionId, RecId, (RecId > 0) ? 177 : 0);
					if (dtRate.Rows.Count > 0)
					{
						rate = Math.Round(Conversion.ToDouble(dtRate.Rows[0]["AvgRate"]), 3);
					}
					row.Cells["AvgRate"].Value = rate;
					row.Cells["Amount"].Value = ItemQty * rate;
					row.EndEdit();
				}
			}
			Grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvgRateUpdateOnDocDateChange()
	{
		try
		{
			if (Grd.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = Grd.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				double ItemQty = Conversion.ToDouble(obj.Cells["ItemQty"].Value);
				double rate = 0.0;
				DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(Conversion.ToInt(obj.Cells["ItemId"].Value), ItemConditionId: Conversion.ToInt(obj.Cells["ItemConditionId"].Value), DocDate: Conversion.ToDateTime((object)DocDate.Value), RecId: RecId, DocumentTypeId: (RecId > 0) ? 177 : 0);
				if (dtRate.Rows.Count > 0)
				{
					rate = Math.Round(Conversion.ToDouble(dtRate.Rows[0]["AvgRate"]), 3);
				}
				obj.BeginEdit();
				obj.Cells["AvgRate"].Value = rate;
				obj.Cells["Amount"].Value = ItemQty * rate;
				obj.EndEdit();
			}
			Grd.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Grd.GetRows().Count() > 0)
			{
				AvgRateUpdateOnDocDateChange();
			}
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
			GridHistoryBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbSenderWarehouseHistory).Text = string.Empty;
			((Control)(object)CmbReceiverWarehouseHistory).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindForHistory();
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
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Expected O, but got Unknown
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Expected O, but got Unknown
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Expected O, but got Unknown
		//IL_2039: Unknown result type (might be due to invalid IL or missing references)
		//IL_2043: Expected O, but got Unknown
		//IL_2067: Unknown result type (might be due to invalid IL or missing references)
		//IL_2071: Expected O, but got Unknown
		GridEXLayout Grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PackingMaterial_Store.StoreSendReceipt));
		GridEXLayout GrdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.Grd = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbReceiverWareHouse = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbSenderWareHouse = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.BtnLoader = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateItemNameFromCurrentPosition = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.GrdHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbSenderWarehouseHistory = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbReceiverWarehouseHistory = new UltraCombo();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).BeginInit();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSenderWareHouse).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSenderWarehouseHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverWarehouseHistory).BeginInit();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1002, 653);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.Grd);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(994, 627);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		((ScrollableBase)this.Grd).BackColor = System.Drawing.Color.White;
		Grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.Grd.DesignTimeLayout = Grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.Grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.Grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.Grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.Grd).Location = new System.Drawing.Point(3, 147);
		((System.Windows.Forms.Control)(object)this.Grd).Name = "Grd";
		((ScrollableBase)this.Grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.Grd).Size = new System.Drawing.Size(988, 477);
		((System.Windows.Forms.Control)(object)this.Grd).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.Grd).Tag = "GrdDetail";
		this.Grd.TotalRow = (InheritableBoolean)1;
		this.Grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.Grd).VisualStyle = (VisualStyle)4;
		this.Grd.CellUpdated += new ColumnActionEventHandler(Grd_CellUpdated);
		this.Grd.ColumnButtonClick += new ColumnActionEventHandler(Grd_ColumnButtonClick);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 123);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(988, 24);
		this.panel1.TabIndex = 5;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(948, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.Grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(58, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReceiverWareHouse);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSenderWareHouse);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.ChkPrint);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 60);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(988, 63);
		this.panel4.TabIndex = 4;
		this.CmbReceiverWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbReceiverWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReceiverWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReceiverWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReceiverWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReceiverWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReceiverWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReceiverWareHouse).Location = new System.Drawing.Point(312, 31);
		((System.Windows.Forms.Control)(object)this.CmbReceiverWareHouse).Name = "CmbReceiverWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbReceiverWareHouse).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbReceiverWareHouse).TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(195, 37);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(116, 15);
		this.label3.TabIndex = 55;
		this.label3.Text = "Receiver WareHouse";
		this.CmbSenderWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbSenderWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSenderWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSenderWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSenderWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSenderWareHouse).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbSenderWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSenderWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSenderWareHouse).Location = new System.Drawing.Point(312, 3);
		((System.Windows.Forms.Control)(object)this.CmbSenderWareHouse).Name = "CmbSenderWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbSenderWareHouse).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbSenderWareHouse).TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(195, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(109, 15);
		this.label4.TabIndex = 53;
		this.label4.Text = "Sender WareHouse";
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.Location = new System.Drawing.Point(532, 36);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(79, 19);
		this.ChkPrint.TabIndex = 4;
		this.ChkPrint.Text = "Is Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(70, 33);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(119, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.Leave += new System.EventHandler(DocDate_ValueChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 37);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(57, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc Date";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(70, 5);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(119, 23);
		this.txtdocno.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(988, 30);
		this.panel3.TabIndex = 3;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(7, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(266, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Packing Material Send To Store";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnsave, this.btnupdate, this.btnRefresh, this.btnattachment, this.BtnLoader, this.btnPrint, this.btnGenerateItemNameFromCurrentPosition });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(988, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.BtnLoader.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnLoader.Image = (System.Drawing.Image)resources.GetObject("BtnLoader.Image");
		this.BtnLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoader.Name = "BtnLoader";
		this.BtnLoader.Size = new System.Drawing.Size(112, 24);
		this.BtnLoader.Text = "Load EmptyBag";
		this.BtnLoader.Click += new System.EventHandler(BtnLoader_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "465-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnGenerateItemNameFromCurrentPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnGenerateItemNameFromCurrentPosition.Image = (System.Drawing.Image)resources.GetObject("btnGenerateItemNameFromCurrentPosition.Image");
		this.btnGenerateItemNameFromCurrentPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateItemNameFromCurrentPosition.Name = "btnGenerateItemNameFromCurrentPosition";
		this.btnGenerateItemNameFromCurrentPosition.Size = new System.Drawing.Size(71, 24);
		this.btnGenerateItemNameFromCurrentPosition.Text = "Generate";
		this.btnGenerateItemNameFromCurrentPosition.Click += new System.EventHandler(btnGenerateItemNameFromCurrentPosition_Click);
		this.tabPage2.BackColor = System.Drawing.Color.White;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDetail);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Controls.Add(this.toolStrip2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(994, 627);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.GrdDetail.AllowEdit = (InheritableBoolean)2;
		this.GrdDetail.AutomaticSort = false;
		((ScrollableBase)this.GrdDetail).BackColor = System.Drawing.Color.White;
		GrdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDetail.DesignTimeLayout = GrdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Location = new System.Drawing.Point(3, 466);
		((System.Windows.Forms.Control)(object)this.GrdDetail).Name = "GrdDetail";
		((ScrollableBase)this.GrdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Size = new System.Drawing.Size(988, 158);
		((System.Windows.Forms.Control)(object)this.GrdDetail).TabIndex = 9;
		this.GrdDetail.TotalRow = (InheritableBoolean)1;
		this.GrdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDetail).VisualStyle = (VisualStyle)4;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 441);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(988, 25);
		this.panel5.TabIndex = 8;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(948, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GrdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(4, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 19);
		this.label6.TabIndex = 0;
		this.label6.Text = "Detail";
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		this.GrdHistory.AutomaticSort = false;
		((ScrollableBase)this.GrdHistory).BackColor = System.Drawing.Color.White;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(3, 129);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(988, 312);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 7;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 30);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(988, 25);
		this.panel2.TabIndex = 4;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(948, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(329, 19);
		this.label2.TabIndex = 0;
		this.label2.Text = "Packing Material Send To Store History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(988, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.panel8.Controls.Add(this.groupBox3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 55);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(988, 74);
		this.panel8.TabIndex = 27;
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSenderWarehouseHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReceiverWarehouseHistory);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(830, 69);
		this.groupBox3.TabIndex = 24;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(767, 35);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(351, 17);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(64, 15);
		this.label60.TabIndex = 141;
		this.label60.Text = "To Doc No";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(271, 17);
		this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(79, 15);
		this.label61.TabIndex = 140;
		this.label61.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(139, 17);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(5, 17);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(351, 37);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(81, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(271, 37);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(79, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(139, 37);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 37);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(131, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(434, 17);
		this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(107, 15);
		this.label16.TabIndex = 150;
		this.label16.Text = "Sender Warehouse";
		this.CmbSenderWarehouseHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbSenderWarehouseHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSenderWarehouseHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSenderWarehouseHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSenderWarehouseHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSenderWarehouseHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSenderWarehouseHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSenderWarehouseHistory).Location = new System.Drawing.Point(434, 35);
		((System.Windows.Forms.Control)(object)this.CmbSenderWarehouseHistory).Name = "CmbSenderWarehouseHistory";
		((System.Windows.Forms.Control)(object)this.CmbSenderWarehouseHistory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbSenderWarehouseHistory).TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(600, 17);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(114, 15);
		this.label7.TabIndex = 152;
		this.label7.Text = "Receiver Warehouse";
		this.CmbReceiverWarehouseHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbReceiverWarehouseHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReceiverWarehouseHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReceiverWarehouseHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReceiverWarehouseHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReceiverWarehouseHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReceiverWarehouseHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReceiverWarehouseHistory).Location = new System.Drawing.Point(600, 35);
		((System.Windows.Forms.Control)(object)this.CmbReceiverWarehouseHistory).Name = "CmbReceiverWarehouseHistory";
		((System.Windows.Forms.Control)(object)this.CmbReceiverWarehouseHistory).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbReceiverWarehouseHistory).TabIndex = 5;
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(68, 24);
		this.btnResetHistory.Text = "Reset";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1002, 653);
		base.Controls.Add(this.tabControl1);
		base.Name = "StoreSendReceipt";
		this.Text = "Store SendReceipt";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StoreSendReceipt_FormClosing);
		base.Load += new System.EventHandler(StoreSendReceipt_Load);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.Grd).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSenderWareHouse).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSenderWarehouseHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReceiverWarehouseHistory).EndInit();
		base.ResumeLayout(false);
	}
}
