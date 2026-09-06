using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.Export;
using Architecture.Model;
using Architecture.Model.Export;

namespace Architecture.BLL.Export;

public class ExImShippedConsignmentFollowUps
{
	public static int Save(Architecture.Model.Export.ExImShippedConsignmentFollowUps obj)
	{
		try
		{
			if (obj.Id == 0)
			{
				return new Architecture.DAL.Export.ExImShippedConsignmentFollowUps().SetData(obj, "Sp_ExImShippedConsignmentFollowUps_Insert");
			}
			return new Architecture.DAL.Export.ExImShippedConsignmentFollowUps().SetData(obj, "sp_ExImShippedConsignmentFollowUps_Update");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int MultiSave(Architecture.Model.Export.ExImShippedConsignmentFollowUps obj)
	{
		try
		{
			int returnId = 0;
			if (obj.ShippedConsignmentFollowUpList != null && obj.ShippedConsignmentFollowUpList.Count > 0 && obj.Id == 0)
			{
				returnId = new Architecture.DAL.Export.ExImShippedConsignmentFollowUps().SetDataMultiple(obj, "Sp_ExImShippedConsignmentFollowUps_Insert");
			}
			return returnId;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateStatuses(ReportsParameters obj)
	{
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			foreach (ReportsParameters item in obj.ReportsParameterslist)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlcon;
				sqlCommand.Transaction = sqlTrn;
				sqlCommand.CommandType = CommandType.StoredProcedure;
				sqlCommand.CommandText = "[dbo].[USP_ExImShippedConsignmentFollowUps_StatusUpdate]";
				sqlCommand.Parameters.AddWithValue("@Id", item.Id);
				sqlCommand.Parameters.AddWithValue("@ExImInvoiceId", item.InvoiceId);
				sqlCommand.Parameters.AddWithValue("@CurrentStatusId", item.StatusId);
				sqlCommand.Parameters.AddWithValue("@DocumentStatusId", item.DocumentStatusId);
				sqlCommand.Parameters.AddWithValue("@ModifyUserId", item.UserId);
				sqlCommand.ExecuteNonQuery();
			}
			sqlTrn.Commit();
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public static DataTable GetByID(int Id)
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
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImShippedConsignmentFollowUps>("Sp_ExImShippedConsignmentFollowUps_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GetIdByInvoiceId(int InvoiceId)
	{
		int DocId = 0;
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = InvoiceId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetIdByInvoiceId"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImShippedConsignmentFollowUps>("Sp_ExImShippedConsignmentFollowUps_GetAllMethod", p);
			if (dt.Rows.Count > 0)
			{
				DocId = Conversion.ToInt(dt.Rows[0][0]);
			}
			return DocId;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable Getall(Architecture.Model.Export.ExImShippedConsignmentFollowUps obj)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "ReadAll"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImShippedConsignmentFollowUps>("Sp_ExImShippedConsignmentFollowUps_GetAllMethod", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExImShippedConsignmentFollowUps_Slipandregister(ReportsParameters obj)
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
			if (obj.ExImInvoiceId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExImInvoiceId",
					Value = obj.ExImInvoiceId
				});
			}
			if (!string.IsNullOrEmpty(obj.Status))
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentStatusIds",
					Value = obj.Status
				});
			}
			new DataTable();
			return GenericProvider.GetDataTableProc<Architecture.Model.Export.ExImShippedConsignmentFollowUps>("Sp_ExImShippedConsignmentFollowUps_SlipAndRegister_Rpt", p);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataSet ExportPendingShipmentsFollowup(ReportsParameters obj)
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
					ParameterName = "@DestinationPort",
					Value = obj.DestinationPortId
				});
			}
			if (obj.Status != null && obj.Status != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentStatusIds",
					Value = obj.Status
				});
			}
			if (obj.CurrentStatusIds != null && obj.CurrentStatusIds != string.Empty)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@CurrentStatusIds",
					Value = obj.CurrentStatusIds
				});
			}
			if (obj.ActivityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ActivityId",
					Value = obj.ActivityId
				});
			}
			if (obj.StatusId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@StatusId",
					Value = obj.StatusId
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
			return GenericProvider.GetDataSetProc<ReportsParameters>("ExportPendingShipmentsFollowup", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable ExportShipmentFollowUpReoprt(ReportsParameters obj)
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
			if (obj.ActivityId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.ActivityId
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
			if (obj.BankGlDrAc != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ExporterBankId",
					Value = obj.BankGlDrAc
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
			return GenericProvider.GetDataTableProc<ReportsParameters>("SpEximInvoice_ShipmentFollowupHistory_Rpt", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
