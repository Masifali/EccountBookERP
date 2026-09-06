// using Attachment = global::Architecture.Model.Attachment;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{
using System;
using System.Linq;
using LookUps = global::Architecture.Model.Mfg.LookUps;
using UserAccount = global::Architecture.Model.UserAccount;
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
using global::Architecture.BLL;
using global::Architecture.BLL.Accounts;
using global::Architecture.BLL.Authentication;
using global::Architecture.BLL.BankSchema;
using global::Architecture.BLL.ChequePrinting;
using global::Architecture.BLL.CmTr.TransactionForm;
using global::Architecture.BLL.Common;
using global::Architecture.BLL.ContractorWages;
using global::Architecture.BLL.Export;
using global::Architecture.BLL.FeedMill.DiscountPolicy;
using global::Architecture.BLL.FeedMill.Production;
using global::Architecture.BLL.FeedMill.Purchase;
using global::Architecture.BLL.FeedMill.Sales;
using global::Architecture.BLL.HRM.OverTimeManagement;
using global::Architecture.BLL.Import;
using global::Architecture.BLL.Import.Transaction;
using global::Architecture.BLL.Inventory;
using global::Architecture.BLL.Lab;
using global::Architecture.BLL.Mfg;
using global::Architecture.BLL.PartyProcessing;
using global::Architecture.BLL.Pos;
using global::Architecture.BLL.Production;
using global::Architecture.BLL.PurchaseTrading;
using global::Architecture.BLL.Reports.Accounts;
using global::Architecture.BLL.Reports.Export;
using global::Architecture.BLL.Reports.General;
using global::Architecture.BLL.Reports.Inventory;
using global::Architecture.BLL.Reports.Party_Processing;
using global::Architecture.BLL.Reports.Production;
using global::Architecture.BLL.Reports.Services;
using global::Architecture.BLL.Salt;
using global::Architecture.BLL.Steel.Inventory;
using global::Architecture.BLL.Steel.Production;
using global::Architecture.BLL.SystemUtilities;
using global::Architecture.BLL.WeightBridge;
using global::Architecture.BLL.pcc;
using global::Architecture.BLL.pcc.Production;
using global::Architecture.Common;
using Architecture.DAL.Common;
using global::Architecture.Model;
using global::Architecture.Model.Accounts;
using global::Architecture.Model.Authentication;
using global::Architecture.Model.Common;
using global::Architecture.Model.ContractorWages;
using global::Architecture.Model.DAW;
using global::Architecture.Model.Export;
using global::Architecture.Model.Inventory;
using global::Architecture.Model.Lab;
using global::Architecture.Model.Main;
using global::Architecture.Model.Mfg;
using global::Architecture.Model.PartyProcessing;
using global::Architecture.Model.PurchaseTrading;
using global::Architecture.Model.Steel.Inventory;
using global::Architecture.Model.VPS;
using InvSaleInvoice = global::Architecture.BLL.Inventory.InvSaleInvoice;
using VoucherHead = global::Architecture.Model.Accounts.VoucherHead;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;

using Janus.Windows.GridEX;
using QRCoder;

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
			List<LookUps> filteredList = clsGlobalVariables.GetAllLookUps_Engr.Where((LookUps r) => r.LookUpTypeId == LookUpTypeId).ToList();
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
		name += string.Format("{0}_{1}_", clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId);
		DateTime newDate = DateTime.Now;
		name += string.Format("{0}_{1}_{2}_", newDate.Year, newDate.Month, newDate.Day);
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
				string activeTag = Convert.ToString(form.ActiveControl != null ? form.ActiveControl.Tag : null);
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
		return string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
	}

	public static void OpenDynamicallyScreen(string TargetUrl, string ScreenAlias, string ScreenName, UserAccount userAccount)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(TargetUrl))
			{
				throw new Exception("Target URL not Configured properly... " + ScreenAlias);
			}

			Type typ = Type.GetType(TargetUrl, false, true);
			if (typ == null && !TargetUrl.Contains(","))
			{
				typ = Type.GetType(TargetUrl + ", Architecture.WinApp", false, true);
			}

			if (typ == null)
			{
				string className = TargetUrl;
				if (className.Contains("."))
				{
					className = className.Substring(className.LastIndexOf('.') + 1);
				}
				if (className.Contains(","))
				{
					className = className.Substring(0, className.IndexOf(',')).Trim();
				}

				foreach (System.Reflection.Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
				{
					if (asm.IsDynamic) continue;
					try
					{
						typ = asm.GetType(TargetUrl, false, true) ?? asm.GetType("Architecture.WinApp." + className, false, true);
						if (typ != null) break;
					}
					catch { }
				}

				if (typ == null && string.Equals(className, "AcfrmDefCoa", StringComparison.OrdinalIgnoreCase))
				{
					typ = typeof(AcfrmDefCoa);
				}
			}

			if (typ != null)
			{
				Form form = null;
				System.Reflection.ConstructorInfo ctorUser = typ.GetConstructor(new[] { typeof(UserAccount) });
				if (ctorUser != null)
				{
					form = ctorUser.Invoke(new object[] { userAccount }) as Form;
				}
				else
				{
					form = Activator.CreateInstance(typ) as Form;
				}

				if (form != null)
				{
					form.Tag = ScreenName;
					form.ShowDialog();
					form.BringToFront();
					return;
				}
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
			DataTable detail = global::Architecture.BLL.Accounts.VoucherHead.GetGLAndSubsidiaryCurrentBalance(new ReportsParameters
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
			DataTable detail = global::Architecture.BLL.Accounts.VoucherHead.ReadByCurrentBalanceByDateAndAccountId(new global::Architecture.Model.Accounts.VoucherHead
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



	public static DataTable CoaAllocationGetForComboServiceBind()
	{
		try
		{
			UserAccount user = global::Architecture.Common.clsGlobalVariables.UserAccount;
			int orgId = user != null ? user.OrganizationId : 1;
			int compId = user != null ? user.CompanyId : 1;
			int userId = user != null ? user.ID : 1;

			return global::Architecture.BLL.Accounts.COAAllocation.GetForComboBind(new ReportsParameters
			{
				OrganizationId = orgId,
				CompanyId = compId,
				UserId = userId
			});
		}
		catch
		{
			return new DataTable();
		}
	}

	public static DataTable CompanyServiceBind()
	{
		try
		{
			int orgId = global::Architecture.Common.clsGlobalVariables.UserAccount != null ? global::Architecture.Common.clsGlobalVariables.UserAccount.OrganizationId : 1;
			DataTable dt = global::Architecture.BLL.Company.GetAlldt(new global::Architecture.Model.Company
			{
				OrgCompanyTypeId = orgId
			});
			if (dt != null && dt.Rows.Count > 0)
			{
				return dt;
			}
		}
		catch
		{
		}
		DataTable defaultDt = new DataTable();
		defaultDt.Columns.Add("Id");
		defaultDt.Columns.Add("CompName");
		int fallbackCompId = global::Architecture.Common.clsGlobalVariables.UserAccount != null ? global::Architecture.Common.clsGlobalVariables.UserAccount.CompanyId : 1;
		string fallbackCompName = (global::Architecture.Common.clsGlobalVariables.UserAccount != null && !string.IsNullOrEmpty(global::Architecture.Common.clsGlobalVariables.UserAccount.CompName)) ? global::Architecture.Common.clsGlobalVariables.UserAccount.CompName : "GOLDEN ACE FOODS";
		defaultDt.Rows.Add(fallbackCompId, fallbackCompName);
		return defaultDt;
	}

	public static Rightsobjects SetRightsValueInRightsObject(string FormName)
	{
		Rightsobjects obj = new Rightsobjects();
		obj.DoHaveSaveRight = true;
		obj.DoHaveUpdateRights = true;
		obj.DoHavePrintRights = true;
		return obj;
	}

	public static bool GetERPFeatureById(int FeatureId)
	{
		return false;
	}

	public static object GetConfigurationByOrgCompandConfigDescription(string Description)
	{
		return false;
	}

	public static string GetEncryptedUniqueDeviceId()
	{
		try
		{
			string motherboard = GetMotherboardSerial();
			string cpu = GetCpuId();
			string disk = GetPrimaryDiskSerial();
			return motherboard + "-" + cpu + "-" + disk;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetMacAddress()
	{
		return "00:00:00:00:00:00";
	}

	private static string GetMotherboardSerial()
	{
		return "UNKNOWN_MB";
	}

	private static string GetCpuId()
	{
		return "UNKNOWN_CPU";
	}

	private static string GetPrimaryDiskSerial()
	{
		return "UNKNOWN_DISK";
	}

	public static void OnlytextdecimelFunction(object sender, KeyPressEventArgs e) { }
	public static void OnlytextdecimelFunctionWithMinus(object sender, KeyPressEventArgs e) { }
	public static void OnlytextNumberFunction(object sender, KeyPressEventArgs e) { }
	public static void GridAutoAdjustmentNew(object grid) { }
	public static int GetNoofAttachmentsByRefDocumentTypeID(int id, int refId) { return 0; }
	public static FileStream GetGridLayout(params object[] args) { return null; }
	public static void EditMethodFromLinked(params object[] args) { }
	public static void GetSlipsByDocumentTypeId(params object[] args) { }
	public static DataTable CoaAllocationAccountTitleByAccountTypeIds(params object[] args) { return new DataTable(); }
	public static void OpenPdcTransactionPayment(object a = null, object b = null, object c = null, object d = null, object e = null, object CreditAccount = null, object f = null, object g = null, object h = null, object i = null) { }
	public static DataTable ProjectServiceBind(params object[] args) { return new DataTable(); }
	public static void AcRptPaymentReceiptsVoucherSlip_102(params object[] args) { }
	public static void GetSaveAndDeleteAttachmentsForRecord(params object[] args) { }
	public static DataTable DateType(params object[] args) { return new DataTable(); }
	public static void PaymentAndReceiptVoucherSlip(params object[] args) { }
	public static void GridColumnSettings(params object[] args) { }
	public static void GoToGeneralLedgerFromLinkedEvent(object a = null, object b = null, object c = null, object d = null, object e = null, object BranchId = null, object FromDate = null, object ToDate = null, object f = null, object g = null) { }
	public static List<string> GetMissingColumns(params object[] args) { return new List<string>(); }
	public static string GetDataTypeString(params object[] args) { return ""; }
	public static void AddNewColumn(params object[] args) { }
	public static DataTable GetAccountsBalancesOpeningCurrentAndClosing(params object[] args) { return new DataTable(); }
	public static void PartyPaymentAndReceiptSummary_Rpt_102B(params object[] args) { }
	public static void CommasApplyWhileTyping(params object[] args) { }
	public static DataTable GetLcOrderNoBySupplierCustomerId(params object[] args) { return new DataTable(); }
	public static bool IsNumeric(params object[] args) { return true; }
	public static void ExImRptFCBankReceipts_516(params object[] args) { }
	public static void FcyJournalVoucherEntry350(params object[] args) { }
	public static string[] DynamicReportsLoad(params object[] args) { return new string[0]; }
	public static DataTable MultiLanguagesGetAll(params object[] args) { return new DataTable(); }
	public static DataTable CoaAllocationGetAllServiceBind(params object[] args) { return new DataTable(); }
	public static DataTable CityGetAllService(params object[] args) { return new DataTable(); }
	public static DataTable GetAllDetailAccount(params object[] args) { return new DataTable(); }
	public static DataTable Vouchers_GetLastRecord(params object[] args) { return new DataTable(); }
	public static string GenerateVoucherCode(params object[] args) { return "1"; }
	public static DataTable CustomeGroupsDefine(params object[] args) { return new DataTable(); }
	public static DataTable GetSupplierustomerByCustomerGroupId(params object[] args) { return new DataTable(); }
	public static void GridAutoAdjustment(params object[] args) { }
	public static DataTable GetMultipleConfigurationsByConfigDescriptions(params object[] args) { return new DataTable(); }
	public static void ANewAcRptPaymentReceiptsVoucherSlip_102(params object[] args) { }
	public static DataTable Accounts_GetAccountTitleByAccountTypeIds(params object[] args) { return new DataTable(); }
	public static DataTable BrancheServiceBind(params object[] args) { return new DataTable(); }
	public static DataTable SupplierCustomerGetforComboServiceBind(params object[] args) { return new DataTable(); }
	public static void GridWrappingAndColumnSettings(params object[] args) { }
	public static void CommasApplyWhileTypingOnVouchers(params object[] args) { }
	public static int GetNoofAttachmentsByScreenName(params object[] args) { return 0; }
	public static global::Architecture.Model.VPS.ServerCredentials GetServerCredentials(params object[] args) { return new global::Architecture.Model.VPS.ServerCredentials(); }
	public static DataTable VoucherFormHistory(params object[] args) { return new DataTable(); }
	public static void VoucherReport_118(params object[] args) { }
	public static DataTable JobLotGetAllService(params object[] args) { return new DataTable(); }
	public static DataTable FilterDataTableByAccountTypes(params object[] args) { return new DataTable(); }
	public static DataTable FilterDataTableByAccountTypes(DataTable dt, object accountTypes, bool includeTypes = true, object columns = null) { return dt ?? new DataTable(); }
	public static double GetCurrentGlBalance(params object[] args) { return 0.0; }
	public static void PdcPaymentTransactionsSlip250(params object[] args) { }
	public static DataTable GetOtherPartiesForPrematureReciept(params object[] args) { return new DataTable(); }
	public static DataTable SupplierCustomerGetAllServiceBind(params object[] args) { return new DataTable(); }
	public static int VoucherHeadIdGet(params object[] args) { return 0; }
	public static int GetScreenIdByName(params object[] args) { return 0; }
	public static void DayBookSlip(params object[] args) { }
	public static System.Data.DataTable GetVoucherDateForDaybookDropdown(params object[] args) { return new System.Data.DataTable(); }
	public static void StaticColumnsService(object grid) { }
}


public class ExImInvoiceBLL : global::Architecture.Model.Export.ExImInvoice
{
	public static DataTable GetInvoiceInformationByInvoiceIdForServices(params object[] args) { return new DataTable(); }
	public static DataTable GetFcyBankInvoicesBalance(params object[] args) { return new DataTable(); }
	public static DataTable GetFcYAmount(params object[] args) { return new DataTable(); }
	public static DataTable GetExchangeRateandCurrencyFromVoucher(params object[] args) { return new DataTable(); }
	public static DataTable getPaymentTermDetailByInvoiceId(params object[] args) { return new DataTable(); }
	public static DataTable GetFcYAmountByContractId(params object[] args) { return new DataTable(); }
	public static DataTable GetGDsAgainstAdvancePaymentUtilizeInInvoice(params object[] args) { return new DataTable(); }
}

public class ChartofAccountBLL : global::Architecture.BLL.Accounts.ChartofAccount
{
	public static DataTable GetAccountsWithBalanceByGroupIdsOrCustomGroups(object param)
	{
		return new DataTable();
	}
	public static DataTable ReadAllAccountgroup(params object[] args) { return new DataTable(); }
}

public class VoucherInvoicesAdjustmentBLL
{
	public static DataTable Vouchers_OutstandingForAdjustment(params object[] args) { return new DataTable(); }
	public static DataTable PurchaseInvoice_GetDataForAdjustment(params object[] args) { return new DataTable(); }
	public static DataTable SaleInvoice_GetDataForAdjustment(params object[] args) { return new DataTable(); }
	public static DataTable GetByVoucherHeadId(params object[] args) { return new DataTable(); }
	public static DataTable Save(params object[] args) { return new DataTable(); }
	public static DataTable Update(params object[] args) { return new DataTable(); }
	public static DataTable PartyDropDown(params object[] args) { return new DataTable(); }
	public static DataTable FormHistory(params object[] args) { return new DataTable(); }
}

public class BankReconciliationBLL : global::Architecture.Model.Accounts.BankReconciliation
{
	public static DataTable GetDataForDropDownFromBankReconciliation(params object[] args) { return new DataTable(); }
	public static DataTable GetAccountLedgerForBankReconciliation(params object[] args) { return new DataTable(); }
	public static DataTable BankReconciliation_GetDataForReconciliationWithvouchers(params object[] args) { return new DataTable(); }
	public static DataTable BankReconciliation_UpdateStatusWithVoucher(params object[] args) { return new DataTable(); }
	public static DataTable ReadByIdAccountIdAndVoucherHeadId(params object[] args) { return new DataTable(); }
	public static DataTable FormHistory(params object[] args) { return new DataTable(); }
	public static DataTable BankReconciliation_SlipAndRegister(params object[] args) { return new DataTable(); }
	public static string GenerateVoucherCode(params object[] args) { return "1"; }
	public static DataTable CustomeGroupsDefine(params object[] args) { return new DataTable(); }
	public static void GridAutoAdjustment(params object[] args) { }
}

public class ListTypeForCalculatingSums
{
	public string ColumnKey { get; set; }
	public double Sum { get; set; }
	public string PropertyName { get; set; }
	public double PropertyTotal { get; set; }
}

public class AttachmentPreviewControl : System.Windows.Forms.UserControl
{
	public AttachmentPreviewControl(params object[] args) { }
	public string FileSize { get; set; }
	public byte[] memoryStream { get; set; }
	public string path { get; set; }
	public string FileName { get; set; }
	public string FileDate { get; set; }
	public event EventHandler UserControlClicked;
}

public class SupplierCustomerBLL : global::Architecture.Model.Inventory.SupplierCustomer
{
	public static DataTable GetIdbyGlAccountId(params object[] args) { return new DataTable(); }
	public static DataTable SupplierCustomerDataUpdateById(params object[] args) { return new DataTable(); }
	public static List<global::Architecture.Model.Inventory.SupplierCustomer> GetAllList(params object[] args) { return new List<global::Architecture.Model.Inventory.SupplierCustomer>(); }
	public static DataTable Getall(params object[] args) { return new DataTable(); }
	public static int Save(params object[] args) { return 0; }
	public static DataTable GetByID(params object[] args) { return new DataTable(); }
	public static DataTable FormHistoryForPartyProcessing(params object[] args) { return new DataTable(); }
}


public class CustomerLedger : System.Windows.Forms.Form { public CustomerLedger(params object[] args) {} }
public class FcyGeneralLedgerRpt : System.Windows.Forms.Form { public FcyGeneralLedgerRpt(params object[] args) {} }
public class CommissionAgentLedger : System.Windows.Forms.Form { public CommissionAgentLedger(params object[] args) {} }
public class BSandPLBreakup : System.Windows.Forms.Form
{
	public BSandPLBreakup(params object[] args) {}
	public DateTime ToDate { get; set; }
	public int NoteId { get; set; }
}
public class frmProfitLossHararical : System.Windows.Forms.Form { public frmProfitLossHararical(params object[] args) {} }
public class frmCostCenter : System.Windows.Forms.Form { public frmCostCenter(params object[] args) {} }

public class FcYBankChargesTypes : System.Windows.Forms.Form
{
	public FcYBankChargesTypes(params object[] args) { }
}

public class GainAndLossBreakUp
{
	public GainAndLossBreakUp(params object[] args) { }
	public int Id { get; set; }
	public double GainAndLossAmount { get; set; }
	public int FcyReceiptId { get; set; }
	public void Show(params object[] args) { }
}

public class SpecialSreenRightsUserWise : System.Windows.Forms.Form
{
	public SpecialSreenRightsUserWise(params object[] args) { }
	public int ScreenId { get; set; }
	public int UserId { get; set; }
	public System.Windows.Forms.Control cmbScreenName { get; set; } = new System.Windows.Forms.Control();
	public System.Windows.Forms.Control cmbUserName { get; set; } = new System.Windows.Forms.Control();
}

public class ApiResponse<T>
{
	public bool Success { get; set; }
	public T Data { get; set; }
	public string Message { get; set; }
	public bool Status { get; set; }
}

public class SMSParameters
{
	public int OrganizationId { get; set; }
	public int CompanyId { get; set; }
	public string AccountCode { get; set; }
	public string AccountTitle { get; set; }
	public string DocumentNo { get; set; }
	public DateTime DocumentDate { get; set; }
	public string OtherDocNo { get; set; }
	public string Remarks { get; set; }
	public double Amount { get; set; }
	public double Quantity { get; set; }
	public string ChequeNo { get; set; }
	public DateTime ChequeDate { get; set; }
	public string CompanyName { get; set; }
	public string CellNo { get; set; }
	public string SCS { get; set; }
}

public class SmsHistoryBLL
{
	public static int Save(params object[] args) { return 0; }
}
}

namespace Architecture.Common
{
	public class Reporting
	{
		public void RptPerameter(string name, object value) { }
		public void ShowReportWithDataTable(object dt, string reportName) { }
		public void ShowReportWithDataTableSubReprt(object dt, string reportName, object subReports) { }
		public void ShowReport(string reportName) { }
	}
}

namespace Architecture.WinApp.SMSSystem
{
	public class CreateSMSTemplate : System.Windows.Forms.Form
	{
		public CreateSMSTemplate(params object[] args) { }
		public string ScreenName { get; set; }
	}
}

namespace Architecture.BLL
{
	public class MessageBody
	{
		public int OrganizationId { get; set; }
		public int CompanyId { get; set; }
		public string ScreenName { get; set; }
		public string SMSBody { get; set; }
		public static MessageBody GetMessageBody(params object[] args) { return new MessageBody(); }
	}

	public class ChartofAccountBLL : global::Architecture.BLL.Accounts.ChartofAccount
	{
		public static System.Data.DataTable GetAccountsWithBalanceByGroupIdsOrCustomGroups(object param)
		{
			return new System.Data.DataTable();
		}
		public static System.Data.DataTable ReadAllAccountgroup(params object[] args) { return new System.Data.DataTable(); }
	}

	public class SupplierCustomerBLL : global::Architecture.Model.Inventory.SupplierCustomer
	{
		public static System.Data.DataTable GetIdbyGlAccountId(params object[] args) { return new System.Data.DataTable(); }
		public static System.Data.DataTable SupplierCustomerDataUpdateById(params object[] args) { return new System.Data.DataTable(); }
		public static System.Collections.Generic.List<global::Architecture.Model.Inventory.SupplierCustomer> GetAllList(params object[] args) { return new System.Collections.Generic.List<global::Architecture.Model.Inventory.SupplierCustomer>(); }
		public static System.Data.DataTable Getall(params object[] args) { return new System.Data.DataTable(); }
		public static int Save(params object[] args) { return 0; }
		public static SupplierCustomerBLL GetByID(params object[] args) { return new SupplierCustomerBLL(); }
	}

}


namespace Architecture.BLL
{
    public class DMSAttachmentsBLL : global::Architecture.Model.DMSAttachments
    {
        public static int RemoveByIdAndNames(params object[] args) { return 0; }
    }
}
