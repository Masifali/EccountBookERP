using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Architecture.DAL.Common;
using Architecture.Model.Production;

namespace Architecture.DAL.Production;

public class InvManufacturingBOMHeader
{
	public int SetData(Architecture.Model.Production.InvManufacturingBOMHeader obj, string ProcName)
	{
		int returnID = 0;
		using SqlConnection sqlcon = ConnectionObject.SQL();
		sqlcon.Open();
		SqlTransaction sqlTrn = sqlcon.BeginTransaction();
		try
		{
			returnID = Convert.ToInt32(GenericProvider.SetProc(sqlTrn, obj, ProcName));
			if (returnID > 0)
			{
				obj.Id = Convert.ToInt32(returnID);
			}
			else
			{
				returnID = obj.Id;
			}
			foreach (InvManufacturingBOMMaterialDetail item4 in obj.InvManufacturingBOMMaterialsDetails)
			{
				item4.InvManufacturingBOMHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item4, "Sp_InvManufacturingBOMMaterialDetail_Insert"));
			}
			foreach (InvManufacturingBOMBiProductDetail item3 in obj.InvManufacturingBOMProductsDetails)
			{
				item3.InvManufacturingBOMHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item3, "Sp_InvManufacturingBOMBiProductDetail_Insert"));
			}
			foreach (InvManufacturingBOMOverHeadsDetail item2 in obj.InvManufacturingOverHeadDetails)
			{
				item2.InvManufacturingBOMHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item2, "Sp_InvManufacturingBOMOverHeadsDetail_Insert"));
			}
			foreach (ManufacturingBOMPackingMaterialDetail item in obj.ManufacturingBOMPackingMaterialDetail)
			{
				item.ManufacturingBOMHeaderId = obj.Id;
				Convert.ToInt32(GenericProvider.SetProc(sqlTrn, item, "USp_ManufacturingBOMPackingMaterialDetail_Insert"));
			}
			sqlTrn.Commit();
			return returnID;
		}
		catch (Exception ex)
		{
			sqlTrn.Rollback();
			throw ex;
		}
	}

	public List<Architecture.Model.Production.InvManufacturingBOMHeader> GetDate(string ProcName, List<SqlParameter> p = null)
	{
		List<Architecture.Model.Production.InvManufacturingBOMHeader> lst = new List<Architecture.Model.Production.InvManufacturingBOMHeader>();
		try
		{
			using (SqlConnection sqlConnection = ConnectionObject.SQL())
			{
				sqlConnection.Open();
				lst = GenericProvider.GetProc<Architecture.Model.Production.InvManufacturingBOMHeader>(sqlConnection, ProcName, p);
			}
			using (SqlConnection sqlConnection2 = ConnectionObject.SQL())
			{
				sqlConnection2.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvManufacturingBOMHeader item4 in lst)
					{
						List<SqlParameter> lstPrm4 = new List<SqlParameter>();
						lstPrm4.Add(new SqlParameter("@Id", item4.Id));
						lstPrm4.Add(new SqlParameter("@Activity", "BOMMaterialDetailReadByHeaderId"));
						item4.InvManufacturingBOMMaterialsDetails = GenericProvider.Get<InvManufacturingBOMMaterialDetail>(sqlConnection2, "Sp_InvManufacturingBOMHeader_GetAllMethod", lstPrm4);
					}
				}
			}
			using (SqlConnection sqlConnection3 = ConnectionObject.SQL())
			{
				sqlConnection3.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvManufacturingBOMHeader item3 in lst)
					{
						List<SqlParameter> lstPrm3 = new List<SqlParameter>();
						lstPrm3.Add(new SqlParameter("@Id", item3.Id));
						lstPrm3.Add(new SqlParameter("@Activity", "BOMBiProductDetailReadById"));
						item3.InvManufacturingBOMProductsDetails = GenericProvider.Get<InvManufacturingBOMBiProductDetail>(sqlConnection3, "Sp_InvManufacturingBOMHeader_GetAllMethod", lstPrm3);
					}
				}
			}
			using (SqlConnection sqlConnection4 = ConnectionObject.SQL())
			{
				sqlConnection4.Open();
				if (lst != null)
				{
					foreach (Architecture.Model.Production.InvManufacturingBOMHeader item2 in lst)
					{
						List<SqlParameter> lstPrm2 = new List<SqlParameter>();
						lstPrm2.Add(new SqlParameter("@Id", item2.Id));
						lstPrm2.Add(new SqlParameter("@Activity", "BOMOverHeadsDetailReadByHeaderId"));
						item2.InvManufacturingOverHeadDetails = GenericProvider.Get<InvManufacturingBOMOverHeadsDetail>(sqlConnection4, "Sp_InvManufacturingBOMHeader_GetAllMethod", lstPrm2);
					}
				}
			}
			using SqlConnection sqlcon = ConnectionObject.SQL();
			sqlcon.Open();
			if (lst != null)
			{
				foreach (Architecture.Model.Production.InvManufacturingBOMHeader item in lst)
				{
					List<SqlParameter> lstPrm = new List<SqlParameter>();
					lstPrm.Add(new SqlParameter("@Id", item.Id));
					lstPrm.Add(new SqlParameter("@Activity", "BOMPackingMaterialDetailReadById"));
					item.ManufacturingBOMPackingMaterialDetail = GenericProvider.Get<ManufacturingBOMPackingMaterialDetail>(sqlcon, "Sp_InvManufacturingBOMHeader_GetAllMethod", lstPrm);
				}
			}
		}
		catch
		{
			throw;
		}
		return lst;
	}
}
