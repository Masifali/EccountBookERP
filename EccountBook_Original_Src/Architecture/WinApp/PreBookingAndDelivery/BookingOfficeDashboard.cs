using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.VPS;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Graph;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp.PreBookingAndDelivery;

public class BookingOfficeDashboard : Form
{
	private bool IsVpsAttachmentsServiceOn;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Label label1;

	internal UltraCombo CmbCostCenter;

	private Label lblCostCenter;

	private FlowLayoutPanel flowLayoutPanel1;

	private Panel QuickActionPanel;

	private Label label2;

	private Label label3;

	private PictureBox QuickActionImage;

	private Button BtnMakePayment;

	private Button btnTrackOrder;

	private Button btnPlaceOrder;

	private Panel OrdersPanel;

	private Label label5;

	private PictureBox OrderImage;

	private Label OrderSubTitle2;

	private Label OrderSubTitle1;

	private Label Description;

	private Label OrderMainTitle;

	private Panel DeliveryPanel;

	private Label label7;

	private PictureBox DeliveryImage;

	private Label DeliverySubTitle2;

	private Label DeliverySubTitle1;

	private Label label10;

	private Label DeliveryMainTitle;

	private Panel SalesPanel;

	private Label label12;

	private PictureBox SalesImage;

	private Label SaleSubTitle2;

	private Label SaleSubTitle1;

	private Label label15;

	private Label SaleMainTitle;

	private Panel AccountsPanel;

	private Label label17;

	private PictureBox AccountsImage;

	private Label AccountsSubTitle1;

	private Label label20;

	private Label AccountsMainTitle;

	private Panel HederInfoPanel;

	private Panel SalesInfoPanel;

	private FlowLayoutPanel flowLayoutPanel2;

	private Panel TopItemsPanel;

	private PictureBox TopItemsImage;

	private Label label23;

	private Label TopItems;

	private FlowLayoutPanel TopItemFlowLayout;

	private Panel TopPacksPanel;

	private FlowLayoutPanel TopPacksFlowLayout;

	private PictureBox TopPacksImage;

	private Label label18;

	private Label TopPacks;

	private Label lblOrderSub2Count;

	private Label lblOrderSub1Count;

	private Label label25;

	private Label label6;

	private Label lblDeliverySub2Count;

	private Label lblDeliverySub1Count;

	private Label label28;

	private Label label29;

	private Label lblSalesSub2Count;

	private Label lblSalesSub1Count;

	private Label label26;

	private Label label27;

	private Label lblAccountsCount;

	private Label label30;

	private Panel panel1;

	private GroupBox groupBox1;

	private Label label33;

	public DateTimePicker fromdate;

	private Label label4;

	public DateTimePicker ToDate;

	private Button btnShow;

	public BookingOfficeDashboard(UserAccount Puser)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
		InitializeComponentMethod();
		ApplyRoundedCorners(QuickActionPanel);
		QuickActionPanel.Paint += MyPanel_Paint;
		ApplyRoundedCorners(OrdersPanel);
		OrdersPanel.Paint += MyPanel_Paint;
		ApplyRoundedCorners(DeliveryPanel);
		DeliveryPanel.Paint += MyPanel_Paint;
		ApplyRoundedCorners(SalesPanel);
		SalesPanel.Paint += MyPanel_Paint;
		ApplyRoundedCorners(AccountsPanel);
		AccountsPanel.Paint += MyPanel_Paint;
		ApplyRoundedCorners(TopItemsPanel);
		TopItemsPanel.Paint += MyPanel_Paint;
		ApplyRoundedCorners(TopPacksPanel);
		TopPacksPanel.Paint += MyPanel_Paint;
	}

	private void BookingOfficeDashboard_FormClosed(object sender, FormClosedEventArgs e)
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

	private DataTable GetCostCenter()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			return UsersWithCostCenter.UsersWithCostCenter_AllocatedData(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CostCenterFill(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCostCenter, "CostCenterId", "CostCenterName", "Cost Center", false);
				((UltraGridBase)CmbCostCenter).Rows[0].Activate();
			}
			else
			{
				((Control)(object)CmbCostCenter).Text = string.Empty;
				((UltraGridBase)CmbCostCenter).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			IsVpsAttachmentsServiceOn = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn"));
			return new
			{
				CostCenterData = GetCostCenter()
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
				AutoScroll = true;
				base.AutoScrollMargin = new Size(20, 20);
				base.AutoScrollMinSize = new Size(1200, 700);
				CostCenterFill(result.CostCenterData);
				((Control)(object)CmbCostCenter).Enabled = false;
				fromdate.Checked = false;
				GetData(Conversion.ToInt(CmbCostCenter.Value));
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetData(int CostCenterId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (fromdate.Checked)
			{
				obj.FromDate = fromdate.Value;
			}
			if (ToDate.Checked)
			{
				obj.ToDate = ToDate.Value;
			}
			obj.UserId = UserAccount.ID;
			obj.AppId = UserAccount.AppId;
			obj.CostCenterId = CostCenterId;
			DataSet ds = PreBookingOrder.BookingOfficeDashboard(obj);
			DataTable headerDt = ds.Tables[0];
			if (headerDt.Rows.Count > 0)
			{
				OrderMainTitle.Text = Conversion.ToString(headerDt.Rows[0]["Activity"]);
				OrderSubTitle1.Text = Conversion.ToString(headerDt.Rows[0]["SubTitle"]);
				OrderSubTitle2.Text = Conversion.ToString(headerDt.Rows[1]["SubTitle"]);
				lblOrderSub1Count.Text = Conversion.ToString(headerDt.Rows[0]["Total"]);
				lblOrderSub2Count.Text = Conversion.ToString(headerDt.Rows[1]["Total"]);
				DeliveryMainTitle.Text = Conversion.ToString(headerDt.Rows[2]["Activity"]);
				DeliverySubTitle1.Text = Conversion.ToString(headerDt.Rows[2]["SubTitle"]);
				DeliverySubTitle2.Text = Conversion.ToString(headerDt.Rows[3]["SubTitle"]);
				lblDeliverySub1Count.Text = Conversion.ToString(headerDt.Rows[2]["Total"]);
				lblDeliverySub2Count.Text = Conversion.ToString(headerDt.Rows[3]["Total"]);
				SaleMainTitle.Text = Conversion.ToString(headerDt.Rows[4]["Activity"]);
				SaleSubTitle1.Text = Conversion.ToString(headerDt.Rows[4]["SubTitle"]);
				SaleSubTitle2.Text = Conversion.ToString(headerDt.Rows[5]["SubTitle"]);
				lblSalesSub1Count.Text = Conversion.ToString(headerDt.Rows[4]["Total"]);
				lblSalesSub2Count.Text = Conversion.ToString(headerDt.Rows[5]["Total"]);
				AccountsMainTitle.Text = Conversion.ToString(headerDt.Rows[6]["Activity"]);
				AccountsSubTitle1.Text = Conversion.ToString(headerDt.Rows[6]["SubTitle"]);
				lblAccountsCount.Text = Conversion.ToString(headerDt.Rows[6]["Total"]);
			}
			TopItemFlowLayout.Controls.Clear();
			TopPacksFlowLayout.Controls.Clear();
			if (ds.Tables[1].Rows.Count > 0)
			{
				ServerCredentials Sc = new ServerCredentials();
				TopImageNamePercent[] cards = new TopImageNamePercent[ds.Tables[1].Rows.Count];
				for (int i = 0; i < cards.Length; i++)
				{
					string ForPath = Conversion.ToString(ds.Tables[1].Rows[i]["ItemImage"]);
					string AttachmentName = ForPath;
					string path = "";
					cards[i] = new TopImageNamePercent();
					if (IsVpsAttachmentsServiceOn)
					{
						Sc = CommonServices.GetServerCredentials();
						string uri = "ftp://" + Sc.ServerIp + "/" + AttachmentName;
						try
						{
							FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
							request.Method = "RETR";
							request.UsePassive = true;
							request.UseBinary = true;
							request.KeepAlive = false;
							request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							using FtpWebResponse ftpWebResponse = (FtpWebResponse)request.GetResponse();
							using Stream responseStream = ftpWebResponse.GetResponseStream();
							using MemoryStream memoryStream = new MemoryStream();
							responseStream.CopyTo(memoryStream);
							Path.GetExtension(Path.GetFileName(request.RequestUri.LocalPath))?.ToUpper();
							memoryStream.Position = 0L;
							byte[] streamData = memoryStream.ToArray();
							cards[i].memoryStream = streamData;
						}
						catch (WebException webEx)
						{
							if (webEx.Response is FtpWebResponse response)
							{
								throw new Exception("FTP Error: " + response.StatusDescription + " - " + webEx.Message);
							}
							throw new Exception("Error: " + webEx.Message);
						}
						catch (Exception ex)
						{
							throw new Exception("Error: " + ex.Message);
						}
					}
					else
					{
						path = clsGlobalVariables.AttachmentFolderPath + ForPath;
						cards[i].ImageName = path;
					}
					cards[i].lblItemName = Conversion.ToString(ds.Tables[1].Rows[i]["ItemName"]);
					double percent = Conversion.ToDouble(ds.Tables[1].Rows[i]["Percentage"]);
					cards[i].lblPercent = percent.ToString("#,##0.###") + "%";
					TopItemFlowLayout.Controls.Add(cards[i]);
				}
				Random random = new Random();
				TopPackNamePercent[] PackCards = new TopPackNamePercent[ds.Tables[2].Rows.Count];
				for (int j = 0; j < ds.Tables[2].Rows.Count; j++)
				{
					PackCards[j] = new TopPackNamePercent();
					PackCards[j].lblStripColor = GetRandomLightColor(random);
					PackCards[j].lblPackName = Conversion.ToString(ds.Tables[2].Rows[j]["PackUom"]);
					double prcnt = Conversion.ToDouble(ds.Tables[2].Rows[j]["Percentage"]);
					PackCards[j].lblPercent = prcnt.ToString("#,##0.###") + "%";
					TopPacksFlowLayout.Controls.Add(PackCards[j]);
				}
			}
			else
			{
				SalesInfoPanel.Visible = false;
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	private Color GetRandomLightColor(Random random)
	{
		return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
	}

	private void BookingOfficeDashboard_Load(object sender, EventArgs e)
	{
	}

	private void ApplyRoundedCorners(Panel panel)
	{
		int diameter = 20 * 2;
		using GraphicsPath path = new GraphicsPath();
		path.StartFigure();
		path.AddArc(1, 1, diameter, diameter, 180f, 90f);
		path.AddArc(panel.Width - diameter - 1, 1, diameter, diameter, 270f, 90f);
		path.AddArc(panel.Width - diameter - 1, panel.Height - diameter - 1, diameter, diameter, 0f, 90f);
		path.AddArc(1, panel.Height - diameter - 1, diameter, diameter, 90f, 90f);
		path.CloseFigure();
		panel.Region = new Region(path);
	}

	private void MyPanel_Paint(object sender, PaintEventArgs e)
	{
		if (sender is Panel panel)
		{
			DrawBorder(panel, e.Graphics, Color.Silver, 2);
		}
	}

	private void DrawBorder(Panel panel, Graphics g, Color borderColor, int borderWidth)
	{
		int diameter = 20 * 2;
		using GraphicsPath path = new GraphicsPath();
		path.StartFigure();
		path.AddArc(borderWidth / 2, borderWidth / 2, diameter, diameter, 180f, 90f);
		path.AddArc(panel.Width - diameter - 1 - borderWidth / 2, borderWidth / 2, diameter, diameter, 270f, 90f);
		path.AddArc(panel.Width - diameter - 1 - borderWidth / 2, panel.Height - diameter - 1 - borderWidth / 2, diameter, diameter, 0f, 90f);
		path.AddArc(borderWidth / 2, panel.Height - diameter - 1 - borderWidth / 2, diameter, diameter, 90f, 90f);
		path.CloseFigure();
		g.SmoothingMode = SmoothingMode.AntiAlias;
		using Pen pen = new Pen(borderColor, borderWidth);
		g.DrawPath(pen, path);
	}

	private void ApplyRoundedCorners(Button btn)
	{
		int diameter = 10 * 2;
		GraphicsPath path = new GraphicsPath();
		path.AddArc(0, 0, diameter, diameter, 180f, 90f);
		path.AddArc(btn.Width - diameter, 0, diameter, diameter, 270f, 90f);
		path.AddArc(btn.Width - diameter, btn.Height - diameter, diameter, diameter, 0f, 90f);
		path.AddArc(0, btn.Height - diameter, diameter, diameter, 90f, 90f);
		path.CloseFigure();
		btn.Region = new Region(path);
		btn.FlatStyle = FlatStyle.Flat;
		btn.FlatAppearance.BorderSize = 0;
	}

	private void DrawBorder(Button btn, Graphics g)
	{
		int diameter = 20 * 2;
		using GraphicsPath path = new GraphicsPath();
		path.AddArc(0, 0, diameter, diameter, 180f, 90f);
		path.AddArc(btn.Width - diameter, 0, diameter, diameter, 270f, 90f);
		path.AddArc(btn.Width - diameter, btn.Height - diameter, diameter, diameter, 0f, 90f);
		path.AddArc(0, btn.Height - diameter, diameter, diameter, 90f, 90f);
		path.CloseFigure();
		using Pen pen = new Pen(btn.FlatAppearance.BorderColor, 3f);
		g.SmoothingMode = SmoothingMode.AntiAlias;
		g.DrawPath(pen, path);
	}

	private void btnPlaceOrder_Click(object sender, EventArgs e)
	{
		try
		{
			new PreBookingOrder(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnMakePayment_Click(object sender, EventArgs e)
	{
		try
		{
			new AccountsPrematurePaymentsReceipts(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTrackOrder_Click(object sender, EventArgs e)
	{
		try
		{
			frmSaleOrderHistory frmSaleOrderHistory = new frmSaleOrderHistory(UserAccount);
			frmSaleOrderHistory.FromDate = clsGlobalVariables.ActiveYr.Start_Period.Date;
			frmSaleOrderHistory.To_Date = DateTime.Now;
			((Control)(object)frmSaleOrderHistory.CmbOrderStatus).Text = "Open";
			((Control)(object)frmSaleOrderHistory.CmbIsApproved).Text = "All";
			frmSaleOrderHistory.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblOrderSub1Count_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)lblOrderSub1Count.Text) > 0)
			{
				LinksOpeningFromBookingOfficeDashboard obj = new LinksOpeningFromBookingOfficeDashboard(UserAccount);
				obj.SortNo = 1;
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				if (fromdate.Checked)
				{
					obj.FromDate = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ToDate = ToDate.Value;
				}
				obj.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblOrderSub2Count_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)lblOrderSub2Count.Text) > 0)
			{
				LinksOpeningFromBookingOfficeDashboard obj = new LinksOpeningFromBookingOfficeDashboard(UserAccount);
				obj.SortNo = 2;
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				if (fromdate.Checked)
				{
					obj.FromDate = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ToDate = ToDate.Value;
				}
				obj.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblDeliverySub2Count_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)lblDeliverySub2Count.Text) > 0)
			{
				LinksOpeningFromBookingOfficeDashboard obj = new LinksOpeningFromBookingOfficeDashboard(UserAccount);
				obj.SortNo = 4;
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				if (fromdate.Checked)
				{
					obj.FromDate = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ToDate = ToDate.Value;
				}
				obj.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblSalesSub2Count_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)lblSalesSub2Count.Text) > 0)
			{
				LinksOpeningFromBookingOfficeDashboard obj = new LinksOpeningFromBookingOfficeDashboard(UserAccount);
				obj.SortNo = 6;
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				if (fromdate.Checked)
				{
					obj.FromDate = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ToDate = ToDate.Value;
				}
				obj.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblDeliverySub1Count_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)lblDeliverySub1Count.Text) > 0)
			{
				LinksOpeningFromBookingOfficeDashboard obj = new LinksOpeningFromBookingOfficeDashboard(UserAccount);
				obj.SortNo = 3;
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				if (fromdate.Checked)
				{
					obj.FromDate = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ToDate = ToDate.Value;
				}
				obj.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblSalesSub1Count_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)lblSalesSub1Count.Text) > 0)
			{
				LinksOpeningFromBookingOfficeDashboard obj = new LinksOpeningFromBookingOfficeDashboard(UserAccount);
				obj.SortNo = 5;
				obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
				if (fromdate.Checked)
				{
					obj.FromDate = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ToDate = ToDate.Value;
				}
				obj.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblAccountsCount_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)lblAccountsCount.Text) > 0)
			{
				new AccountsPrematurePaymentsReceipts(UserAccount).Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TopItems_Click(object sender, EventArgs e)
	{
		try
		{
			if (TopItemFlowLayout.Controls.Count > 0)
			{
				SalesComparisonReportWithGraph obj = new SalesComparisonReportWithGraph(UserAccount);
				obj.tabControl1.SelectedIndex = 1;
				if (fromdate.Checked)
				{
					obj.FromDate = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ToDate = ToDate.Value;
				}
				obj.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TopPacks_Click(object sender, EventArgs e)
	{
		try
		{
			if (TopPacksFlowLayout.Controls.Count > 0)
			{
				SalesComparisonReportWithGraph obj = new SalesComparisonReportWithGraph(UserAccount);
				obj.tabControl1.SelectedIndex = 3;
				if (fromdate.Checked)
				{
					obj.FromDate = fromdate.Value;
				}
				if (ToDate.Checked)
				{
					obj.ToDate = ToDate.Value;
				}
				obj.Show();
			}
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
			GetData(Conversion.ToInt(CmbCostCenter.Value));
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
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
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
		this.label1 = new System.Windows.Forms.Label();
		this.CmbCostCenter = new UltraCombo();
		this.lblCostCenter = new System.Windows.Forms.Label();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.QuickActionPanel = new System.Windows.Forms.Panel();
		this.BtnMakePayment = new System.Windows.Forms.Button();
		this.btnTrackOrder = new System.Windows.Forms.Button();
		this.btnPlaceOrder = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.OrdersPanel = new System.Windows.Forms.Panel();
		this.lblOrderSub2Count = new System.Windows.Forms.Label();
		this.lblOrderSub1Count = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.OrderSubTitle2 = new System.Windows.Forms.Label();
		this.OrderSubTitle1 = new System.Windows.Forms.Label();
		this.Description = new System.Windows.Forms.Label();
		this.OrderMainTitle = new System.Windows.Forms.Label();
		this.DeliveryPanel = new System.Windows.Forms.Panel();
		this.lblDeliverySub2Count = new System.Windows.Forms.Label();
		this.lblDeliverySub1Count = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.DeliverySubTitle2 = new System.Windows.Forms.Label();
		this.DeliverySubTitle1 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.DeliveryMainTitle = new System.Windows.Forms.Label();
		this.SalesPanel = new System.Windows.Forms.Panel();
		this.lblSalesSub2Count = new System.Windows.Forms.Label();
		this.lblSalesSub1Count = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.SaleSubTitle2 = new System.Windows.Forms.Label();
		this.SaleSubTitle1 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.SaleMainTitle = new System.Windows.Forms.Label();
		this.AccountsPanel = new System.Windows.Forms.Panel();
		this.lblAccountsCount = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.AccountsSubTitle1 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.AccountsMainTitle = new System.Windows.Forms.Label();
		this.HederInfoPanel = new System.Windows.Forms.Panel();
		this.SalesInfoPanel = new System.Windows.Forms.Panel();
		this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
		this.TopItemsPanel = new System.Windows.Forms.Panel();
		this.TopItemFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.label23 = new System.Windows.Forms.Label();
		this.TopItems = new System.Windows.Forms.Label();
		this.TopPacksPanel = new System.Windows.Forms.Panel();
		this.TopPacksFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.label18 = new System.Windows.Forms.Label();
		this.TopPacks = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.label33 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.TopItemsImage = new System.Windows.Forms.PictureBox();
		this.TopPacksImage = new System.Windows.Forms.PictureBox();
		this.QuickActionImage = new System.Windows.Forms.PictureBox();
		this.OrderImage = new System.Windows.Forms.PictureBox();
		this.DeliveryImage = new System.Windows.Forms.PictureBox();
		this.SalesImage = new System.Windows.Forms.PictureBox();
		this.AccountsImage = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		this.flowLayoutPanel1.SuspendLayout();
		this.QuickActionPanel.SuspendLayout();
		this.OrdersPanel.SuspendLayout();
		this.DeliveryPanel.SuspendLayout();
		this.SalesPanel.SuspendLayout();
		this.AccountsPanel.SuspendLayout();
		this.HederInfoPanel.SuspendLayout();
		this.SalesInfoPanel.SuspendLayout();
		this.flowLayoutPanel2.SuspendLayout();
		this.TopItemsPanel.SuspendLayout();
		this.TopPacksPanel.SuspendLayout();
		this.panel1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.TopItemsImage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.TopPacksImage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.QuickActionImage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.OrderImage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.DeliveryImage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.SalesImage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.AccountsImage).BeginInit();
		base.SuspendLayout();
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(967, 29);
		this.label1.TabIndex = 0;
		this.label1.Text = "Booking Office Dashboard";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(507, 14);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 2;
		this.lblCostCenter.AutoSize = true;
		this.lblCostCenter.BackColor = System.Drawing.Color.White;
		this.lblCostCenter.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCostCenter.ForeColor = System.Drawing.Color.Black;
		this.lblCostCenter.Location = new System.Drawing.Point(436, 20);
		this.lblCostCenter.Name = "lblCostCenter";
		this.lblCostCenter.Size = new System.Drawing.Size(67, 15);
		this.lblCostCenter.TabIndex = 151;
		this.lblCostCenter.Text = "Cost Center";
		this.flowLayoutPanel1.Controls.Add(this.QuickActionPanel);
		this.flowLayoutPanel1.Controls.Add(this.OrdersPanel);
		this.flowLayoutPanel1.Controls.Add(this.DeliveryPanel);
		this.flowLayoutPanel1.Controls.Add(this.SalesPanel);
		this.flowLayoutPanel1.Controls.Add(this.AccountsPanel);
		this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(932, 421);
		this.flowLayoutPanel1.TabIndex = 153;
		this.QuickActionPanel.Controls.Add(this.BtnMakePayment);
		this.QuickActionPanel.Controls.Add(this.btnTrackOrder);
		this.QuickActionPanel.Controls.Add(this.btnPlaceOrder);
		this.QuickActionPanel.Controls.Add(this.QuickActionImage);
		this.QuickActionPanel.Controls.Add(this.label2);
		this.QuickActionPanel.Controls.Add(this.label3);
		this.QuickActionPanel.Location = new System.Drawing.Point(5, 5);
		this.QuickActionPanel.Margin = new System.Windows.Forms.Padding(5);
		this.QuickActionPanel.Name = "QuickActionPanel";
		this.QuickActionPanel.Size = new System.Drawing.Size(455, 130);
		this.QuickActionPanel.TabIndex = 154;
		this.BtnMakePayment.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnMakePayment.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
		this.BtnMakePayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnMakePayment.Location = new System.Drawing.Point(221, 75);
		this.BtnMakePayment.Name = "BtnMakePayment";
		this.BtnMakePayment.Size = new System.Drawing.Size(94, 23);
		this.BtnMakePayment.TabIndex = 9;
		this.BtnMakePayment.Text = "Make Payment";
		this.BtnMakePayment.UseVisualStyleBackColor = true;
		this.BtnMakePayment.Click += new System.EventHandler(BtnMakePayment_Click);
		this.btnTrackOrder.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnTrackOrder.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
		this.btnTrackOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnTrackOrder.Location = new System.Drawing.Point(117, 75);
		this.btnTrackOrder.Name = "btnTrackOrder";
		this.btnTrackOrder.Size = new System.Drawing.Size(78, 23);
		this.btnTrackOrder.TabIndex = 8;
		this.btnTrackOrder.Text = "Track Order";
		this.btnTrackOrder.UseVisualStyleBackColor = true;
		this.btnTrackOrder.Click += new System.EventHandler(btnTrackOrder_Click);
		this.btnPlaceOrder.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnPlaceOrder.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
		this.btnPlaceOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPlaceOrder.Location = new System.Drawing.Point(13, 75);
		this.btnPlaceOrder.Name = "btnPlaceOrder";
		this.btnPlaceOrder.Size = new System.Drawing.Size(78, 23);
		this.btnPlaceOrder.TabIndex = 7;
		this.btnPlaceOrder.Text = "Place Order";
		this.btnPlaceOrder.UseVisualStyleBackColor = true;
		this.btnPlaceOrder.Click += new System.EventHandler(btnPlaceOrder_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(11, 34);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(208, 15);
		this.label2.TabIndex = 3;
		this.label2.Text = "Instant Decisions, Seamless Execution";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(11, 11);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(93, 17);
		this.label3.TabIndex = 2;
		this.label3.Text = "Quick Actions";
		this.OrdersPanel.Controls.Add(this.lblOrderSub2Count);
		this.OrdersPanel.Controls.Add(this.lblOrderSub1Count);
		this.OrdersPanel.Controls.Add(this.label25);
		this.OrdersPanel.Controls.Add(this.label6);
		this.OrdersPanel.Controls.Add(this.label5);
		this.OrdersPanel.Controls.Add(this.OrderImage);
		this.OrdersPanel.Controls.Add(this.OrderSubTitle2);
		this.OrdersPanel.Controls.Add(this.OrderSubTitle1);
		this.OrdersPanel.Controls.Add(this.Description);
		this.OrdersPanel.Controls.Add(this.OrderMainTitle);
		this.OrdersPanel.Location = new System.Drawing.Point(470, 5);
		this.OrdersPanel.Margin = new System.Windows.Forms.Padding(5);
		this.OrdersPanel.Name = "OrdersPanel";
		this.OrdersPanel.Size = new System.Drawing.Size(455, 130);
		this.OrdersPanel.TabIndex = 155;
		this.lblOrderSub2Count.AutoSize = true;
		this.lblOrderSub2Count.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblOrderSub2Count.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOrderSub2Count.ForeColor = System.Drawing.Color.MediumBlue;
		this.lblOrderSub2Count.Location = new System.Drawing.Point(262, 98);
		this.lblOrderSub2Count.Name = "lblOrderSub2Count";
		this.lblOrderSub2Count.Size = new System.Drawing.Size(14, 15);
		this.lblOrderSub2Count.TabIndex = 27;
		this.lblOrderSub2Count.Text = "0";
		this.lblOrderSub2Count.Click += new System.EventHandler(lblOrderSub2Count_Click);
		this.lblOrderSub1Count.AutoSize = true;
		this.lblOrderSub1Count.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblOrderSub1Count.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOrderSub1Count.ForeColor = System.Drawing.Color.MediumBlue;
		this.lblOrderSub1Count.Location = new System.Drawing.Point(262, 73);
		this.lblOrderSub1Count.Name = "lblOrderSub1Count";
		this.lblOrderSub1Count.Size = new System.Drawing.Size(14, 15);
		this.lblOrderSub1Count.TabIndex = 26;
		this.lblOrderSub1Count.Text = "0";
		this.lblOrderSub1Count.Click += new System.EventHandler(lblOrderSub1Count_Click);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(184, 98);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(73, 15);
		this.label25.TabIndex = 25;
		this.label25.Text = "......................";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(184, 73);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(73, 15);
		this.label6.TabIndex = 24;
		this.label6.Text = "......................";
		this.label5.BackColor = System.Drawing.Color.FromArgb(47, 146, 227);
		this.label5.Location = new System.Drawing.Point(19, 68);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(6, 50);
		this.label5.TabIndex = 23;
		this.OrderSubTitle2.AutoSize = true;
		this.OrderSubTitle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderSubTitle2.Location = new System.Drawing.Point(32, 98);
		this.OrderSubTitle2.Name = "OrderSubTitle2";
		this.OrderSubTitle2.Size = new System.Drawing.Size(58, 15);
		this.OrderSubTitle2.TabIndex = 20;
		this.OrderSubTitle2.Text = "SubTitle2";
		this.OrderSubTitle1.AutoSize = true;
		this.OrderSubTitle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderSubTitle1.Location = new System.Drawing.Point(32, 73);
		this.OrderSubTitle1.Name = "OrderSubTitle1";
		this.OrderSubTitle1.Size = new System.Drawing.Size(56, 15);
		this.OrderSubTitle1.TabIndex = 19;
		this.OrderSubTitle1.Text = "SubTitle1";
		this.Description.AutoSize = true;
		this.Description.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Description.Location = new System.Drawing.Point(19, 37);
		this.Description.Name = "Description";
		this.Description.Size = new System.Drawing.Size(188, 15);
		this.Description.TabIndex = 18;
		this.Description.Text = "Track and Manage Confirm Orders";
		this.OrderMainTitle.AutoSize = true;
		this.OrderMainTitle.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderMainTitle.Location = new System.Drawing.Point(19, 14);
		this.OrderMainTitle.Name = "OrderMainTitle";
		this.OrderMainTitle.Size = new System.Drawing.Size(36, 17);
		this.OrderMainTitle.TabIndex = 17;
		this.OrderMainTitle.Text = "Title";
		this.DeliveryPanel.Controls.Add(this.lblDeliverySub2Count);
		this.DeliveryPanel.Controls.Add(this.lblDeliverySub1Count);
		this.DeliveryPanel.Controls.Add(this.label28);
		this.DeliveryPanel.Controls.Add(this.label29);
		this.DeliveryPanel.Controls.Add(this.label7);
		this.DeliveryPanel.Controls.Add(this.DeliveryImage);
		this.DeliveryPanel.Controls.Add(this.DeliverySubTitle2);
		this.DeliveryPanel.Controls.Add(this.DeliverySubTitle1);
		this.DeliveryPanel.Controls.Add(this.label10);
		this.DeliveryPanel.Controls.Add(this.DeliveryMainTitle);
		this.DeliveryPanel.Location = new System.Drawing.Point(5, 145);
		this.DeliveryPanel.Margin = new System.Windows.Forms.Padding(5);
		this.DeliveryPanel.Name = "DeliveryPanel";
		this.DeliveryPanel.Size = new System.Drawing.Size(455, 130);
		this.DeliveryPanel.TabIndex = 156;
		this.lblDeliverySub2Count.AutoSize = true;
		this.lblDeliverySub2Count.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblDeliverySub2Count.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDeliverySub2Count.ForeColor = System.Drawing.Color.MediumBlue;
		this.lblDeliverySub2Count.Location = new System.Drawing.Point(265, 96);
		this.lblDeliverySub2Count.Name = "lblDeliverySub2Count";
		this.lblDeliverySub2Count.Size = new System.Drawing.Size(14, 15);
		this.lblDeliverySub2Count.TabIndex = 29;
		this.lblDeliverySub2Count.Text = "0";
		this.lblDeliverySub2Count.Click += new System.EventHandler(lblDeliverySub2Count_Click);
		this.lblDeliverySub1Count.AutoSize = true;
		this.lblDeliverySub1Count.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblDeliverySub1Count.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDeliverySub1Count.ForeColor = System.Drawing.Color.MediumBlue;
		this.lblDeliverySub1Count.Location = new System.Drawing.Point(265, 71);
		this.lblDeliverySub1Count.Name = "lblDeliverySub1Count";
		this.lblDeliverySub1Count.Size = new System.Drawing.Size(14, 15);
		this.lblDeliverySub1Count.TabIndex = 28;
		this.lblDeliverySub1Count.Text = "0";
		this.lblDeliverySub1Count.Click += new System.EventHandler(lblDeliverySub1Count_Click);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(186, 96);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(73, 15);
		this.label28.TabIndex = 27;
		this.label28.Text = "......................";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(186, 71);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(73, 15);
		this.label29.TabIndex = 26;
		this.label29.Text = "......................";
		this.label7.BackColor = System.Drawing.Color.FromArgb(201, 50, 237);
		this.label7.Location = new System.Drawing.Point(19, 66);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(6, 50);
		this.label7.TabIndex = 23;
		this.DeliverySubTitle2.AutoSize = true;
		this.DeliverySubTitle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DeliverySubTitle2.Location = new System.Drawing.Point(32, 96);
		this.DeliverySubTitle2.Name = "DeliverySubTitle2";
		this.DeliverySubTitle2.Size = new System.Drawing.Size(58, 15);
		this.DeliverySubTitle2.TabIndex = 20;
		this.DeliverySubTitle2.Text = "SubTitle2";
		this.DeliverySubTitle1.AutoSize = true;
		this.DeliverySubTitle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DeliverySubTitle1.Location = new System.Drawing.Point(32, 71);
		this.DeliverySubTitle1.Name = "DeliverySubTitle1";
		this.DeliverySubTitle1.Size = new System.Drawing.Size(56, 15);
		this.DeliverySubTitle1.TabIndex = 19;
		this.DeliverySubTitle1.Text = "SubTitle1";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(19, 37);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(201, 15);
		this.label10.TabIndex = 18;
		this.label10.Text = "Monitor and Manage Loading Status";
		this.DeliveryMainTitle.AutoSize = true;
		this.DeliveryMainTitle.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DeliveryMainTitle.Location = new System.Drawing.Point(19, 14);
		this.DeliveryMainTitle.Name = "DeliveryMainTitle";
		this.DeliveryMainTitle.Size = new System.Drawing.Size(36, 17);
		this.DeliveryMainTitle.TabIndex = 17;
		this.DeliveryMainTitle.Text = "Title";
		this.SalesPanel.Controls.Add(this.lblSalesSub2Count);
		this.SalesPanel.Controls.Add(this.lblSalesSub1Count);
		this.SalesPanel.Controls.Add(this.label26);
		this.SalesPanel.Controls.Add(this.label27);
		this.SalesPanel.Controls.Add(this.label12);
		this.SalesPanel.Controls.Add(this.SalesImage);
		this.SalesPanel.Controls.Add(this.SaleSubTitle2);
		this.SalesPanel.Controls.Add(this.SaleSubTitle1);
		this.SalesPanel.Controls.Add(this.label15);
		this.SalesPanel.Controls.Add(this.SaleMainTitle);
		this.SalesPanel.Location = new System.Drawing.Point(470, 145);
		this.SalesPanel.Margin = new System.Windows.Forms.Padding(5);
		this.SalesPanel.Name = "SalesPanel";
		this.SalesPanel.Size = new System.Drawing.Size(455, 130);
		this.SalesPanel.TabIndex = 157;
		this.lblSalesSub2Count.AutoSize = true;
		this.lblSalesSub2Count.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblSalesSub2Count.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSalesSub2Count.ForeColor = System.Drawing.Color.MediumBlue;
		this.lblSalesSub2Count.Location = new System.Drawing.Point(263, 97);
		this.lblSalesSub2Count.Name = "lblSalesSub2Count";
		this.lblSalesSub2Count.Size = new System.Drawing.Size(14, 15);
		this.lblSalesSub2Count.TabIndex = 29;
		this.lblSalesSub2Count.Text = "0";
		this.lblSalesSub2Count.Click += new System.EventHandler(lblSalesSub2Count_Click);
		this.lblSalesSub1Count.AutoSize = true;
		this.lblSalesSub1Count.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblSalesSub1Count.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSalesSub1Count.ForeColor = System.Drawing.Color.MediumBlue;
		this.lblSalesSub1Count.Location = new System.Drawing.Point(263, 72);
		this.lblSalesSub1Count.Name = "lblSalesSub1Count";
		this.lblSalesSub1Count.Size = new System.Drawing.Size(14, 15);
		this.lblSalesSub1Count.TabIndex = 28;
		this.lblSalesSub1Count.Text = "0";
		this.lblSalesSub1Count.Click += new System.EventHandler(lblSalesSub1Count_Click);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(184, 97);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(73, 15);
		this.label26.TabIndex = 27;
		this.label26.Text = "......................";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(184, 72);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(73, 15);
		this.label27.TabIndex = 26;
		this.label27.Text = "......................";
		this.label12.BackColor = System.Drawing.Color.FromArgb(34, 131, 19);
		this.label12.Location = new System.Drawing.Point(19, 67);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(6, 50);
		this.label12.TabIndex = 23;
		this.SaleSubTitle2.AutoSize = true;
		this.SaleSubTitle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SaleSubTitle2.Location = new System.Drawing.Point(32, 97);
		this.SaleSubTitle2.Name = "SaleSubTitle2";
		this.SaleSubTitle2.Size = new System.Drawing.Size(58, 15);
		this.SaleSubTitle2.TabIndex = 20;
		this.SaleSubTitle2.Text = "SubTitle2";
		this.SaleSubTitle1.AutoSize = true;
		this.SaleSubTitle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SaleSubTitle1.Location = new System.Drawing.Point(32, 72);
		this.SaleSubTitle1.Name = "SaleSubTitle1";
		this.SaleSubTitle1.Size = new System.Drawing.Size(56, 15);
		this.SaleSubTitle1.TabIndex = 19;
		this.SaleSubTitle1.Text = "SubTitle1";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(19, 37);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(204, 15);
		this.label15.TabIndex = 18;
		this.label15.Text = "Manage Sales, Track Sales Effortlessly";
		this.SaleMainTitle.AutoSize = true;
		this.SaleMainTitle.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SaleMainTitle.Location = new System.Drawing.Point(19, 14);
		this.SaleMainTitle.Name = "SaleMainTitle";
		this.SaleMainTitle.Size = new System.Drawing.Size(36, 17);
		this.SaleMainTitle.TabIndex = 17;
		this.SaleMainTitle.Text = "Title";
		this.AccountsPanel.Controls.Add(this.lblAccountsCount);
		this.AccountsPanel.Controls.Add(this.label30);
		this.AccountsPanel.Controls.Add(this.label17);
		this.AccountsPanel.Controls.Add(this.AccountsImage);
		this.AccountsPanel.Controls.Add(this.AccountsSubTitle1);
		this.AccountsPanel.Controls.Add(this.label20);
		this.AccountsPanel.Controls.Add(this.AccountsMainTitle);
		this.AccountsPanel.Location = new System.Drawing.Point(5, 285);
		this.AccountsPanel.Margin = new System.Windows.Forms.Padding(5);
		this.AccountsPanel.Name = "AccountsPanel";
		this.AccountsPanel.Size = new System.Drawing.Size(455, 130);
		this.AccountsPanel.TabIndex = 158;
		this.lblAccountsCount.AutoSize = true;
		this.lblAccountsCount.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblAccountsCount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAccountsCount.ForeColor = System.Drawing.Color.MediumBlue;
		this.lblAccountsCount.Location = new System.Drawing.Point(265, 84);
		this.lblAccountsCount.Name = "lblAccountsCount";
		this.lblAccountsCount.Size = new System.Drawing.Size(14, 15);
		this.lblAccountsCount.TabIndex = 30;
		this.lblAccountsCount.Text = "0";
		this.lblAccountsCount.Click += new System.EventHandler(lblAccountsCount_Click);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(186, 84);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(73, 15);
		this.label30.TabIndex = 28;
		this.label30.Text = "......................";
		this.label17.BackColor = System.Drawing.Color.FromArgb(91, 204, 235);
		this.label17.Location = new System.Drawing.Point(19, 67);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(6, 50);
		this.label17.TabIndex = 23;
		this.AccountsSubTitle1.AutoSize = true;
		this.AccountsSubTitle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.AccountsSubTitle1.Location = new System.Drawing.Point(32, 84);
		this.AccountsSubTitle1.Name = "AccountsSubTitle1";
		this.AccountsSubTitle1.Size = new System.Drawing.Size(56, 15);
		this.AccountsSubTitle1.TabIndex = 19;
		this.AccountsSubTitle1.Text = "SubTitle1";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(19, 37);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(217, 15);
		this.label20.TabIndex = 18;
		this.label20.Text = "Manage and Track Accounts Effortlessly";
		this.AccountsMainTitle.AutoSize = true;
		this.AccountsMainTitle.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.AccountsMainTitle.Location = new System.Drawing.Point(19, 14);
		this.AccountsMainTitle.Name = "AccountsMainTitle";
		this.AccountsMainTitle.Size = new System.Drawing.Size(36, 17);
		this.AccountsMainTitle.TabIndex = 17;
		this.AccountsMainTitle.Text = "Title";
		this.HederInfoPanel.Controls.Add(this.flowLayoutPanel1);
		this.HederInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.HederInfoPanel.Location = new System.Drawing.Point(0, 74);
		this.HederInfoPanel.Name = "HederInfoPanel";
		this.HederInfoPanel.Size = new System.Drawing.Size(967, 421);
		this.HederInfoPanel.TabIndex = 154;
		this.SalesInfoPanel.Controls.Add(this.flowLayoutPanel2);
		this.SalesInfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.SalesInfoPanel.Location = new System.Drawing.Point(0, 495);
		this.SalesInfoPanel.Name = "SalesInfoPanel";
		this.SalesInfoPanel.Size = new System.Drawing.Size(967, 190);
		this.SalesInfoPanel.TabIndex = 155;
		this.flowLayoutPanel2.Controls.Add(this.TopItemsPanel);
		this.flowLayoutPanel2.Controls.Add(this.TopPacksPanel);
		this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
		this.flowLayoutPanel2.Name = "flowLayoutPanel2";
		this.flowLayoutPanel2.Size = new System.Drawing.Size(932, 190);
		this.flowLayoutPanel2.TabIndex = 0;
		this.TopItemsPanel.Controls.Add(this.TopItemFlowLayout);
		this.TopItemsPanel.Controls.Add(this.TopItemsImage);
		this.TopItemsPanel.Controls.Add(this.label23);
		this.TopItemsPanel.Controls.Add(this.TopItems);
		this.TopItemsPanel.Location = new System.Drawing.Point(5, 5);
		this.TopItemsPanel.Margin = new System.Windows.Forms.Padding(5);
		this.TopItemsPanel.Name = "TopItemsPanel";
		this.TopItemsPanel.Size = new System.Drawing.Size(455, 180);
		this.TopItemsPanel.TabIndex = 159;
		this.TopItemFlowLayout.Location = new System.Drawing.Point(3, 58);
		this.TopItemFlowLayout.Name = "TopItemFlowLayout";
		this.TopItemFlowLayout.Size = new System.Drawing.Size(447, 116);
		this.TopItemFlowLayout.TabIndex = 23;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(19, 37);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(156, 15);
		this.label23.TabIndex = 18;
		this.label23.Text = "Popular Picks High Demand";
		this.TopItems.AutoSize = true;
		this.TopItems.Cursor = System.Windows.Forms.Cursors.Hand;
		this.TopItems.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TopItems.ForeColor = System.Drawing.Color.MediumBlue;
		this.TopItems.Location = new System.Drawing.Point(19, 14);
		this.TopItems.Name = "TopItems";
		this.TopItems.Size = new System.Drawing.Size(115, 17);
		this.TopItems.TabIndex = 17;
		this.TopItems.Text = "Top Traded items";
		this.TopItems.Click += new System.EventHandler(TopItems_Click);
		this.TopPacksPanel.Controls.Add(this.TopPacksFlowLayout);
		this.TopPacksPanel.Controls.Add(this.TopPacksImage);
		this.TopPacksPanel.Controls.Add(this.label18);
		this.TopPacksPanel.Controls.Add(this.TopPacks);
		this.TopPacksPanel.Location = new System.Drawing.Point(470, 5);
		this.TopPacksPanel.Margin = new System.Windows.Forms.Padding(5);
		this.TopPacksPanel.Name = "TopPacksPanel";
		this.TopPacksPanel.Size = new System.Drawing.Size(455, 180);
		this.TopPacksPanel.TabIndex = 160;
		this.TopPacksFlowLayout.Location = new System.Drawing.Point(3, 58);
		this.TopPacksFlowLayout.Name = "TopPacksFlowLayout";
		this.TopPacksFlowLayout.Size = new System.Drawing.Size(447, 116);
		this.TopPacksFlowLayout.TabIndex = 23;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(19, 37);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(166, 15);
		this.label18.TabIndex = 18;
		this.label18.Text = "Effect Quantity , Smart Choice";
		this.TopPacks.AutoSize = true;
		this.TopPacks.Cursor = System.Windows.Forms.Cursors.Hand;
		this.TopPacks.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TopPacks.ForeColor = System.Drawing.Color.MediumBlue;
		this.TopPacks.Location = new System.Drawing.Point(19, 14);
		this.TopPacks.Name = "TopPacks";
		this.TopPacks.Size = new System.Drawing.Size(97, 17);
		this.TopPacks.TabIndex = 17;
		this.TopPacks.Text = "Top Pack Sizes";
		this.TopPacks.Click += new System.EventHandler(TopPacks_Click);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 29);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(967, 45);
		this.panel1.TabIndex = 156;
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.groupBox1.Controls.Add(this.lblCostCenter);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.ToDate);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(925, 45);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnShow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShow.Location = new System.Drawing.Point(709, 14);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(57, 26);
		this.btnShow.TabIndex = 3;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(8, 20);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(63, 15);
		this.label33.TabIndex = 136;
		this.label33.Text = "From Date";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Checked = false;
		this.fromdate.CustomFormat = "dd-MMM-yyyy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(75, 16);
		this.fromdate.Name = "fromdate";
		this.fromdate.ShowCheckBox = true;
		this.fromdate.Size = new System.Drawing.Size(149, 23);
		this.fromdate.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(228, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 137;
		this.label4.Text = "To Date";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yyyy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(280, 16);
		this.ToDate.Name = "ToDate";
		this.ToDate.ShowCheckBox = true;
		this.ToDate.Size = new System.Drawing.Size(150, 23);
		this.ToDate.TabIndex = 1;
		this.TopItemsImage.Image = Architecture.WinApp.Properties.Resources.TopTradedItemsCardIcon;
		this.TopItemsImage.Location = new System.Drawing.Point(322, 12);
		this.TopItemsImage.Name = "TopItemsImage";
		this.TopItemsImage.Size = new System.Drawing.Size(40, 40);
		this.TopItemsImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.TopItemsImage.TabIndex = 22;
		this.TopItemsImage.TabStop = false;
		this.TopPacksImage.Image = Architecture.WinApp.Properties.Resources.TopPacksCardIcon;
		this.TopPacksImage.Location = new System.Drawing.Point(322, 12);
		this.TopPacksImage.Name = "TopPacksImage";
		this.TopPacksImage.Size = new System.Drawing.Size(40, 40);
		this.TopPacksImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.TopPacksImage.TabIndex = 22;
		this.TopPacksImage.TabStop = false;
		this.QuickActionImage.Image = Architecture.WinApp.Properties.Resources.QuickActionCardIcon;
		this.QuickActionImage.Location = new System.Drawing.Point(325, 11);
		this.QuickActionImage.Name = "QuickActionImage";
		this.QuickActionImage.Size = new System.Drawing.Size(40, 40);
		this.QuickActionImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.QuickActionImage.TabIndex = 6;
		this.QuickActionImage.TabStop = false;
		this.OrderImage.Image = Architecture.WinApp.Properties.Resources.OrderCardIcon;
		this.OrderImage.Location = new System.Drawing.Point(322, 12);
		this.OrderImage.Name = "OrderImage";
		this.OrderImage.Size = new System.Drawing.Size(40, 40);
		this.OrderImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.OrderImage.TabIndex = 22;
		this.OrderImage.TabStop = false;
		this.DeliveryImage.Image = Architecture.WinApp.Properties.Resources.DeliveryCardIcon;
		this.DeliveryImage.Location = new System.Drawing.Point(322, 12);
		this.DeliveryImage.Name = "DeliveryImage";
		this.DeliveryImage.Size = new System.Drawing.Size(40, 40);
		this.DeliveryImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.DeliveryImage.TabIndex = 22;
		this.DeliveryImage.TabStop = false;
		this.SalesImage.Image = Architecture.WinApp.Properties.Resources.SaleIconCard;
		this.SalesImage.Location = new System.Drawing.Point(322, 12);
		this.SalesImage.Name = "SalesImage";
		this.SalesImage.Size = new System.Drawing.Size(40, 40);
		this.SalesImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.SalesImage.TabIndex = 22;
		this.SalesImage.TabStop = false;
		this.AccountsImage.Image = Architecture.WinApp.Properties.Resources.AccountsCardIcon;
		this.AccountsImage.Location = new System.Drawing.Point(322, 12);
		this.AccountsImage.Name = "AccountsImage";
		this.AccountsImage.Size = new System.Drawing.Size(40, 40);
		this.AccountsImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.AccountsImage.TabIndex = 22;
		this.AccountsImage.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(967, 807);
		base.Controls.Add(this.SalesInfoPanel);
		base.Controls.Add(this.HederInfoPanel);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.label1);
		base.Name = "BookingOfficeDashboard";
		this.Text = "Booking Office Dashboard";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(BookingOfficeDashboard_FormClosed);
		base.Load += new System.EventHandler(BookingOfficeDashboard_Load);
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		this.flowLayoutPanel1.ResumeLayout(false);
		this.QuickActionPanel.ResumeLayout(false);
		this.QuickActionPanel.PerformLayout();
		this.OrdersPanel.ResumeLayout(false);
		this.OrdersPanel.PerformLayout();
		this.DeliveryPanel.ResumeLayout(false);
		this.DeliveryPanel.PerformLayout();
		this.SalesPanel.ResumeLayout(false);
		this.SalesPanel.PerformLayout();
		this.AccountsPanel.ResumeLayout(false);
		this.AccountsPanel.PerformLayout();
		this.HederInfoPanel.ResumeLayout(false);
		this.SalesInfoPanel.ResumeLayout(false);
		this.flowLayoutPanel2.ResumeLayout(false);
		this.TopItemsPanel.ResumeLayout(false);
		this.TopItemsPanel.PerformLayout();
		this.TopPacksPanel.ResumeLayout(false);
		this.TopPacksPanel.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.TopItemsImage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.TopPacksImage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.QuickActionImage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.OrderImage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.DeliveryImage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.SalesImage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.AccountsImage).EndInit();
		base.ResumeLayout(false);
	}
}
