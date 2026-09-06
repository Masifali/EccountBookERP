using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class EximShippingBookingInfo : Form
{
	public UserAccount UserAccount = new UserAccount();

	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtDetail = new DataTable();

	private DataTable dtGridHistory = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtContainerTypes = new DataTable();

	private string ScreenName = "EximShippingBookingInfo";

	private bool FormInitialized;

	private bool shiftTabPressed;

	private int RecId;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool _isUpdating;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	internal UltraCombo cmbshippingline;

	private TextBox txtVesselName;

	private Label label50;

	private Label label48;

	internal UltraCombo cmbDestinationPort;

	private Label label2;

	internal UltraCombo cmbExportInvoice;

	private Label label1;

	private TextBox txtVoyageNo;

	private Label label5;

	private Label label4;

	private DateTimePicker txtBookingDate;

	internal UltraCombo cmbClearingAgent;

	private Label label3;

	private TextBox txtFreeDays;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label label9;

	internal UltraCombo cmbForwarder;

	private Label label10;

	private TextBox txtBookingCRO;

	private DateTimePicker txtETD;

	private TextBox txtContrsQty;

	private Label label11;

	private Label label12;

	private DateTimePicker txtETA;

	private ToolStripButton btnAttachment;

	private Label label19;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKey;

	private TextBox txtTransitDays;

	private Label label20;

	private Panel panel5;

	internal GridEX grdContainers;

	private Label label13;

	private DateTimePicker txtcutoffdate;

	private Label label16;

	internal UltraCombo cmbCarierMedium;

	private Label label15;

	private ToolStripButton Lookup;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtBookingRate;

	private Label label17;

	private ToolStripButton btnPrint;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel3;

	private Panel panel16;

	private Label label69;

	private ToolStrip toolStrip3;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripSeparator toolStripSeparator1;

	internal GridEX grdfrm;

	private Panel panel15;

	private Label label92;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel4;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label14;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label22;

	private Label label23;

	internal UltraCombo CmbDestinationPortHistory;

	internal UltraCombo CmbClearingAgentHistory;

	internal UltraCombo CmbInvoiceNoHistory;

	internal UltraCombo CmbShippingLinHistory;

	private Label label24;

	internal UltraCombo CmbForwarderHistory;

	private Label label21;

	private Label label18;

	internal UltraCombo CmbBookingNoHistory;

	private Label label25;

	private Label label27;

	private Label label26;

	private Label label29;

	private Label label28;

	private Label label30;

	private DateTimePicker datFinalEtaDate;

	private TextBox txtRemarks;

	private Label label32;

	private Label label31;

	internal UltraCombo CmbContainerDispatchedAtPort;

	internal UltraCombo CmbContainerReceivedFromPort;

	private Label label33;

	internal UltraCombo CmbDELIVEREDAT;

	public EximShippingBookingInfo(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 800);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("ContainerNo");
			dtDetail.Columns.Add("SealNo");
			dtDetail.Columns.Add("ContainerTypeId", typeof(int));
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
			ScreenName = base.Name;
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			GetConfigurationsFromGlobal();
			dtContainerTypes = CommonServices.StaticColumnsService("ExportContainerTypeForCRO");
			DataTable portData = PortsDbCall();
			DataTable invoiceNoData = InvoiceNoDbCall();
			DataTable carrierMediumData = CarrierMediumDbCall();
			DataTable historyComboData = HistoryComboDbCall();
			return new
			{
				PortData = portData,
				InvoiceNoData = invoiceNoData,
				CarrierMediumData = carrierMediumData,
				HistoryComboData = historyComboData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnsave.Enabled = formrights.DoHaveSaveRight;
				btnupdate.Enabled = formrights.DoHaveUpdateRights;
				btnPrint.Enabled = formrights.DoHavePrintRights;
				ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
				ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
				ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
				InvoiceNoBind(result.InvoiceNoData);
				SupplierDtFillFromGlobal();
				ClearingAgentBind();
				CarrierMediumBind(result.CarrierMediumData);
				PortsBind(result.PortData);
				DeliveredAtBind();
				BindGrids();
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				btnsave.Visible = true;
				btnupdate.Visible = false;
				txtBookingDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ExImExportShipingLineBooking.GetDataForDropDownFromShippingBookingInfo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
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
			Dictionary<string, DataTable> dataTables = new Dictionary<string, DataTable>
			{
				{
					"Customer",
					CreateHistoryTable()
				},
				{
					"InvoiceNo",
					CreateHistoryTable()
				},
				{
					"DestinationPort",
					CreateHistoryTable()
				},
				{
					"ShippingLine",
					CreateHistoryTable()
				},
				{
					"ShippingAgent",
					CreateHistoryTable()
				},
				{
					"Forwarder",
					CreateHistoryTable()
				},
				{
					"BookingCroNo",
					CreateHistoryTable()
				}
			};
			foreach (DataRow row in dt.Rows)
			{
				string activity = Conversion.ToString(row["Activity"]);
				if (dataTables.ContainsKey(activity))
				{
					dataTables[activity].Rows.Add(row["Id"], row["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dataTables["Customer"], "Id", "name", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNoHistory, dataTables["InvoiceNo"], "Id", "name", "Export Invoice No", AllColumns: true, Conversion.ToInt(CmbInvoiceNoHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDestinationPortHistory, dataTables["DestinationPort"], "Id", "name", "Destination Port", AllColumns: true, Conversion.ToInt(CmbDestinationPortHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbShippingLinHistory, dataTables["ShippingLine"], "Id", "name", "Shipping Line", AllColumns: true, Conversion.ToInt(CmbShippingLinHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbClearingAgentHistory, dataTables["ShippingAgent"], "Id", "name", "Clearing Agent", AllColumns: true, Conversion.ToInt(CmbClearingAgentHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbForwarderHistory, dataTables["Forwarder"], "Id", "name", "Forwarder", AllColumns: true, Conversion.ToInt(CmbForwarderHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBookingNoHistory, dataTables["BookingCroNo"], "Id", "name", "Booking/CRO No", AllColumns: true, Conversion.ToInt(CmbBookingNoHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private DataTable CreateHistoryTable()
	{
		return new DataTable
		{
			Columns = { "Id", "name" }
		};
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable InvoiceNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			return ExImInvoice.GetInvoiceNoForBookingInfo(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InvoiceNoBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbExportInvoice, dt, "Id", "InvoiceNo", "Invoice No", AllColumns: true, Conversion.ToInt(cmbExportInvoice.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			IEnumerable<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 10);
			if (!customers.Any())
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				object[] rowData = new object[5] { item.Id, item.CompanyName, item.PartyCode, item.CityName, item.MobilePersonal };
				dtSupplier.Rows.Add(rowData);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ClearingAgentBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbshippingline, dtSupplier, "Id", "CompanyName", "Shipping Line", AllColumns: true, Conversion.ToInt(cmbshippingline.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbForwarder, dtSupplier, "Id", "CompanyName", "Forwarder", AllColumns: true, Conversion.ToInt(cmbForwarder.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbClearingAgent, dtSupplier, "Id", "CompanyName", "Clearing Agent", AllColumns: true, Conversion.ToInt(cmbClearingAgent.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable CarrierMediumDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			return ExImLookUps.GetByLookTypeId(new ExImLookUps
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ExImLookUptypesId = 4
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CarrierMediumBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbCarierMedium, dt, "Id", "LookUpName", "Carrier Medium", AllColumns: false, Conversion.ToInt(cmbCarierMedium.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable PortsDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PortsBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbDestinationPort, dt, "Id", "PortName", "Destination Port", AllColumns: false, Conversion.ToInt(cmbDestinationPort.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbContainerDispatchedAtPort, dt, "Id", "PortName", "Container Dispatched At Port", AllColumns: false, Conversion.ToInt(CmbContainerDispatchedAtPort.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbContainerReceivedFromPort, dt, "Id", "PortName", "Container Received From Port", AllColumns: false, Conversion.ToInt(CmbContainerReceivedFromPort.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveredAtBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			dt.Rows.Add(1, UserAccount.CompName);
			dt.Rows.Add(2, "Other");
			InfragisticsHelper.BindAndRetainSelection(CmbDELIVEREDAT, dt, "Id", "Name", "DELIVERED AT", AllColumns: false, Conversion.ToInt(CmbDELIVEREDAT.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrids()
	{
		try
		{
			if (dtDetail != null && dtDetail.Columns.Count > 0)
			{
				DataTable dataTable = dtDetail;
				if (dataTable != null && dataTable.Rows.Count == 0)
				{
					AddRowInGrid();
				}
				grdContainers.DataSource = dtDetail;
				grdContainers.RetrieveStructure();
				grdSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGrid()
	{
		dtDetail.Rows.Add(0, "", "", 0);
		grdContainers.DataSource = dtDetail;
	}

	private void grdSetting()
	{
		try
		{
			grdContainers.ColumnAutoResize = true;
			List<string> hiddenColumns = new List<string> { "Id" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"ContainerNo",
					(EditType)1
				},
				{
					"SealNo",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdContainers, 2, 3, hiddenColumns, editableColumns);
			GridEX_Helper.AddButton(grdContainers, "Add", "Add", 40, 0);
			GridEX_Helper.ComboBind(grdContainers.RootTable.Columns["ContainerTypeId"], dtContainerTypes, "Id", "type", "Container Type", 100);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContainers_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grdContainers.CurrentRow;
			if (currentRow == null || (int)currentRow.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "ContainerNo")
			{
				string currentContainerNo = currentRow.Cells["ContainerNo"].Value?.ToString()?.Trim();
				if (string.IsNullOrEmpty(currentContainerNo))
				{
					return;
				}
				GridEXRow[] rows = grdContainers.GetRows();
				foreach (GridEXRow row in rows)
				{
					if ((int)row.RowType == 0 && row.RowIndex != currentRow.RowIndex)
					{
						string existingContainerNo = row.Cells["ContainerNo"].Value?.ToString()?.Trim();
						if (string.Equals(currentContainerNo, existingContainerNo, StringComparison.OrdinalIgnoreCase))
						{
							currentRow.BeginEdit();
							currentRow.Cells["ContainerNo"].Value = "";
							currentRow.EndEdit();
							MessageBox.Show("Container# " + existingContainerNo + " already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							break;
						}
					}
				}
			}
			grdContainers.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContainers_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Add")
			{
				if (Conversion.ToInt(cmbExportInvoice.Value) <= 0)
				{
					MessageBox.Show("Please Select Invoice First", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					cmbExportInvoice.Focus();
					return;
				}
				if ((double)grdContainers.GetRows().Count() == Conversion.ToDouble((object)txtContrsQty.Text))
				{
					MessageBox.Show("You Can't Add More Rows", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				AddRowInGrid();
			}
			grdContainers.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtContrsQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdContainers.RowCount > 0 && Conversion.ToInt((object)txtContrsQty.Text) < grdContainers.RowCount)
			{
				_ = grdContainers.RowCount;
				Conversion.ToInt((object)txtContrsQty.Text);
				dtDetail.AsEnumerable().Skip(Conversion.ToInt((object)txtContrsQty.Text)).ToList()
					.ForEach(delegate(DataRow row)
					{
						row.Delete();
					});
				dtDetail.AcceptChanges();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			RecId = 0;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((Control)(object)cmbExportInvoice).Enabled = true;
			txtBookingDate.Value = DateTime.Now;
			txtBookingCRO.Text = string.Empty;
			cmbExportInvoice.Value = 0;
			cmbshippingline.Value = 0;
			cmbClearingAgent.Value = 0;
			cmbCarierMedium.Value = 0;
			cmbForwarder.Value = 0;
			txtVesselName.Text = string.Empty;
			txtVoyageNo.Text = string.Empty;
			txtETA.Value = DateTime.Now;
			txtETD.Value = DateTime.Now;
			txtcutoffdate.Value = DateTime.Now;
			txtTransitDays.Text = "0";
			txtFreeDays.Text = string.Empty;
			txtContrsQty.Text = string.Empty;
			txtBookingRate.Text = string.Empty;
			cmbDestinationPort.Value = 0;
			CmbContainerDispatchedAtPort.Value = 0;
			CmbContainerReceivedFromPort.Value = 0;
			CmbDELIVEREDAT.Value = 0;
			txtRemarks.Text = string.Empty;
			dtDetail.Rows.Clear();
			BindGrids();
			InvoiceNoBind(InvoiceNoDbCall());
			txtBookingDate.Focus();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void EximShippingBookingInfo_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			DatatableHelper.GlobalSupplierCustomerListsFillDbCall(UserAccount.OrganizationId, UserAccount.CompanyId);
			GetConfigurationsFromGlobal();
			InvoiceNoBind(InvoiceNoDbCall());
			SupplierDtFillFromGlobal();
			ClearingAgentBind();
			CarrierMediumBind(CarrierMediumDbCall());
			PortsBind(PortsDbCall());
			DeliveredAtBind();
			dtContainerTypes = CommonServices.StaticColumnsService("ExportContainerTypeForCRO");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRemarks_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!shiftTabPressed)
			{
				txtBookingDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Expected O, but got Unknown
		try
		{
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtBookingCRO, "BookingCRO", FormHelper.ValidType.String),
				((Control)(object)cmbExportInvoice, "Invoice No", FormHelper.ValidType.INT),
				((Control)(object)cmbCarierMedium, "Carrier Medium", FormHelper.ValidType.INT),
				((Control)(object)cmbshippingline, "Shipping Line", FormHelper.ValidType.INT),
				(txtContrsQty, "No of Containers", FormHelper.ValidType.INT),
				(txtFreeDays, "Free Days", FormHelper.ValidType.INT)
			}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			ExImExportShipingLineBooking obj = new ExImExportShipingLineBooking();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.Id = RecId;
			obj.BookingDate = txtBookingDate.Value;
			obj.BookingCroNo = Conversion.ToString((object)txtBookingCRO.Text);
			obj.ExImInvoiceId = Conversion.ToInt(cmbExportInvoice.Value);
			obj.ShippingLineId = Conversion.ToInt(cmbshippingline.Value);
			obj.TransporterId = Conversion.ToInt(cmbForwarder.Value);
			obj.ShippingAgentId = Conversion.ToInt(cmbClearingAgent.Value);
			obj.CarierMediumId = Conversion.ToInt(cmbCarierMedium.Value);
			obj.VesselName = txtVesselName.Text.Trim();
			obj.VoyageNo = txtVoyageNo.Text.Trim();
			obj.ETADate = Conversion.ToDateTime((object)txtETA.Value);
			obj.ETDDate = Conversion.ToDateTime((object)txtETD.Value);
			obj.CuttOFFDate = Conversion.ToDateTime((object)txtcutoffdate.Value);
			obj.TransitDays = Conversion.ToInt((object)txtTransitDays.Text);
			obj.FreeDaysAtDestinations = Conversion.ToInt((object)txtFreeDays.Text);
			obj.PortId = Conversion.ToInt(cmbDestinationPort.Value);
			obj.ContrsQty = Conversion.ToInt((object)txtContrsQty.Text);
			obj.BookingRate = Conversion.ToDecimal((object)txtBookingRate.Text);
			obj.ContainerDispatchedAtPortId = Conversion.ToInt(CmbContainerDispatchedAtPort.Value);
			obj.ContainerReceivedFromPortId = Conversion.ToInt(CmbContainerReceivedFromPort.Value);
			obj.DeliveredAtId = Conversion.ToInt(CmbDELIVEREDAT.Value);
			obj.RemarksHeader = txtRemarks.Text;
			obj.ExImExportShipingLineBookingDetailslist = new List<ExImExportShipingLineBookingDetail>();
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			DataTable detail = (DataTable)grdContainers.DataSource;
			GridEXRow[] rows = grdContainers.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToString(item.Cells["ContainerNo"].Value) != null && Conversion.ToString(item.Cells["ContainerNo"].Value) != string.Empty)
				{
					ExImExportShipingLineBookingDetail d = new ExImExportShipingLineBookingDetail();
					d.Id = Conversion.ToInt(item.Cells["Id"].Value);
					d.Container = item.Cells["ContainerNo"].Value.ToString();
					d.SealNo = Conversion.ToString(item.Cells["SealNo"].Value);
					d.ContainerTypeId = Conversion.ToInt(item.Cells["ContainerTypeId"].Value);
					d.ContainerType = Conversion.ToString((object)item.Cells["ContainerTypeId"].Text);
					obj.ExImExportShipingLineBookingDetailslist.Add(d);
				}
			}
			if (obj.ContrsQty < detail.Rows.Count)
			{
				throw new Exception("Container Rows can not greater than No. of Containers ");
			}
			FormHelper.UpdateAttachmentsForObject<ExImExportShipingLineBooking>(obj, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			ExImExportShipingLineBooking.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 0, base.Name);
			}
			else
			{
				MessageBox.Show("Record Saved Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId not found");
			}
			Insert();
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
			Reset();
			RecId = Id;
			ExImExportShipingLineBooking lineBooking = ExImExportShipingLineBooking.GetByID(RecId);
			if (lineBooking == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnsave.Visible = false;
			btnupdate.Visible = true;
			txtBookingDate.Value = Conversion.ToDateTime((object)lineBooking.BookingDate);
			txtBookingCRO.Text = Conversion.ToString((object)lineBooking.BookingCroNo);
			string InvoiceNo = lineBooking.InvoiceNo;
			if (lineBooking.ExImInvoiceId > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("InvoiceNo");
				dt.Rows.Add(lineBooking.ExImInvoiceId, InvoiceNo);
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbExportInvoice, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
					((UltraGridBase)cmbExportInvoice).DisplayLayout.Bands[0].Columns[1].Width = 350;
					((UltraGridBase)cmbExportInvoice).Rows[0].Activate();
				}
			}
			((Control)(object)cmbExportInvoice).Enabled = false;
			cmbshippingline.Value = Conversion.ToInt((object)lineBooking.ShippingLineId);
			cmbForwarder.Value = Conversion.ToInt((object)lineBooking.TransporterId);
			cmbClearingAgent.Value = Conversion.ToInt((object)lineBooking.ShippingAgentId);
			txtContrsQty.Text = Conversion.ToString((object)lineBooking.ContrsQty);
			cmbCarierMedium.Value = Conversion.ToInt((object)lineBooking.CarierMediumId);
			txtVesselName.Text = Conversion.ToString((object)lineBooking.VesselName);
			txtVoyageNo.Text = Conversion.ToString((object)lineBooking.VoyageNo);
			txtETA.Value = Conversion.ToDateTime((object)lineBooking.ETADate);
			txtETD.Value = Conversion.ToDateTime((object)lineBooking.ETDDate);
			txtcutoffdate.Value = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)lineBooking.CuttOFFDate)) ? DateTime.Now : Conversion.ToDateTime((object)lineBooking.CuttOFFDate));
			txtTransitDays.Text = Conversion.ToString((object)lineBooking.TransitDays);
			cmbDestinationPort.Value = Conversion.ToInt((object)lineBooking.PortId);
			txtFreeDays.Text = Conversion.ToString((object)lineBooking.FreeDaysAtDestinations);
			txtBookingRate.Text = lineBooking.BookingRate.ToString("#,##0.###");
			CmbContainerDispatchedAtPort.Value = Conversion.ToInt((object)lineBooking.ContainerDispatchedAtPortId);
			CmbContainerReceivedFromPort.Value = Conversion.ToInt((object)lineBooking.ContainerReceivedFromPortId);
			CmbDELIVEREDAT.Value = Conversion.ToInt((object)lineBooking.DeliveredAtId);
			txtRemarks.Text = Conversion.ToString((object)lineBooking.RemarksHeader);
			AttachmentsValues = lineBooking.AttachmentsValues;
			CustomAttachmentsValues = lineBooking.CustomAttachmentsValues;
			dtDetail.Rows.Clear();
			FillDetailFromListCommonForReadById(lineBooking, dtDetail);
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
			txtBookingDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(ExImExportShipingLineBooking invGrn, DataTable dt)
	{
		if (invGrn == null || invGrn.ExImExportShipingLineBookingDetailslist == null || invGrn.ExImExportShipingLineBookingDetailslist.Count == 0)
		{
			return;
		}
		foreach (ExImExportShipingLineBookingDetail detail in invGrn.ExImExportShipingLineBookingDetailslist)
		{
			List<object> rowValues = new List<object> { detail.Id, detail.Container, detail.SealNo, detail.ContainerTypeId };
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			txtBookingDate.Focus();
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbClearingAgentHistory).Text = string.Empty;
			((Control)(object)CmbBookingNoHistory).Text = string.Empty;
			((Control)(object)CmbInvoiceNoHistory).Text = string.Empty;
			((Control)(object)CmbDestinationPortHistory).Text = string.Empty;
			grdfrm.DataSource = null;
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
			HistoryComboBind(HistoryComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
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
			Obj.Id = Conversion.ToInt(CmbBookingNoHistory.Value);
			Obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			Obj.ForwarderId = Conversion.ToInt(CmbForwarderHistory.Value);
			Obj.ShippingLineId = Conversion.ToInt(CmbShippingLinHistory.Value);
			Obj.ShippingAgentId = Conversion.ToInt(CmbClearingAgentHistory.Value);
			Obj.ExImInvoiceId = Conversion.ToInt(CmbInvoiceNoHistory.Value);
			Obj.DestinationPortId = Conversion.ToInt(CmbDestinationPortHistory.Value);
			DataTable dtgrd = ExImExportShipingLineBooking.FormHistory(Obj);
			if (dtgrd.Rows.Count > 0)
			{
				if (dtGridHistory.Columns.Count == 0)
				{
					dtGridHistory.Columns.Add("Id", typeof(int));
					dtGridHistory.Columns.Add("BookingDate", typeof(DateTime));
					dtGridHistory.Columns.Add("BookingCroNo", typeof(string));
					dtGridHistory.Columns.Add("InvoiceNo", typeof(string));
					dtGridHistory.Columns.Add("ShippingLine", typeof(string));
					dtGridHistory.Columns.Add("DestinationPort", typeof(string));
					dtGridHistory.Columns.Add("ForwarderName", typeof(string));
					dtGridHistory.Columns.Add("VesselName", typeof(string));
					dtGridHistory.Columns.Add("VoyageNo", typeof(string));
					dtGridHistory.Columns.Add("FreeDays", typeof(int));
					dtGridHistory.Columns.Add("NoOfContainers", typeof(int));
					dtGridHistory.Columns.Add("ClearingAgent", typeof(string));
					dtGridHistory.Columns.Add("ETADate", typeof(DateTime));
					dtGridHistory.Columns.Add("ETDDate", typeof(DateTime));
					dtGridHistory.Columns.Add("NotReferredContainerNos", typeof(string));
					dtGridHistory.Columns.Add("ReferredContainerNos", typeof(string));
					dtGridHistory.Columns.Add("Remarks", typeof(string));
					dtGridHistory.Columns.Add("EntryUser");
					dtGridHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtGridHistory.Columns.Add("ModifyUser");
					dtGridHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtGridHistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtGridHistory.Clear();
				for (int i = 0; i < dtgrd.Rows.Count; i++)
				{
					dtGridHistory.Rows.Add(dtgrd.Rows[i]["Id"], Conversion.ToDateTime(dtgrd.Rows[i]["BookingDate"]).ToShortDateString(), dtgrd.Rows[i]["BookingCroNo"], dtgrd.Rows[i]["InvoiceNo"], dtgrd.Rows[i]["ShippingLineName"], dtgrd.Rows[i]["DestinationPort"], dtgrd.Rows[i]["ForwarderName"], dtgrd.Rows[i]["VesselName"], dtgrd.Rows[i]["VoyageNo"], dtgrd.Rows[i]["FreeDaysAtDestinations"], dtgrd.Rows[i]["ContrsQty"], dtgrd.Rows[i]["ClearingAgentName"], Conversion.ToDateTime(dtgrd.Rows[i]["ETADate"]).ToShortDateString(), Conversion.ToDateTime(dtgrd.Rows[i]["ETDDate"]).ToShortDateString(), dtgrd.Rows[i]["ContainerNos"], dtgrd.Rows[i]["ContainerReferred"], dtgrd.Rows[i]["RemarksHeader"], dtgrd.Rows[i]["EntryUserName"], dtgrd.Rows[i]["EntryDate"], dtgrd.Rows[i]["ModifyUserName"], dtgrd.Rows[i]["ModifyDate"], dtgrd.Rows[i]["NoOfAttachments"]);
				}
				grdfrm.DataSource = dtGridHistory;
				grdfrm.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdfrm.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yy hh:mm tt";
			grdfrm.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yy hh:mm tt";
			grdfrm.RootTable.RowHeight = 30;
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 3);
			grdfrm.RootTable.Columns["InvoiceNo"].WordWrap = true;
			grdfrm.RootTable.Columns["ShippingLine"].WordWrap = true;
			grdfrm.RootTable.Columns["DestinationPort"].WordWrap = true;
			grdfrm.RootTable.Columns["ForwarderName"].WordWrap = true;
			grdfrm.RootTable.Columns["ClearingAgent"].WordWrap = true;
			grdfrm.RootTable.Columns["NotReferredContainerNos"].WordWrap = true;
			grdfrm.RootTable.Columns["ReferredContainerNos"].WordWrap = true;
			grdfrm.RootTable.Columns["BookingDate"].Width = Constants.ExportConstants.DateConstant;
			grdfrm.RootTable.Columns["BookingCroNo"].Width = Constants.ExportConstants.BookingCroNoConstant + 40;
			grdfrm.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdfrm.RootTable.Columns["ShippingLine"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
			grdfrm.RootTable.Columns["ForwarderName"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["VesselName"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["VoyageNo"].Width = Constants.ExportConstants.VoyageNoConstant + 30;
			grdfrm.RootTable.Columns["FreeDays"].Width = Constants.ExportConstants.Days;
			grdfrm.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfContainers;
			grdfrm.RootTable.Columns["ClearingAgent"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["ETADate"].Width = Constants.ExportConstants.DateConstant;
			grdfrm.RootTable.Columns["ETDDate"].Width = Constants.ExportConstants.DateConstant;
			grdfrm.RootTable.Columns["NotReferredContainerNos"].Width = 350;
			grdfrm.RootTable.Columns["ReferredContainerNos"].Width = 350;
			grdfrm.RootTable.Columns["Remarks"].Width = Constants.ExportConstants.RemarksConstants;
			grdfrm.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants + 20;
			grdfrm.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grdfrm.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.EntryUserConstants + 20;
			grdfrm.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grdfrm.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			grdfrm.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.AddButton(grdfrm, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(grdfrm, "Print", "Print", 60, 1);
			grdfrm.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (formrights.DoHaveUpdateRights)
				{
					ReadById(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(r.Cells["Id"].Value), base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (e.Column.Key == "Print")
				{
					GenerateReport(CurrId);
				}
			}
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdfrm, dtGridHistory);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdContainers, dtDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
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

	private void txtTransitDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFreeDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtContrsQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbExportInvoice_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImInvoice.GetData(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Ids = "204,1816",
				Id = Conversion.ToInt(cmbExportInvoice.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtContrsQty.Text = Conversion.ToString(dt.Rows[0]["NoOfContainers"]);
				cmbDestinationPort.Value = Conversion.ToInt(dt.Rows[0]["DestinationPortId"]);
			}
			else
			{
				txtContrsQty.Text = "0";
				((Control)(object)cmbDestinationPort).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GenerateReport(RecId);
				return;
			}
			throw new Exception("No Record found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int id)
	{
		try
		{
			CommonServices.ExBookingInfoSlip560(id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Lookup_Click(object sender, EventArgs e)
	{
		try
		{
			ExImLookups exImLookups = new ExImLookups(UserAccount);
			exImLookups.LookupId = 4;
			exImLookups.StartPosition = FormStartPosition.CenterParent;
			exImLookups.ShowDialog();
			exImLookups.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtETA_ValueChanged(object sender, EventArgs e)
	{
		FinalETADate();
	}

	private void txtTransitDays_TextChanged(object sender, EventArgs e)
	{
		FinalETADate();
	}

	private void FinalETADate()
	{
		if (!_isUpdating)
		{
			_isUpdating = true;
			if (double.TryParse(txtTransitDays.Text.Trim(), out var days))
			{
				datFinalEtaDate.Value = txtETA.Value.AddDays(days);
			}
			else
			{
				datFinalEtaDate.Value = txtETA.Value;
			}
			_isUpdating = false;
		}
	}

	private void datFinalEtaDate_ValueChanged(object sender, EventArgs e)
	{
		if (!_isUpdating)
		{
			_isUpdating = true;
			TimeSpan difference = datFinalEtaDate.Value.Date - txtETA.Value.Date;
			txtTransitDays.Text = ((int)difference.TotalDays).ToString();
			_isUpdating = false;
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Booking Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "When Focus On Container List Add New Row");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowRight", "For Toggle in Container Grid And History Grid");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void frmShippingBookingInfo_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					Lookup_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtBookingDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdContainers).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdContainers).Focused)
					{
						txtBookingDate.Focus();
					}
					else
					{
						((Control)(object)grdContainers).Focus();
					}
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
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
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdfrm).Focused)
				{
					FromDateHistory.Focus();
				}
				else
				{
					((Control)(object)grdfrm).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdfrm.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdfrm.CurrentColumn != null && grdfrm.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (grdfrm.CurrentColumn != null && grdfrm.CurrentColumn.Key == "Print")
				{
					GenerateReport(CurrId);
				}
				if (grdfrm.CurrentColumn != null && grdfrm.CurrentColumn.Key == "NoOfAttachment")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formrights.DoHaveUpdateRights)
			{
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRemarks_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		try
		{
			if (e.Shift && e.KeyCode == Keys.Tab)
			{
				shiftTabPressed = true;
			}
			else
			{
				shiftTabPressed = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContainers_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdContainers.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space && grdContainers.CurrentColumn != null && grdContainers.CurrentColumn.Key == "Add")
			{
				if (Conversion.ToInt(cmbExportInvoice.Value) <= 0)
				{
					MessageBox.Show("Please Select Invoice First", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					cmbExportInvoice.Focus();
					return;
				}
				if ((double)grdContainers.GetRows().Count() == Conversion.ToDouble((object)txtContrsQty.Text))
				{
					MessageBox.Show("You Can't Add More Rows", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				AddRowInGrid();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (Conversion.ToInt(cmbExportInvoice.Value) <= 0)
				{
					MessageBox.Show("Please Select Invoice First", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					cmbExportInvoice.Focus();
				}
				else if ((double)grdContainers.GetRows().Count() == Conversion.ToDouble((object)txtContrsQty.Text))
				{
					MessageBox.Show("You Can't Add More Rows", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					AddRowInGrid();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		try
		{
			Color posColor = Color.Green;
			Color negColor = Color.Red;
			string[] array = new string[2] { "ReferredContainerNos", "NotReferredContainerNos" };
			foreach (string colName in array)
			{
				GridEXColumn column = grdfrm.RootTable.Columns[colName];
				GridEXCell Cell = e.Row.Cells[((JanusCollectionElement)column).Index];
				Cell.FormatStyle = new GridEXFormatStyle();
				if (column != null && column.Key == "ReferredContainerNos")
				{
					Cell.FormatStyle.ForeColor = posColor;
					Cell.FormatStyle.FontBold = (TriState)2;
				}
				else if (column != null && column.Key == "NotReferredContainerNos")
				{
					Cell.FormatStyle.FontBold = (TriState)2;
					Cell.FormatStyle.ForeColor = negColor;
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
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Expected O, but got Unknown
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Expected O, but got Unknown
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Expected O, but got Unknown
		//IL_0845: Unknown result type (might be due to invalid IL or missing references)
		//IL_084f: Expected O, but got Unknown
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Expected O, but got Unknown
		//IL_08a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Expected O, but got Unknown
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Expected O, but got Unknown
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f4: Expected O, but got Unknown
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Expected O, but got Unknown
		//IL_0958: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Expected O, but got Unknown
		//IL_543f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5449: Expected O, but got Unknown
		//IL_5456: Unknown result type (might be due to invalid IL or missing references)
		//IL_5460: Expected O, but got Unknown
		//IL_5904: Unknown result type (might be due to invalid IL or missing references)
		//IL_590e: Expected O, but got Unknown
		//IL_591b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5925: Expected O, but got Unknown
		//IL_5932: Unknown result type (might be due to invalid IL or missing references)
		//IL_593c: Expected O, but got Unknown
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
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
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
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
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.EximShippingBookingInfo));
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.datFinalEtaDate = new System.Windows.Forms.DateTimePicker();
		this.label30 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtBookingRate = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.txtcutoffdate = new System.Windows.Forms.DateTimePicker();
		this.label50 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtVesselName = new System.Windows.Forms.TextBox();
		this.cmbCarierMedium = new UltraCombo();
		this.cmbshippingline = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbExportInvoice = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtETD = new System.Windows.Forms.DateTimePicker();
		this.CmbDELIVEREDAT = new UltraCombo();
		this.CmbContainerDispatchedAtPort = new UltraCombo();
		this.CmbContainerReceivedFromPort = new UltraCombo();
		this.cmbDestinationPort = new UltraCombo();
		this.txtContrsQty = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbClearingAgent = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtBookingDate = new System.Windows.Forms.DateTimePicker();
		this.txtETA = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtBookingCRO = new System.Windows.Forms.TextBox();
		this.txtVoyageNo = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtFreeDays = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbForwarder = new UltraCombo();
		this.txtTransitDays = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.Lookup = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdContainers = new GridEX();
		this.label13 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdfrm = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbDestinationPortHistory = new UltraCombo();
		this.CmbClearingAgentHistory = new UltraCombo();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.CmbShippingLinHistory = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.CmbForwarderHistory = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbBookingNoHistory = new UltraCombo();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierMedium).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbshippingline).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExportInvoice).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDELIVEREDAT).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerDispatchedAtPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerReceivedFromPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbClearingAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbForwarder).BeginInit();
		this.tsItemType.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContainers).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel15.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPortHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClearingAgentHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShippingLinHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbForwarderHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.txtRemarks);
		this.panel1.Controls.Add(this.datFinalEtaDate);
		this.panel1.Controls.Add(this.label30);
		this.panel1.Controls.Add(this.label29);
		this.panel1.Controls.Add(this.label28);
		this.panel1.Controls.Add(this.label27);
		this.panel1.Controls.Add(this.label26);
		this.panel1.Controls.Add(this.label25);
		this.panel1.Controls.Add(this.txtBookingRate);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label17);
		this.panel1.Controls.Add(this.label48);
		this.panel1.Controls.Add(this.txtcutoffdate);
		this.panel1.Controls.Add(this.label50);
		this.panel1.Controls.Add(this.label16);
		this.panel1.Controls.Add(this.txtVesselName);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCarierMedium);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbshippingline);
		this.panel1.Controls.Add(this.label15);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbExportInvoice);
		this.panel1.Controls.Add(this.label33);
		this.panel1.Controls.Add(this.label32);
		this.panel1.Controls.Add(this.label31);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.txtETD);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDELIVEREDAT);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPort);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPort);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.panel1.Controls.Add(this.txtContrsQty);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.label11);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbClearingAgent);
		this.panel1.Controls.Add(this.label12);
		this.panel1.Controls.Add(this.txtBookingDate);
		this.panel1.Controls.Add(this.txtETA);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.txtBookingCRO);
		this.panel1.Controls.Add(this.txtVoyageNo);
		this.panel1.Controls.Add(this.label7);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.label8);
		this.panel1.Controls.Add(this.txtFreeDays);
		this.panel1.Controls.Add(this.label9);
		this.panel1.Controls.Add(this.label20);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbForwarder);
		this.panel1.Controls.Add(this.txtTransitDays);
		this.panel1.Controls.Add(this.label10);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(3, 53);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(703, 426);
		this.panel1.TabIndex = 0;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(3, 306);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(694, 116);
		this.txtRemarks.TabIndex = 21;
		this.datFinalEtaDate.CustomFormat = "dd-MMM-yy";
		this.datFinalEtaDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFinalEtaDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFinalEtaDate.Location = new System.Drawing.Point(572, 89);
		this.datFinalEtaDate.Name = "datFinalEtaDate";
		this.datFinalEtaDate.Size = new System.Drawing.Size(125, 23);
		this.datFinalEtaDate.TabIndex = 14;
		this.datFinalEtaDate.ValueChanged += new System.EventHandler(datFinalEtaDate_ValueChanged);
		this.datFinalEtaDate.Leave += new System.EventHandler(datFinalEtaDate_ValueChanged);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(497, 147);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(14, 17);
		this.label30.TabIndex = 16;
		this.label30.Text = "*";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(102, 175);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(14, 17);
		this.label29.TabIndex = 596;
		this.label29.Text = "*";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(102, 92);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(14, 17);
		this.label28.TabIndex = 595;
		this.label28.Text = "*";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(102, 202);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(14, 17);
		this.label27.TabIndex = 594;
		this.label27.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(102, 64);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(14, 17);
		this.label26.TabIndex = 593;
		this.label26.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(102, 35);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(14, 17);
		this.label25.TabIndex = 592;
		this.label25.Text = "*";
		this.txtBookingRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBookingRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBookingRate.Location = new System.Drawing.Point(595, 144);
		this.txtBookingRate.Margin = new System.Windows.Forms.Padding(4);
		this.txtBookingRate.Name = "txtBookingRate";
		this.txtBookingRate.Size = new System.Drawing.Size(102, 23);
		this.txtBookingRate.TabIndex = 17;
		this.txtBookingRate.Text = "0";
		this.txtBookingRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 11);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 16;
		this.label4.Text = "Booking Date";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(562, 148);
		this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(30, 15);
		this.label17.TabIndex = 578;
		this.label17.Text = "Rate";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(7, 230);
		this.label48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(74, 15);
		this.label48.TabIndex = 4;
		this.label48.Text = "Vessel Name";
		this.txtcutoffdate.CustomFormat = "dd-MMM-yy";
		this.txtcutoffdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcutoffdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtcutoffdate.Location = new System.Drawing.Point(512, 61);
		this.txtcutoffdate.Name = "txtcutoffdate";
		this.txtcutoffdate.Size = new System.Drawing.Size(185, 23);
		this.txtcutoffdate.TabIndex = 12;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(7, 93);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(80, 15);
		this.label50.TabIndex = 8;
		this.label50.Text = "Shipping Line";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(370, 65);
		this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(77, 15);
		this.label16.TabIndex = 577;
		this.label16.Text = "Cut OFF Date";
		this.txtVesselName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVesselName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVesselName.Location = new System.Drawing.Point(120, 226);
		this.txtVesselName.Margin = new System.Windows.Forms.Padding(4);
		this.txtVesselName.Name = "txtVesselName";
		this.txtVesselName.Size = new System.Drawing.Size(246, 23);
		this.txtVesselName.TabIndex = 8;
		this.cmbCarierMedium.AllowNull = (DefaultableBoolean)1;
		this.cmbCarierMedium.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCarierMedium.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCarierMedium.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierMedium).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierMedium).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCarierMedium).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCarierMedium).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCarierMedium.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCarierMedium).Location = new System.Drawing.Point(120, 197);
		((System.Windows.Forms.Control)(object)this.cmbCarierMedium).Name = "cmbCarierMedium";
		((System.Windows.Forms.Control)(object)this.cmbCarierMedium).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.cmbCarierMedium).TabIndex = 7;
		this.cmbshippingline.AllowNull = (DefaultableBoolean)1;
		this.cmbshippingline.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbshippingline.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbshippingline.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Appearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbshippingline).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance179;
		((SpecialBoxBase)((UltraGridBase)this.cmbshippingline).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance185).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbshippingline).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbshippingline).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbshippingline.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbshippingline).Location = new System.Drawing.Point(120, 87);
		((System.Windows.Forms.Control)(object)this.cmbshippingline).Name = "cmbshippingline";
		((System.Windows.Forms.Control)(object)this.cmbshippingline).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.cmbshippingline).TabIndex = 3;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(7, 203);
		this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(85, 15);
		this.label15.TabIndex = 575;
		this.label15.Text = "Carier Medium";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 65);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(65, 15);
		this.label1.TabIndex = 10;
		this.label1.Text = "Invoice No";
		this.cmbExportInvoice.AllowNull = (DefaultableBoolean)1;
		this.cmbExportInvoice.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExportInvoice.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExportInvoice.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExportInvoice).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.cmbExportInvoice).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExportInvoice).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExportInvoice).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbExportInvoice.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExportInvoice).Location = new System.Drawing.Point(120, 59);
		((System.Windows.Forms.Control)(object)this.cmbExportInvoice).Name = "cmbExportInvoice";
		((System.Windows.Forms.Control)(object)this.cmbExportInvoice).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.cmbExportInvoice).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbExportInvoice).Leave += new System.EventHandler(cmbExportInvoice_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(370, 230);
		this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(72, 15);
		this.label33.TabIndex = 12;
		this.label33.Text = "Delivered At";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(370, 203);
		this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(136, 15);
		this.label32.TabIndex = 12;
		this.label32.Text = "Container Dispatched At";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(370, 176);
		this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(139, 15);
		this.label31.TabIndex = 12;
		this.label31.Text = "Container Received From";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(370, 121);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 12;
		this.label2.Text = "Destination Port";
		this.txtETD.CustomFormat = "dd-MMM-yy";
		this.txtETD.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtETD.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtETD.Location = new System.Drawing.Point(512, 34);
		this.txtETD.Name = "txtETD";
		this.txtETD.Size = new System.Drawing.Size(185, 23);
		this.txtETD.TabIndex = 11;
		this.CmbDELIVEREDAT.AllowNull = (DefaultableBoolean)1;
		this.CmbDELIVEREDAT.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDELIVEREDAT.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDELIVEREDAT.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDELIVEREDAT).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDELIVEREDAT).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDELIVEREDAT.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDELIVEREDAT).Location = new System.Drawing.Point(511, 224);
		((System.Windows.Forms.Control)(object)this.CmbDELIVEREDAT).Name = "CmbDELIVEREDAT";
		((System.Windows.Forms.Control)(object)this.CmbDELIVEREDAT).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbDELIVEREDAT).TabIndex = 20;
		this.CmbContainerDispatchedAtPort.AllowNull = (DefaultableBoolean)1;
		this.CmbContainerDispatchedAtPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContainerDispatchedAtPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContainerDispatchedAtPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContainerDispatchedAtPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContainerDispatchedAtPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPort).Location = new System.Drawing.Point(511, 197);
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPort).Name = "CmbContainerDispatchedAtPort";
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPort).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbContainerDispatchedAtPort).TabIndex = 19;
		this.CmbContainerReceivedFromPort.AllowNull = (DefaultableBoolean)1;
		this.CmbContainerReceivedFromPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContainerReceivedFromPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContainerReceivedFromPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContainerReceivedFromPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContainerReceivedFromPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPort).Location = new System.Drawing.Point(511, 170);
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPort).Name = "CmbContainerReceivedFromPort";
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPort).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbContainerReceivedFromPort).TabIndex = 18;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(512, 115);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 15;
		this.txtContrsQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContrsQty.Enabled = false;
		this.txtContrsQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContrsQty.Location = new System.Drawing.Point(120, 172);
		this.txtContrsQty.Margin = new System.Windows.Forms.Padding(4);
		this.txtContrsQty.Name = "txtContrsQty";
		this.txtContrsQty.Size = new System.Drawing.Size(246, 23);
		this.txtContrsQty.TabIndex = 6;
		this.txtContrsQty.Text = "0";
		this.txtContrsQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtContrsQty.TextChanged += new System.EventHandler(txtContrsQty_TextChanged);
		this.txtContrsQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtContrsQty_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 148);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(85, 15);
		this.label3.TabIndex = 14;
		this.label3.Text = "Clearing Agent";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(370, 38);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(136, 15);
		this.label11.TabIndex = 32;
		this.label11.Text = "ETD Date (Loading Port)";
		this.cmbClearingAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbClearingAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbClearingAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbClearingAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbClearingAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbClearingAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbClearingAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbClearingAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbClearingAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbClearingAgent).Location = new System.Drawing.Point(120, 142);
		((System.Windows.Forms.Control)(object)this.cmbClearingAgent).Name = "cmbClearingAgent";
		((System.Windows.Forms.Control)(object)this.cmbClearingAgent).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.cmbClearingAgent).TabIndex = 5;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 176);
		this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(96, 15);
		this.label12.TabIndex = 31;
		this.label12.Text = "No of Containers";
		this.txtBookingDate.CustomFormat = "dd-MMM-yy";
		this.txtBookingDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBookingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBookingDate.Location = new System.Drawing.Point(120, 7);
		this.txtBookingDate.Name = "txtBookingDate";
		this.txtBookingDate.Size = new System.Drawing.Size(246, 23);
		this.txtBookingDate.TabIndex = 0;
		this.txtETA.CustomFormat = "dd-MMM-yy";
		this.txtETA.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtETA.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtETA.Location = new System.Drawing.Point(512, 7);
		this.txtETA.Name = "txtETA";
		this.txtETA.Size = new System.Drawing.Size(185, 23);
		this.txtETA.TabIndex = 10;
		this.txtETA.ValueChanged += new System.EventHandler(txtETA_ValueChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(7, 257);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(65, 15);
		this.label5.TabIndex = 17;
		this.label5.Text = "Voyage No";
		this.txtBookingCRO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBookingCRO.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBookingCRO.Location = new System.Drawing.Point(120, 34);
		this.txtBookingCRO.Margin = new System.Windows.Forms.Padding(4);
		this.txtBookingCRO.Name = "txtBookingCRO";
		this.txtBookingCRO.Size = new System.Drawing.Size(246, 23);
		this.txtBookingCRO.TabIndex = 1;
		this.txtVoyageNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoyageNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVoyageNo.Location = new System.Drawing.Point(120, 253);
		this.txtVoyageNo.Margin = new System.Windows.Forms.Padding(4);
		this.txtVoyageNo.Name = "txtVoyageNo";
		this.txtVoyageNo.Size = new System.Drawing.Size(246, 23);
		this.txtVoyageNo.TabIndex = 9;
		this.label7.BackColor = System.Drawing.Color.LightGray;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(3, 280);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(694, 23);
		this.label7.TabIndex = 27;
		this.label7.Text = "Remarks";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(370, 148);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(130, 15);
		this.label6.TabIndex = 19;
		this.label6.Text = "Free Days (Destination)";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(370, 11);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(134, 15);
		this.label8.TabIndex = 25;
		this.label8.Text = "ETA Date (Loading Port)";
		this.txtFreeDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreeDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreeDays.Location = new System.Drawing.Point(512, 144);
		this.txtFreeDays.Margin = new System.Windows.Forms.Padding(4);
		this.txtFreeDays.Name = "txtFreeDays";
		this.txtFreeDays.Size = new System.Drawing.Size(48, 23);
		this.txtFreeDays.TabIndex = 16;
		this.txtFreeDays.Text = "0";
		this.txtFreeDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreeDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreeDays_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(7, 38);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(92, 15);
		this.label9.TabIndex = 24;
		this.label9.Text = "Booking / CRO#";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(370, 93);
		this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(134, 15);
		this.label20.TabIndex = 19;
		this.label20.Text = "Transit Days && Final ETA";
		this.cmbForwarder.AllowNull = (DefaultableBoolean)1;
		this.cmbForwarder.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbForwarder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbForwarder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbForwarder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbForwarder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbForwarder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbForwarder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbForwarder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbForwarder).Location = new System.Drawing.Point(120, 115);
		((System.Windows.Forms.Control)(object)this.cmbForwarder).Name = "cmbForwarder";
		((System.Windows.Forms.Control)(object)this.cmbForwarder).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.cmbForwarder).TabIndex = 4;
		this.txtTransitDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTransitDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTransitDays.Location = new System.Drawing.Point(512, 89);
		this.txtTransitDays.Margin = new System.Windows.Forms.Padding(4);
		this.txtTransitDays.Name = "txtTransitDays";
		this.txtTransitDays.Size = new System.Drawing.Size(59, 23);
		this.txtTransitDays.TabIndex = 13;
		this.txtTransitDays.Text = "0";
		this.txtTransitDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTransitDays.TextChanged += new System.EventHandler(txtTransitDays_TextChanged);
		this.txtTransitDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTransitDays_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 124);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(60, 15);
		this.label10.TabIndex = 22;
		this.label10.Text = "Forwarder";
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnsave, this.btnupdate, this.btnAttachment, this.btnPrint, this.Lookup, this.btnShortCutKey });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1070, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(59, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(75, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "560-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.Lookup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.Lookup.Image = (System.Drawing.Image)resources.GetObject("Lookup.Image");
		this.Lookup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Lookup.Name = "Lookup";
		this.Lookup.Size = new System.Drawing.Size(57, 24);
		this.Lookup.Text = "Lookup";
		this.Lookup.Click += new System.EventHandler(Lookup_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label19);
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1070, 50);
		this.panel2.TabIndex = 1;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(3, 27);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(256, 19);
		this.label19.TabIndex = 574;
		this.label19.Text = "Shipping/Booking Information";
		this.panel5.Controls.Add(this.ctrlGrdBar2);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdContainers);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(366, 426);
		this.panel5.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(328, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdContainers;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		((System.Windows.Forms.Control)(object)this.grdContainers).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdContainers.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdContainers).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContainers.GroupByBoxVisible = false;
		this.grdContainers.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdContainers).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdContainers).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdContainers).Name = "grdContainers";
		((ScrollableBase)this.grdContainers).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdContainers).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdContainers).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContainers).Size = new System.Drawing.Size(366, 403);
		((System.Windows.Forms.Control)(object)this.grdContainers).TabIndex = 0;
		this.grdContainers.TotalRow = (InheritableBoolean)1;
		this.grdContainers.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContainers).VisualStyle = (VisualStyle)4;
		this.grdContainers.CellUpdated += new ColumnActionEventHandler(grdContainers_CellUpdated);
		this.grdContainers.ColumnButtonClick += new ColumnActionEventHandler(grdContainers_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContainers).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContainers_KeyDown);
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Teal;
		this.label13.Location = new System.Drawing.Point(0, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(366, 23);
		this.label13.TabIndex = 0;
		this.label13.Text = "Containers List";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1084, 511);
		this.tabControl1.TabIndex = 3;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1076, 482);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(706, 53);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(367, 426);
		this.panel3.TabIndex = 4;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1076, 482);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(3, 182);
		((System.Windows.Forms.Control)(object)this.grdfrm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1070, 297);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.FormattingRow += new RowLoadEventHandler(grdfrm_FormattingRow);
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label92);
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 160);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1070, 22);
		this.panel15.TabIndex = 49;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(2, 0);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(142, 19);
		this.label92.TabIndex = 1;
		this.label92.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1018, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(49, 21);
		this.ctrlGrdBar1.TabIndex = 51;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1070, 105);
		this.panel4.TabIndex = 48;
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDestinationPortHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbClearingAgentHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShippingLinHistory);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbForwarderHistory);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.label89);
		this.groupBox2.Controls.Add(this.label90);
		this.groupBox2.Controls.Add(this.label91);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBookingNoHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, -1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(962, 102);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(238, 48);
		this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(80, 15);
		this.label22.TabIndex = 91;
		this.label22.Text = "Shipping Line";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(541, 20);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(85, 15);
		this.label23.TabIndex = 92;
		this.label23.Text = "Clearing Agent";
		this.CmbDestinationPortHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDestinationPortHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDestinationPortHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDestinationPortHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance165).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDestinationPortHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDestinationPortHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortHistory).Location = new System.Drawing.Point(634, 70);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortHistory).Name = "CmbDestinationPortHistory";
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortHistory).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPortHistory).TabIndex = 8;
		this.CmbClearingAgentHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbClearingAgentHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbClearingAgentHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbClearingAgentHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbClearingAgentHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbClearingAgentHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbClearingAgentHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbClearingAgentHistory).Location = new System.Drawing.Point(634, 14);
		((System.Windows.Forms.Control)(object)this.CmbClearingAgentHistory).Name = "CmbClearingAgentHistory";
		((System.Windows.Forms.Control)(object)this.CmbClearingAgentHistory).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbClearingAgentHistory).TabIndex = 6;
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance171).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(634, 42);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 7;
		this.CmbShippingLinHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbShippingLinHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShippingLinHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShippingLinHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance175).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShippingLinHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShippingLinHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShippingLinHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShippingLinHistory).Location = new System.Drawing.Point(320, 42);
		((System.Windows.Forms.Control)(object)this.CmbShippingLinHistory).Name = "CmbShippingLinHistory";
		((System.Windows.Forms.Control)(object)this.CmbShippingLinHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbShippingLinHistory).TabIndex = 4;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(238, 76);
		this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(60, 15);
		this.label24.TabIndex = 93;
		this.label24.Text = "Forwarder";
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(864, 70);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 12;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(864, 47);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 11;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.CmbForwarderHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbForwarderHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbForwarderHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbForwarderHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbForwarderHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbForwarderHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbForwarderHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbForwarderHistory).Location = new System.Drawing.Point(320, 70);
		((System.Windows.Forms.Control)(object)this.CmbForwarderHistory).Name = "CmbForwarderHistory";
		((System.Windows.Forms.Control)(object)this.CmbForwarderHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbForwarderHistory).TabIndex = 5;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(541, 76);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(93, 15);
		this.label21.TabIndex = 87;
		this.label21.Text = "Destination Port";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(541, 48);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(65, 15);
		this.label18.TabIndex = 87;
		this.label18.Text = "Invoice No";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(238, 20);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(58, 15);
		this.label14.TabIndex = 87;
		this.label14.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(864, 11);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 9;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(864, 30);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 10;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(2, 76);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(70, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "Booking No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(2, 48);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(2, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbBookingNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBookingNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBookingNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBookingNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance146;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBookingNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBookingNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBookingNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBookingNoHistory).Location = new System.Drawing.Point(75, 70);
		((System.Windows.Forms.Control)(object)this.CmbBookingNoHistory).Name = "CmbBookingNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbBookingNoHistory).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.CmbBookingNoHistory).TabIndex = 2;
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(320, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 3;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(75, 44);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(159, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(75, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(159, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label69);
		this.panel16.Controls.Add(this.toolStrip3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1070, 52);
		this.panel16.TabIndex = 1;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(2, 28);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(305, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Export ShiipingLine/Booking History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.btnRefreshHistory, this.toolStripSeparator1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1070, 27);
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
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 511);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "EximShippingBookingInfo";
		this.Text = "Shipping Booking Info";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(EximShippingBookingInfo_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmShippingBookingInfo_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierMedium).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbshippingline).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExportInvoice).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDELIVEREDAT).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerDispatchedAtPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerReceivedFromPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbClearingAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbForwarder).EndInit();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContainers).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPortHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClearingAgentHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShippingLinHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbForwarderHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
