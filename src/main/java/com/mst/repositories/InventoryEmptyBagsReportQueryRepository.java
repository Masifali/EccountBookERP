package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.Item;
import com.mst.projections.InventoryEbBalancesByPartyAndItemRow;
import com.mst.projections.InventoryEbGlBySupplierAndItemRow;

/**
 * Query-only repository for the empty-bags (EB) inventory reports:
 * Sp_InventoryEBBalancesByPartyAndItem_Rpt and Sp_InventoryEBGLBySupplierandItem_Rpt.
 * Bound to the existing Item entity only to satisfy Spring Data; no entity methods are exposed.
 */
@Repository
public interface InventoryEmptyBagsReportQueryRepository extends org.springframework.data.repository.Repository<Item, Integer> {

    /**
     * Sp_InventoryEBBalancesByPartyAndItem_Rpt (single branch).
     * The procedure's SELECT ... INTO #Data is rewritten as the CTE "Data"; the final SELECT r.* is expanded to the
     * six #Data columns and joined to Company / Organization exactly as in the procedure.
     * Proc defaults: every parameter except organizationId is NULL (optional filter). @BagsConditionId and @TypeId
     * are declared by the procedure but never used, so they are not parameters here.
     * Note: the procedure uses gh.CompanyId = @CompanyId (not an optional filter), so companyId must be supplied.
     */
    @Query(value = "WITH Data AS ("
         + " select sp.CompanyName [Supplier_Name], i.ItemName [Item_Name],"
         + " sum(a.oBalance) OBalance, sum(a.Received) Received, sum(a.Issued) Issued,"
         + " sum(a.oBalance) + Sum(a.Received) - sum(a.Issued) CBalance"
         + " from ("
         + "  select a.SupplierCustomerId, a.ItemId, sum(a.oBalance) oBalance, 0 As Received, 0 As Issued"
         + "  from ("
         + "   select gh.SupplierCustomerId, geb.ItemId, -geb.ReceivedQty as oBalance"
         + "   from InvGrnDetailEmptyBags geb inner join InvGrn gh on geb.InvGrnId = gh.Id"
         + "   where gh.OrganizationId = :organizationId and gh.CompanyId = :companyId"
         + "    and (:branchesId is null or gh.BranchesId = :branchesId)"
         + "    and (:fromDate is null or gh.DocDate < :fromDate)"
         + "    and (:supplierCustomerId is null or gh.SupplierCustomerId = :supplierCustomerId)"
         + "    and (:itemId is null or geb.ItemId = :itemId)"
         + "    and (gh.DocumentTypeId = 46)"
         + "    and (geb.TypeId <> 5)"
         + "   union all"
         + "   select gh.SupplierCustomerId, geb.ItemId, -geb.ReceivedQty as oBalance"
         + "   from InvPurchaseInvoiceEmptyBags geb inner join InvPurchaseInvoice gh on geb.InvPurchaseInvoiceId = gh.Id"
         + "   where gh.OrganizationId = :organizationId and gh.CompanyId = :companyId"
         + "    and (:branchesId is null or gh.BranchesId = :branchesId)"
         + "    and (:fromDate is null or gh.DocDate < :fromDate)"
         + "    and (:supplierCustomerId is null or gh.SupplierCustomerId = :supplierCustomerId)"
         + "    and (:itemId is null or geb.ItemId = :itemId)"
         + "    and (gh.DocumentTypeId = 57)"
         + "   union all"
         + "   select gh.SupplierCustomerId, geb.ItemId, -geb.ItemQty as oBalance"
         + "   from InvGrnDetail geb inner join InvGrn gh on geb.InvGrnId = gh.Id"
         + "   where gh.OrganizationId = :organizationId and gh.CompanyId = :companyId"
         + "    and (:branchesId is null or gh.BranchesId = :branchesId)"
         + "    and (:fromDate is null or gh.DocDate < :fromDate)"
         + "    and (:supplierCustomerId is null or gh.SupplierCustomerId = :supplierCustomerId)"
         + "    and (:itemId is null or geb.ItemId = :itemId)"
         + "    and (gh.DocumentTypeId = 48)"
         + "    and (gh.InwardGatePassId > 0)"
         + "   union all"
         + "   Select d.SupplierIdTo, d.ItemId, -d.ItemQty"
         + "   from InvPmStockWithPartiesTransferHeader h"
         + "    inner join InvPmStockWithPartiesTransferDetail d on h.Id = d.InvPmStockWithPartiesTransferHeaderId"
         + "   where h.OrganizationId = :organizationId"
         + "    and h.CompanyId = :companyId"
         + "    and (:fromDate is null or h.DocDate < :fromDate)"
         + "    and (:supplierCustomerId is null or d.SupplierIdTo = :supplierCustomerId)"
         + "    and (:itemId is null or d.ItemId = :itemId)"
         + "    and d.SupplierIdTo > 0"
         + "   union all"
         + "   Select d.SupplierCustomerId, d.ItemId, d.IssueQty"
         + "   from InvGsStoreIssuanceHeader h"
         + "    inner join InvGsStoreIssuanceDetail d on h.Id = d.InvGsStoreIssuanceHeaderId and d.ActionTypeId <> 3"
         + "   where h.OrganizationId = :organizationId and h.CompanyId = :companyId"
         + "    and (:branchesId is null or h.BranchesId = :branchesId)"
         + "    and (:fromDate is null or h.DocDate < :fromDate) and h.ActionId <> 3"
         + "    and (:supplierCustomerId is null or d.SupplierCustomerId = :supplierCustomerId)"
         + "    and (:itemId is null or d.ItemId = :itemId)"
         + "   union all"
         + "   Select d.SupplierIdFrom, d.ItemId, d.ItemQty"
         + "   from InvPmStockWithPartiesTransferHeader h"
         + "    inner join InvPmStockWithPartiesTransferDetail d on h.Id = d.InvPmStockWithPartiesTransferHeaderId"
         + "   where h.OrganizationId = :organizationId"
         + "    and h.CompanyId = :companyId"
         + "    and (:fromDate is null or h.DocDate < :fromDate)"
         + "    and (:supplierCustomerId is null or d.SupplierIdFrom = :supplierCustomerId)"
         + "    and (:itemId is null or d.ItemId = :itemId)"
         + "    and d.SupplierIdFrom > 0"
         + "  ) a Group By a.ItemId, a.SupplierCustomerId"
         + "  union all"
         + "  select gh.SupplierCustomerId, geb.ItemId, 0, geb.ReceivedQty, 0"
         + "  from InvGrnDetailEmptyBags geb inner join InvGrn gh on geb.InvGrnId = gh.Id"
         + "  where gh.OrganizationId = :organizationId and gh.CompanyId = :companyId"
         + "   and (:branchesId is null or gh.BranchesId = :branchesId)"
         + "   and (:fromDate is null or gh.DocDate >= :fromDate)"
         + "   and (:toDate is null or gh.DocDate <= :toDate)"
         + "   and (:supplierCustomerId is null or gh.SupplierCustomerId = :supplierCustomerId)"
         + "   and (:itemId is null or geb.ItemId = :itemId)"
         + "   and (gh.DocumentTypeId = 46)"
         + "   and (geb.TypeId <> 5)"
         + "  union all"
         + "  select gh.SupplierCustomerId, geb.ItemId, 0, geb.ReceivedQty, 0"
         + "  from InvPurchaseInvoiceEmptyBags geb"
         + "   inner join InvPurchaseInvoice gh on geb.InvPurchaseInvoiceId = gh.Id"
         + "  where gh.OrganizationId = :organizationId and gh.CompanyId = :companyId"
         + "   and (:branchesId is null or gh.BranchesId = :branchesId)"
         + "   and (:fromDate is null or gh.DocDate >= :fromDate)"
         + "   and (:toDate is null or gh.DocDate <= :toDate)"
         + "   and (:supplierCustomerId is null or gh.SupplierCustomerId = :supplierCustomerId)"
         + "   and (:itemId is null or geb.ItemId = :itemId)"
         + "   and (gh.DocumentTypeId = 57)"
         + "  union all"
         + "  select gh.SupplierCustomerId, geb.ItemId, 0, geb.ItemQty, 0"
         + "  from InvGrnDetail geb inner join InvGrn gh on geb.InvGrnId = gh.Id"
         + "  where gh.OrganizationId = :organizationId and gh.CompanyId = :companyId"
         + "   and (:branchesId is null or gh.BranchesId = :branchesId)"
         + "   and (:fromDate is null or gh.DocDate >= :fromDate)"
         + "   and (:toDate is null or gh.DocDate <= :toDate)"
         + "   and (:supplierCustomerId is null or gh.SupplierCustomerId = :supplierCustomerId)"
         + "   and (:itemId is null or geb.ItemId = :itemId)"
         + "   and (gh.DocumentTypeId = 48)"
         + "   and (gh.InwardGatePassId > 0)"
         + "  union all"
         + "  select d.SupplierIdTo, d.ItemId, 0, d.ItemQty, 0"
         + "  from InvPmStockWithPartiesTransferDetail d"
         + "   inner join InvPmStockWithPartiesTransferHeader h on d.InvPmStockWithPartiesTransferHeaderId = h.Id"
         + "  where h.OrganizationId = :organizationId"
         + "   and h.CompanyId = :companyId"
         + "   and (:fromDate is null or h.DocDate >= :fromDate)"
         + "   and (:toDate is null or h.DocDate <= :toDate)"
         + "   and (:supplierCustomerId is null or d.SupplierIdTo = :supplierCustomerId)"
         + "   and (:itemId is null or d.ItemId = :itemId)"
         + "   and d.SupplierIdTo > 0"
         + "  union all"
         + "  Select d.SupplierCustomerId, d.ItemId, 0, 0, d.IssueQty"
         + "  from InvGsStoreIssuanceHeader h"
         + "   inner join InvGsStoreIssuanceDetail d on h.Id = d.InvGsStoreIssuanceHeaderId"
         + "   inner join InvDeliveryOrder doh on h.RefDocNoId = doh.Id"
         + "  where h.OrganizationId = :organizationId"
         + "   and h.CompanyId = :companyId"
         + "   and (:branchesId is null or h.BranchesId = :branchesId)"
         + "   and (:fromDate is null or h.DocDate >= :fromDate)"
         + "   and (:toDate is null or h.DocDate <= :toDate)"
         + "   and (:supplierCustomerId is null or d.SupplierCustomerId = :supplierCustomerId)"
         + "   and (:itemId is null or d.ItemId = :itemId)"
         + "   and (doh.DocumentTypeId = 85)"
         + "  union all"
         + "  select d.SupplierIdFrom, d.ItemId, 0, 0, d.ItemQty"
         + "  from InvPmStockWithPartiesTransferDetail d"
         + "   inner join InvPmStockWithPartiesTransferHeader h on d.InvPmStockWithPartiesTransferHeaderId = h.Id"
         + "  where h.OrganizationId = :organizationId"
         + "   and h.CompanyId = :companyId"
         + "   and (:fromDate is null or h.DocDate >= :fromDate)"
         + "   and (:toDate is null or h.DocDate <= :toDate)"
         + "   and (:supplierCustomerId is null or d.SupplierIdFrom = :supplierCustomerId)"
         + "   and (:itemId is null or d.ItemId = :itemId)"
         + "   and d.SupplierIdFrom > 0"
         + " ) a inner join SupplierCustomer sp on a.SupplierCustomerId = sp.Id"
         + "  inner join item i on a.ItemId = i.Id"
         + " Group By sp.CompanyName, i.ItemName"
         + " having ((sum(a.oBalance) + Sum(a.Received)) - sum(a.Issued) <> 0)"
         + ") "
         + "SELECT r.Supplier_Name, r.Item_Name, r.OBalance, r.Received, r.Issued, r.CBalance"
         + ", co.CompCountry, co.CompContactPerson, co.CompMobileA, co.CompMobileB,"
         + " co.CompMobileC, co.CompEmailA, co.CompEmailB, co.CompLogoImage,"
         + " co.CompanyWebsite, co.CompanyFaxNo, org.ReportingRemarks OrgReportingRemarks "
         + "FROM Data r"
         + " Inner join Company co on co.Id = :companyId"
         + " Inner join Organization org on org.Id = co.OrgCompanyTypeId",
         nativeQuery = true)
    List<InventoryEbBalancesByPartyAndItemRow> ebBalancesByPartyAndItem(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("fromDate") LocalDateTime fromDate,
            @Param("toDate") LocalDateTime toDate,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("itemId") Integer itemId);

    /**
     * Sp_InventoryEBGLBySupplierandItem_Rpt (single branch).
     * // KEEP-EXEC: @FromDate is defaulted from FinancialYear.Start_Period when NULL, the opening row depends on an
     * IF EXISTS(#SubData) / ELSE control-flow branch, and the pipeline uses four temp tables (#EbTypes, #EbCondition,
     * #SubData, #AllData) with a windowed running balance over the merged rows.
     * Proc defaults: every parameter except organizationId is NULL. @BagsConditionId/@TypeId filter only the GRN
     * (46) and purchase-invoice (57) empty-bag rows as in the procedure.
     */
    @Query(value = "EXEC dbo.Sp_InventoryEBGLBySupplierandItem_Rpt"
         + " @OrganizationId = :organizationId, @CompanyId = :companyId, @BranchesId = :branchesId,"
         + " @FromDate = :fromDate, @ToDate = :toDate, @SupplierCustomerId = :supplierCustomerId,"
         + " @ItemId = :itemId, @TypeId = :typeId, @BagsConditionId = :bagsConditionId",
         nativeQuery = true)
    List<InventoryEbGlBySupplierAndItemRow> ebGlBySupplierAndItem(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("branchesId") Integer branchesId,
            @Param("fromDate") LocalDateTime fromDate,
            @Param("toDate") LocalDateTime toDate,
            @Param("supplierCustomerId") Integer supplierCustomerId,
            @Param("itemId") Integer itemId,
            @Param("typeId") Integer typeId,
            @Param("bagsConditionId") Integer bagsConditionId);
}
