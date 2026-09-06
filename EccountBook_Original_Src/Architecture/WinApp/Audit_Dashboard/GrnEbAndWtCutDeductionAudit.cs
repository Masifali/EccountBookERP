using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Audit_Dashboard;

public class GrnEbAndWtCutDeductionAudit : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtPackingWise = new DataTable();

	private DataTable dtParentAndPackWise = new DataTable();

	private DataTable dtParentCityPurTypeAndPackingWise = new DataTable();

	private DataTable dtpackwise = new DataTable();

	private DataTable dtParentCatAndPackWise = new DataTable();

	private DataTable dtParentPurAndPAckWise = new DataTable();

	private DataTable dtParentPurCityAndPackWise = new DataTable();

	private DataTable dtParentPurCitySupplierAndPackWise = new DataTable();

	private DataTable dtParentPurCitySuppQtyWtAndPackWise = new DataTable();

	private IContainer components;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Button btnShow;

	internal UltraCombo cmbPackType;

	private Label label10;

	private DateTimePicker txtToDate;

	private Label label1;

	private DateTimePicker txtDateFrom;

	internal UltraCombo cmbSupplier;

	private Label label3;

	private CtrlGrdBar ctrlGrdDetaSummByDocType;

	internal GridEX grdParentCityPurTypeAndPackingWiseW;

	private CtrlGrdBar ctrlGrdDetailByContractor;

	private CtrlGrdBar ctrlGrdDetailByDocType;

	private Panel panel3;

	private GroupBox groupBox1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel7;

	private Panel panel25;

	internal GridEX grdParentPurTypeAndPackingWiseW;

	private Panel panel5;

	internal GridEX grdParentAndPackingWiseW;

	private Panel panel19;

	private TabPage tabPage2;

	private Panel panel1;

	internal GridEX grdParentPurTypeAndPackingWiseWO;

	private Panel panel4;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label6;

	private Panel panel8;

	internal GridEX grdPackingWiseWO;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label11;

	private Panel panel10;

	private Panel panel11;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label12;

	private TextBox txtEbUnitTo;

	private TextBox txtEbUnitfrom;

	private Label label5;

	private Label label4;

	private Panel panel13;

	internal GridEX grdParentAndPackingWiseWO;

	internal GridEX grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX grdParentCityPurTypeSupplierAndPackingWiseWO;

	private CtrlGrdBar ctrlGrdBar5;

	private Panel panel21;

	private CtrlGrdBar ctrlGrdBar6;

	private Label label9;

	private Label label13;

	private Label label15;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private Label label16;

	private Label label2;

	internal UltraCombo cmbDateType;

	private Panel panel12;

	private Label label14;

	private Panel panel16;

	private Label label7;

	internal GridEX grdParentCityPurTypeAndPackingWiseWO;

	private Panel panel18;

	private Label label8;

	public GrnEbAndWtCutDeductionAudit(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmEvaulationDetailSalesReports_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1300, 770);
			dtPackingWise.Columns.Add("ParentCategory", typeof(string));
			dtPackingWise.Columns.Add("PackingTypeId", typeof(int));
			dtPackingWise.Columns.Add("PackType", typeof(string));
			dtPackingWise.Columns.Add("Qty", typeof(double));
			dtPackingWise.Columns.Add("MinPerBag", typeof(double));
			dtPackingWise.Columns.Add("MaxPerBag", typeof(double));
			dtPackingWise.Columns.Add("CountEbWt", typeof(double));
			dtPackingWise.Columns.Add("TotalCount", typeof(double));
			dtParentAndPackWise.Columns.Add("ParentCategory");
			dtParentAndPackWise.Columns.Add("PackingTypeId");
			dtParentAndPackWise.Columns.Add("PackType", typeof(string));
			dtParentAndPackWise.Columns.Add("PurchaseType", typeof(string));
			dtParentAndPackWise.Columns.Add("Qty", typeof(double));
			dtParentAndPackWise.Columns.Add("MinPerBag", typeof(double));
			dtParentAndPackWise.Columns.Add("MaxPerBag", typeof(double));
			dtParentAndPackWise.Columns.Add("CountEbWt", typeof(double));
			dtParentAndPackWise.Columns.Add("TotalCount", typeof(double));
			dtParentCityPurTypeAndPackingWise.Columns.Add("ParentCategory");
			dtParentCityPurTypeAndPackingWise.Columns.Add("PackingTypeId");
			dtParentCityPurTypeAndPackingWise.Columns.Add("PackType", typeof(string));
			dtParentCityPurTypeAndPackingWise.Columns.Add("PurchaseType", typeof(string));
			dtParentCityPurTypeAndPackingWise.Columns.Add("SupplierName", typeof(string));
			dtParentCityPurTypeAndPackingWise.Columns.Add("Qty", typeof(double));
			dtParentCityPurTypeAndPackingWise.Columns.Add("MinPerBag", typeof(double));
			dtParentCityPurTypeAndPackingWise.Columns.Add("MaxPerBag", typeof(double));
			dtParentCityPurTypeAndPackingWise.Columns.Add("CountEbWt", typeof(double));
			dtParentCityPurTypeAndPackingWise.Columns.Add("TotalCount", typeof(double));
			dtpackwise.Columns.Add("ParentCategory");
			dtpackwise.Columns.Add("PackingTypeId");
			dtpackwise.Columns.Add("PackType");
			dtpackwise.Columns.Add("Eb0Wt", typeof(double));
			dtpackwise.Columns.Add("Eb0Qty", typeof(double));
			dtParentCatAndPackWise.Columns.Add("ParentCategory");
			dtParentCatAndPackWise.Columns.Add("PackingTypeId");
			dtParentCatAndPackWise.Columns.Add("PackType");
			dtParentCatAndPackWise.Columns.Add("PurchaseType");
			dtParentCatAndPackWise.Columns.Add("Eb0Wt", typeof(double));
			dtParentCatAndPackWise.Columns.Add("Eb0Qty", typeof(double));
			dtParentPurAndPAckWise.Columns.Add("ParentCategory");
			dtParentPurAndPAckWise.Columns.Add("PackingTypeId");
			dtParentPurAndPAckWise.Columns.Add("PackType");
			dtParentPurAndPAckWise.Columns.Add("PurchaseType");
			dtParentPurAndPAckWise.Columns.Add("CityName", typeof(double));
			dtParentPurAndPAckWise.Columns.Add("Eb0Wt", typeof(double));
			dtParentPurAndPAckWise.Columns.Add("Eb0Qty", typeof(double));
			dtParentPurCityAndPackWise.Columns.Add("ParentCategory");
			dtParentPurCityAndPackWise.Columns.Add("PackingTypeId");
			dtParentPurCityAndPackWise.Columns.Add("PackType");
			dtParentPurCityAndPackWise.Columns.Add("PurchaseType");
			dtParentPurCityAndPackWise.Columns.Add("CityName");
			dtParentPurCityAndPackWise.Columns.Add("SupplierName", typeof(string));
			dtParentPurCityAndPackWise.Columns.Add("Eb0Wt", typeof(double));
			dtParentPurCityAndPackWise.Columns.Add("Eb0Qty", typeof(double));
			dtParentPurCitySupplierAndPackWise.Columns.Add("PackingTypeId");
			dtParentPurCitySupplierAndPackWise.Columns.Add("PackType");
			dtParentPurCitySupplierAndPackWise.Columns.Add("MinPerBag");
			dtParentPurCitySupplierAndPackWise.Columns.Add("MaxPerBag");
			dtParentPurCitySupplierAndPackWise.Columns.Add("Count0EbWt", typeof(double));
			dtParentPurCitySuppQtyWtAndPackWise.Columns.Add("ParentCategory");
			dtParentPurCitySuppQtyWtAndPackWise.Columns.Add("PackingTypeId");
			dtParentPurCitySuppQtyWtAndPackWise.Columns.Add("PackType");
			dtParentPurCitySuppQtyWtAndPackWise.Columns.Add("CityName");
			dtParentPurCitySuppQtyWtAndPackWise.Columns.Add("SupplierName", typeof(string));
			dtParentPurCitySuppQtyWtAndPackWise.Columns.Add("Qty", typeof(double));
			dtParentPurCitySuppQtyWtAndPackWise.Columns.Add("BillWeight", typeof(double));
			dtParentPurCitySuppQtyWtAndPackWise.Columns.Add("EbPerBag", typeof(double));
			dtParentPurCitySuppQtyWtAndPackWise.Columns.Add("WtDeduc", typeof(double));
			cmbDateType.Focus();
			AllDropDownBind();
			Datetypefill();
			GridFill1();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateType, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateType).Rows[2].Activate();
			((UltraGridBase)cmbDateType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllDropDownBind()
	{
		try
		{
			DataTable dtAll = InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null, (string)null, (string)null, 0);
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "Supplier"
					select r;
				DataTable dtParty = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("Activity") == "PackingType"
					select r;
				DataTable dtItem = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDLNew(dtParty, cmbSupplier, "Id", "ReferenceName", "Supplier Name", true);
				((UltraGridBase)cmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtItem, cmbPackType, "Id", "ReferenceName", "Packing Type", true);
				((UltraGridBase)cmbPackType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill1()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		try
		{
			DataSet DtSet = InvGrn.GRNEmptyBagsWeightCutDeduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Conversion.ToDateTime((object)txtDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value),
				PackingTypeId = Conversion.ToInt(cmbPackType.Value),
				EbPUnitF = Conversion.ToDecimal((object)txtEbUnitfrom.Text),
				EbPUnitT = Conversion.ToDecimal((object)txtEbUnitTo.Text)
			});
			if (DtSet != null && DtSet.Tables.Count > 0)
			{
				DataTable dt1 = DtSet.Tables[0];
				DataTable dt2 = DtSet.Tables[1];
				DataTable dt3 = DtSet.Tables[2];
				dtPackingWise.Rows.Clear();
				for (int k = 0; k < dt1.Rows.Count; k++)
				{
					dtPackingWise.Rows.Add(dt1.Rows[k]["InvParentCateCode"], dt1.Rows[k]["PackingTypeId"], dt1.Rows[k]["PackTypeDesc"], dt1.Rows[k]["Qty"], dt1.Rows[k]["MinPerBag"], dt1.Rows[k]["MaxPerBag"], dt1.Rows[k]["CountEbWt"], dt1.Rows[k]["TotalCount"]);
				}
				dtParentAndPackWise.Rows.Clear();
				for (int j = 0; j < dt2.Rows.Count; j++)
				{
					dtParentAndPackWise.Rows.Add(dt2.Rows[j]["InvParentCateCode"], dt2.Rows[j]["PackingTypeId"], dt2.Rows[j]["PackTypeDesc"], dt2.Rows[j]["PurchaseType"], dt2.Rows[j]["Qty"], dt2.Rows[j]["MinPerBag"], dt2.Rows[j]["MaxPerBag"], dt2.Rows[j]["CountEbWt"], dt2.Rows[j]["TotalCount"]);
				}
				dtParentCityPurTypeAndPackingWise.Rows.Clear();
				for (int i = 0; i < dt3.Rows.Count; i++)
				{
					dtParentCityPurTypeAndPackingWise.Rows.Add(dt3.Rows[i]["InvParentCateCode"], dt3.Rows[i]["PackingTypeId"], dt3.Rows[i]["PackTypeDesc"], dt3.Rows[i]["PurchaseType"], dt3.Rows[i]["SupplierName"], dt3.Rows[i]["Qty"], dt3.Rows[i]["MinPerBag"], dt3.Rows[i]["MaxPerBag"], dt3.Rows[i]["CountEbWt"], dt3.Rows[i]["TotalCount"]);
				}
				if (dtParentAndPackWise.Rows.Count > 0)
				{
					grdParentPurTypeAndPackingWiseW.DataSource = dtParentAndPackWise;
					grdParentPurTypeAndPackingWiseW.RetrieveStructure();
					grdParentPurTypeAndPackingWiseSetting();
				}
				else
				{
					grdParentPurTypeAndPackingWiseW.DataSource = null;
				}
				if (dtPackingWise.Rows.Count > 0)
				{
					grdParentAndPackingWiseW.DataSource = dtPackingWise;
					grdParentAndPackingWiseW.RetrieveStructure();
					grdParentAndPackingWiseSetting();
				}
				else
				{
					grdParentAndPackingWiseW.DataSource = null;
				}
				if (dtParentCityPurTypeAndPackingWise.Rows.Count > 0)
				{
					grdParentCityPurTypeAndPackingWiseW.DataSource = dtParentCityPurTypeAndPackingWise;
					grdParentCityPurTypeAndPackingWiseW.RetrieveStructure();
					GridSummaryByWagesAcSetting();
				}
				else
				{
					grdParentCityPurTypeAndPackingWiseW.DataSource = null;
				}
			}
			else
			{
				grdParentCityPurTypeAndPackingWiseW.DataSource = null;
				grdParentPurTypeAndPackingWiseW.DataSource = null;
				grdParentAndPackingWiseW.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill2()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		try
		{
			DataSet DtSet = InvGrn.GRNTransactionsWithoutEmptyBagsDeductions_Audit(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Conversion.ToDateTime((object)txtDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value),
				PackingTypeId = Conversion.ToInt(cmbPackType.Value),
				EbPUnitF = Conversion.ToDecimal((object)txtEbUnitfrom.Text),
				EbPUnitT = Conversion.ToDecimal((object)txtEbUnitTo.Text)
			});
			if (DtSet.Tables[0].Rows.Count > 0)
			{
				DataTable dt1 = DtSet.Tables[0];
				DataTable dt2 = DtSet.Tables[1];
				DataTable dt3 = DtSet.Tables[2];
				DataTable dt4 = DtSet.Tables[3];
				DataTable dt5 = DtSet.Tables[4];
				DataTable dt6 = DtSet.Tables[5];
				dtpackwise.Rows.Clear();
				for (int n = 0; n < dt1.Rows.Count; n++)
				{
					dtpackwise.Rows.Add(dt1.Rows[n]["InvParentCateCode"], dt1.Rows[n]["PackingTypeId"], dt1.Rows[n]["PackTypeDesc"], dt1.Rows[n]["Eb0Wt"], dt1.Rows[n]["Eb0Qty"]);
				}
				dtParentCatAndPackWise.Rows.Clear();
				for (int m = 0; m < dt2.Rows.Count; m++)
				{
					dtParentCatAndPackWise.Rows.Add(dt2.Rows[m]["InvParentCateCode"], dt2.Rows[m]["PackingTypeId"], dt2.Rows[m]["PackTypeDesc"], dt2.Rows[m]["PurchaseType"], dt2.Rows[m]["Eb0Wt"], dt2.Rows[m]["Eb0Qty"]);
				}
				dtParentPurAndPAckWise.Rows.Clear();
				for (int l = 0; l < dt3.Rows.Count; l++)
				{
					dtParentPurAndPAckWise.Rows.Add(dt3.Rows[l]["InvParentCateCode"], dt3.Rows[l]["PackingTypeId"], dt3.Rows[l]["PackTypeDesc"], dt3.Rows[l]["PurchaseType"], dt3.Rows[l]["CityName"], dt3.Rows[l]["Eb0Wt"], dt3.Rows[l]["Eb0Qty"]);
				}
				dtParentPurCityAndPackWise.Rows.Clear();
				for (int k = 0; k < dt4.Rows.Count; k++)
				{
					dtParentPurCityAndPackWise.Rows.Add(dt4.Rows[k]["InvParentCateCode"], dt4.Rows[k]["PackingTypeId"], dt4.Rows[k]["PackTypeDesc"], dt4.Rows[k]["PurchaseType"], dt4.Rows[k]["CityName"], dt4.Rows[k]["SupplierName"], dt4.Rows[k]["Eb0Wt"], dt4.Rows[k]["Eb0Qty"]);
				}
				dtParentPurCitySupplierAndPackWise.Rows.Clear();
				for (int j = 0; j < dt5.Rows.Count; j++)
				{
					dtParentPurCitySupplierAndPackWise.Rows.Add(dt5.Rows[j]["PackingTypeId"], dt5.Rows[j]["PackTypeDesc"], dt5.Rows[j]["MinPerBag"], dt5.Rows[j]["MaxPerBag"], dt5.Rows[j]["Count0EbWt"]);
				}
				dtParentPurCitySuppQtyWtAndPackWise.Rows.Clear();
				for (int i = 0; i < dt6.Rows.Count; i++)
				{
					dtParentPurCitySuppQtyWtAndPackWise.Rows.Add(dt6.Rows[i]["ItemParentCat"], dt6.Rows[i]["PackingTypeId"], dt6.Rows[i]["PackTypeDesc"], dt6.Rows[i]["CityName"], dt6.Rows[i]["SupplierName"], dt6.Rows[i]["Qty"], dt6.Rows[i]["BillWeight"], dt6.Rows[i]["EbPerBag"], dt6.Rows[i]["WeightDed"]);
				}
				if (dtpackwise.Rows.Count > 0)
				{
					grdPackingWiseWO.DataSource = dtpackwise;
					grdPackingWiseWO.RetrieveStructure();
					grdPackingWiseWOSetting();
				}
				else
				{
					grdPackingWiseWO.DataSource = null;
				}
				if (dtParentCatAndPackWise.Rows.Count > 0)
				{
					grdParentAndPackingWiseWO.DataSource = dtParentCatAndPackWise;
					grdParentAndPackingWiseWO.RetrieveStructure();
					grdParentAndPackingWiseWOSetting();
				}
				else
				{
					grdParentAndPackingWiseWO.DataSource = null;
				}
				if (dtParentPurAndPAckWise.Rows.Count > 0)
				{
					grdParentPurTypeAndPackingWiseWO.DataSource = dtParentPurAndPAckWise;
					grdParentPurTypeAndPackingWiseWO.RetrieveStructure();
					grdParentPurTypeAndPackingWiseWOSetting();
				}
				else
				{
					grdParentPurTypeAndPackingWiseWO.DataSource = null;
				}
				if (dtParentPurCityAndPackWise.Rows.Count > 0)
				{
					grdParentCityPurTypeAndPackingWiseWO.DataSource = dtParentPurCityAndPackWise;
					grdParentCityPurTypeAndPackingWiseWO.RetrieveStructure();
					grdParentCityPurTypeAndPackingWiseWOSetting();
				}
				else
				{
					grdParentCityPurTypeAndPackingWiseWO.DataSource = null;
				}
				if (dtParentPurCitySupplierAndPackWise.Rows.Count > 0)
				{
					grdParentCityPurTypeSupplierAndPackingWiseWO.DataSource = dtParentPurCitySupplierAndPackWise;
					grdParentCityPurTypeSupplierAndPackingWiseWO.RetrieveStructure();
					grdParentCityPurTypeSupplierAndPackingWiseWOSetting();
				}
				else
				{
					grdParentCityPurTypeSupplierAndPackingWiseWO.DataSource = null;
				}
				if (dtParentPurCitySuppQtyWtAndPackWise.Rows.Count > 0)
				{
					grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.DataSource = dtParentPurCitySuppQtyWtAndPackWise;
					grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RetrieveStructure();
					grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWOSetting();
				}
				else
				{
					grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.DataSource = null;
				}
			}
			else
			{
				grdPackingWiseWO.DataSource = null;
				grdParentAndPackingWiseWO.DataSource = null;
				grdParentPurTypeAndPackingWiseWO.DataSource = null;
				grdParentCityPurTypeAndPackingWiseWO.DataSource = null;
				grdParentCityPurTypeSupplierAndPackingWiseWO.DataSource = null;
				grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdParentAndPackingWiseSetting()
	{
		try
		{
			grdParentAndPackingWiseW.RootTable.Columns["ParentCategory"].Caption = "Par Category";
			grdParentAndPackingWiseW.RootTable.Columns["MinPerBag"].Caption = "Min/Bag";
			grdParentAndPackingWiseW.RootTable.Columns["MaxPerBag"].Caption = "Max/Bag";
			grdParentAndPackingWiseW.RootTable.Columns["CountEbWt"].Caption = "CountEbWt";
			grdParentAndPackingWiseW.RootTable.Columns["TotalCount"].Caption = "T.Count";
			grdParentAndPackingWiseW.RootTable.Columns["PackingTypeId"].Visible = false;
			grdParentAndPackingWiseW.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdParentAndPackingWiseW.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdParentAndPackingWiseW.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdParentAndPackingWiseW.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdParentAndPackingWiseW.RootTable.Columns["MinPerBag"].AggregateFunction = (AggregateFunction)2;
			grdParentAndPackingWiseW.RootTable.Columns["MinPerBag"].TextAlignment = (TextAlignment)3;
			grdParentAndPackingWiseW.RootTable.Columns["MinPerBag"].FormatString = "0,0";
			grdParentAndPackingWiseW.RootTable.Columns["MinPerBag"].TotalFormatString = "#,##0.##";
			grdParentAndPackingWiseW.RootTable.Columns["MaxPerBag"].AggregateFunction = (AggregateFunction)2;
			grdParentAndPackingWiseW.RootTable.Columns["MaxPerBag"].TextAlignment = (TextAlignment)3;
			grdParentAndPackingWiseW.RootTable.Columns["MaxPerBag"].FormatString = "#,##0.###";
			grdParentAndPackingWiseW.RootTable.Columns["MaxPerBag"].TotalFormatString = "#,##0.##";
			grdParentAndPackingWiseW.RootTable.Columns["CountEbWt"].AggregateFunction = (AggregateFunction)2;
			grdParentAndPackingWiseW.RootTable.Columns["CountEbWt"].TextAlignment = (TextAlignment)3;
			grdParentAndPackingWiseW.RootTable.Columns["CountEbWt"].FormatString = "#,##0.###";
			grdParentAndPackingWiseW.RootTable.Columns["CountEbWt"].TotalFormatString = "#,##0.##";
			grdParentAndPackingWiseW.RootTable.Columns["TotalCount"].AggregateFunction = (AggregateFunction)2;
			grdParentAndPackingWiseW.RootTable.Columns["TotalCount"].TextAlignment = (TextAlignment)3;
			grdParentAndPackingWiseW.RootTable.Columns["TotalCount"].FormatString = "#,##0.###";
			grdParentAndPackingWiseW.RootTable.Columns["TotalCount"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdParentAndPackingWiseW);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdParentPurTypeAndPackingWiseSetting()
	{
		try
		{
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["ParentCategory"].Caption = "Par Category";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].Caption = "Min/Bag";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].Caption = "Max/Bag";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].Caption = "CountEbWt";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].Caption = "T.Count";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["PackingTypeId"].Visible = false;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].AggregateFunction = (AggregateFunction)2;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].TextAlignment = (TextAlignment)3;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].FormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].TotalFormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].AggregateFunction = (AggregateFunction)2;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].TextAlignment = (TextAlignment)3;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].FormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].TotalFormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].AggregateFunction = (AggregateFunction)2;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].TextAlignment = (TextAlignment)3;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].FormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].TotalFormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].AggregateFunction = (AggregateFunction)2;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].TextAlignment = (TextAlignment)3;
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].FormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdParentPurTypeAndPackingWiseW);
			ctrlGrdDetailByDocType_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummaryByWagesAcSetting()
	{
		try
		{
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["ParentCategory"].Caption = "Par Category";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].Caption = "Min/Bag";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].Caption = "Max/Bag";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].Caption = "CountEbWt";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].Caption = "T.Count";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["PackingTypeId"].Visible = false;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].FormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MinPerBag"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].FormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["MaxPerBag"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].FormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["CountEbWt"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].FormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseW.RootTable.Columns["TotalCount"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdParentCityPurTypeAndPackingWiseW);
			ctrlGrdDetailByContractor_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingWiseWOSetting()
	{
		try
		{
			grdPackingWiseWO.RootTable.Columns["ParentCategory"].Caption = "Par Category";
			grdPackingWiseWO.RootTable.Columns["ParentCategory"].Caption = "Par Category";
			grdPackingWiseWO.RootTable.Columns["PackingTypeId"].Visible = false;
			grdPackingWiseWO.RootTable.Columns["Eb0Wt"].AggregateFunction = (AggregateFunction)2;
			grdPackingWiseWO.RootTable.Columns["Eb0Wt"].TextAlignment = (TextAlignment)3;
			grdPackingWiseWO.RootTable.Columns["Eb0Wt"].FormatString = "#,##0.##";
			grdPackingWiseWO.RootTable.Columns["Eb0Wt"].TotalFormatString = "#,##0.##";
			grdPackingWiseWO.RootTable.Columns["Eb0Qty"].AggregateFunction = (AggregateFunction)2;
			grdPackingWiseWO.RootTable.Columns["Eb0Qty"].TextAlignment = (TextAlignment)3;
			grdPackingWiseWO.RootTable.Columns["Eb0Qty"].FormatString = "#,##0.##";
			grdPackingWiseWO.RootTable.Columns["Eb0Qty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdPackingWiseWO);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdParentAndPackingWiseWOSetting()
	{
		try
		{
			grdParentAndPackingWiseWO.RootTable.Columns["ParentCategory"].Caption = "Par Category";
			grdParentAndPackingWiseWO.RootTable.Columns["PackingTypeId"].Visible = false;
			grdParentAndPackingWiseWO.RootTable.Columns["Eb0Qty"].AggregateFunction = (AggregateFunction)2;
			grdParentAndPackingWiseWO.RootTable.Columns["Eb0Qty"].TextAlignment = (TextAlignment)3;
			grdParentAndPackingWiseWO.RootTable.Columns["Eb0Qty"].FormatString = "#,##0.###";
			grdParentAndPackingWiseWO.RootTable.Columns["Eb0Qty"].TotalFormatString = "#,##0.###";
			grdParentAndPackingWiseWO.RootTable.Columns["Eb0Wt"].AggregateFunction = (AggregateFunction)2;
			grdParentAndPackingWiseWO.RootTable.Columns["Eb0Wt"].TextAlignment = (TextAlignment)3;
			grdParentAndPackingWiseWO.RootTable.Columns["Eb0Wt"].FormatString = "#,##0.###";
			grdParentAndPackingWiseWO.RootTable.Columns["Eb0Wt"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdParentAndPackingWiseWO);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdParentPurTypeAndPackingWiseWOSetting()
	{
		try
		{
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["ParentCategory"].Caption = "Par Category";
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["PackingTypeId"].Visible = false;
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Qty"].AggregateFunction = (AggregateFunction)2;
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Qty"].TextAlignment = (TextAlignment)3;
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Qty"].FormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Qty"].TotalFormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Wt"].AggregateFunction = (AggregateFunction)2;
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Wt"].TextAlignment = (TextAlignment)3;
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Wt"].FormatString = "#,##0.###";
			grdParentPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Wt"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdParentPurTypeAndPackingWiseWO);
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdParentCityPurTypeSupplierAndPackingWiseWOSetting()
	{
		try
		{
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["PackingTypeId"].Visible = false;
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["MinPerBag"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["MinPerBag"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["MinPerBag"].FormatString = "#,##0.###";
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["MinPerBag"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["MaxPerBag"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["MaxPerBag"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["MaxPerBag"].FormatString = "#,##0.###";
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["MaxPerBag"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["Count0EbWt"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["Count0EbWt"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["Count0EbWt"].FormatString = "#,##0.###";
			grdParentCityPurTypeSupplierAndPackingWiseWO.RootTable.Columns["Count0EbWt"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdParentCityPurTypeSupplierAndPackingWiseWO);
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdParentCityPurTypeAndPackingWiseWOSetting()
	{
		try
		{
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["ParentCategory"].Caption = "Par Category";
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["PackingTypeId"].Visible = false;
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Qty"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Qty"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Qty"].FormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Qty"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Wt"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Wt"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Wt"].FormatString = "#,##0.###";
			grdParentCityPurTypeAndPackingWiseWO.RootTable.Columns["Eb0Wt"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdParentCityPurTypeAndPackingWiseWO);
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWOSetting()
	{
		try
		{
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["ParentCategory"].Caption = "Par Category";
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["PackingTypeId"].Visible = false;
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["BillWeight"].FormatString = "#,##0.###";
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["EbPerBag"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["EbPerBag"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["EbPerBag"].FormatString = "#,##0.###";
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["EbPerBag"].TotalFormatString = "#,##0.###";
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["WtDeduc"].AggregateFunction = (AggregateFunction)2;
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["WtDeduc"].TextAlignment = (TextAlignment)3;
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["WtDeduc"].FormatString = "#,##0.###";
			grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.RootTable.Columns["WtDeduc"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO);
			ctrlGrdBar4_Load(null, null);
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
			if (tabControl1.SelectedIndex == 0)
			{
				GridFill1();
			}
			else
			{
				gridfill2();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
	}

	private void PrintButtonManage()
	{
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			txtEbUnitTo.Text = string.Empty;
			txtEbUnitfrom.Text = string.Empty;
			((Control)(object)cmbSupplier).Text = string.Empty;
			((Control)(object)cmbPackType).Text = string.Empty;
			cmbDateType.Focus();
			Datetypefill();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdParentAndPackingWiseW).Name);
			if (fs != null)
			{
				grdParentAndPackingWiseW.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdDetailByDocType_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdParentPurTypeAndPackingWiseW).Name);
			if (fs != null)
			{
				grdParentPurTypeAndPackingWiseW.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdDetaSummByDontractor_Load(object sender, EventArgs e)
	{
	}

	private void ctrlGrdDetailByContractor_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdParentCityPurTypeAndPackingWiseW).Name);
			if (fs != null)
			{
				grdParentCityPurTypeAndPackingWiseW.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmWagesDashboardReport_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtEbUnitfrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbUnitTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrnEbAndWtCutDeductionAudit_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdParentAndPackingWiseW).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbDateType.Focus();
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date Type combo in Filters Box");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			AllDropDownBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPackingWiseWO).Name);
			if (fs != null)
			{
				grdPackingWiseWO.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdParentPurTypeAndPackingWiseWO).Name);
			if (fs != null)
			{
				grdParentPurTypeAndPackingWiseWO.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdParentCityPurTypeAndPackingWiseWO).Name);
			if (fs != null)
			{
				grdParentCityPurTypeAndPackingWiseWO.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdParentCityPurTypeAndPackingWiseWO).Name);
			if (fs != null)
			{
				grdParentCityPurTypeAndPackingWiseWO.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdParentCityPurTypeAndPackingWiseWO).Name);
			if (fs != null)
			{
				grdParentCityPurTypeAndPackingWiseWO.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).Name);
			if (fs != null)
			{
				grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "tabControl1")
			{
				cmbDateType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateType.Value) == 1)
			{
				txtDateFrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				txtDateFrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtDateFrom.Value = first;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtDateFrom.Value = first2;
				txtToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		GridEXLayout grdParentCityPurTypeAndPackingWiseW_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdParentPurTypeAndPackingWiseW_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdParentAndPackingWiseW_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Audit_Dashboard.GrnEbAndWtCutDeductionAudit));
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		GridEXLayout grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdParentCityPurTypeAndPackingWiseWO_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdParentPurTypeAndPackingWiseWO_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdParentAndPackingWiseWO_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingWiseWO_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdParentCityPurTypeSupplierAndPackingWiseWO_DesignTimeLayout = new GridEXLayout();
		this.grdParentCityPurTypeAndPackingWiseW = new GridEX();
		this.grdParentPurTypeAndPackingWiseW = new GridEX();
		this.grdParentAndPackingWiseW = new GridEX();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbSupplier = new UltraCombo();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.btnShow = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbPackType = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.txtEbUnitTo = new System.Windows.Forms.TextBox();
		this.txtEbUnitfrom = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel25 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.grdParentCityPurTypeAndPackingWiseWO = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdParentPurTypeAndPackingWiseWO = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.grdParentAndPackingWiseWO = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdPackingWiseWO = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.grdParentCityPurTypeSupplierAndPackingWiseWO = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.ctrlGrdDetailByContractor = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdDetailByDocType = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdDetaSummByDocType = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		((System.ComponentModel.ISupportInitialize)this.grdParentCityPurTypeAndPackingWiseW).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdParentPurTypeAndPackingWiseW).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdParentAndPackingWiseW).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel25.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel19.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).BeginInit();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentCityPurTypeAndPackingWiseWO).BeginInit();
		this.panel4.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentPurTypeAndPackingWiseWO).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentAndPackingWiseWO).BeginInit();
		this.panel11.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingWiseWO).BeginInit();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentCityPurTypeSupplierAndPackingWiseWO).BeginInit();
		this.panel9.SuspendLayout();
		base.SuspendLayout();
		this.grdParentCityPurTypeAndPackingWiseW.AllowEdit = (InheritableBoolean)2;
		grdParentCityPurTypeAndPackingWiseW_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdParentCityPurTypeAndPackingWiseW.DesignTimeLayout = grdParentCityPurTypeAndPackingWiseW_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseW).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdParentCityPurTypeAndPackingWiseW.DynamicFiltering = true;
		this.grdParentCityPurTypeAndPackingWiseW.FilterMode = (FilterMode)2;
		this.grdParentCityPurTypeAndPackingWiseW.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseW).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentCityPurTypeAndPackingWiseW.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseW).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseW).Name = "grdParentCityPurTypeAndPackingWiseW";
		((ScrollableBase)this.grdParentCityPurTypeAndPackingWiseW).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseW).Size = new System.Drawing.Size(675, 530);
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseW).TabIndex = 4;
		this.grdParentCityPurTypeAndPackingWiseW.TotalRow = (InheritableBoolean)1;
		this.grdParentCityPurTypeAndPackingWiseW.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentCityPurTypeAndPackingWiseW).VisualStyle = (VisualStyle)4;
		this.grdParentPurTypeAndPackingWiseW.AllowEdit = (InheritableBoolean)2;
		grdParentPurTypeAndPackingWiseW_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdParentPurTypeAndPackingWiseW.DesignTimeLayout = grdParentPurTypeAndPackingWiseW_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseW).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdParentPurTypeAndPackingWiseW.DynamicFiltering = true;
		this.grdParentPurTypeAndPackingWiseW.FilterMode = (FilterMode)2;
		this.grdParentPurTypeAndPackingWiseW.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseW).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentPurTypeAndPackingWiseW.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseW).Location = new System.Drawing.Point(0, 338);
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseW).Name = "grdParentPurTypeAndPackingWiseW";
		((ScrollableBase)this.grdParentPurTypeAndPackingWiseW).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseW).Size = new System.Drawing.Size(595, 218);
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseW).TabIndex = 6;
		this.grdParentPurTypeAndPackingWiseW.TotalRow = (InheritableBoolean)1;
		this.grdParentPurTypeAndPackingWiseW.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentPurTypeAndPackingWiseW).VisualStyle = (VisualStyle)4;
		this.grdParentAndPackingWiseW.AllowEdit = (InheritableBoolean)2;
		grdParentAndPackingWiseW_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdParentAndPackingWiseW.DesignTimeLayout = grdParentAndPackingWiseW_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseW).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdParentAndPackingWiseW.DynamicFiltering = true;
		this.grdParentAndPackingWiseW.FilterMode = (FilterMode)2;
		this.grdParentAndPackingWiseW.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseW).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentAndPackingWiseW.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseW).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseW).Name = "grdParentAndPackingWiseW";
		((ScrollableBase)this.grdParentAndPackingWiseW).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseW).Size = new System.Drawing.Size(595, 284);
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseW).TabIndex = 7;
		this.grdParentAndPackingWiseW.TotalRow = (InheritableBoolean)1;
		this.grdParentAndPackingWiseW.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentAndPackingWiseW).VisualStyle = (VisualStyle)4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(9, 51);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(91, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "From && To Date";
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(601, 15);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(245, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 5;
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(104, 47);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(110, 23);
		this.txtDateFrom.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(522, 21);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(51, 15);
		this.label3.TabIndex = 165;
		this.label3.Text = "Supplier";
		this.txtToDate.CustomFormat = "dd-MMM-yy";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDate.Location = new System.Drawing.Point(220, 47);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(124, 23);
		this.txtToDate.TabIndex = 2;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(777, 45);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(69, 26);
		this.btnShow.TabIndex = 7;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(522, 51);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(77, 15);
		this.label10.TabIndex = 159;
		this.label10.Text = "Packing Type";
		this.cmbPackType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Location = new System.Drawing.Point(601, 45);
		((System.Windows.Forms.Control)(object)this.cmbPackType).Name = "cmbPackType";
		((System.Windows.Forms.Control)(object)this.cmbPackType).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabIndex = 6;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label16);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1284, 57);
		this.panel2.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(3, 31);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(332, 19);
		this.label16.TabIndex = 3;
		this.label16.Text = "Grn Eb And Weight Cut Deduction Audit";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnRefresh, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1284, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKeys.Image");
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(101, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.panel3.AutoScroll = true;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 57);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1284, 83);
		this.panel3.TabIndex = 2;
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add(this.txtEbUnitTo);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackType);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.groupBox1.Controls.Add(this.txtEbUnitfrom);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtDateFrom);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtToDate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(858, 79);
		this.groupBox1.TabIndex = 166;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(9, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(60, 15);
		this.label2.TabIndex = 167;
		this.label2.Text = "Date Type";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(104, 15);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 0;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.txtEbUnitTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEbUnitTo.Location = new System.Drawing.Point(436, 47);
		this.txtEbUnitTo.Name = "txtEbUnitTo";
		this.txtEbUnitTo.ShortcutsEnabled = false;
		this.txtEbUnitTo.Size = new System.Drawing.Size(83, 23);
		this.txtEbUnitTo.TabIndex = 4;
		this.txtEbUnitTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbUnitTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnitTo_KeyPress);
		this.txtEbUnitfrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEbUnitfrom.Location = new System.Drawing.Point(436, 17);
		this.txtEbUnitfrom.Name = "txtEbUnitfrom";
		this.txtEbUnitfrom.ShortcutsEnabled = false;
		this.txtEbUnitfrom.Size = new System.Drawing.Size(83, 23);
		this.txtEbUnitfrom.TabIndex = 3;
		this.txtEbUnitfrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbUnitfrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnitfrom_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(354, 51);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 15);
		this.label5.TabIndex = 159;
		this.label5.Text = "E.B Unit To";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(354, 21);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 159;
		this.label4.Text = "E.B Unit From";
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 140);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1284, 591);
		this.tabControl1.TabIndex = 3;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1276, 562);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "GRN With EmptyBags Weight Cut Deduction";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseW);
		this.panel7.Controls.Add(this.panel25);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(598, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(675, 556);
		this.panel7.TabIndex = 2;
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.label13);
		this.panel25.Controls.Add(this.ctrlGrdDetailByContractor);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(675, 26);
		this.panel25.TabIndex = 4;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(6, 5);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(386, 16);
		this.label13.TabIndex = 2;
		this.label13.Text = "Parent Category,Packing,Purchase Type and Supplier Wise";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseW);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseW);
		this.panel5.Controls.Add(this.panel19);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(595, 556);
		this.panel5.TabIndex = 0;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdDetailByDocType);
		this.panel12.Controls.Add(this.label15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 311);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(595, 27);
		this.panel12.TabIndex = 8;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(4, 6);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(233, 16);
		this.label14.TabIndex = 1;
		this.label14.Text = "Parent Category And Packing Wise";
		this.panel19.BackColor = System.Drawing.Color.Teal;
		this.panel19.Controls.Add(this.label14);
		this.panel19.Controls.Add(this.ctrlGrdDetaSummByDocType);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(595, 27);
		this.panel19.TabIndex = 2;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(313, 16);
		this.label15.TabIndex = 2;
		this.label15.Text = "Parent Category,Packing && Purchase TypeWise";
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1276, 562);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "GRN Without EmptyBags Weight Cut Deduction";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.panel1);
		this.panel13.Controls.Add(this.panel10);
		this.panel13.Controls.Add(this.panel8);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1270, 556);
		this.panel13.TabIndex = 6;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO);
		this.panel1.Controls.Add(this.panel16);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseWO);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(824, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(446, 556);
		this.panel1.TabIndex = 5;
		this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.AllowEdit = (InheritableBoolean)2;
		grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.DesignTimeLayout = grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.DynamicFiltering = true;
		this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.FilterMode = (FilterMode)2;
		this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).Location = new System.Drawing.Point(0, 344);
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).Name = "grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO";
		((ScrollableBase)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).Size = new System.Drawing.Size(446, 212);
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).TabIndex = 4;
		this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.TotalRow = (InheritableBoolean)1;
		this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.ctrlGrdBar4);
		this.panel16.Controls.Add(this.label7);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 317);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(446, 27);
		this.panel16.TabIndex = 8;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(11, 4);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(44, 16);
		this.label7.TabIndex = 1;
		this.label7.Text = "Detail";
		this.grdParentCityPurTypeAndPackingWiseWO.AllowEdit = (InheritableBoolean)2;
		grdParentCityPurTypeAndPackingWiseWO_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdParentCityPurTypeAndPackingWiseWO.DesignTimeLayout = grdParentCityPurTypeAndPackingWiseWO_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseWO).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdParentCityPurTypeAndPackingWiseWO.DynamicFiltering = true;
		this.grdParentCityPurTypeAndPackingWiseWO.FilterMode = (FilterMode)2;
		this.grdParentCityPurTypeAndPackingWiseWO.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseWO).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentCityPurTypeAndPackingWiseWO.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseWO).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseWO).Name = "grdParentCityPurTypeAndPackingWiseWO";
		((ScrollableBase)this.grdParentCityPurTypeAndPackingWiseWO).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseWO).Size = new System.Drawing.Size(446, 290);
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeAndPackingWiseWO).TabIndex = 7;
		this.grdParentCityPurTypeAndPackingWiseWO.TotalRow = (InheritableBoolean)1;
		this.grdParentCityPurTypeAndPackingWiseWO.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentCityPurTypeAndPackingWiseWO).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.ctrlGrdBar6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(446, 27);
		this.panel4.TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(6, 6);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(415, 16);
		this.label8.TabIndex = 0;
		this.label8.Text = "Parent Category,Packing,Purchase Type,City and Supplier Wise";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseWO);
		this.panel10.Controls.Add(this.panel18);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseWO);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(357, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(467, 556);
		this.panel10.TabIndex = 3;
		this.grdParentPurTypeAndPackingWiseWO.AllowEdit = (InheritableBoolean)2;
		grdParentPurTypeAndPackingWiseWO_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdParentPurTypeAndPackingWiseWO.DesignTimeLayout = grdParentPurTypeAndPackingWiseWO_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseWO).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdParentPurTypeAndPackingWiseWO.DynamicFiltering = true;
		this.grdParentPurTypeAndPackingWiseWO.FilterMode = (FilterMode)2;
		this.grdParentPurTypeAndPackingWiseWO.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseWO).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentPurTypeAndPackingWiseWO.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseWO).Location = new System.Drawing.Point(0, 344);
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseWO).Name = "grdParentPurTypeAndPackingWiseWO";
		((ScrollableBase)this.grdParentPurTypeAndPackingWiseWO).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseWO).Size = new System.Drawing.Size(467, 212);
		((System.Windows.Forms.Control)(object)this.grdParentPurTypeAndPackingWiseWO).TabIndex = 4;
		this.grdParentPurTypeAndPackingWiseWO.TotalRow = (InheritableBoolean)1;
		this.grdParentPurTypeAndPackingWiseWO.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentPurTypeAndPackingWiseWO).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label9);
		this.panel18.Controls.Add(this.ctrlGrdBar1);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 317);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(467, 27);
		this.panel18.TabIndex = 8;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(3, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(359, 16);
		this.label9.TabIndex = 1;
		this.label9.Text = "Parent Category,Packing,Purchase Type and City Wise";
		this.grdParentAndPackingWiseWO.AllowEdit = (InheritableBoolean)2;
		grdParentAndPackingWiseWO_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdParentAndPackingWiseWO.DesignTimeLayout = grdParentAndPackingWiseWO_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseWO).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdParentAndPackingWiseWO.DynamicFiltering = true;
		this.grdParentAndPackingWiseWO.FilterMode = (FilterMode)2;
		this.grdParentAndPackingWiseWO.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseWO).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentAndPackingWiseWO.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseWO).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseWO).Name = "grdParentAndPackingWiseWO";
		((ScrollableBase)this.grdParentAndPackingWiseWO).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseWO).Size = new System.Drawing.Size(467, 290);
		((System.Windows.Forms.Control)(object)this.grdParentAndPackingWiseWO).TabIndex = 7;
		this.grdParentAndPackingWiseWO.TotalRow = (InheritableBoolean)1;
		this.grdParentAndPackingWiseWO.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentAndPackingWiseWO).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label6);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(467, 27);
		this.panel11.TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(6, 5);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(302, 16);
		this.label6.TabIndex = 1;
		this.label6.Text = "Parent Category,Packing,Purchase Type Wise";
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingWiseWO);
		this.panel8.Controls.Add(this.panel21);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierAndPackingWiseWO);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(357, 556);
		this.panel8.TabIndex = 4;
		this.grdPackingWiseWO.AllowEdit = (InheritableBoolean)2;
		grdPackingWiseWO_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingWiseWO.DesignTimeLayout = grdPackingWiseWO_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingWiseWO).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingWiseWO.DynamicFiltering = true;
		this.grdPackingWiseWO.FilterMode = (FilterMode)2;
		this.grdPackingWiseWO.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackingWiseWO).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingWiseWO.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPackingWiseWO).Location = new System.Drawing.Point(0, 344);
		((System.Windows.Forms.Control)(object)this.grdPackingWiseWO).Name = "grdPackingWiseWO";
		((ScrollableBase)this.grdPackingWiseWO).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingWiseWO).Size = new System.Drawing.Size(357, 212);
		((System.Windows.Forms.Control)(object)this.grdPackingWiseWO).TabIndex = 6;
		this.grdPackingWiseWO.TotalRow = (InheritableBoolean)1;
		this.grdPackingWiseWO.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingWiseWO).VisualStyle = (VisualStyle)4;
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.ctrlGrdBar2);
		this.panel21.Controls.Add(this.label12);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 317);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(357, 27);
		this.panel21.TabIndex = 5;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(5, 5);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(233, 16);
		this.label12.TabIndex = 0;
		this.label12.Text = "Parent Category And Packing Wise";
		this.grdParentCityPurTypeSupplierAndPackingWiseWO.AllowEdit = (InheritableBoolean)2;
		grdParentCityPurTypeSupplierAndPackingWiseWO_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdParentCityPurTypeSupplierAndPackingWiseWO.DesignTimeLayout = grdParentCityPurTypeSupplierAndPackingWiseWO_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierAndPackingWiseWO).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdParentCityPurTypeSupplierAndPackingWiseWO.DynamicFiltering = true;
		this.grdParentCityPurTypeSupplierAndPackingWiseWO.FilterMode = (FilterMode)2;
		this.grdParentCityPurTypeSupplierAndPackingWiseWO.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierAndPackingWiseWO).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentCityPurTypeSupplierAndPackingWiseWO.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierAndPackingWiseWO).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierAndPackingWiseWO).Name = "grdParentCityPurTypeSupplierAndPackingWiseWO";
		((ScrollableBase)this.grdParentCityPurTypeSupplierAndPackingWiseWO).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierAndPackingWiseWO).Size = new System.Drawing.Size(357, 290);
		((System.Windows.Forms.Control)(object)this.grdParentCityPurTypeSupplierAndPackingWiseWO).TabIndex = 7;
		this.grdParentCityPurTypeSupplierAndPackingWiseWO.TotalRow = (InheritableBoolean)1;
		this.grdParentCityPurTypeSupplierAndPackingWiseWO.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentCityPurTypeSupplierAndPackingWiseWO).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar5);
		this.panel9.Controls.Add(this.label11);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(357, 27);
		this.panel9.TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(3, 6);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(91, 16);
		this.label11.TabIndex = 1;
		this.label11.Text = "Packing Wise";
		this.ctrlGrdDetailByContractor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdDetailByContractor.FormName = this;
		this.ctrlGrdDetailByContractor.Location = new System.Drawing.Point(630, 0);
		this.ctrlGrdDetailByContractor.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdDetailByContractor.MyGrid = this.grdParentCityPurTypeAndPackingWiseW;
		this.ctrlGrdDetailByContractor.Name = "ctrlGrdDetailByContractor";
		this.ctrlGrdDetailByContractor.Size = new System.Drawing.Size(43, 24);
		this.ctrlGrdDetailByContractor.TabIndex = 0;
		this.ctrlGrdDetailByContractor.Load += new System.EventHandler(ctrlGrdDetailByContractor_Load);
		this.ctrlGrdDetailByDocType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdDetailByDocType.FormName = this;
		this.ctrlGrdDetailByDocType.Location = new System.Drawing.Point(550, -1);
		this.ctrlGrdDetailByDocType.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdDetailByDocType.MyGrid = this.grdParentPurTypeAndPackingWiseW;
		this.ctrlGrdDetailByDocType.Name = "ctrlGrdDetailByDocType";
		this.ctrlGrdDetailByDocType.Size = new System.Drawing.Size(43, 28);
		this.ctrlGrdDetailByDocType.TabIndex = 0;
		this.ctrlGrdDetailByDocType.Load += new System.EventHandler(ctrlGrdDetailByDocType_Load);
		this.ctrlGrdDetaSummByDocType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdDetaSummByDocType.FormName = this;
		this.ctrlGrdDetaSummByDocType.Location = new System.Drawing.Point(545, 1);
		this.ctrlGrdDetaSummByDocType.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdDetaSummByDocType.MyGrid = this.grdParentAndPackingWiseW;
		this.ctrlGrdDetaSummByDocType.Name = "ctrlGrdDetaSummByDocType";
		this.ctrlGrdDetaSummByDocType.Size = new System.Drawing.Size(47, 25);
		this.ctrlGrdDetaSummByDocType.TabIndex = 1;
		this.ctrlGrdDetaSummByDocType.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(401, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(43, 25);
		this.ctrlGrdBar4.TabIndex = 0;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(401, 0);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar6.MyGrid = null;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(44, 26);
		this.ctrlGrdBar6.TabIndex = 0;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(419, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdParentPurTypeAndPackingWiseWO;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 25);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(417, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdParentAndPackingWiseWO;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(47, 25);
		this.ctrlGrdBar3.TabIndex = 1;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(310, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdPackingWiseWO;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar2.TabIndex = 0;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(310, 0);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar5.MyGrid = this.grdParentCityPurTypeSupplierAndPackingWiseWO;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(47, 25);
		this.ctrlGrdBar5.TabIndex = 1;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1284, 731);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.KeyPreview = true;
		base.Name = "GrnEbAndWtCutDeductionAudit";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmWagesDashboardReport_FormClosing);
		base.Load += new System.EventHandler(frmEvaulationDetailSalesReports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GrnEbAndWtCutDeductionAudit_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grdParentCityPurTypeAndPackingWiseW).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdParentPurTypeAndPackingWiseW).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdParentAndPackingWiseW).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdParentCityPurTypeSupplierQtyWeightAndPackingWiseWO).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentCityPurTypeAndPackingWiseWO).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdParentPurTypeAndPackingWiseWO).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentAndPackingWiseWO).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingWiseWO).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentCityPurTypeSupplierAndPackingWiseWO).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		base.ResumeLayout(false);
	}
}
