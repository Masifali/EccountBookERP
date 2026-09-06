using System;
using System.Collections.Generic;
using Architecture.Common;
using Architecture.DAL;
using Architecture.Model;

namespace Architecture.BLL;

public class CLV
{
	public static List<Architecture.Model.CLV> GetLicense(int OrganizationId, int CompanyId)
	{
		try
		{
			return new Architecture.DAL.CLV().ReadAll(OrganizationId, CompanyId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveLicense(Architecture.Model.CLV CLV)
	{
		try
		{
			CLV.ConvertedLicenseKey = LicenseKey.ReturnKey(CLV.LicenseKey);
			return new Architecture.DAL.CLV().Insert(CLV);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
