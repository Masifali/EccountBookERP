using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImGdEeEfShipmentHistory
{
	public static Architecture.Model.Export.ExImGdEeEfShipmentHistory GetByID(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadById"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			new List<Architecture.Model.Export.ExImGdEeEfShipmentHistory>();
			return new Architecture.DAL.Export.ExImGdEeEfShipmentHistory().GetDate("Sp_ExImGdEeEfShipmentHistory_UpdateFromAll_Rpt", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Export.ExImGdEeEfShipmentHistory obj)
	{
		try
		{
			new List<Architecture.Model.Export.ExImGdEeEfShipmentHistory>();
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
				Value = "ReadAll"
			});
			if (obj.LcOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImLcOrderNoId",
					Value = obj.LcOrderId
				});
			}
			if (obj.ExImInvoiceNoId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceNoId",
					Value = obj.ExImInvoiceNoId
				});
			}
			if (obj.EformNoId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImEformId",
					Value = obj.EformNoId
				});
			}
			if (obj.BillOfLadingNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BLNoId",
					Value = obj.BillOfLadingNo
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImGdEeEfShipmentHistory>("Sp_ExImGdEeEfShipmentHistory_UpdateFromAll_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
