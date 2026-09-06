using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Templates;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Templates;

namespace Architecture.BLL.Templates;

public class Organization_ChartOfAccountTemplate
{
	public static int Save(Architecture.Model.Templates.Organization_ChartOfAccountTemplate obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			if (obj.Id == 0)
			{
				if (!string.IsNullOrEmpty(obj.ParentAccountCode))
				{
					obj.AccountCode = obj.ParentAccountCode;
				}
				DataTable dtcoa = ReadNewCodebyParentCode(obj.ParentAccountCode, obj.OrganizationTemplateId);
				if (dtcoa.Rows.Count > 0)
				{
					int i = 0;
					if (i < dtcoa.Rows.Count)
					{
						obj.AccountCode = dtcoa.Rows[i]["AccountCode"].ToString();
						obj.Account_Level = Conversion.ToInt(dtcoa.Rows[i]["Account_Level"]);
						if (string.IsNullOrEmpty(obj.AccountGroup))
						{
							obj.AccountGroup = "Group";
						}
					}
				}
				if (obj.ParentAccountCode == "" || obj.ParentAccountCode == null)
				{
					obj.ParentAccountCode = "0";
				}
				return new Architecture.DAL.Templates.Organization_ChartOfAccountTemplate().SetData(obj, "[dbo].[USP_Organization_ChartOfAccountTemplate_Insert]");
			}
			return new Architecture.DAL.Templates.Organization_ChartOfAccountTemplate().SetData(obj, "[dbo].[USP_Organization_ChartOfAccountTemplate_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveList(Architecture.Model.Templates.Organization_ChartOfAccountTemplate obj)
	{
		try
		{
			int Id = 0;
			foreach (Architecture.Model.Templates.Organization_ChartOfAccountTemplate item in obj.Organization_ChartOfAccountTemplateList)
			{
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				item.PostDate = DateTime.Now;
				if (!string.IsNullOrEmpty(item.ParentAccountCode))
				{
					item.AccountCode = item.ParentAccountCode;
				}
				DataTable dtcoa = ReadNewCodebyParentCode(item.ParentAccountCode, item.OrganizationTemplateId);
				if (dtcoa.Rows.Count > 0)
				{
					int i = 0;
					if (i < dtcoa.Rows.Count)
					{
						item.AccountCode = dtcoa.Rows[i]["AccountCode"].ToString();
						item.Account_Level = Conversion.ToInt(dtcoa.Rows[i]["Account_Level"]);
						if (string.IsNullOrEmpty(item.AccountGroup))
						{
							item.AccountGroup = "Group";
						}
					}
				}
				if (item.ParentAccountCode == "" || item.ParentAccountCode == null)
				{
					item.ParentAccountCode = "0";
				}
				Id = new Architecture.DAL.Templates.Organization_ChartOfAccountTemplate().SetData(item, "[dbo].[USP_Organization_ChartOfAccountTemplate_Insert]");
			}
			return Id;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Templates.Organization_ChartOfAccountTemplate ReadById(int Id, int OrganizationTemplateId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationTemplateId",
				Value = OrganizationTemplateId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.Templates.Organization_ChartOfAccountTemplate>();
			return new Architecture.DAL.Templates.Organization_ChartOfAccountTemplate().GetData("USP_Organization_ChartOfAccountTemplate_GetAll", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable UpdateAccountTitleById(int Id, int OrganizationTemplateId, string AccountTitle, string OtherErpCode, bool IsApproved)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@Id", Id));
			p.Add(new SqlParameter("@OrganizationTemplateId", OrganizationTemplateId));
			p.Add(new SqlParameter("@AccountTitle", AccountTitle));
			p.Add(new SqlParameter("@OtherErpCode", OtherErpCode));
			p.Add(new SqlParameter("@IsApproved", IsApproved));
			p.Add(new SqlParameter("@Activity", "UpdateAccountTitleById"));
			return GenericProvider.GetDataTableProc<ChartofAccount>("[dbo].[USP_Organization_ChartOfAccountTemplate_GetAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadNewCodebyParentCode(string ParentAccountCode, int OrganizationTemplateId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@ParentAccountCode", ParentAccountCode));
			p.Add(new SqlParameter("@OrganizationTemplateId", OrganizationTemplateId));
			p.Add(new SqlParameter("@Activity", "NewCodeByParentCode"));
			return GenericProvider.GetDataTableProc<ChartofAccount>("[dbo].[USP_Organization_ChartOfAccountTemplate_GetAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDetailAccountsByTemplateId(int OrganizationTemplateId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationTemplateId",
				Value = OrganizationTemplateId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDetailAccountsByTemplateId"
			});
			return GenericProvider.GetDataTableProc<COAAllocation>("[dbo].[USP_Organization_ChartOfAccountTemplate_GetAll]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadAllGroupAccounts(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationTemplateId", obj.OrganizationTemplateId));
			if (obj.Account_Level != 0)
			{
				p.Add(new SqlParameter("@Account_Level", obj.Account_Level));
			}
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			}
			if (obj.AccouuntClassId != 0)
			{
				p.Add(new SqlParameter("@AccountClassId", obj.AccouuntClassId));
			}
			p.Add(new SqlParameter("@Activity", "ReadAllAccountGroup"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.Organization_ChartOfAccountTemplate>("USP_Organization_ChartOfAccountTemplate_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AccountTypesByTemplateId(int OrganizationTemplateId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationTemplateId", OrganizationTemplateId));
			p.Add(new SqlParameter("@Activity", "AccountTypesByTemplateId"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.Organization_ChartOfAccountTemplate>("USP_Organization_ChartOfAccountTemplate_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DataForDropDownFromOrganization_ChartOfAccountTemplate(int OrganizationTemplateId, string Activity = "")
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationTemplateId", OrganizationTemplateId));
			if (!string.IsNullOrEmpty(Activity))
			{
				p.Add(new SqlParameter("@Activity", Activity));
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.Organization_ChartOfAccountTemplate>("USP_GetDataForDropDownFromOrganization_ChartOfAccountTemplate", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Templates.Organization_ChartOfAccountTemplate obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationTemplateId", obj.OrganizationTemplateId));
			if (obj.ParentCodeId != 0)
			{
				p.Add(new SqlParameter("@ParentAcId", obj.ParentCodeId));
			}
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter("@Id", obj.Id));
			}
			p.Add(new SqlParameter("@Activity", "FormHistory"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.Organization_ChartOfAccountTemplate>("USP_Organization_ChartOfAccountTemplate_GetAll", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Organization_ChartOfAccountTemplate_Report(Architecture.Model.Templates.Organization_ChartOfAccountTemplate obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter("@OrganizationTemplateId", obj.OrganizationTemplateId));
			if (obj.ParentCodeId != 0)
			{
				p.Add(new SqlParameter("@ParentAcId", obj.ParentCodeId));
			}
			if (obj.AccountTypeId != 0)
			{
				p.Add(new SqlParameter("@AccountTypeId", obj.AccountTypeId));
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter("@Id", obj.Id));
			}
			p.Add(new SqlParameter("@Activity", "FormHistory"));
			return GenericProvider.GetDataTableProc<Architecture.Model.Templates.Organization_ChartOfAccountTemplate>("[dbo].[USP_Organization_ChartOfAccountTemplate_Report]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int ParentCodeByAccountId(int OrganizationTemplateId, int Id)
	{
		try
		{
			int Code = 0;
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationTemplateId",
				Value = OrganizationTemplateId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ParentCodeByAccountId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Templates.Organization_ChartOfAccountTemplate>("USP_Organization_ChartOfAccountTemplate_GetAll", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["ParentAccountCode"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
