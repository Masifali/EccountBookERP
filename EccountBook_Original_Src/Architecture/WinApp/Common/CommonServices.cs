using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Authentication;
using Architecture.BLL.BankSchema;
using Architecture.BLL.ChequePrinting;
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.BLL.Common;
using Architecture.BLL.ContractorWages;
using Architecture.BLL.Export;
using Architecture.BLL.FeedMill.DiscountPolicy;
using Architecture.BLL.FeedMill.Production;
using Architecture.BLL.FeedMill.Purchase;
using Architecture.BLL.FeedMill.Sales;
using Architecture.BLL.HRM.OverTimeManagement;
using Architecture.BLL.Import;
using Architecture.BLL.Import.Transaction;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.BLL.Mfg;
using Architecture.BLL.PartyProcessing;
using Architecture.BLL.Pos;
using Architecture.BLL.Production;
using Architecture.BLL.PurchaseTrading;
using Architecture.BLL.Reports.Accounts;
using Architecture.BLL.Reports.Export;
using Architecture.BLL.Reports.General;
using Architecture.BLL.Reports.Inventory;
using Architecture.BLL.Reports.Party_Processing;
using Architecture.BLL.Reports.Production;
using Architecture.BLL.Reports.Services;
using Architecture.BLL.Salt;
using Architecture.BLL.Steel.Inventory;
using Architecture.BLL.Steel.Production;
using Architecture.BLL.SystemUtilities;
using Architecture.BLL.WeightBridge;
using Architecture.BLL.pcc;
using Architecture.BLL.pcc.Production;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Common;
using Architecture.Model.ContractorWages;
using Architecture.Model.DAW;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Model.Lab;
using Architecture.Model.Main;
using Architecture.Model.Mfg;
using Architecture.Model.PartyProcessing;
using Architecture.Model.PurchaseTrading;
using Architecture.Model.Steel.Inventory;
using Architecture.Model.VPS;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Account_Definition.VouchersWithTax;
using Architecture.WinApp.Account_Reports;
using Architecture.WinApp.Bank;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Stocks_Report;
using CameraControl;
using EDSDKLib;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using QRCoder;

namespace Architecture.WinApp.Common;

public class CommonServices
{
	private static readonly char[] AlphabetsArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

	private static readonly char[] NumbersArray = "0123456789".ToCharArray();

	public static DataTable CreateDropDownTable()
	{
		return new DataTable
		{
			Columns = 
			{
				{
					"Id",
					typeof(int)
				},
				{
					"name",
					typeof(string)
				}
			}
		};
	}

	public static DataTable GetIdsOfDocumentsForAutoUpdation(int OrganizationId, int CompanyId, int DocumentTypeId, int ProductionTypeId = 0)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ProductionTypeId",
				Value = ProductionTypeId
			});
			return GenericProvider.GetDataTableProc<InvSaleInvoice>("[dbo].[USP_GetIdsOfDocumentsForAutoUpdation]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DatelockValidationAndStatusOpenForCgsRun(int OrganizationId, int CompanyId, DateTime FromDate, DateTime ToDate)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FromDate",
				Value = FromDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ToDate",
				Value = ToDate
			});
			GenericProvider.GetDataTableProc<InvSaleInvoice>("[dbo].[usp_DatelockValidationAndStatusOpenForCgsRun]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetIdsOfDocumentsDateWiseForAutoUpdation(int OrganizationId, int CompanyId, int DocumentTypeId, DateTime docdate)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = docdate
			});
			return GenericProvider.GetDataTableProc<InvSaleInvoice>("[dbo].[USP_GetIdsOfDocumentsForAutoUpdation]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByTypeIdFromGlobalLookUpEngr(int LookUpTypeId)
	{
		DataTable dt = new DataTable();
		try
		{
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("LookupName", typeof(string));
			List<LookUps> filteredList = clsGlobalVariables.GetAllLookUps_Engr?.Where((LookUps r) => r.LookUpTypeId == LookUpTypeId).ToList();
			if (filteredList != null)
			{
				foreach (LookUps item in filteredList)
				{
					dt.Rows.Add(item.Id, item.LookupName);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return dt;
	}

	public static string GenerateCustomFileName()
	{
		string name = "";
		name += $"{clsGlobalVariables.UserAccount.OrganizationId}_{clsGlobalVariables.UserAccount.CompanyId}_";
		DateTime newDate = DateTime.Now;
		name += $"{newDate.Year}_{newDate.Month}_{newDate.Day}_";
		Random random = new Random();
		for (int i = 0; i < 15; i++)
		{
			name += AlphabetsArray[GetRandomInt(0, AlphabetsArray.Length - 1, random)];
			name += NumbersArray[GetRandomInt(0, NumbersArray.Length - 1, random)];
		}
		return name;
	}

	private static int GetRandomInt(int min, int max, Random random)
	{
		return random.Next(min, max + 1);
	}

	public static void SetComboValue<T>(UltraCombo combo, string valueMember, T id, DataTable dt, bool ActivateRow = false, int ActivateRowIndex = 0)
	{
		try
		{
			if (EqualityComparer<T>.Default.Equals(id, default(T)))
			{
				if (ActivateRow)
				{
					((UltraGridBase)combo).Rows[ActivateRowIndex].Activate();
				}
				else
				{
					((Control)(object)combo).Text = string.Empty;
				}
			}
			else if (dt.AsEnumerable().Any((DataRow row) => EqualityComparer<T>.Default.Equals(row.Field<T>(valueMember), id)))
			{
				combo.Value = id;
			}
			else if (ActivateRow)
			{
				((UltraGridBase)combo).Rows[ActivateRowIndex].Activate();
			}
			else
			{
				((Control)(object)combo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateRelatedFieldsCalculation(TextBox textBox1, TextBox textBox2, double factor, Form form, int decimalPointsForTextBox1 = 0, int factor2 = 1, string formatPattern = "#,##0.###", int decimalPointsForTextBox2 = 0)
	{
		try
		{
			if (textBox1 != null && textBox2 != null && form != null)
			{
				double value1 = Conversion.ToDouble((object)textBox1.Text);
				double value2 = Conversion.ToDouble((object)textBox2.Text);
				double calcValue = 0.0;
				string activeTag = Convert.ToString(form.ActiveControl?.Tag);
				string textBox1_AccessibleName = textBox1.AccessibleName;
				string textBox2_AccessibleName = textBox2.AccessibleName;
				if (factor <= 0.0)
				{
					textBox1.Text = "0";
					textBox2.Text = "0";
				}
				else if (activeTag == Convert.ToString(textBox1.Tag))
				{
					calcValue = ((value1 > 0.0) ? (value1 * (double)factor2 / factor) : 0.0);
					textBox2.Text = Math.Round(calcValue, decimalPointsForTextBox2).ToString(formatPattern);
					textBox1.AccessibleName = "True";
					textBox2.AccessibleName = "False";
				}
				else if (activeTag == Convert.ToString(textBox2.Tag))
				{
					calcValue = ((value2 > 0.0) ? (value2 * factor / (double)factor2) : 0.0);
					textBox1.Text = Math.Round(calcValue, decimalPointsForTextBox1).ToString(formatPattern);
					textBox2.AccessibleName = "True";
					textBox1.AccessibleName = "False";
				}
				else if (textBox1_AccessibleName == "True")
				{
					calcValue = ((value1 > 0.0) ? (value1 * (double)factor2 / factor) : 0.0);
					textBox2.Text = Math.Round(calcValue, decimalPointsForTextBox2).ToString(formatPattern);
				}
				else if (textBox2_AccessibleName == "True")
				{
					calcValue = ((value2 > 0.0) ? (value2 * factor / (double)factor2) : 0.0);
					textBox1.Text = Math.Round(calcValue, decimalPointsForTextBox1).ToString(formatPattern);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception("UpdateRelatedFieldsCalculation error: " + ex.Message);
		}
	}

	public static string ColorNameToHexCode(string colorName)
	{
		Color color = Color.FromName(colorName);
		if (color.IsSystemColor || color.A == 0)
		{
			return "#000000";
		}
		return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
	}

	public static void OpenDynamicallyScreen(string TargetUrl, string ScreenAlias, string ScreenName, UserAccount userAccount)
	{
		try
		{
			if (TargetUrl == null)
			{
				throw new Exception("Target URL not Configured properly... " + ScreenAlias);
			}
			if (Type.GetType(TargetUrl) != null)
			{
				if (Activator.CreateInstance(Type.GetType(TargetUrl), userAccount) is Form form)
				{
					form.Tag = ScreenName;
					form.ShowDialog();
					form.BringToFront();
				}
				return;
			}
			throw new Exception("Target URL not Configured properly... " + ScreenAlias);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static double GetSubsiadiaryBalance(int GlAccountId, int SubsidiaryAccountId, DateTime VoucherDate)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		double ClosingBalance = 0.0;
		try
		{
			DataTable detail = VoucherHead.GetGLAndSubsidiaryCurrentBalance(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ToDate = VoucherDate,
				AccountId = GlAccountId,
				SupplierCustomerId = SubsidiaryAccountId
			});
			if (detail.Rows.Count > 0)
			{
				ClosingBalance = Conversion.ToDouble(detail.Rows[0]["BalanceAmount"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return ClosingBalance;
	}

	public static double GetAccountBalance(int AccountId, DateTime voucherdatetime)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		double ClosingBalance = 0.0;
		try
		{
			DataTable detail = VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new VoucherHead
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				VoucherDate = voucherdatetime,
				RefAccountId = AccountId
			});
			if (detail.Rows.Count > 0)
			{
				ClosingBalance = Conversion.ToDouble(detail.Rows[0]["Balance"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return ClosingBalance;
	}

	public static void FcyJournalVoucherEntry350(int VoucherHeadId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherReports.VoucherReport(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 350,
				Id = VoucherHeadId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "350_FcyJournalVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GridColumnSettings(GridEXColumn column, int DecimalCount = 0, int defaultWidth = 100, List<string> skipColumns = null, bool NegativeFormat = false)
	{
		try
		{
			string key = column.Key;
			string keyLower = key.ToLowerInvariant();
			Type colType = column.Type;
			if (colType == typeof(double) || colType == typeof(decimal))
			{
				column.TextAlignment = (TextAlignment)3;
				if (key.Contains("Amount"))
				{
					column.AggregateFunction = (AggregateFunction)2;
					column.FormatString = (NegativeFormat ? clsGlobalVariables.stringFormatboth : clsGlobalVariables.stringFormatsingle);
					column.TotalFormatString = (NegativeFormat ? clsGlobalVariables.stringFormatboth : clsGlobalVariables.stringFormatsingle);
				}
				else if (key.Contains("Rate") || key.Contains("StockInHand") || key.Contains("Average"))
				{
					column.FormatString = clsGlobalVariables.DecimalRateFormate;
				}
				else
				{
					if (DecimalCount == 0)
					{
						DecimalCount = 2;
					}
					string format = "#,##0." + new string('#', DecimalCount);
					string NegFormat = "#,##0." + new string('#', DecimalCount) + ";(0,0." + new string('#', DecimalCount) + "); 0";
					column.AggregateFunction = (AggregateFunction)2;
					column.FormatString = (NegativeFormat ? NegFormat : format);
					column.TotalFormatString = (NegativeFormat ? NegFormat : format);
				}
			}
			if (colType == typeof(int))
			{
				column.TextAlignment = (TextAlignment)2;
			}
			if (skipColumns != null && skipColumns.Contains(key))
			{
				return;
			}
			int widthToSet = defaultWidth;
			if (colType == typeof(int))
			{
				widthToSet = 60;
			}
			else if (colType == typeof(DateTime))
			{
				widthToSet = 80;
			}
			else if (colType == typeof(float) || colType == typeof(double) || colType == typeof(decimal))
			{
				widthToSet = 90;
			}
			else if (colType == typeof(string))
			{
				if (keyLower.Contains("city"))
				{
					widthToSet = Constants.InventoryConstants.CityName;
				}
				else if (keyLower.Contains("term"))
				{
					widthToSet = Constants.InventoryConstants.TermConstant;
				}
				else if (keyLower.Contains("uom"))
				{
					widthToSet = Constants.InventoryConstants.UomConstant;
				}
				else if (keyLower.Contains("name") || keyLower.Contains("description"))
				{
					widthToSet = 150;
				}
			}
			column.Width = widthToSet;
		}
		catch (Exception ex)
		{
			throw new Exception("Error in GridColumnSettings: " + ex.Message, ex);
		}
	}

	public static void SetDefaultColumnWidths(GridEX grid, int defaultWidth = 100, List<string> skipColumns = null)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		object obj;
		if (grid == null)
		{
			obj = null;
		}
		else
		{
			GridEXTable rootTable = grid.RootTable;
			obj = ((rootTable != null) ? rootTable.Columns : null);
		}
		if (obj == null)
		{
			return;
		}
		foreach (GridEXColumn item in (JanusCollectionBase)grid.RootTable.Columns)
		{
			GridEXColumn column = item;
			if (skipColumns != null && skipColumns.Contains(column.Key))
			{
				continue;
			}
			int widthToSet = defaultWidth;
			if (column.Type == typeof(int))
			{
				widthToSet = 60;
			}
			else if (column.Type == typeof(DateTime))
			{
				widthToSet = 120;
			}
			else if (column.Type == typeof(float) || column.Type == typeof(double) || column.Type == typeof(decimal))
			{
				widthToSet = 90;
			}
			else if (column.Type == typeof(string))
			{
				if (column.Key.ToLower().Contains("name") || column.Key.ToLower().Contains("description"))
				{
					widthToSet = 150;
				}
				else if (column.Key.ToLower().Contains("term"))
				{
					widthToSet = Constants.InventoryConstants.TermConstant;
				}
				else if (column.Key.ToLower().Contains("uom"))
				{
					widthToSet = Constants.InventoryConstants.UomConstant;
				}
			}
			column.Width = widthToSet;
		}
	}

	public static void GridWrappingAndColumnSettings(GridEX grd, int HeaderSize, int DecimalCount = 0, string EditType = "false", string[] hiddenColumns = null, bool NegativeFormat = false)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		try
		{
			grd.RootTable.HeaderLines = HeaderSize;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				GridColumnSettings(column, DecimalCount, 100, null, NegativeFormat);
				column.FilterEditType = (FilterEditType)((!(column.Type == typeof(DateTime))) ? 1 : 6);
				if (!string.IsNullOrEmpty(EditType) && EditType != "false")
				{
					column.EditType = (EditType)0;
				}
				if (hiddenColumns != null && hiddenColumns.Contains(column.Key))
				{
					column.Visible = false;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static StockResult GetAvailableStockFromTransactionOrEvaluation(int ItemId, DateTime DocDate, int JobLotId = 0, int WarehouseId = 0, string CropYear = null, int PackingTypeId = 0, int ItemUomId = 0, int actionId = 0, int WithoutStockLogic = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		try
		{
			return GetAvgRatesAndStockInHand.GetAvailableStockByTransactionOrEvaluation(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemId = ItemId,
				DocDate = DocDate,
				JobLotId = JobLotId,
				WarehouseId = WarehouseId,
				CropYear = CropYear,
				PackingTypeId = ItemUomId,
				ItemUomId = ItemUomId,
				ActionId = actionId,
				WithoutStockLogic = WithoutStockLogic
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBalanceQtyAndWeightFromStockEvaluation(int ItemId, DateTime DocDate, int JobLotId = 0, int WarehouseId = 0, string CropYear = null, int PackingTypeId = 0, int ItemUomId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			return GetAvgRatesAndStockInHand.GetBalanceQtyAndWeightFromStockEvaluation(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocDate = DocDate,
				ItemId = ItemId,
				JobLotId = JobLotId,
				WarehouseId = WarehouseId,
				CropYear = CropYear,
				PackingTypeId = ItemUomId,
				stockUOM = ItemUomId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockAdjutmentPackingMaterial215(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("RecordId Not Found");
			}
			DataTable dt = InvStockAdjustment.StockAdjustmentSlipAndRegister409(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@PrintedBy", clsGlobalVariables.UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dt, "215_StockAdjustmentPMSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApplyCommaFormattingToTextBox(TextBox textBox)
	{
		try
		{
			if (decimal.TryParse(textBox.Text.Replace(",", ""), out var number))
			{
				textBox.Text = number.ToString("N0");
				textBox.SelectionStart = textBox.TextLength;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static DataTable GetAllLcOrderSchedulingPolicy()
	{
		try
		{
			return ExImLcOrder.GetAllLcOrderSchedulingPolicy();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetConfigurationsByDefinitionIds(string Ids)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return ConfigrationsAllocation.GetConfigurationsByDefinitionIds(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = Ids
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CompanyServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		try
		{
			return Company.GetAlldt(new Company
			{
				OrgCompanyTypeId = clsGlobalVariables.UserAccount.OrganizationId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable BrancheServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return Branches.GetAll(new Branches
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProjectServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return Projects.GetAlldt(new Projects
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CoaAllocationGetAllServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		try
		{
			return COAAllocation.GetAll(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool CheckCommissionOnPurchaseByItem(int ItemId)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		bool IsOn = false;
		try
		{
			DataTable dt = Item.CheckCommissionOnByItem(new ReportsParameters
			{
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				ItemId = ItemId
			});
			if (dt.Rows.Count > 0)
			{
				IsOn = Conversion.ToBool(dt.Rows[0]["OnPurchase"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return IsOn;
	}

	public static bool CheckCommissionOnSaleByItem(int ItemId)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		bool IsOn = false;
		try
		{
			DataTable dt = Item.CheckCommissionOnByItem(new ReportsParameters
			{
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				ItemId = ItemId
			});
			if (dt.Rows.Count > 0)
			{
				IsOn = Conversion.ToBool(dt.Rows[0]["OnSale"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return IsOn;
	}

	public static DataTable CoaAllocationGetForComboServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		try
		{
			return COAAllocation.GetForComboBind(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAllDetailAccount(int CostCenterId = 0)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("AccountCode");
			dt.Columns.Add("AccountType");
			dt.Columns.Add("AccountClass");
			dt.Columns.Add("ParentAccount");
			DataTable dtcoa = ChartofAccount.DetailAccount(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				AppId = clsGlobalVariables.UserAccount.AppId,
				CostCenterId = CostCenterId
			});
			if (dtcoa.Rows.Count > 0)
			{
				for (int i = 0; i < dtcoa.Rows.Count; i++)
				{
					dt.Rows.Add(dtcoa.Rows[i]["Id"], dtcoa.Rows[i]["AccountTitle"], dtcoa.Rows[i]["AccountCode"], dtcoa.Rows[i]["AccountType"], dtcoa.Rows[i]["ClassName"], dtcoa.Rows[i]["ParentAccountTitle"]);
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PayeeTitleByAccountId(int AccountId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return ChartofAccount.PayeeTitleByAccountId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				AccountId = AccountId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CoaAllocationAccountTitleByAccountTypeIds(string AccountTypeIds = null, string AccountTypeIdsNot = null, int CostCenterId = 0, string AccountClassIds = "", string AccountClassIdsNot = "", int NotReferred = 0, int RecId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		try
		{
			return COAAllocation.GetAccountTitleByAccountTypeIds(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				AppId = clsGlobalVariables.UserAccount.AppId,
				UserId = clsGlobalVariables.UserAccount.ID,
				AccountTypeIds = AccountTypeIds,
				AccountTypeIdsNot = AccountTypeIdsNot,
				CostCenterId = CostCenterId,
				AccountClassIds = AccountClassIds,
				AccountClassIdsNot = AccountClassIdsNot,
				NotReferred = NotReferred,
				RecId = RecId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Accounts_GetAccountTitleByAccountTypeIds(string AccountTypeIds = null, string AccountTypeIdsNot = null, int CostCenterId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			return COAAllocation.Accounts_GetAccountTitleByAccountTypeIds(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				AppId = clsGlobalVariables.UserAccount.AppId,
				UserId = clsGlobalVariables.UserAccount.ID,
				AccountTypeIds = AccountTypeIds,
				AccountTypeIdsNot = AccountTypeIdsNot,
				CostCenterId = CostCenterId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FilterDataTableByAccountTypes(DataTable sourceTable, int[] accountTypeIds, bool includeTypes = true, string[] columnsToReturn = null)
	{
		try
		{
			EnumerableRowCollection<DataRow> query = from row in sourceTable.AsEnumerable()
				where (!includeTypes) ? (!accountTypeIds.Contains(row.Field<int>("AccountTypeId"))) : accountTypeIds.Contains(row.Field<int>("AccountTypeId"))
				select row;
			if (!query.Any())
			{
				if (columnsToReturn != null && columnsToReturn.Any())
				{
					DataTable emptyTable = new DataTable();
					string[] array = columnsToReturn;
					foreach (string col2 in array)
					{
						emptyTable.Columns.Add(col2, sourceTable.Columns[col2].DataType);
					}
					return emptyTable;
				}
				return sourceTable.Clone();
			}
			if (columnsToReturn != null && columnsToReturn.Any())
			{
				DataTable result = new DataTable();
				string[] array = columnsToReturn;
				foreach (string col3 in array)
				{
					result.Columns.Add(col3, sourceTable.Columns[col3].DataType);
				}
				foreach (DataRow row2 in query)
				{
					result.Rows.Add(columnsToReturn.Select((string col) => row2[col]).ToArray());
				}
				return result;
			}
			return query.CopyToDataTable();
		}
		catch (Exception ex)
		{
			throw new Exception("Error filtering DataTable: " + ex.Message, ex);
		}
	}

	public static DataTable GetAccountTitleByAccountClassIds(string AccountClassIds = null, string AccountClassIdsNot = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return COAAllocation.GetAccountTitleByAccountClassIds(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				AccountClassIds = AccountClassIds,
				AccountClassIdsNot = AccountClassIdsNot
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerGetAllServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.Getall(new SupplierCustomer
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AllSupplierCustomerWithCityName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.AllSupplierCustomerWithCityName(new SupplierCustomer
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SupplierCustomerGetforComboServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.GetforComboBinding(new SupplierCustomer
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getSupplierCustomersForDropdown(int PartyTypeId = 0, int CostCnterId = 0, string CustomerGroupIds = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.getSupplierCustomersForDropdown(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				AppId = clsGlobalVariables.UserAccount.AppId,
				UserId = clsGlobalVariables.UserAccount.ID,
				PartyTypeId = PartyTypeId,
				CostCenterId = CostCnterId,
				CustomerGroupIds = CustomerGroupIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetMoistureByItemIdAndPurchaseOrderId(int ItemId, int PurchaseOrderId)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		double Moisture = 0.0;
		try
		{
			DataTable dt = InvLabAnalysisPurchaseHeader.GetMoistureFromOrderByItemIdAndOrder(new InvLabAnalysisPurchaseHeader
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemId = ItemId,
				PurchaseOrderId = PurchaseOrderId
			});
			if (dt.Rows.Count > 0)
			{
				Moisture = Conversion.ToDouble(dt.Rows[0][0]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Moisture;
	}

	public static int GetScreenIdByName(string Name)
	{
		int Id = 0;
		try
		{
			List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == Name.Trim()).ToList();
			if (newList != null && newList.Count > 0)
			{
				Id = Conversion.ToInt((object)newList[0].ScreenID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Id;
	}

	public static DataTable GetVendorsAndCustomers(int PartyTypeId = 0)
	{
		try
		{
			return SupplierCustomer.GetVendorsAndCustomers(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, PartyTypeId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetVendorsAndCustomersWithCityName(int PartyTypeId = 0)
	{
		try
		{
			return SupplierCustomer.GetVendorsAndCustomersWithCityName(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, PartyTypeId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetVendorsAndCustomersForTransporter()
	{
		try
		{
			return SupplierCustomer.GetVendorsAndCustomersForTransporter(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGlAccountIdandCompanyNameBySupplierCustomerId(int Id = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.GetGlAccountIdandCompanyNameBySupplierCustomerId(new SupplierCustomer
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = Id
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetGlAccountIdBySupplierCustomerId(int SupCustId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.GetGlAccountIdBySupplierCustomerId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				SupplierCustomerId = SupCustId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierCustomerInfoByGLAccountId(int GLId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.GetSupplierCustomerInfoByGLAccountId(new SupplierCustomer
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				GlAccountId = GLId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierustomerByCustomerGroupId(string CustomerGroupIds, int ParentId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.GetSupplierustomerByCustomerGroupId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = CustomerGroupIds,
				ParentId = ParentId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetGhallaMandi()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return Ghalla_Mandi.GetAll(new Ghalla_Mandi
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierustomerForPartyProcessing()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.GetSupplierustomerForPartyProcessing(new SupplierCustomer
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadByOrganizationCompanyIdForExportServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemGetAllServiceBind(string ParentCategoryIds = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		try
		{
			return Item.GetAll(new Item
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			}, ParentCategoryIds);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsWithVarientOrWithoutVarient_Concrete(string ApprovedFilter, bool flag)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetItemsWithVarientOrWithoutVarient(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				WithOrWithoutVariant = flag
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllForItemsForBrands()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return Item.ReadAllForItemsForBrands(new Item
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemGetForComboServiceBind(int ParentCategoryId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetAllbyCombobind(new Item
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemCategoryId = ParentCategoryId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllForComboTwoColumnsWithParentId(int ParentCategoryId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.ReadAllForComboTwoColumnsWithParentId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ParentCategoryId = ParentCategoryId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllItemsByParentId(int ParentCategoryId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.ReadAllItemsByParentId(new Item
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemCategoryId = ParentCategoryId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllItems()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return Item.ReadAllItems(new Item
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AllItemsBindForSteel()
	{
		try
		{
			return Item.AllItemsBindForSteel(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllItemsWithPackingAndStore()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return Item.ReadAllItemsWithPackingAndStore(new Item
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllItemsIncludedPM()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return Item.ReadAllItemsIncludedPM(new Item
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPackingMaterialItemsAllocateToFlow(int TransactionFlowId = 1, int ItemTypeId = 0, int ItemCategoryId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			return PackingMaterialItemsAllocateToTransactionFlow.PackingMaterialItemsAllocateToTransactionFlow_GetForCombo(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				TransactionFlowId = TransactionFlowId,
				ItemTypeId = ItemTypeId,
				ItemCategoryId = ItemCategoryId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemByItemTypeId(string Ids)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetItemByItemTypeId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = Ids
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemBindFromPricingSchedule(int priceTypeId, string ItemIds = "", int ItemCategoryId = 0, int ItemTypeId = 0)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("ItemId");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("ItemPrice");
			dt.Columns.Add("RateUomId");
			dt.Columns.Add("Id");
			dt.Columns.Add("ItemCode");
			DataTable data = ItemPricingSchedule.ReadByItemIdAndPriceLookUpId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = priceTypeId,
				ItemIds = ItemIds,
				ItemCategoryId = ItemCategoryId,
				ItemTypeId = ItemTypeId
			});
			if (data.Rows.Count > 0)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{
					dt.Rows.Add(data.Rows[i]["ItemId"], data.Rows[i]["ItemName"], data.Rows[i]["ItemPrice"], data.Rows[i]["RateUomId"], data.Rows[i]["Id"], data.Rows[i]["ItemCode"]);
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemCategoryFromPricingShedule(int PriceTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return ItemPricingSchedule.GetItemCategoryFromPricingShedule(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				PriceTypeId = PriceTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemTypesFromPricingShedule(int PriceTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return ItemPricingSchedule.GetItemTypesFromPricingShedule(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				PriceTypeId = PriceTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWarehouseForStoreFromInventoryTransactions(string TypeIds)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetWarehouseForStoreFromInventoryTransactions(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = TypeIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromItemTaxSchedule()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return ItemTaxSchedule.GetItemsFromItemTaxSchedule(new ItemTaxSchedule
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromWarehouseAllocationByWareHouseId(int warehouseId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return ItemAllocateToWareHouse.GetItemsFromWarehouseAllocationByWareHouseId(new ItemAllocateToWareHouse
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				WarehouseId = warehouseId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExportItemsByOrganizationCompanyIdForComboServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return Item.GetExportItemsByOrganizationCompanyId(new Item
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemCategoryForComboServiceBind(string ParentCategoryIds = "")
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return ItemCategory.Getall(new ItemCategory
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				CategoryCode = ParentCategoryIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemTypeForComboServiceBind(int Type = 0, string ParentCategories = "")
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return ItemType.Getall(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				TypeId = Type,
				ItemIds = ParentCategories
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemGetForComboServiceBindForEngr(int ParentCategoryId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.AllItemsBindForEngr(new Item
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemCategoryId = ParentCategoryId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSeaPortForComboServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDueTermServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return InvDueTerms.GetAll(new InvDueTerms
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WareHouseGetAllService()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return InvWareHouse.Getall(new InvWareHouse
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WareHouseAllocationToPlantsByProductionPlantId(int PlantId = 0)
	{
		try
		{
			return InvWareHouse.WareHouseAllocationToPlantsByProductionPlantId(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, PlantId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable getActiveWareHouse()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return InvWareHouse.GetActiveWareHouse(new InvWareHouse
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetActiveWareHouseByWareHouseType(int WarehouseTypeId = 0, string Ids = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return InvWareHouse.GetActiveWareHouseByWareHouseType(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ActivityId = WarehouseTypeId,
				Activity = Ids
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable JobLotGetAllService()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return jobLot.GetAll(new JobLot
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetMonths()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "January");
			dt.Rows.Add(2, "February");
			dt.Rows.Add(3, "March");
			dt.Rows.Add(4, "April");
			dt.Rows.Add(5, "May");
			dt.Rows.Add(6, "June");
			dt.Rows.Add(7, "July");
			dt.Rows.Add(8, "August");
			dt.Rows.Add(9, "September");
			dt.Rows.Add(10, "October");
			dt.Rows.Add(11, "November");
			dt.Rows.Add(12, "December");
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetYears()
	{
		try
		{
			int year = clsGlobalVariables.ActiveYr.Start_Period.Year;
			DataTable dt = new DataTable
			{
				Columns = { "Id", "Year" }
			};
			for (int i = year; i <= DateTime.Now.Year; i++)
			{
				dt.Rows.Add(i, i);
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable dtUomFromGloablUomScheduleByItemId(int itemId, bool addBaseSecondaryUom = false)
	{
		DataTable dtUom = new DataTable();
		try
		{
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("UOMCode", typeof(string));
			dtUom.Columns.Add("Equivalent", typeof(double));
			dtUom.Columns.Add("BaseRateUom", typeof(bool));
			dtUom.Columns.Add("BasePackUom", typeof(bool));
			if (addBaseSecondaryUom)
			{
				dtUom.Columns.Add("BaseSecondaryUom", typeof(bool));
			}
			List<GloablUomSchedule> dtAllUom = clsGlobalVariables.globalUomSchedule;
			if (dtAllUom == null || dtAllUom.Count == 0)
			{
				return dtUom;
			}
			List<GloablUomSchedule> filtered = dtAllUom.Where((GloablUomSchedule x) => Conversion.ToInt((object)x.ItemId) == itemId).ToList();
			if (!filtered.Any())
			{
				return dtUom;
			}
			foreach (GloablUomSchedule uom in filtered)
			{
				DataRow row = dtUom.NewRow();
				row["Id"] = uom.Id;
				row["UOMCode"] = uom.UOMCode;
				row["Equivalent"] = uom.Equivalent;
				row["BaseRateUom"] = uom.BaseRateUom;
				row["BasePackUom"] = uom.BasePackUom;
				if (addBaseSecondaryUom)
				{
					row["BaseSecondaryUom"] = uom.BaseSecondaryUom;
				}
				dtUom.Rows.Add(row);
			}
			return dtUom;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static DataTable GetItemCategoryOrTypedtFromGlobal(bool FromAllItems = true, bool itemCategory = true)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description", typeof(string));
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (allItems == null || !allItems.Any())
			{
				return dt;
			}
			IEnumerable<getGlobalAllItems> filteredItems = (FromAllItems ? allItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId != 14 && item.ItemTypeOfTypeId != 17) : allItems.Where((getGlobalAllItems item) => item.ItemClassId == 8));
			foreach (var entry in from x in itemCategory ? (from item in filteredItems
					where !string.IsNullOrWhiteSpace(item.ItemCategory)
					select new
					{
						Id = Conversion.ToInt((object)item.ItemCategoryId),
						Description = item.ItemCategory
					}) : (from item in filteredItems
					where !string.IsNullOrWhiteSpace(item.ItemType)
					select new
					{
						Id = Conversion.ToInt((object)item.ItemTypeId),
						Description = item.ItemType
					})
				group x by x.Description into g
				select g.First())
			{
				dt.Rows.Add(entry.Id, entry.Description);
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message, ex);
		}
	}

	public static DataTable CityGetAllService()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return City.GetAll(new City
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvPackingTypeGetAllService()
	{
		try
		{
			return InvPackingType.Getall();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CropYearGetAllService()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return InvCropYear.Getall(new InvCropYear
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable StaticColumnsService(string Activity)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			return GeneralReprots.StaticColumnNames(new ReportsParameters
			{
				Activity = Activity
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemsWithBaseUOM(string InTypeIds = null, string NotInTypeIds = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return Item.ItemsWithBaseUOM(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = InTypeIds,
				ItemIds = NotInTypeIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ItemBaseUOMByItemId(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.ItemBaseUOMByItemId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemId = ItemId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetStaticUOM()
	{
		try
		{
			DataTable dtUom = new DataTable();
			DataTable dt = UOM.UOMStaticAll();
			if (dt.Rows.Count > 0)
			{
				dtUom.Columns.Add("Id");
				dtUom.Columns.Add("UOM");
				dtUom.Columns.Add("Equivalent");
				dtUom.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUom.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["UOMCode"], dt.Rows[i]["Equivalent"]);
				}
			}
			return dtUom;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetConfigurationByOrgCompandConfigDescription(string ConfigDescription)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return ConfigrationsAllocation.GetConfigurationByOrgCompandConfigDescription(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ConfigDescription = ConfigDescription
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool CheckDocumentInApprovalPolicy(int DocumentId)
	{
		try
		{
			if (clsGlobalVariables.Daw_PolicyDocuments.Where((PolicyDocuments x) => x.DocumentId == DocumentId).ToList().Count > 0)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool GetERPFeatureById(int FeatureId)
	{
		try
		{
			if (clsGlobalVariables.ErpFeaturesList.Where((ErpFeaturesByCompany x) => x.Id == FeatureId).ToList().Count > 0)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetMultipleConfigurationsByConfigDescriptions(string ConfigDescription)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return ConfigrationsAllocation.GetMultipleConfigurationsByConfigDescriptions(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ConfigDescription = ConfigDescription
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetWeightAvgRateByItem(ReportsParameters obj)
	{
		try
		{
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			return Item.GetWeightAvgRateByItem(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void OnlytextdecimelFunction(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
			}
			if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void OnlytextNumberFunction(object sender, KeyPressEventArgs e)
	{
		try
		{
			e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void OnlytextdecimelFunctionWithMinus(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
			{
				e.Handled = true;
			}
			if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
			{
				e.Handled = true;
			}
			if (e.KeyChar == '-' && (sender as TextBox).Text.IndexOf('-') > -1)
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void CommasApplyWhileTyping(object sender, EventArgs e)
	{
	}

	public static void CommasApplyWhileTypingOnVouchers(object sender, EventArgs e)
	{
		try
		{
			TextBox textBox = sender as TextBox;
			string textValue = textBox.Text;
			int selectionStart = textBox.SelectionStart;
			textValue = textValue.Replace(",", "");
			if (!double.TryParse(textValue, out var result))
			{
				return;
			}
			bool num = textValue.Contains(".");
			string formattedNumber = $"{Math.Floor(result):#,##0}";
			if (num)
			{
				string decimalPart = textValue.Split('.')[1];
				if (decimalPart.Length > clsGlobalVariables.DefaultNoofDecimalPointsForAmount)
				{
					decimalPart = decimalPart.Substring(0, clsGlobalVariables.DefaultNoofDecimalPointsForAmount);
				}
				formattedNumber = formattedNumber + "." + decimalPart;
			}
			textBox.TextChanged -= CommasApplyWhileTypingOnVouchers;
			textBox.Text = formattedNumber;
			textBox.SelectionStart = Math.Min(selectionStart + GetCursorOffset(textBox.Text, textValue, selectionStart), textBox.Text.Length);
			textBox.TextChanged += CommasApplyWhileTypingOnVouchers;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void CommasApplyWhileTypingOnVouchersForFcyAmount(object sender, EventArgs e)
	{
		try
		{
			TextBox textBox = sender as TextBox;
			string textValue = textBox.Text;
			int selectionStart = textBox.SelectionStart;
			textValue = textValue.Replace(",", "");
			if (!double.TryParse(textValue, out var result))
			{
				return;
			}
			bool num = textValue.Contains(".");
			string formattedNumber = $"{Math.Floor(result):#,##0}";
			if (num)
			{
				string decimalPart = textValue.Split('.')[1];
				if (decimalPart.Length > 0 && decimalPart.Length > clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount)
				{
					decimalPart = decimalPart.Substring(0, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
				}
				formattedNumber = formattedNumber + "." + decimalPart;
			}
			textBox.TextChanged -= CommasApplyWhileTypingOnVouchersForFcyAmount;
			textBox.Text = formattedNumber;
			textBox.SelectionStart = Math.Min(selectionStart + GetCursorOffset(textBox.Text, textValue, selectionStart), textBox.Text.Length);
			textBox.TextChanged += CommasApplyWhileTypingOnVouchersForFcyAmount;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private static int GetCursorOffset(string formattedText, string originalText, int originalCursorPosition)
	{
		int length = formattedText.Length;
		int originalLength = originalText.Length;
		int cursorOffset = length - originalLength;
		if (originalCursorPosition == originalLength)
		{
			return cursorOffset;
		}
		int originalCursorOffset = originalLength - originalCursorPosition;
		return Math.Max(cursorOffset - originalCursorOffset, 0);
	}

	public static bool IsNumeric(UpdatingCellEventArgs e)
	{
		bool isNumeric = false;
		try
		{
			decimal result;
			return decimal.TryParse(e.Value.ToString(), out result);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool IsPositiveNumeric(UpdatingCellEventArgs e)
	{
		bool isPositiveNumeric = false;
		try
		{
			if (decimal.TryParse(e.Value.ToString(), out var result))
			{
				isPositiveNumeric = result >= 0m;
				if (!isPositiveNumeric)
				{
					e.Value = 0;
				}
			}
			else
			{
				e.Value = 0;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		return isPositiveNumeric;
	}

	public static DataTable GetAvgRateQtyAndStockInHand(int ItemId, DateTime DocDate, int ItemConditionId = 0, int RecId = 0, int DocumentTypeId = 0, int WarehouseId = 0, int RackId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			return GetAvgRatesAndStockInHand.GetAvgRateQtyAndStockInHand(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemId = ItemId,
				DocDate = DocDate,
				ItemConditionId = ItemConditionId,
				RecId = RecId,
				DocumentTypeId = DocumentTypeId,
				WarehouseId = WarehouseId,
				StoreRackId = RackId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetStockInHandFromInventoryTrasactions(int ItemId, DateTime DocDate, int JobLotId = 0, int WarehouseId = 0, string CropYear = null, int ItemUomId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			return GetAvgRatesAndStockInHand.GetStockInHandFromInventoryTrasactions(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocDate = DocDate,
				ItemId = ItemId,
				JobLotId = JobLotId,
				WarehouseId = WarehouseId,
				CropYear = CropYear,
				stockUOM = ItemUomId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double AvgRateOnlyForCGS(int ItemId, DateTime DocDate, int DocumentTypeId = 0, int RecId = 0, int JobLotId = 0, int CropYearId = 0, string CropYear = null, int WarehouseId = 0, int BranchesId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		try
		{
			return GetAvgRatesAndStockInHand.AvgRateOnlyForCGS(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemId = ItemId,
				DocDate = DocDate,
				DocumentTypeId = DocumentTypeId,
				Id = RecId,
				JobLotId = JobLotId,
				CropYearId = CropYearId,
				CropYear = CropYear,
				WarehouseId = WarehouseId,
				BranchesId = BranchesId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double getAvgRateByItemId(int ItemId, DateTime DocDate, int DocumentTypeId = 0, int RecId = 0, int JobLotId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			return GetAvgRatesAndStockInHand.getAvgRateByItemId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemId = ItemId,
				DocDate = DocDate,
				DocumentTypeId = DocumentTypeId,
				Id = RecId,
				JobLotId = JobLotId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetAvgRateFromFIFOMethod(ReportsParameters obj)
	{
		try
		{
			double AvgRate = 0.0;
			DataTable dtstock = GetAvgRatesAndStockInHand.GetStockByFifoMethod(obj);
			if (dtstock.Rows.Count > 0)
			{
				List<FIFOStockEvaluation> lstfifo = Conversion.ConvertDataTableToList<FIFOStockEvaluation>(dtstock);
				if (lstfifo.Count > 0)
				{
					double TotalAvailablewt = lstfifo.Sum((FIFOStockEvaluation row) => row.BalWeight);
					lstfifo.Sum((FIFOStockEvaluation row) => row.BalQty);
					double TotalRequriedWQty = obj.ItemQty;
					double TotalRequriedWeight = obj.NetWeight;
					double TotalCgsAmount = 0.0;
					if (obj.NetWeight <= TotalAvailablewt)
					{
						double Required = TotalRequriedWeight;
						double RequiredQty = TotalRequriedWQty;
						double AvailableQty = 0.0;
						double ConsumedQty = 0.0;
						double Available = 0.0;
						double Consumed = 0.0;
						for (int i = 0; i < lstfifo.Count; i++)
						{
							Available = lstfifo[i].BalWeight;
							AvailableQty = lstfifo[i].BalQty;
							if (Available <= Required - Consumed && AvailableQty <= RequiredQty - ConsumedQty)
							{
								Consumed += Available;
								ConsumedQty += AvailableQty;
								TotalCgsAmount += Available * Conversion.ToDouble((object)lstfifo[i].AvgRate);
							}
							else if (Available >= Required - Consumed && AvailableQty >= RequiredQty - ConsumedQty)
							{
								double outQty = 0.0;
								double outWeight = 0.0;
								outQty = Conversion.ToDouble((object)(RequiredQty - ConsumedQty));
								outWeight = Conversion.ToDouble((object)(Required - Consumed));
								TotalCgsAmount += Conversion.ToDouble((object)(Required - Consumed)) * Conversion.ToDouble((object)lstfifo[i].AvgRate);
								ConsumedQty += outQty;
								Consumed += outWeight;
							}
							if (TotalRequriedWeight == Consumed && TotalRequriedWQty == ConsumedQty)
							{
								break;
							}
						}
						if (TotalCgsAmount > 0.0 && TotalRequriedWeight > 0.0)
						{
							AvgRate = TotalCgsAmount / TotalRequriedWeight;
						}
					}
				}
			}
			return AvgRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetAvgRateFromFIFOMethodConcrete(ReportsParameters obj)
	{
		try
		{
			double AvgRate = 0.0;
			DataTable dtstock = GetAvgRatesAndStockInHand.GetStockByFifoMethodConcrete(obj);
			if (dtstock.Rows.Count > 0)
			{
				List<FIFOStockEvaluation> lstfifo = Conversion.ConvertDataTableToList<FIFOStockEvaluation>(dtstock);
				if (lstfifo.Count > 0)
				{
					double TotalAvailablewt = lstfifo.Sum((FIFOStockEvaluation row) => row.BalWeight);
					double TotalAvailableQty = lstfifo.Sum((FIFOStockEvaluation row) => row.BalQty);
					double TotalRequriedWQty = obj.ItemQty;
					double TotalRequriedWeight = obj.NetWeight;
					double TotalCgsAmount = 0.0;
					if (obj.ItemQty <= TotalAvailableQty && obj.NetWeight <= TotalAvailablewt)
					{
						double Required = TotalRequriedWeight;
						double RequiredQty = TotalRequriedWQty;
						double AvailableQty = 0.0;
						double ConsumedQty = 0.0;
						double Available = 0.0;
						double Consumed = 0.0;
						for (int i = 0; i < lstfifo.Count; i++)
						{
							Available = lstfifo[i].BalWeight;
							AvailableQty = lstfifo[i].BalQty;
							if (AvailableQty <= RequiredQty - ConsumedQty)
							{
								Consumed += Available;
								ConsumedQty += AvailableQty;
								TotalCgsAmount += AvailableQty * Conversion.ToDouble((object)lstfifo[i].AvgRate);
							}
							else if (AvailableQty >= RequiredQty - ConsumedQty)
							{
								double outQty = 0.0;
								double outWeight = 0.0;
								outQty = Conversion.ToDouble((object)(RequiredQty - ConsumedQty));
								outWeight = Conversion.ToDouble((object)(Required - Consumed));
								TotalCgsAmount += Conversion.ToDouble((object)(RequiredQty - ConsumedQty)) * Conversion.ToDouble((object)lstfifo[i].AvgRate);
								ConsumedQty += outQty;
								Consumed += outWeight;
							}
							if (TotalRequriedWQty == ConsumedQty)
							{
								break;
							}
						}
						if (TotalCgsAmount > 0.0 && TotalRequriedWQty > 0.0)
						{
							AvgRate = TotalCgsAmount / TotalRequriedWQty;
						}
					}
				}
			}
			return AvgRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWeightCurrStockByItem(ReportsParameters obj)
	{
		try
		{
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.WarehouseId = Conversion.ToInt((object)obj.WarehouseId);
			obj.ItemId = Conversion.ToInt((object)obj.ItemId);
			obj.JobLotId = Conversion.ToInt((object)obj.JobLotId);
			obj.CropYear = Conversion.ToString((object)obj.CropYear);
			obj.ToDate = Conversion.ToDateTime((object)obj.ToDate);
			return InvSaleInvoice.GetWeightCurrStockByItem(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void EditMethodFromLinked(int DocumentTypeId, int Id, int DocumentTypeIdSrNo = 0, int BaseDocumentTypeId = 0, int RefDocumentTypeId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_1df4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfb: Expected O, but got Unknown
		try
		{
			UserAccount UserAccount = new UserAccount();
			UserAccount = clsGlobalVariables.UserAccount;
			string TargetUrl = "";
			string screenName = "";
			string ReadById = "";
			int IdForEdit = 0;
			if ((DocumentTypeId == 1 || DocumentTypeId == 2) && BaseDocumentTypeId == 0)
			{
				Architecture.WinApp.Account_Definition.PaymentVoucherNew Pv9 = new Architecture.WinApp.Account_Definition.PaymentVoucherNew(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					if (DocumentTypeId == 1)
					{
						Pv9.Tag = "frmCashPaymentVoucher";
					}
					else
					{
						Pv9.Tag = "frmBankPaymentVoucher";
					}
					Pv9.Show();
					Pv9.ReadById(Id);
				}
				else if (CheckScreenRight("PaymentVoucherNew"))
				{
					if (DocumentTypeId == 1)
					{
						Pv9.Tag = "frmCashPaymentVoucher";
					}
					else
					{
						Pv9.Tag = "frmBankPaymentVoucher";
					}
					Pv9.Show();
					Pv9.ReadById(Id);
				}
				else
				{
					switch (DocumentTypeId)
					{
					case 1:
						if (CheckScreenRight("frmCashPaymentVoucher"))
						{
							Pv9.Tag = "frmCashPaymentVoucher";
							Pv9.Show();
							Pv9.ReadById(Id);
							break;
						}
						MessageBox.Show("You don't have right");
						return;
					case 2:
						if (CheckScreenRight("frmBankPaymentVoucher"))
						{
							Pv9.Tag = "frmBankPaymentVoucher";
							Pv9.Show();
							Pv9.ReadById(Id);
							break;
						}
						MessageBox.Show("You don't have right");
						return;
					}
				}
			}
			if ((DocumentTypeId == 1 || DocumentTypeId == 2) && BaseDocumentTypeId == 1)
			{
				Architecture.WinApp.Account_Definition.VouchersWithTax.PaymentVoucherNew Pv10 = new Architecture.WinApp.Account_Definition.VouchersWithTax.PaymentVoucherNew(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					if (DocumentTypeId == 1)
					{
						Pv10.Tag = "frmCashPaymentVoucherTax";
					}
					else
					{
						Pv10.Tag = "frmBankPaymentVoucherTax";
					}
					Pv10.Show();
					Pv10.ReadById(Id);
				}
				else if (CheckScreenRight("PaymentVoucherNewTax"))
				{
					if (DocumentTypeId == 1)
					{
						Pv10.Tag = "frmCashPaymentVoucherTax";
					}
					else
					{
						Pv10.Tag = "frmBankPaymentVoucherTax";
					}
					Pv10.Show();
					Pv10.ReadById(Id);
				}
				else
				{
					switch (DocumentTypeId)
					{
					case 1:
						if (CheckScreenRight("frmCashPaymentVoucherTax"))
						{
							Pv10.Tag = "frmCashPaymentVoucherTax";
							Pv10.Show();
							Pv10.ReadById(Id);
							break;
						}
						MessageBox.Show("You don't have right");
						return;
					case 2:
						if (CheckScreenRight("frmBankPaymentVoucherTax"))
						{
							Pv10.Tag = "frmBankPaymentVoucherTax";
							Pv10.Show();
							Pv10.ReadById(Id);
							break;
						}
						MessageBox.Show("You don't have right");
						return;
					}
				}
			}
			if ((DocumentTypeId == 1 && DocumentTypeIdSrNo == 1) || (DocumentTypeId == 2 && DocumentTypeIdSrNo == 2))
			{
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					PaymentByInvoiceVoucherNew paymentByInvoiceVoucherNew = new PaymentByInvoiceVoucherNew(UserAccount);
					paymentByInvoiceVoucherNew.Show();
					paymentByInvoiceVoucherNew.HistoryGridFill(Id);
				}
				else
				{
					if (!CheckScreenRight("PaymentByInvoiceVoucherNew"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					PaymentByInvoiceVoucherNew paymentByInvoiceVoucherNew2 = new PaymentByInvoiceVoucherNew(UserAccount);
					paymentByInvoiceVoucherNew2.Show();
					paymentByInvoiceVoucherNew2.HistoryGridFill(Id);
				}
			}
			else if ((DocumentTypeId == 3 || DocumentTypeId == 4) && BaseDocumentTypeId == 0)
			{
				Architecture.WinApp.Account_Definition.ReceiptsVoucherNew receiptsVoucher = new Architecture.WinApp.Account_Definition.ReceiptsVoucherNew(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					if (DocumentTypeId == 3)
					{
						receiptsVoucher.Tag = "frmCashReceiptVoucher";
					}
					else
					{
						receiptsVoucher.Tag = "frmBankReceiptVoucher";
					}
					receiptsVoucher.Show();
					receiptsVoucher.ReadById(Id);
				}
				else if (clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "ReceiptsVoucherNew").ToList();
					if (newList.Count > 0)
					{
						if (Conversion.ToBool((object)newList[0].Value.ToString()))
						{
							if (DocumentTypeId == 3)
							{
								receiptsVoucher.Tag = "frmCashReceiptVoucher";
							}
							else
							{
								receiptsVoucher.Tag = "frmBankReceiptVoucher";
							}
							receiptsVoucher.Show();
							receiptsVoucher.ReadById(Id);
						}
						else
						{
							switch (DocumentTypeId)
							{
							case 3:
							{
								Architecture.WinApp.Account_Definition.ReceiptsVoucherNew Pv = new Architecture.WinApp.Account_Definition.ReceiptsVoucherNew(UserAccount);
								if (clsGlobalVariables.ScreenViewReights.Count > 0)
								{
									newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmCashReceiptVoucher").ToList();
									if (!Conversion.ToBool((object)newList[0].Value.ToString()))
									{
										MessageBox.Show("You don't have right");
										return;
									}
									Pv.Tag = "frmCashReceiptVoucher";
									Pv.Show();
									Pv.ReadById(Id);
								}
								break;
							}
							case 4:
							{
								Architecture.WinApp.Account_Definition.ReceiptsVoucherNew Pv2 = new Architecture.WinApp.Account_Definition.ReceiptsVoucherNew(UserAccount);
								if (clsGlobalVariables.ScreenViewReights.Count > 0)
								{
									newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmBankReceiptVoucher").ToList();
									if (!Conversion.ToBool((object)newList[0].Value.ToString()))
									{
										MessageBox.Show("You don't have right");
										return;
									}
									Pv2.Tag = "frmBankReceiptVoucher";
									Pv2.Show();
									Pv2.ReadById(Id);
								}
								break;
							}
							default:
								MessageBox.Show("You don't have right");
								return;
							}
						}
					}
					else if (DocumentTypeId == 3)
					{
						Architecture.WinApp.Account_Definition.ReceiptsVoucherNew Pv3 = new Architecture.WinApp.Account_Definition.ReceiptsVoucherNew(UserAccount);
						if (clsGlobalVariables.ScreenViewReights.Count > 0)
						{
							newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmCashReceiptVoucher").ToList();
							if (!Conversion.ToBool((object)newList[0].Value.ToString()))
							{
								MessageBox.Show("You don't have right");
								return;
							}
							Pv3.Tag = "frmCashReceiptVoucher";
							Pv3.Show();
							Pv3.ReadById(Id);
						}
					}
					else
					{
						Architecture.WinApp.Account_Definition.ReceiptsVoucherNew Pv4 = new Architecture.WinApp.Account_Definition.ReceiptsVoucherNew(UserAccount);
						if (clsGlobalVariables.ScreenViewReights.Count > 0)
						{
							newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmBankReceiptVoucher").ToList();
							if (!Conversion.ToBool((object)newList[0].Value.ToString()))
							{
								MessageBox.Show("You don't have right");
								return;
							}
							Pv4.Tag = "frmBankReceiptVoucher";
							Pv4.Show();
							Pv4.ReadById(Id);
						}
					}
				}
			}
			else if ((DocumentTypeId == 3 || DocumentTypeId == 4) && BaseDocumentTypeId == 2)
			{
				Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew receiptsVoucher2 = new Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					if (DocumentTypeId == 3)
					{
						receiptsVoucher2.Tag = "frmCashReceiptVoucherTax";
					}
					else
					{
						receiptsVoucher2.Tag = "frmBankReceiptVoucherTax";
					}
					receiptsVoucher2.Show();
					receiptsVoucher2.ReadById(Id);
				}
				else if (clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					List<tblUserRights> newList2 = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "ReceiptsVoucherNewTax").ToList();
					if (newList2.Count > 0)
					{
						if (Conversion.ToBool((object)newList2[0].Value.ToString()))
						{
							if (DocumentTypeId == 3)
							{
								receiptsVoucher2.Tag = "frmCashReceiptVoucherTax";
							}
							else
							{
								receiptsVoucher2.Tag = "frmBankReceiptVoucherTax";
							}
							receiptsVoucher2.Show();
							receiptsVoucher2.ReadById(Id);
						}
						else
						{
							switch (DocumentTypeId)
							{
							case 3:
							{
								Architecture.WinApp.Account_Definition.ReceiptsVoucherNew Pv5 = new Architecture.WinApp.Account_Definition.ReceiptsVoucherNew(UserAccount);
								if (clsGlobalVariables.ScreenViewReights.Count > 0)
								{
									newList2 = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmCashReceiptVoucherTax").ToList();
									if (!Conversion.ToBool((object)newList2[0].Value.ToString()))
									{
										MessageBox.Show("You don't have right");
										return;
									}
									Pv5.Tag = "frmCashReceiptVoucherTax";
									Pv5.Show();
									Pv5.ReadById(Id);
								}
								break;
							}
							case 4:
							{
								Architecture.WinApp.Account_Definition.ReceiptsVoucherNew Pv6 = new Architecture.WinApp.Account_Definition.ReceiptsVoucherNew(UserAccount);
								if (clsGlobalVariables.ScreenViewReights.Count > 0)
								{
									newList2 = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmBankReceiptVoucherTax").ToList();
									if (!Conversion.ToBool((object)newList2[0].Value.ToString()))
									{
										MessageBox.Show("You don't have right");
										return;
									}
									Pv6.Tag = "frmBankReceiptVoucherTax";
									Pv6.Show();
									Pv6.ReadById(Id);
								}
								break;
							}
							default:
								MessageBox.Show("You don't have right");
								return;
							}
						}
					}
					else if (DocumentTypeId == 3)
					{
						Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew Pv7 = new Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew(UserAccount);
						if (clsGlobalVariables.ScreenViewReights.Count > 0)
						{
							newList2 = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmCashReceiptVoucherTax").ToList();
							if (!Conversion.ToBool((object)newList2[0].Value.ToString()))
							{
								MessageBox.Show("You don't have right");
								return;
							}
							Pv7.Tag = "frmCashReceiptVoucherTax";
							Pv7.Show();
							Pv7.ReadById(Id);
						}
					}
					else
					{
						Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew Pv8 = new Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew(UserAccount);
						if (clsGlobalVariables.ScreenViewReights.Count > 0)
						{
							newList2 = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmBankReceiptVoucherTax").ToList();
							if (!Conversion.ToBool((object)newList2[0].Value.ToString()))
							{
								MessageBox.Show("You don't have right");
								return;
							}
							Pv8.Tag = "frmBankReceiptVoucherTax";
							Pv8.Show();
							Pv8.ReadById(Id);
						}
					}
				}
			}
			else if (DocumentTypeId == 5)
			{
				switch (BaseDocumentTypeId)
				{
				case 0:
				{
					VoucherEntry obj = new VoucherEntry(UserAccount);
					if (clsGlobalVariables.UserAccount.RoleName == "Admin")
					{
						obj.Show();
						obj.ReadById(Id);
						break;
					}
					if (CheckScreenRight("VoucherEntry"))
					{
						obj.Show();
						obj.ReadById(Id);
						break;
					}
					MessageBox.Show("You don't have right");
					return;
				}
				case 4:
				{
					JournalVoucher obj2 = new JournalVoucher(UserAccount);
					if (clsGlobalVariables.UserAccount.RoleName == "Admin")
					{
						obj2.Show();
						obj2.ReadById(Id);
						break;
					}
					if (CheckScreenRight("JournalVoucher"))
					{
						obj2.Show();
						obj2.ReadById(Id);
						break;
					}
					MessageBox.Show("You don't have right");
					return;
				}
				}
			}
			else if (DocumentTypeId == 6)
			{
				frmBillsPayables obj3 = new frmBillsPayables(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					obj3.Show();
					obj3.ReadById(Id);
				}
				else
				{
					if (!CheckScreenRight("frmBillsPayables"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					obj3.Show();
					obj3.ReadById(Id);
				}
			}
			else if (DocumentTypeId == 7)
			{
				frmBillsReceivables obj4 = new frmBillsReceivables(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					obj4.Show();
					obj4.ReadById(Id);
				}
				else
				{
					if (!CheckScreenRight("frmBillsReceivables"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					obj4.Show();
					obj4.ReadById(Id);
				}
			}
			else if (DocumentTypeId == 34)
			{
				frmPartyReceiptVoucher obj5 = new frmPartyReceiptVoucher(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					obj5.Show();
					obj5.ReadById(Id);
				}
				else
				{
					if (!CheckScreenRight("frmPartyReceiptVoucher"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					obj5.Show();
					obj5.ReadById(Id);
				}
			}
			else if (DocumentTypeId == 35)
			{
				frmPartyPaymentVoucher obj6 = new frmPartyPaymentVoucher(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					obj6.Show();
					obj6.ReadById(Id);
				}
				else
				{
					if (!CheckScreenRight("frmPartyPaymentVoucher"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					obj6.Show();
					obj6.ReadById(Id);
				}
			}
			else if (DocumentTypeId == 8)
			{
				frmDayBook obj7 = new frmDayBook(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					obj7.Show();
					obj7.ReadById(Id);
				}
				else
				{
					if (!CheckScreenRight("frmDayBook"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					obj7.Show();
					obj7.ReadById(Id);
				}
			}
			else if (DocumentTypeId == 9)
			{
				DayBook obj8 = new DayBook(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					obj8.Show();
					obj8.ReadById(Id);
				}
				else
				{
					if (!CheckScreenRight("DayBook"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					obj8.Show();
					obj8.ReadById(Id);
				}
			}
			else if (DocumentTypeId == 10 && BaseDocumentTypeId == 0)
			{
				Architecture.WinApp.Account_Definition.ContraVoucher obj9 = new Architecture.WinApp.Account_Definition.ContraVoucher(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					obj9.Show();
					obj9.ReadById(Id);
				}
				else
				{
					if (!CheckScreenRight("ContraVoucher"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					obj9.Show();
					obj9.ReadById(Id);
				}
			}
			else if (DocumentTypeId == 10 && BaseDocumentTypeId == 3)
			{
				Architecture.WinApp.Account_Definition.VouchersWithTax.ContraVoucher obj11 = new Architecture.WinApp.Account_Definition.VouchersWithTax.ContraVoucher(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					obj11.Show();
					obj11.ReadById(Id);
				}
				else
				{
					if (!CheckScreenRight("ContraVoucherTax"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					obj11.Show();
					obj11.ReadById(Id);
				}
			}
			else if (DocumentTypeId == 17 && BaseDocumentTypeId == 0)
			{
				JournalVoucher_New obj10 = new JournalVoucher_New(UserAccount);
				if (clsGlobalVariables.UserAccount.RoleName == "Admin")
				{
					obj10.Show();
					obj10.ReadById(Id);
				}
				else
				{
					if (!CheckScreenRight("JournalVoucher_New"))
					{
						MessageBox.Show("You don't have right");
						return;
					}
					obj10.Show();
					obj10.ReadById(Id);
				}
			}
			else
			{
				switch (DocumentTypeId)
				{
				case 14:
					TargetUrl = "Architecture.WinApp.Account_Definition.PaymentByInvoiceAccount";
					screenName = "PaymentByInvoiceAccount";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 16:
					TargetUrl = "Architecture.WinApp.Account_Definition.AcfrmDefPdcManagment";
					screenName = "AcfrmDefPdcManagment";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 24:
					TargetUrl = "Architecture.WinApp.Account_Definition.PostDatedCheqPaymentVouchers";
					screenName = "PostDatedCheqPaymentVouchers";
					ReadById = "ReadById";
					IdForEdit = Id;
					break;
				case 26:
					if (BaseDocumentTypeId == 0)
					{
						TargetUrl = "Architecture.WinApp.Account_Definition.ExpenseVoucher";
						screenName = "ExpenseVoucher";
					}
					else
					{
						TargetUrl = "Architecture.WinApp.Account_Definition.VouchersWithTax.ExpenseVoucherNew";
						screenName = "ExpenseVoucherNew";
					}
					ReadById = "ReadById";
					IdForEdit = Id;
					break;
				case 27:
					TargetUrl = "Architecture.WinApp.Account_Definition.frmExportInvoiceVoucher";
					screenName = "frmExportInvoiceVoucher";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 155:
					TargetUrl = "Architecture.WinApp.Service.ExImClearingAgentBillDirect";
					screenName = "ExImClearingAgentBillDirect";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 29:
					TargetUrl = "Architecture.WinApp.Account_Definition.ReceiptByContract";
					screenName = "ReceiptByContract";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 39:
					screenName = ((BaseDocumentTypeId != 2) ? "frmStoreOpeningStockBalancing" : "frmPackingMaterialOpeningStockBalance");
					TargetUrl = "Architecture.WinApp.Inventory_Definition.frmStoreOpeningStockBalancing";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 40:
					TargetUrl = "Architecture.WinApp.Inventory_Definition.frmOpeningStockBlancing";
					screenName = "frmOpeningStockBlancing";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 41:
					TargetUrl = "Architecture.WinApp.Purchase.PurchsaeOrder";
					screenName = "PurchsaeOrder";
					ReadById = "ReadById";
					IdForEdit = Id;
					break;
				case 46:
					TargetUrl = "Architecture.WinApp.Purchase.InvFrmGRN";
					screenName = "InvFrmGRN";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 137:
					TargetUrl = "Architecture.WinApp.Purchase.InvFrmGRNDirect";
					screenName = "InvFrmGRNDirect";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 51:
					TargetUrl = "Architecture.WinApp.Purchase.InwardGatePass";
					screenName = "InwardGatePass";
					ReadById = "ReadById";
					IdForEdit = Id;
					break;
				case 56:
					TargetUrl = "Architecture.WinApp.Purchase.InvfrmPurchaseInvoice";
					screenName = "InvfrmPurchaseInvoice";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 57:
					TargetUrl = "Architecture.WinApp.Purchase.InvfrmPurchasedirectInvoice";
					screenName = "InvfrmPurchasedirectInvoice";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 58:
					TargetUrl = "Architecture.WinApp.StoreManagement.PurchaseInvoiceStoreManagement";
					screenName = "PurchaseInvoiceStoreManagement";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 59:
					TargetUrl = "Architecture.WinApp.Purchase.InvfrmInvPurchaseInvoiceReturn";
					screenName = "InvfrmPurchaseReturn";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 61:
				case 131:
					TargetUrl = "Architecture.WinApp.StoreManagement.frmPurchaseInvoiceDirectStore";
					screenName = "frmPurchaseInvoiceDirectStore";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 64:
					TargetUrl = "Architecture.WinApp.StoreManagement.PurchaseInvoiceStoreManagement";
					screenName = "PurchaseInvoiceStoreManagement";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 70:
					TargetUrl = "Architecture.WinApp.StoreManagement.frmStockAdjustment";
					screenName = "frmStockAdjustment";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 80:
					if (RefDocumentTypeId == 401 && BaseDocumentTypeId != 2)
					{
						TargetUrl = "Architecture.WinApp.Production.ProductionAgainstPreCostingJobOrder";
						screenName = "ProductionAgainstPreCostingJobOrder";
						ReadById = "ReadByIdInput";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					else
					{
						TargetUrl = "Architecture.WinApp.Production.frmProductionInput";
						screenName = "FoodProductionWithValues";
						ReadById = "ReadByIdInput";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					break;
				case 126:
					if (BaseDocumentTypeId == 1)
					{
						TargetUrl = "Architecture.WinApp.StoreManagement.InvfrmInvSaleInvoiceDirectPackingMaterial";
						screenName = "frmSaleInvoiceDirectStore";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					else
					{
						TargetUrl = "Architecture.WinApp.StoreManagement.InvfrmInvSaleInvoiceDirectPackingMaterial";
						screenName = "FrmSaleInvoiceDirectPackingMaterial";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					break;
				case 81:
					TargetUrl = "Architecture.WinApp.Sale.SaleOrder";
					screenName = "SaleOrder";
					ReadById = "ReadById";
					IdForEdit = Id;
					break;
				case 84:
					TargetUrl = "Architecture.WinApp.Sale.DeliveryOrder";
					screenName = "DeliveryOrder";
					ReadById = "ReadById";
					IdForEdit = Id;
					break;
				case 85:
					TargetUrl = "Architecture.WinApp.StoreManagement.DeliveryOrderPackingMaterial";
					screenName = "DeliveryOrderPackingMaterial";
					ReadById = "ReadById";
					IdForEdit = Id;
					break;
				case 86:
					TargetUrl = "Architecture.WinApp.Sale.InvFrmGDN";
					screenName = "InvFrmGDN";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 91:
					TargetUrl = "Architecture.WinApp.Sale.OutwardGatePass";
					screenName = "OutwardGatePass";
					ReadById = "ReadById";
					IdForEdit = Id;
					break;
				case 95:
					TargetUrl = "Architecture.WinApp.Sale.InvfrmSaleInvoice";
					screenName = "InvfrmSaleInvoice";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 98:
					TargetUrl = "Architecture.WinApp.Sale.InvfrmSaleInvoiceReturn";
					screenName = "InvfrmSaleInvoiceReturn";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 99:
					if (BaseDocumentTypeId == 0)
					{
						TargetUrl = "Architecture.WinApp.Sale.InvfrmInvSaleInvoiceDirect";
						screenName = "InvfrmInvSaleInvoiceDirect";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					break;
				case 101:
					TargetUrl = "Architecture.WinApp.Contractor_Wages.frmwagesBillHeader";
					screenName = "frmwagesBillHeader";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 112:
					if (RefDocumentTypeId == 401 && BaseDocumentTypeId != 2)
					{
						TargetUrl = "Architecture.WinApp.Production.ProductionAgainstPreCostingJobOrder";
						screenName = "ProductionAgainstPreCostingJobOrder";
						ReadById = "ReadByIdOutPut";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					else
					{
						TargetUrl = "Architecture.WinApp.Production.frmProductionOutput";
						screenName = "FoodProductionWithValues";
						ReadById = "ReadByIdOutPut";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					break;
				case 181:
					if (RefDocumentTypeId == 401 && BaseDocumentTypeId != 2)
					{
						TargetUrl = "Architecture.WinApp.Production.ProductionAgainstPreCostingJobOrder";
						screenName = "ProductionAgainstPreCostingJobOrder";
						ReadById = "ReadByIdConsumption";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					else
					{
						TargetUrl = "Architecture.WinApp.Production.FoodProductionWithValues";
						screenName = "FoodProductionWithValues";
						ReadById = "ReadByIdConsumption";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					break;
				case 111:
					if (RefDocumentTypeId == 401 && BaseDocumentTypeId != 2)
					{
						TargetUrl = "Architecture.WinApp.Production.ProductionAgainstPreCostingJobOrder";
						screenName = "ProductionAgainstPreCostingJobOrder";
						ReadById = "ReadByIdPackingMaterial";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					else
					{
						TargetUrl = "Architecture.WinApp.Production.frmProductionPackingMaterial";
						screenName = "FoodProductionWithValues";
						ReadById = "ReadByIdPackingMaterial";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					}
					break;
				case 110:
					if (RefDocumentTypeId == 401 && BaseDocumentTypeId != 2)
					{
						TargetUrl = "Architecture.WinApp.Production.ProductionAgainstPreCostingJobOrder";
						screenName = "ProductionAgainstPreCostingJobOrder";
						ReadById = "ReadByIdOverHeadJobOrderWise";
						IdForEdit = InvFoodProduction.JobOrderIdByOverHeadId(DocumentTypeIdSrNo);
					}
					else
					{
						TargetUrl = "Architecture.WinApp.Production.frmProductionOverhead";
						screenName = "FoodProductionWithValues";
						ReadById = "ReadByIdOverHeadJobOrderWise";
						IdForEdit = InvFoodProduction.JobOrderIdByOverHeadId(DocumentTypeIdSrNo);
					}
					break;
				case 124:
					TargetUrl = "Architecture.WinApp.Sale.frmGdnDirect";
					screenName = "frmGdnDirect";
					ReadById = "ReadById";
					IdForEdit = Id;
					break;
				case 170:
					TargetUrl = "Architecture.WinApp.Sale.frmGdnAgainstSaleOrder";
					screenName = "frmGdnAgainstSaleOrder";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 66:
					TargetUrl = "Architecture.WinApp.Production.invfrmStockConversionProduction";
					screenName = "invfrmStockConversionProduction";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 68:
					TargetUrl = "Architecture.WinApp.StoreManagement.frmStockTransfer";
					screenName = "frmStockTransfer";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 806:
					TargetUrl = "Architecture.WinApp.StoreManagement.frmStockTransferManual";
					screenName = "frmStockTransferManual";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				default:
					switch (DocumentTypeId)
					{
					case 126:
						switch (BaseDocumentTypeId)
						{
						case 1:
							screenName = "frmSaleInvoiceDirectStore";
							break;
						case 2:
							screenName = "FrmSaleInvoiceDirectPackingMaterial";
							break;
						}
						TargetUrl = "Architecture.WinApp.StoreManagement.InvfrmInvSaleInvoiceDirectPackingMaterial";
						ReadById = "ReadById";
						IdForEdit = Id;
						break;
					case 145:
						switch (BaseDocumentTypeId)
						{
						case 1:
							TargetUrl = "Architecture.WinApp.StoreManagement.PurchaseInvoiceReturn_Store";
							screenName = "PurchaseInvoiceReturn_Store";
							ReadById = "ReadById";
							IdForEdit = DocumentTypeIdSrNo;
							break;
						case 2:
							TargetUrl = "Architecture.WinApp.StoreManagement.PurchaseInvoiceReturn_Store";
							screenName = "PurchaseInvoiceReturnPM";
							ReadById = "ReadById";
							IdForEdit = DocumentTypeIdSrNo;
							break;
						}
						break;
					case 127:
						TargetUrl = "Architecture.WinApp.Sale.BookingOrder";
						screenName = "BookingOrder";
						ReadById = "ReadById";
						IdForEdit = Id;
						break;
					case 138:
						TargetUrl = "Architecture.WinApp.Purchase.frmPurchaseInvoiceAgaintGrnDirect";
						screenName = "PurchaseInvoiceAgainstGrnDirect";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					case 139:
						TargetUrl = "Architecture.WinApp.Sale.frmSaleInvoiceDirect139";
						screenName = "frmSaleInvoiceDirect139";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					case 162:
						TargetUrl = "Architecture.WinApp.CommissionAgent.CommissionAgentTradeBill_162";
						screenName = "CommissionAgentTradeBill_162";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					case 1056:
						TargetUrl = "Architecture.WinApp.Cmagt.frmCommissionAgentTradeBillAgainstGdn";
						screenName = "frmCommissionAgentTradeBillAgainstGdn";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					case 166:
						TargetUrl = "Architecture.WinApp.Paddy_Purchase.frmPurchaseInvoiceForGate";
						screenName = "frmPurchaseInvoiceForGate";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					case 171:
						TargetUrl = "Architecture.WinApp.Sale.frmSaleInvoiceAgainstGdnWithoutWb";
						screenName = "frmSaleInvoiceAgainstGdnWithoutWb";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					case 172:
						TargetUrl = "Architecture.WinApp.Purchase.PurchaseInvoiceAgainstGrnOrder";
						screenName = "PurchaseInvoiceAgainstGrnOrder";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					case 203:
						TargetUrl = "Architecture.WinApp.Account_Definition.Acfrmfcbankreceipt";
						screenName = "Acfrmfcbankreceipt";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					case 204:
						TargetUrl = "Architecture.WinApp.Export.ExImCommercialInvoice";
						screenName = "EximInvoice";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					case 205:
						TargetUrl = "Architecture.WinApp.Export.EximForwarding";
						screenName = "EximForwarding";
						ReadById = "ReadById";
						IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
						break;
					default:
						switch (DocumentTypeId)
						{
						case 205:
							TargetUrl = "Architecture.WinApp.Export.frmForwardingNew";
							screenName = "frmForwardingNew";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 452:
							switch (BaseDocumentTypeId)
							{
							case 0:
								screenName = "StoreIssuanceDirect";
								break;
							case 1:
								screenName = "StoreIssuanceDirectStore";
								break;
							case 2:
								screenName = "StoreIssuanceDirectPM";
								break;
							}
							TargetUrl = "Architecture.WinApp.StoreManagement.StoreIssuanceDirect";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 810:
							TargetUrl = "Architecture.WinApp.Contractor_Wages.frmWagesBillManual";
							screenName = "frmWagesBillManual";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 702:
							TargetUrl = "Architecture.WinApp.PackingMaterial_Store.PurchaseInvoicePackingMaterial";
							screenName = "PurchaseInvoicePackingMaterial";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1304:
							TargetUrl = "Architecture.WinApp.Service.lgstcm.frmLogisticPurchaseServicesBill";
							screenName = "frmLogisticPurchaseServicesBill";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1500:
							TargetUrl = "Architecture.WinApp.Steel.Purchase.PurchsaeOrder_St";
							screenName = "PurchsaeOrder_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1501:
							TargetUrl = "Architecture.WinApp.Steel.Purchase.InvFrmGRN_St";
							screenName = "InvFrmGRN_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1502:
							TargetUrl = "Architecture.WinApp.Steel.Purchase.InvfrmPurchaseInvoice_St";
							screenName = "InvfrmPurchaseInvoice_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1503:
							TargetUrl = "Architecture.WinApp.Steel.Purchase.PurchaseInvoiceDirect_St";
							screenName = "PurchaseInvoiceDirect_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1505:
							TargetUrl = "Architecture.WinApp.Steel.Sale.SaleOrder_St";
							screenName = "SaleOrder_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1506:
							TargetUrl = "Architecture.WinApp.Steel.Sale.DeliveryOrder_St";
							screenName = "DeliveryOrder_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1507:
							TargetUrl = "Architecture.WinApp.Steel.Sale.OutwardGatePass_St";
							screenName = "OutwardGatePass_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1508:
							TargetUrl = "Architecture.WinApp.Steel.Sale.InvFrmGDN_St";
							screenName = "InvFrmGDN_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1509:
							TargetUrl = "Architecture.WinApp.Steel.Sale.SaleInvoice_St";
							screenName = "SaleInvoice_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1510:
							TargetUrl = "Architecture.WinApp.Steel.Sale.SaleInvoiceDirect_St";
							screenName = "SaleInvoiceDirect_St";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1600:
							TargetUrl = "Architecture.WinApp.PurchaseTrading.PurchsaeOrderWithQty";
							screenName = "PurchsaeOrderWithQty";
							ReadById = "ReadById";
							IdForEdit = Id;
							break;
						case 1602:
							TargetUrl = "Architecture.WinApp.PurchaseTrading.InvFrmGRN_Qty";
							screenName = "InvFrmGRN_Qty";
							ReadById = "ReadById";
							IdForEdit = Id;
							break;
						case 1603:
							TargetUrl = "Architecture.WinApp.PurchaseTrading.PurchaseInvoiceTradingWithTax";
							screenName = "PurchaseInvoiceTradingWithTax";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1604:
							TargetUrl = "Architecture.WinApp.PurchaseTrading.PurchaseInvoiceTradePro";
							screenName = "PurchaseInvoiceTradePro";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1605:
							TargetUrl = "Architecture.WinApp.SaleTrading.SaleOrderWithQty";
							screenName = "SaleOrderWithQty";
							ReadById = "ReadById";
							IdForEdit = Id;
							break;
						case 1656:
							TargetUrl = "Architecture.WinApp.Mfg.Sale.frmSaleOrderEngr";
							screenName = "frmSaleOrderEngr";
							ReadById = "ReadById";
							IdForEdit = Id;
							break;
						case 1606:
							TargetUrl = "Architecture.WinApp.SaleTrading.DeliveryOrder_Engr";
							screenName = "DeliveryOrder_Engr";
							ReadById = "ReadById";
							IdForEdit = Id;
							break;
						case 1657:
							TargetUrl = "Architecture.WinApp.Mfg.frmDeliveryOrderEngr";
							screenName = "frmDeliveryOrderEngr";
							ReadById = "ReadById";
							IdForEdit = Id;
							break;
						case 1608:
							TargetUrl = "Architecture.WinApp.SaleTrading.SaleInvoiceQtyWithTax";
							screenName = "SaleInvoiceQtyWithTax";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1609:
							TargetUrl = "Architecture.WinApp.SaleTrading.SaleInvoiceTrading_Engr";
							screenName = "SaleInvoiceTrading_Engr";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1611:
							TargetUrl = "Architecture.WinApp.SaleTrading.frmSaleInvoiceReturn_Engr";
							screenName = "frmSaleInvoiceReturn_Engr";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1654:
							TargetUrl = "Architecture.WinApp.Mfg.Purchase.frmPurchaseInvoiceDirectEngr";
							screenName = "frmPurchaseInvoiceDirectEngr";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1660:
							TargetUrl = "Architecture.WinApp.Mfg.Sale.frmSaleInvoiceEngr";
							screenName = "frmSaleInvoiceEngr";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1661:
							TargetUrl = "Architecture.WinApp.Mfg.Sale.frmSaleInvoiceDirectEngr";
							screenName = "frmSaleInvoiceDirectEngr";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1662:
							TargetUrl = "Architecture.WinApp.Mfg.Sale.frmSaleInvoiceReturnEngr";
							screenName = "frmSaleInvoiceReturnEngr";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1850:
							TargetUrl = "Architecture.WinApp.pcc.frmConcreteProduction";
							screenName = "frmConcreteProduction";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1856:
							TargetUrl = "Architecture.WinApp.pcc.Sale.SaleInvoiceDirectConcrete";
							screenName = "SaleInvoiceDirectConcrete";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1859:
							TargetUrl = "Architecture.WinApp.pcc.purchase.PurchaseInvoiceDirectConcrete";
							screenName = "PurchaseInvoiceDirectConcrete";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1861:
							TargetUrl = "Architecture.WinApp.pcc.Sale.SaleInvoiceAgainstGDNConcrete";
							screenName = "SaleInvoiceAgainstGDNConcrete";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1862:
							TargetUrl = "Architecture.WinApp.pcc.Sale.SaleInvoiceReturnConcrete";
							screenName = "SaleInvoiceReturnConcrete";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1865:
							TargetUrl = "Architecture.WinApp.pcc.purchase.PurchaseInvoiceReturn";
							screenName = "PurchaseInvoiceReturn";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1000:
							TargetUrl = "Architecture.WinApp.HRM.PayRoll.frmPayrollPosting";
							screenName = "PayrollPosting";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1001:
							TargetUrl = "Architecture.WinApp.HRM.LoanManagement.frmEmployeeLoan";
							screenName = "EmployeeLoan";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						case 1104:
							TargetUrl = "Architecture.WinApp.FeedMill.Purchase.PurchaseInvoiceDirect";
							screenName = "PurchaseInvoiceDirect";
							ReadById = "ReadById";
							IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
							break;
						}
						break;
					}
					break;
				case 11:
				case 12:
				case 97:
					break;
				}
			}
			if (string.IsNullOrEmpty(TargetUrl) || string.IsNullOrEmpty(screenName) || string.IsNullOrEmpty(ReadById))
			{
				return;
			}
			if (clsGlobalVariables.UserAccount.RoleName == "Admin")
			{
				Type typ = Type.GetType(TargetUrl);
				if (typ != null)
				{
					UserAccount.UserAccountAllocationsList = new List<UserAccountAllocation>();
					UserAccountAllocation allocation = new UserAccountAllocation();
					allocation.ScreenName = screenName;
					UserAccount.UserAccountAllocationsList.Add(allocation);
					Form form2 = Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount) as Form;
					if (form2 == null)
					{
						return;
					}
					form2.WindowState = FormWindowState.Maximized;
					form2.Tag = Conversion.ToString((object)screenName);
					form2.Show();
					MethodInfo method2 = typ.GetMethod(ReadById, new Type[1] { typeof(int) });
					if (method2 != null)
					{
						object[] parameters2 = new object[1] { IdForEdit };
						bool invoked = false;
						FieldInfo actionField = typ.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((FieldInfo f) => f.FieldType == typeof(Action));
						if (actionField != null)
						{
							Action existing = actionField.GetValue(form2) as Action;
							actionField.SetValue(form2, (Action)Delegate.Combine(existing, (Action)delegate
							{
								method2.Invoke(form2, parameters2);
							}));
							invoked = true;
						}
						if (!invoked)
						{
							FieldInfo initializedField2 = typ.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((FieldInfo f) => f.FieldType == typeof(bool) && f.Name.ToLower().Contains("initialized"));
							if (initializedField2 != null)
							{
								Timer pollTimer2 = new Timer();
								pollTimer2.Interval = 100;
								pollTimer2.Tick += delegate
								{
									if ((bool)initializedField2.GetValue(form2))
									{
										pollTimer2.Stop();
										pollTimer2.Dispose();
										method2.Invoke(form2, parameters2);
									}
								};
								pollTimer2.Start();
								invoked = true;
							}
						}
						if (!invoked)
						{
							method2.Invoke(form2, parameters2);
						}
					}
					else
					{
						MessageBox.Show("Method not found on form " + typ.FullName);
					}
					return;
				}
				throw new Exception("Target URL not Configured properly... " + TargetUrl);
			}
			if (CheckScreenRight(screenName))
			{
				Type typ2 = Type.GetType(TargetUrl);
				if (typ2 != null)
				{
					Form form = Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount) as Form;
					if (form == null)
					{
						return;
					}
					form.WindowState = FormWindowState.Maximized;
					form.Tag = Conversion.ToString((object)screenName);
					form.Show();
					MethodInfo method = typ2.GetMethod(ReadById, new Type[1] { typeof(int) });
					if (method != null)
					{
						object[] parameters = new object[1] { IdForEdit };
						bool invoked2 = false;
						FieldInfo actionField2 = typ2.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((FieldInfo f) => f.FieldType == typeof(Action));
						if (actionField2 != null)
						{
							Action existing2 = actionField2.GetValue(form) as Action;
							actionField2.SetValue(form, (Action)Delegate.Combine(existing2, (Action)delegate
							{
								method.Invoke(form, parameters);
							}));
							invoked2 = true;
						}
						if (!invoked2)
						{
							FieldInfo initializedField = typ2.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((FieldInfo f) => f.FieldType == typeof(bool) && f.Name.ToLower().Contains("initialized"));
							if (initializedField != null)
							{
								Timer pollTimer = new Timer();
								pollTimer.Interval = 100;
								pollTimer.Tick += delegate
								{
									if ((bool)initializedField.GetValue(form))
									{
										pollTimer.Stop();
										pollTimer.Dispose();
										method.Invoke(form, parameters);
									}
								};
								pollTimer.Start();
								invoked2 = true;
							}
						}
						if (!invoked2)
						{
							method.Invoke(form, parameters);
						}
					}
					else
					{
						MessageBox.Show("Method not found on form " + typ2.FullName);
					}
					return;
				}
				throw new Exception("Target URL not Configured properly... " + TargetUrl);
			}
			throw new Exception("You don't have right");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportEditMethodFromLinked(int DocumentTypeId, int Id, int DocumentTypeIdSrNo = 0, int BaseDocumentTypeId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UserAccount UserAccount = new UserAccount();
			UserAccount = clsGlobalVariables.UserAccount;
			string TargetUrl = "";
			string screenName = "";
			string ReadById = "";
			int IdForEdit = 0;
			switch (DocumentTypeId)
			{
			case 27:
				TargetUrl = "Architecture.WinApp.Account_Definition.frmExportInvoiceVoucher";
				screenName = "frmExportInvoiceVoucher";
				ReadById = "ReadById";
				IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
				break;
			case 29:
				TargetUrl = "Architecture.WinApp.Account_Definition.ReceiptByContract";
				screenName = "ReceiptByContract";
				ReadById = "ReadById";
				IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
				break;
			case 203:
				TargetUrl = "Architecture.WinApp.Account_Definition.Acfrmfcbankreceipt";
				screenName = "Acfrmfcbankreceipt";
				ReadById = "ReadById";
				IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
				break;
			case 204:
				TargetUrl = "Architecture.WinApp.Export.ExImCommercialInvoice";
				screenName = "EximInvoice";
				ReadById = "ReadById";
				IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
				break;
			case 205:
				TargetUrl = "Architecture.WinApp.Export.EximForwarding";
				screenName = "EximForwarding";
				ReadById = "ReadById";
				IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
				break;
			default:
				switch (DocumentTypeId)
				{
				case 205:
					TargetUrl = "Architecture.WinApp.Export.frmForwardingNew";
					screenName = "frmForwardingNew";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				case 212:
					TargetUrl = "Architecture.WinApp.Export.CommiercialInvoiceForOpeningBalance";
					screenName = "CommiercialInvoiceForOpeningBalance";
					ReadById = "ReadById";
					IdForEdit = ((DocumentTypeIdSrNo > 0) ? DocumentTypeIdSrNo : Id);
					break;
				}
				break;
			case 97:
				break;
			}
			if (string.IsNullOrEmpty(TargetUrl) || string.IsNullOrEmpty(screenName) || string.IsNullOrEmpty(ReadById))
			{
				return;
			}
			if (clsGlobalVariables.UserAccount.RoleName == "Admin")
			{
				Type typ = Type.GetType(TargetUrl);
				if (typ != null)
				{
					Form form2 = Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount) as Form;
					if (form2 == null)
					{
						return;
					}
					form2.WindowState = FormWindowState.Maximized;
					form2.Tag = Conversion.ToString((object)screenName);
					form2.Show();
					MethodInfo method2 = typ.GetMethod(ReadById, new Type[1] { typeof(int) });
					if (method2 != null)
					{
						object[] parameters2 = new object[1] { IdForEdit };
						bool invoked = false;
						FieldInfo actionField = typ.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((FieldInfo f) => f.FieldType == typeof(Action));
						if (actionField != null)
						{
							Action existing = actionField.GetValue(form2) as Action;
							actionField.SetValue(form2, (Action)Delegate.Combine(existing, (Action)delegate
							{
								method2.Invoke(form2, parameters2);
							}));
							invoked = true;
						}
						if (!invoked)
						{
							FieldInfo initializedField2 = typ.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((FieldInfo f) => f.FieldType == typeof(bool) && f.Name.ToLower().Contains("initialized"));
							if (initializedField2 != null)
							{
								Timer pollTimer2 = new Timer();
								pollTimer2.Interval = 100;
								pollTimer2.Tick += delegate
								{
									if ((bool)initializedField2.GetValue(form2))
									{
										pollTimer2.Stop();
										pollTimer2.Dispose();
										method2.Invoke(form2, parameters2);
									}
								};
								pollTimer2.Start();
								invoked = true;
							}
						}
						if (!invoked)
						{
							method2.Invoke(form2, parameters2);
						}
					}
					else
					{
						MessageBox.Show("Method not found on form " + typ.FullName);
					}
					return;
				}
				throw new Exception("Target URL not Configured properly... " + TargetUrl);
			}
			if (CheckScreenRight(screenName))
			{
				Type typ2 = Type.GetType(TargetUrl);
				if (typ2 != null)
				{
					Form form = Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount) as Form;
					if (form == null)
					{
						return;
					}
					form.WindowState = FormWindowState.Maximized;
					form.Tag = Conversion.ToString((object)screenName);
					form.Show();
					MethodInfo method = typ2.GetMethod(ReadById, new Type[1] { typeof(int) });
					if (method != null)
					{
						object[] parameters = new object[1] { IdForEdit };
						bool invoked2 = false;
						FieldInfo actionField2 = typ2.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((FieldInfo f) => f.FieldType == typeof(Action));
						if (actionField2 != null)
						{
							Action existing2 = actionField2.GetValue(form) as Action;
							actionField2.SetValue(form, (Action)Delegate.Combine(existing2, (Action)delegate
							{
								method.Invoke(form, parameters);
							}));
							invoked2 = true;
						}
						if (!invoked2)
						{
							FieldInfo initializedField = typ2.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((FieldInfo f) => f.FieldType == typeof(bool) && f.Name.ToLower().Contains("initialized"));
							if (initializedField != null)
							{
								Timer pollTimer = new Timer();
								pollTimer.Interval = 100;
								pollTimer.Tick += delegate
								{
									if ((bool)initializedField.GetValue(form))
									{
										pollTimer.Stop();
										pollTimer.Dispose();
										method.Invoke(form, parameters);
									}
								};
								pollTimer.Start();
								invoked2 = true;
							}
						}
						if (!invoked2)
						{
							method.Invoke(form, parameters);
						}
					}
					else
					{
						MessageBox.Show("Method not found on form " + typ2.FullName);
					}
					return;
				}
				throw new Exception("Target URL not Configured properly... " + TargetUrl);
			}
			throw new Exception("You don't have right");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static bool CheckScreenRight(string RightName)
	{
		try
		{
			bool value = false;
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == RightName).ToList();
				if (newList.Count > 0)
				{
					value = Conversion.ToBool((object)newList[0].Value);
				}
			}
			if (value)
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GetNoofAttachmentsByRefDocumentTypeID(int Id, int DocumentTypeId)
	{
		try
		{
			DataTable dtViewAttachments = new DataTable();
			dtViewAttachments.Columns.Add("AttachmentName");
			dtViewAttachments.Columns.Add("CustomName");
			dtViewAttachments.Columns.Add("EntryDate", typeof(DateTime));
			List<DMSAttachments> dMSList = DMSAttachments.ReadAttachmentsbyRefDocumentTypeId(Id, DocumentTypeId);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment, itemList.UploadedFileCustomName, itemList.EntryDate);
				}
				new AttachmentView(dtViewAttachments).Show();
			}
			else
			{
				dtViewAttachments.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GetSaveAndDeleteAttachmentsForRecord(UserAccount user, int Id, int DocumentTypeId, string ScreenName = "", bool ShowSavebutton = false)
	{
		try
		{
			AttachmentViewWithDeleteOption attachmentViewWithDeleteOption = new AttachmentViewWithDeleteOption(user, Id, DocumentTypeId, ScreenName);
			attachmentViewWithDeleteOption.ShowSaveButton = ShowSavebutton;
			attachmentViewWithDeleteOption.BringToFront();
			attachmentViewWithDeleteOption.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GetNoofAttachmentsByScreenName(int Id, string ScreenName)
	{
		try
		{
			DataTable dtViewAttachments = new DataTable();
			dtViewAttachments.Columns.Add("AttachmentName");
			dtViewAttachments.Columns.Add("CustomName");
			dtViewAttachments.Columns.Add("EntryDate", typeof(DateTime));
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(Id, ScreenName);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment, itemList.UploadedFileCustomName, itemList.EntryDate);
				}
				new AttachmentView(dtViewAttachments).Show();
			}
			else
			{
				dtViewAttachments.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToGeneralLedgerFromLinkedEvent(int AccountId, DateTime FromDate, DateTime ToDate, int SubsidiaryAccountId = 0, int BranchId = 0)
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			GeneralLedger obj = new GeneralLedger(userAccount);
			bool value = false;
			if (userAccount.RoleName == "Admin")
			{
				value = true;
			}
			else if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "GeneralLedger").ToList();
				if (newList.Count > 0)
				{
					value = Conversion.ToBool((object)newList[0].Value.ToString());
				}
			}
			if (value)
			{
				GeneralLedger.FromDate = FromDate;
				GeneralLedger.ToDate = ToDate;
				GeneralLedger.GeneralLedgerId = AccountId;
				if (SubsidiaryAccountId > 0)
				{
					obj.CmbSubsidiaryAccount.Value = SubsidiaryAccountId;
				}
				if (GetERPFeatureById(11) && BranchId > 0)
				{
					obj.cmbbranch.Value = BranchId;
				}
				obj.Show();
			}
			else
			{
				MessageBox.Show("you don't have Screen Rights");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	public static void GoToItemLedgerFromLinkedEvent(int AccountId, DateTime FromDate, DateTime ToDate, int ItemId = 0)
	{
		try
		{
			GeneralLedger obj = new GeneralLedger(clsGlobalVariables.UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count <= 0)
			{
				return;
			}
			List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "ItemLedger").ToList();
			if (newList.Count > 0)
			{
				if (Conversion.ToBool((object)newList[0].Value.ToString()))
				{
					GeneralLedger.FromDate = FromDate;
					GeneralLedger.ToDate = ToDate;
					GeneralLedger.ItemLedgerId = AccountId;
					GeneralLedger.ItemLedgerItemId = ItemId;
					obj.Show();
				}
				else
				{
					MessageBox.Show("you dont have Screen Rights");
				}
			}
			else
			{
				MessageBox.Show("you dont have Screen Rights");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	public static void GoToCustomerLedgerFromLinkedEvent(int SupplierCustomerId, DateTime FromDate, DateTime ToDate)
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			bool OpenReport = false;
			CustomerLedger obj = new CustomerLedger(userAccount);
			if (userAccount.RoleName == "Admin")
			{
				OpenReport = true;
			}
			else if (CheckScreenRight("CustomerLedger"))
			{
				OpenReport = true;
			}
			if (OpenReport)
			{
				obj.txtfromdate.Value = FromDate;
				obj.txttodate.Value = ToDate;
				obj.comSupplier.Value = SupplierCustomerId;
				obj.Show();
				return;
			}
			throw new Exception("you don't have Screen Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show("Hello there is an exception " + ex.Message);
		}
	}

	public static void GoToItemEvaluationLedgerFromLinkedEvent(int ItemId, DateTime FromDate, DateTime ToDate)
	{
		try
		{
			bool OpenReport = false;
			UserAccount UserAccount = clsGlobalVariables.UserAccount;
			if (UserAccount.RoleName == "Admin" || (CheckScreenRight("ItemEvaluationLedgerNew") ? true : false))
			{
				ItemEvaluationLedgerNew obj = new ItemEvaluationLedgerNew(UserAccount)
				{
					fromdate = 
					{
						Value = FromDate
					},
					todate = 
					{
						Value = ToDate
					}
				};
				obj.CmbItem.Value = ItemId;
				obj.RequestedByOtherDocument = true;
				obj.Show();
				return;
			}
			throw new Exception("you don't have Screen Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToActivitySummaryFromLinkedEvent(DateTime FromDate, DateTime ToDate, int CompanyId = 0, string BranchesIds = "")
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			ActicitySummery obj = new ActicitySummery(userAccount);
			bool value = false;
			if (userAccount.RoleName == "Admin" || CheckScreenRight("ActicitySummery"))
			{
				obj.FromDate = FromDate;
				obj.ToDate = ToDate;
				obj.CompanyIdFromBreakUp = CompanyId;
				obj.BranchesIds = BranchesIds;
				obj.ShowDialog();
				return;
			}
			throw new Exception("you don't have Screen Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToCashBalancesFromLinkedEvent(DateTime FromDate, DateTime ToDate, int CompanyId = 0, string BranchesIds = "")
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			frmCashBalance obj = new frmCashBalance(userAccount);
			bool value = false;
			if (userAccount.RoleName == "Admin" || CheckScreenRight("CashBalances"))
			{
				obj.FromDate = FromDate;
				obj.ToDate = ToDate;
				obj.CompanyIdFromBreakUp = CompanyId;
				obj.BranchesIds = BranchesIds;
				obj.ShowDialog();
				return;
			}
			throw new Exception("you don't have Screen Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToBankBalancesFromLinkedEvent(DateTime FromDate, DateTime ToDate, int CompanyId = 0, string BranchesIds = "")
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			BankBalances obj = new BankBalances(userAccount);
			bool value = false;
			if (userAccount.RoleName == "Admin" || CheckScreenRight("BankBalances"))
			{
				obj.FromDate = FromDate;
				obj.ToDate = ToDate;
				obj.CompanyIdFromBreakUp = CompanyId;
				List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Bank Control Account").ToList();
				if (list.Count > 0)
				{
					BankBalances.GroupId = Conversion.ToInt((object)list[0].ConfigKey);
				}
				obj.BranchesIds = BranchesIds;
				obj.ShowDialog();
				BankBalances.GroupId = 0;
				return;
			}
			throw new Exception("you don't have Screen Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToTradeDebitorsReportFromLinkedEvent(DateTime FromDate, DateTime ToDate, int CompanyId = 0, bool chkCrAmount = true, bool chkDrAmount = true, string BranchesIds = "")
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			TradeDebitorsReport obj = new TradeDebitorsReport(userAccount);
			bool value = false;
			if (userAccount.RoleName == "Admin" || CheckScreenRight("TradeDebitorsReport"))
			{
				obj.FromDate = FromDate;
				obj.ToDate = ToDate;
				obj.CompanyIdFromBreakUp = CompanyId;
				obj.AccountClass = 2;
				obj.ChkCreditAmount.Checked = chkCrAmount;
				obj.ChkDebitAmount.Checked = chkDrAmount;
				obj.BranchesIds = BranchesIds;
				obj.ShowDialog();
				return;
			}
			throw new Exception("you don't have Screen Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToTradeCreditorsReportFromLinkedEvent(DateTime FromDate, DateTime ToDate, int CompanyId = 0, bool chkCrAmount = true, bool chkDrAmount = true, string BranchesIds = "")
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			TradeDebitorsReport obj = new TradeDebitorsReport(userAccount);
			bool value = false;
			if (userAccount.RoleName == "Admin" || CheckScreenRight("TradeCreditorsReport"))
			{
				obj.Tag = "TradeCreditorsReport";
				obj.FromDate = FromDate;
				obj.ToDate = ToDate;
				obj.CompanyIdFromBreakUp = CompanyId;
				obj.AccountClass = 3;
				obj.ChkCreditAmount.Checked = chkCrAmount;
				obj.ChkDebitAmount.Checked = chkDrAmount;
				obj.BranchesIds = BranchesIds;
				obj.ShowDialog();
				return;
			}
			throw new Exception("you don't have Screen Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToCshAndBankBalancesReportFromLinkedEvent(DateTime FromDate, DateTime ToDate, int CompanyId = 0, string BranchesIds = "")
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			CashAndBankBalances obj = new CashAndBankBalances(userAccount);
			bool value = false;
			if (userAccount.RoleName == "Admin" || CheckScreenRight("CashAndBankBalances"))
			{
				obj.FromDate = FromDate;
				obj.ToDate = ToDate;
				obj.CompanyIdFromBreakUp = CompanyId;
				obj.BranchesIds = BranchesIds;
				obj.ShowDialog();
				return;
			}
			throw new Exception("you don't have Screen Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToFcyGeneralLedgerFromLinkedEvent(int AccountId, DateTime FromDate, DateTime ToDate)
	{
		try
		{
			bool OpenReport = false;
			UserAccount UserAccount = clsGlobalVariables.UserAccount;
			if (UserAccount.RoleName == "Admin")
			{
				OpenReport = true;
			}
			else if (CheckScreenRight("FcyGeneralLedgerRpt"))
			{
				OpenReport = true;
			}
			if (OpenReport)
			{
				FcyGeneralLedgerRpt fcyGeneralLedgerRpt = new FcyGeneralLedgerRpt(UserAccount);
				fcyGeneralLedgerRpt.FromDate = FromDate;
				fcyGeneralLedgerRpt.ToDate = ToDate;
				fcyGeneralLedgerRpt.AccountId = AccountId;
				fcyGeneralLedgerRpt.ShowDialog();
				return;
			}
			throw new Exception("You don't have screen right");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToInventoryPayablesandReceivablesFromLinkedEvent(DateTime FromDate, DateTime ToDate, int ReportTypeStatus = 0, int CompanyId = 0, string BranchesIds = "")
	{
		try
		{
			bool OpenReport = false;
			UserAccount UserAccount = clsGlobalVariables.UserAccount;
			if (UserAccount.RoleName == "Admin")
			{
				OpenReport = true;
			}
			else if (CheckScreenRight("InventoryPayablesandReceivables"))
			{
				OpenReport = true;
			}
			if (OpenReport)
			{
				InventoryPayablesandReceivables inventoryPayablesandReceivables = new InventoryPayablesandReceivables(UserAccount);
				inventoryPayablesandReceivables.FromDate = FromDate;
				inventoryPayablesandReceivables.ToDate = ToDate;
				inventoryPayablesandReceivables.ReportTypeStatus = ReportTypeStatus;
				inventoryPayablesandReceivables.CompanyIdFromBreakUp = CompanyId;
				inventoryPayablesandReceivables.BranchesIds = BranchesIds;
				inventoryPayablesandReceivables.ShowDialog();
				return;
			}
			throw new Exception("You don't have screen right");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToPayablesWithBillAmountFromLinkedEvent(DateTime FromDate, DateTime ToDate, int ReportTypeStatus = 0, int CompanyId = 0, string BranchesIds = "")
	{
		try
		{
			bool OpenReport = false;
			UserAccount UserAccount = clsGlobalVariables.UserAccount;
			if (UserAccount.RoleName == "Admin")
			{
				OpenReport = true;
			}
			else if (CheckScreenRight("PayablesWithBillAmount"))
			{
				OpenReport = true;
			}
			if (OpenReport)
			{
				PayablesWithBillAmount payablesWithBillAmount = new PayablesWithBillAmount(UserAccount);
				payablesWithBillAmount.FromDate = FromDate;
				payablesWithBillAmount.ToDate = ToDate;
				payablesWithBillAmount.CompanyIdFromBreakUp = CompanyId;
				payablesWithBillAmount.BranchesIds = BranchesIds;
				payablesWithBillAmount.ShowDialog();
				return;
			}
			throw new Exception("You don't have screen right");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GoToReceivablesFromLinkedEvent(DateTime FromDate, DateTime ToDate, int CompanyId = 0, string BranchesIds = "")
	{
		try
		{
			bool OpenReport = false;
			UserAccount UserAccount = clsGlobalVariables.UserAccount;
			if (UserAccount.RoleName == "Admin")
			{
				OpenReport = true;
			}
			else if (CheckScreenRight("Receivables"))
			{
				OpenReport = true;
			}
			if (OpenReport)
			{
				Receivables receivables = new Receivables(UserAccount);
				receivables.FromDate = FromDate;
				receivables.ToDate = ToDate;
				receivables.CompanyIdFromBreakUp = CompanyId;
				receivables.BranchesIds = BranchesIds;
				receivables.ShowDialog();
				return;
			}
			throw new Exception("You don't have screen right");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void OpenPdcTransactionPayment(DateTime FromDate, DateTime ToDate, int AccountId = 0, bool CreditAccount = false)
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			frmPdcTransactionPayment obj = new frmPdcTransactionPayment(userAccount);
			bool value = false;
			if (userAccount.RoleName == "Admin" || CheckScreenRight("frmPdcTransactionPayment"))
			{
				obj.FromDate = FromDate;
				obj.ToDate = ToDate;
				if (CreditAccount)
				{
					obj.CreditAccountId = AccountId;
				}
				else
				{
					obj.DebitAccountId = AccountId;
				}
				obj.RequestedByOtherDocument = true;
				obj.ShowDialog();
				return;
			}
			throw new Exception("you don't have Screen Rights");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static double GetCurrentGlBalance(int AccountId, DateTime DocDate)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return VoucherHead.GetCurrentGLAccountBalance(new VoucherHead
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				RefAccountId = AccountId,
				VoucherDate = DocDate
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAccountsBalancesOpeningCurrentAndClosing(int AccountId, DateTime FromDate, DateTime ToDate, int CostCenterId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		try
		{
			return VoucherHead.GetAccountsBalancesOpeningCurrentAndClosing(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				AccountId = AccountId,
				FromDate = FromDate,
				ToDate = ToDate,
				CostCenterId = CostCenterId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSubsidiaryLedgerBalance(int SupplierCustomerId, DateTime ToDate)
	{
		try
		{
			int organizationId = clsGlobalVariables.UserAccount.OrganizationId;
			int CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			return VoucherHead.GetSubsidiaryLedgerBalance(organizationId, CompanyId, SupplierCustomerId, ToDate);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReferencePartyServiceBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return ReferenceParties.GetAll(new ReferenceParties
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GatePassReferenceTypes()
	{
		try
		{
			return GatePassPartyProcessing.GetGatePassReferenceType(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemPartyProcessing(string ParentCategoryIds = "")
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new ReportsParameters();
			int organizationId = clsGlobalVariables.UserAccount.OrganizationId;
			int CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			return Item.ReadAllForItemsForPartyProcessing(organizationId, CompanyId, ParentCategoryIds);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable WareHousePartyProcessingGetAllService()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return InvWareHousePartyProcessing.Getall(new InvWareHousePartyProcessing
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable JobLotPartyProcessingGetAllService()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return JobLotPartyProcessing.GetAll(new JobLotPartyProcessing
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUomScheduleByItemId(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			DataTable dtuom = UOMSchedule.SearchByObject(new UOMSchedule
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemId = ItemId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("UOMCode");
			dt.Columns.Add("Equivalent", typeof(double));
			dt.Columns.Add("QtyEquivalent", typeof(double));
			dt.Columns.Add("BaseRateUom", typeof(bool));
			if (dtuom.Rows.Count > 0)
			{
				for (int i = 0; i < dtuom.Rows.Count; i++)
				{
					dt.Rows.Add(dtuom.Rows[i]["Id"], dtuom.Rows[i]["UOMCode"], dtuom.Rows[i]["Equivalent"], dtuom.Rows[i]["QtyEquivalent"], dtuom.Rows[i]["BaseRateUom"]);
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetUomScheduleByItemIdForExport(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			DataTable dtuom = UOMSchedule.SearchByObjectExport(new UOMSchedule
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemId = ItemId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("UOMCode");
			dt.Columns.Add("Equivalent", typeof(double));
			dt.Columns.Add("QtyEquivalent", typeof(double));
			dt.Columns.Add("BaseRateUom", typeof(bool));
			if (dtuom.Rows.Count > 0)
			{
				for (int i = 0; i < dtuom.Rows.Count; i++)
				{
					dt.Rows.Add(dtuom.Rows[i]["Id"], dtuom.Rows[i]["UOMCode"], dtuom.Rows[i]["Equivalent"], dtuom.Rows[i]["QtyEquivalent"], dtuom.Rows[i]["BaseRateUom"]);
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetBaseRateUomId(DataTable dt)
	{
		try
		{
			int defaultValue = 0;
			foreach (DataRow row in dt.Rows)
			{
				if (Conversion.ToBool(row["BaseRateUom"]))
				{
					defaultValue = Conversion.ToInt(row["Id"]);
					break;
				}
			}
			return defaultValue;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable CustomeGroupsDefine(int TypeId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return AcLookUps.GetAll(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				TypeId = TypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetOtherPartiesForPrematureReciept()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		try
		{
			return DefineOtherParties.GetAll(new DefineOtherParties
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 159
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GetDecimalConfiguration()
	{
		try
		{
			int PointsAmount = (clsGlobalVariables.DefaultNoofDecimalPointsForAmount = Conversion.ToInt((object)GetConfigurationByOrgCompandConfigDescription("Default NoofDecimal Points For Amount")));
			string DecimalPointsFromatForAmount = "";
			switch (PointsAmount)
			{
			case 1:
				DecimalPointsFromatForAmount = "0";
				break;
			case 2:
				DecimalPointsFromatForAmount = "00";
				break;
			case 3:
				DecimalPointsFromatForAmount = "000";
				break;
			case 4:
				DecimalPointsFromatForAmount = "0000";
				break;
			}
			clsGlobalVariables.stringFormatsingle = "#,##0." + DecimalPointsFromatForAmount;
			clsGlobalVariables.stringFormatboth = "#,##0." + DecimalPointsFromatForAmount + ";(0,0." + DecimalPointsFromatForAmount + "); 0";
			int PointsRate = Conversion.ToInt((object)GetConfigurationByOrgCompandConfigDescription("Default NoofDecimal Points For Rate"));
			clsGlobalVariables.DefaultNoofDecimalPointsForRate = ((PointsRate > 0) ? PointsRate : 2);
			string DecimalPointsFromatForRate = "";
			switch (PointsRate)
			{
			case 1:
				DecimalPointsFromatForRate = "0";
				break;
			case 2:
				DecimalPointsFromatForRate = "00";
				break;
			case 3:
				DecimalPointsFromatForRate = "000";
				break;
			case 4:
				DecimalPointsFromatForRate = "0000";
				break;
			case 0:
				DecimalPointsFromatForRate = "00";
				break;
			}
			clsGlobalVariables.DecimalRateFormate = "#,#0." + DecimalPointsFromatForRate;
			int FcyPointsRate = Conversion.ToInt((object)GetConfigurationByOrgCompandConfigDescription("DefaultNoOfDecimalPointsForFcyRate"));
			clsGlobalVariables.DefaultNoofDecimalPointsForFcyRate = ((FcyPointsRate > 0) ? FcyPointsRate : 2);
			string DecimalPointsFromatForFcyRate = "";
			switch (FcyPointsRate)
			{
			case 1:
				DecimalPointsFromatForFcyRate = "0";
				break;
			case 2:
				DecimalPointsFromatForFcyRate = "00";
				break;
			case 3:
				DecimalPointsFromatForFcyRate = "000";
				break;
			case 4:
				DecimalPointsFromatForFcyRate = "0000";
				break;
			}
			clsGlobalVariables.DecimalFCYRateFormate = "#,#0." + DecimalPointsFromatForFcyRate;
			int PointsfcyAmount = (clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount = Conversion.ToInt((object)GetConfigurationByOrgCompandConfigDescription("DefaultNoOfDecimalPointsForFcyAmount")));
			string DecimalPointsFromatForFcyAmount = "";
			switch (PointsfcyAmount)
			{
			case 1:
				DecimalPointsFromatForFcyAmount = "0";
				break;
			case 2:
				DecimalPointsFromatForFcyAmount = "00";
				break;
			case 3:
				DecimalPointsFromatForFcyAmount = "000";
				break;
			case 4:
				DecimalPointsFromatForFcyAmount = "0000";
				break;
			}
			clsGlobalVariables.stringFormatsingleForFcy = "#,##0." + DecimalPointsFromatForFcyAmount;
			clsGlobalVariables.stringFormatbothForFcy = "#,##0." + DecimalPointsFromatForFcyAmount + ";(0,0." + DecimalPointsFromatForFcyAmount + "); 0";
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetMotherboardSerialNumber()
	{
		try
		{
			string serialNumber = string.Empty;
			using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
			{
				foreach (ManagementObject item in searcher.Get())
				{
					serialNumber = item["SerialNumber"]?.ToString().Trim();
					if (!string.IsNullOrEmpty(serialNumber))
					{
						break;
					}
				}
			}
			return string.IsNullOrEmpty(serialNumber) ? "Unknown" : serialNumber;
		}
		catch (Exception ex)
		{
			return "Error: " + ex.Message;
		}
	}

	public static string GetEncryptedUniqueDeviceId()
	{
		try
		{
			string motherboard = GetMotherboardSerial();
			string cpu = GetCpuId();
			string disk = GetPrimaryDiskSerial();
			return Encryption.EncryptString(motherboard + "-" + cpu + "-" + disk);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetMacAddress()
	{
		string Mac = string.Empty;
		foreach (ManagementObject MO in new ManagementClass("Win32_NetworkAdapter").GetInstances())
		{
			if (MO != null && MO["MacAddress"] != null)
			{
				Mac = MO["MACAddress"].ToString();
				if (Mac != string.Empty)
				{
					break;
				}
			}
		}
		return Mac;
	}

	private static string GetMotherboardSerial()
	{
		try
		{
			foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard").Get())
			{
				string serial = ((mo["SerialNumber"] != null) ? mo["SerialNumber"].ToString().Trim() : "");
				if (!string.IsNullOrEmpty(serial))
				{
					return serial;
				}
			}
			return "UNKNOWN_MB";
		}
		catch
		{
			return "UNKNOWN_MB";
		}
	}

	private static string GetCpuId()
	{
		try
		{
			foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor").Get())
			{
				string id = ((mo["ProcessorId"] != null) ? mo["ProcessorId"].ToString().Trim() : "");
				if (!string.IsNullOrEmpty(id))
				{
					return id;
				}
			}
			return "UNKNOWN_CPU";
		}
		catch
		{
			return "UNKNOWN_CPU";
		}
	}

	private static string GetPrimaryDiskSerial()
	{
		try
		{
			foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_PhysicalMedia").Get())
			{
				string serial = ((mo["SerialNumber"] != null) ? mo["SerialNumber"].ToString().Trim() : "");
				if (!string.IsNullOrEmpty(serial))
				{
					return serial;
				}
			}
			return "UNKNOWN_DISK";
		}
		catch
		{
			return "UNKNOWN_DISK";
		}
	}

	private static string GetLocalIPAddress()
	{
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
			foreach (IPAddress ip in hostAddresses)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			return "UNKNOWN_IP";
		}
		catch
		{
			return "UNKNOWN_IP";
		}
	}

	public static void FreightVoucherSlip241(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataSet ds = InvFreightVoucher.FreightVoucherSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 28,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			DataTable dt = ds?.Tables[0].Copy();
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DatatableHelper.ReplaceCompLogoColumn(dt, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "241-FreightVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void VoucherReport_118(int VoucherHeadId, int DocumentTypeId = 0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (VoucherHeadId == 0)
			{
				throw new Exception("VoucherId Not Found");
			}
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			vh.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			vh.UserId = clsGlobalVariables.UserAccount.ID;
			vh.Id = VoucherHeadId;
			vh.DocumentTypeId = DocumentTypeId;
			vh.ApprovedFilter = "All";
			if (VoucherHeadId > 0)
			{
				DataTable dt = VoucherReports.VoucherValidationReport(vh);
				if (dt.Rows.Count > 0 && dt != null)
				{
					Reporting val = new Reporting();
					val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
					val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "118-AcRptVoucherSlip.rpt");
				}
				else
				{
					MessageBox.Show("No Record Found For Display");
				}
			}
			else
			{
				MessageBox.Show("Record Not Found For Display because VoucherHeadId not found");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AcRptPurchaseSalesVoucherSlip_103(int VoucherHeadId, int DocumentTypeId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (VoucherHeadId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.Id = VoucherHeadId;
			obj.ApprovedFilter = "All";
			if (VoucherHeadId > 0)
			{
				DataTable dt = VoucherReports.VoucherSlipForInventoryReport(obj);
				if (dt.Rows.Count > 0 && dt != null)
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "103-AcRptPurchaseSalesVoucherSlip.rpt");
				}
				else
				{
					MessageBox.Show("No Record Found For Display");
				}
			}
			else
			{
				MessageBox.Show("Record Not Found For Display because VoucherHeadId not found");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PaymentAndReceiptVoucherSlip(int VoucherHeadId, int DocumentTypeId)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (VoucherHeadId == 0)
			{
				throw new Exception("VoucherId Not Found");
			}
			string ReportName = "";
			switch (DocumentTypeId)
			{
			case 1:
				ReportName = "102-CashPaymentVoucher.rpt";
				break;
			case 2:
				ReportName = "102-BankPaymentVoucher.rpt";
				break;
			case 3:
				ReportName = "102-CashReceiptVoucher.rpt";
				break;
			case 4:
				ReportName = "102-BankReceiptVoucher.rpt";
				break;
			}
			DataTable dt = VoucherReports.NewVoucherReprot(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				Id = VoucherHeadId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", clsGlobalVariables.UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dt, ReportName);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void BillsPayablesOrReceiveableVoucher102_01(int VoucherHeadId, int DocumentTypeId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (VoucherHeadId == 0)
			{
				throw new Exception("VoucherId Not Found");
			}
			DataTable dt = VoucherReports.NewVoucherReprot(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				Id = VoucherHeadId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", clsGlobalVariables.UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dt, "102_01_BillsPayablesOrReceiveableVoucher.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ANewAcRptPaymentReceiptsVoucherSlip_102(int VoucherHeadId, string VoucherIds = null, int SupCustId = 0, int SupCustGlId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (VoucherHeadId == 0)
			{
				throw new Exception("VoucherId Not Found");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.UserId = clsGlobalVariables.UserAccount.ID;
			obj.Id = VoucherHeadId;
			obj.Ids = VoucherIds;
			DataTable dt = VoucherReports.NewVoucherReprot(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			obj.SupplierCustomerId = SupCustId;
			obj.AccountId = SupCustGlId;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)))
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDate);
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDate);
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject2 = new SubReportObject(InvPurchaseInvoiceReports.GetDataForLedgerSubReport(obj), "getLedgerforPrint.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", clsGlobalVariables.UserAccount.UserName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "102-ANewAcRptPaymentReceiptsVoucherSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PartyProcessingGDNSlip332_01(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtPrint = PartyProcessingGatePassReports.PartyProcessingGrnGdnStoreSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				DocumentTypeId = 45,
				Id = PrintId
			});
			if (dtPrint == null || dtPrint.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtPrint, "332_01-PartyProcessingGDNSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PartyProcessingGRNSlip332(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtPrint = PartyProcessingGatePassReports.PartyProcessingGrnGdnStoreSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				DocumentTypeId = 44,
				Id = PrintId
			});
			if (dtPrint == null || dtPrint.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtPrint, "332-PartyProcessingGRNSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PartyProcessingInwardGatePassSlip299(int PrintId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = PartyProcessingGatePassReports.PartyProcessingGatePassSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 54,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "299-InvRptPartyProcessingInwardGatePassSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PartyProcessingOutwardGatePassSlip299(int PrintId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = PartyProcessingGatePassReports.PartyProcessingGatePassSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 55,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "299-InvRptPartyProcessingOutwardGatePassSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PartyPaymentAndReceiptSummary_Rpt_102B(int VoucherHeadId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (VoucherHeadId == 0)
			{
				throw new Exception("VoucherId Not Found");
			}
			DataTable dt = VoucherReports.SummaryVoucherReprot(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				Id = VoucherHeadId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", clsGlobalVariables.UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dt, "102B-SpVouchers_PartyPaymentAndReceiptSummary.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ExImRptFCBankReceipts_516(int Id)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = ExportReports.ExImRptFCBankReceipts_516(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = Id
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress);
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName);
			val.ShowReportWithDataTable(dt, "516-ExImRptFCBankReceipts.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AcRptPaymentReceiptsVoucherSlip_102(int VoucherHeadId, int DocumentTypeId = 0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (VoucherHeadId == 0)
			{
				throw new Exception("VoucherId Not Found");
			}
			DataTable dt = VoucherReports.VoucherReport(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				Id = VoucherHeadId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "102-AcRptPaymentReceiptsVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AccountsBudgetSlip_11(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("Record Not Found");
			}
			DataTable dt = AccountsBudgetHeader.AccountsBudgetSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "11-AccountsBudgetSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void FcyExchangeRateForwardingSlip(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("Record Not Found");
			}
			DataTable dt = fcyExRateForwardBooking.ExRateForwardBookingSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "01-FcyExRateForwwardBookingSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DayBookSlip(int VoucherHeadId, int DocumentTypeId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GeneralReprots.DayBookSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = VoucherHeadId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			EnumerableRowCollection<DataRow> rows = from r in dt.AsEnumerable()
				where r.Field<double>("DebitAmount") > 0.0
				select r;
			DataTable dtDebit = (rows.Any() ? rows.CopyToDataTable() : dt.Clone());
			EnumerableRowCollection<DataRow> rowscr = from r in dt.AsEnumerable()
				where r.Field<double>("CreditAmount") > 0.0
				select r;
			DataTable obj = (rowscr.Any() ? rowscr.CopyToDataTable() : dt.Clone());
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject DayBookSubReport_DebitTransactions = new SubReportObject(dtDebit, "DayBookDebitSubReport.rpt");
			SubReportObject DayBookSubReport_CreditTransactions = new SubReportObject(obj, "DayBookCreditSubReport.rpt");
			lstSubReports.Add(DayBookSubReport_DebitTransactions);
			lstSubReports.Add(DayBookSubReport_CreditTransactions);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "144-DayBookSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SaleInvoiceDirectSlip_294(string ReportName, int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, ReportName, lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceStorePMSlip(int RecId, int DocumentTypeId)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			if (DocumentTypeId == 126)
			{
				SaleInvoiceDirectItemSlip_294(RecId);
				return;
			}
			Reporting rpt = new Reporting();
			if (clsGlobalVariables.UserAccount.CompName.ToString() == string.Empty)
			{
				rpt.RptPerameter("CompanyName", (string)null);
			}
			else
			{
				rpt.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			}
			rpt.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@OrganizationId", clsGlobalVariables.UserAccount.OrganizationId.ToString());
			rpt.RptPerameter("@CompanyId", clsGlobalVariables.UserAccount.CompanyId.ToString());
			rpt.RptPerameter("@Id", RecId.ToString());
			rpt.RptPerameter("@PihId", RecId.ToString());
			rpt.ShowReport("239-SaleInvoice_StoreBillWithTax.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceDirectItemSlip_294(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "294-InvRptSaleBillDirectWithoutSO.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SupplierResgister_292(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GeneralReprots.SupplierCustomerRegister(new ReportsParameters
			{
				GroupAccountId = 7
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "292-InvRptSupplierRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ConsigneeRegister_292_01()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GeneralReprots.SupplierCustomerRegister(new ReportsParameters
			{
				GroupAccountId = 7,
				ActionId = 1
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "292_01-InvRptSupplierRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void InvRptSupplierSlip_293(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GeneralReprots.SupplierCustomerRegister(new ReportsParameters
			{
				SupplierCustomerId = RecId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "293-InvRptSupplierSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceDirectPartySlip_294(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "294A-InvRptSaleBillDirectWithoutSO.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceDirectItemSlip_294C(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SaleInvoiceItemExpenseSubReport(obj.Id), "SaleInvoiceItemExpense_SubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "294C_SaleBillDirectItemSlip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceDirectSlip_294B(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = RecId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "294B-InvRptSaleBillDirectWithoutSO.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceAutoRatedPartySlip_294(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "291A-SaleInvoice_AutoRated.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoice_DirectFlour_282A(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "282A-SaleInvoice_DirectFlour.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoice_DirectFlour282(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "282-SaleInvoice_DirectFlour.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceReturn_StorePartySlip145(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.Id = PrintId;
				DataTable dt = InvSaleInvoiceReports.PurchaseInvoiceReturn_StoreSip(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "145A-PurchaseInvoiceReturn_StorePartySlip.rpt", lstSubReports);
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceReturn_StoreItemSip145A(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.Id = RecId;
				DataTable dt = InvSaleInvoiceReports.PurchaseInvoiceReturn_StoreSip(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "145-PurchaseInvoiceReturn_StoreItemSlip.rpt", lstSubReports);
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoicetSlip_301(int RecId, int SupCustId = 0, int SupCustGlId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				obj.DocumentTypeId = 95;
				DataTable dt = InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				obj.SupplierCustomerId = SupCustId;
				obj.AccountId = SupCustGlId;
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)))
				{
					obj.FromDate = Conversion.ToDateTime((object)FromDate);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
				{
					obj.ToDate = Conversion.ToDateTime((object)ToDate);
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
				SubReportObject subReportObject2 = new SubReportObject(InvSaleInvoiceReports.SaleInvoiceSubReportJournalExpense(obj.Id), "InvSaleInvoiceCustomerBillRiceJournalExpSubRep.rpt");
				SubReportObject subReportObject3 = new SubReportObject(InvPurchaseInvoiceReports.GetDataForLedgerSubReport(obj), "getLedgerforPrint.rpt");
				lstSubReports.Add(subReportObject);
				lstSubReports.Add(subReportObject2);
				lstSubReports.Add(subReportObject3);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "301-InvRepSaleBillCustomer.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void SaleInvoiceItemSlip_303A(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				obj.DocumentTypeId = 95;
				DataTable dt = InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SaleInvoiceItemExpenseSubReport(obj.Id), "SaleInvoiceItemExpense_SubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "303A_SaleInvoiceItemSlip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void SaleInvoicetSlip_303(int RecId, int DocumentTypeId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				obj.DocumentTypeId = DocumentTypeId;
				DataTable dt = InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "303-InvRepSaleBillCustomer-Format-II.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void SaleInvoicetSlip_318(int RecId, int DocumentTypeId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				obj.DocumentTypeId = DocumentTypeId;
				DataTable dt = InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "318-InvRepSaleBillCustomer.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void SaleInvoicetSlip_318A(int RecId, int DocumentTypeId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				obj.DocumentTypeId = DocumentTypeId;
				DataTable dt = InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "318A-InvRepSaleBillCustomer.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void PurchaseInvoicetDirectSlip_225(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoiceDirectSlip_225(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprtForItem(obj.Id), "SupplierBillReport");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "225-InvRepPurchaseBillDirectWithoutPo.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void PurchaseInvoicetDirectSlip_225A(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoiceDirectSlip_225(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "225A-InvRepPurchaseBillDirectWithoutPo.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void SaleInvoiceReturnItemSlip98(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoiceDirectSlip_225(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprtForItem(obj.Id), "SupplierBillReport");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "98_SaleInvoiceReturnItemSlip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void SaleInvoiceReturnPartySlip98A(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoiceDirectSlip_225(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "98A_SaleInvoiceReturnPartySlip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void PurchaseInvoicettSlip_220R(int RecId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			if (obj.Id > 0)
			{
				dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceReturnSlipReport220R(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "220R-InvRptPurchaseBillReturnSupplierRiceSlip.rpt", lstSubReports);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoicettSlip_220A(int RecId, int SupCustId = 0, int SupCustGlId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			obj.SupplierCustomerId = SupCustId;
			obj.AccountId = SupCustGlId;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)))
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDate);
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDate);
			}
			SubReportObject subReportObject2 = new SubReportObject(InvPurchaseInvoiceReports.GetDataForLedgerSubReport(obj), "getLedgerforPrint.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "220A-InvRptPurchaseBillSupplierRiceSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoicePMSlip_231_01(int RecId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found");
				return;
			}
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = RecId
			};
			DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoice_PM231(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			DataTable dtSupplierOthers = InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id);
			lstSubReports.Add(new SubReportObject(dtSupplierOthers, "InvRptPurchaseBillSupplierOthers.rpt"));
			string orderIds = string.Join(",", (from row in dt.AsEnumerable()
				select row["Id"].ToString() into id
				where !string.IsNullOrEmpty(id)
				select id).Distinct());
			DataTable dtGrnDetail = InvGrnandGdnReports.PurchaseOrder_WithGrnDetail_Report(obj.OrganizationId, obj.CompanyId, orderIds);
			lstSubReports.Add(new SubReportObject(dtGrnDetail, "PurchaseOrder_WithGrnDetail_SubReport.rpt"));
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress?.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName?.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "231_01_PurchaseBillPmSlipWithGrnDetail.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoicePMSlip_231(int RecId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoice_PM231(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "231-InvRptPurchaseBillPackingMaterialSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceStoreBill_230(int RecId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoiceStoreBill_230(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "230-InvRptPurchaseBillStoreSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceStoreBillWithtax_238(int RecId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoiceStoreBillWithTax_238(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "238-InvPurchaseInvoice_StoreBillWithTax.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceStoreDirectSlip(int RecId, int DocumentTypeId)
	{
		try
		{
			if (DocumentTypeId == 61)
			{
				PurchaseInvoiceStoreDirectSlip_233(RecId);
			}
			else
			{
				PurchaseInvoiceStoreDirectSlip_237(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceStoreDirectSlip_233(int RecId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@OrganizationId", clsGlobalVariables.UserAccount.OrganizationId.ToString());
			val.RptPerameter("@CompanyId", clsGlobalVariables.UserAccount.CompanyId.ToString());
			val.RptPerameter("@Id", RecId.ToString());
			val.RptPerameter("@PihId", RecId.ToString());
			val.ShowReport("233-SpInvPurchaseInvoice_StoreBillDirect.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceStoreDirectSlip_237(int RecId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@OrganizationId", clsGlobalVariables.UserAccount.OrganizationId.ToString());
			val.RptPerameter("@CompanyId", clsGlobalVariables.UserAccount.CompanyId.ToString());
			val.RptPerameter("@Id", RecId.ToString());
			val.RptPerameter("@PihId", RecId.ToString());
			val.ShowReport("237-InvPurchaseInvoice_StoreBillDirectWithTax.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ContractorWagesBillManualSlip004(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				DataTable dt = InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					DocumentTypeId = 810,
					Id = RecId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					throw new Exception("No Record Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "004-ContractorWagesBillManualSlip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ContractorWagesBill_SlipandRegister_002(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				DataTable dt = InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					DocumentTypeId = 101,
					Id = RecId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					throw new Exception("No Record Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "002-ContractorWagesSlip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ContractorWagesPartyProcessingSlip(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				DataTable dt = InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					DocumentTypeId = 219,
					Id = RecId,
					ApprovedFilter = "All"
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					throw new Exception("No Record Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "002_01_ContractorWagesPartyProcessingSlip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StoreIssuanceSlip1616(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("Record Id Not Found");
				return;
			}
			DataTable dt = InvGsStoreIssuanceHeader.StoreIssuanceHistory(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1616-StoreIssuanceToConsumableStore_Slip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StoreIssuanceReturnSlip457(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvStoreReturn.StoreReturnHistory(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			string ids = string.Join(",", from row in dt.AsEnumerable()
				select row["IssuanceId"].ToString());
			obj.Id = 0;
			obj.Ids = ids;
			DataTable dataTable = InvGsStoreIssuanceHeader.StoreIssuanceHistory(obj);
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(dataTable, "StoreIssuanceSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "457-StoreIssuanceReturnSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockTransferSlip407(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = InvStockTransferHeader.StockTransferSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 806,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "407-InvStockTransferSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockTransferPartyProcessingSlip(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = InvStockTransferPartyProcessing.StockTransferPartyProcessingSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 220,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "220_StockTransferPartyProcessingSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockOpeningBalanceSlipandRegister416(int PrintId, int DocumentTypeId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = GeneralReprots.StockopeningBalanceRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeIds = Conversion.ToString((object)DocumentTypeId),
				Id = PrintId
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "416-InvStockOpeningBalanceHeader_Register.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockOpeningBalanceSlipAndRegisterEngr(int PrintId, int DocumentTypeId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = GeneralReprots.StockopeningBalanceRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeIds = Conversion.ToString((object)DocumentTypeId),
				Id = PrintId
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "416_01_StockOpeningBalanceHeader_Register.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockOpeningBalancePartyProcSlipandRegister416_01(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = InvStockOpeningBalancePartyProcessing.StockOpeningBalancePartyProcessing_SlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 121,
				Id = PrintId
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "416_01-InvStockOpeningBalancePartyProcessing_Slip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockTransferPackingMaterialAndStore_Slip415(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = InvStockTransferHeader.StockTransferPackingMaterialAndStore_SlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "415-InvStockTransferPackingMaterialAndStore_SlipandRegister.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockTransferSlip406(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = InvStockTransferHeader.StockTransferSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "406-InvStockTransferSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StoreSendReceipt_Slip465(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = InvStoreSendReceipt.StoreSendReceipt_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "465-StoreSendReceipt_Slip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StoreIssuanceHeader_Slip475(int PrintId = 0, int InvoiceId = 0)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0 && InvoiceId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = InvGsStoreIssuanceHeader.StoreIssuanceHistory(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				InvoiceId = InvoiceId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "475-RptInvGsStoreIssuanceHeader_Slip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StoreIssuanceSlip452(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = InvGsStoreIssuanceHeader.StoreIssuanceHistory(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "452-RptInvGsStoreIssuanceHeader_Slip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseDemandSlip454(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = InvPurchaseDemandHeader.InvPurchaseDemondSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName);
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress);
				val.ShowReportWithDataTable(dt, "454-InvPurchaseDemanSlip.rpt");
				return;
			}
			throw new Exception("Record Not Found For Dispaly");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void P2PPMSlip419(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvPmStockWithPartiesTransferHeader.P2PPM419Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName);
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress);
				val.ShowReportWithDataTable(dt, "419-PartyToPartyPackingMaterialSlip.rpt");
				return;
			}
			throw new Exception("Record Not Found For Dispaly");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StorePurchaseInvoiceSlip(int PrintId, string ReportName)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@OrganizationId", clsGlobalVariables.UserAccount.OrganizationId.ToString());
			val.RptPerameter("@CompanyId", clsGlobalVariables.UserAccount.CompanyId.ToString());
			val.RptPerameter("@Id", PrintId.ToString());
			val.RptPerameter("@PihId", PrintId.ToString());
			val.ShowReport(ReportName);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StorePurchaseInvoiceSlip230(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = InvPurchaseInvoiceReports.InvPurchaseInvoice_StoreBillSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(PrintId), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "230-InvRptPurchaseBillStoreSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseOrderSlipReport201(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = PurchaseOrderReports.PurchaseOrderSlipReport201(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "201-InvRptPurchaseOrderGeneralSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseOrderSlipReport203(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = PurchaseOrderReports.PurchaseOrderSlipReport203(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 41,
				OrderId = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(PurchaseOrderReports.PurchaseOrderSlipReport203SubReport(PrintId), "PurchaseOrderSubReport.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(PurchaseOrderReports.PurchaseOrderSupplierExpense_SubReport(PrintId), "PurchaseOrderSupplierExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "203-InvRptPurchaseOrderRiceSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SupplierDispatchSubReport_203A(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = PurchaseOrderReports.PurchaseOrderSlipReport203(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 41,
				OrderId = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(PurchaseOrderReports.SupplierDispatchSubReport_203A(PrintId), "PurchaseOrder_SupplierDispatchSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "203A_PurchaseOrderSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseOrderSlipReport203_01(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = PurchaseOrderReports.PurchaseOrderSlipReport203(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 41,
				OrderId = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(PurchaseOrderReports.PurchaseOrderSlipReport203SubReport(PrintId), "PurchaseOrderSubReport.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(PurchaseOrderReports.PurchaseOrderSupplierExpense_SubReport(PrintId), "PurchaseOrderSupplierExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "203_01_PurchaseOrderRiceSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseOrderSlipReport203_02(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = PurchaseOrderReports.PurchaseOrderSlipReport203(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 41,
				OrderId = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(PurchaseOrderReports.PurchaseOrderSlipReport203SubReport(PrintId), "PurchaseOrderSubReport.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject3 = new SubReportObject(PurchaseOrderReports.PurchaseOrderSupplierExpense_SubReport(PrintId), "PurchaseOrderSupplierExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject3);
			SubReportObject subReportObject2 = new SubReportObject(PurchaseOrderReports.PurchaseOrderLabSample_SubReport(PrintId), "PurchaseOrderLabSampleSubReport.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "203_02_PurchaseOrderRiceSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseOrderPackingMaterialSlip215(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = PurchaseOrderReports.PurchaseOrderSlipReport201(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 700,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "215-PurchaseOrderPackingMaterialSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockAdjustmentSlipAndRegister409(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvStockAdjustment.StockAdjustmentSlipAndRegister409(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@PrintedBy", clsGlobalVariables.UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dt, "409-InvStockAdjustmentSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockAdjustmentPartyProcessingSlip(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvStockAdjustmentPartyProcessing.StockAdjustmentSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "158-InvStockAdjustmentPartyProcessingSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ProductionInPutAndOutPut601(int PrintId, string ReqType = "")
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvFoodProductionReports.InvFoodProductionSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				ReqType = ReqType
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "601-InvFoodProductionSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ProductionConsumptionReport_623(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvFoodProductionReports.FoodProduction_ConsumptionReport(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "623-InvFoodProduction_ConsumptionReport.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ProductionPMAndOverHeads_606(int PrintId, string ReportType)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvFoodProductionReports.InvFoodProductionPackingAndOverHeadReportByJobOrder(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				ReportType = ReportType
			});
			if (dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockConversionSummary_605(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvFoodProductionReports.StockConversionSummeryReport(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "605-StockConversionSummaryNewRpt.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseOrderSlipReport1500(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = PurchaseOrder.PurchaseOrderSlipReport1500(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 1500,
				OrderId = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1500-PurchaseOrderSlip_Rpt.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void WbTransactionSlip280(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "280-InvRptWeighBridgeSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void WbTransactionSlipByGpId(int GpId, int GpDocumentTypeId)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (GpId == 0 || GpDocumentTypeId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				GpId = GpId,
				GpDocumentTypeId = GpDocumentTypeId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "280-InvRptWeighBridgeSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void LabAnalysisReport653(int PrintId = 0, int GpId = 0)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0 && GpId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			obj.GpId = GpId;
			DataTable dt = InvLabAnalysisPurchaseHeader.RptInvLabPurchaseAnalysisSlip653(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvLabAnalysisPurchaseHeader.GetDataForLabPurchaseAnalysisSubReport(obj.Id), "LabPurchaseAnalysisSubParameterReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "653-RptInvLabPurchaseAnalysisSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleOrderReports273(int PrintId, int DocumentTypeId = 0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.ApprovedFilter = "All";
			DataTable dt = SaleOrderReports.SaleOrderReports273(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(SaleOrderReports.SaleOrderCustomerExpense_SubReport(obj.Id), "SaleOrderCustomerExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "273-InvRptSaleOrderSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SaleOrderSlip_90(int PrintId, int DocumentTypeId = 0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = SaleOrderReports.SaleOrderReports273(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId,
				ApprovedFilter = "All"
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "90-SaleOrderStoreAndPmSlip.rpt", (List<SubReportObject>)null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SaleOrderReports273_01(int PrintId, int DocumentTypeId = 0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.ApprovedFilter = "All";
			DataTable dt = SaleOrderReports.SaleOrderReports273(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(SaleOrderReports.SaleOrderCustomerExpense_SubReport(obj.Id), "SaleOrderCustomerExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "273_01_SaleOrderSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void BookOrderReports273A(int PrintId, int DocumentTypeId = 0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = SaleOrderReports.SaleOrderReports273(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId,
				ApprovedFilter = "All"
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "273-InvRptSaleOrderSlip(A).rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SupplierDispatchPreBillSlip251(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = SupplierDispatch.SupplierDispatch_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "251_SupplierDispatchPreBillSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SupplierDispatchSlip246(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = SupplierDispatch.SupplierDispatch_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "246_SupplierDispatchSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PdcPaymentTransactionsSlip250(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = pdcTransaction.pdcTransaction_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "250_PdcPaymentTransactionsSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GatePassOutwardSlipandRegister(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					Id = PrintId
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("No Record Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "295-InvRptOutwardGatePassSlipWithItems.rpt");
				return;
			}
			throw new Exception("PrintId not found...");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvDeliveryOrderSlipWithSubReport(int PrintId, int DocumentTypeId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = InvGrnandGdnReports.InvDeliveryOrderSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "262-DeliveryOrderSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeliveryOrderSlip264(int PrintId, int DocumentTypeId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = InvGrnandGdnReports.InvDeliveryOrderSlip(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					UserId = clsGlobalVariables.UserAccount.ID,
					Id = PrintId,
					DocumentTypeId = DocumentTypeId
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "264-DeliveryChallanByDeliveryOrder.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvDeliveryOrderSlip(int PrintId, int DocumentTypeId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = InvGrnandGdnReports.InvDeliveryOrderSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "262-DeliveryOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AdvanceDeliveryOrderSlip910(int PrintId, int DocumentTypeId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = InvGrnandGdnReports.InvDeliveryOrderSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "910_AdvanceDeliveryOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AdvanceDeliveryOrderSlip910_01(int PrintId, int DocumentTypeId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = InvGrnandGdnReports.InvDeliveryOrderSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "910_01_AdvanceDeliveryOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AdvanceDeliveryOrderChallanSlip910A(int PrintId, int DocumentTypeId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = InvGrnandGdnReports.InvDeliveryOrderSlip(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					UserId = clsGlobalVariables.UserAccount.ID,
					Id = PrintId,
					DocumentTypeId = DocumentTypeId
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "910A_AdvanceDeliveryChallanSlip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ExportDeliveryOrderSlip396(int PrintId, int DocumentTypeId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = InvGrnandGdnReports.InvDeliveryOrderSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "396-ExportDeliveryOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ProductionRecoveryReport602(int jobOrderId, int plantId = 0, string branchesIds = null, bool useDateFilter = false, DateTime? fromDate = null, DateTime? toDate = null)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			UserAccount user = clsGlobalVariables.UserAccount;
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = user.OrganizationId,
				CompanyId = user.CompanyId,
				JobOrderId = jobOrderId,
				PlantId = plantId,
				BranchesIds = (string.IsNullOrWhiteSpace(branchesIds) ? null : branchesIds)
			};
			if (useDateFilter && fromDate.HasValue && toDate.HasValue)
			{
				obj.FromDate = fromDate.Value;
				obj.ToDate = toDate.Value;
			}
			DataTable dt = InvFoodProductionReports.InvFoodProduction_Summery_WithOutValue_Rpt(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			List<SubReportObject> subReports = new List<SubReportObject>();
			DataTable dtSub = InvFoodProductionReports.DailyPlantConsumedHoursByJobOrderId_SubReport(jobOrderId, plantId);
			subReports.Add(new SubReportObject(dtSub, "DailyPlantConsumedHoursByJobOrderId_SubReport"));
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", user.CompAddress);
			val.RptPerameter("@CompanyName", user.CompName);
			val.ShowReportWithDataTableSubReprt(dt, "602-InvRptProductionSummery.rpt", subReports);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void ProductionRecoveryReportWithLab_602_01(int jobOrderId, int plantId = 0, string branchesIds = null, bool useDateFilter = false, DateTime? fromDate = null, DateTime? toDate = null)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			UserAccount user = clsGlobalVariables.UserAccount;
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = user.OrganizationId,
				CompanyId = user.CompanyId,
				JobOrderId = jobOrderId,
				PlantId = plantId,
				BranchesIds = (string.IsNullOrWhiteSpace(branchesIds) ? null : branchesIds)
			};
			if (useDateFilter && fromDate.HasValue && toDate.HasValue)
			{
				obj.FromDate = fromDate.Value;
				obj.ToDate = toDate.Value;
			}
			DataTable dt = InvFoodProductionReports.InvFoodProduction_Summery_WithOutValue_Rpt(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			List<SubReportObject> subReports = new List<SubReportObject>();
			DataTable dtSub = InvFoodProductionReports.DailyPlantConsumedHoursByJobOrderId_SubReport(jobOrderId, plantId);
			subReports.Add(new SubReportObject(dtSub, "DailyPlantConsumedHoursByJobOrderId_SubReport"));
			DataTable dtSub2 = InvFoodProductionReports.InvProductionJobOrderLabStandardDetail_SubReport(jobOrderId);
			subReports.Add(new SubReportObject(dtSub2, "usp_InvProductionJobOrderLabStandardDetail_SubReport.rpt"));
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", user.CompAddress);
			val.RptPerameter("@CompanyName", user.CompName);
			val.ShowReportWithDataTableSubReprt(dt, "602_01_ProductionSummeryWithLab.rpt", subReports);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void ExportInvoiceVoucher_Slip(int VoucherHeadId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (VoucherHeadId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = VoucherReports.ExportInvoiceVoucher(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = VoucherHeadId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", clsGlobalVariables.UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dt, "102-ANewAcRptExportInvoiceVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void InvLabPreProductionExportLotInspection_Slip(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = InvLabPreProductionExportLotInspectionHeader.InvLabPreProductionExportLotInspectionHeader_SlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "513-invLabPreProductionSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ThirdPartyInspectionSlip514(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = InvLabPreProductionExportLotInspectionHeader.InvLabPreThirdPartyInspection_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "514_ThirdPartyInspectionSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExImBillOfLading_Slip(int InvoiceId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (InvoiceId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = ExImBillOfLading.ExImBillOfLading_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				EximInvoiceId = InvoiceId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "505-ExImBillOfLading_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportInvoiceSlip_520(int InvoiceId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (InvoiceId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = ExportPdfReport.EximInvoiceSlip_520(new ReportsParameters
			{
				OrganizationId = Conversion.ToInt((object)clsGlobalVariables.UserAccount.OrganizationId),
				CompanyId = Conversion.ToInt((object)clsGlobalVariables.UserAccount.CompanyId),
				EximInvoiceId = InvoiceId,
				ApprovedFilter = Conversion.ToString((object)"All")
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject1 = new SubReportObject(ExportPdfReport.ExportInvoiceSubReportContainerForPackingliSt529(InvoiceId), "529-SubRpt-ContainerListByInvoice.rpt");
			lstSubReports.Add(subReportObject1);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "530-EximBillOfLadingSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SalesContractExportSlip523(int LcOrderId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (LcOrderId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = ExportPdfReport.ExpRptSalesContractExportRegister_523(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ExImLcOrderId = LcOrderId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "523-ExpRptSalesContractWiseInvoiceRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void LcOrderShipmentScheduleLcOrderWise_Slip(int LcOrderId, int ContractScheduleId = 0)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (LcOrderId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters parameters = new ReportsParameters();
			parameters.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			parameters.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			parameters.ExImLcOrderId = LcOrderId;
			DataTable dtSlip = ExportPdfReport.ExpRptSalesContractExportRegister_523(parameters);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			parameters.ContractScheduleId = ContractScheduleId;
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject2 = new SubReportObject(ExportPdfReport.LcOrderShipmentScheduleLcOrderWise_Slip(parameters), "LcOrderShipmentScheduleLcOrderWise_SubReport1.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject = new SubReportObject(ExportPdfReport.LcOrderShipmentScheduleLcOrderWiseDetail_SubReport(LcOrderId, ContractScheduleId), "LcOrderShipmentScheduleLcOrderWiseDetail_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, "395-LcOrderShipmentScheduleLcOrderWise_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleContractReportsSlipNew501(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			obj.ApprovedFilter = "All";
			dt = SaleContractReports.SaleContractReports501(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject1 = new SubReportObject(SaleContractReports.LcorderOtherItem_SubRpt501(obj.Id), "LcorderOtherItem_SubRpt.rpt");
			lstSubReports.Add(subReportObject1);
			SubReportObject subReportObject2 = new SubReportObject(SaleContractReports.ExImLcOrderPaymentTermsDetail_SubRpt501(obj.Id), "ExImLcOrderPaymentTermsDetail_SubRpt.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject3 = new SubReportObject(SaleContractReports.ExImLcContractPackingMaterialDetail_SubRpt501(obj.Id), "ExImLcContractPackingMaterialDetail_SubRpt.rpt");
			lstSubReports.Add(subReportObject3);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "501-ExportSalesContractExportNew.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleContractReportsSlipNew501A(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			obj.ApprovedFilter = "All";
			dt = SaleContractReports.SaleContractReports501(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject1 = new SubReportObject(SaleContractReports.LcorderOtherItem_SubRpt501(obj.Id), "LcorderOtherItem_SubRpt.rpt");
			lstSubReports.Add(subReportObject1);
			SubReportObject subReportObject2 = new SubReportObject(SaleContractReports.ExImLcOrderPaymentTermsDetail_SubRpt501(obj.Id), "ExImLcOrderPaymentTermsDetail_SubRpt.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject3 = new SubReportObject(SaleContractReports.ExImLcContractPackingMaterialDetail_SubRpt501(obj.Id), "ExImLcContractPackingMaterialDetail_SubRpt.rpt");
			lstSubReports.Add(subReportObject3);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "501A_ExportSalesContractExportNew.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleContractReportsSlipNew501B(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			obj.ApprovedFilter = "All";
			dt = SaleContractReports.SaleContractReports501(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject1 = new SubReportObject(SaleContractReports.LcorderOtherItem_SubRpt501(obj.Id), "LcorderOtherItem_SubRpt.rpt");
			lstSubReports.Add(subReportObject1);
			SubReportObject subReportObject2 = new SubReportObject(SaleContractReports.ExImLcOrderPaymentTermsDetail_SubRpt501(obj.Id), "ExImLcOrderPaymentTermsDetail_SubRpt.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject3 = new SubReportObject(SaleContractReports.ExImLcContractPackingMaterialDetail_SubRpt501(obj.Id), "ExImLcContractPackingMaterialDetail_SubRpt.rpt");
			lstSubReports.Add(subReportObject3);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "501B_ExportSalesContractExportNew.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportSalesContractExport501(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = SaleContractReports.SaleContractReports501(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "501-ExpRptSalesContractExport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportSalesContractByInvoiceIdExport501B(int ContractId = 0, int InvoiceId = 0)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (ContractId == 0 && InvoiceId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = SaleContractReports.SaleContractByInvoiceIdReports501B(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = ContractId,
				InvoiceId = InvoiceId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "501B-ExpRptSalesContractExport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportSalesContractExport501WithSubReport(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			obj.ApprovedFilter = "All";
			dt = SaleContractReports.SaleContractReports501(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject1 = new SubReportObject(SaleContractReports.LcorderOtherItem_SubRpt501(obj.Id), "LcorderOtherItem_SubRpt.rpt");
			lstSubReports.Add(subReportObject1);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "501-ExpRptSalesContractExport.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void CommercialInvoiceSlip521(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.EximInvoiceId = PrintId;
			obj.ApprovedFilter = "All";
			DataTable dtSlip = ExportPdfReport.InvoiceSlip521(obj);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ExportPdfReport.InvoiceSlip521SupReprt(obj.EximInvoiceId), "ExportInvoiceOtherExpense_SubRpt.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(ExportPdfReport.CustomeInvoiceSlip521SupReprt(obj.EximInvoiceId), "ExportInvoiceCustomExpense_SubRpt.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, "521-ExportInvoiceSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ServicesBillSlip540(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = ServicesReports.ServicesBill_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "540-RptServiceBillSlip.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void CommercialInvoiceSlipWithName(int PrintId, string Name)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.EximInvoiceId = PrintId;
			obj.ApprovedFilter = "All";
			DataTable dtSlip = ExportPdfReport.InvoiceSlip521(obj);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ExportPdfReport.InvoiceSlip521SupReprt(obj.EximInvoiceId), "ExportInvoiceOtherExpense_SubRpt.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(ExportPdfReport.CustomeInvoiceSlip521SupReprt(obj.EximInvoiceId), "ExportInvoiceCustomExpense_SubRpt.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, Name + ".rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportPackingList529(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.EximInvoiceId = PrintId;
			obj.ApprovedFilter = "All";
			DataTable dtSlip = ExportPdfReport.InvoiceSlip521(obj);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ExportPdfReport.ExportInvoiceSubReportContainerForPackingliSt529(obj.EximInvoiceId), "529-SubRpt-ContainerListByInvoice.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, "529-ExportInvoiceSlipPackingList.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportPackingList529A(int PrintId)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dtSlip = ExportPdfReport.InvoicePackingList529A(clsGlobalVariables.UserAccount.CompanyId, PrintId);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtSlip, "529A_ExportInvoiceSlipPackingList.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportPackingList529B(int PrintId)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dtSlip = ExportPdfReport.InvoicePackingList529A(clsGlobalVariables.UserAccount.CompanyId, PrintId);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtSlip, "529B_ExportInvoiceSlipPackingList.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportPackingList529C(int PrintId)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dtSlip = ExportPdfReport.InvoicePackingList529A(clsGlobalVariables.UserAccount.CompanyId, PrintId);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtSlip, "529C_ExportInvoiceSlipPackingList.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ImportInvoiceSlip902(long PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0L)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 902;
			obj.RecId = PrintId;
			obj.ApprovedFilter = "All";
			DataTable dtSlip = invoiceMaster.InvoiceMaster_Report(obj);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(invoiceMaster.ImportInvoiceSlipSupReprt(obj.RecId), "ImportInvoice_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, "902_ImportInvoiceSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ProformaInvoiceSlip900(long PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0L)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dtSlip = ProformaMaster.ProformaMaster_Report(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 900,
				RecId = PrintId
			});
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtSlip, "900_ProformaInvoiceSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GDBreakUpBankRequest_Slip560(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = GDBreakUpBankRequestHeader.GDBreakUpBankRequest_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "560-GDBreakUpBankRequest_Slip.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PreBookingSlip(int Id)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Id == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = PreBookingOrder.PreBookingOrder_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = Id
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "274_PreBookingOrder_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExBookingInfoSlip560(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = ExImExportShipingLineBooking.PrintSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "560-ExBooking Info(CRO).rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void BookingInfoSlip560_AgainstInvoice(int InvoiceId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (InvoiceId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = ExImExportShipingLineBooking.PrintSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ExImInvoiceId = InvoiceId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "560-ExBooking Info(CRO).rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ImBookingInfoSlip(long PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0L)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = ShipmentBooking.ShipmentBookingReport(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				RecId = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "903-ImExShipmentBookingInfo.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ForwardingSlip507(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = ExportPdfReport.ExImForwarding_Slip_507(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "507-ExImForwarding_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ForwardingDirectSlip507_01(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = ExportPdfReport.ExImForwarding_Slip_507(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "507_01_ExImForwardingDirect_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void CommissionBillSuppCustndIncomeTransSlip1706(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvCommAgentTradeBill.CommissionBillSupplierCustomerAndIncomeTransactionsSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1706-CommissionBillSupplierCustomerAndIncomeTransactions.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void CommisionTradeBillAndSlip168(int PrintId, string ReportName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = 162;
			obj.Id = PrintId;
			DataTable dt = InvCommAgentTradeBill.TradeBillSlipAndRegister(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvCommAgentTradeBill.TradeBillSlipAndRegisterSupReprt(obj.Id), "InvCommissionAgentBillOtherExp.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, ReportName, lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void LabAnalysisReport659(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvLabAnalysisPurchaseHeader.InvLabAnalysisInProcessHeader_Slip659(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvLabAnalysisPurchaseHeader.GetDataForLabInProcessGroupAnalysisSubReport(obj.Id), "InvLabInProcessGroupAnalysisSub.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(InvLabAnalysisPurchaseHeader.GetDataForLabAnalysisStep_SubReport(obj.Id), "InvLabInProcessStepAnalysisSub.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "659-RptInvLabInProcessAnalysisSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void ExportCommercialInvoicePackingdetailListSlip(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dtSlip = ExportPdfReport.CommercialInvoicePackingDetailListSlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtSlip, "550-CommercialInvoicePackingDetailList_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static int VoucherHeadIdGet(int Id, int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId(new VoucherHead
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				DocumentTypeSrNo = Id
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateVoucherCode(int DocumentTypeId)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			int VoucherCode = 0;
			DataTable dt = VoucherHead.GenerateVoucherCodeByDocumentTypeId(new VoucherHead
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchId = clsGlobalVariables.UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt != null)
			{
				VoucherCode = Conversion.ToInt(dt.Rows[0]["VoucherCode"]);
			}
			return VoucherCode;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string[] DynamicReportsLoad(string FolderName)
	{
		try
		{
			string ConnStringReportPath = ConfigurationManager.ConnectionStrings["TPSSReportDb"].ConnectionString.ToString();
			string reportPath;
			if (ConnStringReportPath != string.Empty)
			{
				reportPath = ConnStringReportPath + "\\";
				if (Directory.Exists(reportPath + FolderName))
				{
					return Directory.GetFiles(reportPath + FolderName, "*rpt");
				}
				Directory.CreateDirectory(reportPath + FolderName);
				return null;
			}
			reportPath = Application.ExecutablePath + "\\..\\Reports\\";
			if (Directory.Exists(reportPath + FolderName))
			{
				return Directory.GetFiles(reportPath + FolderName, "*rpt");
			}
			Directory.CreateDirectory(reportPath + FolderName);
			return null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable MakeDataTable(string PropertyName, string PropertyValue, DataTable dt = null)
	{
		DataTable dtFinal = new DataTable();
		try
		{
			dtFinal.Columns.Add("PropertyName");
			dtFinal.Columns.Add("PropertyValue");
			dtFinal.Rows.Add(PropertyName, PropertyValue);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dtFinal;
	}

	public static int DeliveryOrderGenerateCode(int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			return InvDeliveryOrder.GenerateCode(new InvDeliveryOrder
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetTaxTypeIdAndPercentByItemId(int ItemId, DateTime EffectedDate)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return ItemTaxSchedule.GetTaxScheduleForItemId(new ItemTaxSchedule
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemId = ItemId,
				EffectedDate = EffectedDate
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ExportPreCommercialInvoiceSlip548(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dtSlip = ExportPdfReport.ExportPreInvoiceSlip(obj);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ExportPdfReport.InvoiceSlip521SupReprt(obj.Id), "PreCommercialInvoiceExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, "548-ExportCommercialInvoiceSlipForBank.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportPreCommercialInvoiceSlip549(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dtSlip = ExportPdfReport.ExportPreInvoiceSlip(obj);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ExportPdfReport.InvoiceSlip521SupReprt(obj.Id), "PreCommercialInvoiceExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, "549-ExportCommercialInvoiceSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportPreInvoiceSlipForBank546(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dtSlip = ExportPdfReport.ExportPreInvoiceSlip(obj);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ExportPdfReport.InvoiceSlip521SupReprt(obj.Id), "PreCommercialInvoiceExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, "546-ExportPreInvoiceSlipForBank.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportPreCommercialInvoiceSlip546(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dtSlip = ExportPdfReport.ExportPreInvoiceSlip(obj);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ExportPdfReport.InvoiceSlip521SupReprt(obj.Id), "PreCommercialInvoiceExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, "546-ExportPreInvoiceSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportPreCommercialInvoiceSlip(int PrintId, string ReportName)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dtSlip = ExportPdfReport.ExportPreInvoiceSlip(obj);
			if (dtSlip == null || dtSlip.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ExportPdfReport.InvoiceSlip521SupReprt(obj.Id), "PreCommercialInvoiceExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtSlip, ReportName, lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportReturnGrnSlip241(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = ExportReturnGrn.ExportReturnGrnSlipandRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					throw new Exception("Not Record Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "215-ExportReturnGrnSlip.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ExportReturnInvoiceSlip242(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = ExportReturnInvoice.ExportReturnInvoiceSlipandRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId,
					ApprovedFilter = "All"
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					throw new Exception("Not Record Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "216-ExportReturnInvoiceSlip.rpt");
				return;
			}
			throw new Exception("Not Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static DataTable GetTaxScheduleDetailbyItemIds(string ItemIds, DateTime EffectedDate)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return ItemTaxSchedule.GetTaxScheduleDetailbyItemIds(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ItemIds = ItemIds,
				EffectedDate = EffectedDate
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable MultiLanguagesGetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return MultiLanguages.GetAll(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable VoucherFormHistory(bool CanViewAllRecord, string DocumentTypes, int NoofRecords = 0)
	{
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Expected O, but got Unknown
		try
		{
			DataTable dtHistory = new DataTable();
			dtHistory.Columns.Add("Id");
			dtHistory.Columns.Add("VoucherCode", typeof(int));
			dtHistory.Columns.Add("DocumentTypeId");
			dtHistory.Columns.Add("DocumentTypeCode", typeof(string));
			dtHistory.Columns.Add("VoucherDate", typeof(DateTime));
			dtHistory.Columns.Add("ManualBillNo");
			dtHistory.Columns.Add("AccountTitle");
			dtHistory.Columns.Add("AgainstAccount");
			dtHistory.Columns.Add("Remarks");
			dtHistory.Columns.Add("VoucherAmount", typeof(double));
			dtHistory.Columns.Add("FcyCode");
			dtHistory.Columns.Add("ExchangeRate", typeof(double));
			dtHistory.Columns.Add("FcyAmount", typeof(double));
			dtHistory.Columns.Add("UserName");
			dtHistory.Columns.Add("CheqNo");
			dtHistory.Columns.Add("Attachment");
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			vh.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			vh.UserId = clsGlobalVariables.UserAccount.ID;
			vh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			vh.Ids = DocumentTypes;
			vh.PostState = CanViewAllRecord;
			vh.NoOfRecords = NoofRecords;
			if (!vh.PostState)
			{
				vh.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			DataTable lst = VoucherHead.VoucherFormHistory(vh);
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtHistory.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["VoucherCode"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocumentTypeCode"], Conversion.ToDateTime(lst.Rows[i]["VoucherDate"]).ToString("dd-MMM-yyyy"), lst.Rows[i]["ManualBillNo"], lst.Rows[i]["AccountTitle"], lst.Rows[i]["AgainstAccount"], lst.Rows[i]["Remarks"], lst.Rows[i]["VoucherAmount"], lst.Rows[i]["Currencycode"], lst.Rows[i]["ExchangeCurrencyRate"], lst.Rows[i]["FcAmount"], lst.Rows[i]["UserName"], lst.Rows[i]["ChequeNo"], lst.Rows[i]["NoOfAttachments"]);
				}
			}
			return dtHistory;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetBaseCurrencyInfo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			return MultiCurrency.USP_GetBaseCurrencyInfo(new MultiCurrency
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DepartmentSlip451(int ReportId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = DepartmentRequest.DepartmentRequestHistory(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = ReportId,
				DocumentTypeId = 450,
				ApprovedFilter = "All"
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "451-RptDepartmentRequestSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseOrderSlip1100(int PrintId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = PurchaseOrder.PurchaseOrder_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 1100,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(PurchaseOrder.PurchaseOrderSlip_SubReport(PrintId), "PurchaseOrderSubReport.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(PurchaseOrder.PurchaseOrderSupplierExpense_SubReport(PrintId), "PurchaseOrderSupplierExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1100_PurchaseOrderSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseInvoicetDirectSlip_Feed1104(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				DataTable dt = InvPurchaseInvoice.PurchaseInvoiceDirectSlip(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.PurchaseInvoiceSupReprtForItemExpense(obj.Id), "InvPurchaseInvoice_ItemExpense_SubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "1104_PurchaseInvoiceDirectItemSlip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void PurchaseInvoicetDirectSlip_Feed1104A(int RecId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = RecId;
				DataTable dt = InvPurchaseInvoice.PurchaseInvoiceDirectSlip(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.PurchaseInvoiceSupReprtForSupplierExpense(obj.Id), "PurchaseInvoice_SupplierExpense_SubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "1104A_PurchaseInvoiceDirectPartySlip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void SaleOrderReport_1110(int PrintId)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int organizationId = clsGlobalVariables.UserAccount.OrganizationId;
			int CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			DataTable dt = SaleOrder.SaleOrderSlip(organizationId, CompanyId, PrintId);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> list = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(SaleOrder.SaleOrderCustomerExpense_SubReport(PrintId), "SaleOrderCustomerExpense_SubReport.rpt");
			list.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(SaleOrder.SaleOrderPaymentTermDetail_SubReport(PrintId), "SaleOrderPaymentTermsDetail_SubReport.rpt");
			list.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1110-SaleOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SaleInvoiceDirectSlip_1115(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.InvSaleInvoiceDirectSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceDirectPartySlip_1115A(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.InvSaleInvoiceDirectSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1115-InvRepSaleBillCustomer.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceSlipEngr_1603(int printId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeIds = "1603";
			obj.Id = printId;
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoice_PartySlip_Engr(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprtForItem(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1603-PurchaseBillSupplier_Engr_ItemSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PurchaseInvoiceSlipEngr_1603A(int printId, int SupCustId = 0, int SupCustGlId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeIds = "1603";
			obj.Id = printId;
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoice_PartySlip_Engr(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			obj.SupplierCustomerId = SupCustId;
			obj.AccountId = SupCustGlId;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)))
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDate);
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDate);
			}
			SubReportObject subReportObject2 = new SubReportObject(InvPurchaseInvoiceReports.GetDataForLedgerSubReport(obj), "getLedgerforPrint.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1603A-PurchaseInvoice_Engr_PartySlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PurchaseInvoiceDirectSlipEngr_1604(int printId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeIds = "1604";
			obj.Id = printId;
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprtForItem(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1604-PurchaseInvoiceDirect_Engr_ItemSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PurchaseInvoicePrint1654A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeIds = "1654";
			obj.Id = PrintId;
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprtForItem(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1654A_PurchaseInvoice_CustomerBillDirectSlip_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoicePrint1654(int PrintId, int SupCustId = 0, int SupCustGlId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeIds = "1654";
			obj.Id = PrintId;
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			obj.SupplierCustomerId = SupCustId;
			obj.AccountId = SupCustGlId;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)))
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDate);
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDate);
			}
			SubReportObject subReportObject2 = new SubReportObject(InvPurchaseInvoiceReports.GetDataForLedgerSubReport(obj), "getLedgerforPrint.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1654_PurchaseInvoice_CustomerBillDirectSlip_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceDirectPM_PartySlip_245(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeIds = "245";
			obj.Id = PrintId;
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "245_PurchaseInvoiceDirectPM_PartySlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PurchaseInvoiceDirectSlipEngr_1604A(int printId, int SupCustId = 0, int SupCustGlId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeIds = "1604";
			obj.Id = printId;
			DataTable dt = InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			obj.SupplierCustomerId = SupCustId;
			obj.AccountId = SupCustGlId;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)))
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDate);
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDate);
			}
			SubReportObject subReportObject2 = new SubReportObject(InvPurchaseInvoiceReports.GetDataForLedgerSubReport(obj), "getLedgerforPrint.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1604A-PurchaseInvoiceDirect_Engr_PartySlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleOrderSlipEngr_1605(int printId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = printId;
			obj.DocumentTypeIds = "1605";
			obj.ApprovedFilter = "All";
			DataTable dt = SaleOrder.SaleOrderSlipAndRegister_Engr(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(SaleOrderReports.SaleOrderExtraItemsDetail_SubReport(obj.Id), "SaleOrderExtraItemsDetail_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1605-SaleOrderSlipAndRegister_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleOrderSlipEngr_1656(int printId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = printId;
			obj.DocumentTypeIds = "1656";
			obj.ApprovedFilter = "All";
			DataTable dt = SaleOrder.SaleOrderSlipAndRegister_Engr(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(SaleOrderReports.SaleOrderExtraItemsDetail_SubReport(obj.Id), "SaleOrderExtraItemsDetail_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1656_SaleOrderSlip_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PurchaseOrderSlip_Engr1600(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = PurchaseOrder.PurchaseOrder_Slip_Engr(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				OrderId = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1600-PurchaseOrder_Slip_Engr.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvDeliveryOrderSlip_Engr1606(int PrintId, int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvDeliveryOrder.InvDeliveryOrderSlip_Engr(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1606-DeliveryOrderSlip_Engr.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvDeliveryOrderSlip_1200(int PrintId, int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvDeliveryOrder.InvDeliveryOrderSlip_Engr(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1200-DeliveryOrderSlipReturnable.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvDeliveryOrderSlip_Engr1657(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvDeliveryOrder.InvDeliveryOrderSlip_Engr(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = 1657
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1657_DeliveryOrderSlip_Engr.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void CommissionTradeLoadingDelivery_Slip_1702(int PrintId, int LanguageId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			obj.LanguageId = LanguageId;
			obj.DocumentTypeId = 1702;
			DataTable dt = CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject2 = new SubReportObject(CommTradeLoadingDelivery.CommTradeLoadingDeliveryPMExpense_SubReport(obj.Id, obj.LanguageId), "CommTradeLoadingDeliveryPMExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject = new SubReportObject(CommTradeLoadingDelivery.CommTradeLoadingDeliveryExpense_SubReport(obj.Id), "CommTradeLoadingDeliveryExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1702-CommissionTradeLoadingDelivery_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void CommissionTradeLoadingDelivery_Slip_1702A(int PrintId, int LanguageId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			obj.LanguageId = LanguageId;
			obj.DocumentTypeId = 1702;
			DataTable dt = CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject2 = new SubReportObject(CommTradeLoadingDelivery.CommTradeLoadingDeliveryPMExpense_SubReport(obj.Id, obj.LanguageId), "CommTradeLoadingDeliveryPMExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject = new SubReportObject(CommTradeLoadingDelivery.CommTradeLoadingDeliveryExpense_SubReport(obj.Id), "CommTradeLoadingDeliveryExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1702A-CommissionTradeLoadingDelivery_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void CommTradeTransaction_SlipWithSupplier_1704_01(int PrintId, int LanguageId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			obj.DocumentTypeId = 1704;
			obj.LanguageId = LanguageId;
			DataTable dt = CommTradeTransactionHeader.CommissionTransactionSlipAndRegister(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject2 = new SubReportObject(CommTradeTransactionHeader.CommTradeTransactionSupplierSummary_SubReport(obj.Id, obj.LanguageId), "1704_01_CommTradeTransactionSupplierSummary_SubReport.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject3 = new SubReportObject(CommTradeTransactionHeader.CommTradeTransactionPM_SubReport(obj.Id, obj.LanguageId), "CommTradeTransactionPM_SubReport.rpt");
			lstSubReports.Add(subReportObject3);
			SubReportObject subReportObject = new SubReportObject(CommTradeTransactionHeader.CommTradeTransactionCustomerExpense_SubReport(obj.Id), "1704-CommTradeTransactionCustomerExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1704_01_CommTradeTransaction_SlipWithSupplier.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void CommTradeTransaction_Slip_1704(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			obj.DocumentTypeId = 1704;
			DataTable dt = CommTradeTransactionHeader.CommissionTransactionSlipAndRegister(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(CommTradeTransactionHeader.CommTradeTransactionCustomerExpense_SubReport(obj.Id), "1704-CommTradeTransactionCustomerExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(CommTradeTransactionHeader.CommTradeTransactionPM_SubReport(obj.Id, obj.LanguageId), "CommTradeTransactionPM_SubReport.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1704-CommTradeTransactionHeader_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleInvoicePrint1608(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = 1608;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1608-SaleInvoice_CustomerBillDirect_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoicelSlip_1609(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.SaleInvoiceSlip_Engr(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportJournal = new SubReportObject(InvSaleInvoiceReports.SaleInvoiceSubReportJournalExpense(obj.Id), "InvSaleInvoiceCustomerBillRiceJournalExpSubRep.rpt");
			lstSubReports.Add(subReportJournal);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1609-InvRepSaleBillCustomer.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleInvoicelSlip_1609A(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.SaleInvoiceSlip_Engr(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1609A-InvRepSaleBillCustomer-Format-II.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleInvoicelEngr_Slip_1660(int RecId, int SupCustId = 0, int SupCustGlId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.SaleInvoiceSlip_Engr(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportJournal = new SubReportObject(InvSaleInvoiceReports.SaleInvoiceSubReportJournalExpense(obj.Id), "InvSaleInvoiceCustomerBillRiceJournalExpSubRep.rpt");
			lstSubReports.Add(subReportJournal);
			obj.SupplierCustomerId = SupCustId;
			obj.AccountId = SupCustGlId;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)))
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDate);
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDate);
			}
			SubReportObject subReportObject2 = new SubReportObject(InvPurchaseInvoiceReports.GetDataForLedgerSubReport(obj), "getLedgerforPrint.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1660_SaleInvoiceSlipEngr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleInvoicelEngr_Slip_1660A(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.SaleInvoiceSlip_Engr(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1660A_SaleInvoiceSlipEngr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleInvoicePrint1661(int PrintId, int SupCustId = 0, int SupCustGlId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = 1661;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			obj.SupplierCustomerId = SupCustId;
			obj.AccountId = SupCustGlId;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)))
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDate);
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDate);
			}
			SubReportObject subReportObject2 = new SubReportObject(InvPurchaseInvoiceReports.GetDataForLedgerSubReport(obj), "getLedgerforPrint.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1661_SaleInvoice_CustomerBillDirectSlip_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceEngr_Slip_1661A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.DocumentTypeId = 1661;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1661A_SaleInvoice_CustomerBillDirectSlip_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoicelReturnSlip_1611A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = 1611;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1611A-SaleInvoice_CustomerBillReturn_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleInvoiceRetunrEngr1662(int PrintId, int SupCustId = 0, int SupCustGlId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = 1662;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			obj.SupplierCustomerId = SupCustId;
			obj.AccountId = SupCustGlId;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)))
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDate);
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDate);
			}
			SubReportObject subReportObject2 = new SubReportObject(InvPurchaseInvoiceReports.GetDataForLedgerSubReport(obj), "getLedgerforPrint.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1662_SaleInvoiceReturn_Slip_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceRetunrEngr1662A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = 1662;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.InvSaleInvoiceDirectSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1662A_SaleInvoiceReturn_Slip_Engr.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ProdcutionFohAllocateToJobOrderPrint922(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ProdcutionFohAllocateToJobOrder.ProdcutionFohAllocateToJobOrder_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 922,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "922_ProdcutionFohAllocateToJobOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void BillOfMaterial_Slip1633(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = BomHeader.BillOfMaterial_SlipAndRegister(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(BomHeader.BillOfMaterialAllDetails_SubReport(obj.Id), "1634_BillOfMaterialAllDetails_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1633_USP_BillOfMaterial_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void Recipe_Slip1120(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.Id = PrintId;
				DataTable dt = BomHeader.BillOfMaterial_SlipAndRegister(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(BomHeader.BillOfMaterialAllDetails_SubReport(obj.Id), "1120_Recipe_SubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "1120_Recipe_Slip.rpt", lstSubReports);
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void WorkOrder_Slip1614(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = WorkOrder.WorkOrder_SlipAndRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1614_WorkOrder_Slip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void WorkOrderSemiFinish_Slip1621(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = WorkOrder.WorkOrderForSemiFinish_SlipAndRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1621_WorkOrderSemiFinish_Slip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void WorkOrderInProgress_Slip1620(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = WorkOrderInProgress.WorkOrderInProgress_SlipAndRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1620_WorkOrderInProgress_Slip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void StockTransfer_Slip_1663(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvStockTransferHeader.StockTransferRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 1663,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1663_StockTransfer_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void StockTransferforProductionrejection_Slip_1665(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvStockTransferHeader.StockTransferRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 1665,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1665_StockTransferForProductionRejection_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void BillOfMaterial_Slip1617(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1617;
			obj.Id = PrintId;
			DataTable dt = BomHeader.BillOfMaterial_SlipAndRegister(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(BomHeader.BillOfMaterialAllDetails_SubReport(obj.Id), "1634_BillOfMaterialAllDetails_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1617_USP_BillOfMaterial_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void BOM_Casting_Slip1664(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1664;
			obj.Id = PrintId;
			DataTable dt = BomHeader.BillOfMaterial_SlipAndRegister(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(BomHeader.BillOfMaterialAllDetails_SubReport(obj.Id), "BillOfMaterialAllDetails_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1664_BillOfMaterialCasting_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ProductionSlip_1619(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ProductionHeader.ProductionMfgSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1619-Production_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ProductionSlipEngr_1670(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = ProductionHeader.ProductionHeader_Slip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ProductionHeader.ProductionAllDetails_SubReport(obj.Id), "ProductionAllDetails_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1670_ProductionEngr_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void WorkOrderConcrete_1870(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.Id = PrintId;
				DataTable dtMain = WorkOrderHeader.WorkOrderHeader_Slip(obj);
				if (dtMain != null && dtMain.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(WorkOrderHeader.WorkOrder_InputSubReport(obj.Id), "WorkOrder_InputSubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dtMain, "1870_WorkOrderConcrete_Slip.rpt", lstSubReports);
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceAndRetunFinishGoodsPrint1859A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvPurchaseInvoice.PurchaseInvoiceDirectSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.PurchaseSupplierBillSubReport(obj.Id), "InvPurchaseInvoice_SupplierBillExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1859A_InvPurchaseInvoice_DirectSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceAndRetunFinishGoodsPrint1859(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvPurchaseInvoice.PurchaseInvoiceDirectSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.PurchaseExpenseSubReport(obj.Id), "InvPurchaseInvoice_ItemExpense_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1859_InvPurchaseInvoice_DirectSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void OverTimeSlip(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = OverTimeRequest.OTSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1010-OverTime_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static DataTable GetWagesAccount(string WagesAcIds = null, int WagesActivityId = 0, int ActionId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			return InvConractorWagesAccounts.GetWagesItemsByWagesTypeIds(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = WagesAcIds,
				WagesActivityId = WagesActivityId,
				ActionId = ActionId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLookupsByTypeIdDt(int InvLookupTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return InvLookUp.GetLookupsByTypeIdDt(new InvLookUp
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				InvLookupTypeId = InvLookupTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExportLookUpsDataByTypeIdDt(int LookupTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return ExImLookUps.GetByLookTypeId(new ExImLookUps
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				ExImLookUptypesId = LookupTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLookupsByTypeId_Engr(int TypeId)
	{
		try
		{
			return LookUps.GetDataByTypeId(TypeId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static GetWagesSchedule GetWagesRate(DateTime DocDate, double PackUom, int WagesAccoutId, int ContractorId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		try
		{
			return InvContractorWagesSchedule.GetWagesScheduleRateByEffectiveDateWagesAccountIdandPackSize(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				EffectedDate = Conversion.ToDateTime((object)DocDate),
				PackUomFrom = Conversion.ToDouble((object)PackUom),
				InvConractorWagesAccountsId = Conversion.ToInt((object)WagesAccoutId),
				ContractorId = Conversion.ToInt((object)ContractorId)
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool CheckItemsFreeofcostforWages(DateTime DocDate, int RefDocumentTypeId, int ItemId, int WagesAccountId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		try
		{
			return InvContractorWagesSchedule.CheckItemsFreeofcostforWages(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocDate = Conversion.ToDateTime((object)DocDate),
				RefDocumentTypeId = Conversion.ToInt((object)RefDocumentTypeId),
				ItemId = Conversion.ToInt((object)ItemId),
				AccountId = Conversion.ToInt((object)WagesAccountId)
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GatePassInwardGenerateCode(int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			return GatePassInward.GenerategpCode(new GatePassInward
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GatePassInwardGenerateCodeByRefDocumentTypeId(int DocumentTypeId, int RefDocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return GatePassInward.GenerateCodeByRefDocumentTypeId(new GatePassInward
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				RefDocumentTypeId = RefDocumentTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GatePassInwardGatepassHistory(int DocumentTypeId, bool status, int NoOfRecords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = status;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			obj.NoOfRecords = NoOfRecords;
			return GatePassInward.GatepassHistory(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GatepassHistoryForRefdocumentTypeWise(int DocumentTypeId, bool status, int NoOfRecords, int RefDocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = status;
			obj.RefDocumentTypeId = RefDocumentTypeId;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			return GatePassInward.GatepassHistoryForRefdocumentTypeWise(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByGpNoInward(int DocumentTypeId, int GpId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return GatePassInward.GetByGpNo(new GatePassInward
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				Id = GpId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseOrderNoByGpId(int GpId, int RecId)
	{
		try
		{
			return GatePassInward.PurchaseOrderNoByGpId(GpId, RecId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GatePassInwardSlipAndRegisterReport(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = GatePassInwardReports.GatePassInwardSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 51,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "251-InvRptInwardGatePassSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void GatePassInwardWithDetailAndLabReport(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = GatePassInwardReports.GatePassInwardSlipandRegister_WithDetail_256(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 51,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvLabAnalysisPurchaseHeader.RptInvLabPurchaseAnalysisSlip653(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				GpId = PrintId
			}), "InvLabPurchaseAnalysisSubForGP.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "256-GatePassInward_WithDetailSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static DataTable GatePassTypeFill(string Activity)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return GatepassType.GetAll(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Activity = Activity
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GoodsReceiptsNotesSlip1618(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = InvGrn.InvGrnRegisterAndSlipLoader_Engr(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeIds = "1618",
					Id = PrintId,
					ApprovedFilter = "All"
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "1618-GoodsReceiptsNotesSlip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void GoodsReceiptsNotesSlip1602(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = InvGrn.InvGrnRegisterAndSlipLoader_Engr(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeIds = "1602",
					Id = PrintId,
					ApprovedFilter = "All"
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "1602-GoodsReceiptsNotesSlip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void GoodsReceiptsNotesSlip1602_01(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = InvGrn.InvGrnRegisterAndSlipLoader_Engr(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeIds = "1602",
					Id = PrintId,
					ApprovedFilter = "All"
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "1602_01-GoodsReceiptsNotesSlip.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void InwardGatePassSlipEngr1601(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = GatePassInwardReports.GatePassInwardSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 1601,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1601-InwardGatePassSlipEngr.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void GatePassGeneralInward254(int PrintId, int DocumentTypeId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = GatePassInwardReports.GatePassGeneralInward254(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "254-RptInwardGatePassSlipGeneral.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void OutwardGatePassWithWb(int GatePassId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (GatePassId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = GatePassId;
				DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject1 = new SubReportObject(WbTransactions.WbTransationByOutwardGPID_SubReport(obj.Id), "WbTransationByOutwardGPID_SubReport.rpt");
				lstSubReports.Add(subReportObject1);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "258-OutwardGatePassWithWbAndLabSlip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void InwardGatePassWithWbAndLabSlip(int GatePassId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (GatePassId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = GatePassId;
				DataTable dt = GatePassInwardReports.GatePassInwardSlipandRegister(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(InvLabAnalysisPurchaseHeader.LabAnalysisPurchaseByGP_SubReport(obj), "LabAnalysisPurchaseByGPSubReport.rpt");
				SubReportObject subReportObject2 = new SubReportObject(WbTransactions.WbTransationByGPID_SubReport(obj.Id), "WbTransationByGPIDSubReport.rpt");
				lstSubReports.Add(subReportObject);
				lstSubReports.Add(subReportObject2);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "257-InwardGatePassWithWbAndLabSlip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public static void GrnPMSlipReport214(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvGrnandGdnReports.GrnSlipStore(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 701,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "214-GRNPackingMaterialSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GrnPmSlipWithSubReport214_01(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 701,
				Id = PrintId
			};
			DataTable dt = InvGrnandGdnReports.GrnSlipStore(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			string orderIds = string.Join(",", (from row in dt.AsEnumerable()
				select row["PurchaseOrderId"].ToString()).Distinct());
			DataTable dtsub = InvGrnandGdnReports.PurchaseOrder_WithGrnDetail_Report(obj.OrganizationId, obj.CompanyId, orderIds);
			List<SubReportObject> lstSubReports = new List<SubReportObject>
			{
				new SubReportObject(dtsub, "PurchaseOrder_WithGrnDetail_SubReport.rpt")
			};
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName?.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress?.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "214_01_GrnPackingMaterialSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GrnSlipReport213(int DocumentTypeId, int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.Id = PrintId;
			DataTable dt = InvGrnandGdnReports.InvGrnSlip211(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvGrnandGdnReports.GrnDetailEmptyBagsSubReport(obj.Id), "GrnDetailEmptyBagsSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "213-GoodsReceiptsNotesAgainstOrderSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GrnSlipReport212(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.GrnSlipStore(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 48,
				Id = PrintId
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "212-InvRptGoodsReceiptsNotesStoreSlip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GrnEmptyBagsSlip212_01(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.GrnSlipStore(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 48,
				BaseDocumentTypeId = 4,
				Id = PrintId
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "212_01_GoodsReceiptsNotesEmptyBagsSlip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GrnSlipReport211(int DocumentTypeId, int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.InvGrnSlip211(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "211-InvRptGoodsReceiptsNotesRiceSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GdnForSaleToPartyProcssingSlip221(int PrintId, int DocumentTypeId = 0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject1 = new SubReportObject(InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 101,
				RefRefDocIdNo = PrintId,
				RefDocumentTypeId = DocumentTypeId
			}), "InvContractorWagesSubReport.rpt");
			lstSubReports.Add(subReportObject1);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "221_GdnForSaleToPartyProcssingSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GrnSlipWithSubReports(int PrintId, int DocumentTypeId = 0)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = ((DocumentTypeId > 0) ? DocumentTypeId : 46);
			obj.Id = PrintId;
			DataTable dt = InvGrnandGdnReports.InvGrnSlip211(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvGrnandGdnReports.GrnDetailEmptyBagsSubReport(obj.Id), "GrnDetailEmptyBagsSubReport.rpt");
			lstSubReports.Add(subReportObject);
			ReportsParameters objwages = new ReportsParameters();
			objwages.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			objwages.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			objwages.DocumentTypeId = 101;
			objwages.RefRefDocIdNo = PrintId;
			if (DocumentTypeId == 0)
			{
				objwages.RefDocumentTypeId = 46;
			}
			else
			{
				objwages.RefDocumentTypeId = DocumentTypeId;
			}
			SubReportObject subReportObject2 = new SubReportObject(InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister(objwages), "InvContractorWagesSubReport.rpt");
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "211-InvRptGoodsReceiptsNotesRiceSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void GrnAgainstTypeSlip_Engr(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrn.InvGrnRegisterAndSlipLoader_Engr(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "1618",
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1618-GoodsReceiptsNotesSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static int GetGrnCountAgainstGatepass(int GatePassId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return InvGrn.GetGrnCountAgainstGatepass(new InvGrn
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				InwardGatePassId = GatePassId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByGpNo(int DocumentTypeId, int GpId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return GatePassOutward.GetByGpNo(new GatePassOutward
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				Id = GpId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvGdnSlip260(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "260-InvRptGdnRiceSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void Gdn_PurchaseReturnSlip703(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.InvGdn_PurchaseReturnReport(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "703_Gdn_PurchaseReturnSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void Gdn_PurchaseReturnDeliveryChallanSlip703A(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.InvGdn_PurchaseReturnReport(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "703A_Gdn_PurchaseReturnSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void DepartmentRequestSlip1615(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = DepartmentRequest.DepartmentRequestHistory(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = 1615,
				ApprovedFilter = "All"
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1615-DepartmentRequestToConsumableStoreSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void InvGdnSlip1612(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1612-InvGdn_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void GdnReturnable_Slip_1203(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1203_GdnReturnable_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void GdnStoreSlip144(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "144-InvGdn_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void GdnStoreSlip149(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGrnandGdnReports.InvGdnSlip260(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "149-InvGdnStorePmSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static int InvGdnGenerateCode(int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			return InvGdn.GenerateInvGdnCode(new InvGdn
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvGdnFormHistory(int DocumentTypeId, bool status, int NoOfRecords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = status;
			obj.NoOfRecords = NoOfRecords;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			return InvGdn.GetHisoty(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int PurchaseOrderGenerateCode(int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			return PurchaseOrder.GenerateCode(new PurchaseOrder
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateOrderCategoryCodebyId(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			return InvOrderCategory.GenerateOrderCategoryCodebyId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = Id
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseOrderHistory(int DocumentTypeId, bool status, int NoOfRecords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = status;
			obj.NoOfRecords = NoOfRecords;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			return PurchaseOrder.Getall(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int PurchaseInvoiceGenerateCode(int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			return InvPurchaseInvoice.GenerateInvPurchaseInvoiceCode(new InvPurchaseInvoice
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int PurchaseInvoiceGenerateCodeBranch(int DocumentTypeId, int BranchesId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return InvPurchaseInvoice.GenerateInvPurchaseInvoiceCodeBranch(new InvPurchaseInvoice
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				BranchesId = BranchesId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateSalesTaxNo(bool IsTaxable)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return InvPurchaseInvoice.GenerateSalesTaxNo(new InvPurchaseInvoice
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				IsTaxable = IsTaxable
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable PurchaseInvoiceHistory(int DocumentTypeId, bool status, int NoOfRecords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = status;
			obj.NoOfRecords = NoOfRecords;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			return InvPurchaseInvoice.FormHistory(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int InvSaleGenerateSalesTaxNo(bool IsTaxable)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return InvSaleInvoice.GenerateSalesTaxNo(new InvSaleInvoice
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				IsTaxable = IsTaxable
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int StoreIssuanceGenerateCode(int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			return InvGsStoreIssuanceHeader.GenerateCode(new InvGsStoreIssuanceHeader
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable StoreIssuanceHistory(int DocumentTypeId, bool status, int NoOfRecords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = status;
			obj.NoOfRecords = NoOfRecords;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			return InvGsStoreIssuanceHeader.FormHistory(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLcOrderNoBySupplierCustomerId(int SupplierCustomerId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.GetLcOrderNoBySupplierCustomerId(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				SupplierCustomerId = SupplierCustomerId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLcOrderNo(int FinancialYearId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			if (FinancialYearId > 0)
			{
				obj.FinancialYearId = FinancialYearId;
			}
			return ExImLcOrder.GetLcOrderNo(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExportInvoiceNo(int SupplierCustomerId = 0, int FinancialYearId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImInvoice obj = new ExImInvoice();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.SupplierCustomerId = SupplierCustomerId;
			if (FinancialYearId > 0)
			{
				obj.FinancialYearId = FinancialYearId;
			}
			return ExImInvoice.GetInvoiceNo(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoiceNoandPartiesForFcBankReceipts(int SupplierCustomerId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			return ExImInvoice.GetInvoiceNoandPartiesForFcBankReceipts(new ExImInvoice
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				SupplierCustomerId = SupplierCustomerId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemCategoryFromInventoryStocksEvaluations(string ParentIds = null, int ActionId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return InventoryStockEvalautionDetail.InvItemCategoryTypeInStock(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds,
				ActionId = ActionId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemTypeFromInventoryStocksEvaluations(string ParentIds = null, int ActionId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return InventoryStockEvalautionDetail.InvItemTypeInStock(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds,
				ActionId = ActionId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromInventoryStocksEvaluations(string ParentIds = null, int ActionId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return Item.GetItemsFromInventoryStocksEvaluations(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds,
				ActionId = ActionId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWarehouseFromInventoryStocksEvaluations(string ParentIds = null, int ActionId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return Item.GetWarehouseFromInventoryStocksEvaluations(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds,
				ActionId = ActionId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetJobLotFromInventoryStocksEvaluations(string ParentIds = null, int ActionId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return Item.GetJobLotFromInventoryStocksEvaluations(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds,
				ActionId = ActionId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierCustomerFromInventoryStocksEvaluations(string ParentIds = null, int ActionId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return Item.GetSupplierCustomerFromInventoryStocksEvaluations(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds,
				ActionId = ActionId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemTypeFromInventoryTransactions(string ParentIds = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetItemTypeFromInventoryTransactions(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemCategoryFromInventoryTransactions(string ParentIds = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetItemCategoryFromInventoryTransactions(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetItemsFromInventoryTransactions(string ParentIds = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetItemsFromInventoryTransactions(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWarehouseFromInventoryTransactions(string ParentIds = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetWarehouseFromInventoryTransactions(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetJobLotFromInventoryTransactions(string ParentIds = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetJobLotFromInventoryTransactions(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierCustomerFromInventoryTransactions(string ParentIds = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			return Item.GetSupplierCustomerFromInventoryTransactions(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Ids = ParentIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void LoadCameraSDKForm()
	{
		try
		{
			uint err = EDSDK.EdsInitializeSDK();
			bool isSDKLoaded = false;
			if (err == 0)
			{
				isSDKLoaded = true;
			}
			IntPtr cameraList = IntPtr.Zero;
			if (err == 0)
			{
				err = EDSDK.EdsGetCameraList(out cameraList);
			}
			if (err == 0)
			{
				int count = 0;
				err = EDSDK.EdsGetChildCount(cameraList, out count);
				if (count == 0)
				{
					err = 128u;
				}
			}
			IntPtr camera = IntPtr.Zero;
			if (err == 0)
			{
				err = EDSDK.EdsGetChildAtIndex(cameraList, 0, out camera);
			}
			if (err == 0)
			{
				err = EDSDK.EdsGetDeviceInfo(camera, out var _);
				if (err == 0 && camera == IntPtr.Zero)
				{
					err = 128u;
				}
			}
			if (cameraList != IntPtr.Zero)
			{
				EDSDK.EdsRelease(cameraList);
			}
			CameraModel model = null;
			if (err == 0)
			{
				model = new CameraModel(camera);
			}
			CameraController controller = null;
			if (err != 0)
			{
				new DigitalCameraView(ref controller).Show();
				return;
			}
			EDSDK.EdsPropertyEventHandler handlePropertyEvent = CameraEventListener.HandlePropertyEvent;
			EDSDK.EdsObjectEventHandler handleObjectEvent = CameraEventListener.HandleObjectEvent;
			EDSDK.EdsStateEventHandler handleStateEvent = CameraEventListener.HandleStateEvent;
			if (err == 0)
			{
				controller = new CameraController(ref model);
				GCHandle handle = GCHandle.Alloc(controller);
				IntPtr ptr = GCHandle.ToIntPtr(handle);
				if (err == 0)
				{
					err = EDSDK.EdsSetPropertyEventHandler(camera, 256u, handlePropertyEvent, ptr);
				}
				if (err == 0)
				{
					err = EDSDK.EdsSetObjectEventHandler(camera, 512u, handleObjectEvent, ptr);
				}
				if (err == 0)
				{
					err = EDSDK.EdsSetCameraStateEventHandler(camera, 768u, handleStateEvent, ptr);
				}
				controller.Run();
				IObserver form;
				IObserver observer = (form = new DigitalCameraView(ref controller));
				model.Add(ref form);
				((Form)observer).ShowDialog();
				handle.Free();
			}
			GC.KeepAlive(handlePropertyEvent);
			GC.KeepAlive(handleObjectEvent);
			GC.KeepAlive(handleStateEvent);
			if (camera != IntPtr.Zero)
			{
				EDSDK.EdsRelease(camera);
			}
			if (isSDKLoaded)
			{
				EDSDK.EdsTerminateSDK();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static DataTable Vouchers_GetLastRecord(int AccountTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			return VoucherReports.Vouchers_GetLastRecord(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				AccountTypeId = AccountTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DateType()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Parameters");
			dt.Rows.Add(1, "This Day");
			dt.Rows.Add(2, "This Week");
			dt.Rows.Add(3, "This Month");
			dt.Rows.Add(4, "This Year");
			dt.Rows.Add(5, "Financial Year");
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GetSlipsByDocumentTypeId(int DocumentTypeId, int DocumentTypeIdSrNo = 0, int Id = 0, int AccountId = 0, DateTime? FromDate = null, DateTime? ToDate = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UserAccount UserAccount = new UserAccount();
			UserAccount = clsGlobalVariables.UserAccount;
			string TargetUrl = "";
			string screenName = "";
			switch (DocumentTypeId)
			{
			case 56:
			case 166:
			case 172:
				TargetUrl = "Architecture.WinApp.Commentaries.PurchaseInvoiceReverseActivity";
				screenName = "PurchaseInvoiceReverseActivity";
				if (Type.GetType(TargetUrl) != null)
				{
					if (Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount, DocumentTypeIdSrNo) is Form form)
					{
						form.WindowState = FormWindowState.Maximized;
						form.Tag = Conversion.ToString((object)screenName);
						form.Show();
					}
					return;
				}
				throw new Exception("Target URL not Configured properly... " + TargetUrl);
			case 1502:
				PurchaseInvoiceSteelSlip_1508A(DocumentTypeIdSrNo);
				return;
			case 46:
			case 137:
				GrnSlipWithSubReports(DocumentTypeIdSrNo, DocumentTypeId);
				return;
			case 68:
				StockTransferSlip406(DocumentTypeIdSrNo);
				return;
			case 86:
			case 124:
			case 170:
				InvGdnSlip260(DocumentTypeIdSrNo);
				return;
			case 145:
				PurchaseInvoiceReturn_StorePartySlip145(DocumentTypeIdSrNo);
				return;
			case 205:
			case 210:
				ForwardingSlip507(DocumentTypeIdSrNo);
				return;
			}
			switch (DocumentTypeId)
			{
			case 205:
			case 210:
				ForwardingDirectSlip507_01(DocumentTypeIdSrNo);
				return;
			case 806:
				StockTransferSlip407(DocumentTypeIdSrNo);
				return;
			case 451:
				StoreIssuanceSlip452(DocumentTypeIdSrNo);
				return;
			case 701:
				GrnPmSlipWithSubReport214_01(DocumentTypeIdSrNo);
				return;
			case 1503:
				PurchaseInvoiceSteelSlip_1510A(DocumentTypeIdSrNo);
				return;
			case 1509:
				SaleInvoiceSteelSlip_1514A(DocumentTypeIdSrNo);
				return;
			case 1510:
				SaleInvoiceDirectSteelSlip_1516A(DocumentTypeIdSrNo);
				return;
			case 95:
			case 171:
				TargetUrl = "Architecture.WinApp.Commentaries.SaleInvoiceReverseActivity";
				screenName = "SaleInvoiceReverseActivity";
				if (Type.GetType(TargetUrl) != null)
				{
					if (Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount, DocumentTypeIdSrNo) is Form form2)
					{
						form2.WindowState = FormWindowState.Maximized;
						form2.Tag = Conversion.ToString((object)screenName);
						form2.Show();
					}
					return;
				}
				throw new Exception("Target URL not Configured properly... " + TargetUrl);
			case 99:
			case 139:
				SaleInvoiceDirectSlip_294("294A-InvRptSaleBillDirectWithoutSO.rpt", DocumentTypeIdSrNo);
				return;
			}
			switch (DocumentTypeId)
			{
			case 95:
				SaleInvoicetSlip_301(DocumentTypeIdSrNo, 0, AccountId, FromDate, ToDate);
				break;
			case 98:
				PurchaseInvoicetDirectSlip_225A(DocumentTypeIdSrNo);
				break;
			case 57:
				PurchaseInvoicetDirectSlip_225A(DocumentTypeIdSrNo);
				break;
			case 56:
			case 138:
				PurchaseInvoicettSlip_220A(DocumentTypeIdSrNo);
				break;
			case 61:
				PurchaseInvoiceStoreDirectSlip_233(DocumentTypeIdSrNo);
				break;
			case 131:
				PurchaseInvoiceStoreDirectSlip_237(DocumentTypeIdSrNo);
				break;
			case 64:
				StorePurchaseInvoiceSlip(DocumentTypeIdSrNo, "230-InvRptPurchaseBillStoreSlip.rpt");
				break;
			case 58:
				StorePurchaseInvoiceSlip(DocumentTypeIdSrNo, "238-InvPurchaseInvoice_StoreBillWithTax.rpt");
				break;
			case 59:
				PurchaseInvoicettSlip_220A(DocumentTypeIdSrNo);
				break;
			case 452:
				StoreIssuanceSlip452(DocumentTypeIdSrNo);
				break;
			case 66:
				StockConversionSummary_605(DocumentTypeIdSrNo);
				break;
			case 70:
				StockAdjustmentSlipAndRegister409(DocumentTypeIdSrNo);
				break;
			case 80:
				ProductionInPutAndOutPut601(DocumentTypeIdSrNo, "Input");
				break;
			case 112:
				ProductionInPutAndOutPut601(DocumentTypeIdSrNo, "Output");
				break;
			case 181:
				ProductionConsumptionReport_623(DocumentTypeIdSrNo);
				break;
			case 111:
				ProductionPMAndOverHeads_606(DocumentTypeIdSrNo, "PackingMaterial");
				break;
			case 110:
				ProductionPMAndOverHeads_606(DocumentTypeIdSrNo, "OverHead");
				break;
			case 101:
				ContractorWagesBill_SlipandRegister_002(DocumentTypeIdSrNo);
				break;
			case 810:
				ContractorWagesBillManualSlip004(DocumentTypeIdSrNo);
				break;
			case 702:
				StorePurchaseInvoiceSlip(DocumentTypeIdSrNo, "231-InvRptPurchaseBillPackingMaterialSlip.rpt");
				break;
			case 16:
			case 24:
			case 29:
				VoucherReport_118(Id);
				break;
			case 8:
			case 9:
				DayBookSlip(Id);
				break;
			case 204:
				CommercialInvoiceSlip521(DocumentTypeIdSrNo);
				break;
			case 162:
				CommisionTradeBillAndSlip168(DocumentTypeIdSrNo, "168-CommisionTradeBillAndSlip.rpt");
				break;
			case 1056:
				CrystalReportPrint_Helper.CommissionAgentBillSlip_1056(DocumentTypeIdSrNo);
				break;
			case 1603:
				PurchaseInvoiceSlipEngr_1603A(DocumentTypeIdSrNo, 0, AccountId, FromDate, ToDate);
				break;
			case 1604:
				PurchaseInvoiceDirectSlipEngr_1604A(DocumentTypeIdSrNo, 0, AccountId, FromDate, ToDate);
				break;
			case 1654:
				PurchaseInvoicePrint1654(DocumentTypeIdSrNo, 0, AccountId, FromDate, ToDate);
				break;
			case 1608:
				SaleInvoicePrint1608(DocumentTypeIdSrNo);
				break;
			case 1609:
				SaleInvoicelSlip_1609(DocumentTypeIdSrNo);
				break;
			case 1660:
				SaleInvoicelEngr_Slip_1660(DocumentTypeIdSrNo, 0, AccountId, FromDate, ToDate);
				break;
			case 1661:
				SaleInvoicePrint1661(DocumentTypeIdSrNo, 0, AccountId, FromDate, ToDate);
				break;
			case 1662:
				SaleInvoiceRetunrEngr1662(DocumentTypeIdSrNo, 0, AccountId, FromDate, ToDate);
				break;
			case 1611:
				SaleInvoicelReturnSlip_1611A(DocumentTypeIdSrNo);
				break;
			case 1859:
				PurchaseInvoiceAndRetunFinishGoodsPrint1859A(DocumentTypeIdSrNo);
				break;
			case 1865:
				PurchaseInvoiceAndRetunFinishGoodsPrint1859A(DocumentTypeIdSrNo);
				break;
			case 1856:
				SaleInvoiceDirectConcreteCustomerSlip(DocumentTypeIdSrNo);
				break;
			case 1861:
				SaleInvoiceConcreteCustomerSlip(DocumentTypeIdSrNo);
				break;
			case 1862:
				SaleInvoiceReturnConcreteCustomerSlip(DocumentTypeIdSrNo);
				break;
			case 1850:
				ProductionSlipConcrete(DocumentTypeIdSrNo);
				break;
			case 1104:
				PurchaseInvoicetDirectSlip_Feed1104A(DocumentTypeIdSrNo);
				break;
			case 155:
				ServicesBillSlip540(DocumentTypeIdSrNo);
				break;
			case 1304:
				CrystalReportPrint_Helper.ServicesBillHeader_Slip_1304(DocumentTypeIdSrNo);
				break;
			case 39:
			case 40:
				VoucherReport_118(Id);
				break;
			default:
				ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
				break;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetDataTypeString(Type type)
	{
		if (type == typeof(int))
		{
			return "int";
		}
		if (type == typeof(string))
		{
			return "string";
		}
		if (type == typeof(double))
		{
			return "double";
		}
		if (type == typeof(decimal))
		{
			return "decimal";
		}
		if (type == typeof(DateTime))
		{
			return "DateTime";
		}
		if (type == typeof(bool))
		{
			return "bool";
		}
		return type.ToString();
	}

	public static List<string> GetMissingColumns(DataTable dt, XmlDocument xmlDoc)
	{
		XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//Columns/*");
		HashSet<string> xmlColumnIDs = new HashSet<string>();
		foreach (XmlNode item in xmlNodeList)
		{
			string id = item.Attributes["ID"]?.InnerText;
			if (!string.IsNullOrEmpty(id))
			{
				xmlColumnIDs.Add(id);
			}
		}
		List<string> missingColumns = new List<string>();
		foreach (DataColumn column in dt.Columns)
		{
			if (!xmlColumnIDs.Contains(column.ColumnName))
			{
				missingColumns.Add(column.ColumnName);
			}
		}
		return missingColumns;
	}

	private static int GetNextColumnIndex(XmlDocument xmlDoc)
	{
		XmlNodeList columnNodes = xmlDoc.SelectNodes("//Columns/*");
		if (columnNodes != null && columnNodes.Count > 0)
		{
			return int.Parse(columnNodes[columnNodes.Count - 1].Name.Replace("Column", "")) + 1;
		}
		return 1;
	}

	private static void SetColumnAttributes(XmlElement columnElement, string id, string caption, string dataMember, string key)
	{
		columnElement.SetAttribute("ID", caption);
		AppendChildElement(columnElement, "Caption", caption);
		AppendChildElement(columnElement, "DataTypeCode", dataMember);
		AppendChildElement(columnElement, "DataMember", id);
		AppendChildElement(columnElement, "Key", id);
		AppendChildElement(columnElement, "Position", "0");
		AppendChildElement(columnElement, "Width", "150");
		AppendChildElement(columnElement, "Visible", "false");
	}

	private static void AppendChildElement(XmlElement parentElement, string name, string value)
	{
		XmlElement childElement = parentElement.OwnerDocument.CreateElement(name);
		childElement.InnerText = value;
		parentElement.AppendChild(childElement);
	}

	public static void AddNewColumn(XmlDocument xmlDoc, string filePath, string id, string dataMember)
	{
		try
		{
			XmlNode columnsNode = xmlDoc.SelectSingleNode("//Columns");
			if (columnsNode != null)
			{
				XmlElement newColumn = xmlDoc.CreateElement($"Column{GetNextColumnIndex(xmlDoc)}");
				SetColumnAttributes(newColumn, id, Regex.Replace(id, "([A-Z0-9]+)", " $1").Trim(), dataMember, id);
				columnsNode.AppendChild(newColumn);
				xmlDoc.Save(filePath);
			}
			else
			{
				MessageBox.Show("Columns node not found.");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	public static FileStream GetGridLayout(string FormName, string GridName)
	{
		try
		{
			string LayoutPath = "";
			string FileName = FormName + "_" + GridName + "_" + clsGlobalVariables.UserAccount.UserName;
			FileStream fs = null;
			if (GridLayout.GetByID(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, FileName) != null)
			{
				LayoutPath = "C:\\SCS\\Layout\\" + FileName;
				if (File.Exists(LayoutPath))
				{
					fs = new FileStream(LayoutPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				}
			}
			return fs;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteGrdLayout(string FormName, string GridName)
	{
		try
		{
			string LayoutPath = "";
			string FileName = FormName + "_" + GridName + "_" + clsGlobalVariables.UserAccount.UserName;
			LayoutPath = "C:\\SCS\\Layout\\";
			if (File.Exists(LayoutPath + FileName))
			{
				LayoutPath += FileName;
				using FileStream fs = new FileStream(LayoutPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				fs.Flush();
				fs.Close();
				fs.Dispose();
				File.Delete(fs.Name);
				GridLayout.DeleteByLayout(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, FileName);
				MessageBox.Show("Layout Removed successfully");
				return;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static FileStream GetGridLayoutNew(string FormName, string GridName, GridEX MyGrid)
	{
		try
		{
			string LayoutPath = "";
			string FileName = FormName + "_" + GridName + "_" + clsGlobalVariables.UserAccount.UserName;
			FileStream fs = null;
			GridLayout obj = GridLayout.GetByID(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, FileName);
			if (obj != null)
			{
				LayoutPath = Application.ExecutablePath + "\\..\\Layouts\\";
				if (LayoutPath != string.Empty)
				{
					LayoutPath += FileName;
				}
				if (!File.Exists(LayoutPath))
				{
					ConvertVarBinaryToFileStream(obj.LayoutFile, LayoutPath, MyGrid);
				}
				if (File.Exists(LayoutPath))
				{
					fs = new FileStream(LayoutPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				}
			}
			return fs;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static FileStream ConvertVarBinaryToFileStream(byte[] varbinaryData, string filePath, GridEX MyGrid = null)
	{
		try
		{
			using MemoryStream memoryStream = new MemoryStream(varbinaryData);
			using FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
			MyGrid.SaveLayoutFile((Stream)fileStream);
			memoryStream.CopyTo(fileStream);
			fileStream.Flush();
			fileStream.Close();
			fileStream.Dispose();
			return fileStream;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error: " + ex.Message);
			return null;
		}
	}

	public static void GridAutoAdjustment(int[] AryColWidth, GridEX grd)
	{
		try
		{
			Array.Resize(ref AryColWidth, ((JanusCollectionBase)grd.RootTable.Columns).Count - 1);
			for (int index2 = 0; index2 < ((JanusCollectionBase)grd.RootTable.Columns).Count - 1; index2++)
			{
				AryColWidth[index2] = grd.RootTable.Columns[index2].Width;
			}
			for (int index = 0; index < ((JanusCollectionBase)grd.RootTable.Columns).Count - 1; index++)
			{
				grd.RootTable.Columns[index].AutoSize();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GridAutoAdjustmentNew(GridEX grd)
	{
		try
		{
			int[] AryColWidth = new int[0];
			Array.Resize(ref AryColWidth, ((JanusCollectionBase)grd.RootTable.Columns).Count - 1);
			for (int index2 = 0; index2 < ((JanusCollectionBase)grd.RootTable.Columns).Count - 1; index2++)
			{
				AryColWidth[index2] = grd.RootTable.Columns[index2].Width;
			}
			for (int index = 0; index < ((JanusCollectionBase)grd.RootTable.Columns).Count - 1; index++)
			{
				grd.RootTable.Columns[index].AutoSize();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RemoveByID(int DocumentTypeId, int RecordId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			CommonRepository.RemoveByID(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				UserId = clsGlobalVariables.UserAccount.ID,
				DocumentTypeId = DocumentTypeId,
				Id = RecordId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void WagesDeleteByReferenceIds(int DocumentTypeId, int RecordId)
	{
		try
		{
			InvContractorWagesBillHeader.ContractorWagesRemovebyReferenceIds(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, DocumentTypeId, RecordId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GatePassOutwardSlipandRegisterForSteel(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegisterForSteel(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1512-GatePassOutwardSlipAndRegisterSteel.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void StockMonthlyClosing_Slip(DateTime ClosingDate)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Conversion.CheckDateTimeNull(ClosingDate))
			{
				throw new Exception("Closing Date Not Found");
			}
			DataTable dt = StockMonthlyClosingHeader.MonthlyClosingStock_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocDate = ClosingDate
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Record not found for display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "300-StockMonthlyClosingSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void EditMethodFromLinkedForSteel(int DocumentTypeId, int Id, int DocumentTypeIdSrNo = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UserAccount UserAccount = new UserAccount();
			UserAccount = clsGlobalVariables.UserAccount;
			string TargetUrl = "";
			string screenName = "";
			string ReadById = "";
			switch (DocumentTypeId)
			{
			case 1500:
				TargetUrl = "Architecture.WinApp.Steel.Purchase.PurchsaeOrder_St";
				screenName = "PurchsaeOrder_St";
				ReadById = "ReadById";
				break;
			case 1501:
				TargetUrl = "Architecture.WinApp.Steel.Purchase.InvFrmGRN_St";
				screenName = "InvFrmGRN_St";
				ReadById = "ReadById";
				break;
			case 1502:
				TargetUrl = "Architecture.WinApp.Steel.Purchase.InvfrmPurchaseInvoice_St";
				screenName = "InvfrmPurchaseInvoice_St";
				ReadById = "ReadById";
				break;
			case 1503:
				TargetUrl = "Architecture.WinApp.Steel.Purchase.PurchaseInvoiceDirect_St";
				screenName = "PurchaseInvoiceDirect_St";
				ReadById = "ReadById";
				break;
			case 1505:
				TargetUrl = "Architecture.WinApp.Steel.Sale.SaleOrder_St";
				screenName = "SaleOrder_St";
				ReadById = "ReadById";
				break;
			case 1506:
				TargetUrl = "Architecture.WinApp.Steel.Sale.DeliveryOrder_St";
				screenName = "DeliveryOrder_St";
				ReadById = "ReadById";
				break;
			case 1507:
				TargetUrl = "Architecture.WinApp.Steel.Sale.OutwardGatePass_St";
				screenName = "OutwardGatePass_St";
				ReadById = "ReadById";
				break;
			case 1508:
				TargetUrl = "Architecture.WinApp.Steel.Sale.InvFrmGDN_St";
				screenName = "InvFrmGDN_St";
				ReadById = "ReadById";
				break;
			case 1509:
				TargetUrl = "Architecture.WinApp.Steel.Sale.SaleInvoice_St";
				screenName = "SaleInvoice_St";
				ReadById = "ReadById";
				break;
			case 1510:
				TargetUrl = "Architecture.WinApp.Steel.Sale.SaleInvoiceDirect_St";
				screenName = "SaleInvoiceDirect_St";
				ReadById = "ReadById";
				break;
			}
			if (string.IsNullOrEmpty(TargetUrl) || string.IsNullOrEmpty(screenName) || string.IsNullOrEmpty(ReadById))
			{
				return;
			}
			Type typ = Type.GetType(TargetUrl);
			if (typ != null)
			{
				if (Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount) is Form form)
				{
					form.WindowState = FormWindowState.Maximized;
					form.Tag = Conversion.ToString((object)screenName);
					form.Show();
					MethodInfo method = typ.GetMethod(ReadById, new Type[1] { typeof(int) });
					if (method != null)
					{
						object[] parameters = new object[1] { (Id > 0) ? Id : DocumentTypeIdSrNo };
						method.Invoke(form, parameters);
					}
					else
					{
						MessageBox.Show("Method SomeMethod not found on form " + typ.FullName);
					}
				}
				return;
			}
			throw new Exception("Target URL not Configured properly... " + TargetUrl);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ProductionJobOrderSlipA(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Reporting val = new Reporting();
			val.RptPerameter("@OrganizationId", clsGlobalVariables.UserAccount.OrganizationId.ToString());
			val.RptPerameter("@CompanyId", clsGlobalVariables.UserAccount.CompanyId.ToString());
			val.RptPerameter("@hId", PrintId.ToString());
			val.ShowReport("RptInvProductionJobOrderSlipA.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void InvStockConversionPackingMaterial665(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvFoodProductionReports.StockConversionPackingMaterial_Summery_Rpt(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count > 0)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "665-InvStockConversionPackingMaterial-Summery.rpt");
			}
			else
			{
				MessageBox.Show("No Record Found For Display");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PrintJobOrder620(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvProductionJobOrder.GetPrintSlipAndReport(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvProductionJobOrder.JobOrderPlantAndScheduleSubReport(obj.Id), "ProductionJobOrder_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "620-ProductionJobOrderSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ProductionJobOrderPartyProcessing_626(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvProductionJobOrderPartyProcessing.ProductionJobOrderPartyProcessing_SlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 120,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "626-ProductionJobOrderPartyProcessing.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PrintJobOrder630(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 401;
			obj.Id = PrintId;
			DataTable dt = InvProductionJobOrder.JobOrderPrintNew(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvProductionJobOrder.JobOrderInPutSubReport(obj.Id), "ProductionJobOrderInputSubReport.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(InvProductionJobOrder.JobOrderOutPutSubReport(obj.Id), "ProductionJobOrderOutputSubReport.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject3 = new SubReportObject(InvProductionJobOrder.JobOrderOverHeadSubReport(obj.Id), "ProductionJobOrderOverHeadsSubReport.rpt");
			lstSubReports.Add(subReportObject3);
			SubReportObject subReportObject4 = new SubReportObject(InvProductionJobOrder.JobOrderPackingMaterialSubReport(obj.Id), "ProductionJobOrderPackingMaterialsSubReport.rpt");
			lstSubReports.Add(subReportObject4);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "630-PreCostProductionJobOrderSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ProductionJobOrderSlipSteel1563(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1511;
			obj.Id = PrintId;
			DataTable dt = InvProductionJobOrder.JobOrderPrintNew(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvProductionJobOrder.JobOrderInPutSubReport(obj.Id), "ProductionJobOrderInputSubReport.rpt");
			lstSubReports.Add(subReportObject);
			SubReportObject subReportObject2 = new SubReportObject(InvProductionJobOrder.JobOrderOutPutSubReport(obj.Id), "ProductionJobOrderOutputSubReport.rpt");
			lstSubReports.Add(subReportObject2);
			SubReportObject subReportObject3 = new SubReportObject(InvProductionJobOrder.JobOrderOverHeadSubReport(obj.Id), "ProductionJobOrderOverHeadsSubReport.rpt");
			lstSubReports.Add(subReportObject3);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1563-ProductionJobOrderSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetInvoicesDataForShipmentAnalysis(int OrderId, int SupplierCustomerId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return ExImInvoice.GetInvoicesDataForShipmentAnalysis(new ExImInvoice
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				SupplierCustomerId = SupplierCustomerId,
				LcOrderNoId = OrderId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ContractSchedulesDataForShipmentAnalysis(int OrderId, int SupplierCustomerId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			return ExImInvoice.GetContractSchedulesDataForShipmentAnalysis(new ExImInvoice
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				SupplierCustomerId = SupplierCustomerId,
				LcOrderNoId = OrderId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SaleOrderSlip_1511(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = SaleOrder.InvSaleOrderSlipRegisterForSteel(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				ApprovedFilter = "All"
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1511-SaleOrderSlipAndRegister.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GrnSlipSteelWithSubReports(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvGrn.InvGrnSlip1503(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject1 = new SubReportObject(InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 1501,
				RefRefDocIdNo = PrintId
			}), "InvContractorWagesSubReport.rpt");
			lstSubReports.Add(subReportObject1);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1503-InvGrnSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PurchaseInvoiceSteelSlip_1508A(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvPurchaseInvoice.InvPurchaseInvoiceSlipReport(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.InvPurchaseInvoiceSlipReport1508ASupReprt(obj.Id), "PurchaseInvoicePartyBillOthersAddLessSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1508A-InvPurchaseInvoicePartySlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PurchaseInvoiceSteelSlip_1508(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvPurchaseInvoice.InvPurchaseInvoiceSlipReport(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.InvPurchaseInvoiceSlipReport1508SupReprt(obj.Id), "PurchaseInvoiceItemOthersAddLessSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1508-PurchaseBillSupplierRiceItemSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PurchaseInvoiceSteelSlip_1510(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvPurchaseInvoice.InvPurchaseInvoiceDirectSlipReport(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.InvPurchaseInvoiceSlipReport1508SupReprt(obj.Id), "PurchaseInvoiceItemOthersAddLessSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1510-PurchaseInvoiceDirectPartySlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void PurchaseInvoiceSteelSlip_1510A(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvPurchaseInvoice.InvPurchaseInvoiceDirectSlipReport(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoice.InvPurchaseInvoiceSlipReport1508ASupReprt(obj.Id), "PurchaseInvoicePartyBillOthersAddLessSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1510A-PurchaseInvoiceDirectPartySlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static int DeliveryOrderGenerateCodeForSteel(int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			return InvDeliveryOrder.GenerateCodeForSteel(new InvDeliveryOrder
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeliveryOrderSlipForSteel(int PrintId, int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvDeliveryOrder.DeliveryOrderSlipForSteel(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1513-DeliveryOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SaleInvoiceSteelSlip_1514(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.SaleInvoiceSlipReport(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoice.SaleInvoiceSlipReport1514SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1514-InvRepSaleBillCustomer.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleInvoiceSteelSlip_1514A(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.SaleInvoiceSlipReport(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoice.SaleInvoiceSlipReport1514SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1514A-SaleBillCustomer-Format-II.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleInvoiceDirectSteelSlip_1516(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.SaleInvoiceDirectSlipReport(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoice.SaleInvoiceSlipReport1514SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1516-SaleInvoiceCustomerBill.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void SaleInvoiceDirectSteelSlip_1516A(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvSaleInvoice.SaleInvoiceDirectSlipReport(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoice.SaleInvoiceSlipReport1514SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1516A-SaleBillDirectSupplierBill.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void InvGdnSlipSteel(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = InvGdn.InvGdnSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1515-InvRptGdnSlipAndRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public static void ProductionSlip_1818(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ProductionHeader.ProductionSaltSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1818-ProductionSalt-Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceDirectSalt1809(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.SaleInvoiceDirectSlip_ForSalt(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1809-SaleInvoiceDirectSlip_ForSalt.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceDirectSalt1809A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoiceReports.SaleInvoiceDirectSlip_ForSalt(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoiceReports.SalesCustomerBillSubReport(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1809A-SaleInvoiceDirectSlip_ForSalt.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceDirectSlipSalt1804(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1804;
			obj.Id = PrintId;
			DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoiceDirectForSaltSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "InvRptPurchaseBillSupplierOthers.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1804-InvRepPurchaseBillDirectWithoutPo.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PurchaseInvoiceDirectSlipSalt1804A(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1804;
			obj.Id = PrintId;
			DataTable dt = InvPurchaseInvoiceReports.PurchaseInvoiceDirectForSaltSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt(obj.Id), "SupplierBillReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1804A-InvRepPurchaseBillDirectWithoutPo.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static byte[] GenerateQRCodeBytesReturn(string QrCode)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string Code = clsGlobalVariables.UserAccount.OrganizationId.ToString() + clsGlobalVariables.UserAccount.CompanyId + clsGlobalVariables.ActiveYr.Id + QrCode;
			Bitmap bitMap = new QRCode(new QRCodeGenerator().CreateQrCode(Code, (ECCLevel)2, false, false, (EciMode)0, -1)).GetGraphic(5);
			using MemoryStream ms = new MemoryStream();
			bitMap.Save(ms, ImageFormat.Bmp);
			return ms.ToArray();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetVoucherDateForDaybookDropdown(int DocumentTypeId)
	{
		try
		{
			int organizationId = clsGlobalVariables.UserAccount.OrganizationId;
			int CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			return VoucherHead.GetVouchersDateForDaybookDropdown(organizationId, CompanyId, DocumentTypeId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ExportContractScheduleSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataSet ds = ExportContractSchedule.ExportContractScheduleSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				RecordIdentityId = PrintId,
				DocumentTypeId = 247,
				ApprovedFilter = "All"
			});
			DataTable dt = ds.Tables[0].Copy();
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			DatatableHelper.ReplaceCompLogoColumn(dt, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, ".rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Rightsobjects SetRightsValueInRightsObject(string sreenName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			tblUserRights tblUser = new tblUserRights();
			tblUser.UserId = clsGlobalVariables.UserAccount.ID;
			tblUser.ScreenName = sreenName;
			tblUser.RightName = clsGlobalVariables.UserAccount.RoleName;
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(tblUser);
			Rightsobjects obj = new Rightsobjects();
			obj.DoHaveFieldChooserRights = true;
			obj.DoHaveSaveLayoutRights = true;
			obj.DoHaveGridGroupCollapseRights = true;
			obj.DoHaveGridGroupExpandRights = true;
			if (tblUser.RightName == "Admin")
			{
				obj.DoHaveSaveRight = true;
				obj.DoHaveUpdateRights = true;
				obj.DoHaveCanDelete = true;
				obj.DoHaveCanViewAllRecordRights = true;
				obj.DoHavePrintRights = true;
				obj.DoHaveReportExportRights = true;
				obj.DoHaveReportPrintRights = true;
				obj.CanChangeOrderStatusToComplete = true;
				obj.CanChangeOrderStatusToOpen = true;
				obj.CanChangeOrderStatusToCancel = true;
				obj.CanChangeOrderExpiryDate = true;
				obj.RateandAmountFieldAccessDenied = false;
			}
			for (int i = 0; i < lstRights.Count; i++)
			{
				if (lstRights[i].RightName == "View")
				{
					obj.DoHaveViewRight = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "Save")
				{
					if (tblUser.RightName == "Admin")
					{
						obj.DoHaveSaveRight = true;
					}
					else
					{
						obj.DoHaveSaveRight = lstRights[i].Value;
					}
				}
				else if (lstRights[i].RightName == "Update")
				{
					if (tblUser.RightName == "Admin")
					{
						obj.DoHaveUpdateRights = true;
					}
					else
					{
						obj.DoHaveUpdateRights = lstRights[i].Value;
					}
				}
				else if (lstRights[i].RightName == "Print")
				{
					if (tblUser.RightName == "Admin")
					{
						obj.DoHavePrintRights = true;
					}
					else
					{
						obj.DoHavePrintRights = lstRights[i].Value;
					}
				}
				else if (lstRights[i].RightName == "CanView AllRecord")
				{
					if (tblUser.RightName == "Admin")
					{
						obj.DoHaveCanViewAllRecordRights = true;
					}
					else
					{
						obj.DoHaveCanViewAllRecordRights = lstRights[i].Value;
					}
				}
				else if (lstRights[i].RightName == "Grid Print")
				{
					obj.DoHaveGridPrintRights = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "Grid Export")
				{
					obj.DoHaveGridExportRights = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "Delete")
				{
					obj.DoHaveCanDelete = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "Approve")
				{
					obj.DoHaveApproveRights = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "Rate")
				{
					obj.DoHaveCanRateandAmount = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "RateandAmountFieldAccessDenied")
				{
					if (tblUser.RightName == "Admin")
					{
						obj.RateandAmountFieldAccessDenied = false;
					}
					else
					{
						obj.RateandAmountFieldAccessDenied = lstRights[i].Value;
					}
				}
				else if (lstRights[i].RightName == "ProductionSettlement")
				{
					obj.DoHaveCanSettlementtab = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "CanChangeOrderStatusToComplete" && tblUser.RightName != "Admin")
				{
					obj.CanChangeOrderStatusToComplete = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "CanChangeOrderStatusToOpen" && tblUser.RightName != "Admin")
				{
					obj.CanChangeOrderStatusToOpen = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "CanChangeOrderStatusToCancel" && tblUser.RightName != "Admin")
				{
					obj.CanChangeOrderStatusToCancel = lstRights[i].Value;
				}
				else if (lstRights[i].RightName == "CanChangeOrderExpiryDate" && tblUser.RightName != "Admin")
				{
					obj.CanChangeOrderExpiryDate = lstRights[i].Value;
				}
			}
			return obj;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<string> GetMissingOrRenamedColumns(DataTable dt, XmlDocument xmlDoc, string filePath)
	{
		XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//Columns/*");
		HashSet<string> xmlColumnIDs = new HashSet<string>();
		Dictionary<string, string> xmlColumnDataMembers = new Dictionary<string, string>();
		foreach (XmlNode item in xmlNodeList)
		{
			string id = item.Attributes["ID"]?.InnerText;
			string dataMember = item["DataMember"]?.InnerText;
			if (!string.IsNullOrEmpty(id))
			{
				xmlColumnIDs.Add(id);
				if (!string.IsNullOrEmpty(dataMember))
				{
					xmlColumnDataMembers[id] = dataMember;
				}
			}
		}
		List<string> missingOrRenamedColumns = new List<string>();
		foreach (DataColumn column in dt.Columns)
		{
			if (xmlColumnIDs.Contains(column.ColumnName))
			{
				continue;
			}
			if (xmlColumnDataMembers.Values.Contains(column.ColumnName))
			{
				string oldColumnName = xmlColumnDataMembers.First((KeyValuePair<string, string> x) => x.Value == column.ColumnName).Key;
				RenameColumnInLayout(xmlDoc, oldColumnName, column.ColumnName, filePath);
			}
			else
			{
				missingOrRenamedColumns.Add(column.ColumnName);
			}
		}
		return missingOrRenamedColumns;
	}

	private static void RenameColumnInLayout(XmlDocument xmlDoc, string oldName, string newName, string filePath)
	{
		XmlNode columnNode = xmlDoc.SelectSingleNode("//Columns/*[@ID='" + oldName + "']");
		if (columnNode != null)
		{
			columnNode.Attributes["ID"].Value = newName;
			XmlNode dataMemberNode = columnNode.SelectSingleNode("DataMember");
			if (dataMemberNode != null)
			{
				dataMemberNode.InnerText = newName;
			}
			xmlDoc.Save(filePath);
		}
	}

	public static void RemoveAllColumns(XmlDocument xmlDoc, string filePath)
	{
		XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//Columns/*");
		List<XmlNode> nodesToRemove = new List<XmlNode>();
		foreach (XmlNode node2 in xmlNodeList)
		{
			nodesToRemove.Add(node2);
		}
		foreach (XmlNode node in nodesToRemove)
		{
			node.ParentNode.RemoveChild(node);
		}
		xmlDoc.Save(filePath);
	}

	public static void RemoveExtraColumns(XmlDocument xmlDoc, DataTable dt, string filePath)
	{
		XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//Columns/*");
		List<XmlNode> nodesToRemove = new List<XmlNode>();
		foreach (XmlNode node2 in xmlNodeList)
		{
			string id = node2.Attributes["ID"]?.InnerText;
			if (!string.IsNullOrEmpty(id) && !dt.Columns.Contains(id))
			{
				nodesToRemove.Add(node2);
			}
		}
		foreach (XmlNode node in nodesToRemove)
		{
			node.ParentNode.RemoveChild(node);
		}
		xmlDoc.Save(filePath);
	}

	public static DataTable getItemIdsByDate(int OrganizationId, int CompanyId, DateTime DocDate, string Activity)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = DocDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = Activity
			});
			return GenericProvider.GetDataTableProc<InvSaleInvoice>("[dbo].[usp_getItemIdsByDate]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void CustomerDiscountPolicySlip(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("No Record Found For Display");
			}
			DataTable dt = CustomerDiscountPolicy.CustomerDiscountPolicySlip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1110_CustomerDiscountPolicySlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ContractorWagesBillSlipConcrete(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ContractorWagesBill.ContractorWagesBill_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId,
				DocumentTypeId = 1868
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1868_ContractorWagesBillSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ContractorWagesBillSlipConcrete_1869(int PrintId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("RecordId Not Found");
			}
			DataTable dt = ContractorWagesBill.ContractorWagesBill_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId,
				DocumentTypeId = 1869
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1869_ContractorWagesBillSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SaleOrderSlipConcrete(int PrintId, int DocumentTypeId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = SaleOrder.SaleOrderSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = DocumentTypeId,
				ApprovedFilter = "All"
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("No Record Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1852-InvRptSaleOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PurchaseOrderSlipConcrete(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = PurchaseOrder.PurchaseOrder_Slip(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1863-InvRptPurchaseOrderSlip.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GrnAgainstOrderConcrete1858(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvGrnandGdnReports.InvGrnSlip211(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 1858,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1858-GoodsReceiptsNotes_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceDirectConcreteCustomerSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoice.SaleInvoiceDirectSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoice.SalesCustomerBillSubReport(obj.Id), "SaleInvoice_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1856-SaleInvoiceDirect_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceReturnConcreteCustomerSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoice.SaleInvoiceDirectSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoice.SalesCustomerBillSubReport(obj.Id), "SaleInvoice_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1862-SaleInvoiceRetrurn_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceDirectConcreteItemSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoice.SaleInvoiceDirectSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoice.SaleInvoiceItemExpenseSubReport(obj.Id), "SaleInvoice_ItemExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1856A-SaleInvoiceDirect_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void DeliveryOrderConcreteSlip(int PrintId)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = InvDeliveryOrder.DeliveryOrder_SlipConcrete(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, PrintId);
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("No Record Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1853-DeliveryOrderSlip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeliveryOrderConcreteSlipCustomerWise(int PrintId)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = InvDeliveryOrder.DeliveryOrder_SlipConcrete(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, PrintId);
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("No Record Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1853A-DeliveryOrderSlip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void GrnConcreteSlip(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				DataTable dt = InvGrn.GrnSlipAndRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					Id = PrintId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1864_GoodsRecieptNotesFinish.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ProductionSlipConcrete(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = ProductionHeader.Production_SlipAndRegister(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ProductionHeader.ProductionAllDetails_SubReport(obj.Id), "ProductionAllDetails_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1850_Production_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ConversionSlipConcrete(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = ConversionHeader.Conversion_SlipAndRegister(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ConversionHeader.ConversionAllDetails_SubReport(obj.Id), "ConversionAllDetails_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1867_Conversion_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ProductionSlipFeedMill(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = PrintId;
				DataTable dt = ProductionHeader.Production_SlipAndRegister(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(ProductionHeader.ProductionAllDetails_SubReport(obj.Id), "ProductionAllDetails_SubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "1121_Production_Slip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void ProductionManualSlipFeedMill(int PrintId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
				obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
				obj.Id = PrintId;
				obj.ApprovedFilter = "All";
				DataTable dt = ProductionHeader.Production_SlipAndRegister(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject = new SubReportObject(ProductionHeader.ProductionAllDetails_SubReport(obj.Id), "ProductionAllDetails_SubReport.rpt");
				lstSubReports.Add(subReportObject);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "1122_Production_Slip.rpt", lstSubReports);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceConcreteCustomerSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoice.SaleInvoiceSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoice.SalesCustomerBillSubReport(obj.Id), "SaleInvoice_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1861-SaleInvoice_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void SaleInvoiceConcreteItemSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = InvSaleInvoice.SaleInvoiceSlip(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvSaleInvoice.SaleInvoiceItemExpenseSubReport(obj.Id), "SaleInvoice_ItemExpenseSubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1861A-SaleInvoice_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PrintSelectedCheque(string Ids, string Template)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Ids == "" || Template == "")
			{
				throw new Exception("Id or Template Not Found");
			}
			DataTable dt = ChequePrintList.DataForPrnt(Ids);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Print");
			}
			else
			{
				new Reporting().ReportDirectPrint(dt, Template + ".rpt", (List<SubReportObject>)null, (List<ReportPerameter>)null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateChequePrintStatus(string Ids)
	{
		try
		{
			if (Ids == "")
			{
				throw new Exception("Id  Not Found");
			}
			ChequePrintList.UpdateChequePrintStatus(Ids);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static ServerCredentials GetServerCredentials()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ServerCredentials obj = new ServerCredentials();
		try
		{
			bool CrystalFTPAllow = Conversion.ToBool((object)GetConfigurationByOrgCompandConfigDescription("CrystalAllowFromFTPServer"));
			if (Conversion.ToBool((object)GetConfigurationByOrgCompandConfigDescription("IsVpsAttachmentsServiceOn")) || CrystalFTPAllow)
			{
				string ServerIp = ConfigurationManager.ConnectionStrings["VPSName"].ConnectionString;
				string VPSUser = ConfigurationManager.ConnectionStrings["VPSUser"].ConnectionString;
				string VPSPwd = ConfigurationManager.ConnectionStrings["VPSPwd"].ConnectionString;
				if (string.IsNullOrEmpty(ServerIp))
				{
					throw new InvalidOperationException("The server IP configuration is not set.");
				}
				obj.ServerIp = Encryption.DecryptString(ServerIp);
				if (string.IsNullOrEmpty(VPSUser))
				{
					throw new InvalidOperationException("The VPS user configuration is not set.");
				}
				obj.UserName = Encryption.DecryptString(VPSUser);
				if (string.IsNullOrEmpty(VPSPwd))
				{
					throw new InvalidOperationException("The VPS password configuration is not set.");
				}
				obj.Password = Encryption.DecryptString(VPSPwd);
			}
		}
		catch (Exception ex)
		{
			throw new ApplicationException("Error while getting server credentials", ex);
		}
		return obj;
	}

	public static Bitmap MakeCircularImage(Bitmap originalImage)
	{
		try
		{
			int size = Math.Min(originalImage.Width, originalImage.Height);
			Bitmap circularBitmap = new Bitmap(size, size);
			using (Graphics g = Graphics.FromImage(circularBitmap))
			{
				g.Clear(Color.Transparent);
				using GraphicsPath path = new GraphicsPath();
				path.AddEllipse(0, 0, size, size);
				g.SetClip(path);
				g.DrawImage(originalImage, 0, 0, size, size);
			}
			return circularBitmap;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Bitmap ResizeImage(Bitmap originalImage, int width, int height)
	{
		try
		{
			Bitmap resizedBitmap = new Bitmap(width, height);
			using (Graphics g = Graphics.FromImage(resizedBitmap))
			{
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.DrawImage(originalImage, 0, 0, width, height);
			}
			return resizedBitmap;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void OutwardGatePassReturnable_Slip1201(int GatePassId)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (GatePassId > 0)
			{
				DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					Id = GatePassId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1201_OutwardGatePassReturnable_Slip.rpt");
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}
}
