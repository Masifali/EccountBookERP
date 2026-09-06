using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;

namespace Architecture.DAL.Accounts;

public class InvInLandFreightAgreement
{
	public int SetData(Architecture.Model.Accounts.InvInLandFreightAgreement obj, string ProcName)
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
			if (obj.InvInLandFreightAgreementDetailList != null)
			{
				foreach (InvInLandFreightAgreementDetail item2 in obj.InvInLandFreightAgreementDetailList)
				{
					item2.InvInLandFreightAgreement = obj.Id;
					int code = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_InvInLandFreightAgreementDetail_Insert"));
					item2.Id = code;
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
				foreach (DMSAttachments item in obj.AttachmentsList)
				{
					item.RefDocumentTypeId = obj.DocumentTypeId;
					item.RefDocumentNo = returnID;
					item.EntryDate = DateTime.Now;
					item.ModifyDate = DateTime.Now;
					item.EntryUser = obj.EntryUser;
					item.ModifyUser = obj.EntryUser;
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

	public List<Architecture.Model.Accounts.InvInLandFreightAgreement> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Accounts.InvInLandFreightAgreement> lst = new List<Architecture.Model.Accounts.InvInLandFreightAgreement>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Accounts.InvInLandFreightAgreement>(sqlConnection, ProcName, p);
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Accounts.InvInLandFreightAgreement item in lst)
				{
					if (item.DocumentTypeId == 161)
					{
						List<SqlParameter> lstPrm = new List<SqlParameter>();
						lstPrm.Add(new SqlParameter("@Id", item.Id));
						lstPrm.Add(new SqlParameter("@Activity", "InvInLandFreightAgreementDetailReadById"));
						item.InvInLandFreightAgreementDetailList = GenericProvider.Get<InvInLandFreightAgreementDetail>(sqlcon, "Sp_InvInLandFreightAgreement_GetAll", lstPrm);
					}
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
