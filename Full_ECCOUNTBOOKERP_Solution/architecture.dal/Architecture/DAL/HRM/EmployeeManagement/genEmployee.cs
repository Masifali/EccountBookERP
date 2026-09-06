using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;

namespace Architecture.DAL.HRM.EmployeeManagement;

public class genEmployee
{
	public int SetData(Architecture.Model.HRM.EmployeeManagement.genEmployee obj, string ProcName)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			int returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.EmployeeId = returnID;
			}
			else
			{
				returnID = obj.EmployeeId;
			}
			if (returnID > 0)
			{
				obj.EmployeeId = Convert.ToInt32(returnID);
				List<SqlParameter> s = new List<SqlParameter>();
				s.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = obj.OrganizationId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@USERID",
					Value = obj.EmployeeNo
				});
				GenericProvider.GetDataTableProc<USERINFO>("Sp_UserInfo_GetAllMethod", s);
				USERINFO user = new USERINFO();
				user.USERID = Conversion.ToInt(obj.EmployeeNo);
				user.BADGENUMBER = Conversion.ToInt(obj.EmployeeNo);
				user.NAME = obj.FistName + " " + obj.MiddleName + " " + obj.LastName;
				user.OrganizationId = obj.OrganizationId;
				user.CompanyId = obj.CompanyId;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, user, "Sp_USERINFO_Insert"));
				int EmployeeHistoryId = 0;
				foreach (Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory item10 in obj.EmployeeHistorylist)
				{
					item10.EmployeeId = returnID;
					item10.EmployeeHistoryLineId = returnID;
					EmployeeHistoryId = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item10, "Sp_genEmployeeHistory_Insert"));
				}
				if (obj.EmployeeSalarieslist.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.EmployeeSalary item9 in obj.EmployeeSalarieslist)
					{
						item9.EmployeeId = returnID;
						item9.EmployeeHistoryId = EmployeeHistoryId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item9, "hrm.Sp_EmployeeSalary_Insert"));
					}
				}
				if (obj.HrmEmployeeBenefitslist.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit item8 in obj.HrmEmployeeBenefitslist)
					{
						item8.EmployeeId = returnID;
						item8.EmployeeHistoryId = EmployeeHistoryId;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item8, "Sp_hrmEmployeeBenefit_Insert"));
					}
				}
				foreach (genEmployeeWeekDay item7 in obj.EmployeeWeekDayslist)
				{
					item7.EmployeeId = returnID;
					Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item7, "Sp_genEmployeeWeekDay_Insert"));
				}
				if (obj.EmployeeAddressDetailslist.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.tblEmployeeAddressDetail item6 in obj.EmployeeAddressDetailslist)
					{
						item6.EmployeeId = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item6, "Sp_tblEmployeeAddressDetail_Insert"));
					}
				}
				if (obj.EmployeeExperienceslist.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.hrmEmployeeExperience item5 in obj.EmployeeExperienceslist)
					{
						item5.EmployeeId = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item5, "Sp_hrmEmployeeExperience_Insert"));
					}
				}
				if (obj.EmployeeEducationslist.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.GenEmployeeEducation item4 in obj.EmployeeEducationslist)
					{
						item4.EmployeeId = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_GenEmployeeEducation_Insert"));
					}
				}
				if (obj.EmployeeFamilyInfoslist.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.hrmEmployeeFamilyInfo item3 in obj.EmployeeFamilyInfoslist)
					{
						item3.EmployeeId = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_hrmEmployeeFamilyInfo_Insert"));
					}
				}
				if (obj.EmployeeReferenceslist.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.hrmEmployeeReference item2 in obj.EmployeeReferenceslist)
					{
						item2.EmployeeId = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_hrmEmployeeReference_Insert"));
					}
				}
				if (obj.EmployeeBankAccountslist.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBankAccount item in obj.EmployeeBankAccountslist)
					{
						item.EmployeeId = returnID;
						Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "Sp_hrmEmployeeBankAccount_Insert"));
					}
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

	public List<Architecture.Model.HRM.EmployeeManagement.genEmployee> GetAll(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.HRM.EmployeeManagement.genEmployee> lst = new List<Architecture.Model.HRM.EmployeeManagement.genEmployee>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.HRM.EmployeeManagement.genEmployee>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item10 in lst)
					{
						List<SqlParameter> lstPrm10 = new List<SqlParameter>();
						lstPrm10.Add(new SqlParameter("@EmployeeId", item10.EmployeeId));
						lstPrm10.Add(new SqlParameter("@Activity", "ReadEmployeeHistoryByEmployeeId"));
						item10.EmployeeHistorylist = GenericProvider.Get<Architecture.Model.HRM.EmployeeManagement.genEmployeeHistory>(sqlConnection2, "Sp_genEmployee_GetAllMethod", lstPrm10);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item9 in lst)
					{
						List<SqlParameter> lstPrm9 = new List<SqlParameter>();
						lstPrm9.Add(new SqlParameter("@EmployeeId", item9.EmployeeId));
						lstPrm9.Add(new SqlParameter("@Activity", "ReadEmployeeSalaryByEmployeeId"));
						item9.EmployeeSalarieslist = GenericProvider.Get<Architecture.Model.HRM.EmployeeManagement.EmployeeSalary>(sqlConnection3, "Sp_genEmployee_GetAllMethod", lstPrm9);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item8 in lst)
					{
						List<SqlParameter> lstPrm8 = new List<SqlParameter>();
						lstPrm8.Add(new SqlParameter("@EmployeeId", item8.EmployeeId));
						lstPrm8.Add(new SqlParameter("@Activity", "ReadEmployeeBenfitsByEmployeeId"));
						item8.HrmEmployeeBenefitslist = GenericProvider.Get<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBenefit>(sqlConnection4, "Sp_genEmployee_GetAllMethod", lstPrm8);
					}
				}
			}
			using (SqlConnection sqlConnection5 = ConnectionObject.SQL())
			{
				sqlConnection5.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item7 in lst)
					{
						List<SqlParameter> lstPrm7 = new List<SqlParameter>();
						lstPrm7.Add(new SqlParameter("@EmployeeId", item7.EmployeeId));
						lstPrm7.Add(new SqlParameter("@Activity", "ReadWeekDaysByEmployeeId"));
						item7.EmployeeWeekDayslist = GenericProvider.Get<genEmployeeWeekDay>(sqlConnection5, "Sp_genEmployee_GetAllMethod", lstPrm7);
					}
				}
			}
			using (SqlConnection sqlConnection6 = ConnectionObject.SQL())
			{
				sqlConnection6.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item6 in lst)
					{
						List<SqlParameter> lstPrm6 = new List<SqlParameter>();
						lstPrm6.Add(new SqlParameter("@EmployeeId", item6.EmployeeId));
						lstPrm6.Add(new SqlParameter("@Activity", "ReadEmployeeAddressByEmployeeId"));
						item6.EmployeeAddressDetailslist = GenericProvider.Get<Architecture.Model.HRM.EmployeeManagement.tblEmployeeAddressDetail>(sqlConnection6, "Sp_genEmployee_GetAllMethod", lstPrm6);
					}
				}
			}
			using (SqlConnection sqlConnection7 = ConnectionObject.SQL())
			{
				sqlConnection7.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item5 in lst)
					{
						List<SqlParameter> lstPrm5 = new List<SqlParameter>();
						lstPrm5.Add(new SqlParameter("@EmployeeId", item5.EmployeeId));
						lstPrm5.Add(new SqlParameter("@Activity", "ReadEmployeeExperenceByEmployeeId"));
						item5.EmployeeExperienceslist = GenericProvider.Get<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeExperience>(sqlConnection7, "Sp_genEmployee_GetAllMethod", lstPrm5);
					}
				}
			}
			using (SqlConnection sqlConnection8 = ConnectionObject.SQL())
			{
				sqlConnection8.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@EmployeeId", item4.EmployeeId));
						lstPrm4.Add(new SqlParameter("@Activity", "ReadEmployeeEducationByEmployeeId"));
						item4.EmployeeEducationslist = GenericProvider.Get<Architecture.Model.HRM.EmployeeManagement.GenEmployeeEducation>(sqlConnection8, "Sp_genEmployee_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection9 = ConnectionObject.SQL())
			{
				sqlConnection9.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@EmployeeId", item3.EmployeeId));
						lstPrm3.Add(new SqlParameter("@Activity", "ReadEmployeeReferebceByEmployeeId"));
						item3.EmployeeReferenceslist = GenericProvider.Get<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeReference>(sqlConnection9, "Sp_genEmployee_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection10 = ConnectionObject.SQL())
			{
				sqlConnection10.Open();
				if (lst != null && lst.Count > 0)
				{
					foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@EmployeeId", item2.EmployeeId));
						lstPrm2.Add(new SqlParameter("@Activity", "ReadEmployeeFamilyInfoByEmployeeId"));
						item2.EmployeeFamilyInfoslist = GenericProvider.Get<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeFamilyInfo>(sqlConnection10, "Sp_genEmployee_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null && lst.Count > 0)
			{
				foreach (Architecture.Model.HRM.EmployeeManagement.genEmployee item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@EmployeeId", item.EmployeeId));
					lstPrm.Add(new SqlParameter("@Activity", "ReadEmployeeBankByEmployeeId"));
					item.EmployeeBankAccountslist = GenericProvider.Get<Architecture.Model.HRM.EmployeeManagement.hrmEmployeeBankAccount>(sqlcon, "Sp_genEmployee_GetAllMethod", lstPrm);
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
