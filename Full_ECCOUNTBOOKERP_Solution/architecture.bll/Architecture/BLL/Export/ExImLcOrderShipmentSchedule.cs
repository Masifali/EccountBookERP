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

public class ExImLcOrderShipmentSchedule
{
	public static int Save(Architecture.Model.Export.ExImLcOrderShipmentSchedule obj)
	{
		try
		{
			return new Architecture.DAL.Export.ExImLcOrderShipmentSchedule().SetData(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveScheduleDetailWithPm(Architecture.Model.Export.ExImScheduleDetailWithPm obj, ExImScheduleOtherItemsDetail objOther)
	{
		try
		{
			return new Architecture.DAL.Export.ExImScheduleDetailWithPm().SetDateForPmDetail(obj, objOther);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExImScheduleDetailWithPmByScheduleId(int Id)
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
				Value = "ExImScheduleDetailWithPmByScheduleId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImScheduleDetailWithPm>("USp_ExImLcOrderShipmentSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetOtherItemsDataForSchedule(int OrganizationId, int CompanyId, int ContractId, int ContractScheduleId)
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
				ParameterName = "@ContractId",
				Value = ContractId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ContractScheduleId",
				Value = ContractScheduleId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImScheduleDetailWithPm>("[dbo].[USP_GetOtherItemsDataForSchedule]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Get_ExImLcOrderPmDetailForSchedulePM(int ContractId, int ContractScheduleId, int BrandId, int BrandUomId, int BrandPackingTypeId)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@ContractId",
				Value = ContractId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ContractScheduleId",
				Value = ContractScheduleId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BrandId",
				Value = BrandId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BrandUomId",
				Value = BrandUomId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@BrandPackingTypeId",
				Value = BrandPackingTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImScheduleDetailWithPm>("[dbo].[Get_ExImLcOrderPmDetailForSchedulePM]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<Architecture.Model.Export.ExImLcOrderShipmentSchedule> GetScheduleByContractId(int Id)
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
			new List<Architecture.Model.Export.ExImLcOrderShipmentSchedule>();
			return new Architecture.DAL.Export.ExImLcOrderShipmentSchedule().GetData("USp_ExImLcOrderShipmentSchedule_GetAllMethod", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentSchedule>("USp_ExImLcOrderShipmentSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int SaveDetail(ExImLcOrderShipmentSchedulePackingDetail obj)
	{
		try
		{
			return new Architecture.DAL.Export.ExImLcOrderShipmentSchedule().SetDataForPackingDetail(obj, "USP_ExImLcOrderShipmentSchedulePackingDetail_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PackingMaterialRequirementSchedule_PlanningAutoInsert(int ScheduleId, int ContractId)
	{
		try
		{
			new Architecture.DAL.Export.ExImLcOrderShipmentSchedule().PackingMaterialRequirementSchedule_PlanningAutoInsert(ScheduleId, ContractId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static List<ExImLcOrderShipmentSchedulePackingDetail> GetPackingDetailByContractId(int Id)
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
			new List<ExImLcOrderShipmentSchedulePackingDetail>();
			return new Architecture.DAL.Export.ExImLcOrderShipmentSchedule().GetDataForPackingDetail("USp_ExImLcOrderShipmentSchedule_GetAllMethod", p);
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentSchedule>("USp_ExImLcOrderShipmentSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAttentiveLoadDateByLcOrderId(int Id, string DetailRecIds)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			if (!string.IsNullOrWhiteSpace(DetailRecIds))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DetailRecIds",
					Value = DetailRecIds
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetAttentiveLoadDateByLcOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentSchedule>("USp_ExImLcOrderShipmentSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DateTime GetLatestAttentiveLoadDate(int OrganizationId, int CompanyId)
	{
		DateTime LastScheduleLoadingDate = DateTime.Now;
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
				ParameterName = "@Activity",
				Value = "GetLatestAttentiveLoadDate"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentSchedule>("USp_ExImLcOrderShipmentSchedule_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				LastScheduleLoadingDate = Conversion.ToDateTime(dt.Rows[0][0]);
			}
			return LastScheduleLoadingDate;
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentSchedule>("USp_ExImLcOrderShipmentSchedule_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GridContactsExistInScheduleOrNot(int OrganizationId, int CompanyId, int ActionId = 0, int NoOfRecords = 0)
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
			if (ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActionId",
					Value = ActionId
				});
			}
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("USp_ExImLcOrderShipmentSchedule_GetAllMethod", p);
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

	public static DataTable GetContractScheduleNo(int OrganizationId, int CompanyId, DateTime FromLoading, bool Status, string AllStatus = null)
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
				ParameterName = "@Fromloading",
				Value = FromLoading
			});
			if (AllStatus != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Status",
					Value = Status
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("[dbo].[USP_GetContractScheduleNo]", p);
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
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("USP_ContractSchedule_FormHistory", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ContractSchedule_StatusReport(ReportsParameters obj)
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
			if (obj.PageNumber != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageNumber",
					Value = obj.PageNumber
				});
			}
			if (obj.PageSize != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PageSize",
					Value = obj.PageSize
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("USP_ContractSchedule_StatusReport", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet ExportContractSchedulePeriodicA(ReportsParameters obj)
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
				ParameterName = "@BranchId",
				Value = obj.BranchesId
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
			if (obj.NetWeight != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PerFclTon",
					Value = obj.NetWeight
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("Usp_ExportContractSchedulePeriodicA", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataSet ExportContractSchedulePeriodicB(ReportsParameters obj)
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
				ParameterName = "@BranchId",
				Value = obj.BranchesId
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
			if (obj.NetWeight != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PerFclTon",
					Value = obj.NetWeight
				});
			}
			if (obj.IntervalDays != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DaysInterval",
					Value = obj.IntervalDays
				});
			}
			if (obj.SortNo != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SortNo",
					Value = obj.SortNo
				});
			}
			if (obj.StatusId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferredStatusId",
					Value = obj.StatusId
				});
			}
			return GenericProvider.GetDataSetProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("Usp_ExportContractSchedulePeriodicB", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetContactScheduleNoExistsInShipmentDocumentSchedule(ReportsParameters obj)
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
				ParameterName = "@ReferedInSDS",
				Value = obj.ReferedInSDS
			});
			if (obj.SupplierCustomerId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SupplierCustomerId",
					Value = obj.SupplierCustomerId
				});
			}
			if (obj.SalesPersonId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@SalesPersonId",
					Value = obj.SalesPersonId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImLcOrderShipmentScheduleHeader>("USp_GetContactScheduleNoExistsInShipmentDocumentSchedule", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
