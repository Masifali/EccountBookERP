using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;

namespace Architecture.BLL;

public class AttributeVariant
{
	public static int Save(Architecture.Model.AttributeVariant obj)
	{
		try
		{
			return new Architecture.DAL.AttributeVariant().SetData(obj, "item.USP_AttributeVariant_InsertAndUpdate");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.AttributeVariant> FormHistoryList(Architecture.Model.AttributeVariant obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.organizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.companyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new List<Architecture.Model.AttributeVariant>();
			return new Architecture.DAL.AttributeVariant().GetAll("item.USP_AttributeVariant_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.AttributeVariant obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.organizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.companyId
			});
			if (obj.attributeId != 0L)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.attributeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AttributeVariant>("item.USP_AttributeVariant_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.AttributeVariant ReadById(int Id)
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
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			new List<Architecture.Model.AttributeVariant>();
			return new Architecture.DAL.AttributeVariant().GetAll("item.USP_AttributeVariant_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable AttributeVariantCombo(Architecture.Model.AttributeVariant obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.attributeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "AttributesForCombo"
			});
			new List<Architecture.Model.AttributeVariant>();
			return GenericProvider.GetDataTableProc<Architecture.Model.AttributeVariant>("item.USP_AttributeVariant_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeleteById(int Id)
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
				ParameterName = "@Activity",
				Value = "DeleteById"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AttributeVariant>("item.USP_AttributeVariant_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Attribute_GetForItemCategory(int organizationId, int companyId, int ItemCategoryId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = organizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = companyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ItemCategoryId",
				Value = ItemCategoryId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.AttributeVariant>("item.USP_Attribute_GetForItemCategory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
