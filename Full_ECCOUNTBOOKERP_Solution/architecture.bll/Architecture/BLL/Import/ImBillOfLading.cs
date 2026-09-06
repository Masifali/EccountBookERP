using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Import;
using Architecture.Model;
using Architecture.Model.Import;

namespace Architecture.BLL.Import;

public class ImBillOfLading
{
	public static int Save(Architecture.Model.Import.ImBillOfLading obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Import.ImBillOfLading().SetData(obj, "Sp_ImBillOfLading_Insert");
			}
			return new Architecture.DAL.Import.ImBillOfLading().SetData(obj, "Sp_ImBillOfLading_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Import.ImBillOfLading GetByID(int Id)
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
			new List<Architecture.Model.Import.ImBillOfLading>();
			return new Architecture.DAL.Import.ImBillOfLading().GetAll("Sp_ImBillOfLading_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Import.ImBillOfLading obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByOrganizationCompanyId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImBillOfLading>("Sp_ImBillOfLading_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExImBillOfLading_SlipAndRegister(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = obj.OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = obj.CompanyId
			});
			if (obj.EximInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceId",
					Value = obj.EximInvoiceId
				});
			}
			if (obj.ExImEFormRegistrationId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImEFormRegistrationId",
					Value = obj.ExImEFormRegistrationId
				});
			}
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.ShippingLineId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ShippingLineId",
					Value = obj.ShippingLineId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImBillOfLading>("Sp_ImBillOfLading_SlipAndRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
