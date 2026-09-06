using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Import;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Import;

public class ImLcOrderSchedule : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private bool UpdateModeMain;

	private bool UpdateModeDetail;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable table = new DataTable();

	private DataTable tableSheduleMain = new DataTable();

	private DataTable tablePendingContracts = new DataTable();

	private List<ImLcOrderScheduleHeader> LstRemoveRecordMain = new List<ImLcOrderScheduleHeader>();

	private List<ImLcOrderSchedulePackingDetail> LstRemoveRecordDetail = new List<ImLcOrderSchedulePackingDetail>();

	private int updateDetailIndex;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel6;

	private Panel panel5;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private GroupBox groupBox1;

	internal UltraCombo cmbItem;

	private Label label6;

	private Label label7;

	private Label label8;

	private TextBox txtNoOfBagsDetail;

	private Label label9;

	private TextBox txtMTonDetail;

	internal GridEX grdPendingContract;

	internal GridEX grd;

	private Panel panelMainhistory;

	private Button btnAdd;

	private Button btnCancel;

	private Button btnGridUpdate;

	internal UltraCombo cmbPackingtype;

	private Panel panel8;

	private Panel panel9;

	private Panel panel4;

	private Panel panel16;

	private Panel panel15;

	internal UltraCombo cmbCropYear;

	internal UltraCombo cmbPackUom;

	private Label label16;

	private Label label17;

	internal UltraCombo cmbAttentiveLoadDateBySchedule;

	private Label label15;

	private Panel panel12;

	internal GridEX grdContractShedule;

	private Panel panel13;

	private Label label14;

	private Panel panel10;

	private Panel panel11;

	private Label label13;

	private Panel panel14;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnLoadAllPendingContracts;

	private Label label12;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label18;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewDetailSchedule;

	private ToolStripButton btnSavedetailSchedule;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private Panel panel7;

	private Panel panel19;

	private Panel panel21;

	private Panel panel20;

	private Panel panel17;

	internal GridEX grdHistoryMainByContract;

	private Panel panel18;

	internal GridEX grdHistoryScheduleDetailByContact;

	private Panel panel23;

	internal GridEX grdHistoryScheduleByContract;

	private Panel panel22;

	private Label label2;

	private ToolStrip ToolstripHistoryContract;

	private ToolStripButton LoadAllgrdContract;

	private Label label1;

	private Label label4;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar6;

	private CtrlGrdBar ctrlGrdBar5;

	private CtrlGrdBar ctrlGrdBar4;

	private TextBox txtMtonOfSelectedDate;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripButton btnattachment;

	private ToolStripButton BtnNewMain;

	private TabPage tabPage3;

	internal GridEX DataGridHistory;

	private Panel panel24;

	private CtrlGrdBar ctrlGrdBar7;

	private Label label5;

	private ToolStrip toolStrip4;

	private ToolStripButton btnLoadAllHistoryNew;

	private ToolStripButton BtnHistoryPrint;

	private Panel panel25;

	private GroupBox groupBox2;

	private Label label10;

	internal UltraCombo cmbPort;

	private DateTimePicker txtdateto;

	private Label label50;

	private Label label11;

	internal UltraCombo cmbSupplierName;

	private DateTimePicker txtdatefrom;

	private Label label19;

	private Label label51;

	internal UltraCombo CmbItemHistory;

	private Button btnShow;

	private Panel panel26;

	private Panel panel27;

	private ToolStripButton btnRefreshHistoryCombos;

	public ImLcOrderSchedule(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmSaleContractSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1400, 700);
			formright = CommonServices.SetRightsValueInRightsObject("ImLcOrderSchedule");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnSavedetailSchedule.Enabled = formright.DoHaveSaveRight;
			BtnHistoryPrint.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar4.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar5.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar5.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar5.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar5.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar5.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar5.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar6.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar6.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar6.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar6.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar6.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar6.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			tabControl1.TabPages.Remove(tabPage2);
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			tablePendingContracts.Columns.Add("Id", typeof(int));
			tablePendingContracts.Columns.Add("ContractNo", typeof(string));
			tablePendingContracts.Columns.Add("ContractDate", typeof(DateTime));
			tablePendingContracts.Columns.Add("NoOfContainers", typeof(double));
			tablePendingContracts.Columns.Add("WeightM_Ton", typeof(double));
			tablePendingContracts.Columns.Add("SupplierCustomerId", typeof(int));
			tablePendingContracts.Columns.Add("SupplierName");
			tablePendingContracts.Columns.Add("DestinationPortId");
			tablePendingContracts.Columns.Add("LastShipmentDate", typeof(DateTime));
			grdPendingContract.DataSource = tablePendingContracts;
			grdPendingContract.RetrieveStructure();
			grdPendingContractSetting();
			tableSheduleMain.Columns.Add("Id", typeof(int));
			tableSheduleMain.Columns.Add("ContractId", typeof(int));
			tableSheduleMain.Columns.Add("SupplierCustomerId", typeof(int));
			tableSheduleMain.Columns.Add("SupplierName", typeof(string));
			tableSheduleMain.Columns.Add("NoOfContainers", typeof(double));
			tableSheduleMain.Columns.Add("WeightM_Ton", typeof(double));
			tableSheduleMain.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
			tableSheduleMain.Columns.Add("DestinationPortId", typeof(int));
			tableSheduleMain.Columns.Add("AttentiveProductionDate", typeof(DateTime));
			tableSheduleMain.Columns.Add("AttentiveInspectionDate", typeof(DateTime));
			grdContractShedule.DataSource = tableSheduleMain;
			grdContractShedule.RetrieveStructure();
			grdContractSheduleSettings();
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("ScheduleId", typeof(int));
			table.Columns.Add("ContractId", typeof(int));
			table.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("Item");
			table.Columns.Add("ItemCode");
			table.Columns.Add("CropYearId", typeof(int));
			table.Columns.Add("CropYear");
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("PackingType");
			table.Columns.Add("NoOfBags", typeof(double));
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackUom");
			table.Columns.Add("WeightM_Ton", typeof(double));
			grd.DataSource = table;
			grd.RetrieveStructure();
			GridDetailSetting();
			CropYearFill();
			ItemPackingTypeFill();
			GetpendingContractsforShipmentSchedule(50);
			ComboBindForHistory();
			((Control)(object)grdPendingContract).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AttentiveLoadDateFillByContractId(int Id)
	{
		try
		{
			((UltraGridBase)cmbAttentiveLoadDateBySchedule).DataSource = null;
			((Control)(object)cmbAttentiveLoadDateBySchedule).Text = "";
			DataTable dt = ImLcOrderScheduleHeader.GetAttentiveLoadDateByLcOrderId(Id);
			if (dt.Rows.Count > 0)
			{
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("ScheduleId", typeof(int));
				dt2.Columns.Add("AttentiveLoadingDate", typeof(string));
				dt2.Columns.Add("ContractId", typeof(int));
				dt2.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dt2.Rows.Add(Conversion.ToInt(dt.Rows[i]["ScheduleId"]), Conversion.ToDateTime(dt.Rows[i]["AttentiveLoadingDate"]).ToShortDateString(), Conversion.ToInt(dt.Rows[i]["ContractId"]));
				}
				if (dt2.Rows.Count > 0)
				{
					DDL.BindDDL(dt2, cmbAttentiveLoadDateBySchedule, "ScheduleId", "AttentiveLoadingDate", "Attentive Loading Date", ZeroIndex: true);
					((UltraGridBase)cmbAttentiveLoadDateBySchedule).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbAttentiveLoadDateBySchedule).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemNameFillByContractId(int Id)
	{
		try
		{
			((UltraGridBase)cmbItem).DataSource = null;
			((Control)(object)cmbItem).Text = "";
			DataTable dt = ImLcOrder.GetImportLcOrderItems(UserAccount.OrganizationId, UserAccount.CompanyId, Id);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			if (dt.Rows.Count > 0)
			{
				dtitem.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, cmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(cmbItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbItem.Value);
				}
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, cmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbItem.Value = ItemId;
				cmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindItemPackUom()
	{
		try
		{
			string Packuom = ((Control)(object)cmbPackUom).Text;
			((Control)(object)cmbPackUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, cmbPackUom, "Id", "UOMCode", "PackUom", ZeroIndex: true);
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)cmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)cmbPackUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)cmbPackUom).Text = string.Empty;
				((UltraGridBase)cmbPackUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindItemPackUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemPackingTypeFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbPackingtype.Value);
			DataTable lst = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtpcktype = new DataTable();
			dtpcktype.Columns.Add("Id");
			dtpcktype.Columns.Add("Description");
			dtpcktype.Clear();
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dtpcktype.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["Description"]);
			}
			if (dtpcktype.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtpcktype, cmbPackingtype, "Id", "Description", "Packing Type", ZeroIndex: true);
			if (id > 0)
			{
				if (dtpcktype.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbPackingtype.Value = id;
				}
				else
				{
					((Control)(object)cmbPackingtype).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
		try
		{
			int id = Conversion.ToInt(cmbCropYear.Value);
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(lst, cmbCropYear, "Id", "CropYear", "Crop Year", ZeroIndex: true);
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbCropYear.Value = id;
				}
				else
				{
					((Control)(object)cmbCropYear).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindForHistory()
	{
		try
		{
			DataTable dtPort = new DataTable();
			dtPort.Columns.Add("Id", typeof(int));
			dtPort.Columns.Add("Name");
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("Name");
			DataTable dt = ImLcOrderScheduleHeader.GetDataForDropDownFromShipmentSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "DestinationPort")
					{
						dtPort.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtPort, cmbPort, "Id", "Name", "Port Name", ZeroIndex: false);
			DDL.BindDDL(dtCustomer, cmbSupplierName, "Id", "Name", "Supplier Name", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemHistory, "Id", "Name", "Item Name", ZeroIndex: false);
			((UltraGridBase)cmbPort).DisplayLayout.Bands[0].Columns[1].Width = 250;
			((UltraGridBase)cmbSupplierName).DisplayLayout.Bands[0].Columns[1].Width = 250;
			((UltraGridBase)CmbItemHistory).DisplayLayout.Bands[0].Columns[1].Width = 250;
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
			FormReset();
			DetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormReset()
	{
		try
		{
			LstRemoveRecordMain.Clear();
			btnsave.Visible = true;
			UpdateModeMain = false;
			grdContractShedule.DataSource = null;
			tableSheduleMain.Clear();
			AddRowInScheduleMainGrid();
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			dtAttachmentsUpload.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailFormReset()
	{
		try
		{
			((Control)(object)cmbAttentiveLoadDateBySchedule).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbPackUom).Text = string.Empty;
			txtNoOfBagsDetail.Text = string.Empty;
			txtMTonDetail.Text = string.Empty;
			LstRemoveRecordDetail.Clear();
			btnAdd.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool DetailFormvalidation()
	{
		if (((Control)(object)cmbAttentiveLoadDateBySchedule).Text == string.Empty || Conversion.ToInt(cmbAttentiveLoadDateBySchedule.Value) == 0)
		{
			MessageBox.Show("Attentive Loading Date is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbAttentiveLoadDateBySchedule.Focus();
			return false;
		}
		if (((Control)(object)cmbItem).Text == string.Empty || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbItem.Focus();
			return false;
		}
		if (((Control)(object)cmbCropYear).Text == string.Empty || Conversion.ToInt(cmbCropYear.Value) == 0)
		{
			MessageBox.Show("CropYear is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbCropYear.Focus();
			return false;
		}
		if (((Control)(object)cmbPackingtype).Text == string.Empty || Conversion.ToInt(cmbPackingtype.Value) == 0)
		{
			MessageBox.Show("Packing Type is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbPackingtype.Focus();
			return false;
		}
		if (txtNoOfBagsDetail.Text == string.Empty || Conversion.ToDouble((object)txtNoOfBagsDetail.Text) == 0.0)
		{
			MessageBox.Show("No Of Bags Feild is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtNoOfBagsDetail.Focus();
			return false;
		}
		if (((Control)(object)cmbPackUom).Text == string.Empty || Conversion.ToInt(cmbPackUom.Value) == 0)
		{
			MessageBox.Show("PackUom is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbPackUom.Focus();
			return false;
		}
		if (txtMTonDetail.Text == string.Empty || Conversion.ToDouble((object)txtMTonDetail.Text) == 0.0)
		{
			MessageBox.Show("Weight/M.Ton Feild is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtMTonDetail.Focus();
			return false;
		}
		return true;
	}

	private void BtnLoadAllPendingContracts_Click(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				GetpendingContractsforShipmentSchedule();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetpendingContractsforShipmentSchedule(int NoOfRecords = 0)
	{
		try
		{
			DataTable dtPendingShipments = ImLcOrderScheduleHeader.GridContactsExistInScheduleOrNot(UserAccount.OrganizationId, UserAccount.CompanyId, 1, NoOfRecords);
			if (dtPendingShipments.Rows.Count > 0)
			{
				tablePendingContracts.Clear();
				for (int i = 0; i < dtPendingShipments.Rows.Count; i++)
				{
					tablePendingContracts.Rows.Add(dtPendingShipments.Rows[i]["ImLcOrderId"], dtPendingShipments.Rows[i]["LcOrderNo"], dtPendingShipments.Rows[i]["LcOrderDate"], dtPendingShipments.Rows[i]["NoOfContainers"], Conversion.ToDouble(dtPendingShipments.Rows[i]["WeightM_Ton"]), dtPendingShipments.Rows[i]["SupplierCustomerId"], dtPendingShipments.Rows[i]["Customer"], dtPendingShipments.Rows[i]["DestinationPortId"], Conversion.ToDateTime(dtPendingShipments.Rows[i]["LastShipmentDate"]).ToString("dd-MMM-yyyy"));
				}
				grdPendingContract.DataSource = tablePendingContracts;
				grdPendingContract.RetrieveStructure();
				grdPendingContractSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingContractSetting()
	{
		try
		{
			grdPendingContract.RootTable.Columns["Id"].Visible = false;
			grdPendingContract.RootTable.Columns["LastShipmentDate"].Visible = false;
			grdPendingContract.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdPendingContract.RootTable.Columns["DestinationPortId"].Visible = false;
			grdPendingContract.RootTable.Columns["ContractNo"].ColumnType = (ColumnType)5;
			grdPendingContract.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			grdPendingContract.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			grdPendingContract.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.##";
			grdPendingContract.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.##";
			grdPendingContract.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grdPendingContract.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grdPendingContract.RootTable.Columns["WeightM_Ton"].FormatString = "#,##0.##";
			grdPendingContract.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdPendingContract);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingContract_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			UpdateModeDetail = false;
			UpdateModeMain = false;
			GridEXRow r = grdPendingContract.CurrentRow;
			if (!(e.Column.Key == "ContractNo"))
			{
				return;
			}
			if (grd.GetRows().Count() > 0)
			{
				if (MessageBox.Show("Are you sure to Reload Contract?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				table.Rows.Clear();
			}
			ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["Id"].Value));
			ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["Id"].Value));
			if (!UpdateModeMain && r != null && (int)r.RowType == 0)
			{
				tableSheduleMain.Clear();
				tableSheduleMain.Rows.Add(0, r.Cells["Id"].Value, r.Cells["SupplierCustomerId"].Value, r.Cells["SupplierName"].Value, r.Cells["NoOfContainers"].Value, r.Cells["WeightM_Ton"].Value, r.Cells["LastShipmentDate"].Value, r.Cells["DestinationPortId"].Value, DateTime.Now, DateTime.Now);
				grdContractShedule.DataSource = tableSheduleMain;
				grdContractShedule.RetrieveStructure();
				grdContractSheduleSettings();
			}
			if (!UpdateModeDetail)
			{
				grd.DataSource = null;
			}
			AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
			ItemNameFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnNewMain_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
			DetailFormReset();
			AT = new Attachment();
			dtAttachmentsUpload.Clear();
			grdContractShedule.DataSource = null;
			grd.DataSource = null;
			GetpendingContractsforShipmentSchedule();
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractSheduleSettings()
	{
		try
		{
			grdContractShedule.RootTable.Columns["Id"].Visible = false;
			grdContractShedule.RootTable.Columns["ContractId"].Visible = false;
			grdContractShedule.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdContractShedule.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["ContractId"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["SupplierName"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			grdContractShedule.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			grdContractShedule.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.##";
			grdContractShedule.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.##";
			grdContractShedule.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grdContractShedule.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grdContractShedule.RootTable.Columns["WeightM_Ton"].FormatString = "#,##0.##";
			grdContractShedule.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.##";
			DataTable dtPort = CommonServices.GetSeaPortForComboServiceBind();
			grdContractShedule.RootTable.Columns["DestinationPortId"].ColumnType = (ColumnType)0;
			grdContractShedule.RootTable.Columns["DestinationPortId"].HasValueList = true;
			grdContractShedule.RootTable.Columns["DestinationPortId"].LimitToList = true;
			grdContractShedule.RootTable.Columns["DestinationPortId"].EditType = (EditType)4;
			grdContractShedule.RootTable.Columns["DestinationPortId"].Caption = "Destination Port";
			grdContractShedule.RootTable.Columns["DestinationPortId"].ValueList.PopulateValueList((IEnumerable)dtPort.DefaultView, "Id", "PortName");
			CommonServices.GridAutoAdjustmentNew(grdContractShedule);
			grdContractShedule.RootTable.Columns.Add("Delete");
			grdContractShedule.RootTable.Columns["Delete"].Key = "Delete";
			grdContractShedule.RootTable.Columns["Delete"].Caption = "X";
			grdContractShedule.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractShedule.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdContractShedule.RootTable.Columns["Delete"].ButtonText = "X";
			grdContractShedule.RootTable.Columns["Delete"].Width = 20;
			grdContractShedule.RootTable.Columns["Delete"].Position = 0;
			grdContractShedule.RootTable.Columns.Add("Add");
			grdContractShedule.RootTable.Columns["Add"].Key = "Add";
			grdContractShedule.RootTable.Columns["Add"].Caption = "+";
			grdContractShedule.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractShedule.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdContractShedule.RootTable.Columns["Add"].ButtonText = "+";
			grdContractShedule.RootTable.Columns["Add"].Width = 20;
			grdContractShedule.RootTable.Columns["Add"].Position = 1;
			grdContractShedule.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractShedule_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdContractShedule.CurrentRow;
				if (r == null || (int)r.RowType != 0 || tableSheduleMain.Rows.Count <= 1)
				{
					MessageBox.Show("You Can Not Delete All rows");
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ImLcOrderScheduleHeader exIm = new ImLcOrderScheduleHeader();
					exIm.Id = Conversion.ToInt(r.Cells["Id"].Value);
					exIm.ImLcOrderId = Conversion.ToInt(r.Cells["ContractId"].Value);
					exIm.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					exIm.NoOfContainer = Conversion.ToInt(r.Cells["NoOfContainers"].Value);
					exIm.NetWeight = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) * 1000m;
					exIm.AttentiveLoadingDate = Conversion.ToDateTime(r.Cells["AttentiveLoadingDate"].Value);
					exIm.DestinationPortId = Conversion.ToInt(r.Cells["DestinationPortId"].Value);
					exIm.AttentiveProductionDate = Conversion.ToDateTime(r.Cells["AttentiveProductionDate"].Value);
					exIm.AttentiveInspectionDate = Conversion.ToDateTime(r.Cells["AttentiveInspectionDate"].Value);
					exIm.EntryDate = DateTime.Now;
					exIm.EntryUserId = UserAccount.ID;
					exIm.ModifyDate = DateTime.Now;
					exIm.ModifyUserId = UserAccount.ID;
					exIm.OrganizationId = UserAccount.OrganizationId;
					exIm.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					exIm.CompanyId = UserAccount.CompanyId;
					exIm.BranchesId = UserAccount.BranchesId;
					exIm.ActionId = 3;
					LstRemoveRecordMain.Add(exIm);
					tableSheduleMain.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grdContractShedule.UpdateData();
			}
			if (e.Column.Key == "Add")
			{
				AddRowInScheduleMainGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AddRowInScheduleMainGrid()
	{
		try
		{
			if (grdContractShedule.GetRows().Count() == 0)
			{
				tableSheduleMain.Rows.Add(0, 0, 0, "", 0, 0, DateTime.Now, 0, DateTime.Now, DateTime.Now);
				grdContractShedule.DataSource = tableSheduleMain;
				grdContractShedule.RetrieveStructure();
				grdContractSheduleSettings();
				return;
			}
			double TotalWeightM_Ton = 0.0;
			double TotalNoOfContainers = 0.0;
			int selectedContractId = Conversion.ToInt(grdContractShedule.CurrentRow.Cells["ContractId"].Value);
			GridEXRow[] rows = grdPendingContract.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				_ = rows[i];
				DataRow[] matchingRows = tablePendingContracts.Select("Id=" + selectedContractId);
				if (matchingRows.Length != 0)
				{
					TotalWeightM_Ton += Conversion.ToDouble(matchingRows[0]["WeightM_Ton"]);
					TotalNoOfContainers += Conversion.ToDouble(matchingRows[0]["NoOfContainers"]);
					break;
				}
			}
			double WeightM_Ton = 0.0;
			double NoOfContainers = 0.0;
			if (grdContractShedule.GetRows().Count() > 0)
			{
				rows = grdContractShedule.GetRows();
				foreach (GridEXRow item in rows)
				{
					NoOfContainers += Conversion.ToDouble(item.Cells["NoOfContainers"].Value);
					WeightM_Ton += Conversion.ToDouble(item.Cells["WeightM_Ton"].Value);
				}
			}
			double GrossWeightM_Ton = TotalWeightM_Ton - WeightM_Ton;
			double GrossNoOfContainers = TotalNoOfContainers - NoOfContainers;
			if (GrossWeightM_Ton > 0.0 || GrossNoOfContainers > 0.0)
			{
				if (GrossWeightM_Ton > 0.0)
				{
					tableSheduleMain.Rows.Add(0, grdContractShedule.CurrentRow.Cells["ContractId"].Value, grdContractShedule.CurrentRow.Cells["SupplierCustomerId"].Value, grdContractShedule.CurrentRow.Cells["SupplierName"].Value, GrossNoOfContainers, GrossWeightM_Ton, grdContractShedule.CurrentRow.Cells["AttentiveLoadingDate"].Value, grdContractShedule.CurrentRow.Cells["DestinationPortId"].Value, grdContractShedule.CurrentRow.Cells["AttentiveProductionDate"].Value, grdContractShedule.CurrentRow.Cells["AttentiveInspectionDate"].Value);
					grdContractShedule.DataSource = tableSheduleMain;
				}
				else
				{
					MessageBox.Show("Please Check Grid WeightM_Ton  and TotalWeightM_Ton of Contract ");
				}
			}
			else
			{
				MessageBox.Show("Please Check Grid WeightM_Ton  and TotalWeightM_Ton of Contract ");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractShedule_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdContractShedule.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				double TotalWeightM_Ton = 0.0;
				double TotalNoOfContainers = 0.0;
				int selectedContractId = Conversion.ToInt(grdContractShedule.CurrentRow.Cells["ContractId"].Value);
				GridEXRow[] rows = grdPendingContract.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					_ = rows[i];
					DataRow[] matchingRows = tablePendingContracts.Select("Id=" + selectedContractId);
					if (matchingRows.Length != 0)
					{
						TotalWeightM_Ton += Conversion.ToDouble(matchingRows[0]["WeightM_Ton"]);
						TotalNoOfContainers += Conversion.ToDouble(matchingRows[0]["NoOfContainers"]);
						break;
					}
				}
				double AllWeightM_TonExceptSelected = (from GridEXRow row in grdContractShedule.GetRows()
					where row != item
					select (double)row.Cells["WeightM_Ton"].Value).Sum();
				double AllNoOfContainersExceptSelected = (from GridEXRow row in grdContractShedule.GetRows()
					where row != item
					select (double)row.Cells["NoOfContainers"].Value).Sum();
				double RegularTotalWeightM_Ton = TotalWeightM_Ton - AllWeightM_TonExceptSelected;
				double RegularTotalNoOfContainers = TotalNoOfContainers - AllNoOfContainersExceptSelected;
				if ((e.Column.Key == "WeightM_Ton" || e.Column.Key == "NoOfContainers") && Conversion.ToDouble(item.Cells["WeightM_Ton"].Value) > 0.0 && Conversion.ToDouble(item.Cells["NoOfContainers"].Value) > 0.0)
				{
					double NoOfContainers = Conversion.ToDouble(item.Cells["NoOfContainers"].Value);
					double WeightM_Ton = Conversion.ToDouble(item.Cells["WeightM_Ton"].Value);
					if (NoOfContainers > RegularTotalNoOfContainers)
					{
						MessageBox.Show("NoOfContainers can not be Greater than total NoOfContainers of this Contract..");
						item.Cells["NoOfContainers"].Value = Math.Round(RegularTotalNoOfContainers, 2);
					}
					else
					{
						item.Cells["NoOfContainers"].Value = Math.Round(NoOfContainers, 2);
					}
					if (WeightM_Ton > RegularTotalWeightM_Ton)
					{
						MessageBox.Show("WeightM_Ton can not be Greater than  total WeightM_Ton of This Contract..");
						item.Cells["WeightM_Ton"].Value = Math.Round(RegularTotalWeightM_Ton, 2);
					}
					else
					{
						item.Cells["WeightM_Ton"].Value = Math.Round(WeightM_Ton, 2);
					}
				}
				_ = e.Column.Key == "AttentiveLoadingDate";
			}
			grdContractShedule.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			InsertScheduleMain();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InsertScheduleMain()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		try
		{
			bool Update = false;
			GridEXRow[] rows = grdContractShedule.GetRows();
			for (int l = 0; l < rows.Length; l++)
			{
				if (Conversion.ToInt(rows[l].Cells["Id"].Value) > 0)
				{
					Update = true;
					break;
				}
			}
			ImLcOrderScheduleHeader obj = new ImLcOrderScheduleHeader();
			if (Update)
			{
				if (MessageBox.Show("Are you sure to Update Main Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save Main Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if ((from row in ((DataTable)grdContractShedule.DataSource).AsEnumerable()
				group row by (DateTime)row["AttentiveLoadingDate"] into @group
				where @group.Count() > 1
				select @group.Key).Any())
			{
				MessageBox.Show("The Grid Contact Schedule have more than 1 rows of same Attentive Loading date.Please check...");
				return;
			}
			obj.EntryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.ImLcOrderScheduleHeaderList = new List<ImLcOrderScheduleHeader>();
			if (LstRemoveRecordMain.Count > 0)
			{
				for (int j = 0; j < LstRemoveRecordMain.Count; j++)
				{
					obj.ImLcOrderScheduleHeaderList.Add(LstRemoveRecordMain[j]);
				}
			}
			if (grdContractShedule.RowCount > 0)
			{
				rows = grdContractShedule.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r.Cells["AttentiveLoadingDate"].Value)))
					{
						MessageBox.Show("AttentiveLoadingDate cannot be null in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					ImLcOrderScheduleHeader exIm = new ImLcOrderScheduleHeader();
					exIm.Id = Conversion.ToInt(r.Cells["Id"].Value);
					exIm.ImLcOrderId = Conversion.ToInt(r.Cells["ContractId"].Value);
					exIm.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					if (Conversion.ToInt(r.Cells["NoOfContainers"].Value) == 0)
					{
						MessageBox.Show("NoOfContainers Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.NoOfContainer = Conversion.ToInt(r.Cells["NoOfContainers"].Value);
					if (Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) == 0m)
					{
						MessageBox.Show("WeightM_Ton Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.NetWeight = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) * 1000m;
					exIm.AttentiveLoadingDate = Conversion.ToDateTime(r.Cells["AttentiveLoadingDate"].Value);
					if (Conversion.ToInt(r.Cells["DestinationPortId"].Value) == 0)
					{
						MessageBox.Show("DestinationPort Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.DestinationPortId = Conversion.ToInt(r.Cells["DestinationPortId"].Value);
					exIm.AttentiveProductionDate = Conversion.ToDateTime(r.Cells["AttentiveProductionDate"].Value);
					exIm.AttentiveInspectionDate = Conversion.ToDateTime(r.Cells["AttentiveInspectionDate"].Value);
					exIm.EntryDate = DateTime.Now;
					exIm.EntryUserId = UserAccount.ID;
					exIm.ModifyDate = DateTime.Now;
					exIm.ModifyUserId = UserAccount.ID;
					exIm.OrganizationId = UserAccount.OrganizationId;
					exIm.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					exIm.CompanyId = UserAccount.CompanyId;
					exIm.BranchesId = UserAccount.BranchesId;
					exIm.ActionId = ((exIm.Id <= 0) ? 1 : 2);
					obj.ImLcOrderScheduleHeaderList.Add(exIm);
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
				{
					DMSAttachments attach = new DMSAttachments();
					attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
					attach.ScreenName = base.Name;
					obj.AttachmentsList.Add(attach);
					if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
					string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
					if (!File.Exists(fileSavePath))
					{
						File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
					}
				}
				ImLcOrderScheduleHeader.Save(obj);
				if (!Update)
				{
					MessageBox.Show("Main Schedule Save SuccessFully ");
					FormReset();
					return;
				}
				MessageBox.Show("Main Schedule Update SuccessFully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int k = 0; k < AT.RemovedAttachmentListInUpdateCase.Count; k++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[k].Path, FileMode.Open, FileAccess.ReadWrite))
						{
							fs.Flush();
							fs.Close();
							fs.Dispose();
						}
						bool fileDeleted = false;
						int attempts = 0;
						while (!fileDeleted && attempts < 5)
						{
							try
							{
								File.Delete(AT.RemovedAttachmentListInUpdateCase[k].Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
						if (!fileDeleted)
						{
							throw new Exception("Unable to delete file. It is in use by another process.");
						}
					}
				}
				FormReset();
			}
			else
			{
				MessageBox.Show("Schedule Grid have no row", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadbyMainScheduleByContractId(int Id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			FormReset();
			ImLcOrderScheduleHeader obj = new ImLcOrderScheduleHeader();
			obj.ImLcOrderScheduleHeaderList = ImLcOrderScheduleHeader.GetScheduleByContractId(Id);
			if (obj.ImLcOrderScheduleHeaderList == null || obj.ImLcOrderScheduleHeaderList.Count <= 0)
			{
				return;
			}
			UpdateModeMain = true;
			tableSheduleMain.Rows.Clear();
			for (int i = 0; i < obj.ImLcOrderScheduleHeaderList.Count; i++)
			{
				tableSheduleMain.Rows.Add(obj.ImLcOrderScheduleHeaderList[i].Id, obj.ImLcOrderScheduleHeaderList[i].ImLcOrderId, obj.ImLcOrderScheduleHeaderList[i].SupplierCustomerId, obj.ImLcOrderScheduleHeaderList[i].CustomerName, obj.ImLcOrderScheduleHeaderList[i].NoOfContainer, obj.ImLcOrderScheduleHeaderList[i].NetWeight / 1000m, obj.ImLcOrderScheduleHeaderList[i].AttentiveLoadingDate, obj.ImLcOrderScheduleHeaderList[i].DestinationPortId, obj.ImLcOrderScheduleHeaderList[i].AttentiveProductionDate, obj.ImLcOrderScheduleHeaderList[i].AttentiveInspectionDate);
			}
			grdContractShedule.DataSource = tableSheduleMain;
			grdContractShedule.RetrieveStructure();
			grdContractSheduleSettings();
			List<DMSAttachments> byID = DMSAttachments.GetByID(Id, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetTotalMTonOfDate(int ContractId, int ScheduleId)
	{
		try
		{
			double ScheduleMTon = 0.0;
			DataTable Data = ImLcOrderScheduleHeader.GetNoOfContainersByScheduleAndLcOrderId(ContractId, ScheduleId);
			if (Data.Rows.Count > 0)
			{
				ScheduleMTon = Conversion.ToDouble(Data.Rows[0]["NetWeight"]) / 1000.0;
				Conversion.ToDouble(Data.Rows[0]["NoOfContainer"]);
				txtMtonOfSelectedDate.Text = ScheduleMTon.ToString();
			}
			else
			{
				txtMtonOfSelectedDate.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!DetailFormvalidation())
			{
				return;
			}
			DateTime targetDate = Conversion.ToDateTime((object)((Control)(object)cmbAttentiveLoadDateBySchedule).Text);
			double ScheduleMTon = Conversion.ToDouble((object)txtMtonOfSelectedDate.Text);
			double grdWeightM_Ton = 0.0;
			if (grd.GetDataRows().Length == 0)
			{
				if (Conversion.ToDouble((object)txtMTonDetail.Text) > ScheduleMTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract against Date " + targetDate.ToString() + ".Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			else if (grd.GetDataRows().Length != 0)
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow row in dataRows)
				{
					if (Conversion.ToDateTime(row.Cells["AttentiveLoadingDate"].Value).ToShortDateString() == targetDate.ToShortDateString())
					{
						grdWeightM_Ton += Conversion.ToDouble(row.Cells["WeightM_Ton"].Value);
					}
				}
				if (Conversion.ToDouble((object)txtMTonDetail.Text) + grdWeightM_Ton > ScheduleMTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract against Date " + targetDate.ToString() + ".Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			table.Rows.Add(Conversion.ToInt((object)0), Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleId"].Value), Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ContractId"].Value), Conversion.ToDateTime(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["AttentiveLoadingDate"].Value), Conversion.ToInt(cmbItem.Value), Conversion.ToString((object)((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Text), Conversion.ToString((object)((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Text), Conversion.ToInt(cmbCropYear.Value), Conversion.ToString((object)((Control)(object)cmbCropYear).Text), Conversion.ToInt(cmbPackingtype.Value), Conversion.ToString((object)((Control)(object)cmbPackingtype).Text), Conversion.ToDouble((object)txtNoOfBagsDetail.Text), Conversion.ToInt(cmbPackUom.Value), Conversion.ToString((object)((Control)(object)cmbPackUom).Text), Conversion.ToDouble((object)txtMTonDetail.Text));
			grd.DataSource = table;
			grd.RetrieveStructure();
			GridDetailSetting();
			DetailFormReset();
			cmbAttentiveLoadDateBySchedule.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbAttentiveLoadDateBySchedule.Value = item.Cells["ScheduleId"].Value.ToString();
				cmbItem.Value = item.Cells["ItemId"].Text.ToString();
				bindItemPackUom();
				cmbCropYear.Value = item.Cells["CropYearId"].Text.ToString();
				cmbPackingtype.Value = item.Cells["PackingTypeId"].Text.ToString();
				txtNoOfBagsDetail.Text = item.Cells["NoOfBags"].Text.ToString();
				cmbPackUom.Value = item.Cells["PackUomId"].Text.ToString();
				txtMTonDetail.Text = item.Cells["WeightM_Ton"].Text.ToString();
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
				btnAdd.Visible = false;
				cmbAttentiveLoadDateBySchedule.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnGridUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (!DetailFormvalidation())
			{
				return;
			}
			DateTime targetDate = Conversion.ToDateTime((object)((Control)(object)cmbAttentiveLoadDateBySchedule).Text);
			double grdWeightM_Ton = 0.0;
			double ScheduleMTon = Conversion.ToDouble((object)txtMtonOfSelectedDate.Text);
			if (grd.GetDataRows().Length == 0)
			{
				if (Conversion.ToDouble((object)txtMTonDetail.Text) > ScheduleMTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract.Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			else if (grd.GetDataRows().Length != 0)
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow row in dataRows)
				{
					if (Conversion.ToDateTime(row.Cells["AttentiveLoadingDate"].Value).ToShortDateString() == targetDate.ToShortDateString() && row.RowIndex != updateDetailIndex)
					{
						grdWeightM_Ton += Conversion.ToDouble(row.Cells["WeightM_Ton"].Value);
					}
				}
				if (Conversion.ToDouble((object)txtMTonDetail.Text) + grdWeightM_Ton > ScheduleMTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract.Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			table.Rows[updateDetailIndex]["ScheduleId"] = Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleId"].Value);
			table.Rows[updateDetailIndex]["ContractId"] = Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ContractId"].Value);
			table.Rows[updateDetailIndex]["AttentiveLoadingDate"] = Conversion.ToDateTime(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["AttentiveLoadingDate"].Value);
			table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItem.Value);
			table.Rows[updateDetailIndex]["Item"] = Conversion.ToString((object)((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Text);
			table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString((object)((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Text);
			table.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(cmbCropYear.Value);
			table.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)cmbCropYear).Text);
			table.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(cmbPackingtype.Value);
			table.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)cmbPackingtype).Text);
			table.Rows[updateDetailIndex]["NoOfBags"] = Conversion.ToDouble((object)txtNoOfBagsDetail.Text);
			table.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(cmbPackUom.Value);
			table.Rows[updateDetailIndex]["PackUom"] = Conversion.ToString((object)((Control)(object)cmbPackUom).Text);
			table.Rows[updateDetailIndex]["WeightM_Ton"] = Conversion.ToDouble((object)txtMTonDetail.Text);
			DetailFormReset();
			cmbAttentiveLoadDateBySchedule.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			DetailFormReset();
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
			grd.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NoOfBags"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightM_Ton"].FormatString = "#,##0.##";
			grd.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.##";
			grd.RootTable.Groups.Add("AttentiveLoadingDate");
			grd.RootTable.Columns["AttentiveLoadingDate"].Visible = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ScheduleId"].Visible = false;
			grd.RootTable.Columns["ContractId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ImLcOrderSchedulePackingDetail exIm = new ImLcOrderSchedulePackingDetail();
					exIm.Id = Conversion.ToInt(r.Cells["Id"].Value);
					exIm.ImLcOrderId = Conversion.ToInt(r.Cells["ContractId"].Value);
					exIm.ImLcOrderScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					exIm.ImItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					exIm.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					exIm.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					exIm.NoOfBags = Conversion.ToInt(r.Cells["NoOfBags"].Value);
					exIm.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					exIm.NetWeight = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) * 1000m;
					exIm.ActionTypeId = 3;
					LstRemoveRecordDetail.Add(exIm);
					table.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbAttentiveLoadDateBySchedule_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbAttentiveLoadDateBySchedule).ActiveRow != null)
			{
				GetTotalMTonOfDate(Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ContractId"].Value), Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleId"].Value));
			}
			else
			{
				txtMtonOfSelectedDate.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateMtonDetail()
	{
		try
		{
			if (cmbPackUom != null && Conversion.ToInt(cmbPackUom.Value) != 0)
			{
				double NoOfbags = Conversion.ToDouble((object)txtNoOfBagsDetail.Text);
				double Equivalent = Conversion.ToDouble(((UltraDropDownBase)cmbPackUom).SelectedRow.Cells[2].Value);
				double weightMton = Conversion.ToDouble((object)txtMTonDetail.Text);
				if (Conversion.ToString(base.ActiveControl.Tag) == "NoOfBags")
				{
					txtMTonDetail.Text = (NoOfbags * Equivalent / 1000.0).ToString();
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "WeightMton")
				{
					txtNoOfBagsDetail.Text = (weightMton * 1000.0 / Equivalent).ToString("#,##0.##");
				}
				else
				{
					txtMTonDetail.Text = (NoOfbags * Equivalent / 1000.0).ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAdd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "rdbtnItemName")
			{
				cmbAttentiveLoadDateBySchedule.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void InsertScheduleDetail()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected O, but got Unknown
		try
		{
			bool Update = false;
			GridEXRow[] rows = grd.GetRows();
			for (int j = 0; j < rows.Length; j++)
			{
				if (Conversion.ToInt(rows[j].Cells["Id"].Value) > 0)
				{
					Update = true;
					break;
				}
			}
			ImLcOrderSchedulePackingDetail obj = new ImLcOrderSchedulePackingDetail();
			if (Update)
			{
				if (MessageBox.Show("Are you sure to Update Detail Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save Detail Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.ImLcOrderSchedulePackingDetailList = new List<ImLcOrderSchedulePackingDetail>();
			if (LstRemoveRecordDetail.Count > 0)
			{
				for (int i = 0; i < LstRemoveRecordDetail.Count; i++)
				{
					obj.ImLcOrderSchedulePackingDetailList.Add(LstRemoveRecordDetail[i]);
				}
			}
			if (grd.RowCount > 0)
			{
				rows = grd.GetDataRows();
				foreach (GridEXRow r in rows)
				{
					ImLcOrderSchedulePackingDetail exIm = new ImLcOrderSchedulePackingDetail();
					exIm.Id = Conversion.ToInt(r.Cells["Id"].Value);
					exIm.ImLcOrderId = Conversion.ToInt(r.Cells["ContractId"].Value);
					exIm.ImLcOrderScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.ImItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (Conversion.ToInt(r.Cells["CropYearId"].Value) == 0)
					{
						MessageBox.Show("CropYear Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					if (Conversion.ToInt(r.Cells["PackingTypeId"].Value) == 0)
					{
						MessageBox.Show("PackingType Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					if (Conversion.ToInt(r.Cells["NoOfBags"].Value) == 0)
					{
						MessageBox.Show("NoOfBags Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.NoOfBags = Conversion.ToInt(r.Cells["NoOfBags"].Value);
					if (Conversion.ToInt(r.Cells["PackUomId"].Value) == 0)
					{
						MessageBox.Show("PackUom Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					if (Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) == 0m)
					{
						MessageBox.Show("WeightM_Ton Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.NetWeight = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) * 1000m;
					exIm.ActionTypeId = ((exIm.Id <= 0) ? 1 : 2);
					obj.ImLcOrderSchedulePackingDetailList.Add(exIm);
				}
				ImLcOrderScheduleHeader.SaveDetail(obj);
				if (!Update)
				{
					MessageBox.Show("Detail Schedule Save SuccessFully");
				}
				else
				{
					MessageBox.Show("Detail Schedule Update SuccessFully");
				}
				DetailFormReset();
				grd.DataSource = null;
			}
			else
			{
				MessageBox.Show("Schedule Grid have no row", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSavedetailSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			InsertScheduleDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNewDetailSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			DetailFormReset();
			grd.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadScheduleDetailByContactId(int Id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			DetailFormReset();
			ImLcOrderSchedulePackingDetail obj = new ImLcOrderSchedulePackingDetail();
			obj.ImLcOrderSchedulePackingDetailList = ImLcOrderScheduleHeader.GetPackingDetailByContractId(Id);
			if (obj.ImLcOrderSchedulePackingDetailList != null && obj.ImLcOrderSchedulePackingDetailList.Count > 0)
			{
				UpdateModeDetail = true;
				table.Rows.Clear();
				for (int i = 0; i < obj.ImLcOrderSchedulePackingDetailList.Count; i++)
				{
					table.Rows.Add(obj.ImLcOrderSchedulePackingDetailList[i].Id, obj.ImLcOrderSchedulePackingDetailList[i].ImLcOrderScheduleId, obj.ImLcOrderSchedulePackingDetailList[i].ImLcOrderId, obj.ImLcOrderSchedulePackingDetailList[i].AttentiveLoadingDate, obj.ImLcOrderSchedulePackingDetailList[i].ImItemId, obj.ImLcOrderSchedulePackingDetailList[i].ItemName, obj.ImLcOrderSchedulePackingDetailList[i].ItemCode, obj.ImLcOrderSchedulePackingDetailList[i].CropYearId, obj.ImLcOrderSchedulePackingDetailList[i].CropYear, obj.ImLcOrderSchedulePackingDetailList[i].PackingTypeId, obj.ImLcOrderSchedulePackingDetailList[i].PackingType, obj.ImLcOrderSchedulePackingDetailList[i].NoOfBags, obj.ImLcOrderSchedulePackingDetailList[i].PackUomId, obj.ImLcOrderSchedulePackingDetailList[i].PackUom, obj.ImLcOrderSchedulePackingDetailList[i].NetWeight / 1000m);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				DataGridHistoryFill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistoryFill(int NoOfRecords = 0)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = txtdatefrom.Value;
			obj.ToDate = txtdateto.Value;
			obj.DestinationPortId = Conversion.ToInt(cmbPort.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value);
			obj.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			dtHistory = ImLcOrderScheduleHeader.ContractSchedule_FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("ImLcOrderId", typeof(int));
				dtcol.Columns.Add("ContractNo", typeof(string));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("NoOfContainer", typeof(int));
				dtcol.Columns.Add("MTon", typeof(decimal));
				dtcol.Columns.Add("DestinationPort", typeof(string));
				dtcol.Columns.Add("LoadingDate", typeof(DateTime));
				dtcol.Columns.Add("ProductionDate", typeof(DateTime));
				dtcol.Columns.Add("InspectionDate", typeof(DateTime));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtHistory.Rows[i]["RecordNo"], dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["ImLcOrderId"], dtHistory.Rows[i]["ContractNo"], dtHistory.Rows[i]["SupplierCustomerId"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["ItemName"], dtHistory.Rows[i]["NoOfContainer"], dtHistory.Rows[i]["MTon"], dtHistory.Rows[i]["DestinationPort"], Conversion.ToDateTime(dtHistory.Rows[i]["AttentiveLoadingDate"]).ToShortDateString(), Conversion.ToDateTime(dtHistory.Rows[i]["AttentiveProductionDate"]).ToShortDateString(), Conversion.ToDateTime(dtHistory.Rows[i]["AttentiveInspectionDate"]).ToShortDateString(), dtHistory.Rows[i]["ScheduleRemarks"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["EntryUser"], dtHistory.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["ImLcOrderId"].Visible = false;
			DataGridHistory.RootTable.Columns["InspectionDate"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["RecordNo"].EditType = (EditType)1;
			DataGridHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ImLcOrderId"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ContractNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["SupplierName"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["NoOfContainer"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["MTon"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ItemName"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DestinationPort"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ProductionDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["LoadingDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["InspectionDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Remarks"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["EntryDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["EntryUser"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["NoOfContainer"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NoOfContainer"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NoOfContainer"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NoOfContainer"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["MTon"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["MTon"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["MTon"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["MTon"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			if (formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			}
			if (formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns.Add("Select");
				DataGridHistory.RootTable.Columns["Select"].ActAsSelector = true;
				DataGridHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
				DataGridHistory.RootTable.Columns["Select"].Width = 40;
				DataGridHistory.RootTable.Columns["RecordNo"].Visible = true;
			}
			if (formright.DoHavePrintRights && formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Select"].Position = 0;
				DataGridHistory.RootTable.Columns["Edit"].Position = 1;
				DataGridHistory.RootTable.Columns["RecordNo"].Position = 2;
				DataGridHistory.FrozenColumns = 1;
			}
			else if (formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns["Select"].Position = 0;
				DataGridHistory.RootTable.Columns["RecordNo"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadAllHistoryNew_Click(object sender, EventArgs e)
	{
		try
		{
			DataGridHistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				tabControl1.SelectedIndex = 0;
				UpdateModeDetail = false;
				UpdateModeMain = false;
				ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["ImLcOrderId"].Value));
				ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["ImLcOrderId"].Value));
				if (!UpdateModeMain)
				{
					grdContractShedule.DataSource = null;
				}
				if (!UpdateModeDetail)
				{
					grd.DataSource = null;
				}
				AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["ImLcOrderId"].Value));
				ItemNameFillByContractId(Conversion.ToInt(r.Cells["ImLcOrderId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["ImLcOrderId"].Value), base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnHistoryPrint_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || DataGridHistory.GetCheckedRows().Length == 0)
			{
				return;
			}
			DataTable dtReport = new DataTable();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RecordNo", typeof(int));
			GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
			foreach (GridEXRow i in checkedRows)
			{
				dt.Rows.Add(Conversion.ToInt(i.Cells["Id"].Value), Conversion.ToInt(i.Cells["RecordNo"].Value));
			}
			if (dt.Rows.Count > 0)
			{
				dtReport = (from historyRow in dtHistory.AsEnumerable()
					join dtRow in dt.AsEnumerable() on historyRow.Field<int>("Id") equals dtRow.Field<int>("Id")
					orderby dtRow.Field<int>("RecordNo")
					select historyRow).CopyToDataTable();
			}
			if (dtReport == null || dtReport.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtReport, "239-ImLcOrderSchedule_FormHistory.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void LoadAllgrdContract_Click(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				GridHistoryMainOfContactsExistInSchedule();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridHistoryMainOfContactsExistInSchedule(int NoOfRecords = 0)
	{
		try
		{
			DataTable dtContactsExistInSchedule = ImLcOrderScheduleHeader.GridContactsExistInScheduleOrNot(UserAccount.OrganizationId, UserAccount.CompanyId, 2, NoOfRecords);
			if (dtContactsExistInSchedule.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ContractNo", typeof(string));
				dt.Columns.Add("ContractDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("NoOfContainers", typeof(double));
				dt.Columns.Add("WeightM_Ton", typeof(double));
				dt.Columns.Add("LastShipmentDate", typeof(DateTime));
				dt.Clear();
				for (int i = 0; i < dtContactsExistInSchedule.Rows.Count; i++)
				{
					dt.Rows.Add(dtContactsExistInSchedule.Rows[i]["ImLcOrderId"], dtContactsExistInSchedule.Rows[i]["LcOrderNo"], dtContactsExistInSchedule.Rows[i]["LcOrderDate"], dtContactsExistInSchedule.Rows[i]["SupCustId"], dtContactsExistInSchedule.Rows[i]["Customer"], dtContactsExistInSchedule.Rows[i]["NoOfContainers"], Conversion.ToDouble(dtContactsExistInSchedule.Rows[i]["WeightM_Ton"]), Conversion.ToDateTime(dtContactsExistInSchedule.Rows[i]["LastShipmentDate"]).ToString("dd-MMM-yyyy"));
				}
				grdHistoryMainByContract.DataSource = dt;
				grdHistoryMainByContract.RetrieveStructure();
				grdHistoryMainByContractSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryMainByContractSetting()
	{
		try
		{
			grdHistoryMainByContract.RootTable.Columns["Id"].Visible = false;
			grdHistoryMainByContract.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistoryMainByContract.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMainByContract.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			grdHistoryMainByContract.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.##";
			grdHistoryMainByContract.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.##";
			grdHistoryMainByContract.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMainByContract.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grdHistoryMainByContract.RootTable.Columns["WeightM_Ton"].FormatString = "#,##0.##";
			grdHistoryMainByContract.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdHistoryMainByContract);
			if (formright.DoHaveUpdateRights)
			{
				grdHistoryMainByContract.RootTable.Columns.Add("Edit");
				grdHistoryMainByContract.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistoryMainByContract.RootTable.Columns["Edit"].Key = "Edit";
				grdHistoryMainByContract.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistoryMainByContract.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistoryMainByContract.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistoryMainByContract.RootTable.Columns["Edit"].Width = 50;
				grdHistoryMainByContract.RootTable.Columns["Edit"].Position = 0;
				grdHistoryMainByContract.FrozenColumns = 1;
			}
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryMainByContract_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMainByContract.CurrentRow;
			tabControl1.SelectedIndex = 0;
			UpdateModeDetail = false;
			UpdateModeMain = false;
			ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["Id"].Value));
			ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["Id"].Value));
			if (!UpdateModeMain && r != null && (int)r.RowType == 0)
			{
				tableSheduleMain.Clear();
				tableSheduleMain.Rows.Add(0, r.Cells["Id"].Value, r.Cells["SupplierCustomerId"].Value, r.Cells["SupplierName"].Value, r.Cells["NoOfContainers"].Value, r.Cells["WeightM_Ton"].Value, DateTime.Now, 0, DateTime.Now, DateTime.Now);
				grdContractShedule.DataSource = tableSheduleMain;
				grdContractShedule.RetrieveStructure();
				grdContractSheduleSettings();
			}
			if (!UpdateModeDetail)
			{
				grd.DataSource = null;
			}
			AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
			ItemNameFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryMainByContract_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMainByContract.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GridScheduleFillByContactId(Conversion.ToInt(r.Cells["Id"].Value));
				GridShedulePackingDetailFillByContactID(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridScheduleFillByContactId(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ImLcOrderScheduleHeader obj = new ImLcOrderScheduleHeader();
			obj.ImLcOrderScheduleHeaderList = ImLcOrderScheduleHeader.GetScheduleByContractId(Id);
			if (obj.ImLcOrderScheduleHeaderList != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("ContactId", typeof(int));
				dtcol.Columns.Add("ContactNo", typeof(string));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("ScheduleCode", typeof(string));
				dtcol.Columns.Add("NoOfContainer", typeof(int));
				dtcol.Columns.Add("NetWeight", typeof(decimal));
				dtcol.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
				dtcol.Columns.Add("DestinationPort", typeof(string));
				dtcol.Columns.Add("AttentiveProductionDate", typeof(DateTime));
				dtcol.Columns.Add("AttentiveInspectionDate", typeof(DateTime));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < obj.ImLcOrderScheduleHeaderList.Count; i++)
				{
					dtcol.Rows.Add(obj.ImLcOrderScheduleHeaderList[i].Id, obj.ImLcOrderScheduleHeaderList[i].ImLcOrderId, obj.ImLcOrderScheduleHeaderList[i].LcOrderNo, obj.ImLcOrderScheduleHeaderList[i].SupplierCustomerId, obj.ImLcOrderScheduleHeaderList[i].CustomerName, obj.ImLcOrderScheduleHeaderList[i].ScheduleCode, obj.ImLcOrderScheduleHeaderList[i].NoOfContainer, obj.ImLcOrderScheduleHeaderList[i].NetWeight / 1000m, obj.ImLcOrderScheduleHeaderList[i].AttentiveLoadingDate, obj.ImLcOrderScheduleHeaderList[i].DestinationPort, obj.ImLcOrderScheduleHeaderList[i].AttentiveProductionDate, obj.ImLcOrderScheduleHeaderList[i].AttentiveInspectionDate, obj.ImLcOrderScheduleHeaderList[i].EntryDate, obj.ImLcOrderScheduleHeaderList[i].EntryUser, obj.ImLcOrderScheduleHeaderList[i].ModifyDate, obj.ImLcOrderScheduleHeaderList[i].ModifyUser);
				}
				grdHistoryScheduleByContract.DataSource = dtcol;
				grdHistoryScheduleByContract.RetrieveStructure();
				grdHistoryScheduleByContractSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryScheduleByContractSetting()
	{
		try
		{
			grdHistoryScheduleByContract.RootTable.Columns["Id"].Visible = false;
			grdHistoryScheduleByContract.RootTable.Columns["ContactId"].Visible = false;
			grdHistoryScheduleByContract.RootTable.Columns["SupplierCustomerId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdHistoryScheduleByContract);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridShedulePackingDetailFillByContactID(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ImLcOrderSchedulePackingDetail obj = new ImLcOrderSchedulePackingDetail();
			obj.ImLcOrderSchedulePackingDetailList = ImLcOrderScheduleHeader.GetPackingDetailByContractId(Id);
			if (obj.ImLcOrderSchedulePackingDetailList != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("ScheduleId", typeof(int));
				dtcol.Columns.Add("ContractId", typeof(int));
				dtcol.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
				dtcol.Columns.Add("ItemId", typeof(int));
				dtcol.Columns.Add("Item");
				dtcol.Columns.Add("ItemCode");
				dtcol.Columns.Add("CropYear");
				dtcol.Columns.Add("PackingType");
				dtcol.Columns.Add("NoOfBags", typeof(double));
				dtcol.Columns.Add("PackUom");
				dtcol.Columns.Add("WeightM_Ton", typeof(double));
				dtcol.Rows.Clear();
				for (int i = 0; i < obj.ImLcOrderSchedulePackingDetailList.Count; i++)
				{
					dtcol.Rows.Add(obj.ImLcOrderSchedulePackingDetailList[i].Id, obj.ImLcOrderSchedulePackingDetailList[i].ImLcOrderScheduleId, obj.ImLcOrderSchedulePackingDetailList[i].ImLcOrderId, obj.ImLcOrderSchedulePackingDetailList[i].AttentiveLoadingDate, obj.ImLcOrderSchedulePackingDetailList[i].ImItemId, obj.ImLcOrderSchedulePackingDetailList[i].ItemName, obj.ImLcOrderSchedulePackingDetailList[i].ItemCode, obj.ImLcOrderSchedulePackingDetailList[i].CropYear, obj.ImLcOrderSchedulePackingDetailList[i].PackingType, obj.ImLcOrderSchedulePackingDetailList[i].NoOfBags, obj.ImLcOrderSchedulePackingDetailList[i].PackUom, obj.ImLcOrderSchedulePackingDetailList[i].NetWeight / 1000m);
				}
				grdHistoryScheduleDetailByContact.DataSource = dtcol;
				grdHistoryScheduleDetailByContact.RetrieveStructure();
				grdHistoryScheduleDetailByContactSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryScheduleDetailByContactSetting()
	{
		try
		{
			grdHistoryScheduleDetailByContact.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdHistoryScheduleDetailByContact.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdHistoryScheduleDetailByContact.RootTable.Columns["NoOfBags"].FormatString = "0,0";
			grdHistoryScheduleDetailByContact.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grdHistoryScheduleDetailByContact.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grdHistoryScheduleDetailByContact.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grdHistoryScheduleDetailByContact.RootTable.Columns["WeightM_Ton"].FormatString = "0,0";
			grdHistoryScheduleDetailByContact.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.##";
			grdHistoryScheduleDetailByContact.RootTable.Columns["Id"].Visible = false;
			grdHistoryScheduleDetailByContact.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryScheduleDetailByContact.RootTable.Columns["ContractId"].Visible = false;
			grdHistoryScheduleDetailByContact.RootTable.Columns["ScheduleId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdHistoryScheduleDetailByContact);
			ctrlGrdBar6_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingContract).Name);
			if (fs != null)
			{
				grdPendingContract.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdContractShedule).Name);
			if (fs != null)
			{
				grdContractShedule.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryMainByContract).Name);
			if (fs != null)
			{
				grdHistoryMainByContract.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryScheduleByContract).Name);
			if (fs != null)
			{
				grdHistoryScheduleByContract.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryScheduleDetailByContact).Name);
			if (fs != null)
			{
				grdHistoryScheduleDetailByContact.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtNoOfBagsDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateMtonDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmSaleContractSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					((Control)(object)grdPendingContract).Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					if (((Control)(object)grdPendingContract).Focused)
					{
						BtnNewMain_Click(null, null);
					}
					else if (((Control)(object)grdContractShedule).Focused)
					{
						btnnew_Click(null, null);
					}
					else
					{
						btnNewDetailSchedule_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.L)
				{
					BtnLoadAllPendingContracts_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					if (formright.DoHaveSaveRight)
					{
						if (((Control)(object)grd).Focused)
						{
							btnSavedetailSchedule_Click(null, null);
						}
						else if (((Control)(object)grdContractShedule).Focused)
						{
							btnsave_Click(null, null);
						}
					}
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					((Control)(object)grdPendingContract).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbAttentiveLoadDateBySchedule.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdPendingContract).Focused)
					{
						((Control)(object)grdContractShedule).Focus();
					}
					else if (((Control)(object)grdContractShedule).Focused)
					{
						((Control)(object)grd).Focus();
					}
					else
					{
						((Control)(object)grdPendingContract).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistoryCombos_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				LoadAllgrdContract_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.P)
			{
				BtnHistoryPrint_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				txtdatefrom.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				txtdatefrom.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdHistoryMainByContract).Focused)
				{
					((Control)(object)grdHistoryScheduleByContract).Focus();
				}
				else if (((Control)(object)grdHistoryScheduleByContract).Focused)
				{
					((Control)(object)grdHistoryScheduleDetailByContact).Focus();
				}
				else
				{
					((Control)(object)grdHistoryMainByContract).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save of Focused Grid");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New of Focused Grid");
			dt.Rows.Add("Ctrl+L", "For Load All");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on 'Sale Contract for Schedule' Grid");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Right", "For Move focus From One Grid to Next");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Schedule Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date Combo in Detail Box");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingContract_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdPendingContract.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPendingContract.CurrentColumn == null)
			{
				return;
			}
			if (grdPendingContract.CurrentColumn.Key == "ContractNo")
			{
				UpdateModeDetail = false;
				UpdateModeMain = false;
				ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["Id"].Value));
				ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["Id"].Value));
				if (!UpdateModeMain && r != null && (int)r.RowType == 0)
				{
					tableSheduleMain.Clear();
					tableSheduleMain.Rows.Add(0, r.Cells["Id"].Value, r.Cells["SupplierCustomerId"].Value, r.Cells["SupplierName"].Value, r.Cells["NoOfContainers"].Value, r.Cells["WeightM_Ton"].Value, DateTime.Now, 0, DateTime.Now, DateTime.Now);
					grdContractShedule.DataSource = tableSheduleMain;
					grdContractShedule.RetrieveStructure();
					grdContractSheduleSettings();
				}
				if (!UpdateModeDetail)
				{
					grd.DataSource = null;
				}
				AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
				ItemNameFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
			}
			grdPendingContract.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractShedule_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow row = grdContractShedule.CurrentRow;
			if (row == null || (int)row.RowType != 0 || grdContractShedule.CurrentColumn == null)
			{
				return;
			}
			if (grdContractShedule.CurrentColumn.Key == "Delete")
			{
				GridEXRow item = grdContractShedule.CurrentRow;
				if (item == null || (int)item.RowType != 0 || tableSheduleMain.Rows.Count <= 1)
				{
					MessageBox.Show("You Can Not Delete All rows");
					return;
				}
				if (Conversion.ToInt(item.Cells["Id"].Value) != 0)
				{
					MessageBox.Show("You Can Not Delete Save Record");
					return;
				}
				item.Delete();
			}
			if (grdContractShedule.CurrentColumn.Key == "Add")
			{
				AddRowInScheduleMainGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow row = grd.CurrentRow;
				if (row != null && (int)row.RowType == 0 && grd.CurrentColumn != null && grd.CurrentColumn.Key == "Delete")
				{
					GridEXRow item = grd.CurrentRow;
					if (item != null && (int)item.RowType == 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						if (Conversion.ToInt(item.Cells["Id"].Value) != 0)
						{
							MessageBox.Show("You Can Not Delete Saved Records");
							return;
						}
						item.Delete();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryMainByContract_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = grdHistoryMainByContract.CurrentRow;
				if (r != null && (int)r.RowType == 0 && grdHistoryMainByContract.CurrentColumn != null && grdHistoryMainByContract.CurrentColumn.Key == "Edit")
				{
					tabControl1.SelectedIndex = 0;
					UpdateModeDetail = false;
					UpdateModeMain = false;
					ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["Id"].Value));
					ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["Id"].Value));
					if (!UpdateModeMain && r != null && (int)r.RowType == 0)
					{
						tableSheduleMain.Clear();
						tableSheduleMain.Rows.Add(0, r.Cells["Id"].Value, r.Cells["SupplierCustomerId"].Value, r.Cells["CustomerName"].Value, r.Cells["NoOfContainers"].Value, r.Cells["WeightM_Ton"].Value, DateTime.Now, 0, DateTime.Now, DateTime.Now);
						grdContractShedule.DataSource = tableSheduleMain;
						grdContractShedule.RetrieveStructure();
						grdContractSheduleSettings();
					}
					if (!UpdateModeDetail)
					{
						grd.DataSource = null;
					}
					AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
					ItemNameFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdHistoryMainByContract_ColumnButtonClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = DataGridHistory.CurrentRow;
				if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
				{
					if (DataGridHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
					{
						tabControl1.SelectedIndex = 0;
						UpdateModeDetail = false;
						UpdateModeMain = false;
						ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["ImLcOrderId"].Value));
						ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["ImLcOrderId"].Value));
						if (!UpdateModeMain)
						{
							grdContractShedule.DataSource = null;
						}
						if (!UpdateModeDetail)
						{
							grd.DataSource = null;
						}
						AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["ImLcOrderId"].Value));
						ItemNameFillByContractId(Conversion.ToInt(r.Cells["ImLcOrderId"].Value));
					}
					if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
					{
						DataGridHistory_LinkClicked(null, null);
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				DataGridHistory_ColumnButtonClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtMTonDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateMtonDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnRefreshHistoryCombos_Click(object sender, EventArgs e)
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

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			DataGridHistoryFill(50);
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
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Expected O, but got Unknown
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Expected O, but got Unknown
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Expected O, but got Unknown
		//IL_0e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e56: Expected O, but got Unknown
		//IL_3459: Unknown result type (might be due to invalid IL or missing references)
		//IL_3463: Expected O, but got Unknown
		//IL_3470: Unknown result type (might be due to invalid IL or missing references)
		//IL_347a: Expected O, but got Unknown
		//IL_3a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a8c: Expected O, but got Unknown
		//IL_4a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a19: Expected O, but got Unknown
		//IL_50b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_50bb: Expected O, but got Unknown
		//IL_50c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_50d2: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		GridEXLayout grdContractShedule_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingContract_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryScheduleDetailByContact_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryScheduleByContract_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryMainByContract_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.cmbCropYear = new UltraCombo();
		this.cmbPackUom = new UltraCombo();
		this.cmbPackingtype = new UltraCombo();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.txtMTonDetail = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtMtonOfSelectedDate = new System.Windows.Forms.TextBox();
		this.txtNoOfBagsDetail = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbAttentiveLoadDateBySchedule = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewDetailSchedule = new System.Windows.Forms.ToolStripButton();
		this.btnSavedetailSchedule = new System.Windows.Forms.ToolStripButton();
		this.label18 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdContractShedule = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdPendingContract = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label13 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewMain = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadAllPendingContracts = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panelMainhistory = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.grdHistoryScheduleDetailByContact = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.grdHistoryScheduleByContract = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdHistoryMainByContract = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.ToolstripHistoryContract = new System.Windows.Forms.ToolStrip();
		this.LoadAllgrdContract = new System.Windows.Forms.ToolStripButton();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel26 = new System.Windows.Forms.Panel();
		this.panel27 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel24 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbPort = new UltraCombo();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label50 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbSupplierName = new UltraCombo();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label19 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.CmbItemHistory = new UltraCombo();
		this.btnShow = new System.Windows.Forms.Button();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAllHistoryNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistoryCombos = new System.Windows.Forms.ToolStripButton();
		this.BtnHistoryPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAttentiveLoadDateBySchedule).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractShedule).BeginInit();
		this.panel13.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingContract).BeginInit();
		this.panel11.SuspendLayout();
		this.panel14.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panelMainhistory.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryScheduleDetailByContact).BeginInit();
		this.panel23.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryScheduleByContract).BeginInit();
		this.panel22.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryMainByContract).BeginInit();
		this.panel18.SuspendLayout();
		this.ToolstripHistoryContract.SuspendLayout();
		this.panel8.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel26.SuspendLayout();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel24.SuspendLayout();
		this.panel25.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		this.toolStrip4.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1385, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1385, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1377, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1371, 626);
		this.panel2.TabIndex = 0;
		this.panel9.Controls.Add(this.panel4);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 27);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1371, 599);
		this.panel9.TabIndex = 4;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Controls.Add(this.panel16);
		this.panel4.Controls.Add(this.panel15);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(415, 306);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(956, 293);
		this.panel4.TabIndex = 5;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 156);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(956, 137);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.ctrlGrdBar3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 133);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(956, 23);
		this.panel16.TabIndex = 4;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(909, -1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(42, 24);
		this.ctrlGrdBar3.TabIndex = 56;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel15.Controls.Add(this.groupBox1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 50);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(956, 83);
		this.panel15.TabIndex = 3;
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingtype);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnGridUpdate);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtMTonDetail);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtMtonOfSelectedDate);
		this.groupBox1.Controls.Add(this.txtNoOfBagsDetail);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(911, 74);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(275, 19);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 130;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(219, 19);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 129;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(332, 41);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 2;
		this.cmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Location = new System.Drawing.Point(595, 41);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Name = "cmbPackUom";
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TextChanged += new System.EventHandler(txtNoOfBagsDetail_TextChanged);
		this.cmbPackingtype.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Location = new System.Drawing.Point(422, 41);
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Name = "cmbPackingtype";
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Size = new System.Drawing.Size(93, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).TabIndex = 3;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(774, 42);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(35, 25);
		this.btnAdd.TabIndex = 7;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnAdd.Leave += new System.EventHandler(btnAdd_Leave);
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(843, 42);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(63, 25);
		this.btnCancel.TabIndex = 8;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnCancel.Leave += new System.EventHandler(btnAdd_Leave);
		this.btnGridUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnGridUpdate.ForeColor = System.Drawing.Color.White;
		this.btnGridUpdate.Location = new System.Drawing.Point(774, 42);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(66, 25);
		this.btnGridUpdate.TabIndex = 7;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = false;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(676, 21);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(91, 15);
		this.label9.TabIndex = 69;
		this.label9.Text = "Weight / M.Ton";
		this.txtMTonDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMTonDetail.Location = new System.Drawing.Point(679, 43);
		this.txtMTonDetail.Name = "txtMTonDetail";
		this.txtMTonDetail.Size = new System.Drawing.Size(91, 23);
		this.txtMTonDetail.TabIndex = 6;
		this.txtMTonDetail.Tag = "WeightMton";
		this.txtMTonDetail.TextChanged += new System.EventHandler(txtMTonDetail_TextChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(518, 21);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 67;
		this.label8.Text = "No Of Bags";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(332, 21);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(57, 15);
		this.label16.TabIndex = 65;
		this.label16.Text = "Crop Year";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(595, 21);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(61, 15);
		this.label17.TabIndex = 65;
		this.label17.Text = "Pack Uom";
		this.txtMtonOfSelectedDate.Enabled = false;
		this.txtMtonOfSelectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMtonOfSelectedDate.Location = new System.Drawing.Point(774, 13);
		this.txtMtonOfSelectedDate.Name = "txtMtonOfSelectedDate";
		this.txtMtonOfSelectedDate.Size = new System.Drawing.Size(131, 23);
		this.txtMtonOfSelectedDate.TabIndex = 4;
		this.txtMtonOfSelectedDate.WordWrap = false;
		this.txtNoOfBagsDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBagsDetail.Location = new System.Drawing.Point(518, 43);
		this.txtNoOfBagsDetail.Name = "txtNoOfBagsDetail";
		this.txtNoOfBagsDetail.Size = new System.Drawing.Size(74, 23);
		this.txtNoOfBagsDetail.TabIndex = 4;
		this.txtNoOfBagsDetail.Tag = "NoOfBags";
		this.txtNoOfBagsDetail.TextChanged += new System.EventHandler(txtNoOfBagsDetail_TextChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(422, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(77, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Packing Type";
		this.cmbAttentiveLoadDateBySchedule.AllowNull = (DefaultableBoolean)1;
		this.cmbAttentiveLoadDateBySchedule.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAttentiveLoadDateBySchedule.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAttentiveLoadDateBySchedule.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAttentiveLoadDateBySchedule.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).Location = new System.Drawing.Point(3, 41);
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).Name = "cmbAttentiveLoadDateBySchedule";
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).TabIndex = 0;
		this.cmbAttentiveLoadDateBySchedule.ValueChanged += new System.EventHandler(cmbAttentiveLoadDateBySchedule_ValueChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(3, 21);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(129, 15);
		this.label15.TabIndex = 65;
		this.label15.Text = "Attentive Loading Date";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(144, 41);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(144, 21);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(74, 15);
		this.label6.TabIndex = 65;
		this.label6.Text = "Item / Brand";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.toolStrip3);
		this.panel5.Controls.Add(this.label18);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(956, 50);
		this.panel5.TabIndex = 2;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewDetailSchedule, this.btnSavedetailSchedule });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(956, 27);
		this.toolStrip3.TabIndex = 55;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewDetailSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewDetailSchedule.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewDetailSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewDetailSchedule.Name = "btnNewDetailSchedule";
		this.btnNewDetailSchedule.Size = new System.Drawing.Size(59, 24);
		this.btnNewDetailSchedule.Text = "&New";
		this.btnNewDetailSchedule.Click += new System.EventHandler(btnNewDetailSchedule_Click);
		this.btnSavedetailSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSavedetailSchedule.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSavedetailSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSavedetailSchedule.Name = "btnSavedetailSchedule";
		this.btnSavedetailSchedule.Size = new System.Drawing.Size(117, 24);
		this.btnSavedetailSchedule.Text = "&Save/Update";
		this.btnSavedetailSchedule.Click += new System.EventHandler(btnSavedetailSchedule_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(6, 30);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(352, 17);
		this.label18.TabIndex = 54;
		this.label18.Text = "Contract Schedule Brand Detail Loading Date wise";
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractShedule);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(415, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(956, 306);
		this.panel12.TabIndex = 3;
		grdContractShedule_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractShedule.DesignTimeLayout = grdContractShedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdContractShedule.DynamicFiltering = true;
		this.grdContractShedule.FilterMode = (FilterMode)2;
		this.grdContractShedule.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractShedule.GroupByBoxVisible = false;
		this.grdContractShedule.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Location = new System.Drawing.Point(0, 51);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Name = "grdContractShedule";
		((ScrollableBase)this.grdContractShedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Size = new System.Drawing.Size(956, 255);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).TabIndex = 0;
		this.grdContractShedule.TotalRow = (InheritableBoolean)1;
		this.grdContractShedule.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractShedule).VisualStyle = (VisualStyle)4;
		this.grdContractShedule.CellUpdated += new ColumnActionEventHandler(grdContractShedule_CellUpdated);
		this.grdContractShedule.ColumnButtonClick += new ColumnActionEventHandler(grdContractShedule_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractShedule_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar2);
		this.panel13.Controls.Add(this.label14);
		this.panel13.Controls.Add(this.panel3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(956, 51);
		this.panel13.TabIndex = 6;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(907, 28);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdContractShedule;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar2.TabIndex = 56;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 30);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(135, 17);
		this.label14.TabIndex = 54;
		this.label14.Text = "Contract Schedule";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(956, 27);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnsave, this.btnattachment });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(956, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(117, 24);
		this.btnsave.Text = "&Save/Update";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attechment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingContract);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(415, 599);
		this.panel10.TabIndex = 4;
		this.grdPendingContract.AllowEdit = (InheritableBoolean)2;
		grdPendingContract_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingContract.DesignTimeLayout = grdPendingContract_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingContract.DynamicFiltering = true;
		this.grdPendingContract.FilterMode = (FilterMode)2;
		this.grdPendingContract.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingContract.GroupByBoxVisible = false;
		this.grdPendingContract.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Location = new System.Drawing.Point(0, 51);
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Name = "grdPendingContract";
		((ScrollableBase)this.grdPendingContract).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Size = new System.Drawing.Size(415, 548);
		((System.Windows.Forms.Control)(object)this.grdPendingContract).TabIndex = 0;
		this.grdPendingContract.TotalRow = (InheritableBoolean)1;
		this.grdPendingContract.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingContract).VisualStyle = (VisualStyle)4;
		this.grdPendingContract.LinkClicked += new ColumnActionEventHandler(grdPendingContract_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPendingContract).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingContract_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar1);
		this.panel11.Controls.Add(this.label13);
		this.panel11.Controls.Add(this.panel14);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(415, 51);
		this.panel11.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(371, 28);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdPendingContract;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 22);
		this.ctrlGrdBar1.TabIndex = 55;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(5, 30);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(191, 17);
		this.label13.TabIndex = 54;
		this.label13.Text = "Sale Contract for Schedule";
		this.panel14.Controls.Add(this.toolStrip2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(415, 27);
		this.panel14.TabIndex = 1;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.BtnNewMain, this.BtnLoadAllPendingContracts, this.BtnShortCutkeys });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(415, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewMain.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewMain.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewMain.Name = "BtnNewMain";
		this.BtnNewMain.Size = new System.Drawing.Size(59, 24);
		this.BtnNewMain.Text = "&New";
		this.BtnNewMain.Click += new System.EventHandler(BtnNewMain_Click);
		this.BtnLoadAllPendingContracts.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadAllPendingContracts.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadAllPendingContracts.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadAllPendingContracts.Name = "BtnLoadAllPendingContracts";
		this.BtnLoadAllPendingContracts.Size = new System.Drawing.Size(82, 24);
		this.BtnLoadAllPendingContracts.Text = "Load All";
		this.BtnLoadAllPendingContracts.Click += new System.EventHandler(BtnLoadAllPendingContracts_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(106, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label12);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1371, 27);
		this.panel6.TabIndex = 3;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(5, 5);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(258, 19);
		this.label12.TabIndex = 54;
		this.label12.Text = "Import Sale Contract Schedule";
		this.tabPage2.Controls.Add(this.panelMainhistory);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1377, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "HistoryNotInUse";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panelMainhistory.Controls.Add(this.panel7);
		this.panelMainhistory.Controls.Add(this.panel8);
		this.panelMainhistory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMainhistory.Location = new System.Drawing.Point(3, 3);
		this.panelMainhistory.Name = "panelMainhistory";
		this.panelMainhistory.Size = new System.Drawing.Size(1371, 626);
		this.panelMainhistory.TabIndex = 0;
		this.panel7.Controls.Add(this.panel19);
		this.panel7.Controls.Add(this.panel17);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 35);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1371, 591);
		this.panel7.TabIndex = 51;
		this.panel19.Controls.Add(this.panel21);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(525, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(846, 591);
		this.panel19.TabIndex = 52;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact);
		this.panel21.Controls.Add(this.panel23);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 291);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(846, 300);
		this.panel21.TabIndex = 52;
		this.grdHistoryScheduleDetailByContact.AllowEdit = (InheritableBoolean)2;
		grdHistoryScheduleDetailByContact_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryScheduleDetailByContact.DesignTimeLayout = grdHistoryScheduleDetailByContact_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryScheduleDetailByContact.DynamicFiltering = true;
		this.grdHistoryScheduleDetailByContact.FilterMode = (FilterMode)2;
		this.grdHistoryScheduleDetailByContact.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryScheduleDetailByContact.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Name = "grdHistoryScheduleDetailByContact";
		((ScrollableBase)this.grdHistoryScheduleDetailByContact).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Size = new System.Drawing.Size(846, 271);
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).TabIndex = 52;
		this.grdHistoryScheduleDetailByContact.TotalRow = (InheritableBoolean)1;
		this.grdHistoryScheduleDetailByContact.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryScheduleDetailByContact).VisualStyle = (VisualStyle)4;
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlGrdBar6);
		this.panel23.Controls.Add(this.label4);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(846, 29);
		this.panel23.TabIndex = 51;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(793, 3);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(9, 7, 9, 7);
		this.ctrlGrdBar6.MyGrid = this.grdHistoryScheduleDetailByContact;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(49, 24);
		this.ctrlGrdBar6.TabIndex = 56;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(2, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(491, 17);
		this.label4.TabIndex = 55;
		this.label4.Text = "Sale Contract Schedule Packing Detail of Selected Row in History Main ";
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract);
		this.panel20.Controls.Add(this.panel22);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(846, 291);
		this.panel20.TabIndex = 51;
		this.grdHistoryScheduleByContract.AllowEdit = (InheritableBoolean)2;
		grdHistoryScheduleByContract_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryScheduleByContract.DesignTimeLayout = grdHistoryScheduleByContract_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryScheduleByContract.DynamicFiltering = true;
		this.grdHistoryScheduleByContract.FilterMode = (FilterMode)2;
		this.grdHistoryScheduleByContract.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryScheduleByContract.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Name = "grdHistoryScheduleByContract";
		((ScrollableBase)this.grdHistoryScheduleByContract).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Size = new System.Drawing.Size(846, 264);
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).TabIndex = 51;
		this.grdHistoryScheduleByContract.TotalRow = (InheritableBoolean)1;
		this.grdHistoryScheduleByContract.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryScheduleByContract).VisualStyle = (VisualStyle)4;
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.ctrlGrdBar5);
		this.panel22.Controls.Add(this.label2);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(846, 27);
		this.panel22.TabIndex = 50;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(799, 2);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar5.MyGrid = this.grdHistoryScheduleByContract;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar5.TabIndex = 56;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(392, 17);
		this.label2.TabIndex = 55;
		this.label2.Text = "Sale Contract Schedule of Selected Row in History Main ";
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract);
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(525, 591);
		this.panel17.TabIndex = 51;
		this.grdHistoryMainByContract.AllowEdit = (InheritableBoolean)2;
		grdHistoryMainByContract_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryMainByContract.DesignTimeLayout = grdHistoryMainByContract_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryMainByContract.DynamicFiltering = true;
		this.grdHistoryMainByContract.FilterMode = (FilterMode)2;
		this.grdHistoryMainByContract.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryMainByContract.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Location = new System.Drawing.Point(0, 54);
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Name = "grdHistoryMainByContract";
		((ScrollableBase)this.grdHistoryMainByContract).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Size = new System.Drawing.Size(525, 537);
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).TabIndex = 51;
		this.grdHistoryMainByContract.TotalRow = (InheritableBoolean)1;
		this.grdHistoryMainByContract.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryMainByContract).VisualStyle = (VisualStyle)4;
		this.grdHistoryMainByContract.ColumnButtonClick += new ColumnActionEventHandler(grdHistoryMainByContract_ColumnButtonClick);
		this.grdHistoryMainByContract.SelectionChanged += new System.EventHandler(grdHistoryMainByContract_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistoryMainByContract_KeyDown);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar4);
		this.panel18.Controls.Add(this.label3);
		this.panel18.Controls.Add(this.ToolstripHistoryContract);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(525, 54);
		this.panel18.TabIndex = 2;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(480, 28);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdHistoryMainByContract;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(43, 25);
		this.ctrlGrdBar4.TabIndex = 56;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(6, 30);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(211, 19);
		this.label3.TabIndex = 55;
		this.label3.Text = "History Main By Contract";
		this.ToolstripHistoryContract.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.ToolstripHistoryContract.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.LoadAllgrdContract });
		this.ToolstripHistoryContract.Location = new System.Drawing.Point(0, 0);
		this.ToolstripHistoryContract.Name = "ToolstripHistoryContract";
		this.ToolstripHistoryContract.Size = new System.Drawing.Size(525, 27);
		this.ToolstripHistoryContract.TabIndex = 8;
		this.ToolstripHistoryContract.Text = "toolStrip4";
		this.LoadAllgrdContract.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.LoadAllgrdContract.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.LoadAllgrdContract.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.LoadAllgrdContract.Name = "LoadAllgrdContract";
		this.LoadAllgrdContract.Size = new System.Drawing.Size(82, 24);
		this.LoadAllgrdContract.Text = "Load All";
		this.LoadAllgrdContract.Click += new System.EventHandler(LoadAllgrdContract_Click);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1371, 35);
		this.panel8.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(260, 19);
		this.label1.TabIndex = 55;
		this.label1.Text = "Sale Contract Schedule History";
		this.tabPage3.Controls.Add(this.panel26);
		this.tabPage3.Controls.Add(this.panel25);
		this.tabPage3.Controls.Add(this.toolStrip4);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1377, 632);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel26.Controls.Add(this.panel27);
		this.panel26.Controls.Add(this.panel24);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 112);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1377, 520);
		this.panel26.TabIndex = 55;
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel27.Location = new System.Drawing.Point(0, 27);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1377, 493);
		this.panel27.TabIndex = 54;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1377, 493);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 53;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel24.BackColor = System.Drawing.Color.Teal;
		this.panel24.Controls.Add(this.ctrlGrdBar7);
		this.panel24.Controls.Add(this.label5);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1377, 27);
		this.panel24.TabIndex = 52;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1337, 3);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar7.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 56;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 19);
		this.label5.TabIndex = 55;
		this.label5.Text = "History";
		this.panel25.Controls.Add(this.groupBox2);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel25.Location = new System.Drawing.Point(0, 27);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1377, 85);
		this.panel25.TabIndex = 54;
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPort);
		this.groupBox2.Controls.Add(this.txtdateto);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierName);
		this.groupBox2.Controls.Add(this.txtdatefrom);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(920, 74);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(238, 21);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(29, 15);
		this.label10.TabIndex = 577;
		this.label10.Text = "Port";
		this.cmbPort.AllowNull = (DefaultableBoolean)1;
		this.cmbPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPort).Location = new System.Drawing.Point(238, 38);
		((System.Windows.Forms.Control)(object)this.cmbPort).Name = "cmbPort";
		((System.Windows.Forms.Control)(object)this.cmbPort).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.cmbPort).TabIndex = 2;
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(123, 40);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(114, 23);
		this.txtdateto.TabIndex = 1;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(444, 21);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(86, 15);
		this.label50.TabIndex = 16;
		this.label50.Text = "Supplier Name";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(123, 21);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(48, 15);
		this.label11.TabIndex = 575;
		this.label11.Text = "Date To";
		this.cmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Location = new System.Drawing.Point(442, 38);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Name = "cmbSupplierName";
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).TabIndex = 3;
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(6, 40);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(116, 23);
		this.txtdatefrom.TabIndex = 0;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(655, 21);
		this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(32, 15);
		this.label19.TabIndex = 18;
		this.label19.Text = "Item";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(6, 21);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(63, 15);
		this.label51.TabIndex = 572;
		this.label51.Text = "Date From";
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(655, 38);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 4;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(851, 38);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnLoadAllHistoryNew, this.btnRefreshHistoryCombos, this.BtnHistoryPrint });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1377, 27);
		this.toolStrip4.TabIndex = 8;
		this.toolStrip4.Text = "toolStrip4";
		this.btnLoadAllHistoryNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAllHistoryNew.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAllHistoryNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllHistoryNew.Name = "btnLoadAllHistoryNew";
		this.btnLoadAllHistoryNew.Size = new System.Drawing.Size(82, 24);
		this.btnLoadAllHistoryNew.Text = "Load All";
		this.btnLoadAllHistoryNew.Click += new System.EventHandler(btnLoadAllHistoryNew_Click);
		this.btnRefreshHistoryCombos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistoryCombos.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistoryCombos.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistoryCombos.Name = "btnRefreshHistoryCombos";
		this.btnRefreshHistoryCombos.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistoryCombos.Text = "&Refresh";
		this.btnRefreshHistoryCombos.Click += new System.EventHandler(btnRefreshHistoryCombos_Click);
		this.BtnHistoryPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnHistoryPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnHistoryPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnHistoryPrint.Name = "BtnHistoryPrint";
		this.BtnHistoryPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnHistoryPrint.Text = "239-&Print";
		this.BtnHistoryPrint.Click += new System.EventHandler(BtnHistoryPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1385, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ImLcOrderSchedule";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmSaleContractSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleContractSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAttentiveLoadDateBySchedule).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractShedule).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingContract).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panelMainhistory.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryScheduleDetailByContact).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panel20.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryScheduleByContract).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryMainByContract).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.ToolstripHistoryContract.ResumeLayout(false);
		this.ToolstripHistoryContract.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.tabPage3.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel27.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		this.panel25.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		base.ResumeLayout(false);
	}
}
