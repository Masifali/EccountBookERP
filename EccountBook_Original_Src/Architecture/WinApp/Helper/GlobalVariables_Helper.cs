using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.Common;
using Architecture.Model.Authentication;

namespace Architecture.WinApp.Helper;

public static class GlobalVariables_Helper
{
	public static string GetConfigValueFromGlobal(string configName)
	{
		ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation?.FirstOrDefault((ConfigrationsAllocation x) => x.ConfigDescription == configName);
		return Conversion.ToString((object)((obj != null) ? obj.ConfigKey : null));
	}

	public static bool GetWagesRefDocumentsStatusById(int documentTypeId)
	{
		WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList?.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == documentTypeId);
		return Conversion.ToBool((object)(obj != null && obj.IsActive));
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
