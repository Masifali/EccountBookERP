using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.DAL.Mfg;
using Architecture.Model.Mfg;

namespace Architecture.BLL.Mfg;

public class WarehouseAndProductionStagesAllocateToStockTransferType
{
	public static int Save(Architecture.Model.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType().SetData(obj, "[Mfg].[USP_WarehouseAndProductionStagesAllocateToStockTransferType_Insert]");
			}
			return new Architecture.DAL.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType().SetData(obj, "[Mfg].[USP_WarehouseAndProductionStagesAllocateToStockTransferType_Update]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(Architecture.Model.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType obj)
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
			if (obj.TransferTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@TransferTypeId",
					Value = obj.TransferTypeId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType>("[Mfg].[USP_WarehouseAndProductionStagesAllocateToStockTransferType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetWarehouseAndProductionStageDataByTransferTypeId(int OrganizationId, int CompanyId, int TransferTypeId)
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
			p.Add(new SqlParameter
			{
				ParameterName = "@TransferTypeId",
				Value = TransferTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType>("[Mfg].[usp_getWarehouseAndProductionStageDataByTransferTypeId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType> FormHistoryWithList(Architecture.Model.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType obj)
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
				Value = "FormHistory"
			});
			new List<Architecture.Model.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType>();
			return new Architecture.DAL.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType().GetAll("[Mfg].[USP_WarehouseAndProductionStagesAllocateToStockTransferType_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType ReadById(int Id)
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
			new List<Architecture.Model.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType>();
			return new Architecture.DAL.Mfg.WarehouseAndProductionStagesAllocateToStockTransferType().GetAll("[Mfg].[USP_WarehouseAndProductionStagesAllocateToStockTransferType_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
