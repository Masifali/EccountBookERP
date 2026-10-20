package com.mst.repositories;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.InvPurchaseInvoice;
import com.mst.projections.DashboardApprovedVouchersForUnPostRow;
import com.mst.projections.DashboardPendingForApprovalRow;
import com.mst.projections.DashboardPendingForSpecialApprovalsRow;

/**
 * Query-only repository for the approval dashboards (batch L_dashboards_approvals).
 * Procedures: Sp_Inventory_ReadDashboardPendingForApproval, Sp_Inventory_ReadDashboardApprovedVouchersForUnPost,
 * usp_ReadDashboardPendingForSpecialApprovals. All three are flattened into ONE native statement each:
 * <ul>
 *   <li>{@code SELECT * INTO #ApprovalRights ...} becomes the CTE {@code ApprovalRights} (same SELECT, same filter).</li>
 *   <li>{@code DECLARE @From = ISNULL(@FromDate,'19000101'); @To = ISNULL(DATEADD(DAY,1,@ToDate),'99991231')}
 *       become the inline expressions {@link #FROM_D} / {@link #TO_D}.</li>
 *   <li>Every {@code SET @Flag = CONVERT(BIT, dbo.GetConfigurationByConfigId(@OrganizationId,@CompanyId,N))}
 *       is inlined as that very expression at each place the variable was used ({@link #CFG}).</li>
 *   <li>{@code dbo.fnSplitString(@LabStatuses, ',')} keeps the TVF, with the IF/ELSE that built @LabStatuses
 *       rewritten as a CASE expression ({@link #LAB_STATUSES}).</li>
 * </ul>
 * Bound to the existing entity InvPurchaseInvoice only because Spring Data needs a managed entity.
 */
@Repository
public interface DashboardApprovalsQueryRepository extends org.springframework.data.repository.Repository<InvPurchaseInvoice, Integer> {

    // ------------------------------------------------------------------------------------------------
    // Shared SQL fragments (compile-time constants, used inside the @Query values below)
    // ------------------------------------------------------------------------------------------------

    /** #ApprovalRights of both Sp_Inventory_ReadDashboard* procs (ScreenName = 'PendingVouchersForApproval'). */
    String RIGHTS_CTE =
          "WITH ApprovalRights AS ("
        + "SELECT r.Id, r.Value, sr.RightName "
        + "FROM tblUserRights r "
        + "INNER JOIN ScreenDefinition s ON s.Id = r.ScreenId "
        + "INNER JOIN ScreenRights sr ON sr.Id = r.RightId "
        + "WHERE r.CompanyId = :companyId AND r.UserId = :userId AND s.ScreenName = 'PendingVouchersForApproval') ";

    /** {@code AND CONVERT(BIT,1) = (SELECT DISTINCT #ApprovalRights.Value FROM #ApprovalRights WHERE RightName='...')} — open part. */
    String AR = " AND CONVERT(BIT,1) = (SELECT DISTINCT ar.Value FROM ApprovalRights ar WHERE ar.RightName = '";
    /** Close part of {@link #AR}. */
    String AR_END = "') ";

    /** {@code @From = ISNULL(@FromDate, '19000101')}. */
    String FROM_D = "COALESCE(CAST(:fromDate AS DATE), CAST('19000101' AS DATE))";
    /** {@code @To = ISNULL(DATEADD(DAY, 1, @ToDate), '99991231')}. */
    String TO_D = "COALESCE(DATEADD(DAY, 1, CAST(:toDate AS DATE)), CAST('99991231' AS DATE))";

    /** {@code AND DocDate >= @From AND DocDate < @To}. */
    String DD = " AND DocDate >= " + FROM_D + " AND DocDate < " + TO_D;
    /** {@code AND GpDate >= @From AND GpDate < @To}. */
    String GPD = " AND GpDate >= " + FROM_D + " AND GpDate < " + TO_D;

    /** {@code AND (@FinancialYearId IS NULL OR FinancialYearId=@FinancialYearId)}. */
    String FYO = " AND (:financialYearId IS NULL OR FinancialYearId = :financialYearId)";
    /** {@code AND FinancialYearId = @FinancialYearId} (mandatory form used by some branches). */
    String FYM = " AND FinancialYearId = :financialYearId";
    /** {@code AND (@BranchesId is null or BranchesId = @BranchesId)}. */
    String BRO = " AND (:branchesId IS NULL OR BranchesId = :branchesId)";
    /** {@code WHERE OrganizationId = @OrganizationId AND CompanyId = @CompanyId}. */
    String OC = " WHERE OrganizationId = :organizationId AND CompanyId = :companyId";
    /** {@code HAVING COUNT(*) > 0}. */
    String HAV = " HAVING COUNT(*) > 0";

    /** {@code CONVERT(BIT, dbo.GetConfigurationByConfigId(@OrganizationId, @CompanyId, <id>))} — open part. */
    String CFG = "CONVERT(BIT, dbo.GetConfigurationByConfigId(:organizationId, :companyId, ";

    String CFG_PO_BW   = CFG + "273))";   // @PurchaseOrderBranchWise
    String CFG_PI_BW   = CFG + "274))";   // @PurchaseInvoiceBranchWise
    String CFG_PID_BW  = CFG + "275))";   // @PurchaseInvoiceDirectBranchWise
    String CFG_PIR_BW  = CFG + "276))";   // @PurchaseInvoiceReturnBranchWise
    String CFG_SO_BW   = CFG + "277))";   // @SaleOrderBranchWise
    String CFG_SI_BW   = CFG + "278))";   // @SaleInvoiceBranchWise
    String CFG_SID_BW  = CFG + "279))";   // @SaleInvoiceDirectBranchWise
    String CFG_SIR_BW  = CFG + "280))";   // @SaleInvoiceReturnBranchWise
    String CFG_PROD_OUT_LAB = CFG + "5632))"; // @ProductionOutputLabApprovalRequired
    String CFG_GPI_APPROVAL = CFG + "97))";   // @GPIApproval
    String CFG_GPO_WITHOUT_DO = CFG + "94))"; // @GPOCreateWithoutDoApproval
    String CFG_GPO_UPDATE_WITHOUT_APPROVAL = CFG + "81))"; // @GatePassOutwardUpdateWithoutApproval
    String CFG_DEPT_REQ_COMPULSORY = CFG + "119))";        // @DepartmentRequestApprovalCompulsory
    /** @LabStatusesAgainstWhichTheLabApprovalWillBeCounted: '0' when config 114 is 0/NULL, else '0,1'. */
    String LAB_STATUSES = "CASE WHEN ISNULL(" + CFG + "114)), 0) = 0 THEN '0' ELSE '0,1' END";

    /** Branch-wise filter: {@code and ((@Flag = 1 AND BranchesId = @BranchesId) or (isnull(@Flag,0) = 0 or @BranchesId is null))}. */
    String BW_PO  = " AND ((" + CFG_PO_BW  + " = 1 AND BranchesId = :branchesId) OR (ISNULL(" + CFG_PO_BW  + ", 0) = 0 OR :branchesId IS NULL))";
    String BW_PI  = " AND ((" + CFG_PI_BW  + " = 1 AND BranchesId = :branchesId) OR (ISNULL(" + CFG_PI_BW  + ", 0) = 0 OR :branchesId IS NULL))";
    String BW_PID = " AND ((" + CFG_PID_BW + " = 1 AND BranchesId = :branchesId) OR (ISNULL(" + CFG_PID_BW + ", 0) = 0 OR :branchesId IS NULL))";
    String BW_PIR = " AND ((" + CFG_PIR_BW + " = 1 AND BranchesId = :branchesId) OR (ISNULL(" + CFG_PIR_BW + ", 0) = 0 OR :branchesId IS NULL))";
    String BW_SO  = " AND ((" + CFG_SO_BW  + " = 1 AND BranchesId = :branchesId) OR (ISNULL(" + CFG_SO_BW  + ", 0) = 0 OR :branchesId IS NULL))";
    String BW_SI  = " AND ((" + CFG_SI_BW  + " = 1 AND BranchesId = :branchesId) OR (ISNULL(" + CFG_SI_BW  + ", 0) = 0 OR :branchesId IS NULL))";
    String BW_SID = " AND ((" + CFG_SID_BW + " = 1 AND BranchesId = :branchesId) OR (ISNULL(" + CFG_SID_BW + ", 0) = 0 OR :branchesId IS NULL))";
    String BW_SIR = " AND ((" + CFG_SIR_BW + " = 1 AND BranchesId = :branchesId) OR (ISNULL(" + CFG_SIR_BW + ", 0) = 0 OR :branchesId IS NULL))";

    // ------------------------------------------------------------------------------------------------
    // Sp_Inventory_ReadDashboardPendingForApproval
    // ------------------------------------------------------------------------------------------------

    /** Outer SELECT of Sp_Inventory_ReadDashboardPendingForApproval (shared by both parts). */
    String PENDING_HEAD = RIGHTS_CTE
        + "SELECT A.TypeID, A.Description, A.Count, A.ActionId, A.SortNo, "
        + ":organizationId AS OrganizationId, :companyId AS CompanyId, :branchesId AS BranchesId, "
        + ":financialYearId AS FinancialYearId, :userId AS EntryUser, "
        + "R.PostDate, R.Activity, R.RecordId, R.DocumentTypeId, R.ReqType, R.ApprovedFilter, R.CompanyName, "
        + "R.CompanyAddress, R.ReportName, R.VoucherReportName, R.ApiEndPoint, R.ApprovedEndPoint, R.PrintEndPoint, "
        + "R.PrintEndPointVoucher, R.ComboBindApiEndPoint "
        + "FROM ( ";
    /** Outer join/filter of Sp_Inventory_ReadDashboardPendingForApproval (shared by both parts; ORDER BY is done in Java). */
    String PENDING_TAIL = " ) A LEFT JOIN InventoryApprovedParams R ON A.TypeID = R.TypeId "
        + "WHERE (:appId IS NULL OR R.TypeId > 0)";

    /**
     * Sp_Inventory_ReadDashboardPendingForApproval (single branch) — pending-for-approval counts per document type.
     * Proc defaults: @UserId = NULL, @FinancialYearId = NULL, @BranchesId = NULL, @FromDate = NULL, @ToDate = NULL, @AppId = NULL.
     * Native: temp table #ApprovalRights -> CTE; config variables inlined; UNION ALL of 102 per-document-type counts;
     * LEFT JOIN InventoryApprovedParams; ORDER BY A.ActionId, A.SortNo.
     * Note: @AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt is hard-coded 0 in the proc (its SET is commented out)
     * and is kept as the literal {@code CONVERT(BIT,0)}.
     * <p>The SQL exceeds the 65535-byte limit of a single Java string constant, so the UNION ALL is split into
     * {@link #readDashboardPendingForApprovalPart1} (members 1-51) and {@link #readDashboardPendingForApprovalPart2}
     * (members 52-102); this default method concatenates both result lists and applies the proc's
     * {@code ORDER BY A.ActionId ASC, A.SortNo ASC} with a stable sort (nulls last).
     */
    default List<DashboardPendingForApprovalRow> readDashboardPendingForApproval(
            Integer organizationId, Integer companyId, Integer userId, Integer financialYearId,
            Integer branchesId, LocalDate fromDate, LocalDate toDate, Integer appId) {
        List<DashboardPendingForApprovalRow> rows = new ArrayList<DashboardPendingForApprovalRow>();
        rows.addAll(readDashboardPendingForApprovalPart1(organizationId, companyId, userId, financialYearId, branchesId, fromDate, toDate, appId));
        rows.addAll(readDashboardPendingForApprovalPart2(organizationId, companyId, userId, financialYearId, branchesId, fromDate, toDate, appId));
        Comparator<Integer> nullsLast = Comparator.nullsLast(Comparator.<Integer>naturalOrder());
        rows.sort(Comparator.comparing(DashboardPendingForApprovalRow::getActionId, nullsLast)
                .thenComparing(DashboardPendingForApprovalRow::getSortNo, nullsLast));
        return rows;
    }

    /**
     * Sp_Inventory_ReadDashboardPendingForApproval — part 1 of 2 (UNION ALL members 1-51: Purchase Order ... Sale Return Invoice 1611).
     * No ORDER BY; use {@link #readDashboardPendingForApproval} for the full proc result.
     */
    @Query(value = PENDING_HEAD
        // -- Purchase Order
        + "SELECT 20 AS TypeId, 'Purchase Order' AS Description, COUNT(h.Id) AS Count, 1 AS ActionId, 1 AS SortNo "
        + "FROM [dbo].[PurchaseOrder] h "
        + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND h.DocumentTypeId = 41 "
        + "AND (:financialYearId IS NULL OR h.FinancialYearId = :financialYearId) "
        + "AND ISNULL(h.IsAproved, 0) <> 1" + DD
        + AR + "PurchaseOrder" + AR_END
        + " AND ((" + CFG_PO_BW + " = 1 AND h.BranchesId = :branchesId) OR (ISNULL(" + CFG_PO_BW + ", 0) = 0 OR :branchesId IS NULL))"
        + " HAVING COUNT(h.Id) > 0 "
        // -- GatePassInward
        + "UNION ALL SELECT 56, 'Inward GatePass', COUNT(*), 1, 2 FROM GatePassInward"
        + OC + " AND DocumentTypeId = 51 AND IsApproved <> 1" + GPD + FYO + BRO
        + " AND (CONVERT(BIT,1) = " + CFG_GPI_APPROVAL + ")"
        + " AND (CONVERT(BIT,1) = (SELECT DISTINCT ar.Value FROM ApprovalRights ar WHERE ar.RightName = 'GatePassInward'))"
        + " AND (ISNULL(CONVERT(BIT,0), 0) = 0 OR ISNULL(AccessWeight, 0) <= 0)" + HAV
        // -- Gate Pass General (inward)
        + " UNION ALL SELECT TypeID = 189, Description = 'Inward GatePass General', Count = COUNT(*), ActionId = 1, SortNo = 3 FROM GatePassGeneral"
        + OC + " AND DocumentTypeId = 52 AND IsApproved <> 1" + GPD + FYO + BRO
        + AR + "GatePassInwardGeneral" + AR_END + HAV
        // -- FreightVoucher
        + " UNION ALL SELECT TypeID = 142, Description = 'Freight Voucher', Count = COUNT(*), ActionId = 1, SortNo = 4 FROM [dbo].InvFreightVoucher"
        + OC + " AND (FinancialYearId = :financialYearId) AND DocumentTypeId = 28" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "FreightVoucher" + AR_END + HAV
        // -- Labour Wages
        + " UNION ALL SELECT TypeID = 101, Description = 'Labour Wages', Count = COUNT(*), ActionId = 1, SortNo = 5 FROM InvContractorWagesBillHeader"
        + OC + " AND DocumentTypeId = 101 AND ISNULL(IsAproved, 0) <> 1" + DD + FYO + BRO
        + AR + "LabourWages" + AR_END + HAV
        // -- Labour Wages Manual
        + " UNION ALL SELECT TypeID = 810, Description = 'Labour Wages Manual', Count = COUNT(*), ActionId = 1, SortNo = 5 FROM InvContractorWagesBillHeader"
        + OC + " AND DocumentTypeId = 810 AND ISNULL(IsAproved, 0) <> 1" + DD + FYO + BRO
        + AR + "LabourWagesManual" + AR_END + HAV
        // -- Purchase Invoice
        + " UNION ALL SELECT TypeID = 23, Description = 'Purchase Invoice', Count = COUNT(*), ActionId = 1, SortNo = 5 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 56 AND IsApproved <> 1" + DD + FYO + BW_PI
        + AR + "PurchaseInvoice" + AR_END + HAV
        // -- Purchase Invoice Direct
        + " UNION ALL SELECT TypeID = 24, Description = 'Purchase Invoice Direct', Count = COUNT(*), ActionId = 1, SortNo = 6 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 57 AND IsApproved <> 1" + DD + FYO
        + AR + "PurchaseInvoiceDirect" + AR_END + BW_PID + HAV
        // -- Purchase Invoice Against Direct Grn
        + " UNION ALL SELECT TypeID = 138, Description = 'Purchase Invoice Against Direct Grn', Count = COUNT(*), ActionId = 1, SortNo = 7 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 138 AND IsApproved <> 1" + DD + FYO + BW_PI
        + AR + "PurchaseInvoiceAgainstGrnDirect" + AR_END + HAV
        // -- Purchase Invoice (172)
        + " UNION ALL SELECT TypeID = 60, Description = 'Purchase Invoice', Count = COUNT(*), ActionId = 1, SortNo = 8 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 172 AND IsApproved <> 1" + DD + FYO
        + AR + "PurchaseInvoice" + AR_END + HAV
        // -- Purchase Invoice Return
        + " UNION ALL SELECT TypeID = 26, Description = 'Purchase Return', Count = COUNT(*), ActionId = 1, SortNo = 9 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 59 AND IsApproved <> 1" + DD + FYO
        + AR + "PurchaseInvoiceReturn" + AR_END + BW_PIR + HAV
        // -- Purchase Order Steel
        + " UNION ALL SELECT TypeID = 1500, Description = 'Purchase Order', Count = COUNT(Id), ActionId = 1, SortNo = 1 FROM [ST].[PurchaseOrder]"
        + OC + " AND DocumentTypeId = 1500" + FYM + " AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "PurchaseOrder" + AR_END + " HAVING COUNT(Id) > 0"
        // -- Purchase Order Feed
        + " UNION ALL SELECT TypeID = 1100, Description = 'Purchase Order Feed', Count = COUNT(Id), ActionId = 1, SortNo = 1 FROM [fed].[PurchaseOrder]"
        + OC + " AND DocumentTypeId = 1100" + FYM + " AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "PurchaseOrder" + AR_END + " HAVING COUNT(Id) > 0"
        // -- Purchase Order Store (42)
        + " UNION ALL SELECT TypeID = 21, Description = 'Purchase Order Store', Count = COUNT(*), ActionId = 1, SortNo = 10 FROM [dbo].[PurchaseOrder]"
        + OC + " AND DocumentTypeId = 42 AND IsAproved <> 1" + DD + FYO
        + AR + "PurchaseOrderStore" + AR_END + HAV
        // -- Purchase Order Store (132)
        + " UNION ALL SELECT TypeID = 132, Description = 'Purchase Order Store', Count = COUNT(*), ActionId = 1, SortNo = 10 FROM [dbo].[PurchaseOrder]"
        + OC + " AND DocumentTypeId = 132 AND IsAproved <> 1" + DD + FYO
        + AR + "PurchaseOrderStore" + AR_END + HAV
        // -- DepartmentRequest
        + " UNION ALL SELECT TypeID = 58, Description = 'Department Request', Count = COUNT(*), ActionId = 1, SortNo = 11 FROM DepartmentRequest"
        + OC + " AND DocTypeId = 450" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + " AND (:branchesId IS NULL OR BranchId = :branchesId)"
        + " AND (CONVERT(BIT,1) = ISNULL(" + CFG_DEPT_REQ_COMPULSORY + ", 0))"
        + AR + "DepartmentRequest" + AR_END + HAV
        // -- Purchase Demand Store
        + " UNION ALL SELECT TypeID = 188, Description = 'Purchase Demand Store', Count = COUNT(*), ActionId = 1, SortNo = 11 FROM [dbo].InvPurchaseDemandHeader"
        + OC + " AND DocumentTypeId = 141" + DD + " AND ISNULL(IsApproved, 0) <> 1 AND ISNULL(ParentCategoryId, 0) IN (0, 8)" + FYO
        + AR + "PurchaseDemand" + AR_END + HAV
        // -- DepartmentRequest to consumable store
        + " UNION ALL SELECT TypeID = 75, Description = 'Department Request To Cosumable Store', Count = COUNT(*), ActionId = 1, SortNo = 12 FROM DepartmentRequest"
        + OC + " AND DocTypeId = 1616" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + " AND (CONVERT(BIT,1) = ISNULL(" + CFG_DEPT_REQ_COMPULSORY + ", 0))"
        + AR + "frmStoreIssuanceToCosumableStore" + AR_END + HAV
        // -- Purchase Order Packing Material (proc aliases 5th column 'ortNo' here; UNION keeps first SELECT's alias)
        + " UNION ALL SELECT TypeID = 700, Description = 'Purchase Order_PM', Count = COUNT(*), ActionId = 1, ortNo = 12 FROM [dbo].[PurchaseOrder]"
        + OC + " AND DocumentTypeId = 700" + DD + FYO + BW_PO + " AND IsAproved <> 1"
        + AR + "PurchaseOrderPM" + AR_END + HAV
        // -- PM_PurchaseInvoice
        + " UNION ALL SELECT TypeID = 72, Description = 'PM_PurchaseInvoice', Count = COUNT(*), ActionId = 1, SortNo = 13 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 702 AND IsApproved <> 1" + DD + FYO
        + AR + "PM_PurchaseInvoice" + AR_END + HAV
        // -- Purchase Demand Packing Material
        + " UNION ALL SELECT TypeID = 141, Description = 'Purchase Demand Packing Material', Count = COUNT(*), ActionId = 1, SortNo = 14 FROM [dbo].InvPurchaseDemandHeader"
        + OC + " AND DocumentTypeId = 141" + DD + " AND ISNULL(IsApproved, 0) <> 1 AND ISNULL(ParentCategoryId, 0) = 7" + FYO
        + AR + "PurchaseDemandPM" + AR_END + HAV
        // -- PM_PurchaseInvoiceDirect
        + " UNION ALL SELECT TypeID = 245, Description = 'PM_PurchaseInvoiceDirect', Count = COUNT(*), ActionId = 1, SortNo = 15 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 245 AND IsApproved <> 1" + DD + FYO
        + AR + "PM_PurchaseInvoiceDirect" + AR_END + HAV
        // -- Purchase Invoice Store
        + " UNION ALL SELECT TypeID = 65, Description = 'Purchase Invoice Store', Count = COUNT(*), ActionId = 1, SortNo = 16 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 64 AND IsApproved <> 1" + DD + FYO
        + AR + "PurchaseInvoiceTrading" + AR_END + HAV
        // -- Purchase Invoice Return Store
        + " UNION ALL SELECT TypeID = 145, Description = 'Purchase Invoice Return Store', Count = COUNT(*), ActionId = 1, SortNo = 17 FROM dbo.InvSaleInvoice"
        + OC + " AND DocumentTypeId = 145 AND ISNULL(BaseDocumentTypeId, 0) = 1 AND ISNULL(ActionId, 0) <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "PurchaseInvoiceReturn_Store" + AR_END + HAV
        // -- Purchase Invoice Return PM
        + " UNION ALL SELECT TypeID = 146, Description = 'Purchase Invoice Return PM', Count = COUNT(*), ActionId = 1, SortNo = 18 FROM dbo.InvSaleInvoice"
        + OC + " AND DocumentTypeId = 145 AND ISNULL(BaseDocumentTypeId, 0) = 2 AND ISNULL(ActionId, 0) <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "PurchaseInvoiceReturnPM" + AR_END + HAV
        // -- Sales Order
        + " UNION ALL SELECT TypeID = 28, Description = 'Sales Order', Count = COUNT(*), ActionId = 2, SortNo = 1 FROM [dbo].SaleOrder"
        + OC + " AND DocumentTypeId = 81 AND ISNULL(IsAproved, 0) <> 1 AND OrderStatus = 'Open'" + DD + FYO
        + AR + "SaleOrder" + AR_END + BW_SO + HAV
        // -- Sales Order Store And PM
        + " UNION ALL SELECT TypeID = 90, Description = 'Sales Order Store And Pm', Count = COUNT(*), ActionId = 2, SortNo = 1 FROM [dbo].SaleOrder"
        + OC + " AND DocumentTypeId = 90 AND ISNULL(IsAproved, 0) <> 1 AND OrderStatus = 'Open'" + DD + FYO
        + AR + "frmSaleOrderStoreAndPm" + AR_END + BW_SO + HAV
        // -- Booking Order
        + " UNION ALL SELECT TypeID = 54, Description = 'Booking Order', Count = COUNT(*), ActionId = 2, SortNo = 2 FROM SaleOrder"
        + OC + " AND DocumentTypeId = 127 AND ISNULL(IsAproved, 0) <> 1" + DD + FYO + BW_SO
        + AR + "BookingOrder" + AR_END + HAV
        // -- Delivery Order Local Sale
        + " UNION ALL SELECT TypeID = 37, Description = 'Delivery Order Sale', Count = COUNT(*), ActionId = 2, SortNo = 3 FROM [dbo].InvDeliveryOrder"
        + OC + " AND DocumentTypeId = 84 AND DeliveryOrderType = 'Local' AND ISNULL(ActionId, 0) <> 3 AND ISNULL(IsApproved, 0) <> 1" + DD
        + " AND (CONVERT(BIT,1) <> ISNULL(" + CFG_GPO_WITHOUT_DO + ", 0))" + FYO
        + AR + "DeliveryOrder" + AR_END + HAV
        // -- Advance Delivery Order
        + " UNION ALL SELECT TypeID = 910, Description = 'Advance Delivery Order', Count = COUNT(*), ActionId = 2, SortNo = 4 FROM [dbo].InvDeliveryOrder"
        + OC + " AND DocumentTypeId = 910 AND DeliveryOrderType = 'AdvanceDO' AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1" + FYO
        + AR + "AdvanceDeliveryOrder" + AR_END + HAV
        // -- Delivery Order Packing Material
        + " UNION ALL SELECT TypeID = 38, Description = 'Delivery Packing Material', Count = COUNT(*), ActionId = 2, SortNo = 4 FROM [dbo].InvDeliveryOrder"
        + OC + " AND DocumentTypeId = 85 AND ISNULL(ActionId, 0) <> 3 AND ISNULL(IsApproved, 0) <> 1" + DD + FYO + BRO
        + AR + "DeliveryOrderPackingMaterial" + AR_END + HAV
        // -- GatePassOutward
        + " UNION ALL SELECT TypeID = 39, Description = 'Outward GatePass', Count = COUNT(*), ActionId = 2, SortNo = 5 FROM GatePassOutward GP "
        + "INNER JOIN (SELECT ReferenceDocTypeId, ReferenceDocNoId, ISNULL(SUM(NetWbWeight), 0) NetWbWeight FROM WbTransactions GROUP BY ReferenceDocTypeId, ReferenceDocNoId) wb "
        + "ON GP.Id = wb.ReferenceDocNoId AND GP.DocumentTypeId = wb.ReferenceDocTypeId"
        + OC + " AND DocumentTypeId = 91 AND IsApproved <> 1" + GPD + FYO + BRO
        + " AND (RefDocumentTypeId <> 1606) AND (wb.NetWbWeight > 0)"
        + " AND (CONVERT(BIT,1) <> ISNULL(" + CFG_GPO_UPDATE_WITHOUT_APPROVAL + ", 0))"
        + AR + "GatePassOutward" + AR_END + HAV
        // -- GatePassOutward General
        + " UNION ALL SELECT TypeID = 40, Description = 'Outward GatePass General', Count = COUNT(*), ActionId = 2, SortNo = 6 FROM GatePassGeneral"
        + OC + " AND DocumentTypeId = 92 AND IsApproved <> 1" + GPD + FYO + BRO
        + AR + "GatePassOutwardGeneral" + AR_END + HAV
        // -- Sale Invoice
        + " UNION ALL SELECT TypeID = 32, Description = 'Sale Invoice', Count = COUNT(*), ActionId = 2, SortNo = 7 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 95 AND IsApproved <> 1" + DD + FYO + BW_SI
        + AR + "SaleInvoice" + AR_END + HAV
        // -- Sale Invoice Direct
        + " UNION ALL SELECT TypeID = 33, Description = 'Sale Invoice Direct', Count = COUNT(*), ActionId = 2, SortNo = 8 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 99 AND IsApproved <> 1" + DD + FYO
        + AR + "SaleInvoiceDirect" + AR_END + BW_SID + HAV
        // -- Sale Invoice Direct Auto Rated
        + " UNION ALL SELECT TypeID = 184, Description = 'Sale Invoice Direct Auto Rated', Count = COUNT(*), ActionId = 2, SortNo = 9 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 184 AND IsApproved <> 1" + DD + FYO
        + AR + "SaleInvoiceDirectAutoRated" + AR_END + BW_SID + HAV
        // -- Sale Invoice Without WeighBridge
        + " UNION ALL SELECT TypeID = 59, Description = 'Sale Invoice', Count = COUNT(*), ActionId = 2, SortNo = 10 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 171 AND IsApproved <> 1" + DD + FYO
        + AR + "SaleInvoice" + AR_END + HAV
        // -- Sale Invoice Return
        + " UNION ALL SELECT TypeID = 36, Description = 'Sale Return Invoice', Count = COUNT(*), ActionId = 2, SortNo = 11 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 98 AND IsApproved <> 1" + DD + FYO
        + AR + "SaleInvoiceReturn" + AR_END + BW_SIR + HAV
        // -- Delivery Order Transfer
        + " UNION ALL SELECT TypeID = 63, Description = 'Delivery Order Transfer', Count = COUNT(*), ActionId = 2, SortNo = 12 FROM [dbo].InvDeliveryOrder"
        + OC + " AND DocumentTypeId = 84 AND DeliveryOrderType = 'StockTransfer' AND ActionId <> 3 AND IsApproved <> 1" + DD
        + " AND (CONVERT(BIT,1) <> ISNULL(" + CFG_GPO_WITHOUT_DO + ", 0))" + FYO
        + AR + "DeliveryOrder" + AR_END + HAV
        // -- Store Issuance
        + " UNION ALL SELECT TypeID = 74, Description = 'Stor Issuance', Count = COUNT(*), ActionId = 2, SortNo = 13 FROM [dbo].InvGsStoreIssuanceHeader"
        + OC + " AND DocumentTypeId = 451 AND IsApproved <> 1" + DD + FYO
        + AR + "StorIssuance" + AR_END + HAV
        // -- Stock Transfer Manual
        + " UNION ALL SELECT TypeID = 806, Description = 'Stock Transfer Mannual', Count = COUNT(*), ActionId = 2, SortNo = 14 FROM [dbo].InvStockTransferHeader"
        + OC + " AND DocumentTypeId = 806 AND IsApproved <> 1" + DD + FYO
        + AR + "StockTransferManual" + AR_END + HAV
        // -- Stock Transfer Auto
        + " UNION ALL SELECT TypeID = 68, Description = 'Stock Transfer Auto', Count = COUNT(*), ActionId = 2, SortNo = 15 FROM [dbo].InvStockTransferHeader"
        + OC + " AND DocumentTypeId = 68 AND IsApproved <> 1" + DD + FYO
        + AR + "StockTransferAuto" + AR_END + HAV
        // -- Delivery Order Returnable
        + " UNION ALL SELECT TypeID = 1200, Description = 'Delivery Order Returnable', Count = COUNT(*), ActionId = 2, SortNo = 16 FROM [dbo].InvDeliveryOrder"
        + OC + " AND DocumentTypeId = 1200 AND IsApproved <> 1" + DD + FYO
        + AR + "DeliveryOrder_Returnable" + AR_END + HAV
        // -- Purchase Order Engineering With Tax
        + " UNION ALL SELECT TypeID = 1600, Description = 'Purchase Order', Count = COUNT(*), ActionId = 1, SortNo = 1 FROM [dbo].[PurchaseOrder]"
        + OC + " AND DocumentTypeId = 1600" + DD + FYO + " AND IsAproved <> 1"
        + AR + "PurchaseOrder" + AR_END + HAV
        // -- Goods Receiving Notes (1602/1618)
        + " UNION ALL SELECT TypeID = 1618, Description = 'Goods Receiving Notes', Count = COUNT(*), ActionId = 1, SortNo = 2 FROM InvGrn "
        + "INNER JOIN (SELECT InvGrnId FROM InvGrnDetail GROUP BY InvGrnId) d ON InvGrn.Id = d.InvGrnId"
        + OC + " AND DocumentTypeId IN (1602, 1618) AND ISNULL(IsApproved, 0) = 0" + DD
        + AR + "Goods Receiving Notes" + AR_END + HAV
        // -- Sale Order (1605)
        + " UNION ALL SELECT TypeID = 70, Description = 'Sale Order', Count = COUNT(*), ActionId = 2, SortNo = 1 FROM [dbo].SaleOrder"
        + OC + " AND DocumentTypeId = 1605 AND IsAproved <> 1" + DD + FYO
        + AR + "SaleOrderWithQty" + AR_END + HAV
        // -- Delivery Order (1606)
        + " UNION ALL SELECT TypeID = 71, Description = 'Delivery Order', Count = COUNT(*), ActionId = 2, SortNo = 2 FROM [dbo].InvDeliveryOrder"
        + OC + " AND DocumentTypeId = 1606 AND IsApproved <> 1" + DD + FYO
        + AR + "DeliveryOrder_Engr" + AR_END + HAV
        // -- Good Dispatch Notes
        + " UNION ALL SELECT TypeID = 31, Description = 'Good Dispatch Notes', Count = COUNT(*), ActionId = 2, SortNo = 3 FROM [dbo].InvGdn h "
        + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND h.DocumentTypeId IN (1612, 1607) AND h.IsApproved <> 1" + DD + BRO
        + AR + "GoodsDispatchedNotes" + AR_END + HAV
        // -- Sale Invoice Direct (1608)
        + " UNION ALL SELECT TypeID = 186, Description = 'Sale Invoice Direct', Count = COUNT(*), ActionId = 2, SortNo = 4 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 1608 AND IsApproved <> 1" + DD + FYO
        + AR + "SaleInvoiceDirect" + AR_END + HAV
        // -- Sale Invoice (1609)
        + " UNION ALL SELECT TypeID = 185, Description = 'Sale Invoice', Count = COUNT(*), ActionId = 2, SortNo = 5 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 1609 AND IsApproved <> 1" + DD + FYO
        + AR + "SaleInvoice" + AR_END + HAV
        // -- Sale Invoice Return (1611)
        + " UNION ALL SELECT TypeID = 187, Description = 'Sale Return Invoice ', Count = COUNT(*), ActionId = 2, SortNo = 6 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 1611 AND IsApproved <> 1" + DD + FYO
        + AR + "SaleInvoiceReturn" + AR_END + HAV
        + PENDING_TAIL,
        nativeQuery = true)
    List<DashboardPendingForApprovalRow> readDashboardPendingForApprovalPart1(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("userId") Integer userId,
            @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("appId") Integer appId);

    /**
     * Sp_Inventory_ReadDashboardPendingForApproval — part 2 of 2 (UNION ALL members 52-102: Sale Order 1656 ... Commission Trade Bill).
     * No ORDER BY; use {@link #readDashboardPendingForApproval} for the full proc result.
     */
    @Query(value = PENDING_HEAD
        // -- Sale Order (1656)
        + "SELECT TypeID = 1656, Description = 'Sale Order', Count = COUNT(*), ActionId = 2, SortNo = 1 FROM [dbo].SaleOrder"
        + OC + " AND DocumentTypeId = 1656 AND IsAproved <> 1" + DD + FYO
        + AR + "SaleOrderWithQty" + AR_END + BW_SO + HAV
        // -- Delivery Order (1657)
        + " UNION ALL SELECT TypeID = 1657, Description = 'Delivery Order', Count = COUNT(*), ActionId = 2, SortNo = 2 FROM [dbo].InvDeliveryOrder"
        + OC + " AND DocumentTypeId = 1657 AND IsApproved <> 1" + DD + FYO
        + AR + "DeliveryOrder_Engr" + AR_END + HAV
        // -- Sale Invoice (1660)
        + " UNION ALL SELECT TypeID = 1660, Description = 'Sale Invoice', Count = COUNT(*), ActionId = 2, SortNo = 3 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 1660 AND IsApproved <> 1" + DD + FYO + BW_SI
        + AR + "SaleInvoice" + AR_END + HAV
        // -- Sale Invoice Direct (1661)
        + " UNION ALL SELECT TypeID = 1661, Description = 'Sale Invoice Direct', Count = COUNT(*), ActionId = 2, SortNo = 4 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 1661 AND IsApproved <> 1" + DD + FYO
        + AR + "SaleInvoiceDirect" + AR_END + BW_SID + HAV
        // -- Export Pre Shipment Analysis
        + " UNION ALL SELECT TypeID = 51, Description = 'Pre-Shipment Analysis', Count = COUNT(*), ActionId = 3, SortNo = 4 FROM InvLabPreProductionExportLotInspectionHeader"
        + OC + " AND RequestDate >= " + FROM_D + " AND RequestDate < " + TO_D + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "PreShipmentAnalysis" + AR_END + HAV
        // -- Lab Purchase Analysis
        + " UNION ALL SELECT TypeID = 57, Description = 'Purchase Analysis', Count = COUNT(*), ActionId = 1, SortNo = 3 FROM InvLabAnalysisPurchaseHeader"
        + OC + BRO + " AND DocumentTypeId = 303" + DD
        + " AND (ISNULL(IsAccepted, 0) IN (SELECT CONVERT(BIT, DATA) FROM dbo.fnSplitString(" + LAB_STATUSES + ", ',')))"
        + " AND (ISNULL(IsApproved, 0) <> 1) AND ISNULL(IsAcceptedApproved, 0) = 0"
        + AR + "LabPurchaseAnalysis" + AR_END + HAV
        // -- Apply && Approve Gate Purchase Rate
        + " UNION ALL SELECT TypeID = 303, Description = 'Apply && Approve Gate Purchase Rate', Count = COUNT(*), ActionId = 1, SortNo = 3 "
        + "FROM InvLabAnalysisPurchaseHeader h INNER JOIN GatePassInward GP ON h.GatePassInwardId = GP.Id "
        + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND (:branchesId IS NULL OR h.BranchesId = :branchesId) "
        + "AND h.DocumentTypeId = 303 AND (:financialYearId IS NULL OR h.FinancialYearId = :financialYearId) "
        + "AND GP.RefDocumentTypeId = 106 AND h.IsApproved = 1" + DD + " AND (ISNULL(h.RateApplied, 0) = 0)"
        + AR + "LabPurchaseAnalysis" + AR_END + HAV
        // -- Apply && Approve Rate/Weight Cut (PO)
        + " UNION ALL SELECT TypeID = 301, Description = 'Apply && Approve Rate/Weight Cut (PO)', Count = COUNT(*), ActionId = 1, SortNo = 3 "
        + "FROM InvLabAnalysisPurchaseHeader h INNER JOIN GatePassInward GP ON h.GatePassInwardId = GP.Id "
        + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND (:branchesId IS NULL OR h.BranchesId = :branchesId) "
        + "AND h.DocumentTypeId = 303 AND (:financialYearId IS NULL OR h.FinancialYearId = :financialYearId) "
        + "AND GP.RefDocumentTypeId = 41 AND h.IsApproved = 1" + DD + " AND (ISNULL(h.WeightApplied, 0) = 0)"
        + AR + "LabPurchaseAnalysis" + AR_END
        + " AND NOT EXISTS (SELECT 1 FROM InvGrn gg WHERE gg.OrganizationId = :organizationId AND gg.CompanyId = :companyId "
        + "AND (:financialYearId IS NULL OR gg.FinancialYearId = :financialYearId) AND (:branchesId IS NULL OR gg.BranchesId = :branchesId) "
        + "AND gg.InwardGatePassId = GP.Id)" + HAV
        // -- Lab Sample Analysis
        + " UNION ALL SELECT TypeID = 61, Description = 'Sample Analysis', Count = COUNT(*), ActionId = 1, SortNo = 4 FROM InvLabSampleAnalysisHeader"
        + OC + " AND DocumentTypeId = 302" + DD + " AND (ISNULL(IsApproved, 0) <> 1)" + BRO
        + AR + "LabSampleAnalysis" + AR_END + HAV
        // -- Production Settlement
        + " UNION ALL SELECT TypeID = 62, Description = 'Production Settlement', Count = COUNT(*), ActionId = 2, SortNo = 17 FROM InvProductionJobOrder j "
        + "INNER JOIN (SELECT InvProductionJobOrderId, ISNULL(SUM(QTY), 0) AS IssueQty, ISNULL(SUM(Weight), 0) AS IssueWeight, ISNULL(SUM(TotalAmount), 0) AS IssueAmount "
        + "FROM InvFoodProductionDetail WHERE EntryType = 'Issue' GROUP BY InvProductionJobOrderId) d ON j.Id = d.InvProductionJobOrderId "
        + "INNER JOIN (SELECT InvProductionJobOrderId, ISNULL(SUM(QTY), 0) AS FinishQty, ISNULL(SUM(Weight), 0) AS FinishWeight, ISNULL(SUM(TotalAmount), 0) AS FinishAmount "
        + "FROM InvFoodProductionDetail WHERE EntryType = 'FinishGoods' GROUP BY InvProductionJobOrderId) fg ON j.Id = fg.InvProductionJobOrderId"
        + OC + " AND DocumentTypeId = 403 AND PlanDate >= " + FROM_D + " AND PlanDate < " + TO_D + FYO + " AND (ISNULL(IsApproved, 0) <> 1)"
        + AR + "ProductionSettlement" + AR_END + HAV
        // -- Production Output
        + " UNION ALL SELECT TypeID = 112, Description = 'Production Output', [Count] = COUNT(*), ActionId = 2, SortNo = 17 "
        + "FROM InvFoodProduction H JOIN InvProductionJobOrder J ON H.InvJobOrderId = J.Id "
        + "WHERE H.OrganizationId = :organizationId AND H.CompanyId = :companyId AND H.DocumentTypeId = 112 "
        + "AND H.DocDate >= " + FROM_D + " AND H.DocDate < " + TO_D
        + " AND (:financialYearId IS NULL OR H.FinancialYearId = :financialYearId) AND (J.IsApproved <> 1) "
        + "AND EXISTS (SELECT 1 FROM InvFoodProductionDetail D WHERE D.InvFoodProductionId = H.Id AND ISNULL(D.IsApproved, 0) <> 1) "
        + "AND ISNULL(" + CFG_PROD_OUT_LAB + ", 0) = 1" + HAV
        // -- pcc Sale Order
        + " UNION ALL SELECT TypeID = 77, Description = 'Sale Order', Count = COUNT(*), ActionId = 2, SortNo = 1 FROM pcc.SaleOrder"
        + OC + " AND DocumentTypeId = 1852 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "SaleOrder" + AR_END + HAV
        // -- pcc DeliveryOrder
        + " UNION ALL SELECT TypeID = 78, Description = 'Delivery Order', Count = COUNT(*), ActionId = 2, SortNo = 2 FROM pcc.invDeliveryOrder"
        + OC + " AND DocumentTypeId = 1853 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "DeliveryOrder" + AR_END + HAV
        // -- pcc SaleInvoiceDirect
        + " UNION ALL SELECT TypeID = 79, Description = 'Sale Invoice Direct', Count = COUNT(*), ActionId = 2, SortNo = 3 FROM pcc.invSaleInvoice"
        + OC + " AND DocumentTypeId = 1856 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "SaleInvoiceDirect" + AR_END + HAV
        // -- pcc SaleInvoice
        + " UNION ALL SELECT TypeID = 80, Description = 'Sale Invoice', Count = COUNT(*), ActionId = 2, SortNo = 4 FROM pcc.invSaleInvoice"
        + OC + " AND DocumentTypeId = 1861 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "SaleInvoice" + AR_END + HAV
        // -- pcc SaleInvoiceReturn
        + " UNION ALL SELECT TypeID = 84, Description = 'Sale Invoice Return', Count = COUNT(*), ActionId = 2, SortNo = 5 FROM pcc.invSaleInvoice"
        + OC + " AND DocumentTypeId = 1862 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "SaleInvoiceReturn" + AR_END + HAV
        // -- pcc PurchaseOrderFinish
        + " UNION ALL SELECT TypeID = 81, Description = 'Purchase Order Finish', Count = COUNT(*), ActionId = 1, SortNo = 1 FROM pcc.PurchaseOrder"
        + OC + " AND DocumentTypeId = 1863 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "PurchaseOrderFinish" + AR_END + HAV
        // -- pcc Purchase Invoice Finish
        + " UNION ALL SELECT TypeID = 82, Description = 'Purchase Invoice Finish', Count = COUNT(*), ActionId = 1, SortNo = 2 FROM pcc.InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 1859 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "PurchaseInvoiceFinish" + AR_END + HAV
        // -- pcc Purchase Invoice Return Finish
        + " UNION ALL SELECT TypeID = 83, Description = 'Purchase Invoice Return', Count = COUNT(*), ActionId = 1, SortNo = 3 FROM pcc.InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 1865 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "PurchaseInvoiceReturnFinish" + AR_END + HAV
        // -- Export Sales Contract
        + " UNION ALL SELECT TypeID = 202, Description = 'Export Sales Contract', Count = COUNT(*), ActionId = 3, SortNo = 1 FROM dbo.ExImLcOrder"
        + OC + FYM + " AND DocumentTypeId = 202 AND LcOrderDate >= " + FROM_D + " AND LcOrderDate < " + TO_D
        + " AND ISNULL(IsApproved, 0) <> 1 AND ISNULL(ActionId, 0) <> 3"
        + AR + "ExportSalesContract" + AR_END + HAV
        // -- Delivery Order Export
        + " UNION ALL SELECT TypeID = 64, Description = 'Delivery Order Export', Count = COUNT(*), ActionId = 3, SortNo = 2 FROM [dbo].InvDeliveryOrder"
        + OC + " AND DocumentTypeId = 84 AND DeliveryOrderType = 'Export' AND ISNULL(ActionId, 0) <> 3 AND ISNULL(IsApproved, 0) <> 1" + DD + FYO
        + AR + "DeliveryOrder" + AR_END + HAV
        // -- Export Return Invoice (note: proc uses '@BranchesId IS NULL AND BranchesId = @BranchesId' — kept verbatim)
        + " UNION ALL SELECT TypeID = 242, Description = 'Export Return Invoice', Count = COUNT(*), ActionId = 3, SortNo = 3 FROM [dbo].ExportReturnInvoice"
        + OC + " AND DocumentTypeId = 242 AND IsApproved <> 1" + DD + FYO
        + AR + "ExportReturnInvoice" + AR_END
        + " AND (:branchesId IS NULL AND BranchesId = :branchesId)" + HAV
        // -- Third Party (Lot) Inspection
        + " UNION ALL SELECT TypeID = 999, Description = 'Third Party (Lot) Inspection', Count = COUNT(*), ActionId = 3, SortNo = 4 FROM dbo.InvLabPreProductionExportLotInspectionHeader h "
        + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND h.FinancialYearId = :financialYearId "
        + "AND RequestDate >= " + FROM_D + " AND RequestDate < " + TO_D
        + " AND ISNULL(h.IsApproved, 0) = 0 AND ISNULL(h.ApprovedUserId, 0) = 0 AND ISNULL(h.ActionId, 0) <> 3"
        + AR + "ThirdPartyLotInspection" + AR_END + HAV
        // -- Logistic Rate Negotiation
        + " UNION ALL SELECT TypeID = 1300, Description = 'Logistic Rate Negotiation', Count = COUNT(*), ActionId = 3, SortNo = 5 FROM lgstcm.logisticRateNegotiationHeader"
        + OC + FYM + " AND DocumentTypeId = 1300 AND ActionId <> 3 AND documentDate >= " + FROM_D + " AND documentDate < " + TO_D + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "LogisticRateNegotiation" + AR_END + HAV
        // -- Logistic Agreement
        + " UNION ALL SELECT TypeID = 1301, Description = 'Logistic Agreement', Count = COUNT(*), ActionId = 3, SortNo = 6 FROM lgstcm.AgreementHeader"
        + OC + FYM + " AND DocumentTypeId = 1301 AND ActionId <> 3 AND DocumentDate >= " + FROM_D + " AND DocumentDate < " + TO_D + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "LogisticAgreement" + AR_END + HAV
        // -- Logistic Rate Negotiation For Transporter
        + " UNION ALL SELECT TypeID = 1302, Description = 'Logistic Rate Negotiation For Transporter', Count = COUNT(*), ActionId = 3, SortNo = 7 FROM lgstcm.logisticRateNegotiationHeader"
        + OC + FYM + " AND DocumentTypeId = 1302 AND ActionId <> 3 AND documentDate >= " + FROM_D + " AND documentDate < " + TO_D + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "LogisticRateNegotiationTransporter" + AR_END + HAV
        // -- Logistic Purchase Order
        + " UNION ALL SELECT TypeID = 1303, Description = 'Logistic Purchase Order', Count = COUNT(*), ActionId = 3, SortNo = 8 FROM lgstcm.PurchaseOrderHeader"
        + OC + FYM + " AND DocumentTypeId = 1303 AND ActionId <> 3 AND DocumentDate >= " + FROM_D + " AND DocumentDate < " + TO_D + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "LogisticPurchaseOrder" + AR_END + HAV
        // -- Logistic Services Bill
        + " UNION ALL SELECT TypeID = 1304, Description = 'Logistic Services Bill', Count = COUNT(*), ActionId = 3, SortNo = 9 FROM lgstcm.ServicesBillHeader"
        + OC + FYM + " AND DocumentTypeId = 1304 AND ActionId <> 3 AND DocumentDate >= " + FROM_D + " AND DocumentDate < " + TO_D + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "LogisticPurchaseServicesBill" + AR_END + HAV
        // -- Logistic Services Bill Direct
        + " UNION ALL SELECT TypeID = 155, Description = 'Logistic Services Bill Direct', Count = COUNT(*), ActionId = 3, SortNo = 10 FROM dbo.ExImClearingAgentBill"
        + OC + FYM + " AND DocTypeId = 155" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "ServicesBillDirect" + AR_END + HAV
        // -- Purchase Invoice Trading
        + " UNION ALL SELECT TypeID = 25, Description = 'Purchase Invoice Trading', count = COUNT(*), ActionId = 1, SortNo = 11 FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 58 AND IsApproved <> 1" + DD + FYO
        + AR + "PurchaseInvoice" + AR_END + HAV
        // -- Purchase Invoice Trade Pro
        + " UNION ALL SELECT TypeID = 50, Description = 'Purchase Invoice', Count = COUNT(*), ActionId = 1, SortNo = 12 FROM InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 63 AND ISNULL(IsApproved, 0) <> 1" + DD + FYO
        + AR + "PurchaseInvoiceTrading" + AR_END + HAV
        // -- Purchase Return Trading
        + " UNION ALL SELECT TypeID = 49, Description = 'Purchase Return', Count = COUNT(*), ActionId = 1, SortNo = 13 FROM InvSaleInvoice"
        + OC + " AND DocumentTypeId = 60" + DD + " AND ISNULL(IsApproved, 0) <> 1" + FYO
        + AR + "PurchaseInvoiceReturn" + AR_END + HAV
        // -- Sale Order Trading (82)
        + " UNION ALL SELECT TypeID = 55, Description = 'Sale Order Trading', Count = COUNT(*), ActionId = 2, SortNo = 1 FROM SaleOrder"
        + OC + " AND DocumentTypeId = 82" + DD + " AND ISNULL(IsAproved, 0) <> 1" + FYO
        + AR + "SaleOrder" + AR_END + HAV
        // -- Sales Order Trading (83)
        + " UNION ALL SELECT TypeID = 29, Description = 'Sales Order Trading', Count = COUNT(*), ActionId = 2, SortNo = 2 FROM [dbo].SaleOrder"
        + OC + " AND DocumentTypeId = 83 AND IsAproved <> 1" + DD + FYO
        + AR + "SaleOrder" + AR_END + HAV
        // -- Sale Invoice Trading
        + " UNION ALL SELECT TypeID = 34, Description = 'Sale Invoice Trading', Count = COUNT(*), ActionId = 2, SortNo = 3 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 103 AND IsApproved <> 1" + DD + FYO
        + AR + "SaleInvoice" + AR_END + HAV
        // -- Sale Invoice Store
        + " UNION ALL SELECT TypeID = 126, Description = 'Sale Invoice Store', Count = COUNT(*), ActionId = 2, SortNo = 3 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 126 AND ISNULL(BaseDocumentTypeId, 1) = 1 AND IsApproved <> 1"
        + " AND (" + FROM_D + " IS NULL OR DocDate >= " + FROM_D + ") AND (" + TO_D + " IS NULL OR DocDate < " + TO_D + ")" + FYO
        + AR + "SaleInvoiceStore" + AR_END + HAV
        // -- Sale Invoice Packing Material
        + " UNION ALL SELECT TypeID = 127, Description = 'Sale Invoice Packing Material', Count = COUNT(*), ActionId = 2, SortNo = 3 FROM [dbo].InvSaleInvoice"
        + OC + " AND DocumentTypeId = 126 AND BaseDocumentTypeId = 2 AND IsApproved <> 1"
        + " AND (" + FROM_D + " IS NULL OR DocDate >= " + FROM_D + ") AND (" + TO_D + " IS NULL OR DocDate < " + TO_D + ")" + FYO
        + AR + "SaleInvoicePM" + AR_END + HAV
        // -- Sale Return Trading
        + " UNION ALL SELECT TypeID = 48, Description = 'Sale Return', Count = COUNT(*), ActionId = 2, SortNo = 4 FROM InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 104 AND ISNULL(IsApproved, 0) <> 1" + DD + FYO + BW_SIR
        + AR + "SaleInvoiceReturn" + AR_END + HAV
        // -- Supply Order
        + " UNION ALL SELECT TypeID = 41, Description = 'Supply Order', Count = COUNT(*), ActionId = 4, SortNo = 1 FROM InvSupplyOrder"
        + OC + " AND DocumentTypeId = 104 AND IsApproved <> 1" + DD + FYO
        + AR + "SupplyOrder" + AR_END + HAV
        // -- Wsrm Requisition Order
        + " UNION ALL SELECT TypeID = 42, Description = 'Requisition Order', Count = COUNT(*), ActionId = 4, SortNo = 2 FROM WsRmRequisitionPo "
        + "WHERE OrganizationId = :organizationId AND DestinationLocationId = :companyId AND ISNULL(IsApproved, 0) <> 1"
        + AR + "RequisitionOrder" + AR_END + HAV
        // -- Import Invoice
        + " UNION ALL SELECT TypeID = 43, Description = 'Import Invoice', Count = COUNT(*), ActionId = 4, SortNo = 3 FROM ImInvoice"
        + OC + " AND ISNULL(IsApproved, 0) <> 1" + DD
        + AR + "ImportInvoice" + AR_END + HAV
        // -- Wsrm Sale Order
        + " UNION ALL SELECT TypeID = 44, Description = 'Wsrm Sale Order', Count = COUNT(*), ActionId = 4, SortNo = 4 FROM WsRmSalesOrder"
        + OC + " AND ISNULL(IsApproved, 0) <> 1" + DD
        + AR + "WsrmSaleOrder" + AR_END + HAV
        // -- Wsrm Sale Invoice
        + " UNION ALL SELECT TypeID = 45, Description = 'Wsrm Sale Invoice', Count = COUNT(*), ActionId = 4, SortNo = 5 FROM WsRmSaleInvoice"
        + OC + " AND ISNULL(IsApproved, 0) <> 1" + DD
        + AR + "WsrmSaleInvoice" + AR_END + HAV
        // -- Import Contract
        + " UNION ALL SELECT TypeID = 46, Description = 'Import Contract', Count = COUNT(*), ActionId = 4, SortNo = 6 FROM ImLcOrder"
        + OC + " AND ISNULL(IsApproved, 0) <> 1" + DD
        + AR + "ImportContract" + AR_END + HAV
        // -- Import Purchase Order
        + " UNION ALL SELECT TypeID = 47, Description = 'Import Purchase Order', Count = COUNT(*), ActionId = 4, SortNo = 7 FROM ExImLcOrderPurchaseOrder"
        + OC + " AND DocumentTypeId = 236 AND ISNULL(IsApproved, 0) <> 1" + DD
        + AR + "ImportPurchaseOrder" + AR_END + HAV
        // -- Stock Transfer Notes
        + " UNION ALL SELECT TypeID = 52, Description = 'Stock Transfer Notes', Count = COUNT(*), ActionId = 4, SortNo = 8 FROM WsRmStockTransferNotes"
        + OC + " AND ISNULL(IsApproved, 0) <> 1" + DD
        + AR + "StockTransfertNotes" + AR_END + HAV
        // -- Stock Received Notes
        + " UNION ALL SELECT TypeID = 53, Description = 'Stock Received Notes', Count = COUNT(*), ActionId = 4, SortNo = 9 FROM WsRmStockReceivedNotes"
        + OC + " AND ISNULL(IsApproved, 0) <> 1" + DD
        + AR + "StockReceivedNotes" + AR_END + HAV
        // -- Work Order
        + " UNION ALL SELECT TypeID = 1870, Description = 'Work Order', Count = COUNT(*), ActionId = 4, SortNo = 10 FROM pcc.WorkOrderHeader"
        + OC + FYM + " AND DocumentTypeId = 1870 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "WorkOrder" + AR_END + HAV
        // -- Commission Sale Order
        + " UNION ALL SELECT TypeID = 1053, Description = 'Commission Sale Order', Count = COUNT(*), ActionId = 5, SortNo = 1 FROM cmagt.saleOrderMaster"
        + OC + FYM + " AND DocumentTypeId = 1053 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "CommissionSaleOrder" + AR_END + HAV
        // -- Commission Purchase Order
        + " UNION ALL SELECT TypeID = 1052, Description = 'Commission Purchase Order', Count = COUNT(*), ActionId = 5, SortNo = 2 FROM cmagt.PurchaseOrdermaster"
        + OC + FYM + " AND DocumentTypeId = 1052 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "CommissionPurchaseOrder" + AR_END + HAV
        // -- Commission Trade Bill
        + " UNION ALL SELECT TypeID = 1056, Description = 'Commission Trade Bill', Count = COUNT(*), ActionId = 5, SortNo = 3 FROM dbo.InvCommAgentTradeBill"
        + OC + FYM + " AND DocumentTypeId = 1056 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) <> 1"
        + AR + "CommissionTradeBill" + AR_END + HAV
        + PENDING_TAIL,
        nativeQuery = true)
    List<DashboardPendingForApprovalRow> readDashboardPendingForApprovalPart2(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("userId") Integer userId,
            @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("appId") Integer appId);

    // ------------------------------------------------------------------------------------------------
    // Sp_Inventory_ReadDashboardApprovedVouchersForUnPost
    // ------------------------------------------------------------------------------------------------

    /**
     * Sp_Inventory_ReadDashboardApprovedVouchersForUnPost (single branch) — counts of APPROVED vouchers eligible for un-post.
     * Proc defaults: @FinancialYearId = NULL, @FromDate = NULL, @ToDate = NULL (@UserId is mandatory in the proc).
     * Native: #ApprovalRights -> CTE; @ProductionOutputLabApprovalRequired inlined; UNION ALL of 21 per-document-type counts;
     * LEFT JOIN InventoryApprovedParams (no ORDER BY in the proc).
     */
    @Query(value = RIGHTS_CTE
        + "SELECT A.TypeID, A.Description, A.Count, "
        + ":organizationId OrganizationId, :companyId CompanyId, R.BranchesId, :financialYearId FinancialYearId, :userId EntryUser, "
        + "R.PostDate, R.Activity, R.RecordId, R.DocumentTypeId, R.ReqType, R.ApprovedFilter, R.CompanyName, R.CompanyAddress, "
        + "R.ReportName, R.VoucherReportName, R.ApiEndPoint, R.ApprovedEndPoint, R.PrintEndPoint, R.PrintEndPointVoucher, A.ActionId "
        + "FROM ( "
        // -- Purchase Invoice
        + "SELECT TypeID = 23, Description = 'Purchase Invoice', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvPurchaseInvoice"
        + OC + FYO + " AND DocumentTypeId = 56 AND IsApproved = 1" + DD
        + AR + "PurchaseInvoice" + AR_END + HAV
        // -- Purchase Invoice Direct
        + " UNION ALL SELECT TypeID = 24, Description = 'Purchase Invoice Direct', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvPurchaseInvoice"
        + OC + FYO + " AND DocumentTypeId = 57 AND IsApproved = 1" + DD
        + AR + "PurchaseInvoiceDirect" + AR_END + HAV
        // -- Purchase Invoice Against Direct Grn
        + " UNION ALL SELECT TypeID = 138, Description = 'Purchase Invoice Against Direct Grn', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 138 AND IsApproved = 1" + DD + FYO
        + AR + "PurchaseInvoiceAgainstGrnDirect" + AR_END + HAV
        // -- Purchase Invoice Return
        + " UNION ALL SELECT TypeID = 26, Description = 'Purchase Return', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvSaleInvoice"
        + OC + FYO + " AND DocumentTypeId = 59 AND IsApproved = 1" + DD
        + AR + "PurchaseInvoiceReturn" + AR_END + HAV
        // -- Sale Invoice
        + " UNION ALL SELECT TypeID = 32, 'Sale Invoice', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvSaleInvoice"
        + OC + FYO + " AND DocumentTypeId = 95 AND IsApproved = 1" + DD
        + AR + "SaleInvoice" + AR_END + HAV
        // -- Sale Invoice Direct
        + " UNION ALL SELECT TypeID = 33, 'Sale Invoice Direct', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvSaleInvoice"
        + OC + FYO + " AND DocumentTypeId = 99 AND IsApproved = 1" + DD
        + AR + "SaleInvoiceDirect" + AR_END + HAV
        // -- Sale Invoice Return
        + " UNION ALL SELECT TypeID = 36, 'Sale Return Invoice ', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvPurchaseInvoice"
        + OC + FYO + " AND DocumentTypeId = 98 AND IsApproved = 1" + DD
        + AR + "SaleInvoiceReturn" + AR_END + HAV
        // -- Goods Receiving Notes
        + " UNION ALL SELECT TypeID = 37, 'Goods Receiving Notes', Count = COUNT(*), 1 AS ActionId FROM InvGrn "
        + "INNER JOIN (SELECT InvGrnId FROM InvGrnDetail GROUP BY InvGrnId) d ON InvGrn.Id = d.InvGrnId"
        + OC + FYO + " AND DocumentTypeId IN (1602, 1618) AND ISNULL(IsApproved, 0) = 1" + DD
        + " AND (NOT EXISTS (SELECT 1 FROM InvPurchaseInvoiceDetail INNER JOIN InvPurchaseInvoice ON InvPurchaseInvoice.Id = InvPurchaseInvoiceId "
        + "WHERE InvGrn.Id = InvPurchaseInvoiceDetail.InvGrnId AND OrganizationId = :organizationId AND CompanyId = :companyId))"
        + AR + "Goods Receiving Notes" + AR_END + HAV
        // -- Good Dispatch Notes
        + " UNION ALL SELECT TypeID = 38, 'Good Dispatch Notes', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvGdn h "
        + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId" + FYO
        + " AND h.DocumentTypeId IN (1612, 1607) AND h.IsApproved = 1" + DD
        + " AND (NOT EXISTS (SELECT 1 FROM InvSaleInvoiceDetail INNER JOIN InvSaleInvoice ON InvSaleInvoice.Id = InvSaleInvoiceId "
        + "WHERE OrganizationId = h.OrganizationId AND CompanyId = h.CompanyId AND InvSaleInvoiceDetail.InvGdnId = h.Id))"
        + AR + "GoodsDispatchedNotes" + AR_END + HAV
        // -- PM_PurchaseInvoice
        + " UNION ALL SELECT TypeID = 39, Description = 'PM_PurchaseInvoice', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 702 AND IsApproved = 1" + DD + FYO
        + AR + "PM_PurchaseInvoice" + AR_END + HAV
        // -- Purchase Demand Store
        + " UNION ALL SELECT TypeID = 188, 'Purchase Demand Store', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvPurchaseDemandHeader"
        + OC + " AND DocumentTypeId = 141 AND ISNULL(IsApproved, 0) = 1" + DD + " AND ISNULL(ParentCategoryId, 0) IN (0, 8)" + FYO
        + AR + "PurchaseDemand" + AR_END + HAV
        // -- Purchase Demand Packing Material
        + " UNION ALL SELECT TypeID = 141, 'Purchase Demand Packing Material', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvPurchaseDemandHeader"
        + OC + " AND DocumentTypeId = 141" + DD + " AND ISNULL(IsApproved, 0) = 1 AND ISNULL(ParentCategoryId, 0) = 7" + FYO
        + AR + "PurchaseDemandPM" + AR_END + HAV
        // -- Production Settlement
        + " UNION ALL SELECT TypeID = 62, 'Production Settlement', Count = COUNT(*), 1 AS ActionId FROM InvProductionJobOrder j "
        + "INNER JOIN (SELECT InvProductionJobOrderId, ISNULL(SUM(QTY), 0) AS IssueQty, ISNULL(SUM(Weight), 0) AS IssueWeight, ISNULL(SUM(TotalAmount), 0) AS IssueAmount "
        + "FROM InvFoodProductionDetail WHERE EntryType = 'Issue' GROUP BY InvProductionJobOrderId) d ON j.Id = d.InvProductionJobOrderId "
        + "LEFT JOIN (SELECT InvProductionJobOrderId, ISNULL(SUM(QTY), 0) AS ByProductQty, ISNULL(SUM(Weight), 0) AS ByProductWeight, ISNULL(SUM(TotalAmount), 0) AS ByProductAmount "
        + "FROM InvFoodProductionDetail WHERE EntryType = 'ByProduct' GROUP BY InvProductionJobOrderId) b ON j.Id = b.InvProductionJobOrderId "
        + "INNER JOIN (SELECT InvProductionJobOrderId, ISNULL(SUM(QTY), 0) AS FinishQty, ISNULL(SUM(Weight), 0) AS FinishWeight, ISNULL(SUM(TotalAmount), 0) AS FinishAmount "
        + "FROM InvFoodProductionDetail WHERE EntryType = 'FinishGoods' GROUP BY InvProductionJobOrderId) fg ON j.Id = fg.InvProductionJobOrderId"
        + OC + " AND DocumentTypeId = 403 AND IsApproved = 1 AND PlanDate >= " + FROM_D + " AND PlanDate < " + TO_D + FYO
        + AR + "ProductionSettlement" + AR_END + HAV
        // -- PM_PurchaseInvoiceDirect
        + " UNION ALL SELECT TypeID = 245, Description = 'PM_PurchaseInvoiceDirect', Count = COUNT(*), 1 AS ActionId FROM [dbo].InvPurchaseInvoice"
        + OC + " AND DocumentTypeId = 245 AND IsApproved = 1" + DD + FYO
        + AR + "PM_PurchaseInvoiceDirect" + AR_END + HAV
        // -- Purchase Invoice Return Store
        + " UNION ALL SELECT TypeID = 145, Description = 'Purchase Invoice Return Store', Count = COUNT(*), ActionId = 1 FROM dbo.InvSaleInvoice"
        + OC + " AND DocumentTypeId = 145 AND ISNULL(BaseDocumentTypeId, 0) = 1 AND ISNULL(ActionId, 0) <> 3" + DD + " AND ISNULL(IsApproved, 0) = 1"
        + AR + "PurchaseInvoiceReturn_Store" + AR_END + HAV
        // -- Purchase Invoice Return PM
        + " UNION ALL SELECT TypeID = 146, Description = 'Purchase Invoice Return PM', Count = COUNT(*), ActionId = 1 FROM dbo.InvSaleInvoice"
        + OC + " AND DocumentTypeId = 145 AND ISNULL(BaseDocumentTypeId, 0) = 2 AND ISNULL(ActionId, 0) <> 3" + DD + " AND ISNULL(IsApproved, 0) = 1"
        + AR + "PurchaseInvoiceReturnPM" + AR_END + HAV
        // -- Work Order
        + " UNION ALL SELECT TypeID = 1870, 'Work Order', Count = COUNT(*), 1 AS ActionId FROM pcc.WorkOrderHeader"
        + OC + FYM + " AND DocumentTypeId = 1870 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) = 1"
        + AR + "WorkOrder" + AR_END + HAV
        // -- Commission Sale Order
        + " UNION ALL SELECT TypeID = 1053, Description = 'Commission Sale Order', Count = COUNT(*), ActionId = 2 FROM cmagt.saleOrderMaster"
        + OC + FYM + " AND DocumentTypeId = 1053 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) = 1"
        + AR + "CommissionSaleOrder" + AR_END + HAV
        // -- Commission Purchase Order
        + " UNION ALL SELECT TypeID = 1052, Description = 'Commission Purchase Order', Count = COUNT(*), ActionId = 2 FROM cmagt.PurchaseOrdermaster"
        + OC + FYM + " AND DocumentTypeId = 1052 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) = 1"
        + AR + "CommissionPurchaseOrder" + AR_END + HAV
        // -- Commission Trade Bill
        + " UNION ALL SELECT TypeID = 1056, Description = 'Commission Trade Bill', Count = COUNT(*), ActionId = 2 FROM dbo.InvCommAgentTradeBill"
        + OC + FYM + " AND DocumentTypeId = 1056 AND ActionId <> 3" + DD + " AND ISNULL(IsApproved, 0) = 1"
        + AR + "CommissionTradeBill" + AR_END + HAV
        // -- Production Output
        + " UNION ALL SELECT TypeID = 112, Description = 'Production Output', [Count] = COUNT(*), ActionId = 1 "
        + "FROM InvFoodProduction H INNER JOIN InvProductionJobOrder J ON H.InvJobOrderId = J.Id "
        + "WHERE H.OrganizationId = :organizationId AND H.CompanyId = :companyId AND H.DocumentTypeId = 112 "
        + "AND H.DocDate >= " + FROM_D + " AND H.DocDate < " + TO_D
        + " AND (:financialYearId IS NULL OR H.FinancialYearId = :financialYearId) "
        + "AND ISNULL(" + CFG_PROD_OUT_LAB + ", 0) = 1 AND ISNULL(J.IsApproved, 0) <> 1 "
        + "AND EXISTS (SELECT 1 FROM InvFoodProductionDetail D WHERE D.InvFoodProductionId = H.Id AND ISNULL(D.IsApproved, 0) = 1)" + HAV
        + " ) A LEFT JOIN InventoryApprovedParams R ON A.TypeID = R.TypeId",
        nativeQuery = true)
    List<DashboardApprovedVouchersForUnPostRow> readDashboardApprovedVouchersForUnPost(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("userId") Integer userId,
            @Param("financialYearId") Integer financialYearId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate);

    // ------------------------------------------------------------------------------------------------
    // usp_ReadDashboardPendingForSpecialApprovals
    // ------------------------------------------------------------------------------------------------

    /**
     * usp_ReadDashboardPendingForSpecialApprovals (single branch) — pending special-approval counts with reason text.
     * Proc defaults: @UserId = NULL, @FinancialYearId = NULL, @BranchesId = NULL.
     * Native: @ToleranceWeightForDifferenceInDispatchedAndInvoice (float from ConfigrationsAllocation, ConfigrationsDefinitionId 5618)
     * is the CTE {@code Tol}; the proc's '@CompanyId = @CompanyId' predicate is kept verbatim. The proc also builds #ApprovalRights
     * (ScreenName 'SpecialApprovalDashboard'), reads config 5070 and SpecialApprovalReason Id=1 into variables, but none of them
     * is referenced by the final SELECT, so they are dropped (which is why @UserId is not a parameter here).
     */
    @Query(value =
          "WITH Tol AS (SELECT CONVERT(FLOAT, (SELECT ConfigKey FROM ConfigrationsAllocation "
        + "WHERE OrganizationId = :organizationId AND :companyId = :companyId AND ConfigrationsDefinitionId = 5618)) AS ToleranceWeight) "
        + "SELECT a.TypeID, a.Description, a.Count, a.ActionId, a.ReasonId, SR.SpecialApprovalReason AS ReasonForSpecialApproval "
        + "FROM ( "
        // -- Stock Transfer
        + "SELECT TypeID = 1, Description = 'Stock Transfer', Count = COUNT(Id), 2 AS ActionId, 0 AS ReasonId FROM [dbo].[InvStockTransferHeader]"
        + OC + " AND DocumentTypeId = 68" + FYO + " AND ISNULL(SpecialApproval, 0) = 1 AND ISNULL(SpecialApprovalUserId, 0) = 0 "
        + "HAVING COUNT(Id) > 0"
        // -- GatePassInward
        + " UNION ALL SELECT TypeID = 2, 'Inward GatePass', Count = COUNT(*), 2 AS ActionId, 1 AS ReasonId FROM GatePassInward"
        + OC + " AND DocumentTypeId = 51" + FYO + BRO + " AND ISNULL(AccessWeight, 0) > 0 AND ActionIdForSpecialApproval = 1" + HAV
        // -- GRN: Due To Purchase Order Excess Weight (ReasonId 1)
        + " UNION ALL SELECT TypeID = 3, 'Goods Receiving Notes', Count = COUNT(*), 2 AS ActionId, 1 AS ReasonId FROM InvGrn "
        + "LEFT JOIN GatePassInward gp ON gp.Id = InvGrn.InwardGatePassId "
        + "INNER JOIN SpecialApprovalLog lg ON lg.RefDocumentTypeId = InvGrn.DocumentTypeId AND lg.RefDocId = InvGrn.Id AND lg.ActionId <> 3 "
        + "WHERE InvGrn.OrganizationId = :organizationId AND InvGrn.CompanyId = :companyId AND (:branchesId IS NULL OR InvGrn.BranchesId = :branchesId) "
        + "AND InvGrn.DocumentTypeId IN (46, 137, 217, 143) AND ISNULL(lg.IsApproved, 0) = 0 AND ISNULL(lg.ReasonId, 0) = 1" + HAV
        // -- GRN: Bill Weight exceeds Stock Weight (ReasonId 2)
        + " UNION ALL SELECT TypeID = 3, 'Goods Receiving Notes', Count = COUNT(*), 2 AS ActionId, 2 AS ReasonId FROM InvGrn "
        + "LEFT JOIN GatePassInward gp ON gp.Id = InvGrn.InwardGatePassId "
        + "INNER JOIN SpecialApprovalLog lg ON lg.RefDocumentTypeId = InvGrn.DocumentTypeId AND lg.RefDocId = InvGrn.Id AND lg.ActionId <> 3 "
        + "WHERE InvGrn.OrganizationId = :organizationId AND InvGrn.CompanyId = :companyId AND (:branchesId IS NULL OR InvGrn.BranchesId = :branchesId) "
        + "AND InvGrn.DocumentTypeId IN (46, 137, 217, 143) AND ISNULL(lg.IsApproved, 0) = 0 AND ISNULL(lg.ReasonId, 0) = 2" + HAV
        // -- GRN: EmptyBag Weight Total is Zero (ReasonId 3)
        + " UNION ALL SELECT TypeID = 3, 'Goods Receiving Notes', Count = COUNT(*), 2 AS ActionId, 3 AS ReasonId FROM InvGrn "
        + "LEFT JOIN GatePassInward gp ON gp.Id = InvGrn.InwardGatePassId "
        + "INNER JOIN SpecialApprovalLog lg ON lg.RefDocumentTypeId = InvGrn.DocumentTypeId AND lg.RefDocId = InvGrn.Id AND lg.ActionId <> 3 "
        + "WHERE InvGrn.OrganizationId = :organizationId AND InvGrn.CompanyId = :companyId AND (:branchesId IS NULL OR InvGrn.BranchesId = :branchesId) "
        + "AND InvGrn.DocumentTypeId IN (46, 137, 217, 143) AND ISNULL(lg.IsApproved, 0) = 0 AND ISNULL(lg.ReasonId, 0) = 3" + HAV
        // -- GRN: Weight Difference Between Supplier and Factory Exceeds Allowed Tolerance (ReasonId 4)
        + " UNION ALL SELECT TypeID = 3, 'Goods Receiving Notes', Count = COUNT(*), 2 AS ActionId, 4 AS ReasonId FROM InvGrn "
        + "LEFT JOIN GatePassInward gp ON gp.Id = InvGrn.InwardGatePassId "
        + "INNER JOIN SpecialApprovalLog lg ON lg.RefDocumentTypeId = InvGrn.DocumentTypeId AND lg.RefDocId = InvGrn.Id AND lg.ActionId <> 3 "
        + "WHERE InvGrn.OrganizationId = :organizationId AND InvGrn.CompanyId = :companyId AND (:branchesId IS NULL OR InvGrn.BranchesId = :branchesId) "
        + "AND InvGrn.DocumentTypeId IN (46, 137, 217, 143) AND ISNULL(lg.IsApproved, 0) = 0 AND ISNULL(lg.ReasonId, 0) = 4" + HAV
        // -- GRN: Rate Cut with Excess Weight (ReasonId 5)
        + " UNION ALL SELECT TypeID = 3, 'Goods Receiving Notes', Count = COUNT(*), 2 AS ActionId, 5 AS ReasonId FROM InvGrn "
        + "LEFT JOIN GatePassInward gp ON gp.Id = InvGrn.InwardGatePassId "
        + "INNER JOIN SpecialApprovalLog lg ON lg.RefDocumentTypeId = InvGrn.DocumentTypeId AND lg.RefDocId = InvGrn.Id AND lg.ActionId <> 3 "
        + "WHERE InvGrn.OrganizationId = :organizationId AND InvGrn.CompanyId = :companyId AND (:branchesId IS NULL OR InvGrn.BranchesId = :branchesId) "
        + "AND InvGrn.DocumentTypeId IN (46, 137, 217, 143) AND ISNULL(lg.IsApproved, 0) = 0 AND ISNULL(lg.ReasonId, 0) = 5" + HAV
        // -- Production Settlement
        + " UNION ALL SELECT TypeID = 4, 'Production Settlement', Count = COUNT(*), 2 AS ActionId, 0 AS ReasonId FROM InvProductionJobOrder j "
        + "INNER JOIN (SELECT InvProductionJobOrderId, ISNULL(SUM(QTY), 0) AS IssueQty, ISNULL(SUM(Weight), 0) AS IssueWeight, ISNULL(SUM(TotalAmount), 0) AS IssueAmount "
        + "FROM InvFoodProductionDetail WHERE EntryType = 'Issue' GROUP BY InvProductionJobOrderId) d ON j.Id = d.InvProductionJobOrderId "
        + "LEFT JOIN (SELECT InvProductionJobOrderId, ISNULL(SUM(QTY), 0) AS ByProductQty, ISNULL(SUM(Weight), 0) AS ByProductWeight, ISNULL(SUM(TotalAmount), 0) AS ByProductAmount "
        + "FROM InvFoodProductionDetail WHERE EntryType = 'ByProduct' GROUP BY InvProductionJobOrderId) b ON j.Id = b.InvProductionJobOrderId "
        + "INNER JOIN (SELECT InvProductionJobOrderId, ISNULL(SUM(QTY), 0) AS FinishQty, ISNULL(SUM(Weight), 0) AS FinishWeight, ISNULL(SUM(TotalAmount), 0) AS FinishAmount "
        + "FROM InvFoodProductionDetail WHERE EntryType = 'FinishGoods' GROUP BY InvProductionJobOrderId) fg ON j.Id = fg.InvProductionJobOrderId"
        + OC + " AND DocumentTypeId = 403" + FYO + " AND (ISNULL(SpecialApprovalId, 0) = 1) AND (ISNULL(SpecialApprovalUserId, 0) = 0)" + HAV
        // -- Commercial Invoice
        + " UNION ALL SELECT TypeID = 19, 'Commercial Invoice', Count = COUNT(*), 2 AS ActionId, 0 AS ReasonId FROM dbo.ExImInvoice R "
        + "INNER JOIN (SELECT d.ExImInvoiceId, ISNULL(SUM(d.NetWeight), 0) AS NetWeight FROM ExImInvoicePackingDetail d WHERE ISNULL(d.ActionTypeId, 0) <> 3 GROUP BY d.ExImInvoiceId) d ON d.ExImInvoiceId = R.Id "
        + "LEFT JOIN (SELECT fd.InvoiceId, ISNULL(SUM(fd.NetWeight), 0) AS NetWeight FROM ExImForwardingPackingDetail fd WHERE ISNULL(fd.ActionTypeId, 0) <> 3 GROUP BY fd.InvoiceId) fd ON fd.InvoiceId = R.Id "
        + "WHERE R.FinancialYearId = :financialYearId AND R.OrganizationId = :organizationId AND R.CompanyId = :companyId "
        + "AND ISNULL(R.ActionId, 0) <> 3 AND R.DocumentTypeId = 204 "
        + "AND (d.NetWeight != fd.NetWeight "
        + "AND ISNULL((SELECT t.ToleranceWeight FROM Tol t), 0) > 0 "
        + "AND ISNULL(R.IsSpecialApproved, 0) = 0 "
        + "AND d.NetWeight <= fd.NetWeight + ISNULL((SELECT t.ToleranceWeight FROM Tol t), 0))" + HAV
        // -- Export Invoice Voucher
        + " UNION ALL SELECT TypeID = 27, 'Export Invoice Voucher', Count = COUNT(*), 1 AS ActionId, 0 AS ReasonId FROM DBO.ExportVoucher "
        + "WHERE FinancialYearId = :financialYearId AND OrganizationId = :organizationId AND CompanyId = :companyId AND DocumentTypeId = 27 "
        + "AND ISNULL(IsSpecialApproved, 0) = 0 AND ISNULL(IsSpecialApprovalRequired, 0) = 1" + HAV
        // -- Commission Agent Trade Bill: Sale Weight Less Than Purchase Weight (ReasonId 6)
        + " UNION ALL SELECT TypeID = 1056, 'Commission Agent Trade Bill', Count = COUNT(*), 3 AS ActionId, 6 AS ReasonId FROM InvCommAgentTradeBill h "
        + "INNER JOIN SpecialApprovalLog lg ON lg.RefDocumentTypeId = h.DocumentTypeId AND lg.RefDocId = h.Id AND lg.ActionId <> 3 "
        + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND (:branchesId IS NULL OR h.BranchesId = :branchesId) "
        + "AND h.DocumentTypeId IN (1056) AND ISNULL(lg.IsApproved, 0) = 0 AND ISNULL(lg.ReasonId, 0) = 6 AND h.ActionId <> 3" + HAV
        // -- Commission Agent Trade Bill: Sale Weight Greater Than Purchase Weight (ReasonId 7)
        + " UNION ALL SELECT TypeID = 1056, 'Commission Agent Trade Bill', Count = COUNT(*), 3 AS ActionId, 7 AS ReasonId FROM InvCommAgentTradeBill h "
        + "INNER JOIN SpecialApprovalLog lg ON lg.RefDocumentTypeId = h.DocumentTypeId AND lg.RefDocId = h.Id AND lg.ActionId <> 3 "
        + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND (:branchesId IS NULL OR h.BranchesId = :branchesId) "
        + "AND h.DocumentTypeId IN (1056) AND ISNULL(lg.IsApproved, 0) = 0 AND ISNULL(lg.ReasonId, 0) = 7 AND h.ActionId <> 3" + HAV
        + " ) a LEFT JOIN SpecialApprovalReason SR ON a.ReasonId = SR.Id",
        nativeQuery = true)
    List<DashboardPendingForSpecialApprovalsRow> readDashboardPendingForSpecialApprovals(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("financialYearId") Integer financialYearId,
            @Param("branchesId") Integer branchesId);
}
