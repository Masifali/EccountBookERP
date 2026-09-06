using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.Common;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Helper
{

public static class GlobalVariables_Helper
{
	public static string GetConfigValueFromGlobal(string configName)
	{
		ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation?.FirstOrDefault((ConfigrationsAllocation x) => x.ConfigDescription == configName);
		return Conversion.ToString((object)((obj != null) ? obj.ConfigKey : null));
	}

	public static bool GetWagesRefDocumentsStatusById(int documentTypeId)
	{
		if (clsGlobalVariables.WagesRefDocumentsStatusList == null) return false;
		foreach (dynamic row in clsGlobalVariables.WagesRefDocumentsStatusList)
		{
			if (row.RefDocumentTypeId == documentTypeId)
			{
				return (bool)row.IsActive;
			}
		}
		return false;
	}

	public static List<DateTime> GetMonthListByFinancialYEarFromGlobal()
	{
		try
		{
			DateTime start_Period = clsGlobalVariables.ActiveYr.Start_Period;
			DateTime financialEndDate = clsGlobalVariables.ActiveYr.End_Period;
			List<DateTime> monthsList = new List<DateTime>();
			DateTime date = start_Period;
			while (date <= financialEndDate)
			{
				monthsList.Add(date);
				date = date.AddMonths(1);
			}
			return monthsList;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<DateTime> GetMonthListByAllFinancialYEarFromGlobal()
	{
		try
		{
			DateTime firstFinancialStartDate = clsGlobalVariables.FirstFinancialStartDate;
			DateTime financialEndDate = clsGlobalVariables.RunningFinancialEndDate;
			List<DateTime> monthsList = new List<DateTime>();
			DateTime date = firstFinancialStartDate;
			while (date <= financialEndDate)
			{
				monthsList.Add(date);
				date = date.AddMonths(1);
			}
			return monthsList;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}

}
