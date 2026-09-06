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
using Architecture.Model.Accounts;
using Architecture.Model.OrganizationRegistration;

namespace Architecture.DAL.OrganizationRegistration;

public class OrganizationRegistration
{
	public int SetData(Architecture.Model.OrganizationRegistration.OrganizationRegistration obj, string ProcName)
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
			DataTable NewlyGeneratedOrganizationInfo = GenericProvider.GetDataTableProc<VoucherHead>("Sp_Organization_GetAllMethod", parametrs);
			if (NewlyGeneratedOrganizationInfo.Rows.Count <= 0)
			{
				throw new Exception("Cannot Find Info Of Newly Generated Organization!");
			}
			DateTime NewDate = DateTime.Now;
			NewDate.AddDays(30.0);
			string LicenseKey = GetKey(Conversion.ToString(NewlyGeneratedOrganizationInfo.Rows[0]["CompName"]), NewDate);
			Architecture.Model.CLV License = new Architecture.Model.CLV();
			License.LicenseKey = LicenseKey;
			License.CompanyId = Conversion.ToInt(NewlyGeneratedOrganizationInfo.Rows[0]["CompanyId"]);
			License.OrganizationId = Conversion.ToInt(NewlyGeneratedOrganizationInfo.Rows[0]["OrganizationId"]);
			License.Id = 0;
			License.CompanyName = Conversion.ToString(NewlyGeneratedOrganizationInfo.Rows[0]["CompName"]);
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

	public string GetKey(string CompanyName, DateTime ToDate)
	{
		string License = "";
		string EncryptionKey = "AMIR2SPSCS44602";
		byte[] clearBytes = Encoding.Unicode.GetBytes(CompanyName + "(" + ToDate.ToString());
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
