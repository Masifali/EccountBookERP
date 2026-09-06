using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Architecture.Common;
using Architecture.Model;

namespace Architecture.DAL;

public class CLV : DALBase
{
	private const string PROC_INSERT = "[dbo].[Insert_CLV]";

	private const string PROC_READALL = "[dbo].[GetLicenseKey]";

	private const string ID = "Id";

	private const string COMPANYNAME = "CompanyName";

	private const string LICENSEKEY = "LicenseKey";

	public int Insert(Architecture.Model.CLV CLV)
	{
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("[dbo].[Insert_CLV]");
			sqlDataBase.AddInParameter(cmd, "LicenseKey", SqlDbType.NVarChar, CLV.LicenseKey);
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, CLV.OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CLV.CompanyId);
			sqlDataBase.AddInParameter(cmd, "ConvertedLicenseKey", SqlDbType.NVarChar, CLV.ConvertedLicenseKey);
			return Conversion.ToInt(sqlDataBase.ExecuteScalar(cmd));
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public List<Architecture.Model.CLV> ReadAll(int OrganizationId, int CompanyId)
	{
		IDataReader reader = null;
		List<Architecture.Model.CLV> CLVList = new List<Architecture.Model.CLV>();
		try
		{
			DbCommand cmd = sqlDataBase.GetStoredProcCommand("[dbo].[GetLicenseKey]");
			sqlDataBase.AddInParameter(cmd, "OrganizationId", SqlDbType.Int, OrganizationId);
			sqlDataBase.AddInParameter(cmd, "CompanyId", SqlDbType.Int, CompanyId);
			reader = sqlDataBase.ExecuteReader(cmd);
			while (reader.Read())
			{
				CLVList.Add(FillDTO(reader));
			}
			return CLVList;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			if (reader != null && !reader.IsClosed)
			{
				reader.Close();
			}
		}
	}

	private Architecture.Model.CLV FillDTO(IDataReader reader)
	{
		try
		{
			return new Architecture.Model.CLV
			{
				Id = Conversion.ToInt(reader["Id"]),
				CompanyName = Conversion.ToString(reader["CompanyName"]),
				LicenseKey = Conversion.ToString(reader["LicenseKey"])
			};
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
