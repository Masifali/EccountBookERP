using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Templates;

namespace Architecture.DAL;

public class Company
{
	public int SetData(Architecture.Model.Company obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Company> GetData(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Company> lst = new List<Architecture.Model.Company>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Company>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Company item in lst)
				{
					new List<SqlParameter>().Add(new SqlParameter("@Id", item.Id));
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}

	public int SetDataForAll(CompanyRegistration obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID <= 0)
			{
				throw new Exception("Cannot Find Id of Newly Generated Organization Info");
			}
			List<SqlParameter> parametrs = new List<SqlParameter>();
			parametrs.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetNewlyGeneratedCompanyData"
			});
			parametrs.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = returnID
			});
			DataTable NewlyGeneratedOrganizationInfo = GenericProvider.GetDataTableProc<CompanyRegistration>("[dbo].[Sp_Company_GetAllMethod]", parametrs);
			if (NewlyGeneratedOrganizationInfo.Rows.Count <= 0)
			{
				throw new Exception("Cannot Find Info Of Newly Generated Organization!");
			}
			DateTime NewDate = DateTime.Now.AddDays(90.0);
			string LicenseKey = GetKey(Conversion.ToString(NewlyGeneratedOrganizationInfo.Rows[0]["CompName"]), NewDate.ToString("dd-MMM-yyyy"));
			Architecture.Model.CLV License = new Architecture.Model.CLV();
			License.LicenseKey = LicenseKey;
			License.CompanyId = Conversion.ToInt(NewlyGeneratedOrganizationInfo.Rows[0]["CompanyId"]);
			License.OrganizationId = Conversion.ToInt(NewlyGeneratedOrganizationInfo.Rows[0]["OrganizationId"]);
			License.Id = 0;
			License.CompanyName = Conversion.ToString(NewlyGeneratedOrganizationInfo.Rows[0]["CompName"]);
			License.ConvertedLicenseKey = Architecture.Common.LicenseKey.ReturnKey(License.LicenseKey);
			Convert.ToInt32(GenericProvider.SetProc(sqlTrn, License, "Insert_CLV"));
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public string GetKey(string CompanyName, string ToDate)
	{
		string License = "";
		string EncryptionKey = "AMIR2SPSCS44602";
		byte[] clearBytes = Encoding.Unicode.GetBytes(CompanyName + "(" + ToDate);
		using Aes encryptor = Aes.Create();
		Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[13]
		{
			73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
			100, 101, 118
		});
		encryptor.Key = pdb.GetBytes(32);
		encryptor.IV = pdb.GetBytes(16);
		using MemoryStream ms = new MemoryStream();
		using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cs.Write(clearBytes, 0, clearBytes.Length);
			cs.Close();
		}
		return Convert.ToBase64String(ms.ToArray());
	}
}
