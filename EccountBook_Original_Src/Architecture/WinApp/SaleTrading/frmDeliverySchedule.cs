using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.General;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.SaleTrading;

public class frmDeliverySchedule : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private bool DoHaveCanDelete;

	public static int SupplierId;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private double qtysum;

	private bool chkstatus;

	private int RecId;

	private bool UpdateMode;

	private int updateDetailIndex;

	private DataTable dtdetail = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	public string GRNS = string.Empty;

	public bool SupplySelected;

	private LoadSaleOrder LoadSupply;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel7;

	private Panel panel6;

	private Panel panel5;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private DateTimePicker txtDocdate;

	private Label label2;

	private Label label1;

	private TextBox txtdocnumber;

	internal UltraCombo comproject;

	private Label label18;

	internal UltraCombo combranches;

	private Label label13;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label4;

	private TextBox txtPaymentSch;

	private TextBox txtProductionsch;

	private TextBox txtCustomerSch;

	private TextBox txtSupplierSch;

	internal UltraCombo cmbSupplier;

	private Label label8;

	private Label label11;

	private TextBox txtRemarks;

	private DateTimePicker txtToDate;

	private Label label10;

	private DateTimePicker txtFromDate;

	private Label label9;

	internal GridEX grdDetail;

	private Panel panel8;

	private Panel panel11;

	private Panel panel9;

	private ToolStrip toolStrip2;

	private ToolStripButton btnHistoryRefresh;

	private ToolStripButton BtnPrint;

	private ToolStripSeparator toolStripSeparator1;

	internal GridEX DataGridHistory;

	private ToolStripButton toolStripButton6;

	private ToolStripButton toolStripButton7;

	private Panel panel4;

	internal GridEX grdHistoryGridDetail;

	private CheckBox chkPrint;

	private ToolStripButton btnLoadOrder;

	private Timer timer1;

	private ToolStripButton btnDelete;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnRefresh;

	public frmDeliverySchedule(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSupplySchedule_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = base.Name,
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Print").ToList();
				DoHavePrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Delete").ToList();
				DoHaveCanDelete = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnsave.Enabled = DoHaveSaveRight;
			Print.Enabled = DoHavePrintRights;
			btnUpdate.Enabled = DoHaveUpdateRights;
			btnDelete.Enabled = DoHaveCanDelete;
			dtViewAttachments.Columns.Add("AttchmentPath");
			braches();
			Project();
			GenerateDocNo();
			customerbind();
			txtDocdate.Focus();
			chkPrint.Checked = true;
			dtdetail.Columns.Add("OrderId");
			dtdetail.Columns.Add("OrderNo");
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("UOMId");
			dtdetail.Columns.Add("UOM");
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("Month");
			dtdetail.Columns.Add("D1", typeof(double));
			dtdetail.Columns.Add("D2", typeof(double));
			dtdetail.Columns.Add("D3", typeof(double));
			dtdetail.Columns.Add("D4", typeof(double));
			dtdetail.Columns.Add("D5", typeof(double));
			dtdetail.Columns.Add("D6", typeof(double));
			dtdetail.Columns.Add("D7", typeof(double));
			dtdetail.Columns.Add("D8", typeof(double));
			dtdetail.Columns.Add("D9", typeof(double));
			dtdetail.Columns.Add("D10", typeof(double));
			dtdetail.Columns.Add("D11", typeof(double));
			dtdetail.Columns.Add("D12", typeof(double));
			dtdetail.Columns.Add("D13", typeof(double));
			dtdetail.Columns.Add("D14", typeof(double));
			dtdetail.Columns.Add("D15", typeof(double));
			dtdetail.Columns.Add("D16", typeof(double));
			dtdetail.Columns.Add("D17", typeof(double));
			dtdetail.Columns.Add("D18", typeof(double));
			dtdetail.Columns.Add("D19", typeof(double));
			dtdetail.Columns.Add("D20", typeof(double));
			dtdetail.Columns.Add("D21", typeof(double));
			dtdetail.Columns.Add("D22", typeof(double));
			dtdetail.Columns.Add("D23", typeof(double));
			dtdetail.Columns.Add("D24", typeof(double));
			dtdetail.Columns.Add("D25", typeof(double));
			dtdetail.Columns.Add("D26", typeof(double));
			dtdetail.Columns.Add("D27", typeof(double));
			dtdetail.Columns.Add("D28", typeof(double));
			dtdetail.Columns.Add("D29", typeof(double));
			dtdetail.Columns.Add("D30", typeof(double));
			dtdetail.Columns.Add("D31", typeof(double));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			int code = DeliverySchedule.GenerateCode(new DeliverySchedule
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			if (code > 0)
			{
				txtdocnumber.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Project()
	{
		try
		{
			DataTable dtproject = CommonServices.ProjectServiceBind();
			if (dtproject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtproject, comproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)comproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void braches()
	{
		try
		{
			DataTable dtbran = CommonServices.BrancheServiceBind();
			if (dtbran.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtbran, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void customerbind()
	{
		try
		{
			DataTable dtcustomersup = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dtcustomersup.Rows.Count > 0)
			{
				DDL.BindDDL(dtcustomersup, cmbSupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Expected O, but got Unknown
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_093e: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			DeliverySchedule supply = new DeliverySchedule();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				supply.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			supply.ThisDocumentTypeId = 88;
			supply.BranchesId = Conversion.ToInt(combranches.Value);
			supply.ProjectId = Conversion.ToInt(comproject.Value);
			supply.DocNo = Conversion.ToInt((object)txtdocnumber.Text);
			supply.DocDate = txtDocdate.Value;
			supply.SupCustId = Conversion.ToInt(cmbSupplier.Value);
			supply.SupplierSchCode = txtSupplierSch.Text;
			supply.CustomerSchCode = txtCustomerSch.Text;
			supply.ProductionSchCode = txtProductionsch.Text;
			supply.PaymentSchCode = txtPaymentSch.Text;
			supply.Remarks = txtRemarks.Text;
			supply.FromDate = txtFromDate.Value;
			supply.ToDate = txtToDate.Value;
			supply.OrganizationId = UserAccount.OrganizationId;
			supply.CompanyId = UserAccount.CompanyId;
			supply.EntryUser = UserAccount.ID;
			supply.ModifyUser = UserAccount.ID;
			supply.DeliveryScheduleDetail = new List<DeliveryScheduleDetail>();
			supply.DeliveryScheduleDetaildate = new List<DeliveryScheduleDetailDates>();
			if (grdDetail.RowCount <= 0)
			{
				return;
			}
			Cal();
			if (!chkstatus)
			{
				return;
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow a2 in rows)
			{
				DeliveryScheduleDetail scheduleDetail = new DeliveryScheduleDetail();
				scheduleDetail.OrderId = Conversion.ToInt(a2.Cells["OrderId"].Value);
				scheduleDetail.ItemId = Conversion.ToInt(a2.Cells["ItemId"].Value);
				scheduleDetail.ItemUOMId = Conversion.ToInt(a2.Cells["UOMId"].Value);
				scheduleDetail.TotalQty = Conversion.ToDouble((object)a2.Cells["Qty"].Text);
				scheduleDetail.MonthDesc = Conversion.ToDateTime(a2.Cells["Month"].Value);
				scheduleDetail.Day1 = Conversion.ToDouble((object)a2.Cells["D1"].Text);
				scheduleDetail.Day2 = Conversion.ToDouble((object)a2.Cells["D2"].Text);
				scheduleDetail.Day3 = Conversion.ToDouble((object)a2.Cells["D3"].Text);
				scheduleDetail.Day4 = Conversion.ToDouble((object)a2.Cells["D4"].Text);
				scheduleDetail.Day5 = Conversion.ToDouble((object)a2.Cells["D5"].Text);
				scheduleDetail.Day6 = Conversion.ToDouble((object)a2.Cells["D6"].Text);
				scheduleDetail.Day7 = Conversion.ToDouble((object)a2.Cells["D7"].Text);
				scheduleDetail.Day8 = Conversion.ToDouble((object)a2.Cells["D8"].Text);
				scheduleDetail.Day9 = Conversion.ToDouble((object)a2.Cells["D9"].Text);
				scheduleDetail.Day10 = Conversion.ToDouble((object)a2.Cells["D10"].Text);
				scheduleDetail.Day11 = Conversion.ToDouble((object)a2.Cells["D11"].Text);
				scheduleDetail.Day12 = Conversion.ToDouble((object)a2.Cells["D12"].Text);
				scheduleDetail.Day13 = Conversion.ToDouble((object)a2.Cells["D13"].Text);
				scheduleDetail.Day14 = Conversion.ToDouble((object)a2.Cells["D14"].Text);
				scheduleDetail.Day15 = Conversion.ToDouble((object)a2.Cells["D15"].Text);
				scheduleDetail.Day16 = Conversion.ToDouble((object)a2.Cells["D16"].Text);
				scheduleDetail.Day17 = Conversion.ToDouble((object)a2.Cells["D17"].Text);
				scheduleDetail.Day18 = Conversion.ToDouble((object)a2.Cells["D18"].Text);
				scheduleDetail.Day19 = Conversion.ToDouble((object)a2.Cells["D19"].Text);
				scheduleDetail.Day20 = Conversion.ToDouble((object)a2.Cells["D20"].Text);
				scheduleDetail.Day21 = Conversion.ToDouble((object)a2.Cells["D21"].Text);
				scheduleDetail.Day22 = Conversion.ToDouble((object)a2.Cells["D22"].Text);
				scheduleDetail.Day23 = Conversion.ToDouble((object)a2.Cells["D23"].Text);
				scheduleDetail.Day24 = Conversion.ToDouble((object)a2.Cells["D24"].Text);
				scheduleDetail.Day25 = Conversion.ToDouble((object)a2.Cells["D25"].Text);
				scheduleDetail.Day26 = Conversion.ToDouble((object)a2.Cells["D26"].Text);
				scheduleDetail.Day27 = Conversion.ToDouble((object)a2.Cells["D27"].Text);
				scheduleDetail.Day28 = Conversion.ToDouble((object)a2.Cells["D28"].Text);
				scheduleDetail.Day29 = Conversion.ToDouble((object)a2.Cells["D29"].Text);
				scheduleDetail.Day30 = Conversion.ToDouble((object)a2.Cells["D30"].Text);
				scheduleDetail.Day31 = Conversion.ToDouble((object)a2.Cells["D31"].Text);
				supply.DeliveryScheduleDetail.Add(scheduleDetail);
			}
			rows = grdDetail.GetRows();
			foreach (GridEXRow a in rows)
			{
				int z = 8;
				int b = 1;
				for (int j = 0; j < 39; j++)
				{
					if (z == 39)
					{
						break;
					}
					if (Conversion.ToInt(a.Cells[z].Value) != 0 && a.Cells[z].Text != string.Empty)
					{
						DeliveryScheduleDetailDates scheduleDetailDates = new DeliveryScheduleDetailDates();
						scheduleDetailDates.OrderId = Conversion.ToInt(a.Cells["OrderId"].Value);
						scheduleDetailDates.ItemId = Conversion.ToInt(a.Cells["ItemId"].Value);
						scheduleDetailDates.ItemUOMId = Conversion.ToInt(a.Cells["UOMId"].Value);
						scheduleDetailDates.TotalQty = Conversion.ToDouble((object)a.Cells["Qty"].Text);
						scheduleDetailDates.MonthDesc = Conversion.ToDateTime((object)a.Cells["Month"].Text);
						scheduleDetailDates.SchDate = Conversion.ToDateTime((object)(b + "-" + a.Cells["Month"].Text));
						scheduleDetailDates.SchQty = Conversion.ToDouble((object)a.Cells[z].Text);
						supply.DeliveryScheduleDetaildate.Add(scheduleDetailDates);
					}
					z++;
					b++;
				}
			}
			supply.AttachmentsList = new List<DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				supply.AttachmentsList.Add(attach);
			}
			int success = DeliverySchedule.Save(supply);
			if (RecId > 0)
			{
				MessageBox.Show("Update SuccessFully");
			}
			else
			{
				MessageBox.Show("Save SuccessFully");
			}
			formReset();
			if (!chkPrint.Checked)
			{
				return;
			}
			DataTable dt = GeneralReprots.DeliveryScheduleSlip(success);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting rpt = new Reporting();
			if (UserAccount.CompAddress != string.Empty)
			{
				rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			}
			if (UserAccount.CompName != string.Empty)
			{
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			}
			rpt.ShowReportWithDataTable(dt, "460-InvRptDeliverySchedule.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Cal()
	{
		try
		{
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow a in rows)
			{
				qtysum = Conversion.ToDouble((object)a.Cells["D1"].Text) + Conversion.ToDouble((object)a.Cells["D2"].Text) + Conversion.ToDouble((object)a.Cells["D3"].Text) + Conversion.ToDouble((object)a.Cells["D4"].Text) + Conversion.ToDouble((object)a.Cells["D5"].Text) + Conversion.ToDouble((object)a.Cells["D6"].Text) + Conversion.ToDouble((object)a.Cells["D7"].Text) + Conversion.ToDouble((object)a.Cells["D8"].Text) + Conversion.ToDouble((object)a.Cells["D9"].Text) + Conversion.ToDouble((object)a.Cells["D10"].Text) + Conversion.ToDouble((object)a.Cells["D11"].Text) + Conversion.ToDouble((object)a.Cells["D12"].Text) + Conversion.ToDouble((object)a.Cells["D13"].Text) + Conversion.ToDouble((object)a.Cells["D14"].Text) + Conversion.ToDouble((object)a.Cells["D15"].Text) + Conversion.ToDouble((object)a.Cells["D16"].Text) + Conversion.ToDouble((object)a.Cells["D17"].Text) + Conversion.ToDouble((object)a.Cells["D18"].Text) + Conversion.ToDouble((object)a.Cells["D19"].Text) + Conversion.ToDouble((object)a.Cells["D20"].Text) + Conversion.ToDouble((object)a.Cells["D21"].Text) + Conversion.ToDouble((object)a.Cells["D22"].Text) + Conversion.ToDouble((object)a.Cells["D23"].Text) + Conversion.ToDouble((object)a.Cells["D24"].Text) + Conversion.ToDouble((object)a.Cells["D25"].Text) + Conversion.ToDouble((object)a.Cells["D26"].Text) + Conversion.ToDouble((object)a.Cells["D27"].Text) + Conversion.ToDouble((object)a.Cells["D28"].Text) + Conversion.ToDouble((object)a.Cells["D29"].Text) + Conversion.ToDouble((object)a.Cells["D30"].Text) + Conversion.ToDouble((object)a.Cells["D31"].Text);
				if (Conversion.ToDouble(a.Cells["Qty"].Value) != Conversion.ToDouble((object)qtysum))
				{
					MessageBox.Show("Quantity Not Matched");
					break;
				}
				chkstatus = true;
			}
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
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
			grdDetail.RootTable.Columns["OrderNo"].Width = 50;
			grdDetail.RootTable.Columns["Item"].Width = 300;
			grdDetail.RootTable.Columns["UOM"].Width = 70;
			grdDetail.RootTable.Columns["Qty"].Width = 70;
			grdDetail.RootTable.Columns["Month"].Width = 100;
			grdDetail.RootTable.Columns["D1"].Width = 70;
			grdDetail.RootTable.Columns["D2"].Width = 70;
			grdDetail.RootTable.Columns["D3"].Width = 70;
			grdDetail.RootTable.Columns["D4"].Width = 70;
			grdDetail.RootTable.Columns["D5"].Width = 70;
			grdDetail.RootTable.Columns["D6"].Width = 70;
			grdDetail.RootTable.Columns["D7"].Width = 70;
			grdDetail.RootTable.Columns["D8"].Width = 70;
			grdDetail.RootTable.Columns["D9"].Width = 70;
			grdDetail.RootTable.Columns["D10"].Width = 70;
			grdDetail.RootTable.Columns["D11"].Width = 70;
			grdDetail.RootTable.Columns["D12"].Width = 70;
			grdDetail.RootTable.Columns["D13"].Width = 70;
			grdDetail.RootTable.Columns["D14"].Width = 70;
			grdDetail.RootTable.Columns["D15"].Width = 70;
			grdDetail.RootTable.Columns["D16"].Width = 70;
			grdDetail.RootTable.Columns["D17"].Width = 70;
			grdDetail.RootTable.Columns["D18"].Width = 70;
			grdDetail.RootTable.Columns["D19"].Width = 70;
			grdDetail.RootTable.Columns["D20"].Width = 70;
			grdDetail.RootTable.Columns["D21"].Width = 70;
			grdDetail.RootTable.Columns["D22"].Width = 70;
			grdDetail.RootTable.Columns["D23"].Width = 70;
			grdDetail.RootTable.Columns["D24"].Width = 70;
			grdDetail.RootTable.Columns["D25"].Width = 70;
			grdDetail.RootTable.Columns["D26"].Width = 70;
			grdDetail.RootTable.Columns["D27"].Width = 70;
			grdDetail.RootTable.Columns["D28"].Width = 70;
			grdDetail.RootTable.Columns["D29"].Width = 70;
			grdDetail.RootTable.Columns["D30"].Width = 70;
			grdDetail.RootTable.Columns["D31"].Width = 70;
			grdDetail.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["UOM"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Month"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderNo"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["UOM"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D1"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D2"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D3"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D4"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D5"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D6"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D7"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D8"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D9"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D10"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D11"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D12"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D13"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D14"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D15"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D16"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D17"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D18"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D19"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D20"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D21"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D22"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D23"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D24"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D25"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D26"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D27"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D28"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D29"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D30"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["D31"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D1"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D2"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D3"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D4"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D5"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D6"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D7"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D8"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D9"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D10"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D11"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D12"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D13"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D14"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D15"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D16"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D17"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D18"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D19"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D20"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D21"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D22"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D23"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D24"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D25"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D26"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D27"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D28"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D29"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D30"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["D31"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["UOMId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters header = new ReportsParameters();
			header.OrganizationId = UserAccount.OrganizationId;
			header.CompanyId = UserAccount.CompanyId;
			header.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			header.NoOfRecords = 50;
			if (!header.CanViewAllRecord)
			{
				header.EntryUser = UserAccount.ID;
			}
			DataTable dthis = new DataTable();
			dthis = DeliverySchedule.GetAll(header);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocNo");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("Customer");
			dt.Columns.Add("SupplierSchedule");
			dt.Columns.Add("CustomerSchedule");
			dt.Columns.Add("PaymentSchedule");
			dt.Columns.Add("ProductionSchedule");
			dt.Columns.Add("FromDate");
			dt.Columns.Add("ToDate");
			dt.Columns.Add("Remarks");
			dt.Columns.Add("Attachments");
			for (int i = 0; i < dthis.Rows.Count; i++)
			{
				dt.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocNo"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["CustomerName"], dthis.Rows[i]["SupplierSchCode"], dthis.Rows[i]["CustomerSchCode"], dthis.Rows[i]["PaymentSchCode"], dthis.Rows[i]["ProductionSchCode"], Conversion.ToDateTime(dthis.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dthis.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["Remarks"], dthis.Rows[i]["NoOfAttachments"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			HistoryDataGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void HistoryDataGridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocNo"].Width = 50;
			DataGridHistory.RootTable.Columns["DocDate"].Width = 80;
			DataGridHistory.RootTable.Columns["Customer"].Width = 250;
			DataGridHistory.RootTable.Columns["SupplierSchedule"].Width = 100;
			DataGridHistory.RootTable.Columns["CustomerSchedule"].Width = 100;
			DataGridHistory.RootTable.Columns["PaymentSchedule"].Width = 100;
			DataGridHistory.RootTable.Columns["ProductionSchedule"].Width = 100;
			DataGridHistory.RootTable.Columns["FromDate"].Width = 80;
			DataGridHistory.RootTable.Columns["ToDate"].Width = 80;
			DataGridHistory.RootTable.Columns["Remarks"].Width = 300;
			DataGridHistory.RootTable.Columns["Attachments"].ColumnType = (ColumnType)5;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void formReset()
	{
		AT = new Attachment();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		((Control)(object)cmbSupplier).Enabled = true;
		txtFromDate.Enabled = true;
		UpdateMode = false;
		RecId = 0;
		((Control)(object)cmbSupplier).Text = string.Empty;
		txtdocnumber.Text = string.Empty;
		txtDocdate.Value = DateTime.Now;
		txtFromDate.Value = DateTime.Now;
		txtToDate.Value = DateTime.Now;
		txtSupplierSch.Text = string.Empty;
		txtProductionsch.Text = string.Empty;
		txtPaymentSch.Text = string.Empty;
		txtCustomerSch.Text = string.Empty;
		txtRemarks.Text = string.Empty;
		GenerateDocNo();
		grdDetail.ClearStructure();
		dtdetail.Clear();
		txtDocdate.Focus();
	}

	private bool FormValidation()
	{
		if (((Control)(object)combranches).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Branch Field Required");
			combranches.Focus();
			return false;
		}
		if (((Control)(object)comproject).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Project Field Required");
			comproject.Focus();
			return false;
		}
		if (txtdocnumber.Text.Trim() == string.Empty || txtdocnumber.Text.Trim() == "0")
		{
			MessageBox.Show("DocNumber Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (((Control)(object)cmbSupplier).Text.Trim() == string.Empty)
		{
			MessageBox.Show("supplier Field Required");
			cmbSupplier.Focus();
			return false;
		}
		return true;
	}

	private void frmSupplySchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && btnsave.Visible && btnsave.Enabled)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnnew_Click(null, null);
				}
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				Print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOrder_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString()));
	}

	public void ReadById(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			DeliverySchedule supply = new DeliverySchedule();
			RecId = Id;
			supply = DeliverySchedule.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			combranches.Value = supply.BranchesId;
			comproject.Value = supply.ProjectId;
			txtdocnumber.Text = supply.DocNo.ToString();
			txtDocdate.Value = supply.DocDate;
			cmbSupplier.Value = supply.SupCustId;
			((Control)(object)cmbSupplier).Enabled = false;
			txtSupplierSch.Text = supply.SupplierSchCode;
			txtCustomerSch.Text = supply.CustomerSchCode;
			txtProductionsch.Text = supply.ProductionSchCode;
			txtPaymentSch.Text = supply.PaymentSchCode;
			txtRemarks.Text = supply.Remarks;
			txtFromDate.Value = supply.FromDate;
			txtToDate.Value = supply.ToDate;
			dtdetail.Clear();
			grdDetail.ClearStructure();
			for (int i = 0; i < supply.DeliveryScheduleDetail.Count; i++)
			{
				dtdetail.Rows.Add(supply.DeliveryScheduleDetail[i].OrderId, supply.DeliveryScheduleDetail[i].SaleOrderNo, supply.DeliveryScheduleDetail[i].ItemId, supply.DeliveryScheduleDetail[i].ItemName, supply.DeliveryScheduleDetail[i].ItemUOMId, supply.DeliveryScheduleDetail[i].UOMCode, supply.DeliveryScheduleDetail[i].TotalQty, supply.DeliveryScheduleDetail[i].MonthDesc.ToString("dd-MMM-yyyy"), supply.DeliveryScheduleDetail[i].Day1, supply.DeliveryScheduleDetail[i].Day2, supply.DeliveryScheduleDetail[i].Day3, supply.DeliveryScheduleDetail[i].Day4, supply.DeliveryScheduleDetail[i].Day5, supply.DeliveryScheduleDetail[i].Day6, supply.DeliveryScheduleDetail[i].Day7, supply.DeliveryScheduleDetail[i].Day8, supply.DeliveryScheduleDetail[i].Day9, supply.DeliveryScheduleDetail[i].Day10, supply.DeliveryScheduleDetail[i].Day11, supply.DeliveryScheduleDetail[i].Day12, supply.DeliveryScheduleDetail[i].Day13, supply.DeliveryScheduleDetail[i].Day14, supply.DeliveryScheduleDetail[i].Day15, supply.DeliveryScheduleDetail[i].Day16, supply.DeliveryScheduleDetail[i].Day17, supply.DeliveryScheduleDetail[i].Day18, supply.DeliveryScheduleDetail[i].Day19, supply.DeliveryScheduleDetail[i].Day20, supply.DeliveryScheduleDetail[i].Day21, supply.DeliveryScheduleDetail[i].Day22, supply.DeliveryScheduleDetail[i].Day23, supply.DeliveryScheduleDetail[i].Day24, supply.DeliveryScheduleDetail[i].Day25, supply.DeliveryScheduleDetail[i].Day26, supply.DeliveryScheduleDetail[i].Day27, supply.DeliveryScheduleDetail[i].Day28, supply.DeliveryScheduleDetail[i].Day29, supply.DeliveryScheduleDetail[i].Day30, supply.DeliveryScheduleDetail[i].Day31);
			}
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			GridSetting();
			UpdateMode = true;
			List<DMSAttachments> byID = DMSAttachments.GetByID(int.Parse(Id.ToString()), base.Name);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			DeliverySchedule supply = new DeliverySchedule();
			supply = DeliverySchedule.GetByID(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString()));
			if (supply != null)
			{
				DataTable dtdet = new DataTable();
				dtdet.Columns.Add("OrderNo");
				dtdet.Columns.Add("Item");
				dtdet.Columns.Add("UOM");
				dtdet.Columns.Add("Qty", typeof(double));
				dtdet.Columns.Add("Month");
				dtdet.Columns.Add("D1");
				dtdet.Columns.Add("D2");
				dtdet.Columns.Add("D3");
				dtdet.Columns.Add("D4");
				dtdet.Columns.Add("D5");
				dtdet.Columns.Add("D6");
				dtdet.Columns.Add("D7");
				dtdet.Columns.Add("D8");
				dtdet.Columns.Add("D9");
				dtdet.Columns.Add("D10");
				dtdet.Columns.Add("D11");
				dtdet.Columns.Add("D12");
				dtdet.Columns.Add("D13");
				dtdet.Columns.Add("D14");
				dtdet.Columns.Add("D15");
				dtdet.Columns.Add("D16");
				dtdet.Columns.Add("D17");
				dtdet.Columns.Add("D18");
				dtdet.Columns.Add("D19");
				dtdet.Columns.Add("D20");
				dtdet.Columns.Add("D21");
				dtdet.Columns.Add("D22");
				dtdet.Columns.Add("D23");
				dtdet.Columns.Add("D24");
				dtdet.Columns.Add("D25");
				dtdet.Columns.Add("D26");
				dtdet.Columns.Add("D27");
				dtdet.Columns.Add("D28");
				dtdet.Columns.Add("D29");
				dtdet.Columns.Add("D30");
				dtdet.Columns.Add("D31");
				for (int i = 0; i < supply.DeliveryScheduleDetail.Count; i++)
				{
					dtdet.Rows.Add(supply.DeliveryScheduleDetail[i].SaleOrderNo, supply.DeliveryScheduleDetail[i].ItemName, supply.DeliveryScheduleDetail[i].UOMCode, supply.DeliveryScheduleDetail[i].TotalQty, supply.DeliveryScheduleDetail[i].MonthDesc.ToString("MMM-yyyy"), supply.DeliveryScheduleDetail[i].Day1, supply.DeliveryScheduleDetail[i].Day2, supply.DeliveryScheduleDetail[i].Day3, supply.DeliveryScheduleDetail[i].Day4, supply.DeliveryScheduleDetail[i].Day5, supply.DeliveryScheduleDetail[i].Day6, supply.DeliveryScheduleDetail[i].Day7, supply.DeliveryScheduleDetail[i].Day8, supply.DeliveryScheduleDetail[i].Day9, supply.DeliveryScheduleDetail[i].Day10, supply.DeliveryScheduleDetail[i].Day11, supply.DeliveryScheduleDetail[i].Day12, supply.DeliveryScheduleDetail[i].Day13, supply.DeliveryScheduleDetail[i].Day14, supply.DeliveryScheduleDetail[i].Day15, supply.DeliveryScheduleDetail[i].Day16, supply.DeliveryScheduleDetail[i].Day17, supply.DeliveryScheduleDetail[i].Day18, supply.DeliveryScheduleDetail[i].Day19, supply.DeliveryScheduleDetail[i].Day20, supply.DeliveryScheduleDetail[i].Day21, supply.DeliveryScheduleDetail[i].Day22, supply.DeliveryScheduleDetail[i].Day23, supply.DeliveryScheduleDetail[i].Day24, supply.DeliveryScheduleDetail[i].Day25, supply.DeliveryScheduleDetail[i].Day26, supply.DeliveryScheduleDetail[i].Day27, supply.DeliveryScheduleDetail[i].Day28, supply.DeliveryScheduleDetail[i].Day29, supply.DeliveryScheduleDetail[i].Day30, supply.DeliveryScheduleDetail[i].Day31);
				}
				grdHistoryGridDetail.DataSource = dtdet;
				grdHistoryGridDetail.RetrieveStructure();
				HistorygridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void HistorygridSetting()
	{
		try
		{
			grdHistoryGridDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["OrderNo"].Width = 50;
			grdHistoryGridDetail.RootTable.Columns["Item"].Width = 300;
			grdHistoryGridDetail.RootTable.Columns["UOM"].Width = 100;
			grdHistoryGridDetail.RootTable.Columns["Qty"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["Month"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D1"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D2"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D3"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D4"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D5"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D6"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D7"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D8"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D9"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D10"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D11"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D12"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D13"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D14"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D15"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D16"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D17"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D18"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D19"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D20"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D21"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D22"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D23"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D24"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D25"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D26"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D27"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D28"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D29"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D30"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D31"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["UOM"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["Qty"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["Month"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D1"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D2"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D3"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D4"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D5"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D6"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D7"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D8"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D9"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D10"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D11"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D12"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D13"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D14"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D15"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D16"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D17"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D18"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D19"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D20"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D21"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D22"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D23"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D24"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D25"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D26"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D27"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D28"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D29"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D30"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["D31"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["OrderNo"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["UOM"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["Qty"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["Month"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D1"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D2"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D3"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D4"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D5"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D6"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D7"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D8"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D9"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D10"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D11"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D12"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D13"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D14"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D15"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D16"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D17"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D18"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D19"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D20"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D21"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D22"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D23"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D24"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D25"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D26"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D27"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D28"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D29"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D30"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["D31"].FilterEditType = (FilterEditType)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadDataDetailAgainstSaleOrder(ReportsParameters sale)
	{
		try
		{
			DataTable dtSod = new DataTable();
			dtSod = SaleOrder.GetSaleOrderDetailForDeliverySchedule(sale);
			if (dtSod.Rows.Count > 0)
			{
				cmbSupplier.Value = dtSod.Rows[0]["OrderSupCustId"];
				((Control)(object)cmbSupplier).Enabled = false;
				txtFromDate.Enabled = false;
				for (int i = 0; i < dtSod.Rows.Count; i++)
				{
					dtdetail.Rows.Add(dtSod.Rows[i]["Id"], dtSod.Rows[i]["DocNo"], dtSod.Rows[i]["ItemId"], dtSod.Rows[i]["ItemName"], dtSod.Rows[i]["OrderItemUOMId"], dtSod.Rows[i]["UOMCode"], dtSod.Rows[i]["OrderItemQty"], txtFromDate.Value.ToString("MMM-yyyy"), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				GridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadInGridDetail()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		try
		{
			dtdetail.Rows.Clear();
			for (int i = 0; i < LoadSupply.dtSupply.Rows.Count; i++)
			{
				ReportsParameters sale = new ReportsParameters();
				sale.Id = int.Parse(LoadSupply.dtSupply.Rows[i][0].ToString());
				sale.OrganizationId = UserAccount.OrganizationId;
				sale.CompanyId = UserAccount.CompanyId;
				sale.DocumentTypeId = 82;
				sale.FromDate = Conversion.ToDateTime((object)null);
				sale.ToDate = Conversion.ToDateTime((object)null);
				LoadDataDetailAgainstSaleOrder(sale);
			}
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			GridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnLoadOrder_Click(object sender, EventArgs e)
	{
		try
		{
			SupplySelected = true;
			timer1.Enabled = true;
			LoadSupply = new LoadSaleOrder(UserAccount);
			SupplierId = Convert.ToInt32(cmbSupplier.Value);
			if (SupplierId > 0)
			{
				LoadSupply.ShowDialog();
				LoadSupply.BringToFront();
			}
			else
			{
				MessageBox.Show("Customer Select First");
				cmbSupplier.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			if (SupplySelected && LoadSupply.dtSupply != null && LoadSupply.dtSupply.Rows.Count > 0)
			{
				Refresh();
				LoadInGridDetail();
				LoadSupply.dtSupply.Rows.Clear();
				timer1.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocnumber_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			string code = DeliverySchedule.GetIdbyDocNo(new DeliverySchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ThisDocumentTypeId = 88,
				DocNo = Conversion.ToInt((object)txtdocnumber.Text.Trim())
			});
			if (Conversion.ToInt((object)code) > 0)
			{
				RecId = Conversion.ToInt((object)code);
				ReadById(RecId);
			}
			else
			{
				formReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		formReset();
	}

	private void button3_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters header = new ReportsParameters();
			header.OrganizationId = UserAccount.OrganizationId;
			header.CompanyId = UserAccount.CompanyId;
			header.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			if (!header.CanViewAllRecord)
			{
				header.EntryUser = UserAccount.ID;
			}
			DataTable dthis = new DataTable();
			dthis = DeliverySchedule.GetAll(header);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocNo");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("Customer");
			dt.Columns.Add("SupplierSchedule");
			dt.Columns.Add("CustomerSchedule");
			dt.Columns.Add("PaymentSchedule");
			dt.Columns.Add("ProductionSchedule");
			dt.Columns.Add("FromDate");
			dt.Columns.Add("ToDate");
			dt.Columns.Add("Remarks");
			dt.Columns.Add("Attachments");
			for (int i = 0; i < dthis.Rows.Count; i++)
			{
				dt.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocNo"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["CustomerName"], dthis.Rows[i]["SupplierSchCode"], dthis.Rows[i]["CustomerSchCode"], dthis.Rows[i]["PaymentSchCode"], dthis.Rows[i]["ProductionSchCode"], Conversion.ToDateTime(dthis.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dthis.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["Remarks"], dthis.Rows[i]["NoOfAttachments"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			HistoryDataGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			bindGrid();
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		try
		{
			DataTable dt = GeneralReprots.DeliveryScheduleSlip(RecId);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting rpt = new Reporting();
			if (UserAccount.CompAddress != string.Empty)
			{
				rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			}
			if (UserAccount.CompName != string.Empty)
			{
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			}
			rpt.ShowReportWithDataTable(dt, "460-InvRptDeliverySchedule.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 88,
					Id = RecId,
					EntryUser = UserAccount.ID
				});
				MessageBox.Show("Delete Record Seccessfully");
				formReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			int Code = Conversion.ToInt(DataGridHistory.CurrentRow.Cells[1].Value);
			if (Code <= 0)
			{
				return;
			}
			DataTable dt = GeneralReprots.DeliveryScheduleSlip(Code);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting rpt = new Reporting();
			if (UserAccount.CompAddress != string.Empty)
			{
				rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			}
			if (UserAccount.CompName != string.Empty)
			{
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			}
			rpt.ShowReportWithDataTable(dt, "460-InvRptDeliverySchedule.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		braches();
		Project();
		GenerateDocNo();
		customerbind();
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.UpdateData();
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				double D20 = Conversion.ToDouble((object)item.Cells["D2"].Value.ToString());
				double num = Conversion.ToDouble((object)item.Cells["D1"].Value.ToString());
				double D31 = Conversion.ToDouble((object)item.Cells["D3"].Value.ToString());
				double D34 = Conversion.ToDouble((object)item.Cells["D4"].Value.ToString());
				double D35 = Conversion.ToDouble((object)item.Cells["D5"].Value.ToString());
				double D36 = Conversion.ToDouble((object)item.Cells["D6"].Value.ToString());
				double D37 = Conversion.ToDouble((object)item.Cells["D7"].Value.ToString());
				double D38 = Conversion.ToDouble((object)item.Cells["D8"].Value.ToString());
				double D39 = Conversion.ToDouble((object)item.Cells["D9"].Value.ToString());
				double D10 = Conversion.ToDouble((object)item.Cells["D10"].Value.ToString());
				double D11 = Conversion.ToDouble((object)item.Cells["D11"].Value.ToString());
				double D12 = Conversion.ToDouble((object)item.Cells["D12"].Value.ToString());
				double D13 = Conversion.ToDouble((object)item.Cells["D13"].Value.ToString());
				double D14 = Conversion.ToDouble((object)item.Cells["D14"].Value.ToString());
				double D15 = Conversion.ToDouble((object)item.Cells["D15"].Value.ToString());
				double D16 = Conversion.ToDouble((object)item.Cells["D16"].Value.ToString());
				double D17 = Conversion.ToDouble((object)item.Cells["D17"].Value.ToString());
				double D18 = Conversion.ToDouble((object)item.Cells["D18"].Value.ToString());
				double D19 = Conversion.ToDouble((object)item.Cells["D19"].Value.ToString());
				double D21 = Conversion.ToDouble((object)item.Cells["D20"].Value.ToString());
				double D22 = Conversion.ToDouble((object)item.Cells["D21"].Value.ToString());
				double D23 = Conversion.ToDouble((object)item.Cells["D22"].Value.ToString());
				double D24 = Conversion.ToDouble((object)item.Cells["D23"].Value.ToString());
				double D25 = Conversion.ToDouble((object)item.Cells["D24"].Value.ToString());
				double D26 = Conversion.ToDouble((object)item.Cells["D25"].Value.ToString());
				double D27 = Conversion.ToDouble((object)item.Cells["D26"].Value.ToString());
				double D28 = Conversion.ToDouble((object)item.Cells["D27"].Value.ToString());
				double D29 = Conversion.ToDouble((object)item.Cells["D28"].Value.ToString());
				double D30 = Conversion.ToDouble((object)item.Cells["D29"].Value.ToString());
				double D32 = Conversion.ToDouble((object)item.Cells["D30"].Value.ToString());
				double D33 = Conversion.ToDouble((object)item.Cells["D31"].Value.ToString());
				double Amount = num + D20 + D31 + D34 + D35 + D36 + D37 + D38 + D39 + D10 + D11 + D12 + D13 + D14 + D15 + D16 + D17 + D18 + D19 + D21 + D22 + D23 + D24 + D25 + D26 + D27 + D28 + D29 + D30 + D32 + D33;
				item.Cells["Qty"].Value = Amount;
			}
			grdDetail.UpdateData();
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Expected O, but got Unknown
		//IL_2b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b34: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SaleTrading.frmDeliverySchedule));
		GridEXLayout grdHistoryGridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtPaymentSch = new System.Windows.Forms.TextBox();
		this.txtProductionsch = new System.Windows.Forms.TextBox();
		this.txtCustomerSch = new System.Windows.Forms.TextBox();
		this.txtSupplierSch = new System.Windows.Forms.TextBox();
		this.cmbSupplier = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnLoadOrder = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdHistoryGridDetail = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
		this.btnHistoryRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryGridDetail).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1320, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1320, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1312, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1306, 717);
		this.panel2.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 136);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1306, 581);
		this.panel7.TabIndex = 4;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1306, 581);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 126);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1306, 10);
		this.panel6.TabIndex = 3;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.chkPrint);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.txtRemarks);
		this.panel5.Controls.Add(this.txtToDate);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.txtFromDate);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.txtPaymentSch);
		this.panel5.Controls.Add(this.txtProductionsch);
		this.panel5.Controls.Add(this.txtCustomerSch);
		this.panel5.Controls.Add(this.txtSupplierSch);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel5.Controls.Add(this.label18);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add(this.txtDocdate);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.txtdocnumber);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 30);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1306, 96);
		this.panel5.TabIndex = 2;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Location = new System.Drawing.Point(1042, 74);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(15, 14);
		this.chkPrint.TabIndex = 11;
		this.chkPrint.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(553, 44);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(57, 15);
		this.label11.TabIndex = 54;
		this.label11.Text = "Remarks";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(648, 41);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(388, 21);
		this.txtRemarks.TabIndex = 6;
		this.txtToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(217, 115);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(163, 21);
		this.txtToDate.TabIndex = 11;
		this.txtToDate.Visible = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(138, 118);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(50, 15);
		this.label10.TabIndex = 52;
		this.label10.Text = "To Date";
		this.label10.Visible = false;
		this.txtFromDate.CustomFormat = "MMM-yyyy";
		this.txtFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(385, 41);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(163, 21);
		this.txtFromDate.TabIndex = 5;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(296, 44);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(42, 15);
		this.label9.TabIndex = 50;
		this.label9.Text = "Month";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(781, 70);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 48;
		this.label7.Text = "Payment Sch";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(553, 70);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(90, 15);
		this.label6.TabIndex = 48;
		this.label6.Text = "Production Sch";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(296, 69);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(84, 15);
		this.label5.TabIndex = 48;
		this.label5.Text = "Customer Sch";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 70);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 15);
		this.label4.TabIndex = 48;
		this.label4.Text = "supplierSch";
		this.txtPaymentSch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPaymentSch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPaymentSch.Location = new System.Drawing.Point(877, 67);
		this.txtPaymentSch.Name = "txtPaymentSch";
		this.txtPaymentSch.Size = new System.Drawing.Size(159, 21);
		this.txtPaymentSch.TabIndex = 10;
		this.txtProductionsch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionsch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionsch.Location = new System.Drawing.Point(648, 67);
		this.txtProductionsch.Name = "txtProductionsch";
		this.txtProductionsch.Size = new System.Drawing.Size(126, 21);
		this.txtProductionsch.TabIndex = 9;
		this.txtCustomerSch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerSch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerSch.Location = new System.Drawing.Point(385, 67);
		this.txtCustomerSch.Name = "txtCustomerSch";
		this.txtCustomerSch.Size = new System.Drawing.Size(163, 21);
		this.txtCustomerSch.TabIndex = 8;
		this.txtSupplierSch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierSch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierSch.Location = new System.Drawing.Point(86, 67);
		this.txtSupplierSch.Name = "txtSupplierSch";
		this.txtSupplierSch.Size = new System.Drawing.Size(204, 21);
		this.txtSupplierSch.TabIndex = 7;
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(86, 39);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(204, 24);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(9, 44);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(53, 15);
		this.label8.TabIndex = 42;
		this.label8.Text = "Supplier";
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(385, 13);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(163, 24);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(298, 16);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(45, 15);
		this.label18.TabIndex = 46;
		this.label18.Text = "Project";
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance26;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(86, 13);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(204, 24);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(9, 15);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 15);
		this.label13.TabIndex = 45;
		this.label13.Text = "Branch";
		this.txtDocdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocdate.Location = new System.Drawing.Point(911, 13);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(125, 21);
		this.txtDocdate.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(812, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 40;
		this.label2.Text = "Document Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(553, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 39;
		this.label1.Text = "Document No";
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(648, 13);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.Size = new System.Drawing.Size(158, 21);
		this.txtdocnumber.TabIndex = 2;
		this.txtdocnumber.Leave += new System.EventHandler(txtdocnumber_Leave);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1306, 30);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripButton6, this.btnnew, this.btnsave, this.btnUpdate, this.Print, this.btnLoadOrder, this.btnDelete, this.btnAttachment, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1306, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton6.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(163, 24);
		this.toolStripButton6.Text = "Delivery Schedule       ";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(79, 24);
		this.Print.Text = "460-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnLoadOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadOrder.Name = "btnLoadOrder";
		this.btnLoadOrder.Size = new System.Drawing.Size(70, 24);
		this.btnLoadOrder.Text = "Load Order";
		this.btnLoadOrder.Click += new System.EventHandler(btnLoadOrder_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1312, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel4);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1306, 717);
		this.panel8.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 349);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1306, 368);
		this.panel4.TabIndex = 3;
		this.grdHistoryGridDetail.AllowDelete = (InheritableBoolean)1;
		grdHistoryGridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryGridDetail.DesignTimeLayout = grdHistoryGridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryGridDetail.DynamicFiltering = true;
		this.grdHistoryGridDetail.FilterMode = (FilterMode)2;
		this.grdHistoryGridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryGridDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail).Name = "grdHistoryGridDetail";
		((ScrollableBase)this.grdHistoryGridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail).Size = new System.Drawing.Size(1306, 368);
		((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail).TabIndex = 47;
		this.grdHistoryGridDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryGridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryGridDetail).VisualStyle = (VisualStyle)4;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 29);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1306, 320);
		this.panel11.TabIndex = 2;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1306, 320);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 46;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Click += new System.EventHandler(DataGridHistory_Click);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel9.Controls.Add(this.toolStrip2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1306, 29);
		this.panel9.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripButton7, this.btnHistoryRefresh, this.BtnPrint, this.toolStripSeparator1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1306, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton7.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton7.Name = "toolStripButton7";
		this.toolStripButton7.Size = new System.Drawing.Size(78, 24);
		this.toolStripButton7.Text = "History    ";
		this.btnHistoryRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnHistoryRefresh.Name = "btnHistoryRefresh";
		this.btnHistoryRefresh.Size = new System.Drawing.Size(51, 24);
		this.btnHistoryRefresh.Text = "&LoadAll";
		this.btnHistoryRefresh.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(56, 24);
		this.BtnPrint.Text = "&Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1320, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmDeliverySchedule";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmSupplySchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSupplySchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryGridDetail).EndInit();
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
