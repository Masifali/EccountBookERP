// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.PurchaseTrading;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PurchaseTrading;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.PurchaseTrading
{

public class frmSupplySchedule : Form
{
	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private double qtysum;

	private bool UpdateMode;

	private bool chkstatus;

	private int updateDetailIndex;

	private bool clickCheck;

	private DataTable dtdetail = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	public string GRNS = string.Empty;

	public bool SupplySelected;

	private LoadPurchaseOrder LoadSupply;

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

	internal UltraCombo comCompany;

	private Label label3;

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

	private GroupBox groupBox1;

	private Label label12;

	internal UltraCombo cmbPackUOM;

	private Label label31;

	internal UltraCombo cmbItem;

	private Label label28;

	private TextBox txtQty;

	private Label label14;

	private Label label16;

	private TextBox txtd26;

	private TextBox txtd20;

	private TextBox txtd13;

	private TextBox txtd12;

	private TextBox txtd23;

	private TextBox txtd6;

	private TextBox txtd17;

	private TextBox txtd29;

	private TextBox txtd9;

	private TextBox txtd3;

	private Label label43;

	private Label label30;

	private Label label42;

	private Label label50;

	private Label label23;

	private Label label41;

	private Label label29;

	private TextBox txtd25;

	private Label label40;

	private Label label49;

	private Label label20;

	private TextBox txtd19;

	private TextBox txtd31;

	private TextBox txtd11;

	private TextBox txtd22;

	private TextBox txtd5;

	private TextBox txtd16;

	private TextBox txtd28;

	private TextBox txtd8;

	private TextBox txtd2;

	private Label label39;

	private Label label27;

	private Label label38;

	private Label label48;

	private Label label22;

	private TextBox txtd24;

	private Label label37;

	private Label label26;

	private TextBox txtd18;

	private Label label36;

	private Label label47;

	private Label label19;

	private TextBox txtd30;

	private TextBox txtd10;

	private TextBox txtd4;

	private TextBox txtd21;

	private Label label35;

	private Label label25;

	private TextBox txtd14;

	private Label label34;

	private Label label46;

	private Label label21;

	private TextBox txtd15;

	private TextBox txtd7;

	private TextBox txtd27;

	private Label label33;

	private Label label45;

	private Label label24;

	private TextBox txtd1;

	private Label label44;

	private Label label32;

	private Label label17;

	private Button btnCancel;

	private Button btnGridUpdate;

	private Button btnAddinGrid;

	internal GridEX grdDetail;

	private DateTimePicker txtmonthdate;

	private Panel panel8;

	private Panel panel11;

	private Panel panel9;

	private ToolStrip toolStrip2;

	private ToolStripButton btnHistoryRefresh;

	private ToolStripButton BtnPrint;

	private ToolStripSeparator toolStripSeparator1;

	internal GridEX DataGridHistory;

	internal UltraCombo cmbOrderNo;

	private Button button3;

	private ToolStripButton toolStripButton6;

	private ToolStripButton toolStripButton7;

	private Panel panel4;

	internal GridEX grdHistoryGridDetail;

	private CheckBox chkPrint;

	private ToolStripButton btnLoadOrder;

	private Timer timer1;

	public frmSupplySchedule(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSupplySchedule_Load(object sender, EventArgs e)
	{
		try
		{
			Company();
			braches();
			Project();
			item();
			GenerateDocNo();
			bindGrid();
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

	public void GenerateDocNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			string code = SupplySchedule.GenerateCode(new SupplySchedule
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			if (code != null)
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
			DataTable dtbranch = CommonServices.BrancheServiceBind();
			if (dtbranch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtbranch, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Company()
	{
		try
		{
			DataTable dt = CommonServices.CompanyServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, comCompany, "Id", "CompName", "CompanyName", false);
				((UltraGridBase)comCompany).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void customerbind()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbSupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void item()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			List<Item> lst = Item.GetList(new Item());
			DataTable dtItemCombo = new DataTable();
			dtItemCombo.Columns.Add("Id");
			dtItemCombo.Columns.Add("ItemName");
			for (int i = 0; i < lst.Count; i++)
			{
				dtItemCombo.Rows.Add(lst[i].Id, lst[i].ItemName);
			}
			if (dtItemCombo.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemCombo, cmbItem, "Id", "ItemName", "Item", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void PurchaseOrder()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			PurchaseOrder val = new PurchaseOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				OrderSupCustId = Conversion.ToInt(cmbSupplier.Value),
				DocumentTypeId = 42
			};
			DataTable dtgetgrn = new DataTable();
			dtgetgrn = PurchaseOrder.ReadForInvGrn(val);
			if (dtgetgrn.Rows.Count > 0)
			{
				DataTable dtItemCombo = new DataTable();
				dtItemCombo.Columns.Add("Id");
				dtItemCombo.Columns.Add("DocNo");
				for (int i = 0; i < dtgetgrn.Rows.Count; i++)
				{
					dtItemCombo.Rows.Add(dtgetgrn.Rows[i]["Id"], dtgetgrn.Rows[i]["DocNo"]);
				}
				if (dtItemCombo.Rows.Count > 0)
				{
					DDL.BindDDL(dtItemCombo, cmbOrderNo, "Id", "DocNo", "Order No", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void PackUOM()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(cmbItem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtuomsearch = new DataTable();
			dtuomsearch = UOMSchedule.SearchByObject(val);
			if (dtuomsearch.Rows.Count > 0)
			{
				DataTable dtCOMMUOM = new DataTable();
				dtCOMMUOM.Columns.Add("Id");
				dtCOMMUOM.Columns.Add("Equivalent");
				for (int i = 0; i < dtuomsearch.Rows.Count; i++)
				{
					dtCOMMUOM.Rows.Add(dtuomsearch.Rows[i]["Id"], dtuomsearch.Rows[i]["Equivalent"]);
				}
				if (dtCOMMUOM.Rows.Count > 0)
				{
					DDL.BindDDL(dtCOMMUOM, cmbPackUOM, "Id", "Equivalent", "Unit Of Measure ", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		PackUOM();
	}

	public void Cal()
	{
		try
		{
			double Qty = 0.0;
			qtysum = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow a in rows)
			{
				qtysum += Conversion.ToDouble((object)a.Cells["D1"].Text) + Conversion.ToDouble((object)a.Cells["D2"].Text) + Conversion.ToDouble((object)a.Cells["D3"].Text) + Conversion.ToDouble((object)a.Cells["D4"].Text) + Conversion.ToDouble((object)a.Cells["D5"].Text) + Conversion.ToDouble((object)a.Cells["D6"].Text) + Conversion.ToDouble((object)a.Cells["D7"].Text) + Conversion.ToDouble((object)a.Cells["D8"].Text) + Conversion.ToDouble((object)a.Cells["D9"].Text) + Conversion.ToDouble((object)a.Cells["D10"].Text) + Conversion.ToDouble((object)a.Cells["D11"].Text) + Conversion.ToDouble((object)a.Cells["D12"].Text) + Conversion.ToDouble((object)a.Cells["D13"].Text) + Conversion.ToDouble((object)a.Cells["D14"].Text) + Conversion.ToDouble((object)a.Cells["D15"].Text) + Conversion.ToDouble((object)a.Cells["D16"].Text) + Conversion.ToDouble((object)a.Cells["D17"].Text) + Conversion.ToDouble((object)a.Cells["D18"].Text) + Conversion.ToDouble((object)a.Cells["D19"].Text) + Conversion.ToDouble((object)a.Cells["D20"].Text) + Conversion.ToDouble((object)a.Cells["D21"].Text) + Conversion.ToDouble((object)a.Cells["D22"].Text) + Conversion.ToDouble((object)a.Cells["D23"].Text) + Conversion.ToDouble((object)a.Cells["D24"].Text) + Conversion.ToDouble((object)a.Cells["D25"].Text) + Conversion.ToDouble((object)a.Cells["D26"].Text) + Conversion.ToDouble((object)a.Cells["D27"].Text) + Conversion.ToDouble((object)a.Cells["D28"].Text) + Conversion.ToDouble((object)a.Cells["D29"].Text) + Conversion.ToDouble((object)a.Cells["D30"].Text) + Conversion.ToDouble((object)a.Cells["D31"].Text);
				Qty += Conversion.ToDouble(a.Cells["Qty"].Value);
			}
			if (Qty != Conversion.ToDouble((object)qtysum))
			{
				throw new Exception("Quantity Not Matched");
			}
			chkstatus = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Expected O, but got Unknown
		try
		{
			if (!FormValidation() || MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GenerateDocNo();
			SupplySchedule supply = new SupplySchedule();
			supply.ThisDocumentTypeId = 65;
			supply.OrganizationId = UserAccount.OrganizationId;
			supply.CompanyId = Conversion.ToInt(comCompany.Value);
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
			supply.EntryUser = UserAccount.ID;
			supply.EntryDate = DateTime.Now;
			supply.ModifyUser = UserAccount.ID;
			supply.ModifyDate = DateTime.Now;
			supply.PostDate = DateTime.Now;
			supply.PostState = false;
			supply.PostUser = UserAccount.ID;
			supply.supplyScheduleDetail = new List<SupplyScheduleDetail>();
			supply.supplyScheduleDetaildate = new List<SupplyScheduleDetailDates>();
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
				SupplyScheduleDetail scheduleDetail = new SupplyScheduleDetail();
				scheduleDetail.OrderId = Conversion.ToInt(a2.Cells["OrderId"].Value);
				scheduleDetail.ItemId = Conversion.ToInt(a2.Cells["ItemId"].Value);
				scheduleDetail.ItemUOMId = Conversion.ToInt(a2.Cells["UOMId"].Value);
				scheduleDetail.TotalQty = Conversion.ToDouble((object)a2.Cells["Qty"].Text);
				scheduleDetail.MonthDesc = Conversion.ToDateTime((object)a2.Cells["Month"].Text);
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
				supply.supplyScheduleDetail.Add(scheduleDetail);
			}
			rows = grdDetail.GetRows();
			foreach (GridEXRow a in rows)
			{
				int z = 8;
				int b = 1;
				for (int i = 0; i < 39; i++)
				{
					if (z == 39)
					{
						break;
					}
					if (Conversion.ToInt(a.Cells[z].Value) != 0 && a.Cells[z].Text != string.Empty)
					{
						SupplyScheduleDetailDates scheduleDetailDates = new SupplyScheduleDetailDates();
						scheduleDetailDates.OrderId = Conversion.ToInt(a.Cells["OrderId"].Value);
						scheduleDetailDates.ItemId = Conversion.ToInt(a.Cells["ItemId"].Value);
						scheduleDetailDates.ItemUOMId = Conversion.ToInt(a.Cells["UOMId"].Value);
						scheduleDetailDates.TotalQty = Conversion.ToDouble((object)a.Cells["Qty"].Text);
						scheduleDetailDates.MonthDesc = Conversion.ToDateTime((object)a.Cells["Month"].Text);
						scheduleDetailDates.SchDate = Conversion.ToDateTime((object)(b + "-" + a.Cells["Month"].Text));
						scheduleDetailDates.SchQty = Conversion.ToDouble((object)a.Cells[z].Text);
						supply.supplyScheduleDetaildate.Add(scheduleDetailDates);
					}
					z++;
					b++;
				}
			}
			int success = SupplySchedule.Save(supply);
			if (success <= 0)
			{
				return;
			}
			MessageBox.Show("Save SuccessFully");
			formReset();
			DetailReset();
			if (chkPrint.Checked)
			{
				Reporting rpt = new Reporting();
				if (UserAccount.CompAddress != string.Empty)
				{
					rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				}
				if (UserAccount.CompName != string.Empty)
				{
					rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				}
				rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				rpt.RptPerameter("@hId", success.ToString());
				rpt.ShowReport("RptInvSupplySchedule.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Expected O, but got Unknown
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Expected O, but got Unknown
		try
		{
			if (!FormValidation() || MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			SupplySchedule supply = new SupplySchedule();
			supply.Id = RecId;
			supply.ThisDocumentTypeId = 65;
			supply.OrganizationId = UserAccount.OrganizationId;
			supply.CompanyId = Conversion.ToInt(comCompany.Value);
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
			supply.EntryDate = DateTime.Now;
			supply.ModifyUser = UserAccount.ID;
			supply.ModifyDate = DateTime.Now;
			supply.PostDate = DateTime.Now;
			supply.PostState = false;
			supply.PostUser = UserAccount.ID;
			supply.supplyScheduleDetail = new List<SupplyScheduleDetail>();
			supply.supplyScheduleDetaildate = new List<SupplyScheduleDetailDates>();
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
				SupplyScheduleDetail scheduleDetail = new SupplyScheduleDetail();
				scheduleDetail.OrderId = Conversion.ToInt(a2.Cells["OrderId"].Value);
				scheduleDetail.ItemId = Conversion.ToInt(a2.Cells["ItemId"].Value);
				scheduleDetail.ItemUOMId = Conversion.ToInt(a2.Cells["UOMId"].Value);
				scheduleDetail.TotalQty = Conversion.ToDouble((object)a2.Cells["Qty"].Text);
				scheduleDetail.MonthDesc = Conversion.ToDateTime((object)a2.Cells["Month"].Text);
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
				supply.supplyScheduleDetail.Add(scheduleDetail);
			}
			rows = grdDetail.GetRows();
			foreach (GridEXRow a in rows)
			{
				int z = 8;
				int b = 1;
				for (int i = 0; i < 39; i++)
				{
					if (z == 39)
					{
						break;
					}
					if (Conversion.ToInt(a.Cells[z].Value) != 0 && a.Cells[z].Text != string.Empty)
					{
						SupplyScheduleDetailDates scheduleDetailDates = new SupplyScheduleDetailDates();
						scheduleDetailDates.OrderId = Conversion.ToInt(a.Cells["OrderId"].Value);
						scheduleDetailDates.ItemId = Conversion.ToInt(a.Cells["ItemId"].Value);
						scheduleDetailDates.ItemUOMId = Conversion.ToInt(a.Cells["UOMId"].Value);
						scheduleDetailDates.TotalQty = Conversion.ToDouble((object)a.Cells["Qty"].Text);
						scheduleDetailDates.MonthDesc = Conversion.ToDateTime((object)a.Cells["Month"].Text);
						scheduleDetailDates.SchDate = Conversion.ToDateTime((object)(b + "-" + a.Cells["Month"].Text));
						scheduleDetailDates.SchQty = Conversion.ToDouble((object)a.Cells[z].Text);
						supply.supplyScheduleDetaildate.Add(scheduleDetailDates);
					}
					z++;
					b++;
				}
			}
			int success = SupplySchedule.Save(supply);
			if (success <= 0)
			{
				return;
			}
			MessageBox.Show("Update SuccessFully");
			formReset();
			DetailReset();
			if (chkPrint.Checked)
			{
				Reporting rpt = new Reporting();
				if (UserAccount.CompAddress != string.Empty)
				{
					rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				}
				if (UserAccount.CompName != string.Empty)
				{
					rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				}
				rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				rpt.RptPerameter("@hId", success.ToString());
				rpt.ShowReport("RptInvSupplySchedule.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
	}

	public void GridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["OrderNo"].Width = 50;
			grdDetail.RootTable.Columns["Item"].Width = 200;
			grdDetail.RootTable.Columns["UOM"].Width = 100;
			grdDetail.RootTable.Columns["Qty"].Width = 40;
			grdDetail.RootTable.Columns["Month"].Width = 100;
			grdDetail.RootTable.Columns["D1"].Width = 40;
			grdDetail.RootTable.Columns["D2"].Width = 40;
			grdDetail.RootTable.Columns["D3"].Width = 40;
			grdDetail.RootTable.Columns["D4"].Width = 40;
			grdDetail.RootTable.Columns["D5"].Width = 40;
			grdDetail.RootTable.Columns["D6"].Width = 40;
			grdDetail.RootTable.Columns["D7"].Width = 40;
			grdDetail.RootTable.Columns["D8"].Width = 40;
			grdDetail.RootTable.Columns["D9"].Width = 40;
			grdDetail.RootTable.Columns["D10"].Width = 40;
			grdDetail.RootTable.Columns["D11"].Width = 40;
			grdDetail.RootTable.Columns["D12"].Width = 40;
			grdDetail.RootTable.Columns["D13"].Width = 40;
			grdDetail.RootTable.Columns["D14"].Width = 40;
			grdDetail.RootTable.Columns["D15"].Width = 40;
			grdDetail.RootTable.Columns["D16"].Width = 40;
			grdDetail.RootTable.Columns["D17"].Width = 40;
			grdDetail.RootTable.Columns["D18"].Width = 40;
			grdDetail.RootTable.Columns["D19"].Width = 40;
			grdDetail.RootTable.Columns["D20"].Width = 40;
			grdDetail.RootTable.Columns["D21"].Width = 40;
			grdDetail.RootTable.Columns["D22"].Width = 40;
			grdDetail.RootTable.Columns["D23"].Width = 40;
			grdDetail.RootTable.Columns["D24"].Width = 40;
			grdDetail.RootTable.Columns["D25"].Width = 40;
			grdDetail.RootTable.Columns["D26"].Width = 40;
			grdDetail.RootTable.Columns["D27"].Width = 40;
			grdDetail.RootTable.Columns["D28"].Width = 40;
			grdDetail.RootTable.Columns["D29"].Width = 40;
			grdDetail.RootTable.Columns["D30"].Width = 40;
			grdDetail.RootTable.Columns["D31"].Width = 40;
			grdDetail.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["UOM"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderNo"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["UOM"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grdDetail.RootTable.Columns["Month"].FilterEditType = (FilterEditType)8;
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

	public void GriddetailHistorySetting()
	{
		try
		{
			grdHistoryGridDetail.RootTable.Columns["OrderNo"].Width = 50;
			grdHistoryGridDetail.RootTable.Columns["Item"].Width = 200;
			grdHistoryGridDetail.RootTable.Columns["UOM"].Width = 100;
			grdHistoryGridDetail.RootTable.Columns["Qty"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["Month"].Width = 100;
			grdHistoryGridDetail.RootTable.Columns["D1"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D2"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D3"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D4"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D5"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D6"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D7"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D8"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D9"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D10"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D11"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D12"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D13"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D14"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D15"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D16"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D17"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D18"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D19"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D20"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D21"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D22"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D23"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D24"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D25"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D26"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D27"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D28"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D29"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D30"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D31"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["UOM"].EditType = (EditType)0;
			grdHistoryGridDetail.RootTable.Columns["OrderNo"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["UOM"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grdHistoryGridDetail.RootTable.Columns["Month"].FilterEditType = (FilterEditType)8;
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
			grdHistoryGridDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D1"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D2"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D3"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D4"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D5"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D6"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D7"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D8"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D9"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D10"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D11"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D12"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D13"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D14"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D15"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D16"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D17"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D18"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D19"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D20"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D21"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D22"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D23"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D24"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D25"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D26"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D27"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D28"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D29"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D30"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["D31"].AggregateFunction = (AggregateFunction)2;
			grdHistoryGridDetail.RootTable.Columns["OrderId"].Visible = false;
			grdHistoryGridDetail.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryGridDetail.RootTable.Columns["UOMId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindGrid()
	{
		try
		{
			DataTable dtgetall = new DataTable();
			dtgetall = SupplySchedule.GetAll();
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
			for (int i = 0; i < dtgetall.Rows.Count; i++)
			{
				dt.Rows.Add(dtgetall.Rows[i]["Id"], dtgetall.Rows[i]["DocNo"], Conversion.ToDateTime(dtgetall.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtgetall.Rows[i]["CustomerName"], dtgetall.Rows[i]["SupplierSchCode"], dtgetall.Rows[i]["CustomerSchCode"], dtgetall.Rows[i]["PaymentSchCode"], dtgetall.Rows[i]["ProductionSchCode"], Conversion.ToDateTime(dtgetall.Rows[i]["FromDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dtgetall.Rows[i]["ToDate"]).ToString("dd-MMM-yyyy"), dtgetall.Rows[i]["Remarks"]);
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
			DataGridHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Customer"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["SupplierSchedule"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["CustomerSchedule"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["PaymentSchedule"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ProductionSchedule"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["FromDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ToDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Remarks"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Customer"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["SupplierSchedule"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["CustomerSchedule"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["PaymentSchedule"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["ProductionSchedule"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["FromDate"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["ToDate"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)0;
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void formReset()
	{
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		UpdateMode = false;
		chkstatus = false;
		((Control)(object)cmbSupplier).Text = string.Empty;
		((Control)(object)cmbSupplier).Enabled = true;
		txtDocdate.Value = DateTime.Now;
		txtFromDate.Value = DateTime.Now;
		txtToDate.Value = DateTime.Now;
		txtSupplierSch.Text = string.Empty;
		txtProductionsch.Text = string.Empty;
		txtPaymentSch.Text = string.Empty;
		txtCustomerSch.Text = string.Empty;
		txtRemarks.Text = string.Empty;
		grdDetail.ClearStructure();
		txtDocdate.Focus();
		GenerateDocNo();
	}

	public void DetailReset()
	{
		((Control)(object)cmbOrderNo).Text = string.Empty;
		((Control)(object)cmbItem).Text = string.Empty;
		((Control)(object)cmbPackUOM).Text = string.Empty;
		txtQty.Text = string.Empty;
		txtmonthdate.Text = string.Empty;
		txtd1.Text = "0";
		txtd2.Text = "0";
		txtd3.Text = "0";
		txtd4.Text = "0";
		txtd5.Text = "0";
		txtd6.Text = "0";
		txtd7.Text = "0";
		txtd8.Text = "0";
		txtd9.Text = "0";
		txtd10.Text = "0";
		txtd11.Text = "0";
		txtd12.Text = "0";
		txtd13.Text = "0";
		txtd14.Text = "0";
		txtd15.Text = "0";
		txtd16.Text = "0";
		txtd17.Text = "0";
		txtd18.Text = "0";
		txtd19.Text = "0";
		txtd20.Text = "0";
		txtd21.Text = "0";
		txtd22.Text = "0";
		txtd23.Text = "0";
		txtd24.Text = "0";
		txtd25.Text = "0";
		txtd26.Text = "0";
		txtd27.Text = "0";
		txtd28.Text = "0";
		txtd29.Text = "0";
		txtd30.Text = "0";
		txtd31.Text = "0";
		btnGridUpdate.Visible = false;
		btnCancel.Visible = false;
		btnAddinGrid.Visible = true;
	}

	private bool FormValidation()
	{
		if (((Control)(object)comCompany).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Company Field Required");
			cmbSupplier.Focus();
			return false;
		}
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

	private bool FormValidationDetail()
	{
		if (((Control)(object)cmbItem).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Item Field Required");
			cmbItem.Focus();
			return false;
		}
		if (((Control)(object)cmbPackUOM).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Pack UOM  Field Required");
			cmbPackUOM.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty)
		{
			MessageBox.Show(" Qty  Field Required");
			txtQty.Focus();
			return false;
		}
		if (txtd1.Text == string.Empty && txtd2.Text == string.Empty && txtd3.Text == string.Empty && txtd4.Text == string.Empty && txtd5.Text == string.Empty && txtd6.Text == string.Empty && txtd7.Text == string.Empty && txtd8.Text == string.Empty && txtd9.Text == string.Empty && txtd10.Text == string.Empty && txtd11.Text == string.Empty && txtd12.Text == string.Empty && txtd13.Text == string.Empty && txtd14.Text == string.Empty && txtd15.Text == string.Empty && txtd16.Text == string.Empty && txtd17.Text == string.Empty && txtd18.Text == string.Empty && txtd19.Text == string.Empty && txtd20.Text == string.Empty && txtd21.Text == string.Empty && txtd22.Text == string.Empty && txtd23.Text == string.Empty && txtd24.Text == string.Empty && txtd25.Text == string.Empty && txtd26.Text == string.Empty && txtd27.Text == string.Empty && txtd28.Text == string.Empty && txtd29.Text == string.Empty && txtd30.Text == string.Empty && txtd31.Text == string.Empty)
		{
			MessageBox.Show(" AtLeast Qty is Mandatory Required in any days");
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
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
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
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
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

	private void btnCancel_Click(object sender, EventArgs e)
	{
		btnGridUpdate.Visible = false;
		btnCancel.Visible = false;
		btnAddinGrid.Visible = true;
		DetailReset();
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
			SupplySchedule supply = new SupplySchedule();
			RecId = Id;
			supply = SupplySchedule.GetByID(RecId);
			comCompany.Value = supply.CompanyId;
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
			if (!clickCheck)
			{
				tabControl1.SelectedIndex = 0;
				for (int i = 0; i < supply.supplyScheduleDetail.Count; i++)
				{
					dtdetail.Rows.Add(supply.supplyScheduleDetail[i].OrderId, supply.supplyScheduleDetail[i].PurchaseOrderNo, supply.supplyScheduleDetail[i].ItemId, supply.supplyScheduleDetail[i].ItemName, supply.supplyScheduleDetail[i].ItemUOMId, supply.supplyScheduleDetail[i].UOMCode, supply.supplyScheduleDetail[i].TotalQty, supply.supplyScheduleDetail[i].MonthDesc.ToString("MMM-yyyy"), supply.supplyScheduleDetail[i].Day1, supply.supplyScheduleDetail[i].Day2, supply.supplyScheduleDetail[i].Day3, supply.supplyScheduleDetail[i].Day4, supply.supplyScheduleDetail[i].Day5, supply.supplyScheduleDetail[i].Day6, supply.supplyScheduleDetail[i].Day7, supply.supplyScheduleDetail[i].Day8, supply.supplyScheduleDetail[i].Day9, supply.supplyScheduleDetail[i].Day10, supply.supplyScheduleDetail[i].Day11, supply.supplyScheduleDetail[i].Day12, supply.supplyScheduleDetail[i].Day13, supply.supplyScheduleDetail[i].Day14, supply.supplyScheduleDetail[i].Day15, supply.supplyScheduleDetail[i].Day16, supply.supplyScheduleDetail[i].Day17, supply.supplyScheduleDetail[i].Day18, supply.supplyScheduleDetail[i].Day19, supply.supplyScheduleDetail[i].Day20, supply.supplyScheduleDetail[i].Day21, supply.supplyScheduleDetail[i].Day22, supply.supplyScheduleDetail[i].Day23, supply.supplyScheduleDetail[i].Day24, supply.supplyScheduleDetail[i].Day25, supply.supplyScheduleDetail[i].Day26, supply.supplyScheduleDetail[i].Day27, supply.supplyScheduleDetail[i].Day28, supply.supplyScheduleDetail[i].Day29, supply.supplyScheduleDetail[i].Day30, supply.supplyScheduleDetail[i].Day31);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				GridSetting();
				UpdateMode = true;
				return;
			}
			dtdetail.Clear();
			grdHistoryGridDetail.ClearStructure();
			for (int j = 0; j < supply.supplyScheduleDetail.Count; j++)
			{
				dtdetail.Rows.Add(supply.supplyScheduleDetail[j].OrderId, supply.supplyScheduleDetail[j].PurchaseOrderNo, supply.supplyScheduleDetail[j].ItemId, supply.supplyScheduleDetail[j].ItemName, supply.supplyScheduleDetail[j].ItemUOMId, supply.supplyScheduleDetail[j].UOMCode, supply.supplyScheduleDetail[j].TotalQty, supply.supplyScheduleDetail[j].MonthDesc.ToString("MMM-yyyy"), supply.supplyScheduleDetail[j].Day1, supply.supplyScheduleDetail[j].Day2, supply.supplyScheduleDetail[j].Day3, supply.supplyScheduleDetail[j].Day4, supply.supplyScheduleDetail[j].Day5, supply.supplyScheduleDetail[j].Day6, supply.supplyScheduleDetail[j].Day7, supply.supplyScheduleDetail[j].Day8, supply.supplyScheduleDetail[j].Day9, supply.supplyScheduleDetail[j].Day10, supply.supplyScheduleDetail[j].Day11, supply.supplyScheduleDetail[j].Day12, supply.supplyScheduleDetail[j].Day13, supply.supplyScheduleDetail[j].Day14, supply.supplyScheduleDetail[j].Day15, supply.supplyScheduleDetail[j].Day16, supply.supplyScheduleDetail[j].Day17, supply.supplyScheduleDetail[j].Day18, supply.supplyScheduleDetail[j].Day19, supply.supplyScheduleDetail[j].Day20, supply.supplyScheduleDetail[j].Day21, supply.supplyScheduleDetail[j].Day22, supply.supplyScheduleDetail[j].Day23, supply.supplyScheduleDetail[j].Day24, supply.supplyScheduleDetail[j].Day25, supply.supplyScheduleDetail[j].Day26, supply.supplyScheduleDetail[j].Day27, supply.supplyScheduleDetail[j].Day28, supply.supplyScheduleDetail[j].Day29, supply.supplyScheduleDetail[j].Day30, supply.supplyScheduleDetail[j].Day31);
			}
			grdHistoryGridDetail.DataSource = dtdetail;
			grdHistoryGridDetail.RetrieveStructure();
			GriddetailHistorySetting();
			clickCheck = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		formReset();
		DetailReset();
		Company();
		braches();
		Project();
		GenerateDocNo();
		bindGrid();
		customerbind();
		PurchaseOrder();
		grdDetail.ClearStructure();
		dtdetail.Clear();
	}

	private void button3_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void DataGridHistory_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			SupplySchedule supply = new SupplySchedule();
			int num = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString());
			clickCheck = true;
			ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString()));
			supply = SupplySchedule.GetByID(num);
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
				for (int i = 0; i < supply.supplyScheduleDetail.Count; i++)
				{
					dtdet.Rows.Add(supply.supplyScheduleDetail[i].PurchaseOrderNo, supply.supplyScheduleDetail[i].ItemName, supply.supplyScheduleDetail[i].UOMEquivalent, supply.supplyScheduleDetail[i].TotalQty, supply.supplyScheduleDetail[i].MonthDesc.ToString("MMM-yyyy"), supply.supplyScheduleDetail[i].Day1, supply.supplyScheduleDetail[i].Day2, supply.supplyScheduleDetail[i].Day3, supply.supplyScheduleDetail[i].Day4, supply.supplyScheduleDetail[i].Day5, supply.supplyScheduleDetail[i].Day6, supply.supplyScheduleDetail[i].Day7, supply.supplyScheduleDetail[i].Day8, supply.supplyScheduleDetail[i].Day9, supply.supplyScheduleDetail[i].Day10, supply.supplyScheduleDetail[i].Day11, supply.supplyScheduleDetail[i].Day12, supply.supplyScheduleDetail[i].Day13, supply.supplyScheduleDetail[i].Day14, supply.supplyScheduleDetail[i].Day15, supply.supplyScheduleDetail[i].Day16, supply.supplyScheduleDetail[i].Day17, supply.supplyScheduleDetail[i].Day18, supply.supplyScheduleDetail[i].Day19, supply.supplyScheduleDetail[i].Day20, supply.supplyScheduleDetail[i].Day21, supply.supplyScheduleDetail[i].Day22, supply.supplyScheduleDetail[i].Day23, supply.supplyScheduleDetail[i].Day24, supply.supplyScheduleDetail[i].Day25, supply.supplyScheduleDetail[i].Day26, supply.supplyScheduleDetail[i].Day27, supply.supplyScheduleDetail[i].Day28, supply.supplyScheduleDetail[i].Day29, supply.supplyScheduleDetail[i].Day30, supply.supplyScheduleDetail[i].Day31);
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
			grdHistoryGridDetail.RootTable.Columns["Item"].Width = 200;
			grdHistoryGridDetail.RootTable.Columns["UOM"].Width = 100;
			grdHistoryGridDetail.RootTable.Columns["Qty"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["Month"].Width = 70;
			grdHistoryGridDetail.RootTable.Columns["D1"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D2"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D3"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D4"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D5"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D6"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D7"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D8"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D9"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D10"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D11"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D12"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D13"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D14"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D15"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D16"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D17"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D18"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D19"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D20"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D21"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D22"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D23"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D24"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D25"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D26"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D27"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D28"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D29"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D30"].Width = 40;
			grdHistoryGridDetail.RootTable.Columns["D31"].Width = 40;
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

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		bindGrid();
		grdHistoryGridDetail.ClearStructure();
	}

	private void cmbSupplier_Leave_1(object sender, EventArgs e)
	{
		PurchaseOrder();
	}

	private void cmbOrderNo_Leave(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		try
		{
			if (cmbOrderNo.Value != null && cmbOrderNo.Value != "0")
			{
				int num = Conversion.ToInt(cmbOrderNo.Value);
				PurchaseOrder purchaseOrder = new PurchaseOrder();
				purchaseOrder = PurchaseOrder.GetByID(num);
				if (purchaseOrder != null)
				{
					DataTable dtItemCombo = new DataTable();
					dtItemCombo.Columns.Add("ItemId");
					dtItemCombo.Columns.Add("ItemName");
					for (int i = 0; i < purchaseOrder.purchaseOrderDetailList.Count; i++)
					{
						dtItemCombo.Rows.Add(purchaseOrder.purchaseOrderDetailList[i].OrderItemId, purchaseOrder.purchaseOrderDetailList[i].ItemName);
					}
					if (dtItemCombo.Rows.Count > 0)
					{
						DDL.BindDDL(dtItemCombo, cmbItem, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
					}
				}
			}
			else
			{
				item();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		bindGrid();
	}

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			if (UserAccount.CompAddress != string.Empty)
			{
				rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			}
			if (UserAccount.CompName != string.Empty)
			{
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			}
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@hId", RecId.ToString());
			rpt.ShowReport("RptInvSupplySchedule.rpt");
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			string code = SupplySchedule.GetIdbyDocNo(new SupplySchedule
			{
				ThisDocumentTypeId = 65,
				DocNo = Conversion.ToInt((object)txtdocnumber.Text.Trim())
			});
			if (Conversion.ToInt((object)code) > 0)
			{
				RecId = Conversion.ToInt((object)code);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailAgainstPurchaseOrder(ReportsParameters purchase)
	{
		try
		{
			DataTable dtPOSS = new DataTable();
			dtPOSS = PurchaseOrder.GetPurchaseOrderDetailForSupplySchedule(purchase);
			if (dtPOSS.Rows.Count > 0)
			{
				cmbSupplier.Value = dtPOSS.Rows[0]["OrderSupCustId"];
				((Control)(object)cmbSupplier).Enabled = false;
				for (int i = 0; i < dtPOSS.Rows.Count; i++)
				{
					dtdetail.Rows.Add(dtPOSS.Rows[i]["Id"], dtPOSS.Rows[i]["DocNo"], dtPOSS.Rows[i]["ItemId"], dtPOSS.Rows[i]["ItemName"], dtPOSS.Rows[i]["OrderItemUOMId"], dtPOSS.Rows[i]["UOMCode"], dtPOSS.Rows[i]["OrderItemQty"], txtFromDate.Value.ToString("MMM-yyyy"), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			dtdetail.Rows.Clear();
			string Ids = "";
			for (int i = 0; i < LoadSupply.dtSupply.Rows.Count; i++)
			{
				Ids = Ids + "," + Conversion.ToString(LoadSupply.dtSupply.Rows[i][0]);
			}
			ReportsParameters purchase = new ReportsParameters();
			purchase.OrganizationId = UserAccount.OrganizationId;
			purchase.CompanyId = UserAccount.CompanyId;
			purchase.Ids = Ids;
			LoadDataDetailAgainstPurchaseOrder(purchase);
			Ids = "";
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
			LoadSupply = new LoadPurchaseOrder(UserAccount);
			LoadSupply.ShowDialog();
			LoadSupply.BringToFront();
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
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected O, but got Unknown
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Expected O, but got Unknown
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Expected O, but got Unknown
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Expected O, but got Unknown
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Expected O, but got Unknown
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Expected O, but got Unknown
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Expected O, but got Unknown
		//IL_07d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Expected O, but got Unknown
		//IL_0d02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0c: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PurchaseTrading.frmSupplySchedule));
		GridEXLayout grdHistoryGridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbOrderNo = new UltraCombo();
		this.txtmonthdate = new System.Windows.Forms.DateTimePicker();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.txtd26 = new System.Windows.Forms.TextBox();
		this.txtd20 = new System.Windows.Forms.TextBox();
		this.txtd13 = new System.Windows.Forms.TextBox();
		this.txtd12 = new System.Windows.Forms.TextBox();
		this.txtd23 = new System.Windows.Forms.TextBox();
		this.txtd6 = new System.Windows.Forms.TextBox();
		this.txtd17 = new System.Windows.Forms.TextBox();
		this.txtd29 = new System.Windows.Forms.TextBox();
		this.txtd9 = new System.Windows.Forms.TextBox();
		this.txtd3 = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtd25 = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtd19 = new System.Windows.Forms.TextBox();
		this.txtd31 = new System.Windows.Forms.TextBox();
		this.txtd11 = new System.Windows.Forms.TextBox();
		this.txtd22 = new System.Windows.Forms.TextBox();
		this.txtd5 = new System.Windows.Forms.TextBox();
		this.txtd16 = new System.Windows.Forms.TextBox();
		this.txtd28 = new System.Windows.Forms.TextBox();
		this.txtd8 = new System.Windows.Forms.TextBox();
		this.txtd2 = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtd24 = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtd18 = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtd30 = new System.Windows.Forms.TextBox();
		this.txtd10 = new System.Windows.Forms.TextBox();
		this.txtd4 = new System.Windows.Forms.TextBox();
		this.txtd21 = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtd14 = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtd15 = new System.Windows.Forms.TextBox();
		this.txtd7 = new System.Windows.Forms.TextBox();
		this.txtd27 = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtd1 = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbPackUOM = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
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
		this.comCompany = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.button3 = new System.Windows.Forms.Button();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnLoadOrder = new System.Windows.Forms.ToolStripButton();
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
		this.panel6.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1305, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1305, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1297, 723);
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
		this.panel2.Size = new System.Drawing.Size(1291, 717);
		this.panel2.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 141);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1291, 576);
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1291, 576);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 2;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.groupBox1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 131);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1291, 10);
		this.panel6.TabIndex = 3;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbOrderNo);
		this.groupBox1.Controls.Add(this.txtmonthdate);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnGridUpdate);
		this.groupBox1.Controls.Add(this.btnAddinGrid);
		this.groupBox1.Controls.Add(this.txtd26);
		this.groupBox1.Controls.Add(this.txtd20);
		this.groupBox1.Controls.Add(this.txtd13);
		this.groupBox1.Controls.Add(this.txtd12);
		this.groupBox1.Controls.Add(this.txtd23);
		this.groupBox1.Controls.Add(this.txtd6);
		this.groupBox1.Controls.Add(this.txtd17);
		this.groupBox1.Controls.Add(this.txtd29);
		this.groupBox1.Controls.Add(this.txtd9);
		this.groupBox1.Controls.Add(this.txtd3);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtd25);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.txtd19);
		this.groupBox1.Controls.Add(this.txtd31);
		this.groupBox1.Controls.Add(this.txtd11);
		this.groupBox1.Controls.Add(this.txtd22);
		this.groupBox1.Controls.Add(this.txtd5);
		this.groupBox1.Controls.Add(this.txtd16);
		this.groupBox1.Controls.Add(this.txtd28);
		this.groupBox1.Controls.Add(this.txtd8);
		this.groupBox1.Controls.Add(this.txtd2);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtd24);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtd18);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.txtd30);
		this.groupBox1.Controls.Add(this.txtd10);
		this.groupBox1.Controls.Add(this.txtd4);
		this.groupBox1.Controls.Add(this.txtd21);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.txtd14);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.txtd15);
		this.groupBox1.Controls.Add(this.txtd7);
		this.groupBox1.Controls.Add(this.txtd27);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtd1);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUOM);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Location = new System.Drawing.Point(9, 27);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1264, 147);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.cmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.cmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Location = new System.Drawing.Point(10, 49);
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Name = "cmbOrderNo";
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Size = new System.Drawing.Size(95, 22);
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Leave += new System.EventHandler(cmbOrderNo_Leave);
		this.txtmonthdate.CustomFormat = "MMM-yyyy";
		this.txtmonthdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtmonthdate.Location = new System.Drawing.Point(561, 50);
		this.txtmonthdate.Name = "txtmonthdate";
		this.txtmonthdate.Size = new System.Drawing.Size(127, 20);
		this.txtmonthdate.TabIndex = 4;
		this.txtmonthdate.Visible = false;
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.Location = new System.Drawing.Point(915, 105);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(56, 25);
		this.btnCancel.TabIndex = 38;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.Location = new System.Drawing.Point(853, 105);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(56, 25);
		this.btnGridUpdate.TabIndex = 37;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = true;
		this.btnGridUpdate.Visible = false;
		this.btnAddinGrid.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddinGrid.Location = new System.Drawing.Point(810, 106);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(27, 22);
		this.btnAddinGrid.TabIndex = 36;
		this.btnAddinGrid.UseVisualStyleBackColor = true;
		this.btnAddinGrid.Visible = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.txtd26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd26.Location = new System.Drawing.Point(538, 108);
		this.txtd26.Name = "txtd26";
		this.txtd26.Size = new System.Drawing.Size(44, 22);
		this.txtd26.TabIndex = 30;
		this.txtd26.Visible = false;
		this.txtd20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd20.Location = new System.Drawing.Point(274, 108);
		this.txtd20.Name = "txtd20";
		this.txtd20.Size = new System.Drawing.Size(44, 22);
		this.txtd20.TabIndex = 24;
		this.txtd20.Visible = false;
		this.txtd13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd13.Location = new System.Drawing.Point(1191, 49);
		this.txtd13.Name = "txtd13";
		this.txtd13.Size = new System.Drawing.Size(44, 22);
		this.txtd13.TabIndex = 17;
		this.txtd12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd12.Location = new System.Drawing.Point(1156, 49);
		this.txtd12.Name = "txtd12";
		this.txtd12.Size = new System.Drawing.Size(44, 22);
		this.txtd12.TabIndex = 16;
		this.txtd23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd23.Location = new System.Drawing.Point(406, 108);
		this.txtd23.Name = "txtd23";
		this.txtd23.Size = new System.Drawing.Size(44, 22);
		this.txtd23.TabIndex = 27;
		this.txtd23.Visible = false;
		this.txtd6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd6.Location = new System.Drawing.Point(904, 49);
		this.txtd6.Name = "txtd6";
		this.txtd6.Size = new System.Drawing.Size(44, 22);
		this.txtd6.TabIndex = 10;
		this.txtd6.Visible = false;
		this.txtd17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd17.Location = new System.Drawing.Point(142, 108);
		this.txtd17.Name = "txtd17";
		this.txtd17.Size = new System.Drawing.Size(44, 22);
		this.txtd17.TabIndex = 21;
		this.txtd17.Visible = false;
		this.txtd29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd29.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd29.Location = new System.Drawing.Point(670, 108);
		this.txtd29.Name = "txtd29";
		this.txtd29.Size = new System.Drawing.Size(44, 22);
		this.txtd29.TabIndex = 33;
		this.txtd29.Visible = false;
		this.txtd9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd9.Location = new System.Drawing.Point(1030, 49);
		this.txtd9.Name = "txtd9";
		this.txtd9.Size = new System.Drawing.Size(44, 22);
		this.txtd9.TabIndex = 13;
		this.txtd3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd3.Location = new System.Drawing.Point(778, 49);
		this.txtd3.Name = "txtd3";
		this.txtd3.Size = new System.Drawing.Size(44, 22);
		this.txtd3.TabIndex = 8;
		this.txtd3.Visible = false;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(540, 85);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(36, 17);
		this.label43.TabIndex = 32;
		this.label43.Text = "D-26";
		this.label43.Visible = false;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(763, 85);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(36, 17);
		this.label30.TabIndex = 32;
		this.label30.Text = "D-31";
		this.label30.Visible = false;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(279, 85);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(36, 17);
		this.label42.TabIndex = 32;
		this.label42.Text = "D-20";
		this.label42.Visible = false;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(905, 25);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(29, 17);
		this.label50.TabIndex = 32;
		this.label50.Text = "D-6";
		this.label50.Visible = false;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(1190, 25);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(36, 17);
		this.label23.TabIndex = 32;
		this.label23.Text = "D-13";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(410, 85);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(36, 17);
		this.label41.TabIndex = 32;
		this.label41.Text = "D-23";
		this.label41.Visible = false;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(628, 85);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(36, 17);
		this.label29.TabIndex = 32;
		this.label29.Text = "D-28";
		this.label29.Visible = false;
		this.txtd25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd25.Location = new System.Drawing.Point(494, 108);
		this.txtd25.Name = "txtd25";
		this.txtd25.Size = new System.Drawing.Size(44, 22);
		this.txtd25.TabIndex = 29;
		this.txtd25.Visible = false;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(146, 85);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(36, 17);
		this.label40.TabIndex = 32;
		this.label40.Text = "D-17";
		this.label40.Visible = false;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(782, 25);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(29, 17);
		this.label49.TabIndex = 32;
		this.label49.Text = "D-3";
		this.label49.Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1071, 25);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(36, 17);
		this.label20.TabIndex = 32;
		this.label20.Text = "D-10";
		this.txtd19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd19.Location = new System.Drawing.Point(230, 108);
		this.txtd19.Name = "txtd19";
		this.txtd19.Size = new System.Drawing.Size(44, 22);
		this.txtd19.TabIndex = 23;
		this.txtd19.Visible = false;
		this.txtd31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd31.Location = new System.Drawing.Point(758, 108);
		this.txtd31.Name = "txtd31";
		this.txtd31.Size = new System.Drawing.Size(44, 22);
		this.txtd31.TabIndex = 35;
		this.txtd31.Visible = false;
		this.txtd11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd11.Location = new System.Drawing.Point(1114, 49);
		this.txtd11.Name = "txtd11";
		this.txtd11.Size = new System.Drawing.Size(44, 22);
		this.txtd11.TabIndex = 15;
		this.txtd22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd22.Location = new System.Drawing.Point(362, 108);
		this.txtd22.Name = "txtd22";
		this.txtd22.Size = new System.Drawing.Size(44, 22);
		this.txtd22.TabIndex = 26;
		this.txtd22.Visible = false;
		this.txtd5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd5.Location = new System.Drawing.Point(862, 49);
		this.txtd5.Name = "txtd5";
		this.txtd5.Size = new System.Drawing.Size(44, 22);
		this.txtd5.TabIndex = 9;
		this.txtd5.Visible = false;
		this.txtd16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd16.Location = new System.Drawing.Point(98, 108);
		this.txtd16.Name = "txtd16";
		this.txtd16.Size = new System.Drawing.Size(44, 22);
		this.txtd16.TabIndex = 20;
		this.txtd16.Visible = false;
		this.txtd28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd28.Location = new System.Drawing.Point(626, 108);
		this.txtd28.Name = "txtd28";
		this.txtd28.Size = new System.Drawing.Size(44, 22);
		this.txtd28.TabIndex = 32;
		this.txtd28.Visible = false;
		this.txtd8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd8.Location = new System.Drawing.Point(988, 49);
		this.txtd8.Name = "txtd8";
		this.txtd8.Size = new System.Drawing.Size(44, 22);
		this.txtd8.TabIndex = 11;
		this.txtd2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd2.Location = new System.Drawing.Point(736, 49);
		this.txtd2.Name = "txtd2";
		this.txtd2.Size = new System.Drawing.Size(44, 22);
		this.txtd2.TabIndex = 6;
		this.txtd2.Visible = false;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(496, 85);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(36, 17);
		this.label39.TabIndex = 30;
		this.label39.Text = "D-25";
		this.label39.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(718, 85);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(36, 17);
		this.label27.TabIndex = 30;
		this.label27.Text = "D-30";
		this.label27.Visible = false;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(236, 85);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(36, 17);
		this.label38.TabIndex = 30;
		this.label38.Text = "D-19";
		this.label38.Visible = false;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(864, 25);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(29, 17);
		this.label48.TabIndex = 30;
		this.label48.Text = "D-5";
		this.label48.Visible = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(1153, 25);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(36, 17);
		this.label22.TabIndex = 30;
		this.label22.Text = "D-12";
		this.txtd24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd24.Location = new System.Drawing.Point(450, 108);
		this.txtd24.Name = "txtd24";
		this.txtd24.Size = new System.Drawing.Size(44, 22);
		this.txtd24.TabIndex = 28;
		this.txtd24.Visible = false;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(367, 85);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(36, 17);
		this.label37.TabIndex = 30;
		this.label37.Text = "D-22";
		this.label37.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(583, 85);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(36, 17);
		this.label26.TabIndex = 30;
		this.label26.Text = "D-27";
		this.label26.Visible = false;
		this.txtd18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd18.Location = new System.Drawing.Point(186, 108);
		this.txtd18.Name = "txtd18";
		this.txtd18.Size = new System.Drawing.Size(44, 22);
		this.txtd18.TabIndex = 22;
		this.txtd18.Visible = false;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(102, 85);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(36, 17);
		this.label36.TabIndex = 30;
		this.label36.Text = "D-16";
		this.label36.Visible = false;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(741, 25);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(29, 17);
		this.label47.TabIndex = 30;
		this.label47.Text = "D-2";
		this.label47.Visible = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1030, 25);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(29, 17);
		this.label19.TabIndex = 30;
		this.label19.Text = "D-9";
		this.txtd30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd30.Location = new System.Drawing.Point(714, 108);
		this.txtd30.Name = "txtd30";
		this.txtd30.Size = new System.Drawing.Size(44, 22);
		this.txtd30.TabIndex = 34;
		this.txtd30.Visible = false;
		this.txtd10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd10.Location = new System.Drawing.Point(1072, 49);
		this.txtd10.Name = "txtd10";
		this.txtd10.Size = new System.Drawing.Size(44, 22);
		this.txtd10.TabIndex = 14;
		this.txtd4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd4.Location = new System.Drawing.Point(820, 49);
		this.txtd4.Name = "txtd4";
		this.txtd4.Size = new System.Drawing.Size(44, 22);
		this.txtd4.TabIndex = 8;
		this.txtd4.Visible = false;
		this.txtd21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd21.Location = new System.Drawing.Point(318, 108);
		this.txtd21.Name = "txtd21";
		this.txtd21.Size = new System.Drawing.Size(44, 22);
		this.txtd21.TabIndex = 25;
		this.txtd21.Visible = false;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(453, 85);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(36, 17);
		this.label35.TabIndex = 28;
		this.label35.Text = "D-24";
		this.label35.Visible = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(673, 85);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(36, 17);
		this.label25.TabIndex = 28;
		this.label25.Text = "D-29";
		this.label25.Visible = false;
		this.txtd14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd14.Location = new System.Drawing.Point(10, 108);
		this.txtd14.Name = "txtd14";
		this.txtd14.Size = new System.Drawing.Size(44, 22);
		this.txtd14.TabIndex = 18;
		this.txtd14.Visible = false;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(191, 85);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(36, 17);
		this.label34.TabIndex = 28;
		this.label34.Text = "D-18";
		this.label34.Visible = false;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(823, 25);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(29, 17);
		this.label46.TabIndex = 28;
		this.label46.Text = "D-4";
		this.label46.Visible = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1112, 25);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(36, 17);
		this.label21.TabIndex = 28;
		this.label21.Text = "D-11";
		this.txtd15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd15.Location = new System.Drawing.Point(54, 108);
		this.txtd15.Name = "txtd15";
		this.txtd15.Size = new System.Drawing.Size(44, 22);
		this.txtd15.TabIndex = 19;
		this.txtd15.Visible = false;
		this.txtd7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd7.Location = new System.Drawing.Point(946, 49);
		this.txtd7.Name = "txtd7";
		this.txtd7.Size = new System.Drawing.Size(44, 22);
		this.txtd7.TabIndex = 11;
		this.txtd7.Visible = false;
		this.txtd27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd27.Location = new System.Drawing.Point(582, 108);
		this.txtd27.Name = "txtd27";
		this.txtd27.Size = new System.Drawing.Size(44, 22);
		this.txtd27.TabIndex = 31;
		this.txtd27.Visible = false;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(323, 85);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(36, 17);
		this.label33.TabIndex = 28;
		this.label33.Text = "D-21";
		this.label33.Visible = false;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(946, 25);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(29, 17);
		this.label45.TabIndex = 28;
		this.label45.Text = "D-7";
		this.label45.Visible = false;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(10, 85);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(36, 17);
		this.label24.TabIndex = 28;
		this.label24.Text = "D-14";
		this.label24.Visible = false;
		this.txtd1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtd1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtd1.Location = new System.Drawing.Point(694, 49);
		this.txtd1.Name = "txtd1";
		this.txtd1.Size = new System.Drawing.Size(44, 22);
		this.txtd1.TabIndex = 5;
		this.txtd1.Visible = false;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(697, 25);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(29, 17);
		this.label44.TabIndex = 28;
		this.label44.Text = "D-1";
		this.label44.Visible = false;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(57, 85);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(36, 17);
		this.label32.TabIndex = 28;
		this.label32.Text = "D-15";
		this.label32.Visible = false;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(989, 25);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(29, 17);
		this.label17.TabIndex = 28;
		this.label17.Text = "D-8";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(562, 27);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(46, 17);
		this.label16.TabIndex = 26;
		this.label16.Text = "Month";
		this.label16.Visible = false;
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(482, 49);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(74, 22);
		this.txtQty.TabIndex = 3;
		this.txtQty.Visible = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(488, 25);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(28, 17);
		this.label14.TabIndex = 24;
		this.label14.Text = "Qty";
		this.label14.Visible = false;
		this.cmbPackUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Location = new System.Drawing.Point(292, 49);
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Name = "cmbPackUOM";
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Size = new System.Drawing.Size(184, 22);
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Visible = false;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(293, 25);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 17);
		this.label31.TabIndex = 23;
		this.label31.Text = "Pack UOM";
		this.label31.Visible = false;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(107, 49);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(179, 22);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItem).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(106, 27);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(33, 17);
		this.label28.TabIndex = 21;
		this.label28.Text = "Item";
		this.label28.Visible = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 26);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(65, 17);
		this.label12.TabIndex = 18;
		this.label12.Text = "Order No";
		this.label12.Visible = false;
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
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.comCompany);
		this.panel5.Controls.Add(this.label3);
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
		this.panel5.Size = new System.Drawing.Size(1291, 101);
		this.panel5.TabIndex = 2;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Location = new System.Drawing.Point(1027, 75);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(15, 14);
		this.chkPrint.TabIndex = 12;
		this.chkPrint.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(296, 71);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(57, 15);
		this.label11.TabIndex = 54;
		this.label11.Text = "Remarks";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(375, 68);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(646, 21);
		this.txtRemarks.TabIndex = 11;
		this.txtToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(362, 162);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(163, 21);
		this.txtToDate.TabIndex = 11;
		this.txtToDate.TabStop = false;
		this.txtToDate.Visible = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(283, 166);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(50, 15);
		this.label10.TabIndex = 52;
		this.label10.Text = "To Date";
		this.label10.Visible = false;
		this.txtFromDate.CustomFormat = "MMM-yyyy";
		this.txtFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDate.Location = new System.Drawing.Point(86, 68);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(204, 21);
		this.txtFromDate.TabIndex = 10;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(9, 72);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(42, 15);
		this.label9.TabIndex = 50;
		this.label9.Text = "Month";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1028, 45);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 48;
		this.label7.Text = "Payment Sch";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(797, 45);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(90, 15);
		this.label6.TabIndex = 48;
		this.label6.Text = "Production Sch";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(544, 45);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(84, 15);
		this.label5.TabIndex = 48;
		this.label5.Text = "Customer Sch";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(296, 45);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 15);
		this.label4.TabIndex = 48;
		this.label4.Text = "supplierSch";
		this.txtPaymentSch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPaymentSch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPaymentSch.Location = new System.Drawing.Point(1124, 40);
		this.txtPaymentSch.Name = "txtPaymentSch";
		this.txtPaymentSch.Size = new System.Drawing.Size(149, 21);
		this.txtPaymentSch.TabIndex = 9;
		this.txtProductionsch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionsch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionsch.Location = new System.Drawing.Point(895, 40);
		this.txtProductionsch.Name = "txtProductionsch";
		this.txtProductionsch.Size = new System.Drawing.Size(126, 21);
		this.txtProductionsch.TabIndex = 8;
		this.txtCustomerSch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerSch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerSch.Location = new System.Drawing.Point(633, 40);
		this.txtCustomerSch.Name = "txtCustomerSch";
		this.txtCustomerSch.Size = new System.Drawing.Size(158, 21);
		this.txtCustomerSch.TabIndex = 7;
		this.txtSupplierSch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierSch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierSch.Location = new System.Drawing.Point(375, 40);
		this.txtSupplierSch.Name = "txtSupplierSch";
		this.txtSupplierSch.Size = new System.Drawing.Size(163, 21);
		this.txtSupplierSch.TabIndex = 6;
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(86, 40);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(204, 24);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Leave += new System.EventHandler(cmbSupplier_Leave_1);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(9, 43);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(53, 15);
		this.label8.TabIndex = 42;
		this.label8.Text = "Supplier";
		this.comCompany.AllowNull = (DefaultableBoolean)1;
		this.comCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.comCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCompany).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCompany).Location = new System.Drawing.Point(86, 13);
		((System.Windows.Forms.Control)(object)this.comCompany).Name = "comCompany";
		((System.Windows.Forms.Control)(object)this.comCompany).Size = new System.Drawing.Size(204, 24);
		((System.Windows.Forms.Control)(object)this.comCompany).TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(9, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 15);
		this.label3.TabIndex = 42;
		this.label3.Text = "Company";
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
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
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(633, 13);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(158, 24);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 2;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(544, 16);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(45, 15);
		this.label18.TabIndex = 46;
		this.label18.Text = "Project";
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(375, 13);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(163, 24);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 1;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(296, 16);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 15);
		this.label13.TabIndex = 45;
		this.label13.Text = "Branch";
		this.txtDocdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocdate.Location = new System.Drawing.Point(1124, 14);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(149, 21);
		this.txtDocdate.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1028, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 40;
		this.label2.Text = "Document Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(797, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 39;
		this.label1.Text = "Document No";
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(895, 13);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.Size = new System.Drawing.Size(126, 21);
		this.txtdocnumber.TabIndex = 3;
		this.txtdocnumber.Leave += new System.EventHandler(txtdocnumber_Leave);
		this.panel3.Controls.Add(this.button3);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1291, 30);
		this.panel3.TabIndex = 0;
		this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button3.BackColor = System.Drawing.Color.Transparent;
		this.button3.FlatAppearance.BorderSize = 0;
		this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button3.Image = (System.Drawing.Image)resources.GetObject("button3.Image");
		this.button3.Location = new System.Drawing.Point(1243, -1);
		this.button3.Margin = new System.Windows.Forms.Padding(2);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(44, 28);
		this.button3.TabIndex = 8;
		this.button3.UseVisualStyleBackColor = false;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripButton6, this.btnnew, this.btnsave, this.btnUpdate, this.Print, this.btnLoadOrder });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1291, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton6.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(154, 24);
		this.toolStripButton6.Text = "Supply Schedule       ";
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
		this.Print.Size = new System.Drawing.Size(56, 24);
		this.Print.Text = "&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnLoadOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadOrder.Name = "btnLoadOrder";
		this.btnLoadOrder.Size = new System.Drawing.Size(70, 24);
		this.btnLoadOrder.Text = "Load Order";
		this.btnLoadOrder.Click += new System.EventHandler(btnLoadOrder_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1297, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel4);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1291, 717);
		this.panel8.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 349);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1291, 368);
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
		((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail).Size = new System.Drawing.Size(1291, 368);
		((System.Windows.Forms.Control)(object)this.grdHistoryGridDetail).TabIndex = 47;
		this.grdHistoryGridDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryGridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryGridDetail).VisualStyle = (VisualStyle)4;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 29);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1291, 320);
		this.panel11.TabIndex = 2;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1291, 320);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 46;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Click += new System.EventHandler(DataGridHistory_Click);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel9.Controls.Add(this.toolStrip2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1291, 29);
		this.panel9.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripButton7, this.btnHistoryRefresh, this.BtnPrint, this.toolStripSeparator1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1291, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton7.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton7.Name = "toolStripButton7";
		this.toolStripButton7.Size = new System.Drawing.Size(78, 24);
		this.toolStripButton7.Text = "History    ";
		this.btnHistoryRefresh.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnHistoryRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnHistoryRefresh.Name = "btnHistoryRefresh";
		this.btnHistoryRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnHistoryRefresh.Text = "&Refresh";
		this.btnHistoryRefresh.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(56, 24);
		this.BtnPrint.Text = "&Print";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1305, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmSupplySchedule";
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
		this.panel6.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).EndInit();
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

}
