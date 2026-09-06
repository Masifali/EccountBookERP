using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.ChequePrinting;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.ChequePrinting;

public class ChequePrinting : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtDetailList = new DataTable();

	private DataTable dtBanks = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private WaitForm waitForm = new WaitForm();

	private string reportPath = "";

	public static int VoucherId;

	private int BankId;

	private string BankName = "";

	private int selectedChequeId;

	private List<int> SelectedChequesList = new List<int>();

	private bool selectall;

	public string ChequeTemplate = "";

	private IContainer components;

	private Panel panel2;

	private Label label1;

	private FlowLayoutPanel flowLayoutPanel1;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnLoadAll;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnCheckAll;

	private ToolStripLabel SelectedCount;

	private ToolStripButton btn294APrint;

	private Label label64;

	internal UltraCombo cmbBanks;

	private Button btnLoad;

	private SplitContainer splitContainer1;

	internal GridEX grdChequeCount;

	public ChequePrinting(UserAccount puserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void getChequePrintListABL()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			ABL[] CardsList = new ABL[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new ABL();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintList()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			ChequePrint[] CardsList = new ChequePrint[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new ChequePrint();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListBankAlfalah()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			BankAlFalah[] CardsList = new BankAlFalah[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new BankAlFalah();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListAskariBank()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			AskariBank[] CardsList = new AskariBank[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new AskariBank();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListBankAlHabib()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			BankAlHabib[] CardsList = new BankAlHabib[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new BankAlHabib();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListFaysalBank()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			FaysalBank[] CardsList = new FaysalBank[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new FaysalBank();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListHabibMetro()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			HabibMetro[] CardsList = new HabibMetro[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new HabibMetro();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListHBL()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			HBL[] CardsList = new HBL[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new HBL();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListMCB()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			MCB[] CardsList = new MCB[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new MCB();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListMeezanBank()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			MeezanBank[] CardsList = new MeezanBank[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new MeezanBank();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListNBP()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			NBP[] CardsList = new NBP[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new NBP();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListNIB()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			NIB[] CardsList = new NIB[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new NIB();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListStandardChartered()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			SaadiqStandardCharteredBank[] CardsList = new SaadiqStandardCharteredBank[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new SaadiqStandardCharteredBank();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListSilkBank()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			SilkBank[] CardsList = new SilkBank[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new SilkBank();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void getChequePrintListSoneriBank()
	{
		try
		{
			flowLayoutPanel1.Controls.Clear();
			if (dtDetailList == null)
			{
				return;
			}
			SoneriBank[] CardsList = new SoneriBank[dtDetailList.Rows.Count];
			int i = 0;
			foreach (DataRow Module in dtDetailList.Rows)
			{
				CardsList[i] = new SoneriBank();
				CardsList[i].Margin = new Padding(15, 5, 5, 5);
				CardsList[i].Id = Conversion.ToInt((object)Module["Id"].ToString());
				CardsList[i].ChequeTypeId = Conversion.ToInt((object)Module["ChequeTypeId"].ToString());
				CardsList[i].ChequeNumber = Module["ChequeNumber"].ToString();
				CardsList[i].CheqType = Module["CheqType"].ToString();
				CardsList[i].PayeeTitle = Module["PayeeTitle"].ToString();
				CardsList[i].AmountInWords = Module["AmountInWords"].ToString();
				CardsList[i].Amount = Conversion.ToDouble(Module["Amount"]).ToString("##,#.####");
				CardsList[i].D1 = Module["D1"].ToString();
				CardsList[i].D2 = Module["D2"].ToString();
				CardsList[i].M1 = Module["M1"].ToString();
				CardsList[i].M2 = Module["M2"].ToString();
				CardsList[i].Y1 = Module["Y1"].ToString();
				CardsList[i].Y2 = Module["Y2"].ToString();
				CardsList[i].Y3 = Module["Y3"].ToString();
				CardsList[i].Y4 = Module["Y4"].ToString();
				CardsList[i].UserControlClicked += ChequePrintCard_Click;
				flowLayoutPanel1.Controls.Add(CardsList[i]);
				i++;
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void btnCheckAll_Click(object sender, EventArgs e)
	{
		try
		{
			if (ChequeTemplate == "UBL")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control in flowLayoutPanel1.Controls)
					{
						if (control is ChequePrint chequeprin)
						{
							chequeprin.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control2 in flowLayoutPanel1.Controls)
				{
					if (control2 is ChequePrint chequeprin3)
					{
						chequeprin3.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "BankAlFalah")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control3 in flowLayoutPanel1.Controls)
					{
						if (control3 is BankAlFalah chequeprin4)
						{
							chequeprin4.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin4.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control4 in flowLayoutPanel1.Controls)
				{
					if (control4 is BankAlFalah chequeprin6)
					{
						chequeprin6.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "ABL")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control5 in flowLayoutPanel1.Controls)
					{
						if (control5 is ABL chequeprin7)
						{
							chequeprin7.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin7.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control6 in flowLayoutPanel1.Controls)
				{
					if (control6 is ABL chequeprin9)
					{
						chequeprin9.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "AskariBank")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control7 in flowLayoutPanel1.Controls)
					{
						if (control7 is AskariBank chequeprin10)
						{
							chequeprin10.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin10.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control8 in flowLayoutPanel1.Controls)
				{
					if (control8 is AskariBank chequeprin12)
					{
						chequeprin12.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "BankAlHabib")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control9 in flowLayoutPanel1.Controls)
					{
						if (control9 is BankAlHabib chequeprin13)
						{
							chequeprin13.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin13.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control10 in flowLayoutPanel1.Controls)
				{
					if (control10 is BankAlHabib chequeprin15)
					{
						chequeprin15.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "FaysalBank")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control11 in flowLayoutPanel1.Controls)
					{
						if (control11 is FaysalBank chequeprin16)
						{
							chequeprin16.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin16.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control12 in flowLayoutPanel1.Controls)
				{
					if (control12 is FaysalBank chequeprin18)
					{
						chequeprin18.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "HabibMetro")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control13 in flowLayoutPanel1.Controls)
					{
						if (control13 is HabibMetro chequeprin19)
						{
							chequeprin19.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin19.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control14 in flowLayoutPanel1.Controls)
				{
					if (control14 is HabibMetro chequeprin21)
					{
						chequeprin21.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "HBL")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control15 in flowLayoutPanel1.Controls)
					{
						if (control15 is HBL chequeprin22)
						{
							chequeprin22.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin22.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control16 in flowLayoutPanel1.Controls)
				{
					if (control16 is HBL chequeprin24)
					{
						chequeprin24.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "MCB")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control17 in flowLayoutPanel1.Controls)
					{
						if (control17 is MCB chequeprin25)
						{
							chequeprin25.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin25.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control18 in flowLayoutPanel1.Controls)
				{
					if (control18 is MCB chequeprin27)
					{
						chequeprin27.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "MeezanBank")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control19 in flowLayoutPanel1.Controls)
					{
						if (control19 is MeezanBank chequeprin28)
						{
							chequeprin28.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin28.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control20 in flowLayoutPanel1.Controls)
				{
					if (control20 is MeezanBank chequeprin30)
					{
						chequeprin30.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "NBP")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control21 in flowLayoutPanel1.Controls)
					{
						if (control21 is NBP chequeprin29)
						{
							chequeprin29.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin29.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control22 in flowLayoutPanel1.Controls)
				{
					if (control22 is NBP chequeprin26)
					{
						chequeprin26.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "NIB")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control23 in flowLayoutPanel1.Controls)
					{
						if (control23 is NIB chequeprin23)
						{
							chequeprin23.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin23.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control24 in flowLayoutPanel1.Controls)
				{
					if (control24 is NIB chequeprin20)
					{
						chequeprin20.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "SaadiqStandardCharteredBank")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control25 in flowLayoutPanel1.Controls)
					{
						if (control25 is SaadiqStandardCharteredBank chequeprin17)
						{
							chequeprin17.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin17.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control26 in flowLayoutPanel1.Controls)
				{
					if (control26 is SaadiqStandardCharteredBank chequeprin14)
					{
						chequeprin14.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "SilkBank")
			{
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control27 in flowLayoutPanel1.Controls)
					{
						if (control27 is SilkBank chequeprin11)
						{
							chequeprin11.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin11.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control28 in flowLayoutPanel1.Controls)
				{
					if (control28 is SilkBank chequeprin8)
					{
						chequeprin8.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else
			{
				if (!(ChequeTemplate == "SoneriBank"))
				{
					return;
				}
				if (!selectall)
				{
					SelectedChequesList.Clear();
					foreach (Control control29 in flowLayoutPanel1.Controls)
					{
						if (control29 is SoneriBank chequeprin5)
						{
							chequeprin5.cbSelect.Checked = true;
							SelectedChequesList.Add(chequeprin5.Id);
							SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
						}
					}
					selectall = true;
					btnCheckAll.Text = "Deselect All";
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control30 in flowLayoutPanel1.Controls)
				{
					if (control30 is SoneriBank chequeprin2)
					{
						chequeprin2.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChequePrintCard_Click(object sender, EventArgs e)
	{
		try
		{
			try
			{
				if (ChequeTemplate == "UBL")
				{
					ChequePrint obj = (ChequePrint)sender;
					HandleChequeSelection(obj);
				}
				else if (ChequeTemplate == "BankAlFalah")
				{
					BankAlFalah obj2 = (BankAlFalah)sender;
					HandleChequeSelection(obj2);
				}
				else if (ChequeTemplate == "ABL")
				{
					ABL obj3 = (ABL)sender;
					HandleChequeSelection(obj3);
				}
				else if (ChequeTemplate == "AskariBank")
				{
					AskariBank obj4 = (AskariBank)sender;
					HandleChequeSelection(obj4);
				}
				else if (ChequeTemplate == "BankAlHabib")
				{
					BankAlHabib obj5 = (BankAlHabib)sender;
					HandleChequeSelection(obj5);
				}
				else if (ChequeTemplate == "FaysalBank")
				{
					FaysalBank obj6 = (FaysalBank)sender;
					HandleChequeSelection(obj6);
				}
				else if (ChequeTemplate == "HabibMetro")
				{
					HabibMetro obj7 = (HabibMetro)sender;
					HandleChequeSelection(obj7);
				}
				else if (ChequeTemplate == "HBL")
				{
					HBL obj8 = (HBL)sender;
					HandleChequeSelection(obj8);
				}
				else if (ChequeTemplate == "MCB")
				{
					MCB obj9 = (MCB)sender;
					HandleChequeSelection(obj9);
				}
				else if (ChequeTemplate == "MeezanBank")
				{
					MeezanBank obj10 = (MeezanBank)sender;
					HandleChequeSelection(obj10);
				}
				else if (ChequeTemplate == "NBP")
				{
					NBP obj11 = (NBP)sender;
					HandleChequeSelection(obj11);
				}
				else if (ChequeTemplate == "NIB")
				{
					NIB obj12 = (NIB)sender;
					HandleChequeSelection(obj12);
				}
				else if (ChequeTemplate == "SaadiqStandardCharteredBank")
				{
					SaadiqStandardCharteredBank obj13 = (SaadiqStandardCharteredBank)sender;
					HandleChequeSelection(obj13);
				}
				else if (ChequeTemplate == "SilkBank")
				{
					SilkBank obj14 = (SilkBank)sender;
					HandleChequeSelection(obj14);
				}
				else if (ChequeTemplate == "SoneriBank")
				{
					SoneriBank obj15 = (SoneriBank)sender;
					HandleChequeSelection(obj15);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	public void GetDetailData(int BankId, int VoucherId)
	{
		try
		{
			SelectedChequesList.Clear();
			if (VoucherId == 0)
			{
				dtDetailList = ChequePrintListCards.getChequePrintListCards(UserAccount.OrganizationId, UserAccount.CompanyId, BankId);
			}
			else
			{
				dtDetailList = ChequePrintListCards.getChequePrintListCardsByVoucherId(VoucherId);
			}
			if (dtDetailList.Rows.Count == 0)
			{
				flowLayoutPanel1.Controls.Clear();
				return;
			}
			DataRow dtRow = dtDetailList.Rows[0];
			ChequeTemplate = dtRow["ChequeTemplete"].ToString();
			if (ChequeTemplate == "UBL")
			{
				getChequePrintList();
			}
			else if (ChequeTemplate == "BankAlFalah")
			{
				getChequePrintListBankAlfalah();
			}
			else if (ChequeTemplate == "ABL")
			{
				getChequePrintListABL();
			}
			else if (ChequeTemplate == "AskariBank")
			{
				getChequePrintListAskariBank();
			}
			else if (ChequeTemplate == "BankAlHabib")
			{
				getChequePrintListBankAlHabib();
			}
			else if (ChequeTemplate == "FaysalBank")
			{
				getChequePrintListFaysalBank();
			}
			else if (ChequeTemplate == "HabibMetro")
			{
				getChequePrintListHabibMetro();
			}
			else if (ChequeTemplate == "HBL")
			{
				getChequePrintListHBL();
			}
			else if (ChequeTemplate == "MCB")
			{
				getChequePrintListMCB();
			}
			else if (ChequeTemplate == "MeezanBank")
			{
				getChequePrintListMeezanBank();
			}
			else if (ChequeTemplate == "NBP")
			{
				getChequePrintListNBP();
			}
			else if (ChequeTemplate == "NIB")
			{
				getChequePrintListNIB();
			}
			else if (ChequeTemplate == "SaadiqStandardCharteredBank")
			{
				getChequePrintListStandardChartered();
			}
			else if (ChequeTemplate == "SilkBank")
			{
				getChequePrintListSilkBank();
			}
			else if (ChequeTemplate == "SoneriBank")
			{
				getChequePrintListSoneriBank();
			}
			else
			{
				MessageBox.Show("Template Not Available");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BankFill()
	{
		try
		{
			dtBanks = Bank.GetByCompanyOrganization(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dtBanks.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtBanks, cmbBanks, "Id", "BranchName", "Bank", false);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void cmbBanks_ValueChanged(object sender, EventArgs e)
	{
	}

	private void btnLoad_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbBanks.Value) > 0)
			{
				GetDetailData(Conversion.ToInt(cmbBanks.Value), 0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btn294APrint_Click(object sender, EventArgs e)
	{
		try
		{
			PrintCheque();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	public void PrintCheque()
	{
		try
		{
			if (SelectedChequesList.Count > 0)
			{
				waitForm.Show();
				btn294APrint.Enabled = false;
				string ids = string.Join(",", SelectedChequesList);
				waitForm.Close();
				CommonServices.PrintSelectedCheque(ids, ChequeTemplate);
				CommonServices.UpdateChequePrintStatus(ids);
			}
			else
			{
				waitForm.Close();
				MessageBox.Show("Please Select Cheque to print");
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
			throw ex;
		}
		finally
		{
			ChequeCountGridFill();
			GetDetailData(BankId, 0);
			btn294APrint.Enabled = true;
			waitForm.Close();
		}
	}

	private void ChequePrinting_Load(object sender, EventArgs e)
	{
		try
		{
			BankFill();
			ChequeCountGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBanks_Leave(object sender, EventArgs e)
	{
		btnLoad.PerformClick();
	}

	private void HandleChequeSelection(dynamic obj)
	{
		selectedChequeId = obj.Id;
		if (Conversion.ToString(obj.Tag) == "FormCheckBox")
		{
			int id = 0;
			if (SelectedChequesList.Count > 0)
			{
				id = SelectedChequesList.Find((int Id) => Id == obj.Id);
			}
			if (obj.IsSelected == false)
			{
				if (id > 0)
				{
					SelectedChequesList.Remove(id);
				}
			}
			else if (id <= 0)
			{
				SelectedChequesList.Add(obj.Id);
			}
			else
			{
				SelectedChequesList.Remove(id);
			}
			SelectedCount.Text = Conversion.ToString((object)SelectedChequesList.Count);
		}
		else if (Conversion.ToString(obj.Tag) == "ButtonClicked")
		{
			SelectedChequesList.Clear();
			uncheckAll();
			SelectedChequesList.Add(obj.Id);
			PrintCheque();
		}
	}

	public void uncheckAll()
	{
		try
		{
			if (ChequeTemplate == "UBL")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control in flowLayoutPanel1.Controls)
				{
					if (control is ChequePrint chequeprin)
					{
						chequeprin.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "BankAlFalah")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control2 in flowLayoutPanel1.Controls)
				{
					if (control2 is BankAlFalah chequeprin3)
					{
						chequeprin3.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "ABL")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control3 in flowLayoutPanel1.Controls)
				{
					if (control3 is ABL chequeprin4)
					{
						chequeprin4.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "AskariBank")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control4 in flowLayoutPanel1.Controls)
				{
					if (control4 is AskariBank chequeprin6)
					{
						chequeprin6.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "BankAlHabib")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control5 in flowLayoutPanel1.Controls)
				{
					if (control5 is BankAlHabib chequeprin7)
					{
						chequeprin7.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "FaysalBank")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control6 in flowLayoutPanel1.Controls)
				{
					if (control6 is FaysalBank chequeprin9)
					{
						chequeprin9.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "HabibMetro")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control7 in flowLayoutPanel1.Controls)
				{
					if (control7 is HabibMetro chequeprin10)
					{
						chequeprin10.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "HBL")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control8 in flowLayoutPanel1.Controls)
				{
					if (control8 is HBL chequeprin12)
					{
						chequeprin12.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "MCB")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control9 in flowLayoutPanel1.Controls)
				{
					if (control9 is MCB chequeprin13)
					{
						chequeprin13.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "MeezanBank")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control10 in flowLayoutPanel1.Controls)
				{
					if (control10 is MeezanBank chequeprin15)
					{
						chequeprin15.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "NBP")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control11 in flowLayoutPanel1.Controls)
				{
					if (control11 is NBP chequeprin14)
					{
						chequeprin14.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "NIB")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control12 in flowLayoutPanel1.Controls)
				{
					if (control12 is NIB chequeprin11)
					{
						chequeprin11.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "SaadiqStandardCharteredBank")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control13 in flowLayoutPanel1.Controls)
				{
					if (control13 is SaadiqStandardCharteredBank chequeprin8)
					{
						chequeprin8.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else if (ChequeTemplate == "SilkBank")
			{
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control14 in flowLayoutPanel1.Controls)
				{
					if (control14 is SilkBank chequeprin5)
					{
						chequeprin5.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
			else
			{
				if (!(ChequeTemplate == "SoneriBank"))
				{
					return;
				}
				selectall = false;
				SelectedChequesList.Clear();
				foreach (Control control15 in flowLayoutPanel1.Controls)
				{
					if (control15 is SoneriBank chequeprin2)
					{
						chequeprin2.cbSelect.Checked = false;
						SelectedCount.Text = Conversion.ToString((object)0);
					}
				}
				btnCheckAll.Text = "Select All";
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public string getReportPath()
	{
		string ConnStringReportPath = ConfigurationManager.ConnectionStrings["TPSSReportDb"].ConnectionString.ToString();
		if (ConnStringReportPath != string.Empty)
		{
			reportPath = ConnStringReportPath + "\\";
		}
		else
		{
			reportPath = Application.ExecutablePath + "\\..\\Reports\\";
		}
		return reportPath + ChequeTemplate;
	}

	private void ChequePrinting_Shown(object sender, EventArgs e)
	{
		getReportPath();
		if (VoucherId != 0)
		{
			GetDetailData(0, VoucherId);
		}
	}

	private void BtnLoadAll_Click(object sender, EventArgs e)
	{
	}

	private void ChequeCountGridFill()
	{
		try
		{
			DataTable dtChequeCount = ChequeCount.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dtChequeCount.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("BranchName");
				dt.Columns.Add("Counts");
				for (int i = 0; i < dtChequeCount.Rows.Count; i++)
				{
					dt.Rows.Add(dtChequeCount.Rows[i]["Id"], dtChequeCount.Rows[i]["BranchName"], dtChequeCount.Rows[i]["Counts"]);
				}
				grdChequeCount.DataSource = dt;
				grdChequeCount.RetrieveStructure();
				grdChequeCount.RootTable.Columns["Id"].Visible = false;
				grdChequeCount.RootTable.Columns["BranchName"].Width = 250;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdChequeCount_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			BankId = Conversion.ToInt((object)grdChequeCount.CurrentRow.Cells["Id"].Value.ToString());
			GetDetailData(BankId, 0);
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
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ChequePrinting.ChequePrinting));
		GridEXLayout grdChequeCount_DesignTimeLayout = new GridEXLayout();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnLoad = new System.Windows.Forms.Button();
		this.label64 = new System.Windows.Forms.Label();
		this.cmbBanks = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btn294APrint = new System.Windows.Forms.ToolStripButton();
		this.btnCheckAll = new System.Windows.Forms.ToolStripButton();
		this.SelectedCount = new System.Windows.Forms.ToolStripLabel();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.grdChequeCount = new GridEX();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBanks).BeginInit();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdChequeCount).BeginInit();
		base.SuspendLayout();
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.btnLoad);
		this.panel2.Controls.Add(this.label64);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBanks);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1251, 65);
		this.panel2.TabIndex = 3;
		this.btnLoad.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoad.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoad.ForeColor = System.Drawing.Color.White;
		this.btnLoad.Location = new System.Drawing.Point(323, 24);
		this.btnLoad.Margin = new System.Windows.Forms.Padding(2);
		this.btnLoad.Name = "btnLoad";
		this.btnLoad.Size = new System.Drawing.Size(57, 25);
		this.btnLoad.TabIndex = 216;
		this.btnLoad.Text = "Load";
		this.btnLoad.UseVisualStyleBackColor = false;
		this.btnLoad.Visible = false;
		this.btnLoad.Click += new System.EventHandler(btnLoad_Click);
		this.label64.AutoSize = true;
		this.label64.BackColor = System.Drawing.Color.White;
		this.label64.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(12, 26);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(34, 15);
		this.label64.TabIndex = 215;
		this.label64.Text = "Bank";
		this.label64.Visible = false;
		this.cmbBanks.AllowNull = (DefaultableBoolean)1;
		this.cmbBanks.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBanks.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBanks.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbBanks).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBanks).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBanks).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBanks).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbBanks).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBanks).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbBanks).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBanks).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBanks).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbBanks).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBanks).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBanks).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBanks).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBanks.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBanks).Location = new System.Drawing.Point(52, 23);
		((System.Windows.Forms.Control)(object)this.cmbBanks).Name = "cmbBanks";
		((System.Windows.Forms.Control)(object)this.cmbBanks).Size = new System.Drawing.Size(266, 26);
		((System.Windows.Forms.Control)(object)this.cmbBanks).TabIndex = 214;
		((System.Windows.Forms.Control)(object)this.cmbBanks).Visible = false;
		this.cmbBanks.ValueChanged += new System.EventHandler(cmbBanks_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbBanks).Leave += new System.EventHandler(cmbBanks_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(495, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(279, 40);
		this.label1.TabIndex = 0;
		this.label1.Text = "Cheque Printing List";
		this.flowLayoutPanel1.AutoScroll = true;
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(947, 594);
		this.flowLayoutPanel1.TabIndex = 4;
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.BtnLoadAll, this.btnAttachment, this.btn294APrint, this.btnCheckAll, this.SelectedCount });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1251, 27);
		this.toolStrip3.TabIndex = 5;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_search_16;
		this.BtnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadAll.Name = "BtnLoadAll";
		this.BtnLoadAll.Size = new System.Drawing.Size(74, 24);
		this.BtnLoadAll.Text = "Load All";
		this.BtnLoadAll.Visible = false;
		this.BtnLoadAll.Click += new System.EventHandler(BtnLoadAll_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(37, 24);
		this.btnAttachment.Text = "0";
		this.btnAttachment.Visible = false;
		this.btn294APrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn294APrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn294APrint.Name = "btn294APrint";
		this.btn294APrint.Size = new System.Drawing.Size(103, 24);
		this.btn294APrint.Text = "Print Selected";
		this.btn294APrint.ToolTipText = "Print All";
		this.btn294APrint.Click += new System.EventHandler(btn294APrint_Click);
		this.btnCheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCheckAll.Image = (System.Drawing.Image)resources.GetObject("btnCheckAll.Image");
		this.btnCheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCheckAll.Name = "btnCheckAll";
		this.btnCheckAll.Size = new System.Drawing.Size(59, 24);
		this.btnCheckAll.Text = "Select All";
		this.btnCheckAll.ToolTipText = "Select All";
		this.btnCheckAll.Click += new System.EventHandler(btnCheckAll_Click);
		this.SelectedCount.Name = "SelectedCount";
		this.SelectedCount.Size = new System.Drawing.Size(13, 24);
		this.SelectedCount.Text = "0";
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainer1.Location = new System.Drawing.Point(0, 92);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdChequeCount);
		this.splitContainer1.Panel2.AutoScroll = true;
		this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
		this.splitContainer1.Size = new System.Drawing.Size(1251, 594);
		this.splitContainer1.SplitterDistance = 300;
		this.splitContainer1.TabIndex = 6;
		this.grdChequeCount.AllowDelete = (InheritableBoolean)1;
		this.grdChequeCount.AllowEdit = (InheritableBoolean)2;
		grdChequeCount_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdChequeCount.DesignTimeLayout = grdChequeCount_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdChequeCount).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdChequeCount.FilterMode = (FilterMode)2;
		this.grdChequeCount.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdChequeCount).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdChequeCount.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdChequeCount).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdChequeCount).Name = "grdChequeCount";
		((ScrollableBase)this.grdChequeCount).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdChequeCount).Size = new System.Drawing.Size(300, 594);
		((System.Windows.Forms.Control)(object)this.grdChequeCount).TabIndex = 3;
		this.grdChequeCount.TotalRow = (InheritableBoolean)1;
		this.grdChequeCount.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdChequeCount).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdChequeCount).DoubleClick += new System.EventHandler(grdChequeCount_DoubleClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1251, 686);
		base.Controls.Add(this.splitContainer1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.toolStrip3);
		base.Name = "ChequePrinting";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Cheque Printing";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ChequePrinting_Load);
		base.Shown += new System.EventHandler(ChequePrinting_Shown);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBanks).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdChequeCount).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
