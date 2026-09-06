using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.DAL.pcc;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;

namespace Architecture.BLL.pcc;

public class InvDeliveryOrder
{
	public static int Save(Architecture.Model.pcc.InvDeliveryOrder obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			if (obj.Id == 0)
			{
				obj.ActionId = 1;
				return new Architecture.DAL.pcc.InvDeliveryOrder().SetData(obj, "[pcc].[USP_InvDeliveryOrder_InsertAndUpdate]");
			}
			obj.ActionId = 2;
			return new Architecture.DAL.pcc.InvDeliveryOrder().SetData(obj, "[pcc].[USP_InvDeliveryOrder_InsertAndUpdate]");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static Architecture.Model.pcc.InvDeliveryOrder ReadById(int Id)
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
			new List<Architecture.Model.pcc.InvDeliveryOrder>();
			return new Architecture.DAL.pcc.InvDeliveryOrder().GetData("[pcc].[USP_InvDeliveryOrder_GetAllMethod]", p)[0];
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void DeleteByID(int EntryUserId, int Id)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@EntryUserId",
				Value = EntryUserId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "DeleteById"
			});
			GenericProvider.GetDataTableProc<Architecture.Model.pcc.InvStockTransferHeader>("[pcc].[USP_InvDeliveryOrder_GetAllMethod]", p);
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
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
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
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
			if (obj.VehicleNo != string.Empty && obj.VehicleNo != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VehicleNo",
					Value = obj.VehicleNo
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
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.InventoryParentCategories
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
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
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "FormHistory"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[pcc].[USP_InvDeliveryOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static int GenerateCode(Architecture.Model.pcc.InvDeliveryOrder obj)
	{
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GenerateCode"
			});
			DataTable dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[pcc].[USP_InvDeliveryOrder_GetAllMethod]", p);
			if (dt.Rows.Count > 0 && dt != null)
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

	public static DataTable GetDoBalQtyByOrder(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Id",
				Value = obj.Id
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@SaleOrderDetailId",
				Value = obj.OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = obj.DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetDoBalQtyByOrder"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[pcc].[USP_InvDeliveryOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string DeliveryOrderStackWarningMessage(Architecture.Model.pcc.InvDeliveryOrder obj)
	{
		try
		{
			string WarningMessage = "";
			DataTable dt = new DataTable();
			List<SqlParameter> s = new List<SqlParameter>();
			for (int i = 0; i < obj.InvDeliveryOrderDetailList.Count; i++)
			{
				s.Clear();
				s.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = obj.OrganizationId
				});
				s.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = obj.CompanyId
				});
				if (obj.InvDeliveryOrderDetailList[i].ItemId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@ItemId",
						Value = obj.InvDeliveryOrderDetailList[i].ItemId
					});
				}
				if (!Conversion.CheckDateTimeNull(obj.DocDate))
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@DoDate",
						Value = obj.DocDate
					});
				}
				if (obj.InvDeliveryOrderDetailList[i].WarehouseId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@WarehouseId",
						Value = obj.InvDeliveryOrderDetailList[i].WarehouseId
					});
				}
				if (obj.InvDeliveryOrderDetailList[i].JobLotId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@JobLotId",
						Value = obj.InvDeliveryOrderDetailList[i].JobLotId
					});
				}
				if (obj.InvDeliveryOrderDetailList[i].ItemAttributeVarientId != 0)
				{
					s.Add(new SqlParameter
					{
						ParameterName = "@ItemAttributeVarientId",
						Value = obj.InvDeliveryOrderDetailList[i].ItemAttributeVarientId
					});
				}
				s.Add(new SqlParameter
				{
					ParameterName = "@ItemQty",
					Value = obj.InvDeliveryOrderDetailList[i].DoQty
				});
				dt = GenericProvider.GetDataTableProc<Architecture.Model.Inventory.SaleOrder>("[pcc].[USP_DeliveryOrderStackWarningMessage]", s);
				if (dt.Rows.Count > 0)
				{
					WarningMessage = Conversion.ToString(dt.Rows[0]["WarningMessage"]);
					break;
				}
			}
			return WarningMessage;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static DataTable GetDataForDropDownFromDeliveryOrder(ReportsParameters obj)
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
			if (obj.Activity != string.Empty || obj.Activity != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@Activity",
					Value = obj.Activity
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[pcc].[USP_GetDataForDropDownFromDeliveryOrder]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrder_SlipConcrete(int OrganizationId, int CompanyId, int Id)
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
				ParameterName = "@Id",
				Value = Id
			});
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[pcc].[USP_InvDeliveryOrder_Slip]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable DeliveryOrderRegister(ReportsParameters obj)
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
				ParameterName = "@BranchesId",
				Value = obj.BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@FinancialYearId",
				Value = obj.FinancialYearId
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
					ParameterName = "@Todate",
					Value = obj.ToDate
				});
			}
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
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
			if (obj.ApprovedFilter != "All")
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsApproved",
					Value = obj.IsApproved
				});
			}
			if (obj.DocumentTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@DocumentTypeId",
					Value = obj.DocumentTypeId
				});
			}
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.InventoryParentCategories
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.ActionId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@IsFOC",
					Value = obj.ActionId
				});
			}
			return GenericProvider.GetDataTableProc<Architecture.Model.pcc.SaleOrder>("[pcc].[USP_DeliveryOrderRegister]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDeliveryOrderForApproval(ReportsParameters obj)
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
			if (obj.FromDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@FromDocNo",
					Value = obj.FromDocNo
				});
			}
			if (obj.ToDocNo != 0.0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ToDocNo",
					Value = obj.ToDocNo
				});
			}
			if (obj.VehicleNo != string.Empty && obj.VehicleNo != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@VehicleNo",
					Value = obj.VehicleNo
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
			if (obj.ReferencePartyName != string.Empty && obj.ReferencePartyName != null)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ReferenceParty",
					Value = obj.ReferencePartyName
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
			if (obj.ItemCategoryId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemCategoryId",
					Value = obj.ItemCategoryId
				});
			}
			if (obj.ItemTypeId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@ItemTypeId",
					Value = obj.ItemTypeId
				});
			}
			if (obj.InventoryParentCategories != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@InventoryParentCategories",
					Value = obj.InventoryParentCategories
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
			if (obj.PendingForView != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@PendingForView",
					Value = obj.PendingForView
				});
			}
			return GenericProvider.GetDataTableProc<DeliverySchedule>("[pcc].[USP_InvDeliveryOrder_ApprovalHistory]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void UpdateDelivertOrderStatusById(ApprovalList obj)
	{
		try
		{
			foreach (ApprovalList item in obj.AllApprovalLists)
			{
				List<SqlParameter> p = new List<SqlParameter>();
				p.Add(new SqlParameter
				{
					ParameterName = "@OrganizationId",
					Value = item.OrganizationId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@CompanyId",
					Value = item.CompanyId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@BranchesId",
					Value = item.BranchesId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@FinancialYearId",
					Value = item.FinancialYearId
				});
				p.Add(new SqlParameter
				{
					ParameterName = "@ApprovedUserId",
					Value = item.EntryUser
				});
				if (item.ReqType != string.Empty)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@ReqType",
						Value = item.ReqType
					});
				}
				if (item.Id != 0)
				{
					p.Add(new SqlParameter
					{
						ParameterName = "@Id",
						Value = item.Id
					});
				}
				GenericProvider.GetDataTableProc<Architecture.Model.pcc.InvDeliveryOrder>("[pcc].[USP_UpdateDeliveryOrderStatusById]", p);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable OutstandingDeliveryorderForDispatch(int OrganizationId, int CompanyId, int BranchesId, int DocumentTypeId)
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
				ParameterName = "@BranchesId",
				Value = BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[pcc].[USP_OutstandingDeliveryorderForDispatch]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetSupplierCustomerFromDeliveryOrder(int OrganizationId, int CompanyId, int BranchesId, int DocumentTypeId)
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
				ParameterName = "@BranchesId",
				Value = BranchesId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DocumentTypeId",
				Value = DocumentTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetSupplierCustomerFromDeliveryOrder"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[pcc].[USP_InvDeliveryOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetVendorsAndCustomersFromDeliveryOrder(int OrganizationId, int CompanyId, int PartyTypeId)
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
				ParameterName = "@PartyTypeId",
				Value = PartyTypeId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@Activity",
				Value = "GetVendorsAndCustomersFromDeliveryOrder"
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.Inventory.InvGrn>("[pcc].[USP_InvDeliveryOrder_GetAllMethod]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetPendingCustomerAndOrderForGDN(int OrganizationId, int CompanyId, int DoId, int RecId = 0)
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
				ParameterName = "@DoId",
				Value = DoId
			});
			if (RecId != 0)
			{
				p.Add(new SqlParameter
				{
					ParameterName = "@RecId",
					Value = RecId
				});
			}
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[pcc].[USP_GetPendingCustomerAndOrderForGDN]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static DataTable GetDeliveryOrderDataByPartyDoAndOrderId(int OrganizationId, int CompanyId, int SupplierCustomerId, int OrderId, int DoId)
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
				ParameterName = "@SupplierCustomerId",
				Value = SupplierCustomerId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@OrderId",
				Value = OrderId
			});
			p.Add(new SqlParameter
			{
				ParameterName = "@DoId",
				Value = DoId
			});
			return GenericProvider.GetDataTableProc<SupplierCustomer>("[pcc].[USP_GetDeliveryOrderDataByPartyDoAndOrderId]", p);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
