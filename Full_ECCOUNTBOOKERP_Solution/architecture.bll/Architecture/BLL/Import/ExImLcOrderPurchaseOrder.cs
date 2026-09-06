using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Import;
using Architecture.Model;
using Architecture.Model.Import;

namespace Architecture.BLL.Import;

public class ExImLcOrderPurchaseOrder
{
	public static int Save(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Import.ExImLcOrderPurchaseOrder().SetDate(obj, "Sp_ExImLcOrderPurchaseOrder_Insert");
			}
			return new Architecture.DAL.Import.ExImLcOrderPurchaseOrder().SetDate(obj, "Sp_ExImLcOrderPurchaseOrder_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistoryData(ReportsParameters obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUser",
					Value = obj.EntryUser
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
	{
		DataTable dt = null;
		try
		{
			int code = 0;
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateDocNoCompanyIdOrganizationId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateDocCodeByLcContract(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
	{
		DataTable dt = null;
		try
		{
			int CODE = 0;
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateDocNoByLcOrderCompanyIdOrganizationId"
			});
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				CODE = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return CODE;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPurchaseOrderNo(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPurchaseOrderNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Import.ExImLcOrderPurchaseOrder GetByID(int Id)
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
			new List<Architecture.Model.Import.ExImLcOrderPurchaseOrder>();
			return new Architecture.DAL.Import.ExImLcOrderPurchaseOrder().GetDate("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadLcOrderPurchaesOrderDetailByPoHeaderId(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLcContractDetailByPurchaseOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetLcContractDetailByPurchaseOrderId(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetLcContractDetailByPurchaseOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ReadPurchaseOrderByContainerNo(ReportsParameters obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			if (obj.Container1 != null && obj.Container1 != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ContainerNo",
					Value = obj.Container1
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadPurchaseOrderByContainerNo"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPurchaseOrderNoForBillOfLading(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPurchaseOrderNoForBillOfLading"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetImportPurchaseOrderDetailById(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetImportPurchaseOrderDetailById"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GETIMPORTGRNDETAILBYPURCHASEORDERID(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GETIMPORTGRNDETAILBYPURCHASEORDERID"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GETIMPORTCONTRACTDETAILBYCONTRACTID(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GETIMPORTCONTRACTDETAILBYCONTRACTID"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetContainerNosByOrderId(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetContainerNosByOrderId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetContractNoForPackingList(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetPurchaseOrderByContractId"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void ApporveImportPurchaseOrder(Architecture.Model.Import.ExImLcOrderPurchaseOrder obj)
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
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@IsApproved",
				Value = obj.IsApproved
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@ApprovedDate",
				Value = obj.ApprovedDate
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUser",
				Value = obj.ApprovedUserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ApporveImportPurchaseOrder"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.Import.ExImLcOrderPurchaseOrder>("Sp_ExImLcOrderPurchaseOrder_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
