using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Import;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Model.Inventory;

namespace Architecture.BLL.Import;

public class invoiceMaster
{
	private static void FillEntity(Architecture.Model.Import.invoiceMaster obj, ActionMode mod)
	{
		try
		{
			switch (mod)
			{
			case ActionMode.NEW:
				obj.AppActionId = 1;
				break;
			case ActionMode.EDIT:
				obj.AppActionId = 2;
				break;
			case ActionMode.DELETE:
				obj.AppActionId = 3;
				break;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public object Delete(Architecture.Model.Import.invoiceMaster obj, ParameterObject p = null)
	{
		try
		{
			FillEntity(obj, ActionMode.DELETE);
			return invoiceMasterProvider.Set(obj, "Delete");
		}
		catch (Exception ex)
		{
			throw new Exception("Error deleting invoiceMaster", ex);
		}
	}

	public static object Save(Architecture.Model.Import.invoiceMaster obj, ParameterObject p = null)
	{
		try
		{
			string action = ((obj.AppActionId > 0) ? "UPDATE" : "INSERT");
			return invoiceMasterProvider.Set(obj, action);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static Architecture.Model.Import.invoiceMaster ReadById(long invoiceMasterID)
	{
		try
		{
			List<ParameterList> p = new List<ParameterList>
			{
				new ParameterList
				{
					ParameterName = "@invoiceMasterID",
					ParameterValue = invoiceMasterID
				}
			};
			return invoiceMasterProvider.GetAll("ID", p)?.FirstOrDefault();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable SEARCHHistory(ReportsParameters obj)
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
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsForPackingList",
					Value = obj.ActionId
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.ExporterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@exporterId",
					Value = obj.ExporterId
				});
			}
			if (obj.ImporterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@importerId",
					Value = obj.ImporterId
				});
			}
			if (!obj.CanViewAllRecord)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@createdUserId",
					Value = obj.EntryUser
				});
			}
			return GenericProvider.GetAllDataTableCore<invoiceMaster>("SEARCH", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.Import.invoiceMaster obj)
	{
		DataTable dt = null;
		try
		{
			int DocNo = 0;
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
				Value = obj.documentTypeId
			});
			if (obj.BranchesId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = obj.BranchesId
				});
			}
			dt = GenericProvider.GetDataTableProc<Architecture.Model.Import.invoiceMaster>("[ImEx].[usp_Get_invoiceMasterGenerateDocNo]", p);
			if (dt.Rows.Count > 0)
			{
				DocNo = Conversion.ToInt(dt.Rows[0]["DocNo"]);
			}
			return DocNo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ImportInvoiceSlipSupReprt(long Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@InvoiceMasterId",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<InvPurchaseInvoice>("[ImEx].[usp_Get_Invoice_SubReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable InvoiceMaster_Report(ReportsParameters obj)
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
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsForPackingList",
					Value = obj.ActionId
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoFrom",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocNoTo",
					Value = obj.ToDocNo
				});
			}
			if (obj.ExporterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@exporterId",
					Value = obj.ExporterId
				});
			}
			if (obj.ImporterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@importerId",
					Value = obj.ImporterId
				});
			}
			if (obj.ImporterId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@importerId",
					Value = obj.ImporterId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.Import.invoiceMaster>("[ImEx].[usp_Get_invoiceMasterReport]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFrom_ImportRelated(ReportsParameters obj)
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
			if (!string.IsNullOrEmpty(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<ExImInvoice>("ImEx.USP_GetDataForDropDownFrom_ImportRelated", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDataForDropDownFrom_Invoice(ReportsParameters obj)
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
			if (!string.IsNullOrEmpty(obj.Activity))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<ExImInvoice>("ImEx.USP_GetDataForDropDownFrom_Invoice", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
