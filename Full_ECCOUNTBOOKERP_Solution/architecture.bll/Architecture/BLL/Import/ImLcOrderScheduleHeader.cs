using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Import;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Import;

public class ImLcOrderScheduleHeader
{
	public static int Save(Architecture.Model.Import.ImLcOrderScheduleHeader obj)
	{
		try
		{
			return new Architecture.DAL.Import.ImLcOrderScheduleHeader().SetData(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Import.ImLcOrderScheduleHeader> GetScheduleByContractId(int Id)
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
			new List<Architecture.Model.Import.ImLcOrderScheduleHeader>();
			return new Architecture.DAL.Import.ImLcOrderScheduleHeader().GetData("USP_ImLcOrderSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ScheduleHistoryByContractId(int Id)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrderScheduleHeader>("USP_ImLcOrderSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveDetail(ImLcOrderSchedulePackingDetail obj)
	{
		try
		{
			return new Architecture.DAL.Import.ImLcOrderScheduleHeader().SetDataForPackingDetail(obj, "USP_ImLcOrderSchedulePackingDetail_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<ImLcOrderSchedulePackingDetail> GetPackingDetailByContractId(int Id)
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
				Value = "ReadDetailByLcorderId"
			});
			new List<ImLcOrderSchedulePackingDetail>();
			return new Architecture.DAL.Import.ImLcOrderScheduleHeader().GetDataForPackingDetail("USP_ImLcOrderSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable SchedulePackingDetailHistoryByContractId(int Id)
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
				Value = "ReadDetailByLcorderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrderScheduleHeader>("USP_ImLcOrderSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAttentiveLoadDateByLcOrderId(int Id)
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
				Value = "GetAttentiveLoadDateByLcOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrderScheduleHeader>("USP_ImLcOrderSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetNoOfContainersByScheduleAndLcOrderId(int ContractId, int ScheduleId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = ContractId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ScheduleId",
				Value = ScheduleId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetNoOfContainersByScheduleAndLcOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrderScheduleHeader>("USP_ImLcOrderSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GridContactsExistInScheduleOrNot(int OrganizationId, int CompanyId, int ActionId, int NoOfRecords = 0)
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
				ParameterName = "@ActionId",
				Value = ActionId
			});
			if (NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = NoOfRecords
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GridContactsExistInScheduleOrNot"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrderScheduleHeader>("USP_ImLcOrderSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ContractSchedule_FormHistory(ReportsParameters obj)
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
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.DestinationPortId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DestinationPortId",
					Value = obj.DestinationPortId
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
			if (obj.NoOfRecords != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@NoOfRecords",
					Value = obj.NoOfRecords
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrderScheduleHeader>("[USP_ImLcOrderSchedule_FormHistory]", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrderScheduleHeader>("USP_GetDataForDropDownFromImportLcOrderSchedule", p);
		}
		catch (Exception ex)
		{
			throw ex;
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ImLcOrderScheduleHeader>("USP_ImLcOrderSchedule_GetAllMethod", p);
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
			return GenericProvider.GetDataTableProc<ExImLcOrder>("SpExImLcOrder_ShipmentSchedule_Rpt", p);
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
}
