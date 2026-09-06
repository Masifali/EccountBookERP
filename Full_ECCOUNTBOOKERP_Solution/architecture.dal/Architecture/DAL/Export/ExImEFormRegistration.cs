using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Export;

namespace Architecture.DAL.Export;

public class ExImEFormRegistration : DALBase
{
	public int SetDate(Architecture.Model.Export.ExImEFormRegistration obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			if (obj.EFormAdvancePaymentUtilizationDetail != null)
			{
				foreach (ExImEFormRegistrationAdvancePaymentUtilizationDetail item4 in obj.EFormAdvancePaymentUtilizationDetail)
				{
					item4.ExImEFormRegistrationId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_ExImEFormRegistrationAdvancePaymentUtilizationDetail_Insert"));
				}
			}
			if (obj.EFormRegistrationComodityDetail != null)
			{
				foreach (ExImEFormRegistrationComodityDetail item3 in obj.EFormRegistrationComodityDetail)
				{
					item3.ExImEFormRegistrationId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_ExImEFormRegistrationComodityDetail_Insert"));
				}
			}
			if (obj.EFormRegistrationPaymentTerms != null)
			{
				foreach (ExImEFormRegistrationPaymentTerms item2 in obj.EFormRegistrationPaymentTerms)
				{
					item2.ExImEFormRegistrationId = obj.Id;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_ExImEFormRegistrationPaymentTerms_Insert"));
				}
			}
			if (obj.AttachmentsList != null && obj.AttachmentsList.Count > 0)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@ScreenName",
					Value = obj.AttachmentsList[0].ScreenName
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = returnID
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = "DeleteById"
				});
				new DMSAttachment().GetData("Sp_DMSAttachments_GetAllMethod", p);
				foreach (DMSAttachments item in obj.AttachmentsList.ToList())
				{
					item.RefAccountId = obj.IssuingBankId;
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUser;
					item.ModifyUser = obj.ModifyUser;
					item.OrganizationId = obj.OrganizationId;
					item.CompanyId = obj.CompanyId;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Proc_DMSAttachments_Insert"));
				}
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

	public List<Architecture.Model.Export.ExImEFormRegistration> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Export.ExImEFormRegistration> lst = new List<Architecture.Model.Export.ExImEFormRegistration>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Export.ExImEFormRegistration>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImEFormRegistration item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadEFormRegistrationAdvancePaymentUtilizationDetailbyEformHeaderId"));
						item3.EFormAdvancePaymentUtilizationDetail = GenericProvider.Get<ExImEFormRegistrationAdvancePaymentUtilizationDetail>(sqlConnection2, "Sp_ExImEFormRegistration_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Export.ExImEFormRegistration item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadEFormRegistrationComodityDetailbyEformHeaderId"));
						item2.EFormRegistrationComodityDetail = GenericProvider.Get<ExImEFormRegistrationComodityDetail>(sqlConnection3, "Sp_ExImEFormRegistration_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Export.ExImEFormRegistration item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "ReadExImEFormRegistrationPaymentTermsbyEformHeaderId"));
					item.EFormRegistrationPaymentTerms = GenericProvider.Get<ExImEFormRegistrationPaymentTerms>(sqlcon, "Sp_ExImEFormRegistration_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
