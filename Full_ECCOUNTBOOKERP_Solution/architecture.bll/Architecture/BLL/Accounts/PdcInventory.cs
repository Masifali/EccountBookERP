using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Accounts;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Accounts;

public class PdcInventory
{
	public static int Save(PdcInventoryHeader obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Accounts.PdcInventory().SetDate(obj, "Sp_PdcInventoryHeader_Insert");
			}
			return new Architecture.DAL.Accounts.PdcInventory().SetDate(obj, "Sp_PdcInventoryHeader_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetGenerateCode(PdcInventoryHeader obj)
	{
		DataTable dt = null;
		try
		{
			int Code = 0;
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
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			dt = GenericProvider.GetDataTableProc<PdcInventoryHeader>("Sp_PdcInventory_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetData(Architecture.Model.Accounts.PdcInventory obj)
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
				ParameterName = "@MethodType",
				Value = "GetAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PdcInventory>("SP_CheqBookHeader_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static PdcInventoryHeader GetByID(int Id)
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
				Value = "ReadByIdHeader"
			});
			new List<PdcInventoryHeader>();
			return new Architecture.DAL.Accounts.PdcInventory().GetDate("Sp_PdcInventory_GetAllMethod", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PdcStatusUpdate(Architecture.Model.Accounts.VoucherHead obj)
	{
		try
		{
			foreach (VoucherDetail VD in obj.voucherDetailList.ToList())
			{
				new Architecture.DAL.Accounts.PdcInventory().PdcCheqStatusUpdate(VD.InvoiceNoRefId, "Pending");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetAll(Architecture.Model.Accounts.PdcInventory obj)
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
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PdcInventory>("Sp_PdcInventory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetByType(ReportsParameters obj)
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
				ParameterName = "@AccountId",
				Value = obj.AccountId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DebitAcId",
				Value = obj.Id
			});
			if (obj.CheqId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CheqId",
					Value = obj.CheqId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadByType"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Accounts.PdcInventory>("Sp_PdcInventory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetIdByDocNo(PdcInventoryHeader obj)
	{
		DataTable dt = null;
		try
		{
			int Code = 0;
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
				ParameterName = "@DocNo",
				Value = obj.DocNo
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdByDocNo"
			});
			dt = GenericProvider.GetDataTableProc<PdcInventoryHeader>("Sp_PdcInventory_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				Code = Conversion.ToInt(dt.Rows[0]["Id"]);
			}
			return Code;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable FormHistory(ReportsParameters obj)
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
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (obj.FinancialYearId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = obj.FinancialYearId
				});
			}
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
			return GenericProvider.GetDataTableProc<PurchaseOrder>("Sp_PdcInventory_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void PdcCheqDeleteByPdcInventoryId(PdcInventoryHeader obj)
	{
		try
		{
			new Architecture.DAL.Accounts.PdcInventory().PdcCheqDeleteByPdcInventoryId(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
