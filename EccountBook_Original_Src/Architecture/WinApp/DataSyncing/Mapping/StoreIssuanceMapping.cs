using System;
using System.Collections.Generic;
using System.Data;
using Architecture.BLL.Inventory;
using Architecture.Model.PurchaseTrading;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.WinApp.DataSyncing.Mapping;

public class StoreIssuanceMapping
{
	public static InvGsStoreIssuanceHeader ConvertLocalSaleObjectToTaxSaleObject(InvGsStoreIssuanceHeader Obj)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		try
		{
			DataTable dt = InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany(Obj.OrganizationId, Obj.CompanyId);
			if (dt == null || dt.Rows.Count == 0)
			{
				throw new Exception("Tax company setup not found for the given company.");
			}
			int TaxOrganizationId = Convert.ToInt32(dt.Rows[0]["TaxOrganizationId"]);
			int TaxCompanyId = Convert.ToInt32(dt.Rows[0]["TaxCompanyId"]);
			int TaxFinancialYearId = Convert.ToInt32(dt.Rows[0]["TaxFinancialYearId"]);
			int TaxBranchesId = Convert.ToInt32(dt.Rows[0]["TaxBranchesId"]);
			int TaxDefaultUserId = Convert.ToInt32(dt.Rows[0]["TaxDefaultUserId"]);
			InvGsStoreIssuanceHeader TaxObj = new InvGsStoreIssuanceHeader
			{
				OrganizationId = TaxOrganizationId,
				CompanyId = TaxCompanyId,
				FinancialYearId = TaxFinancialYearId,
				BranchesId = TaxBranchesId,
				ProjectsId = Obj.ProjectsId,
				EntryUser = TaxDefaultUserId,
				ModifyUser = TaxDefaultUserId,
				ApprovedUserId = TaxDefaultUserId,
				EntryDate = DateTime.Now,
				ModifyDate = DateTime.Now,
				ApprovedDate = DateTime.Now,
				IsApproved = false,
				Id = 0,
				DocumentTypeId = 452,
				DocNo = 0,
				DocDate = Obj.DocDate,
				BranchSrNo = 0,
				ManualNo = Obj.ManualNo,
				TypeId = 1,
				Remarks = Obj.Remarks,
				ActionForVoucherId = 1,
				AttachmentsValues = string.Empty,
				CustomAttachmentsValues = string.Empty,
				AttachmentsList = null,
				DeleteAttachmentsList = null
			};
			TaxObj.gsStoreIssuanceDetails = new List<InvGsStoreIssuanceDetail>();
			ConvertDetailEntry(Obj, TaxObj);
			return TaxObj;
		}
		catch (Exception ex)
		{
			throw new Exception("Error converting sale invoice: " + ex.Message, ex);
		}
	}

	private static void ConvertDetailEntry(InvGsStoreIssuanceHeader source, InvGsStoreIssuanceHeader Destination)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		foreach (InvGsStoreIssuanceDetail item in source.gsStoreIssuanceDetails)
		{
			InvGsStoreIssuanceDetail List = new InvGsStoreIssuanceDetail
			{
				Id = 0,
				InvGsStoreIssuanceHeaderId = item.InvGsStoreIssuanceHeaderId,
				SupplierCustomerId = item.SupplierCustomerId,
				EmployeeId = item.EmployeeId,
				Warehouseid = item.Warehouseid,
				ItemId = item.ItemId,
				ItemUomSchId = item.ItemUomSchId,
				IssueQty = item.IssueQty,
				ItemRate = item.ItemRate,
				ItemAmount = item.ItemAmount,
				DepartmentId = item.DepartmentId,
				AssetsId = item.AssetsId,
				IssuanceTypeId = item.IssuanceTypeId,
				DrAccountId = item.DrAccountId,
				ExImInvoiceId = item.ExImInvoiceId,
				LineId = item.LineId,
				ReamarksDetail = item.ReamarksDetail,
				ActionTypeId = 1
			};
			Destination.gsStoreIssuanceDetails.Add(List);
		}
	}
}
