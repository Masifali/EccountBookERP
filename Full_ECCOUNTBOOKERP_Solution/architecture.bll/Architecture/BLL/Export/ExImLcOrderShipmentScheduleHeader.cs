using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Export;

public class ExImLcOrderShipmentScheduleHeader
{
	public static int Save(Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Export.ExImLcOrderShipmentScheduleHeader().SetData(obj, "Sp_ExImLcOrderShipmentScheduleHeader_Insert");
			}
			return new Architecture.DAL.Export.ExImLcOrderShipmentScheduleHeader().SetData(obj, "Sp_ExImLcOrderShipmentScheduleHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveDetail(ExImLcOrderShipmentNInspectionNProductionSchedule obj)
	{
		try
		{
			return new Architecture.DAL.Export.ExImLcOrderShipmentScheduleHeader().SetDataForDetail(obj, "Sp_ExImLcOrderShipmentNInspectionNProductionSchedule_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader GetByID(int Id)
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
			new List<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>();
			return new Architecture.DAL.Export.ExImLcOrderShipmentScheduleHeader().GetDate("Sp_ExImLcOrderShipmentScheduleHeader_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetIdShipmentScheduleByLcOrderId(int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ExImLcOrder",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdShipmentScheduleByLcOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("Sp_ExImLcOrderShipmentScheduleHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByCompanyOoganizationId(ReportsParameters obj)
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
				Value = "ReadByCompanyOrganizationId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("Sp_ExImLcOrderShipmentScheduleHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetpendingContractsforShipmentSchedule(ReportsParameters obj)
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
				Value = "GetpendingContractsforShipmentSchedule"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("Sp_ExImLcOrderShipmentScheduleHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportShipmentsHistory(ReportsParameters obj)
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
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.ProjectsId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ProjectsId",
					Value = obj.ProjectsId
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.FromDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDate",
					Value = obj.FromDate
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ToDate))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDate",
					Value = obj.ToDate
				});
			}
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = obj.Status
				});
			}
			if (obj.OrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@LcOrderId",
					Value = obj.OrderId
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
			if (obj.ItemId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemId",
					Value = obj.ItemId
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrder>("SpExImLcOrder_ShipmentSchedule_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void UpdateStatusandIsApprovedbyDetailId(ReportsParameters obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "UpdateStatusShipmentScheduleDetailByDetailId"
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Status",
				Value = obj.Status
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@UserId",
				Value = obj.EntryUser
			});
			GenericProvider.GetDataTableProc<PurchaseOrder>("Sp_ExImLcOrderShipmentScheduleHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFromShipmentSchedule(int OrganizationId, int CompanyId, string Activity = null)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@OrganizationId",
				Value = OrganizationId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@CompanyId",
				Value = CompanyId
			});
			if (Activity != null && Activity != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = Activity
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentSchedule>("USP_GetDataForDropDownFromShipmentSchedule", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
