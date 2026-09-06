using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model.Export;
using Architecture.Model.lgstcm;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.ModernHistories;
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
namespace Architecture.WinApp.LogisticManagement.FromWise_Helper_Methods
{

public class LogisticsServicesBill_Helper
{
	private static PreviewToolTipCard invoicePreviewCard;

	private static Form invoicePreviewForm;

	private static Timer hideTimer;

	public static void InitializeDetailtable(DataTable dtCustomerDetail)
	{
		if (dtCustomerDetail.Columns.Count == 0)
		{
			dtCustomerDetail.Columns.Add("Id", typeof(int));
			dtCustomerDetail.Columns.Add("ChargesTypeId", typeof(int));
			dtCustomerDetail.Columns.Add("AgreementHeaderId", typeof(int));
			dtCustomerDetail.Columns.Add("AgreementDetailId", typeof(int));
			dtCustomerDetail.Columns.Add("AgreementNo", typeof(int));
			dtCustomerDetail.Columns.Add("PurchaseOrderHeaderId", typeof(int));
			dtCustomerDetail.Columns.Add("PurchaseOrderDetailId", typeof(int));
			dtCustomerDetail.Columns.Add("PurchaseOrderNo", typeof(int));
			dtCustomerDetail.Columns.Add("PartyRefDocNo", typeof(string));
			dtCustomerDetail.Columns.Add("PartyRefDocDate", typeof(DateTime));
			dtCustomerDetail.Columns.Add("ItemId", typeof(int));
			dtCustomerDetail.Columns.Add("ServiceItemCode");
			dtCustomerDetail.Columns.Add("ServiceItemName");
			dtCustomerDetail.Columns.Add("TransactionCurrencyId", typeof(int));
			dtCustomerDetail.Columns.Add("TransactionCurrency", typeof(string));
			dtCustomerDetail.Columns.Add("TransactionExchangeRate", typeof(decimal));
			dtCustomerDetail.Columns.Add("ServiceRate", typeof(decimal));
			dtCustomerDetail.Columns.Add("RateBaseId", typeof(int));
			dtCustomerDetail.Columns.Add("RateBase", typeof(string));
			dtCustomerDetail.Columns.Add("RateBaseEquivalent", typeof(decimal));
			dtCustomerDetail.Columns.Add("Qty", typeof(decimal));
			dtCustomerDetail.Columns.Add("NetWeight", typeof(decimal));
			dtCustomerDetail.Columns.Add("TcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("LcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("TaxNameId", typeof(int));
			dtCustomerDetail.Columns.Add("TaxName", typeof(string));
			dtCustomerDetail.Columns.Add("Tax%", typeof(decimal));
			dtCustomerDetail.Columns.Add("TaxAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("TotalTcyAmount", typeof(decimal));
			dtCustomerDetail.Columns.Add("DebitAccountId", typeof(int));
			dtCustomerDetail.Columns.Add("LocationTypeId", typeof(int));
			dtCustomerDetail.Columns.Add("LocationType", typeof(string));
			dtCustomerDetail.Columns.Add("LocationFromId", typeof(int));
			dtCustomerDetail.Columns.Add("LocationFrom", typeof(string));
			dtCustomerDetail.Columns.Add("LocationToId", typeof(int));
			dtCustomerDetail.Columns.Add("LocationTo", typeof(string));
			dtCustomerDetail.Columns.Add("Remarks");
		}
	}

	public static void InitializeFreightvoucherDetailtable(DataTable dtGrid)
	{
		if (dtGrid.Columns.Count == 0)
		{
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("FreightVoucherOutwardId", typeof(int));
			dtGrid.Columns.Add("GatePassOutwardId", typeof(int));
			dtGrid.Columns.Add("DeliveryOrderId", typeof(int));
			dtGrid.Columns.Add("Qty", typeof(double));
			dtGrid.Columns.Add("Weight", typeof(double));
			dtGrid.Columns.Add("FreightRate", typeof(double));
			dtGrid.Columns.Add("RateBaseUomId", typeof(int));
			dtGrid.Columns.Add("RateBaseUom", typeof(string));
			dtGrid.Columns.Add("QtyForRate", typeof(double));
			dtGrid.Columns.Add("BiltyFreight", typeof(double));
			dtGrid.Columns.Add("OtherCharges", typeof(double));
			dtGrid.Columns.Add("TotalBiltyFreight", typeof(double));
			dtGrid.Columns.Add("ThisRowFreightAmount", typeof(double));
			dtGrid.Columns.Add("Remarks", typeof(string));
		}
	}

	public static void FillDetailFromListCommonForReadById(ServicesBillHeader invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (((invGrn != null) ? invGrn.ServicesBillDetailList : null) == null || invGrn.ServicesBillDetailList.Count == 0)
		{
			return;
		}
		foreach (ServicesBillDetail detail in invGrn.ServicesBillDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.ServicesBillDetailId;
			dr["ChargesTypeId"] = detail.ChargesTypeId;
			dr["AgreementHeaderId"] = detail.AgreementHeaderId;
			dr["AgreementDetailId"] = detail.AgreementDetailId;
			dr["AgreementNo"] = detail.AgreementDocNo;
			dr["PurchaseOrderHeaderId"] = detail.PurchaseOrderHeaderId;
			dr["PurchaseOrderDetailId"] = detail.PurchaseOrderDetailId;
			dr["PurchaseOrderNo"] = detail.PurchaseOrderDocNo;
			dr["PartyRefDocNo"] = detail.partyRefDocNo;
			dr["PartyRefDocDate"] = detail.partyRefDocDate;
			dr["ItemId"] = detail.serviceItemId;
			dr["ServiceItemCode"] = detail.ItemCode;
			dr["ServiceItemName"] = detail.ItemName;
			dr["TransactionCurrencyId"] = detail.transactionCurrencyId;
			dr["TransactionCurrency"] = detail.TransactionCurrencyCode;
			dr["TransactionExchangeRate"] = detail.transactionExchangeRate;
			dr["ServiceRate"] = detail.ServiceRate;
			dr["RateBaseId"] = detail.RateBaseUomId;
			dr["RateBase"] = detail.RateBaseUom;
			dr["RateBaseEquivalent"] = detail.RateBaseUomEquivalent;
			dr["Qty"] = detail.Qty;
			dr["NetWeight"] = detail.NetWeight;
			dr["TcyAmount"] = detail.tcyAmount;
			dr["LcyAmount"] = detail.lcyAmount;
			dr["TaxNameId"] = detail.TaxNameId;
			dr["TaxName"] = detail.TaxName;
			dr["Tax%"] = detail.TaxPercent;
			dr["TaxAmount"] = detail.TaxAmount;
			dr["TotalTcyAmount"] = detail.TotalAmount;
			dr["DebitAccountId"] = detail.DebitAccountId;
			dr["LocationTypeId"] = detail.LocationTypeId;
			dr["LocationType"] = detail.LocationType;
			dr["LocationFromId"] = ((detail.LocationTypeId == 16) ? detail.LoadingPortId : detail.FromCityId);
			dr["LocationFrom"] = ((detail.LocationTypeId == 16) ? detail.LoadingPort : detail.FromCity);
			dr["LocationToId"] = ((detail.LocationTypeId == 16) ? detail.DestinationPortId : detail.ToCityId);
			dr["LocationTo"] = ((detail.LocationTypeId == 16) ? detail.DestinationPort : detail.ToCity);
			dr["Remarks"] = detail.Remarks;
			if (AddExtraColumns)
			{
				dr["DebitAccountTitle"] = detail.DebitAccountTitle;
			}
			dt.Rows.Add(dr);
		}
	}

	public static void FillFreightVoucherDetailFromListCommonForReadById(ServicesBillHeader invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.ServicesBillFreightVoucherDetailList : null) == null || invGrn.ServicesBillFreightVoucherDetailList.Count == 0)
		{
			return;
		}
		foreach (ServicesBillFreightVoucherDetail detail in invGrn.ServicesBillFreightVoucherDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.ServicesBillFreightVoucherDetailId;
			dr["FreightVoucherOutwardId"] = detail.FreightVoucherOutwardId;
			dr["GatePassOutwardId"] = detail.GatePassOutwardId;
			dr["DeliveryOrderId"] = detail.DeliveryOrderId;
			dr["Qty"] = detail.Qty;
			dr["Weight"] = detail.Weight;
			dr["FreightRate"] = detail.FreightRate;
			dr["RateBaseUomId"] = detail.RateBaseUomId;
			dr["QtyForRate"] = detail.QtyForRate;
			dr["BiltyFreight"] = detail.BiltyFreight;
			dr["OtherCharges"] = detail.OtherCharges;
			dr["TotalBiltyFreight"] = detail.TotalBiltyFreight;
			dr["ThisRowFreightAmount"] = detail.ThisRowFreightAmount;
			dr["Remarks"] = detail.Remarks;
			dt.Rows.Add(dr);
		}
	}

	public static void DetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "AgreementHeaderId", "ChargesTypeId", "AgreementDetailId", "PurchaseOrderHeaderId", "PurchaseOrderDetailId", "ItemId", "TransactionCurrencyId", "RateBaseId", "RateBaseEquivalent",
				"TaxNameId", "LocationTypeId", "LocationFromId", "LocationToId"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
			GridEX_Helper.AddConditionalFormatting(gridEX, "PurchaseOrderNo", new List<(ConditionOperator, object, Color, bool)>
			{
				((ConditionOperator)12, 0, Color.Red, true),
				((ConditionOperator)0, 0, Color.Red, true)
			});
			gridEX.RootTable.Columns["AgreementNo"].Width = 80;
			gridEX.RootTable.Columns["PurchaseOrderNo"].Width = 75;
			gridEX.RootTable.Columns["PartyRefDocNo"].Width = 100;
			gridEX.RootTable.Columns["PartyRefDocDate"].Width = 80;
			gridEX.RootTable.Columns["ServiceItemName"].Width = 160;
			gridEX.RootTable.Columns["ServiceItemCode"].Width = 100;
			gridEX.RootTable.Columns["TransactionCurrency"].Width = 80;
			gridEX.RootTable.Columns["TransactionExchangeRate"].Width = 80;
			gridEX.RootTable.Columns["ServiceRate"].Width = 80;
			gridEX.RootTable.Columns["RateBase"].Width = 120;
			gridEX.RootTable.Columns["Qty"].Width = 70;
			gridEX.RootTable.Columns["NetWeight"].Width = 90;
			gridEX.RootTable.Columns["TaxName"].Width = 90;
			gridEX.RootTable.Columns["Tax%"].Width = 65;
			gridEX.RootTable.Columns["TaxAmount"].Width = 90;
			gridEX.RootTable.Columns["TcyAmount"].Width = 90;
			gridEX.RootTable.Columns["LcyAmount"].Width = 90;
			gridEX.RootTable.Columns["TotalTcyAmount"].Width = 100;
			gridEX.RootTable.Columns["Remarks"].Width = 180;
			gridEX.RootTable.Columns["LocationType"].Width = 80;
			gridEX.RootTable.Columns["LocationFrom"].Width = 100;
			gridEX.RootTable.Columns["LocationTo"].Width = 100;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ShowInvoiceInfo(UltraCombo CmbInvoiceNo)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int invoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			if (invoiceId > 0)
			{
				DataTable dt = ExImInvoice.GetInvoiceInformationByInvoiceIdForServices(new ExImInvoice
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					Id = invoiceId
				});
				if (dt.Rows.Count > 0)
				{
					DataRow dataRow = dt.Rows[0];
					string customerName = Convert.ToString(dataRow["CustomerName"]);
					string forwarderName = Convert.ToString(dataRow["ForwarderName"]);
					string contractNo = Convert.ToString(dataRow["LcOrderNo"]);
					string blNo = Convert.ToString(dataRow["BLNumber"]);
					string gdNo = Convert.ToString(dataRow["GdNo"]);
					string loadingPort = Convert.ToString(dataRow["LoadingPort"]);
					string destinationPort = Convert.ToString(dataRow["DestinationPort"]);
					string dispatchedPort = Convert.ToString(dataRow["ContainerDispatchedAtPort"]);
					string receivedPort = Convert.ToString(dataRow["ContainerReceivedFromPort"]);
					string deliveredAt = Convert.ToString(dataRow["DeliveredAt"]);
					string vesselName = Convert.ToString(dataRow["VesselName"]);
					string incoTerm = Convert.ToString(dataRow["DeliveryTerm"]);
					string noOfContainers = Convert.ToString(dataRow["NoOfContainers"]);
					double mTon = Convert.ToDouble(dataRow["NetWeight"]) / 1000.0;
					double bookingRate = Convert.ToDouble(dataRow["BookingRate"]);
					invoicePreviewForm?.Dispose();
					invoicePreviewCard = new PreviewToolTipCard();
					invoicePreviewCard.FlpMain.BringToFront();
					invoicePreviewCard.FlpMain.SuspendLayout();
					invoicePreviewCard.FlpMain.Controls.Clear();
					invoicePreviewCard.labelCaption.Text = "Invoice Preview — " + ((Control)(object)CmbInvoiceNo).Text;
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Customer Name",
						ValueControl = new TextBox
						{
							Text = customerName
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Forwarder Name",
						ValueControl = new TextBox
						{
							Text = forwarderName
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "LC / Contract",
						ValueControl = new TextBox
						{
							Text = contractNo
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "BL No",
						ValueControl = new TextBox
						{
							Text = blNo
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "GD No",
						ValueControl = new TextBox
						{
							Text = gdNo,
							ReadOnly = true
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Loading Place",
						ValueControl = new TextBox
						{
							Text = loadingPort,
							ReadOnly = true
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Destination Place",
						ValueControl = new TextBox
						{
							Text = destinationPort,
							ReadOnly = true
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Container Dispatched At",
						ValueControl = new TextBox
						{
							Text = dispatchedPort,
							ReadOnly = true
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Container Received From",
						ValueControl = new TextBox
						{
							Text = receivedPort,
							ReadOnly = true
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Delivered At",
						ValueControl = new TextBox
						{
							Text = deliveredAt,
							ReadOnly = true
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Vessel Name",
						ValueControl = new TextBox
						{
							Text = vesselName
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Income Term",
						ValueControl = new TextBox
						{
							Text = incoTerm,
							ReadOnly = true
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "No Of Containers",
						ValueControl = new TextBox
						{
							Text = noOfContainers,
							ReadOnly = true
						}
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "M.Ton",
						ValueControl = new TextBox
						{
							Text = mTon.ToString("N2")
						},
						IsHighlight = true
					});
					ApprovalFormHelper.AddPreviewField(invoicePreviewCard.FlpMain, new PreviewToolTipField
					{
						Caption = "Booking Rate",
						ValueControl = new TextBox
						{
							Text = bookingRate.ToString("N2")
						},
						IsHighlight = true
					});
					invoicePreviewCard.FlpMain.ResumeLayout();
					invoicePreviewCard.FlpMain.AutoScroll = false;
					invoicePreviewForm = new Form
					{
						FormBorderStyle = FormBorderStyle.None,
						StartPosition = FormStartPosition.Manual,
						TopMost = true,
						ShowInTaskbar = false,
						AutoScroll = false,
						BackColor = Color.White,
						Padding = new Padding(1)
					};
					invoicePreviewForm.Controls.Add(invoicePreviewCard);
					invoicePreviewCard.Dock = DockStyle.Fill;
					Point screenPoint = ((Control)(object)CmbInvoiceNo).PointToScreen(new Point(0, ((Control)(object)CmbInvoiceNo).Height));
					invoicePreviewForm.Location = new Point(screenPoint.X, screenPoint.Y);
					invoicePreviewForm.MouseLeave += InvoicePreviewCard_MouseLeave;
					invoicePreviewCard.MouseLeave += InvoicePreviewCard_MouseLeave;
					invoicePreviewForm.Width = 920;
					invoicePreviewForm.Height = 200;
					ApprovalFormHelper.FadeInCard(invoicePreviewForm);
				}
				else
				{
					invoicePreviewForm?.Hide();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading invoice info: " + ex.Message);
		}
	}

	private static void InvoicePreviewCard_MouseLeave(object sender, EventArgs e)
	{
		StartHidePreviewTimer();
	}

	public static void StartHidePreviewTimer()
	{
		if (hideTimer == null)
		{
			hideTimer = new Timer();
			hideTimer.Interval = 300;
			hideTimer.Tick += delegate
			{
				hideTimer.Stop();
				hideTimer.Dispose();
				hideTimer = null;
				invoicePreviewForm?.Hide();
				invoicePreviewForm?.Dispose();
				invoicePreviewForm = null;
				invoicePreviewCard = null;
			};
		}
		hideTimer.Start();
	}
}

}
