using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Production;
using Architecture.Model;
using Architecture.Model.Production;

namespace Architecture.BLL.Production;

public class ProdcutionFohAllocateToJobOrder
{
	public static int Save(Architecture.Model.Production.ProdcutionFohAllocateToJobOrder obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
			}
			else
			{
				obj.ActionId = 2;
			}
			return new Architecture.DAL.Production.ProdcutionFohAllocateToJobOrder().SetData(obj, "[dbo].[USP_ProdcutionFohAllocateToJobOrder_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.Production.ProdcutionFohAllocateToJobOrder GetByID(int Id)
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
			new List<Architecture.Model.Production.ProdcutionFohAllocateToJobOrder>();
			return new Architecture.DAL.Production.ProdcutionFohAllocateToJobOrder().GetData("[dbo].[USP_ProdcutionFohAllocateToJobOrder_GetAllMethod]", p)[0];
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@CanViewAllRecord",
				Value = obj.CanViewAllRecord
			});
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryUserId",
					Value = obj.EntryUser
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryFromDate",
					Value = obj.EntryDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.EntryDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@EntryToDate",
					Value = obj.EntryDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyFromDate",
					Value = obj.ModifyDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ModifyDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ModifyToDate",
					Value = obj.ModifyDateTo
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateFrom))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedFromDate",
					Value = obj.ApprovedDateFrom
				});
			}
			if (!Conversion.CheckDateTimeNull(obj.ApprovedDateTo))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedToDate",
					Value = obj.ApprovedDateTo
				});
			}
			if (obj.BranchesIds != string.Empty || obj.BranchesIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderId",
					Value = obj.JobOrderId
				});
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			if (!string.IsNullOrWhiteSpace(obj.Month))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Month",
					Value = obj.Month
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProdcutionFohAllocateToJobOrder>("[dbo].[USP_ProdcutionFohAllocateToJobOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int Id, int EntryUserId)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlcon;
			sqlCommand.Transaction = sqlTrn;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "[dbo].[USP_ProductionFohAllocateToJobOrder_DeleteById]";
			sqlCommand.Parameters.AddWithValue("@EntryUserId", EntryUserId);
			sqlCommand.Parameters.AddWithValue("@Id", Id);
			sqlCommand.ExecuteNonQuery();
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static DataTable GetDataForDropDownFromProdcutionFohAllocateToJobOrder(ReportsParameters obj)
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
			if (obj.Activity != string.Empty && obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			if (obj.BranchesIds != string.Empty || obj.BranchesIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesIds",
					Value = obj.BranchesIds
				});
			}
			if (obj.DocumentTypeIds != string.Empty || obj.DocumentTypeIds != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeIds",
					Value = obj.DocumentTypeIds
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProdcutionFohAllocateToJobOrder>("[dbo].[USP_GetDataForDropDownFromProdcutionFohAllocateToJobOrder]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetExpensesAccountForFOHAllocation(int OrganizationId, int CompanyId, DateTime DocDate)
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
				ParameterName = "@DocDate",
				Value = DocDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.BranchesAllocationToUser>("[dbo].[usp_getExpensesAccountForFOHAllocation]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProdcutionFohAllocateToJobOrder_DataForOverHeads(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			p.Add(new SqlParameter
			{
				ParameterName = "@PlantId",
				Value = obj.PlantId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@JobOrderId",
				Value = obj.JobOrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocDate",
				Value = obj.DocDate
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProdcutionFohAllocateToJobOrder>("[dbo].[USP_ProdcutionFohAllocateToJobOrder_DataForOverHeads]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ProdcutionFohAllocateToJobOrder_SlipAndRegister(ReportsParameters obj)
	{
		try
		{
			new DataTable();
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			if (obj.Id != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Id",
					Value = obj.Id
				});
			}
			if (obj.PlantId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PlantId",
					Value = obj.PlantId
				});
			}
			if (obj.JobOrderId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@JobOrderId",
					Value = obj.JobOrderId
				});
			}
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Production.ProdcutionFohAllocateToJobOrder>("[dbo].[USP_ProdcutionFohAllocateToJobOrder_SlipAndRegister]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
