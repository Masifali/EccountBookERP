package com.mst.repositories;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.projections.ExpenseDetailFromPoAndPreBillRow;
import com.mst.projections.PackingMaterialRequirementDetailRow;
import com.mst.projections.PackingMaterialRequirementSummaryRow;
import com.mst.projections.PurchaseOrderPackingMaterialRptRow;
import com.mst.projections.PurchaseOrderSlipRptRow;

/**
 * Query-only repository (reports / lookups) of batch E_purchase_order, bound to the owned PurchaseOrder entity:
 * Sp_PurchaseOrderSlip_Rpt, Sp_PurchaseOrder_PackingMaterial_Rpt, usp_PackingMaterialRequirementPlanning,
 * USP_GetExpenseDetailFromPOAndPreBill.
 */
@Repository
public interface PurchaseOrderReportQueryRepository
        extends org.springframework.data.repository.Repository<com.mst.models.PurchaseOrder, Integer> {

    /**
     * Sp_PurchaseOrderSlip_Rpt (single branch).
     * Native: config values 77/78/8 (dbo.GetConfigurationByConfigId), V_UomScheduleAndUom view, InvDueTerms /
     * InvOrderCategory / Organization without entity, CONVERT(datetime, .., 102). The #TmpResult pipeline (base select +
     * 3 UPDATEs: first DMS attachment / count, GRN received qty per order, rejected gate-pass qty per order) is rewritten as
     * one statement with LEFT JOINs; ISNULL keeps the temp-table defaults (0 / ''). Defaults: all filters NULL (= no filter).
     */
    @Query(value = "WITH Cfg AS ( "
         + "SELECT CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 77) AS int) AS DecimalsForRate, "
         + "       CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 78) AS int) AS DecimalsForAmount, "
         + "       CAST(dbo.GetConfigurationByConfigId(:organizationId, :companyId, 8) AS nvarchar(max)) AS AttachementPath "
         + "), R AS ( "
         + "SELECT PoH.Id, PoH.DocumentTypeId, Dtp.DocumentTypeCode, Dtp.DocumentTypeDescription, PoH.DocNo, "
         + "convert(datetime, PoH.DocDate, 102) as DocDate, PoH.OrderCatagoryId, Oct.OrderCategoryName, Oct.OrderCategoryDescription, "
         + "PoH.CatagorySrNo, PoH.OrderSupCustId, Sp.CompanyName AS SupplierName, PoH.SupplierRefNo, PoH.RefrenenceParty, "
         + "SpRp.CompanyName AS CompanyNameSpRp, PoH.SupplierCustomerIdStockParty, SpStk.CompanyName AS CompanyNameSpStk, "
         + "PoH.BrokerAgentSupCustId, SpCagent.CompanyName AS CompanyNameSpCAgent, PoH.CommissionType, PoH.CommRate, "
         + "PoH.UomScheduleIdCmRate, PoH.CommAmount, PoH.CommissionRemarks, PoH.PaymentTermsId, InvDueTerms.TermsCode, "
         + "PoH.BrokerAgentId, poh.BrokeryRate, poh.BrokeryType, poh.BrokeryUom, poh.BrokeryAmount, bk.CompanyName as BrokerName, "
         + "InvDueTerms.TermsDescription, PoH.OrderDueDays, PoH.OrderDueDate, PoH.OrderExpiryDate, PoH.DeliveryTerm, "
         + "PoH.DeliveryStartDate, PoH.DeliveryDays, PoH.DeliveryRemarks, PoH.DeliveryTermId, DDT.Description AS DeliveryTermDB, "
         + "PoH.OrderTaxable, PoH.RemarksHeader, PoH.OrderStatus, "
         + "case when IsAproved = 0 then 'Not Approved' else 'Approved' End IsApproved, "
         + "PoH.EntryUser, Eusr.UserName AS UserNameEusr, PoH.EntryDate, PoH.ModifyUser, Musr.UserName AS UserNameMusr, PoH.ModifyDate, "
         + "PoH.PostUser, Ausr.UserName AS UserNameAusr, PoH.PostDate, PoH.PostState, PoH.OrganizationId, PoH.CompanyId, PoH.BranchesId, "
         + "PoH.ProjectsId, PoD.Id AS PoDId, Cfg.DecimalsForRate as DecimalsForRate, Cfg.DecimalsForAmount as DecimalsForAmount, "
         + "PoD.PurchaseOrderId, PoD.OrderItemId, Item.ItemCode, Item.ItemName, Item.BaseUnitId, Item.ItemCategoryId, Item.ItemTypeId, "
         + "Item.ItemClassId, PoD.OrderItemUOMId, PoD.Moisture, PoD.OrderItemQty, ItmUom.UOMCode AS UOMCodeItm, PoD.Crop, PoD.NetWeight, "
         + "PoD.OrderItemRate, PoD.OrderItemRateUOMId, UomSchRate.UOMCode AS UOMCodeRate, UomSchRate.Equivalent AS EquivalentRate, "
         + "PoD.Amount, PoD.JobLotId, PoD.CityArea, PoD.LabSampleNo, PoD.AmountCalcType, PoD.TaxableStatus, PoD.OrderRemarks, "
         + "JobLot.JobLotCode, JobLot.JobLotDescription, PoD.BagWeight, PoD.BagPrice, ItmUom.Equivalent, PoH.OrderType, poh.BookingPersonId, "
         + "Cfg.AttachementPath AS CfgAttachementPath "
         + "FROM PurchaseOrderDetail AS PoD "
         + "INNER JOIN PurchaseOrder AS PoH ON PoD.PurchaseOrderId = PoH.Id "
         + "INNER JOIN SupplierCustomer AS Sp ON PoH.OrderSupCustId = Sp.Id "
         + "INNER JOIN Item ON PoD.OrderItemId = Item.Id "
         + "INNER JOIN Company cmp ON cmp.Id = :companyId "
         + "CROSS JOIN Cfg "
         + "LEFT JOIN JobLot ON PoD.JobLotId = JobLot.Id "
         + "LEFT JOIN V_UomScheduleAndUom AS UomSchRate ON PoD.OrderItemRateUOMId = UomSchRate.Id "
         + "LEFT JOIN V_UomScheduleAndUom AS ItmUom ON PoD.OrderItemUOMId = ItmUom.Id "
         + "LEFT JOIN DocumentType AS Dtp ON PoH.DocumentTypeId = Dtp.Id "
         + "LEFT JOIN UserAccount AS Ausr ON PoH.PostUser = Ausr.ID "
         + "LEFT JOIN UserAccount AS Musr ON PoH.ModifyUser = Musr.ID "
         + "LEFT JOIN UserAccount AS Eusr ON PoH.EntryUser = Eusr.ID "
         + "LEFT JOIN InvDueTerms ON PoH.PaymentTermsId = InvDueTerms.Id "
         + "LEFT JOIN SupplierCustomer AS SpCagent ON PoH.BrokerAgentSupCustId = SpCagent.Id "
         + "LEFT JOIN SupplierCustomer AS SpStk ON PoH.SupplierCustomerIdStockParty = SpStk.Id "
         + "LEFT JOIN SupplierCustomer AS SpRp ON PoH.RefrenenceParty = SpRp.Id "
         + "LEFT JOIN InvOrderCategory AS Oct ON PoH.OrderCatagoryId = Oct.Id "
         + "LEFT JOIN SupplierCustomer bk ON poh.BrokerAgentId = bk.Id "
         + "LEFT JOIN DeliveryTerm DDT ON DDT.Id = Poh.DeliveryTermId "
         + "WHERE (PoH.OrganizationId = :organizationId) "
         + "AND (:companyId IS NULL OR PoH.CompanyId = :companyId) "
         + "AND (:orderId IS NULL OR Poh.id = :orderId) "
         + "AND (:startOrderDate IS NULL OR PoH.DocDate >= CONVERT(date, :startOrderDate)) "
         + "AND (:endOrderDate IS NULL OR PoH.DocDate <= CONVERT(date, :endOrderDate)) "
         + "AND (:poSrFrom IS NULL OR poh.DocNo >= :poSrFrom) "
         + "AND (:poSrTo IS NULL OR poh.DocNo <= :poSrTo) "
         + "AND (:supplierCustomerId IS NULL OR Poh.OrderSupCustId = :supplierCustomerId) "
         + "AND (:itemId IS NULL OR PoD.OrderItemId = :itemId) "
         + "AND (:isApproved IS NULL OR poh.IsAproved = :isApproved) "
         + "AND (:status IS NULL OR poh.OrderStatus = :status) "
         + "AND (:documentTypeId IS NULL OR PoH.DocumentTypeId = :documentTypeId) "
         + ") "
         + "SELECT R.Id, R.DocumentTypeId, R.DocumentTypeCode, R.DocumentTypeDescription, R.DocNo, R.DocDate, R.OrderCatagoryId, "
         + "R.OrderCategoryName, R.OrderCategoryDescription, R.CatagorySrNo, R.OrderSupCustId, R.SupplierName, R.SupplierRefNo, "
         + "R.RefrenenceParty, R.CompanyNameSpRp, R.SupplierCustomerIdStockParty, R.CompanyNameSpStk, R.BrokerAgentSupCustId, "
         + "R.CompanyNameSpCAgent, R.CommissionType, R.CommRate, R.UomScheduleIdCmRate, R.CommAmount, R.CommissionRemarks, "
         + "R.PaymentTermsId, R.TermsCode, R.BrokerAgentId, R.BrokeryRate, R.BrokeryType, R.BrokeryUom, R.BrokeryAmount, R.BrokerName, "
         + "R.TermsDescription, R.OrderDueDays, R.OrderDueDate, R.OrderExpiryDate, R.DeliveryTerm, R.DeliveryTermDB, R.DeliveryStartDate, "
         + "R.DeliveryDays, R.DeliveryRemarks, R.OrderTaxable, R.RemarksHeader, R.OrderStatus, R.IsApproved, R.EntryUser, R.UserNameEusr, "
         + "R.EntryDate, R.ModifyUser, R.UserNameMusr, R.ModifyDate, R.PostUser, R.UserNameAusr, R.PostDate, R.PostState, R.OrganizationId, "
         + "R.CompanyId, R.BranchesId, R.ProjectsId, R.PoDId, R.DecimalsForRate, R.DecimalsForAmount, R.PurchaseOrderId, R.OrderItemId, "
         + "R.ItemCode, R.ItemName, R.BaseUnitId, R.ItemCategoryId, R.ItemTypeId, R.ItemClassId, R.OrderItemUOMId, R.Moisture, R.OrderItemQty, "
         + "ISNULL(RQ.ReceivedQty, CAST(0 AS FLOAT)) AS ReceivedQty, "
         + "R.OrderItemQty - ISNULL(RQ.ReceivedQty, CAST(0 AS FLOAT)) AS BalQty, "
         + "ISNULL(RJ.RejQty, 0) AS RejQty, "
         + "R.UOMCodeItm, R.Crop, R.NetWeight, R.OrderItemRate, R.OrderItemRateUOMId, R.UOMCodeRate, R.EquivalentRate, R.Amount, R.JobLotId, "
         + "R.CityArea, R.LabSampleNo, R.AmountCalcType, R.TaxableStatus, R.OrderRemarks, R.JobLotCode, R.JobLotDescription, R.BagWeight, "
         + "R.BagPrice, R.Equivalent, R.OrderType, "
         + "ISNULL(A.NoOfAttachments, 0) AS NoOfAttachments, "
         + "CASE WHEN A.RowNum = 1 THEN R.CfgAttachementPath + '\\' + A.Attachment ELSE CAST('' AS NVARCHAR(MAX)) END AS AttachementPath, "
         + "co.CompCountry, co.CompContactPerson, co.CompMobileA, co.CompMobileB, co.CompMobileC, co.CompEmailA, co.CompEmailB, co.CompLogoImage, "
         + "co.CompanyWebsite, co.CompanyFaxNo, org.ReportingRemarks OrgReportingRemarks, r.BookingPersonId, bp.CompanyName as BookingPersonName "
         + "FROM R r "
         + "INNER JOIN Company co ON co.Id = :companyId "
         + "INNER JOIN Organization org ON org.Id = co.OrgCompanyTypeId "
         + "LEFT JOIN SupplierCustomer bp ON bp.Id = r.BookingPersonId "
         + "LEFT JOIN (SELECT d.PurchaseOrderId, SUM(d.ItemQty) AS ReceivedQty FROM InvGrnDetail d GROUP BY d.PurchaseOrderId) RQ "
         + "   ON RQ.PurchaseOrderId = r.Id "
         + "LEFT JOIN (SELECT d.PurchaseOrderId, SUM(d.NoOfPackages) AS RejQty FROM GatePassInward d WHERE d.Status = 'Rejected' "
         + "   GROUP BY d.PurchaseOrderId) RJ ON RJ.PurchaseOrderId = r.Id "
         + "LEFT JOIN (SELECT d.RefDocumentTypeId, d.RefDocumentNo, d.Attachment, "
         + "   ROW_NUMBER() OVER (PARTITION BY d.RefDocumentTypeId, d.RefDocumentNo ORDER BY d.Id) AS RowNum, "
         + "   COUNT(*) OVER (PARTITION BY d.RefDocumentTypeId, d.RefDocumentNo) AS NoOfAttachments FROM DMSAttachments d) A "
         + "   ON A.RefDocumentTypeId = r.DocumentTypeId AND A.RefDocumentNo = r.Id AND A.RowNum = 1",
         nativeQuery = true)
    List<PurchaseOrderSlipRptRow> purchaseOrderSlipRpt(@Param("organizationId") Integer organizationId,
                                                       @Param("companyId") Integer companyId,
                                                       @Param("orderId") Integer orderId,
                                                       @Param("startOrderDate") LocalDateTime startOrderDate,
                                                       @Param("endOrderDate") LocalDateTime endOrderDate,
                                                       @Param("poSrFrom") Integer poSrFrom,
                                                       @Param("poSrTo") Integer poSrTo,
                                                       @Param("supplierCustomerId") Integer supplierCustomerId,
                                                       @Param("itemId") Integer itemId,
                                                       @Param("isApproved") Boolean isApproved,
                                                       @Param("status") String status,
                                                       @Param("documentTypeId") Integer documentTypeId);

    /**
     * Sp_PurchaseOrder_PackingMaterial_Rpt (single branch).
     * // KEEP-EXEC: builds the ReportCriteria string procedurally (FORMAT, conditional concatenation), 9 temp tables,
     * STRING_AGG approval user lists from DAW.DocumentApprovalDetail / DAW.DocumentApprovalPolicy, window functions.
     * Defaults: all parameters except @OrganizationId are NULL.
     */
    @Query(value = "EXEC dbo.Sp_PurchaseOrder_PackingMaterial_Rpt @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@DocumentTypeId = :documentTypeId, @OrderId = :orderId, @StartOrderDate = :startOrderDate, @EndOrderDate = :endOrderDate, "
         + "@PoSrFrom = :poSrFrom, @PoSrTo = :poSrTo, @SupplierCustomerId = :supplierCustomerId, @ItemId = :itemId, @Status = :status, "
         + "@IsApproved = :isApproved, @PostState = :postState, @UserId = :userId, @BranchesIds = :branchesIds",
         nativeQuery = true)
    List<PurchaseOrderPackingMaterialRptRow> purchaseOrderPackingMaterialRpt(@Param("organizationId") Integer organizationId,
                                                                             @Param("companyId") Integer companyId,
                                                                             @Param("documentTypeId") Integer documentTypeId,
                                                                             @Param("orderId") Integer orderId,
                                                                             @Param("startOrderDate") LocalDateTime startOrderDate,
                                                                             @Param("endOrderDate") LocalDateTime endOrderDate,
                                                                             @Param("poSrFrom") Integer poSrFrom,
                                                                             @Param("poSrTo") Integer poSrTo,
                                                                             @Param("supplierCustomerId") Integer supplierCustomerId,
                                                                             @Param("itemId") Integer itemId,
                                                                             @Param("status") String status,
                                                                             @Param("isApproved") Boolean isApproved,
                                                                             @Param("postState") Boolean postState,
                                                                             @Param("userId") Integer userId,
                                                                             @Param("branchesIds") String branchesIds);

    /**
     * usp_PackingMaterialRequirementPlanning with @Activity = NULL (summary per packing-material item).
     * // KEEP-EXEC: InventoryStockAsOnDate fallback, 8 temp tables, UNION ALL of issuance / production / conversion
     * consumption, running-window CTE chain (Base/Calc/StockCalc/FinalCalc) and HAVING on @OnlyRequired.
     * Defaults: @OnlyRequired NULL, @ItemId NULL.
     */
    @Query(value = "EXEC dbo.usp_PackingMaterialRequirementPlanning @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@LoadingDate = :loadingDate, @LoadingToDate = :loadingToDate, @OnlyRequired = :onlyRequired, @ItemId = :itemId, @Activity = NULL",
         nativeQuery = true)
    List<PackingMaterialRequirementSummaryRow> packingMaterialRequirementPlanningSummary(@Param("organizationId") Integer organizationId,
                                                                                         @Param("companyId") Integer companyId,
                                                                                         @Param("loadingDate") LocalDate loadingDate,
                                                                                         @Param("loadingToDate") LocalDate loadingToDate,
                                                                                         @Param("onlyRequired") Integer onlyRequired,
                                                                                         @Param("itemId") Integer itemId);

    /**
     * usp_PackingMaterialRequirementPlanning with @Activity = 'Detail' (per schedule / invoice row).
     * // KEEP-EXEC: same temp-table / window-function pipeline as the summary branch.
     * Defaults: @OnlyRequired NULL (unused in this branch), @ItemId NULL.
     */
    @Query(value = "EXEC dbo.usp_PackingMaterialRequirementPlanning @OrganizationId = :organizationId, @CompanyId = :companyId, "
         + "@LoadingDate = :loadingDate, @LoadingToDate = :loadingToDate, @OnlyRequired = :onlyRequired, @ItemId = :itemId, @Activity = 'Detail'",
         nativeQuery = true)
    List<PackingMaterialRequirementDetailRow> packingMaterialRequirementPlanningDetail(@Param("organizationId") Integer organizationId,
                                                                                       @Param("companyId") Integer companyId,
                                                                                       @Param("loadingDate") LocalDate loadingDate,
                                                                                       @Param("loadingToDate") LocalDate loadingToDate,
                                                                                       @Param("onlyRequired") Integer onlyRequired,
                                                                                       @Param("itemId") Integer itemId);

    /**
     * USP_GetExpenseDetailFromPOAndPreBill (single branch; IF EXISTS supplier-dispatch expenses ELSE empty-bag amounts from PO).
     * Native: SupplierDispatchExpense / fnSplitString without entity. The @Result table variable (DECIMAL(18,2) / NVARCHAR(MAX)
     * columns) is rewritten as UNION ALL where the second part is guarded by NOT EXISTS of the first part's source rows.
     * gdnIds = comma separated InvGrn ids.
     */
    @Query(value = "SELECT d.SupplierDispatchId, d.Id SupplierDispatchExpenseId, d.OtherItemId, "
         + "CAST(d.Qty AS DECIMAL(18, 2)) AS Qty, CAST(d.Rate AS DECIMAL(18, 2)) AS Rate, CAST(d.Amount AS DECIMAL(18, 2)) AS Amount, "
         + "CAST(d.Remarks AS NVARCHAR(MAX)) AS Remarks, gd.Id GrnId, gd.DocNo GrnNo, 0 PoId, 0 PoNo "
         + "FROM SupplierDispatch h "
         + "INNER JOIN SupplierDispatchExpense d ON d.SupplierDispatchId = h.Id "
         + "INNER JOIN InvGrn gd ON gd.SupplierDispatchId = d.SupplierDispatchId "
         + "WHERE (gd.Id IN (SELECT data FROM dbo.fnSplitString(:gdnIds, ','))) "
         + "UNION ALL "
         + "SELECT 0 AS SupplierDispatchId, 0 AS SupplierDispatchExpenseId, 0 AS OtherItemId, "
         + "CAST(SUM(ISNULL(gdd.ItemQty, 0)) AS DECIMAL(18, 2)) AS Qty, CAST(d.BagPrice AS DECIMAL(18, 2)) AS Rate, "
         + "CAST(SUM(ISNULL(gdd.ItemQty, 0)) * d.BagPrice AS DECIMAL(18, 2)) AS Amount, CAST('' AS NVARCHAR(MAX)) AS Remarks, "
         + "gdd.InvGrnId AS GrnId, gh.DocNo AS GrnNo, h.Id, h.DocNo "
         + "FROM PurchaseOrder h "
         + "INNER JOIN PurchaseOrderDetail d ON h.Id = d.PurchaseOrderId "
         + "INNER JOIN InvGrnDetail gdd ON d.PurchaseOrderId = gdd.PurchaseOrderId AND gdd.ItemId = d.OrderItemId "
         + "INNER JOIN InvGrn gh ON gh.Id = gdd.InvGrnId "
         + "WHERE gdd.InvGrnId IN (SELECT data FROM dbo.fnSplitString(:gdnIds, ',')) "
         + "AND NOT EXISTS (SELECT 1 FROM SupplierDispatch h2 "
         + "   INNER JOIN SupplierDispatchExpense d2 ON d2.SupplierDispatchId = h2.Id "
         + "   INNER JOIN InvGrn gd2 ON gd2.SupplierDispatchId = d2.SupplierDispatchId "
         + "   WHERE gd2.Id IN (SELECT data FROM dbo.fnSplitString(:gdnIds, ','))) "
         + "GROUP BY d.BagPrice, gdd.InvGrnId, gh.DocNo, h.Id, h.DocNo",
         nativeQuery = true)
    List<ExpenseDetailFromPoAndPreBillRow> getExpenseDetailFromPoAndPreBill(@Param("gdnIds") String gdnIds);
}
