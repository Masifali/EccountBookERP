using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.ContractorWages;
using Architecture.BLL.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;
using Architecture.Model.WeightBridge;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Contractor_Wages;

public class frmMoveOrder : Form
{
	private int RECID;

	private int Id;

	public bool UpdateMode;

	private int updateDetailIndex;

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtPendingTicket = new DataTable();

	private DataTable dtdetail = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	private Panel panel6;

	private Label label15;

	private Panel panel3;

	private GroupBox groupBox1;

	private Label label11;

	private Button btnAdd;

	internal UltraCombo cmbmoveTo;

	private Label label21;

	internal UltraCombo cmbMoveFrom;

	private Label label20;

	internal UltraCombo cmbPackingType;

	private Label label18;

	internal UltraCombo cmbCrop;

	private Label label16;

	internal UltraCombo cmbJobLot;

	private Label label14;

	internal UltraCombo cmbItem;

	private Label label13;

	private TextBox txtqtyMain;

	private Label label17;

	private Label label29;

	private TextBox txtNetWeight;

	internal UltraCombo cmbScaleSlip;

	private Label label19;

	private Label label35;

	private TextBox txtPackSize;

	private Label label33;

	private TextBox txtsecondweight;

	private Label label31;

	private TextBox txtfirstweight;

	private ComboBox comRefenenceType;

	private Label label30;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private CheckBox chkisapprove;

	private TextBox txtdocnumber;

	internal UltraCombo comWagesAccount;

	internal UltraCombo comSupplier;

	private Label label7;

	private DateTimePicker txtDocdate;

	private TextBox txtRemarks;

	private Label label5;

	private TextBox txtqty;

	private Label label6;

	private Label label1;

	private Label label8;

	private TextBox txtwagesrate;

	internal UltraCombo comReferenceNo;

	private Label label3;

	private Label label9;

	private Label label12;

	private Label label4;

	private TextBox txtwageamount;

	private Label label2;

	private TextBox txtWeight;

	private Label label10;

	private Panel panel4;

	internal GridEX grdwagesDetail;

	private Panel panel5;

	private Panel panel8;

	private Panel panel7;

	private Panel panel9;

	private Label label26;

	internal GridEX DataGridHistory;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton1;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton4;

	private Panel panel10;

	internal GridEX gridPending;

	private CheckBox ChkBoxPrint;

	private TextBox txtBillWeight;

	private Label label24;

	private TextBox txtBillQty;

	private Label label23;

	private TextBox txtWeightCut;

	private Label label22;

	private Panel panel11;

	internal GridEX GrdHistoryDetail;

	public frmMoveOrder(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmMoveOrder_Load(object sender, EventArgs e)
	{
		dtdetail.Columns.Add("SupplierId");
		dtdetail.Columns.Add("SupplierName");
		dtdetail.Columns.Add("WagesId");
		dtdetail.Columns.Add("WagesAccount");
		dtdetail.Columns.Add("packingTypeId");
		dtdetail.Columns.Add("packingType");
		dtdetail.Columns.Add("ScaleSlipNoId");
		dtdetail.Columns.Add("ScaleSlipNo");
		dtdetail.Columns.Add("Weight");
		dtdetail.Columns.Add("PackSize");
		dtdetail.Columns.Add("Quantity");
		dtdetail.Columns.Add("WeightCut");
		dtdetail.Columns.Add("BillQty", typeof(double));
		dtdetail.Columns.Add("BillWeight", typeof(double));
		dtdetail.Columns.Add("Rate");
		dtdetail.Columns.Add("Amount", typeof(double));
		dtdetail.Columns.Add("ItemId");
		dtdetail.Columns.Add("Item");
		dtdetail.Columns.Add("jobLotId");
		dtdetail.Columns.Add("jobLot");
		dtdetail.Columns.Add("Crop");
		dtdetail.Columns.Add("MoveFromId");
		dtdetail.Columns.Add("MoveFrom");
		dtdetail.Columns.Add("MoveToId");
		dtdetail.Columns.Add("MoveTo");
		dtPendingTicket.Columns.Add("TranType");
		dtPendingTicket.Columns.Add("ReferenceNo");
		dtPendingTicket.Columns.Add("Date");
		dtPendingTicket.Columns.Add("Variety");
		dtPendingTicket.Columns.Add("Slip#");
		Warehouse();
		Item();
		CropYear();
		combojoblotfill();
		bindPackType();
		suppliercustomer();
		GenerateDocNo();
		accountName();
		bindHistory();
		PendingTicket();
		comRefenenceType.SelectedIndex = 0;
		ChkBoxPrint.Checked = true;
		comRefenenceType_Leave(null, null);
	}

	public void Warehouse()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		try
		{
			List<InvWareHouse> lst2 = InvWareHouse.GetList(new InvWareHouse());
			DataTable dtwarehousefrom = new DataTable();
			dtwarehousefrom.Columns.Add("Id");
			dtwarehousefrom.Columns.Add("WareHouseName");
			dtwarehousefrom.Clear();
			for (int j = 0; j < lst2.Count; j++)
			{
				dtwarehousefrom.Rows.Add(lst2[j].Id, lst2[j].WareHouseName);
			}
			if (dtwarehousefrom.Rows.Count > 0)
			{
				DDL.BindDDL(dtwarehousefrom, cmbMoveFrom, "Id", "WareHouseName", "WareHouse ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		try
		{
			List<InvWareHouse> lst = InvWareHouse.GetList(new InvWareHouse());
			DataTable dtwarehouseTO = new DataTable();
			dtwarehouseTO.Columns.Add("Id");
			dtwarehouseTO.Columns.Add("WareHouseName");
			dtwarehouseTO.Clear();
			for (int i = 0; i < lst.Count; i++)
			{
				dtwarehouseTO.Rows.Add(lst[i].Id, lst[i].WareHouseName);
			}
			if (dtwarehouseTO.Rows.Count > 0)
			{
				DDL.BindDDL(dtwarehouseTO, cmbmoveTo, "Id", "WareHouseName", "WareHouse ", ZeroIndex: true);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	public void Item()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			List<Item> lst = Item.GetList(new Item());
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ItemName");
			for (int i = 0; i < lst.Count; i++)
			{
				dt.Rows.Add(lst[i].Id, lst[i].ItemName);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbItem, "Id", "ItemName", "Item Name ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		DataTable dtCrop = CommonServices.CropYearGetAllService();
		if (dtCrop.Rows.Count > 0)
		{
			DDL.BindDDLNew(dtCrop, cmbCrop, "Id", "CropYear", "Crop Year", false);
		}
	}

	private void combojoblotfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			List<JobLot> lst = jobLot.GetList(new JobLot());
			DataTable dtJob = new DataTable();
			dtJob.Columns.Add("Id");
			dtJob.Columns.Add("JobLotDescription");
			dtJob.Clear();
			for (int i = 0; i < lst.Count; i++)
			{
				dtJob.Rows.Add(lst[i].Id, lst[i].JobLotDescription);
			}
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDL(dtJob, cmbJobLot, "Id", "JobLotDescription", "JobLot Description", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindPackType()
	{
		try
		{
			DataTable dtpack = new DataTable();
			dtpack = InvPackingType.Getall();
			DataTable dtPackType = new DataTable();
			dtPackType.Columns.Add("Id");
			dtPackType.Columns.Add("PackType");
			dtPackType.Clear();
			for (int i = 0; i < dtpack.Rows.Count; i++)
			{
				dtPackType.Rows.Add(dtpack.Rows[i]["Id"], dtpack.Rows[i]["PackTypeDesc"]);
			}
			if (dtPackType.Rows.Count > 0)
			{
				DDL.BindDDL(dtPackType, cmbPackingType, "Id", "PackType", "Pack Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ScaleSlip()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			WbTransactions wbtTicket = new WbTransactions();
			if (comRefenenceType.SelectedIndex == 0)
			{
				wbtTicket.ReferenceDocTypeId = 74;
			}
			wbtTicket.ReferenceDocNoId = Conversion.ToInt((object)((Control)(object)comReferenceNo).Text);
			DataTable dtref = new DataTable();
			dtref = WbTransactions.GetByTicketNoByRefDocId(wbtTicket);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ScaleSlip");
			for (int i = 0; i < dtref.Rows.Count; i++)
			{
				dt.Rows.Add(dtref.Rows[i]["Id"], dtref.Rows[i]["TicketNo"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbScaleSlip, "Id", "ScaleSlip", "Scale Slip ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, comSupplier, "Id", "CompanyName", "Suplier ", ZeroIndex: false);
			}
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int code = InvContractorWagesBillHeader.GenerateCode(new InvContractorWagesBillHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				RefDocumentTypeId = 74,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
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

	public void accountName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtget = new DataTable();
			dtget = InvConractorWagesAccounts.GetAll(val);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountName");
			for (int i = 0; i < dtget.Rows.Count; i++)
			{
				dt.Rows.Add(dtget.Rows[i]["Id"], dtget.Rows[i]["WagesAccountName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, comWagesAccount, "Id", "AccountName", "Account Name", ZeroIndex: false);
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void comReferenceNo_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			WbTransactions refereceid = new WbTransactions();
			if (comRefenenceType.SelectedIndex == 0)
			{
				refereceid.ReferenceDocTypeId = 74;
			}
			refereceid.ReferenceDocNoId = Conversion.ToInt((object)((Control)(object)comReferenceNo).Text);
			DataTable dtgpread = new DataTable();
			dtgpread = WbTransactions.ReadByGatePassId(refereceid);
			if (dtgpread.Rows.Count > 0)
			{
				txtfirstweight.Text = dtgpread.Rows[0]["FirstWeight"].ToString();
				txtsecondweight.Text = dtgpread.Rows[0]["SecondWeight"].ToString();
				txtqtyMain.Text = dtgpread.Rows[0]["ItemQty"].ToString();
				if (int.Parse(txtfirstweight.Text.ToString()) > int.Parse(txtsecondweight.Text.ToString()))
				{
					txtNetWeight.Text = (double.Parse(txtfirstweight.Text) - double.Parse(txtsecondweight.Text)).ToString();
					txtWeight.Text = txtNetWeight.Text;
				}
				if (int.Parse(txtfirstweight.Text.ToString()) < int.Parse(txtsecondweight.Text.ToString()))
				{
					txtNetWeight.Text = (double.Parse(txtsecondweight.Text) - double.Parse(txtfirstweight.Text)).ToString();
					txtWeight.Text = txtNetWeight.Text;
				}
			}
			else
			{
				txtfirstweight.Text = string.Empty;
				txtsecondweight.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		if (!UpdateMode)
		{
			ScaleSlip();
		}
		CalculateWeight();
	}

	private void comWagesAccount_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			InvContractorWagesSchedule val = new InvContractorWagesSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtwageacc = new DataTable();
			dtwageacc = InvContractorWagesSchedule.GetAll(val)?.Tables[0];
			if (dtwageacc.Rows.Count > 0)
			{
				for (int i = 0; i < dtwageacc.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtwageacc.Rows[i]["InvConractorWagesAccountsId"]) == Conversion.ToInt(comWagesAccount.Value))
					{
						txtwagesrate.Text = dtwageacc.Rows[i]["WageRate"].ToString();
						txtPackSize.Text = dtwageacc.Rows[i]["RateUom"].ToString();
						if (txtPackSize.Text == "100")
						{
							txtWeightCut.Text = "1.25";
						}
						else
						{
							txtWeightCut.Text = "1";
						}
						if (txtWeightCut.Text != string.Empty)
						{
							calculateNetWeight();
						}
					}
				}
			}
			CalaulationQuantity();
			CalculationAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWeight_TextChanged(object sender, EventArgs e)
	{
		CalaulationQuantity();
		CalculationAmount();
		calculateNetWeight();
	}

	private void txtPackSize_TextChanged(object sender, EventArgs e)
	{
		CalaulationQuantity();
		if (txtPackSize.Text == "100")
		{
			txtWeightCut.Text = "1.25";
		}
		else
		{
			txtWeightCut.Text = "1";
		}
		calculateNetWeight();
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		CalculationUOM();
		CalculationAmount();
		calculateNetWeight();
	}

	public void CalaulationQuantity()
	{
		try
		{
			if (txtWeight.Text != string.Empty && txtPackSize.Text != string.Empty)
			{
				double num = double.Parse(txtWeight.Text.Trim());
				double b = double.Parse(txtPackSize.Text.Trim());
				double qty = num / b;
				txtqty.Text = Math.Round(qty).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CalculationUOM()
	{
	}

	public void CalculationAmount()
	{
		try
		{
			if (txtBillQty.Text != string.Empty && txtwagesrate.Text != string.Empty)
			{
				double num = double.Parse(txtwagesrate.Text.Trim());
				double b = double.Parse(txtBillQty.Text.Trim());
				double Amount = num * b;
				txtwageamount.Text = Math.Round(Amount).ToString();
			}
			else
			{
				txtwageamount.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CalculateWeight()
	{
		try
		{
			if (txtfirstweight.Text != string.Empty && txtsecondweight.Text != string.Empty)
			{
				double frate = double.Parse(txtfirstweight.Text.Trim());
				double secondRate = double.Parse(txtsecondweight.Text.Trim());
				if (frate > secondRate)
				{
					double NetWeight = frate - secondRate;
					txtWeight.Text = Math.Round(NetWeight).ToString();
				}
				if (secondRate > frate)
				{
					double NetWeight = secondRate - frate;
					txtWeight.Text = Math.Round(NetWeight).ToString();
				}
			}
			else
			{
				txtwageamount.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GetPackUOM()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		ReportsParameters val = new ReportsParameters
		{
			PackUomFrom = Conversion.ToInt((object)txtPackSize.Text),
			InvConractorWagesAccountsId = Conversion.ToInt(comWagesAccount.Value)
		};
		double Rate = 0.0;
		GetWagesSchedule d = InvContractorWagesSchedule.GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize(val);
		if (d != null)
		{
			Rate = d.WagesRate;
		}
		if (Rate > 0.0)
		{
			txtwagesrate.Text = Rate.ToString();
		}
		else
		{
			txtwagesrate.Text = "";
		}
	}

	private void txtwagesrate_TextChanged(object sender, EventArgs e)
	{
		CalculationAmount();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Expected O, but got Unknown
		try
		{
			if (!FormValidation() || MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GenerateDocNo();
			InvContractorWagesBillHeader billheader = new InvContractorWagesBillHeader();
			billheader.CompanyId = UserAccount.CompanyId;
			billheader.OrganizationId = UserAccount.OrganizationId;
			billheader.DocNo = Conversion.ToInt((object)txtdocnumber.Text);
			billheader.DocDate = txtDocdate.Value;
			billheader.DocumentTypeId = 101;
			billheader.RefDocumentTypeId = 74;
			billheader.RefDocNo = Conversion.ToInt((object)((Control)(object)comReferenceNo).Text);
			billheader.RefDocNoId = Conversion.ToInt(comReferenceNo.Value);
			billheader.WeightTotal = Conversion.ToDouble((object)txtNetWeight.Text);
			billheader.QtyTotal = Conversion.ToInt((object)txtqtyMain.Text);
			billheader.OtherRemarks = Conversion.ToString((object)txtRemarks.Text);
			billheader.EntryUser = Conversion.ToInt((object)UserAccount.ID);
			billheader.ModifyUser = Conversion.ToInt((object)UserAccount.ID);
			billheader.ScaleSlipNo = 0;
			billheader.ModifyDate = DateTime.Now;
			billheader.EntryDate = DateTime.Now;
			if (chkisapprove.Checked)
			{
				billheader.IsAproved = true;
			}
			else
			{
				billheader.IsAproved = false;
			}
			billheader.ApprovedDate = DateTime.Now;
			billheader.ApprovedUserId = Conversion.ToInt((object)UserAccount.ID);
			billheader.BranchesId = 0;
			billheader.ProjectsId = 0;
			billheader.invContractWagesBillDateil = new List<InvContractorWagesBillDetail>();
			if (grdwagesDetail.RowCount > 0)
			{
				GridEXRow[] rows = grdwagesDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvContractorWagesBillDetail wagesbilldetail = new InvContractorWagesBillDetail();
					wagesbilldetail.InvConractorWagesAccountsId = Conversion.ToInt(r.Cells["WagesId"].Value);
					wagesbilldetail.WbTransactionsIdDt = Conversion.ToInt(r.Cells["ScaleSlipNoId"].Value);
					wagesbilldetail.Qty = Conversion.ToInt((object)r.Cells["Quantity"].Text);
					wagesbilldetail.Weight = Conversion.ToDouble((object)r.Cells["Weight"].Text);
					wagesbilldetail.PackSize = Conversion.ToInt((object)r.Cells["PackSize"].Text);
					wagesbilldetail.WageRate = Conversion.ToDouble((object)r.Cells["Rate"].Text);
					wagesbilldetail.WagesAmount = Conversion.ToDouble((object)r.Cells["Amount"].Text);
					wagesbilldetail.ContractorId = Conversion.ToInt((object)r.Cells["SupplierId"].Text);
					wagesbilldetail.ItemId = Conversion.ToInt((object)r.Cells["ItemId"].Text);
					wagesbilldetail.JobLotId = Conversion.ToInt((object)r.Cells["JobLotId"].Text);
					wagesbilldetail.Crop = Conversion.ToString((object)r.Cells["Crop"].Text);
					wagesbilldetail.InvPackingTypeId = Conversion.ToInt((object)r.Cells["packingTypeId"].Text);
					wagesbilldetail.WareHouseFromId = Conversion.ToInt((object)r.Cells["MoveFromId"].Text);
					wagesbilldetail.WareHouseToId = Conversion.ToInt((object)r.Cells["MoveToId"].Text);
					wagesbilldetail.BillQty = Conversion.ToInt((object)r.Cells["BillQty"].Text);
					wagesbilldetail.BillWeight = Conversion.ToDouble((object)r.Cells["BillWeight"].Text);
					wagesbilldetail.WeightCut = Conversion.ToDouble((object)r.Cells["WeightCut"].Text);
					billheader.invContractWagesBillDateil.Add(wagesbilldetail);
				}
				int success = InvContractorWagesBillHeader.Save(billheader);
				MessageBox.Show("Saved SuccessFully" + billheader.DocNo);
				bindHistory();
				FormRest();
				dtdetail.Clear();
				grdwagesDetail.ClearStructure();
				comRefenenceType.Focus();
				if (ChkBoxPrint.Checked)
				{
					Reporting rpt = new Reporting();
					rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
					rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
					rpt.RptPerameter("@HeaderId", success.ToString());
					rpt.RptPerameter("@BillDateFrom", string.Empty);
					rpt.RptPerameter("@BillDateTo", string.Empty);
					rpt.RptPerameter("@BillSrFrom", "0");
					rpt.RptPerameter("@BillSrTo", "0");
					rpt.RptPerameter("@SupplierCustomerId", "0");
					rpt.RptPerameter("@CwCoaId", "0");
					if (chkisapprove.Checked)
					{
						rpt.RptPerameter("@IsApproved", "True");
					}
					else
					{
						rpt.RptPerameter("@IsApproved", "False");
					}
					rpt.ShowReport("RptCWBInward.rpt");
				}
			}
			else
			{
				MessageBox.Show("Add Detail Record in Grid");
				cmbScaleSlip.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		try
		{
			if (!FormValidation() || MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			InvContractorWagesBillHeader billheader = new InvContractorWagesBillHeader();
			billheader.Id = RECID;
			billheader.CompanyId = UserAccount.CompanyId;
			billheader.OrganizationId = UserAccount.OrganizationId;
			billheader.DocNo = Conversion.ToInt((object)txtdocnumber.Text);
			billheader.DocDate = txtDocdate.Value;
			billheader.DocumentTypeId = 101;
			billheader.RefDocumentTypeId = 74;
			billheader.RefDocNo = Conversion.ToInt((object)((Control)(object)comReferenceNo).Text);
			billheader.RefDocNoId = Conversion.ToInt(comReferenceNo.Value);
			billheader.WeightTotal = Conversion.ToDouble((object)txtNetWeight.Text);
			billheader.QtyTotal = Conversion.ToInt((object)txtqtyMain.Text);
			billheader.OtherRemarks = Conversion.ToString((object)txtRemarks.Text);
			billheader.ModifyUser = Conversion.ToInt((object)UserAccount.ID);
			billheader.ScaleSlipNo = 0;
			billheader.EntryDate = DateTime.Now;
			billheader.ModifyDate = DateTime.Now;
			if (chkisapprove.Checked)
			{
				billheader.IsAproved = true;
			}
			else
			{
				billheader.IsAproved = false;
			}
			billheader.ApprovedDate = DateTime.Now;
			billheader.ApprovedUserId = Conversion.ToInt((object)UserAccount.ID);
			billheader.BranchesId = 0;
			billheader.ProjectsId = 0;
			billheader.invContractWagesBillDateil = new List<InvContractorWagesBillDetail>();
			if (grdwagesDetail.RowCount > 0)
			{
				GridEXRow[] rows = grdwagesDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvContractorWagesBillDetail wagesbilldetail = new InvContractorWagesBillDetail();
					wagesbilldetail.InvConractorWagesAccountsId = Conversion.ToInt((object)r.Cells["WagesId"].Text);
					wagesbilldetail.WbTransactionsIdDt = Conversion.ToInt((object)r.Cells["ScaleSlipNoId"].Text);
					wagesbilldetail.Qty = Conversion.ToInt((object)r.Cells["Quantity"].Text);
					wagesbilldetail.Weight = Conversion.ToDouble((object)r.Cells["Weight"].Text);
					wagesbilldetail.PackSize = Conversion.ToInt((object)r.Cells["PackSize"].Text);
					wagesbilldetail.WageRate = Conversion.ToDouble((object)r.Cells["Rate"].Text);
					wagesbilldetail.WagesAmount = Conversion.ToDouble((object)r.Cells["Amount"].Text);
					wagesbilldetail.ContractorId = Conversion.ToInt((object)r.Cells["SupplierId"].Text);
					wagesbilldetail.ItemId = Conversion.ToInt((object)r.Cells["ItemId"].Text);
					wagesbilldetail.JobLotId = Conversion.ToInt((object)r.Cells["JobLotId"].Text);
					wagesbilldetail.Crop = Conversion.ToString((object)r.Cells["Crop"].Text);
					wagesbilldetail.InvPackingTypeId = Conversion.ToInt((object)r.Cells["packingTypeId"].Text);
					wagesbilldetail.WareHouseFromId = Conversion.ToInt((object)r.Cells["MoveFromId"].Text);
					wagesbilldetail.WareHouseToId = Conversion.ToInt((object)r.Cells["MoveToId"].Text);
					wagesbilldetail.BillQty = Conversion.ToInt((object)r.Cells["BillQty"].Text);
					wagesbilldetail.BillWeight = Conversion.ToDouble((object)r.Cells["BillWeight"].Text);
					wagesbilldetail.WeightCut = Conversion.ToDouble((object)r.Cells["WeightCut"].Text);
					billheader.invContractWagesBillDateil.Add(wagesbilldetail);
				}
				int success = InvContractorWagesBillHeader.Save(billheader);
				MessageBox.Show("Update SuccessFully" + billheader.DocNo);
				bindHistory();
				FormRest();
				dtdetail.Clear();
				grdwagesDetail.ClearStructure();
				comRefenenceType.Focus();
				if (ChkBoxPrint.Checked)
				{
					Reporting rpt = new Reporting();
					rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
					rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
					rpt.RptPerameter("@HeaderId", success.ToString());
					rpt.RptPerameter("@BillDateFrom", string.Empty);
					rpt.RptPerameter("@BillDateTo", string.Empty);
					rpt.RptPerameter("@BillSrFrom", "0");
					rpt.RptPerameter("@BillSrTo", "0");
					rpt.RptPerameter("@SupplierCustomerId", "0");
					rpt.RptPerameter("@CwCoaId", "0");
					if (chkisapprove.Checked)
					{
						rpt.RptPerameter("@IsApproved", "True");
					}
					else
					{
						rpt.RptPerameter("@IsApproved", "False");
					}
					rpt.ShowReport("RptCWBInward.rpt");
				}
			}
			else
			{
				MessageBox.Show("Add Detail Record in Grid");
				cmbScaleSlip.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	public void bindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		ReportsParameters val = new ReportsParameters
		{
			CompanyId = UserAccount.CompanyId,
			OrganizationId = UserAccount.OrganizationId,
			RefDocumentTypeIdMo = 74
		};
		DataTable dtgetAllheader = new DataTable();
		dtgetAllheader = InvContractorWagesBillHeader.GetAll(val);
		DataTable dt = new DataTable();
		dt.Columns.Add("Id");
		dt.Columns.Add("ScaleSlip#");
		dt.Columns.Add("DocumentNo");
		dt.Columns.Add("DocumentDate");
		dt.Columns.Add("DocumentType");
		dt.Columns.Add("ReferenceNo");
		dt.Columns.Add("Supplier/Customer");
		dt.Columns.Add("Weight");
		dt.Columns.Add("Qty");
		dt.Columns.Add("OtherRemarks");
		dt.Columns.Add("IsApproved");
		DataGridHistory.DataSource = null;
		for (int i = 0; i < dtgetAllheader.Rows.Count; i++)
		{
			dt.Rows.Add(dtgetAllheader.Rows[i]["Id"], dtgetAllheader.Rows[i]["TicketNo"], dtgetAllheader.Rows[i]["DocNo"], Conversion.ToDateTime(dtgetAllheader.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtgetAllheader.Rows[i]["DocumentTypeDescription"], dtgetAllheader.Rows[i]["RefDocNo"], dtgetAllheader.Rows[i]["CompanyName"], dtgetAllheader.Rows[i]["WeightTotal"], dtgetAllheader.Rows[i]["QtyTotal"], dtgetAllheader.Rows[i]["OtherRemarks"], dtgetAllheader.Rows[i]["IsAproved"]);
		}
		DataGridHistory.DataSource = dt;
		DataGridHistory.RetrieveStructure();
		HistoryGridSettings();
	}

	private void HistoryGridSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ScaleSlip#"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocumentNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocumentDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocumentType"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ReferenceNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Supplier/Customer"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["IsApproved"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Weight"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Qty"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["OtherRemarks"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Id"].Width = 100;
			DataGridHistory.RootTable.Columns["DocumentNo"].Width = 100;
			DataGridHistory.RootTable.Columns["ReferenceNo"].Width = 100;
			DataGridHistory.RootTable.Columns["Supplier/Customer"].Width = 200;
			DataGridHistory.RootTable.Columns["IsApproved"].Width = 100;
			DataGridHistory.RootTable.Columns["DocumentType"].Width = 200;
			DataGridHistory.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ScaleSlip#"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["DocumentNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocumentDate"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocumentType"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ReferenceNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Supplier/Customer"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["OtherRemarks"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["IsApproved"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["IsApproved"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["IsApproved"].ColumnType = (ColumnType)3;
			DataGridHistory.RootTable.Columns["IsApproved"].CheckBoxTrueValue = "True";
			DataGridHistory.RootTable.Columns["IsApproved"].CheckBoxFalseValue = "False";
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns.Add("Detail");
			DataGridHistory.RootTable.Columns["Detail"].Caption = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	public void ReadById(int ID)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			InvContractorWagesBillHeader head = new InvContractorWagesBillHeader();
			RECID = ID;
			head = InvContractorWagesBillHeader.GetByID(RECID);
			tabControl1.SelectedIndex = 0;
			head.Id = RECID;
			txtDocdate.Value = head.DocDate;
			txtdocnumber.Text = head.DocNo.ToString();
			comRefenenceType.SelectedValue = head.RefDocumentTypeId;
			comReferenceNo.Value = head.RefDocNoId;
			comRefenenceType.Enabled = false;
			((Control)(object)comReferenceNo).Enabled = false;
			((Control)(object)cmbScaleSlip).Enabled = false;
			txtRemarks.Text = head.OtherRemarks;
			chkisapprove.Checked = head.IsAproved;
			dtdetail.Clear();
			for (int i = 0; i < head.invContractWagesBillDateil.Count; i++)
			{
				dtdetail.Rows.Add(head.invContractWagesBillDateil[i].ContractorId, head.invContractWagesBillDateil[i].CompanyName, head.invContractWagesBillDateil[i].InvConractorWagesAccountsId, head.invContractWagesBillDateil[i].WagesAccountName, head.invContractWagesBillDateil[i].InvPackingTypeId, head.invContractWagesBillDateil[i].PackTypeDesc, head.invContractWagesBillDateil[i].WbTransactionsIdDt, head.invContractWagesBillDateil[i].TicketNo, head.invContractWagesBillDateil[i].Weight, head.invContractWagesBillDateil[i].PackSize, head.invContractWagesBillDateil[i].Qty, head.invContractWagesBillDateil[i].WeightCut, head.invContractWagesBillDateil[i].BillQty, head.invContractWagesBillDateil[i].BillWeight, head.invContractWagesBillDateil[i].WageRate, head.invContractWagesBillDateil[i].WagesAmount, head.invContractWagesBillDateil[i].ItemId, head.invContractWagesBillDateil[i].ItemName, head.invContractWagesBillDateil[i].JobLotId, head.invContractWagesBillDateil[i].JobLotDescription, head.invContractWagesBillDateil[i].Crop, head.invContractWagesBillDateil[i].WareHouseFromId, head.invContractWagesBillDateil[i].WareHouseFrom, head.invContractWagesBillDateil[i].WareHouseToId, head.invContractWagesBillDateil[i].WareHouseTo);
			}
			grdwagesDetail.DataSource = dtdetail;
			grdwagesDetail.RetrieveStructure();
			UpdateMode = true;
			DetailGridSettings();
			comReferenceNo_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void PendingTicket()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		WbTransactions val = new WbTransactions
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			RefDocumentTypeIdGpIn = 0,
			RefDocumentTypeIdGpOut = 0,
			RefDocumentTypeIdSt = 74
		};
		DataTable dtpendingtick = new DataTable();
		dtpendingtick = WbTransactions.GetAllPendingTicketNo(val);
		dtPendingTicket.Clear();
		for (int i = 0; i < dtpendingtick.Rows.Count; i++)
		{
			dtPendingTicket.Rows.Add(dtpendingtick.Rows[i]["TranType"], dtpendingtick.Rows[i]["ReferenceDocNoId"], dtpendingtick.Rows[i]["GpDate"], dtpendingtick.Rows[i]["VarietyName"], dtpendingtick.Rows[i]["TicketNo"]);
		}
		gridPending.DataSource = dtPendingTicket;
		gridPending.RetrieveStructure();
		GridPendingTicketSetting();
	}

	public void GridPendingTicketSetting()
	{
		gridPending.RootTable.Columns["TranType"].EditType = (EditType)0;
		gridPending.RootTable.Columns["ReferenceNo"].EditType = (EditType)0;
		gridPending.RootTable.Columns["Date"].EditType = (EditType)0;
		gridPending.RootTable.Columns["Variety"].EditType = (EditType)0;
		gridPending.RootTable.Columns["Slip#"].EditType = (EditType)0;
		gridPending.RootTable.Columns["TranType"].FilterEditType = (FilterEditType)0;
		gridPending.RootTable.Columns["ReferenceNo"].FilterEditType = (FilterEditType)0;
		gridPending.RootTable.Columns["Date"].FilterEditType = (FilterEditType)0;
		gridPending.RootTable.Columns["Variety"].FilterEditType = (FilterEditType)0;
		gridPending.RootTable.Columns["Slip#"].FilterEditType = (FilterEditType)0;
		gridPending.RootTable.Columns["TranType"].Width = 200;
		gridPending.RootTable.Columns["ReferenceNo"].Width = 200;
		gridPending.RootTable.Columns["Date"].Width = 200;
		gridPending.RootTable.Columns["Variety"].Width = 200;
		gridPending.RootTable.Columns["Slip#"].Width = 200;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		((Control)(object)comReferenceNo).Enabled = true;
		comRefenenceType.Enabled = true;
		((Control)(object)cmbScaleSlip).Enabled = true;
		FormRest();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		UpdateMode = false;
		suppliercustomer();
		Warehouse();
		Item();
		CropYear();
		combojoblotfill();
		bindPackType();
		accountName();
		suppliercustomer();
		dtdetail.Clear();
		PendingTicket();
		comRefenenceType.Focus();
	}

	public void FormRest()
	{
		txtqtyMain.Text = string.Empty;
		txtfirstweight.Text = string.Empty;
		txtsecondweight.Text = string.Empty;
		txtqty.Text = string.Empty;
		txtRemarks.Text = string.Empty;
		txtwageamount.Text = string.Empty;
		txtwagesrate.Text = string.Empty;
		txtWeight.Text = string.Empty;
		txtPackSize.Text = string.Empty;
		txtNetWeight.Text = string.Empty;
		((Control)(object)comReferenceNo).Text = string.Empty;
		((Control)(object)cmbScaleSlip).Text = string.Empty;
		((Control)(object)comSupplier).Text = string.Empty;
		((Control)(object)comWagesAccount).Text = string.Empty;
		((Control)(object)cmbPackingType).Text = string.Empty;
		((Control)(object)cmbJobLot).Text = string.Empty;
		((Control)(object)cmbCrop).Text = string.Empty;
		((Control)(object)cmbItem).Text = string.Empty;
		((Control)(object)cmbMoveFrom).Text = string.Empty;
		((Control)(object)cmbmoveTo).Text = string.Empty;
		txtWeightCut.Text = string.Empty;
		txtBillQty.Text = string.Empty;
		txtBillWeight.Text = string.Empty;
		GenerateDocNo();
		txtPackSize.Text = string.Empty;
		grdwagesDetail.ClearStructure();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		btnAdd.Visible = true;
		((Control)(object)comReferenceNo).Enabled = true;
		comRefenenceType.Enabled = true;
		((Control)(object)cmbScaleSlip).Enabled = true;
		comRefenenceType.Focus();
	}

	public void resetDetail()
	{
		txtwageamount.Text = string.Empty;
		txtwagesrate.Text = string.Empty;
		txtqty.Text = string.Empty;
		txtWeight.Text = string.Empty;
		txtPackSize.Text = string.Empty;
		txtWeightCut.Text = string.Empty;
		txtBillQty.Text = string.Empty;
		txtBillWeight.Text = string.Empty;
		((Control)(object)cmbScaleSlip).Text = string.Empty;
		((Control)(object)comSupplier).Text = string.Empty;
		((Control)(object)comWagesAccount).Text = string.Empty;
		((Control)(object)cmbPackingType).Text = string.Empty;
		((Control)(object)cmbJobLot).Text = string.Empty;
		((Control)(object)cmbCrop).Text = string.Empty;
		((Control)(object)cmbItem).Text = string.Empty;
		((Control)(object)cmbMoveFrom).Text = string.Empty;
		((Control)(object)cmbmoveTo).Text = string.Empty;
		accountName();
		Warehouse();
		Item();
		CropYear();
		combojoblotfill();
		bindPackType();
		suppliercustomer();
	}

	private bool FormValidationOfDetailPortion()
	{
		if (((Control)(object)comWagesAccount).Text == string.Empty)
		{
			MessageBox.Show("Wages Account Type Field Required");
			comWagesAccount.Focus();
			return false;
		}
		if (((Control)(object)comSupplier).Text == string.Empty)
		{
			MessageBox.Show("Contractor Name Type Field Required");
			comWagesAccount.Focus();
			return false;
		}
		if (((Control)(object)cmbScaleSlip).Text == string.Empty)
		{
			MessageBox.Show("Scale Slip # Field Required");
			cmbScaleSlip.Focus();
			return false;
		}
		if (txtWeight.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Weight  Field Required");
			txtWeight.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Quantity  Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (txtPackSize.Text.Trim() == string.Empty)
		{
			MessageBox.Show("UOM  Field Required");
			txtPackSize.Focus();
			return false;
		}
		if (txtwagesrate.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Wages Rate Field Required");
			txtwagesrate.Focus();
			return false;
		}
		if (txtwageamount.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Wages Amount Field Required");
			txtwageamount.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidation()
	{
		if (txtdocnumber.Text.Trim() == string.Empty || txtdocnumber.Text.Trim() == "0")
		{
			MessageBox.Show("document Number Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (!UpdateMode)
		{
			if (((Control)(object)comReferenceNo).Text.Trim() == string.Empty || ((Control)(object)comReferenceNo).Text.Trim() == "0")
			{
				MessageBox.Show("Reference No Field Required");
				comReferenceNo.Focus();
				return false;
			}
			return true;
		}
		return true;
	}

	private void frmMoveOrder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
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
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortion())
			{
				dtdetail.Rows.Add(comSupplier.Value, ((Control)(object)comSupplier).Text, comWagesAccount.Value, ((Control)(object)comWagesAccount).Text, cmbPackingType.Value, ((Control)(object)cmbPackingType).Text, cmbScaleSlip.Value, ((Control)(object)cmbScaleSlip).Text, txtWeight.Text, txtPackSize.Text, txtqty.Text, txtWeightCut.Text, txtBillQty.Text, txtBillWeight.Text, txtwagesrate.Text, txtwageamount.Text, cmbItem.Value, ((Control)(object)cmbItem).Text, cmbJobLot.Value, ((Control)(object)cmbJobLot).Text, ((Control)(object)cmbCrop).Text, cmbMoveFrom.Value, ((Control)(object)cmbMoveFrom).Text, cmbmoveTo.Value, ((Control)(object)cmbmoveTo).Text);
				grdwagesDetail.DataSource = dtdetail;
				grdwagesDetail.RetrieveStructure();
				resetDetail();
				DetailGridSettings();
				Warehouse();
				Item();
				CropYear();
				combojoblotfill();
				bindPackType();
				suppliercustomer();
				accountName();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comRefenenceType_Leave(object sender, EventArgs e)
	{
		referenceNo();
	}

	public void referenceNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			DataTable dtregDocNoIdbyDocTypeId = GatePassInward.GetRefDocTypeIdbyRefDocNoId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				RefDocumentTypeId = 74,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ReferenceNo");
			for (int i = 0; i < dtregDocNoIdbyDocTypeId.Rows.Count; i++)
			{
				dt.Rows.Add(dtregDocNoIdbyDocTypeId.Rows[i]["Id"], dtregDocNoIdbyDocTypeId.Rows[i]["ReferenceDocNoId"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, comReferenceNo, "Id", "ReferenceNo", "Reference No", ZeroIndex: false);
			}
			else
			{
				DDL.BindDDL(dt, comReferenceNo, "Id", "ReferenceNo", "Reference No", ZeroIndex: false);
			}
			comReferenceNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSettings()
	{
		try
		{
			grdwagesDetail.RootTable.Columns["SupplierName"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["packingType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["ScaleSlipNo"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Quantity"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["jobLot"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveFrom"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveTo"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Crop"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["SupplierName"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["WagesAccount"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["packingType"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["ScaleSlipNo"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["PackSize"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["Item"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["jobLot"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["MoveFrom"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["MoveTo"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["Crop"].FilterEditType = (FilterEditType)0;
			grdwagesDetail.RootTable.Columns["SupplierId"].Visible = false;
			grdwagesDetail.RootTable.Columns["WagesId"].Visible = false;
			grdwagesDetail.RootTable.Columns["ItemId"].Visible = false;
			grdwagesDetail.RootTable.Columns["jobLotId"].Visible = false;
			grdwagesDetail.RootTable.Columns["packingTypeId"].Visible = false;
			grdwagesDetail.RootTable.Columns["MoveFromId"].Visible = false;
			grdwagesDetail.RootTable.Columns["MoveToId"].Visible = false;
			grdwagesDetail.RootTable.Columns["ScaleSlipNoId"].Visible = false;
			grdwagesDetail.RootTable.Columns["SupplierName"].Width = 200;
			grdwagesDetail.RootTable.Columns["WagesAccount"].Width = 200;
			grdwagesDetail.RootTable.Columns["packingType"].Width = 150;
			grdwagesDetail.RootTable.Columns["ScaleSlipNo"].Width = 50;
			grdwagesDetail.RootTable.Columns["Weight"].Width = 50;
			grdwagesDetail.RootTable.Columns["PackSize"].Width = 50;
			grdwagesDetail.RootTable.Columns["Quantity"].Width = 50;
			grdwagesDetail.RootTable.Columns["Rate"].Width = 50;
			grdwagesDetail.RootTable.Columns["Amount"].Width = 50;
			grdwagesDetail.RootTable.Columns["Item"].Width = 200;
			grdwagesDetail.RootTable.Columns["jobLot"].Width = 100;
			grdwagesDetail.RootTable.Columns["MoveFrom"].Width = 200;
			grdwagesDetail.RootTable.Columns["MoveTo"].Width = 200;
			grdwagesDetail.RootTable.Columns["Crop"].Width = 100;
			grdwagesDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortion())
			{
				dtdetail.Rows[updateDetailIndex]["SupplierId"] = comSupplier.Value;
				dtdetail.Rows[updateDetailIndex]["SupplierName"] = ((Control)(object)comSupplier).Text;
				dtdetail.Rows[updateDetailIndex]["WagesId"] = comWagesAccount.Value;
				dtdetail.Rows[updateDetailIndex]["WagesAccount"] = ((Control)(object)comWagesAccount).Text;
				dtdetail.Rows[updateDetailIndex]["ScaleSlipNoId"] = cmbScaleSlip.Value;
				dtdetail.Rows[updateDetailIndex]["ScaleSlipNo"] = ((Control)(object)cmbScaleSlip).Text;
				dtdetail.Rows[updateDetailIndex]["Quantity"] = txtqty.Text;
				dtdetail.Rows[updateDetailIndex]["Weight"] = txtWeight.Text;
				dtdetail.Rows[updateDetailIndex]["PackSize"] = txtPackSize.Text;
				dtdetail.Rows[updateDetailIndex]["Rate"] = txtwagesrate.Text;
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtwageamount.Text;
				dtdetail.Rows[updateDetailIndex]["ItemId"] = cmbItem.Value;
				dtdetail.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbItem).Text;
				dtdetail.Rows[updateDetailIndex]["jobLotId"] = cmbJobLot.Value;
				dtdetail.Rows[updateDetailIndex]["jobLot"] = ((Control)(object)cmbJobLot).Text;
				dtdetail.Rows[updateDetailIndex]["Crop"] = ((Control)(object)cmbCrop).Text;
				dtdetail.Rows[updateDetailIndex]["packingTypeId"] = cmbPackingType.Value;
				dtdetail.Rows[updateDetailIndex]["packingType"] = ((Control)(object)cmbPackingType).Text;
				dtdetail.Rows[updateDetailIndex]["MoveFromId"] = cmbMoveFrom.Value;
				dtdetail.Rows[updateDetailIndex]["MoveFrom"] = ((Control)(object)cmbMoveFrom).Text;
				dtdetail.Rows[updateDetailIndex]["MoveToId"] = cmbmoveTo.Value;
				dtdetail.Rows[updateDetailIndex]["MoveTo"] = ((Control)(object)cmbmoveTo).Text;
				dtdetail.Rows[updateDetailIndex]["WeightCut"] = txtWeightCut.Text;
				dtdetail.Rows[updateDetailIndex]["BillWeight"] = txtBillWeight.Text;
				dtdetail.Rows[updateDetailIndex]["BillQty"] = txtBillQty.Text;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				btnAdd.Visible = true;
				resetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		btnAdd.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelDetail.Visible = false;
		resetDetail();
	}

	private void grdwagesDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdwagesDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)comWagesAccount).Text = item.Cells["WagesAccount"].Text.ToString();
				((Control)(object)comSupplier).Text = item.Cells["SupplierName"].Text.ToString();
				cmbItem.Value = item.Cells["ItemId"].Text;
				cmbJobLot.Value = item.Cells["JobLotId"].Text;
				((Control)(object)cmbCrop).Text = item.Cells["Crop"].Text;
				cmbPackingType.Value = item.Cells["packingTypeId"].Text;
				cmbmoveTo.Value = item.Cells["MoveToId"].Text;
				cmbMoveFrom.Value = item.Cells["MoveFromId"].Text;
				cmbScaleSlip.Value = item.Cells["ScaleSlipNoId"].Text;
				txtWeight.Text = item.Cells["Weight"].Text.ToString();
				txtqty.Text = item.Cells["Quantity"].Text.ToString();
				txtwagesrate.Text = item.Cells["Rate"].Text.ToString();
				txtPackSize.Text = item.Cells["PackSize"].Text.ToString();
				txtwageamount.Text = item.Cells["Amount"].Text.ToString();
				txtWeightCut.Text = item.Cells["WeightCut"].Text.ToString();
				txtBillQty.Text = item.Cells["BillQty"].Text.ToString();
				txtBillWeight.Text = item.Cells["BillWeight"].Text.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				comReferenceNo_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@HeaderId", Id.ToString());
			rpt.RptPerameter("@BillDateFrom", string.Empty);
			rpt.RptPerameter("@BillDateTo", string.Empty);
			rpt.RptPerameter("@BillSrFrom", "0");
			rpt.RptPerameter("@BillSrTo", "0");
			rpt.RptPerameter("@SupplierCustomerId", "0");
			rpt.RptPerameter("@CwCoaId", "0");
			if (chkisapprove.Checked)
			{
				rpt.RptPerameter("@IsApproved", "True");
			}
			else
			{
				rpt.RptPerameter("@IsApproved", "False");
			}
			rpt.ShowReport("RptCWBInward.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocnumber_TextChanged(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			comRefenenceType.SelectedIndex = 0;
			string code = InvContractorWagesBillHeader.GetContractorWagesBillHeaderIdByDocNo(new InvContractorWagesBillHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocNo = Conversion.ToInt((object)txtdocnumber.Text.Trim()),
				RefDocumentTypeId = 74
			});
			if (Conversion.ToInt((object)code) > 0)
			{
				ReadById(Conversion.ToInt((object)code));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton4_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Reporting rpt = new Reporting();
			rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@HeaderId", DataGridHistory.CurrentRow.Cells[0].Value.ToString());
			rpt.RptPerameter("@BillDateFrom", string.Empty);
			rpt.RptPerameter("@BillDateTo", string.Empty);
			rpt.RptPerameter("@BillSrFrom", "0");
			rpt.RptPerameter("@BillSrTo", "0");
			rpt.RptPerameter("@SupplierCustomerId", "0");
			rpt.RptPerameter("@CwCoaId", "0");
			if (chkisapprove.Checked)
			{
				rpt.RptPerameter("@IsApproved", "True");
			}
			else
			{
				rpt.RptPerameter("@IsApproved", "False");
			}
			rpt.ShowReport("RptCWBInward.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void calculateNetWeight()
	{
		try
		{
			if (txtqty.Text != string.Empty && txtWeight.Text != string.Empty)
			{
				if (txtWeightCut.Text.Trim() != string.Empty)
				{
					if (double.Parse(txtWeightCut.Text.ToString()) == 0.0)
					{
						double Fweight = double.Parse(txtWeight.Text.Trim());
						txtBillWeight.Text = Math.Round(Fweight).ToString();
					}
					if (double.Parse(txtWeightCut.Text) > 0.0)
					{
						double fQty = double.Parse(txtqty.Text.Trim());
						double wtct = double.Parse(txtWeightCut.Text);
						double num = double.Parse(txtWeight.Text.Trim());
						double calculateqty = fQty * wtct;
						double NetWeight = num - calculateqty;
						txtBillWeight.Text = Math.Round(NetWeight).ToString();
					}
				}
			}
			else
			{
				txtwageamount.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void calculateNetQty()
	{
		try
		{
			if (txtBillWeight.Text != string.Empty && txtPackSize.Text != string.Empty)
			{
				double num = double.Parse(txtBillWeight.Text.Trim());
				double b = double.Parse(txtPackSize.Text.Trim());
				double qty = num / b;
				txtBillQty.Text = Math.Round(qty).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWeightCut_TextChanged(object sender, EventArgs e)
	{
		if (txtWeightCut.Text != string.Empty)
		{
			calculateNetWeight();
		}
	}

	private void txtBillQty_TextChanged(object sender, EventArgs e)
	{
		CalculationAmount();
	}

	private void txtBillWeight_TextChanged(object sender, EventArgs e)
	{
		calculateNetQty();
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Detail")
			{
				HistoryDetailGridBind(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void HistoryDetailGridBind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvContractorWagesBillHeader head = new InvContractorWagesBillHeader();
			head = InvContractorWagesBillHeader.GetByID(Id);
			DataTable dtdetailHistory = new DataTable();
			dtdetailHistory.Columns.Add("SupplierName");
			dtdetailHistory.Columns.Add("WagesAccount");
			dtdetailHistory.Columns.Add("packingType");
			dtdetailHistory.Columns.Add("ScaleSlipNo");
			dtdetailHistory.Columns.Add("Weight", typeof(double));
			dtdetailHistory.Columns.Add("PackSize");
			dtdetailHistory.Columns.Add("Quantity", typeof(double));
			dtdetailHistory.Columns.Add("WeightCut");
			dtdetailHistory.Columns.Add("BillQty", typeof(double));
			dtdetailHistory.Columns.Add("BillWeight", typeof(double));
			dtdetailHistory.Columns.Add("Rate");
			dtdetailHistory.Columns.Add("Amount", typeof(double));
			for (int i = 0; i < head.invContractWagesBillDateil.Count; i++)
			{
				dtdetailHistory.Rows.Add(head.invContractWagesBillDateil[i].CompanyName, head.invContractWagesBillDateil[i].WagesAccountName, head.invContractWagesBillDateil[i].PackTypeDesc, head.invContractWagesBillDateil[i].TicketNo, head.invContractWagesBillDateil[i].Weight, head.invContractWagesBillDateil[i].PackSize, head.invContractWagesBillDateil[i].Qty, head.invContractWagesBillDateil[i].WeightCut, head.invContractWagesBillDateil[i].BillQty, head.invContractWagesBillDateil[i].BillWeight, head.invContractWagesBillDateil[i].WageRate, head.invContractWagesBillDateil[i].WagesAmount);
			}
			GrdHistoryDetail.DataSource = dtdetailHistory;
			GrdHistoryDetail.RetrieveStructure();
			HistoryDetailGridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailGridSettings()
	{
		try
		{
			GrdHistoryDetail.RootTable.Columns["SupplierName"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["packingType"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["ScaleSlipNo"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["Weight"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["PackSize"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["Quantity"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["WeightCut"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["BillQty"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].EditType = (EditType)0;
			GrdHistoryDetail.RootTable.Columns["SupplierName"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["WagesAccount"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["packingType"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["ScaleSlipNo"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["PackSize"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["WeightCut"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["BillQty"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].FilterEditType = (FilterEditType)1;
			GrdHistoryDetail.RootTable.Columns["WagesAccount"].Width = 200;
			GrdHistoryDetail.RootTable.Columns["packingType"].Width = 150;
			GrdHistoryDetail.RootTable.Columns["ScaleSlipNo"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["Weight"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["PackSize"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["Quantity"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["Rate"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["Amount"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["BillQty"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["WeightCut"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["SupplierName"].Width = 200;
			GrdHistoryDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["BillQty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["BillQty"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Amount"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["Weight"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["Quantity"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["BillWeight"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["BillQty"].FormatString = "0,0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			GrdHistoryDetail.ClearStructure();
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
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_049a: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Expected O, but got Unknown
		//IL_5d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d5d: Expected O, but got Unknown
		GridEXLayout gridPending_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdwagesDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
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
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Contractor_Wages.frmMoveOrder));
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.gridPending = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdwagesDetail = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtWeightCut = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtBillWeight = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.txtBillQty = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBoxPrint = new System.Windows.Forms.CheckBox();
		this.label11 = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.cmbmoveTo = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.cmbMoveFrom = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbPackingType = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbCrop = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbJobLot = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtqtyMain = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.cmbScaleSlip = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtPackSize = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.txtsecondweight = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.txtfirstweight = new System.Windows.Forms.TextBox();
		this.comRefenenceType = new System.Windows.Forms.ComboBox();
		this.label30 = new System.Windows.Forms.Label();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.chkisapprove = new System.Windows.Forms.CheckBox();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.comWagesAccount = new UltraCombo();
		this.comSupplier = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtwagesrate = new System.Windows.Forms.TextBox();
		this.comReferenceNo = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtwageamount = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtWeight = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.GrdHistoryDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridPending).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdwagesDetail).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbmoveTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMoveFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCrop).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbScaleSlip).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comWagesAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comReferenceNo).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1344, 727);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1344, 727);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1336, 701);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.gridPending);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 448);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1330, 250);
		this.panel10.TabIndex = 4;
		this.gridPending.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.gridPending).BackColor = System.Drawing.Color.White;
		gridPending_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridPending.DesignTimeLayout = gridPending_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridPending).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.gridPending).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridPending.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridPending).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.gridPending).Name = "gridPending";
		((ScrollableBase)this.gridPending).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridPending).Size = new System.Drawing.Size(1330, 250);
		((System.Windows.Forms.Control)(object)this.gridPending).TabIndex = 2;
		this.gridPending.TotalRow = (InheritableBoolean)1;
		this.gridPending.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridPending).VisualStyle = (VisualStyle)4;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdwagesDetail);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 313);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1330, 135);
		this.panel4.TabIndex = 3;
		this.grdwagesDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdwagesDetail).BackColor = System.Drawing.Color.White;
		grdwagesDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdwagesDetail.DesignTimeLayout = grdwagesDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdwagesDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Name = "grdwagesDetail";
		((ScrollableBase)this.grdwagesDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Size = new System.Drawing.Size(1330, 135);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).TabIndex = 1;
		this.grdwagesDetail.TotalRow = (InheritableBoolean)1;
		this.grdwagesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdwagesDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).DoubleClick += new System.EventHandler(grdwagesDetail_DoubleClick);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 65);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1330, 248);
		this.panel3.TabIndex = 2;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.txtWeightCut);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtBillWeight);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtBillQty);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.ChkBoxPrint);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbmoveTo);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMoveFrom);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingType);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCrop);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.txtqtyMain);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtNetWeight);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbScaleSlip);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtPackSize);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtsecondweight);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.txtfirstweight);
		this.groupBox1.Controls.Add(this.comRefenenceType);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.btnCancelDetail);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.chkisapprove);
		this.groupBox1.Controls.Add(this.txtdocnumber);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comWagesAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comSupplier);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtqty);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtwagesrate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comReferenceNo);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtwageamount);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtWeight);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Location = new System.Drawing.Point(7, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1320, 237);
		this.groupBox1.TabIndex = 3;
		this.groupBox1.TabStop = false;
		this.txtWeightCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCut.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtWeightCut.Location = new System.Drawing.Point(971, 139);
		this.txtWeightCut.Name = "txtWeightCut";
		this.txtWeightCut.Size = new System.Drawing.Size(73, 25);
		this.txtWeightCut.TabIndex = 18;
		this.txtWeightCut.Text = "0";
		this.txtWeightCut.TextChanged += new System.EventHandler(txtWeightCut_TextChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label22.Location = new System.Drawing.Point(975, 112);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(47, 17);
		this.label22.TabIndex = 109;
		this.label22.Text = "Wt Cut";
		this.txtBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillWeight.Enabled = false;
		this.txtBillWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtBillWeight.Location = new System.Drawing.Point(1223, 44);
		this.txtBillWeight.Name = "txtBillWeight";
		this.txtBillWeight.Size = new System.Drawing.Size(91, 25);
		this.txtBillWeight.TabIndex = 106;
		this.txtBillWeight.TextChanged += new System.EventHandler(txtBillWeight_TextChanged);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label24.Location = new System.Drawing.Point(1137, 51);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(68, 17);
		this.label24.TabIndex = 107;
		this.label24.Text = "Bill Weight";
		this.txtBillQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillQty.Enabled = false;
		this.txtBillQty.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtBillQty.Location = new System.Drawing.Point(1223, 14);
		this.txtBillQty.Name = "txtBillQty";
		this.txtBillQty.Size = new System.Drawing.Size(91, 25);
		this.txtBillQty.TabIndex = 104;
		this.txtBillQty.TextChanged += new System.EventHandler(txtBillQty_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label23.Location = new System.Drawing.Point(1137, 21);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(76, 17);
		this.label23.TabIndex = 105;
		this.label23.Text = "Bill Quantity";
		this.ChkBoxPrint.AutoSize = true;
		this.ChkBoxPrint.Location = new System.Drawing.Point(1201, 84);
		this.ChkBoxPrint.Name = "ChkBoxPrint";
		this.ChkBoxPrint.Size = new System.Drawing.Size(88, 17);
		this.ChkBoxPrint.TabIndex = 10;
		this.ChkBoxPrint.Text = "Print Preivew";
		this.ChkBoxPrint.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Times New Roman", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(12, 152);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(50, 19);
		this.label11.TabIndex = 97;
		this.label11.Text = "Detail";
		this.btnAdd.BackColor = System.Drawing.Color.White;
		this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.Location = new System.Drawing.Point(910, 200);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(26, 22);
		this.btnAdd.TabIndex = 26;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.cmbmoveTo.AllowNull = (DefaultableBoolean)1;
		this.cmbmoveTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbmoveTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbmoveTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbmoveTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.cmbmoveTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbmoveTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbmoveTo).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbmoveTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbmoveTo).Location = new System.Drawing.Point(712, 198);
		((System.Windows.Forms.Control)(object)this.cmbmoveTo).Name = "cmbmoveTo";
		((System.Windows.Forms.Control)(object)this.cmbmoveTo).Size = new System.Drawing.Size(193, 27);
		((System.Windows.Forms.Control)(object)this.cmbmoveTo).TabIndex = 25;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label21.Location = new System.Drawing.Point(711, 173);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(59, 17);
		this.label21.TabIndex = 96;
		this.label21.Text = "Move To";
		this.cmbMoveFrom.AllowNull = (DefaultableBoolean)1;
		this.cmbMoveFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMoveFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMoveFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMoveFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbMoveFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMoveFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMoveFrom).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbMoveFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbMoveFrom).Location = new System.Drawing.Point(530, 198);
		((System.Windows.Forms.Control)(object)this.cmbMoveFrom).Name = "cmbMoveFrom";
		((System.Windows.Forms.Control)(object)this.cmbMoveFrom).Size = new System.Drawing.Size(176, 27);
		((System.Windows.Forms.Control)(object)this.cmbMoveFrom).TabIndex = 24;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label20.Location = new System.Drawing.Point(529, 173);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(75, 17);
		this.label20.TabIndex = 94;
		this.label20.Text = "Move From";
		this.cmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Location = new System.Drawing.Point(603, 138);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Name = "cmbPackingType";
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Size = new System.Drawing.Size(166, 27);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TabIndex = 14;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label18.Location = new System.Drawing.Point(602, 112);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(83, 17);
		this.label18.TabIndex = 92;
		this.label18.Text = "Packing Type";
		this.cmbCrop.AllowNull = (DefaultableBoolean)1;
		this.cmbCrop.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCrop.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCrop.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCrop).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCrop).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCrop).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCrop).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbCrop).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCrop).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbCrop).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCrop).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCrop).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbCrop).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCrop).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCrop).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCrop).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbCrop.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCrop).Location = new System.Drawing.Point(408, 198);
		((System.Windows.Forms.Control)(object)this.cmbCrop).Name = "cmbCrop";
		((System.Windows.Forms.Control)(object)this.cmbCrop).Size = new System.Drawing.Size(117, 27);
		((System.Windows.Forms.Control)(object)this.cmbCrop).TabIndex = 23;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label16.Location = new System.Drawing.Point(407, 173);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(37, 17);
		this.label16.TabIndex = 90;
		this.label16.Text = "Crop";
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(291, 198);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(113, 27);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 22;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label14.Location = new System.Drawing.Point(290, 173);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(47, 17);
		this.label14.TabIndex = 88;
		this.label14.Text = "JobLot";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(112, 198);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(176, 27);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 21;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label13.Location = new System.Drawing.Point(109, 173);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(33, 17);
		this.label13.TabIndex = 86;
		this.label13.Text = "Item";
		this.txtqtyMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqtyMain.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtqtyMain.Location = new System.Drawing.Point(911, 49);
		this.txtqtyMain.Name = "txtqtyMain";
		this.txtqtyMain.ReadOnly = true;
		this.txtqtyMain.Size = new System.Drawing.Size(181, 25);
		this.txtqtyMain.TabIndex = 7;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label17.Location = new System.Drawing.Point(830, 53);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(56, 17);
		this.label17.TabIndex = 84;
		this.label17.Text = "Quantity";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label29.Location = new System.Drawing.Point(557, 53);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(73, 17);
		this.label29.TabIndex = 81;
		this.label29.Text = "Net Weight";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(648, 48);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(176, 25);
		this.txtNetWeight.TabIndex = 6;
		this.cmbScaleSlip.AllowNull = (DefaultableBoolean)1;
		this.cmbScaleSlip.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbScaleSlip.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbScaleSlip.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbScaleSlip).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbScaleSlip).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbScaleSlip).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbScaleSlip).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbScaleSlip.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbScaleSlip).Location = new System.Drawing.Point(114, 138);
		((System.Windows.Forms.Control)(object)this.cmbScaleSlip).Name = "cmbScaleSlip";
		((System.Windows.Forms.Control)(object)this.cmbScaleSlip).Size = new System.Drawing.Size(124, 27);
		((System.Windows.Forms.Control)(object)this.cmbScaleSlip).TabIndex = 11;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label19.Location = new System.Drawing.Point(111, 112);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(71, 17);
		this.label19.TabIndex = 78;
		this.label19.Text = "Scale Slip#";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label35.Location = new System.Drawing.Point(845, 112);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(61, 17);
		this.label35.TabIndex = 74;
		this.label35.Text = "Pack Size";
		this.txtPackSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackSize.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtPackSize.Location = new System.Drawing.Point(848, 139);
		this.txtPackSize.Name = "txtPackSize";
		this.txtPackSize.Size = new System.Drawing.Size(58, 25);
		this.txtPackSize.TabIndex = 16;
		this.txtPackSize.TextChanged += new System.EventHandler(txtPackSize_TextChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label33.Location = new System.Drawing.Point(295, 53);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(74, 17);
		this.label33.TabIndex = 71;
		this.label33.Text = "2nd Weight";
		this.txtsecondweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsecondweight.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtsecondweight.Location = new System.Drawing.Point(396, 49);
		this.txtsecondweight.Name = "txtsecondweight";
		this.txtsecondweight.ReadOnly = true;
		this.txtsecondweight.Size = new System.Drawing.Size(154, 25);
		this.txtsecondweight.TabIndex = 5;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label31.Location = new System.Drawing.Point(13, 54);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 17);
		this.label31.TabIndex = 68;
		this.label31.Text = "1st Weight";
		this.txtfirstweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfirstweight.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtfirstweight.Location = new System.Drawing.Point(114, 49);
		this.txtfirstweight.Name = "txtfirstweight";
		this.txtfirstweight.ReadOnly = true;
		this.txtfirstweight.Size = new System.Drawing.Size(172, 25);
		this.txtfirstweight.TabIndex = 4;
		this.comRefenenceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comRefenenceType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comRefenenceType.FormattingEnabled = true;
		this.comRefenenceType.Items.AddRange(new object[1] { "Stock Move" });
		this.comRefenenceType.Location = new System.Drawing.Point(648, 18);
		this.comRefenenceType.Name = "comRefenenceType";
		this.comRefenenceType.Size = new System.Drawing.Size(176, 25);
		this.comRefenenceType.TabIndex = 2;
		this.comRefenenceType.Leave += new System.EventHandler(comRefenenceType_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label30.Location = new System.Drawing.Point(1116, 82);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(79, 17);
		this.label30.TabIndex = 9;
		this.label30.Text = "Is Approved";
		this.btnCancelDetail.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.btnCancelDetail.Location = new System.Drawing.Point(1017, 206);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(75, 26);
		this.btnCancelDetail.TabIndex = 28;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = true;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.btnUpdateDetail.Location = new System.Drawing.Point(1017, 177);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(75, 27);
		this.btnUpdateDetail.TabIndex = 27;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = true;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.chkisapprove.AutoSize = true;
		this.chkisapprove.Location = new System.Drawing.Point(1095, 85);
		this.chkisapprove.Name = "chkisapprove";
		this.chkisapprove.Size = new System.Drawing.Size(15, 14);
		this.chkisapprove.TabIndex = 6;
		this.chkisapprove.UseVisualStyleBackColor = true;
		this.txtdocnumber.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocnumber.Location = new System.Drawing.Point(114, 18);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.Size = new System.Drawing.Size(175, 25);
		this.txtdocnumber.TabIndex = 0;
		this.txtdocnumber.Leave += new System.EventHandler(txtdocnumber_TextChanged);
		this.comWagesAccount.AllowNull = (DefaultableBoolean)1;
		this.comWagesAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWagesAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWagesAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWagesAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.comWagesAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comWagesAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comWagesAccount).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comWagesAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comWagesAccount).Location = new System.Drawing.Point(421, 138);
		((System.Windows.Forms.Control)(object)this.comWagesAccount).Name = "comWagesAccount";
		((System.Windows.Forms.Control)(object)this.comWagesAccount).Size = new System.Drawing.Size(176, 27);
		((System.Windows.Forms.Control)(object)this.comWagesAccount).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.comWagesAccount).TextChanged += new System.EventHandler(comWagesAccount_TextChanged);
		this.comSupplier.AllowNull = (DefaultableBoolean)1;
		this.comSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.comSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.comSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.comSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comSupplier).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comSupplier).Location = new System.Drawing.Point(243, 138);
		((System.Windows.Forms.Control)(object)this.comSupplier).Name = "comSupplier";
		((System.Windows.Forms.Control)(object)this.comSupplier).Size = new System.Drawing.Size(174, 27);
		((System.Windows.Forms.Control)(object)this.comSupplier).TabIndex = 12;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label7.Location = new System.Drawing.Point(418, 112);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(97, 17);
		this.label7.TabIndex = 27;
		this.label7.Text = "Wages Account";
		this.txtDocdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocdate.Location = new System.Drawing.Point(396, 18);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(154, 25);
		this.txtDocdate.TabIndex = 1;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(114, 80);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(978, 25);
		this.txtRemarks.TabIndex = 8;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.Location = new System.Drawing.Point(241, 112);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(70, 17);
		this.label5.TabIndex = 55;
		this.label5.Text = "Contractor";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(912, 139);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(53, 25);
		this.txtqty.TabIndex = 17;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(13, 86);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(95, 17);
		this.label6.TabIndex = 59;
		this.label6.Text = "Other Remarks";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.Location = new System.Drawing.Point(13, 26);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(89, 17);
		this.label1.TabIndex = 24;
		this.label1.Text = "Document No";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label8.Location = new System.Drawing.Point(912, 112);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(56, 17);
		this.label8.TabIndex = 35;
		this.label8.Text = "Quantity";
		this.txtwagesrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwagesrate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtwagesrate.Location = new System.Drawing.Point(1052, 139);
		this.txtwagesrate.Name = "txtwagesrate";
		this.txtwagesrate.ReadOnly = true;
		this.txtwagesrate.Size = new System.Drawing.Size(73, 25);
		this.txtwagesrate.TabIndex = 19;
		this.comReferenceNo.AllowNull = (DefaultableBoolean)1;
		this.comReferenceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.comReferenceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comReferenceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comReferenceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.comReferenceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comReferenceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comReferenceNo).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comReferenceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comReferenceNo).Location = new System.Drawing.Point(911, 17);
		((System.Windows.Forms.Control)(object)this.comReferenceNo).Name = "comReferenceNo";
		((System.Windows.Forms.Control)(object)this.comReferenceNo).Size = new System.Drawing.Size(181, 27);
		((System.Windows.Forms.Control)(object)this.comReferenceNo).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.comReferenceNo).Leave += new System.EventHandler(comReferenceNo_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label3.Location = new System.Drawing.Point(557, 22);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(85, 17);
		this.label3.TabIndex = 40;
		this.label3.Text = "Ref Doc Type";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label9.Location = new System.Drawing.Point(1055, 112);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(77, 17);
		this.label9.TabIndex = 47;
		this.label9.Text = "Wages Rate";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label12.Location = new System.Drawing.Point(770, 112);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(48, 17);
		this.label12.TabIndex = 41;
		this.label12.Text = "Weight";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label4.Location = new System.Drawing.Point(829, 22);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(76, 17);
		this.label4.TabIndex = 48;
		this.label4.Text = "Ref Doc No";
		this.txtwageamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwageamount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtwageamount.Location = new System.Drawing.Point(1131, 139);
		this.txtwageamount.Name = "txtwageamount";
		this.txtwageamount.ReadOnly = true;
		this.txtwageamount.Size = new System.Drawing.Size(93, 25);
		this.txtwageamount.TabIndex = 20;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label2.Location = new System.Drawing.Point(295, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(98, 17);
		this.label2.TabIndex = 33;
		this.label2.Text = "Document Date";
		this.txtWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtWeight.Location = new System.Drawing.Point(773, 139);
		this.txtWeight.Name = "txtWeight";
		this.txtWeight.Size = new System.Drawing.Size(68, 25);
		this.txtWeight.TabIndex = 15;
		this.txtWeight.TextChanged += new System.EventHandler(txtWeight_TextChanged);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label10.Location = new System.Drawing.Point(1138, 112);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(57, 17);
		this.label10.TabIndex = 54;
		this.label10.Text = " Amount";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 31);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1330, 34);
		this.panel6.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(13, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(290, 24);
		this.label15.TabIndex = 0;
		this.label15.Text = "Stock Move Order Labour Wages";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1330, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.Print });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1330, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
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
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(56, 24);
		this.Print.Text = "&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1336, 701);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Controls.Add(this.panel9);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1330, 695);
		this.panel5.TabIndex = 0;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 306);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1330, 389);
		this.panel11.TabIndex = 3;
		this.GrdHistoryDetail.AllowDelete = (InheritableBoolean)1;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1330, 389);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 62;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 64);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1330, 242);
		this.panel9.TabIndex = 2;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1330, 242);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 61;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel8.Controls.Add(this.label26);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 27);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1330, 37);
		this.panel8.TabIndex = 1;
		this.label26.AutoSize = true;
		this.label26.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label26.Location = new System.Drawing.Point(5, 6);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(224, 24);
		this.label26.TabIndex = 1;
		this.label26.Text = "Stock Move Order History";
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1330, 27);
		this.panel7.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripButton1, this.toolStripSeparator1, this.toolStripButton4 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1330, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton4.Text = "&Print";
		this.toolStripButton4.Click += new System.EventHandler(toolStripButton4_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1344, 727);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmMoveOrder";
		this.Text = "frmMoveOrder";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmMoveOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmMoveOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridPending).EndInit();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdwagesDetail).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbmoveTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMoveFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCrop).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbScaleSlip).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comWagesAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comReferenceNo).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
