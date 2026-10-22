package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ChartofAccount;
import com.mst.projections.PayablesAgingCaptionRow;
import com.mst.projections.PayablesPaymentScheduleRow;
import com.mst.projections.ReceivablesReceiptsScheduleRow;

/**
 * Query-only repository (bound to the existing ChartofAccount entity) for the payment / receipt schedule
 * reports of batch M2: usp_PayablesAndPaymentSchedule and usp_ReceivablesAndReceiptsSchedule.
 * Both procedures are linear temp-table pipelines (#Final -> #MyCredits/#MyDebits -> trade parties ->
 * order / received(dispatch) quantities -> #TmpResult -> #AllData due balance -> #TmpNotYetDue -> final)
 * and are rewritten as ONE native statement each; every UPDATE became a LEFT JOIN + CASE computed column
 * and the temp-table column types the procedures rely on (INT truncation of quantities/amounts,
 * NVARCHAR(50) dates, DECIMAL(18,4)/(18,2)) are reproduced with CASTs. KEEP-EXEC fallbacks are provided.
 * Both procedures hard-code @ActionId = 1 and @TradeTypeId = 1 internally (the parameters are ignored),
 * and @ParentCategoryId is unused.
 */
@Repository
public interface AccountsScheduleQueryRepository extends org.springframework.data.repository.Repository<ChartofAccount, Integer> {

    // ------------------------------------------------------------------ usp_PayablesAndPaymentSchedule

    /**
     * usp_PayablesAndPaymentSchedule (single branch) -- FIRST result set: supplier accounts (AccountTypeId 3,
     * AccountClass 3, credit closing balances) with Opening / CurrDebit / -CurrCredit / Closing, DueBalance
     * (opening + purchase invoices by DueDate + other vouchers + payments in the due window, capped by
     * |Closing| at DECIMAL(18,0)), NotYetDue (invoices due after @DueDateTo, capped by |Closing| - DueBalance),
     * last bill / last payment date and amount, purchase qty / weight / amount (InvPurchaseInvoiceDetail),
     * order vs received quantities (PurchaseOrderDetail vs InvGrnDetail + un-GRN'd InvPurchaseInvoiceDetail)
     * and the echoed date parameters, ordered by ABS(DueBalance) DESC.
     * Native CTE rewrite (Final0, MyCredits, MyDebits, TradeParties, OnlyPurchaseParties, TmpPurchaseOrder,
     * TotalPurchaseOrder, ReceivedData, TotalReceivedData, PartyGroup, Final, TmpResult, TempIds, AllData,
     * DueBalance, NotYetDueT, Res2). The procedure's `UPDATE ... PartyGroupId = SP.CustomerGroupId` picks an
     * arbitrary SupplierCustomer when several share a GlAccountId; this statement picks the lowest Id.
     * Parameters not used by the first result set: @AgingDays (second set only; see
     * {@link #payablesAgingCaptions(Integer)}), @ActionId / @TradeTypeId (overwritten), @ParentCategoryId.
     */
    @Query(value = "WITH AccStatus AS ( "
            + "SELECT CASE WHEN EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) "
            + "THEN (SELECT TOP 1 AccountStatus FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) ELSE 0 END AS AccountStatus "
            + "), BrancheIds AS ( "
            + "SELECT data AS BranchId FROM dbo.fnSplitString(:branchesIds, ',') "
            + "), Final0 AS ( "
            + "SELECT f.AccountId, SUM(f.Opening) AS Opening, SUM(f.CurrDebit) AS CurrDebit, SUM(f.CurrCredit) AS CurrCredit, "
            + "SUM(f.Opening) + SUM(f.CurrDebit) - SUM(f.CurrCredit) AS Closing, "
            + "f.AccountTypeId, f.AccountClass, C.ParentCodeId, f.BranchesId "
            + "FROM ( "
            + "SELECT a.AccountId, a.BranchesId, SUM(a.Debit) - SUM(a.Credit) AS Opening, 0 AS CurrDebit, 0 AS CurrCredit, 0 AS Closing, a.AccountTypeId, a.AccountClass "
            + "FROM ( "
            + "SELECT o.ChartOfAccountId AS AccountId, 0 AS BranchesId, ISNULL(SUM(o.YearObDebit), 0) AS Debit, ISNULL(SUM(o.YearObCredit), 0) AS Credit, c.AccountTypeId, c.AccountClass "
            + "FROM AccountsOpeningBalances o INNER JOIN ChartofAccount c ON o.ChartOfAccountId = c.Id "
            + "WHERE o.OrganizationId = :organizationId AND o.CompanyId = :companyId AND c.AccountGroup = 'DETAIL' AND c.AccountTypeId = 3 AND c.AccountClass = 3 "
            + "GROUP BY o.ChartOfAccountId, c.AccountTypeId, c.AccountClass "
            + "UNION ALL "
            + "SELECT d.AccountId, CASE WHEN :branchesIds IS NULL THEN 0 ELSE d.BranchesId END AS BranchesId, "
            + "ISNULL(SUM(d.DebitAmount), 0), ISNULL(SUM(d.CreditAmount), 0), c.AccountTypeId, c.AccountClass "
            + "FROM VoucherHead h INNER JOIN VoucherDetail d ON h.Id = d.VoucherHeadId INNER JOIN ChartofAccount c ON d.AccountId = c.Id "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND c.AccountGroup = 'DETAIL' AND c.AccountTypeId = 3 AND c.AccountClass = 3 "
            + "AND (:fromDate IS NULL OR CONVERT(DATE, h.VoucherDate) < CONVERT(DATE, :fromDate)) "
            + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM BrancheIds WHERE BranchId = d.BranchesId)) "
            + "GROUP BY d.AccountId, c.AccountTypeId, c.AccountClass, CASE WHEN :branchesIds IS NULL THEN 0 ELSE d.BranchesId END "
            + ") a GROUP BY a.AccountId, a.AccountTypeId, a.AccountClass, a.BranchesId "
            + "UNION ALL "
            + "SELECT vd.AccountId, CASE WHEN :branchesIds IS NULL THEN 0 ELSE vd.BranchesId END AS BranchesId, 0 AS Opening, "
            + "SUM(vd.DebitAmount) AS Debit, SUM(vd.CreditAmount) AS Credit, 0 AS Closing, ca.AccountTypeId, ca.AccountClass "
            + "FROM VoucherHead vh INNER JOIN VoucherDetail vd ON vh.Id = vd.VoucherHeadId INNER JOIN ChartofAccount ca ON vd.AccountId = ca.Id "
            + "WHERE vh.OrganizationId = :organizationId AND vh.CompanyId = :companyId "
            + "AND (:fromDate IS NULL OR CONVERT(DATE, vh.VoucherDate) >= CONVERT(DATE, :fromDate)) "
            + "AND (:toDate IS NULL OR CONVERT(DATE, vh.VoucherDate) <= CONVERT(DATE, :toDate)) "
            + "AND ca.AccountTypeId = 3 AND ca.AccountClass = 3 "
            + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM BrancheIds WHERE BranchId = vd.BranchesId)) "
            + "GROUP BY vd.AccountId, ca.AccountTypeId, ca.AccountClass, CASE WHEN :branchesIds IS NULL THEN 0 ELSE vd.BranchesId END "
            + ") f INNER JOIN ChartofAccount C ON f.AccountId = C.Id "
            + "GROUP BY f.AccountId, f.AccountTypeId, f.AccountClass, C.ParentCodeId, f.BranchesId "
            + "), MyCredits AS ( "
            + "SELECT a.row_num, a.AccountId, a.VoucherDate, a.CreditAmount FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY Vd.AccountId ORDER BY Maxdt.VoucherDate DESC, Vh.Id DESC) AS row_num, "
            + "Vd.AccountId AS AccountId, Maxdt.VoucherDate, ISNULL(SUM(Vd.CreditAmount), 0) AS CreditAmount "
            + "FROM VoucherDetail AS Vd INNER JOIN VoucherHead AS Vh ON Vd.VoucherHeadId = Vh.Id "
            + "INNER JOIN V_AcReceiablesMaxDateAndTotalCredit AS Maxdt ON Vd.AccountId = Maxdt.AccountId AND Vh.VoucherDate = Maxdt.VoucherDate "
            + "INNER JOIN Final0 T ON Maxdt.AccountId = T.AccountId "
            + "WHERE Vh.OrganizationId = :organizationId AND Vh.CompanyId = :companyId "
            + "AND Vh.DocumentTypeId IN (56,57,138,1603,1604,1104,702,1653,1654) AND T.AccountClass = 3 "
            + "GROUP BY Vd.AccountId, Maxdt.VoucherDate, Vh.Id "
            + ") a WHERE a.row_num = 1 "
            + "), MyDebits AS ( "
            + "SELECT a.row_num, a.AccountId, a.VoucherDate, a.DebitAmount FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY Maxdt.AccountId ORDER BY Maxdt.VoucherDate DESC, Vh.Id DESC) AS row_num, "
            + "Maxdt.AccountId AS AccountId, Maxdt.VoucherDate, ISNULL(SUM(Vd.DebitAmount), 0) AS DebitAmount "
            + "FROM VoucherDetail AS Vd INNER JOIN VoucherHead AS Vh ON Vd.VoucherHeadId = Vh.Id "
            + "INNER JOIN V_AcReceiablesMaxDateAndTotalDebit AS Maxdt ON Vd.AccountId = Maxdt.AccountId AND Vh.VoucherDate = Maxdt.VoucherDate "
            + "INNER JOIN Final0 T ON Maxdt.AccountId = T.AccountId "
            + "WHERE Vh.DocumentTypeId IN (1,2,5,35) AND T.AccountClass = 3 "
            + "GROUP BY Maxdt.AccountId, Maxdt.VoucherDate, Vh.Id "
            + ") a WHERE a.row_num = 1 "
            + "), TradeParties AS ( "
            + "SELECT sp.GlAccountId, SUM(d.InvoiceQty) AS ItemQty, SUM(d.InvoiceWeight) AS BillWeight, SUM(d.PurchaseAmount) AS PurchaseAmount "
            + "FROM Final0 r INNER JOIN SupplierCustomer sp ON r.AccountId = sp.GlAccountId "
            + "INNER JOIN InvPurchaseInvoice h ON h.SupplierCustomerId = sp.Id "
            + "INNER JOIN (SELECT InvPurchaseInvoiceId, SUM(ItemQty) AS InvoiceQty, SUM(NetBillWeight) AS InvoiceWeight, SUM(BillAmount) AS PurchaseAmount "
            + "FROM InvPurchaseInvoiceDetail GROUP BY InvPurchaseInvoiceId) d ON h.Id = d.InvPurchaseInvoiceId "
            + "WHERE h.DocumentTypeId IN (56,57,1603,1604,1653,1654) "
            + "AND (:purchaseFromDate IS NULL OR CAST(h.DocDate AS DATE) >= CAST(:purchaseFromDate AS DATE)) "
            + "AND (:purchaseToDate IS NULL OR CAST(h.DocDate AS DATE) <= CAST(:purchaseToDate AS DATE)) "
            + "GROUP BY sp.GlAccountId "
            + "), OnlyPurchaseParties AS ( "
            + "SELECT DISTINCT sp.GlAccountId, h.SupplierCustomerId "
            + "FROM Final0 r INNER JOIN SupplierCustomer sp ON r.AccountId = sp.GlAccountId "
            + "INNER JOIN InvPurchaseInvoice h ON h.SupplierCustomerId = sp.Id "
            + "WHERE h.DocumentTypeId IN (56,57,1604,1603,1653,1654) "
            + "GROUP BY sp.GlAccountId, h.SupplierCustomerId "
            + "), TmpPurchaseOrder AS ( "
            + "SELECT T.GlAccountId, T.SupplierCustomerId, H.Id AS OrderId, D.Id AS OrderDetailId, D.OrderItemQty AS OrderQty, D.NetWeight AS OrderWeight "
            + "FROM PurchaseOrderDetail D INNER JOIN PurchaseOrder H ON D.PurchaseOrderId = H.Id "
            + "INNER JOIN OnlyPurchaseParties T ON H.OrderSupCustId = T.SupplierCustomerId "
            + "WHERE H.OrganizationId = :organizationId AND H.CompanyId = :companyId "
            + "), TotalPurchaseOrder AS ( "
            + "SELECT R.GlAccountId, SUM(R.OrderQty) AS OrderQty, SUM(R.OrderWeight) AS OrderWeight FROM TmpPurchaseOrder R GROUP BY R.GlAccountId "
            + "), ReceivedData AS ( "
            + "SELECT H.SupplierCustomerId, R.GlAccountId, R.OrderId, R.OrderDetailId, CAST(SUM(D.ItemQty) AS FLOAT) AS ReceivedQty, CAST(SUM(D.NetBillWeight) AS FLOAT) AS ReceivedWeight "
            + "FROM InvGrnDetail D INNER JOIN InvGrn H ON D.InvGrnId = H.Id "
            + "INNER JOIN TmpPurchaseOrder R ON D.PurchaseOrderId = R.OrderId AND D.PurchaseOrderDetailId = R.OrderDetailId "
            + "GROUP BY H.SupplierCustomerId, R.GlAccountId, R.OrderId, R.OrderDetailId "
            + "UNION ALL "
            + "SELECT H.SupplierCustomerId, R.GlAccountId, R.OrderId, R.OrderDetailId, CAST(SUM(D.ItemQty) AS FLOAT), CAST(SUM(D.NetBillWeight) AS FLOAT) "
            + "FROM InvPurchaseInvoiceDetail D INNER JOIN InvPurchaseInvoice H ON D.InvPurchaseInvoiceId = H.Id "
            + "INNER JOIN TmpPurchaseOrder R ON D.PurchaseOrderId = R.OrderId AND D.PurchaseOrderDetailId = R.OrderDetailId "
            + "WHERE ISNULL(D.InvGrnId, 0) = 0 "
            + "GROUP BY H.SupplierCustomerId, R.GlAccountId, R.OrderId, R.OrderDetailId "
            + "), TotalReceivedData AS ( "
            + "SELECT R.GlAccountId, SUM(R.ReceivedQty) AS ReceivedQty, SUM(R.ReceivedWeight) AS ReceivedWeight FROM ReceivedData R GROUP BY R.GlAccountId "
            + "), PartyGroup AS ( "
            + "SELECT x.GlAccountId, x.CustomerGroupId FROM ( "
            + "SELECT SP.GlAccountId, SP.CustomerGroupId, ROW_NUMBER() OVER (PARTITION BY SP.GlAccountId ORDER BY SP.Id) AS rn FROM SupplierCustomer SP "
            + ") x WHERE x.rn = 1 "
            + "), Final AS ( "
            + "SELECT f.AccountId, f.Opening, f.CurrDebit, f.CurrCredit, f.Closing, f.AccountTypeId, f.AccountClass, f.ParentCodeId, f.BranchesId, "
            + "CASE WHEN mc.AccountId IS NULL THEN CONVERT(NVARCHAR(50), '') ELSE CONVERT(NVARCHAR(50), mc.VoucherDate) END AS LastBillDate, "
            + "CASE WHEN mc.AccountId IS NULL THEN 0 ELSE CAST(mc.CreditAmount AS INT) END AS LastBillsAmount, "
            + "CASE WHEN md.AccountId IS NULL THEN CONVERT(NVARCHAR(50), '') ELSE CONVERT(NVARCHAR(50), md.VoucherDate) END AS LastPaymentDate, "
            + "CASE WHEN md.AccountId IS NULL THEN 0 ELSE CAST(md.DebitAmount AS INT) END AS LastPaymentAmount, "
            + "CASE WHEN tp.GlAccountId IS NULL THEN 0 ELSE CAST(tp.ItemQty AS INT) END AS ItemQty, "
            + "CASE WHEN tp.GlAccountId IS NULL THEN 0 ELSE CAST(tp.BillWeight AS INT) END AS BillWeight, "
            + "CASE WHEN tp.GlAccountId IS NULL THEN 0 ELSE CAST(tp.PurchaseAmount AS INT) END AS PurchaseAmount, "
            + "CASE WHEN po.GlAccountId IS NULL THEN 0 ELSE CAST(po.OrderQty AS INT) END AS OrderQty, "
            + "CASE WHEN po.GlAccountId IS NULL THEN 0 ELSE CAST(po.OrderWeight AS INT) END AS OrderWeight, "
            + "CASE WHEN rd.GlAccountId IS NULL THEN 0 ELSE CAST(rd.ReceivedQty AS INT) END AS ReceivedQty, "
            + "CASE WHEN rd.GlAccountId IS NULL THEN 0 ELSE CAST(rd.ReceivedWeight AS INT) END AS ReceivedWeight, "
            + "CASE WHEN pg.GlAccountId IS NULL THEN 0 ELSE pg.CustomerGroupId END AS PartyGroupId "
            + "FROM Final0 f "
            + "LEFT JOIN MyCredits mc ON f.AccountId = mc.AccountId "
            + "LEFT JOIN MyDebits md ON f.AccountId = md.AccountId "
            + "LEFT JOIN TradeParties tp ON f.AccountId = tp.GlAccountId "
            + "LEFT JOIN TotalPurchaseOrder po ON f.AccountId = po.GlAccountId "
            + "LEFT JOIN TotalReceivedData rd ON f.AccountId = rd.GlAccountId "
            + "LEFT JOIN PartyGroup pg ON f.AccountId = pg.GlAccountId "
            + "), TmpResult AS ( "
            + "SELECT a.AccountId, a.AccountTypeId, TP.Abbreviation AS AccountType, AC.ClassName, Pac.AccountTitle AS ParentAccount, "
            + "c.AccountCode, c.AccountTitle, a.Opening, a.CurrDebit, a.CurrCredit, a.Closing, a.Closing - a.Opening AS ShortExcess, "
            + "a.LastBillDate, a.LastBillsAmount, "
            + "CASE WHEN ISNULL(a.LastBillDate, '') <> '' THEN DATEDIFF(DAY, CONVERT(DATE, a.LastBillDate), CONVERT(DATE, GETDATE())) ELSE 0 END AS BillDays, "
            + "a.LastPaymentDate, a.LastPaymentAmount, "
            + "CASE WHEN ISNULL(a.LastPaymentDate, '') <> '' THEN DATEDIFF(DAY, CONVERT(DATE, a.LastPaymentDate), CONVERT(DATE, GETDATE())) ELSE 0 END AS PaymentDays, "
            + "a.ItemQty, a.BillWeight, a.PurchaseAmount, a.OrderQty, a.ReceivedQty, a.OrderQty - a.ReceivedQty AS BalQty, "
            + "a.OrderWeight, a.ReceivedWeight, a.OrderWeight - a.ReceivedWeight AS BalWeight, a.BranchesId, br.BranchName "
            + "FROM Final a "
            + "INNER JOIN ChartofAccount c ON a.AccountId = c.Id "
            + "INNER JOIN ChartofAccount Pac ON a.ParentCodeId = Pac.Id "
            + "INNER JOIN AccountTypes TP ON a.AccountTypeId = TP.Id "
            + "INNER JOIN vAccountClasses AC ON a.AccountClass = AC.ClassId "
            + "LEFT JOIN AccountsCustomGroups cg ON a.AccountId = cg.ChartOfAccountId "
            + "LEFT JOIN Branches br ON a.BranchesId = br.Id "
            + "WHERE (:customGroupId IS NULL OR cg.AcLookUpsId = :customGroupId) "
            + "AND (:balanceFrom IS NULL OR ABS(a.Closing) >= :balanceFrom) "
            + "AND (:balanceTo IS NULL OR ABS(a.Closing) <= :balanceTo) "
            + "AND (:partyGroupId IS NULL OR a.PartyGroupId = :partyGroupId) "
            + "AND (a.Closing < 0) "
            + "AND (:controlAccountIds IS NULL OR EXISTS (SELECT 1 FROM fnSplitString(:controlAccountIds, ',') AS PC WHERE PC.Data = a.ParentCodeId)) "
            + "), TempIds AS ( "
            + "SELECT DISTINCT AccountId AS GlAccountId FROM TmpResult "
            + "), AllData AS ( "
            + "SELECT o.ChartOfAccountId AS AccountId, CAST(SUM(YearObDebit) AS DECIMAL(18,4)) AS DebitAmount, CAST(SUM(YearObCredit) AS DECIMAL(18,4)) AS CreditAmount "
            + "FROM AccountsOpeningBalances o INNER JOIN ChartofAccount c ON o.ChartOfAccountId = c.Id INNER JOIN TempIds ti ON c.Id = ti.GlAccountId "
            + "WHERE o.OrganizationId = :organizationId AND o.CompanyId = :companyId "
            + "GROUP BY o.ChartOfAccountId "
            + "UNION ALL "
            + "SELECT d.AccountId, CAST(0 AS DECIMAL(18,4)), CAST(SUM(d.CreditAmount) AS DECIMAL(18,4)) "
            + "FROM VoucherDetail d INNER JOIN VoucherHead h ON d.VoucherHeadId = h.Id "
            + "INNER JOIN TempIds ti ON d.AccountId = ti.GlAccountId AND h.DocumentTypeId IN (56,57,1603,1604,1653,1654) "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND (:dueDateFrom IS NULL OR CAST(h.DueDate AS DATE) >= CAST(:dueDateFrom AS DATE)) "
            + "AND (:dueDateTo IS NULL OR CAST(h.DueDate AS DATE) <= CAST(:dueDateTo AS DATE)) "
            + "GROUP BY d.AccountId "
            + "UNION ALL "
            + "SELECT d.AccountId, CAST(0 AS DECIMAL(18,4)), CAST(SUM(d.CreditAmount) AS DECIMAL(18,4)) "
            + "FROM VoucherDetail d INNER JOIN VoucherHead h ON d.VoucherHeadId = h.Id "
            + "INNER JOIN TempIds ti ON d.AccountId = ti.GlAccountId "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND h.DocumentTypeId NOT IN (56,57,1603,1604,1653,1654) "
            + "AND (:dueDateFrom IS NULL OR CAST(h.VoucherDate AS DATE) >= CAST(:dueDateFrom AS DATE)) "
            + "AND (:dueDateTo IS NULL OR CAST(h.VoucherDate AS DATE) <= CAST(:dueDateTo AS DATE)) "
            + "GROUP BY d.AccountId "
            + "UNION ALL "
            + "SELECT d.AccountId, CAST(SUM(d.DebitAmount) AS DECIMAL(18,4)), CAST(0 AS DECIMAL(18,4)) "
            + "FROM VoucherDetail d INNER JOIN VoucherHead h ON d.VoucherHeadId = h.Id "
            + "INNER JOIN TempIds ti ON d.AccountId = ti.GlAccountId "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND (:dueDateFrom IS NULL OR CAST(h.VoucherDate AS DATE) >= CAST(:dueDateFrom AS DATE)) "
            + "AND (:dueDateTo IS NULL OR CAST(h.VoucherDate AS DATE) <= CAST(:dueDateTo AS DATE)) "
            + "GROUP BY d.AccountId "
            + "), DueBalance AS ( "
            + "SELECT R.AccountId, SUM(R.DebitAmount) - SUM(R.CreditAmount) AS DueAmount FROM AllData R GROUP BY R.AccountId "
            + "), NotYetDueT AS ( "
            + "SELECT d.AccountId, SUM(d.CreditAmount) AS NotYetDue "
            + "FROM VoucherDetail d INNER JOIN VoucherHead h ON d.VoucherHeadId = h.Id "
            + "INNER JOIN TempIds ti ON d.AccountId = ti.GlAccountId AND h.DocumentTypeId IN (56,57,1603,1604,1653,1654) "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND (CAST(h.DueDate AS DATE) > CAST(:dueDateTo AS DATE)) "
            + "GROUP BY d.AccountId "
            + "), Res2 AS ( "
            + "SELECT R.AccountId, R.AccountTypeId, R.AccountType, R.ClassName, R.ParentAccount, R.AccountCode, R.AccountTitle, "
            + "R.Opening, R.CurrDebit, R.CurrCredit, R.Closing, R.ShortExcess, "
            + "CASE WHEN db.AccountId IS NULL THEN CAST(0 AS DECIMAL(18,2)) ELSE CAST(ABS(db.DueAmount) AS DECIMAL(18,2)) END AS DueBalance1, "
            + "CASE WHEN ny.AccountId IS NULL THEN CAST(0 AS DECIMAL(18,2)) ELSE CAST(ny.NotYetDue AS DECIMAL(18,2)) END AS NotYetDue, "
            + "R.LastBillDate, R.LastBillsAmount, R.BillDays, R.LastPaymentDate, R.LastPaymentAmount, R.PaymentDays, "
            + "R.ItemQty, R.BillWeight, R.PurchaseAmount, R.OrderQty, R.ReceivedQty, R.BalQty, R.OrderWeight, R.ReceivedWeight, R.BalWeight, R.BranchesId, R.BranchName "
            + "FROM TmpResult R "
            + "LEFT JOIN DueBalance db ON R.AccountId = db.AccountId "
            + "LEFT JOIN NotYetDueT ny ON R.AccountId = ny.AccountId "
            + "), Res3 AS ( "
            + "SELECT R.AccountId, R.AccountTypeId, R.AccountType, R.ClassName, R.ParentAccount, R.AccountCode, R.AccountTitle, "
            + "R.Opening, R.CurrDebit, R.CurrCredit, R.Closing, R.ShortExcess, "
            + "CASE WHEN R.DueBalance1 > 0 AND CAST(ABS(R.Closing) AS DECIMAL(18,0)) >= CAST(R.DueBalance1 AS DECIMAL(18,0)) THEN R.DueBalance1 ELSE CAST(0 AS DECIMAL(18,2)) END AS DueBalance, "
            + "R.NotYetDue, R.LastBillDate, R.LastBillsAmount, R.BillDays, R.LastPaymentDate, R.LastPaymentAmount, R.PaymentDays, "
            + "R.ItemQty, R.BillWeight, R.PurchaseAmount, R.OrderQty, R.ReceivedQty, R.BalQty, R.OrderWeight, R.ReceivedWeight, R.BalWeight, R.BranchesId, R.BranchName "
            + "FROM Res2 R "
            + ") "
            + "SELECT R.AccountId, R.AccountTypeId, R.AccountType, R.ClassName, R.ParentAccount, R.AccountCode, R.AccountTitle, "
            + "R.Opening, R.CurrDebit, -R.CurrCredit AS CurrCredit, R.Closing, R.ShortExcess AS [Short/Excess], R.DueBalance, "
            + "CASE WHEN R.NotYetDue > 0 AND (ABS(R.Closing) - R.DueBalance) > R.NotYetDue THEN R.NotYetDue ELSE ABS(R.Closing) - R.DueBalance END AS NotYetDue, "
            + "CAST(0 AS FLOAT) AS PayToday, "
            + "R.LastBillDate, R.LastBillsAmount, R.BillDays, R.LastPaymentDate, R.LastPaymentAmount, R.PaymentDays, "
            + "R.ItemQty AS PurchaseQty, R.BillWeight AS PurchaseWeight, R.PurchaseAmount, R.OrderQty, R.ReceivedQty, "
            + "CASE WHEN R.BalQty < 0 THEN 0 ELSE R.BalQty END AS BalQty, "
            + "R.OrderWeight, R.ReceivedWeight, R.BalWeight, R.BranchesId, R.BranchName, "
            + "CAST(:fromDate AS DATE) AS FromDate, CAST(:toDate AS DATE) AS ToDate, "
            + "CAST(:dueDateFrom AS DATE) AS DueDateFrom, CAST(:dueDateTo AS DATE) AS DueDateTo, "
            + "CAST(:purchaseFromDate AS DATE) AS PurchaseFromDate, CAST(:purchaseToDate AS DATE) AS PurchaseToDate "
            + "FROM Res3 R CROSS JOIN AccStatus st "
            + "WHERE ( "
            + "(st.AccountStatus = 1 AND NOT EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = R.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 2 AND EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = R.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 0) ) "
            + "ORDER BY ABS(R.DueBalance) DESC",
            nativeQuery = true)
    List<PayablesPaymentScheduleRow> payablesAndPaymentSchedule(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("dueDateFrom") LocalDate dueDateFrom,
            @Param("dueDateTo") LocalDate dueDateTo,
            @Param("purchaseFromDate") LocalDate purchaseFromDate,
            @Param("purchaseToDate") LocalDate purchaseToDate,
            @Param("balanceFrom") Double balanceFrom,
            @Param("balanceTo") Double balanceTo,
            @Param("customGroupId") Integer customGroupId,
            @Param("partyGroupId") Integer partyGroupId,
            @Param("userId") Integer userId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("branchesIds") String branchesIds);

    /**
     * usp_PayablesAndPaymentSchedule -- SECOND result set (three caption rows: 'Net Payables', 'OverDue
     * Payables', 'NotYetDue Payables' with the interval captions built from @AgingDays). The procedure's
     * `if @AgingDays = NULL` never fires, so a NULL @AgingDays yields NULL captions here as well.
     */
    @Query(value = "SELECT 'Net Payables' AS [Description], CAST(0.00 AS DECIMAL(3,2)) AS Amount, "
            + "'0_' + CONVERT(NVARCHAR(20), :agingDays) + '' AS IstIntervale, "
            + "CAST(:agingDays + 1 AS NVARCHAR(10)) + '_' + CAST(:agingDays * 2 AS NVARCHAR(10)) AS ScnInterval, "
            + "CAST(:agingDays * 2 + 1 AS NVARCHAR(10)) + '_' + CAST(:agingDays * 3 AS NVARCHAR(10)) AS TrdIntarval, "
            + "CAST(:agingDays * 3 AS NVARCHAR(10)) + '_Above' AS Above, 0 AS Value_1, 0 AS Value_2, 0 AS Value_3, 0 AS Value_4 "
            + "UNION ALL "
            + "SELECT 'OverDue Payables' AS [Description], CAST(0.00 AS DECIMAL(3,2)) AS Amount, "
            + "'0_' + CONVERT(NVARCHAR(20), :agingDays) + '' AS IstIntervale, "
            + "CAST(:agingDays + 1 AS NVARCHAR(10)) + '_' + CAST(:agingDays * 2 AS NVARCHAR(10)) AS ScnInterval, "
            + "CAST(:agingDays * 2 + 1 AS NVARCHAR(10)) + '_' + CAST(:agingDays * 3 AS NVARCHAR(10)) AS TrdIntarval, "
            + "CAST(:agingDays * 3 AS NVARCHAR(10)) + '_Above' AS Above, 0 AS Value_1, 0 AS Value_2, 0 AS Value_3, 0 AS Value_4 "
            + "UNION ALL "
            + "SELECT 'NotYetDue Payables' AS [Description], CAST(0.00 AS DECIMAL(3,2)) AS Amount, "
            + "'0_' + CONVERT(NVARCHAR(20), :agingDays) + '' AS IstIntervale, "
            + "CAST(:agingDays + 1 AS NVARCHAR(10)) + '_' + CAST(:agingDays * 2 AS NVARCHAR(10)) AS ScnInterval, "
            + "CAST(:agingDays * 2 + 1 AS NVARCHAR(10)) + '_' + CAST(:agingDays * 3 AS NVARCHAR(10)) AS TrdIntarval, "
            + "CAST(:agingDays * 3 AS NVARCHAR(10)) + '_Above' AS Above, 0 AS Value_1, 0 AS Value_2, 0 AS Value_3, 0 AS Value_4",
            nativeQuery = true)
    List<PayablesAgingCaptionRow> payablesAgingCaptions(@Param("agingDays") Integer agingDays);

    /**
     * usp_PayablesAndPaymentSchedule -- KEEP-EXEC fallback: the original procedure call with all 19
     * parameters (@ActionId and @TradeTypeId are overwritten to 1 inside; @ParentCategoryId unused;
     * @AgingDays only feeds the second result set, which JPA does not return). Same projection.
     */
    @Query(value = "EXEC dbo.usp_PayablesAndPaymentSchedule "
            + "@OrganizationId = :organizationId, @CompanyId = :companyId, @FromDate = :fromDate, @ToDate = :toDate, "
            + "@DueDateFrom = :dueDateFrom, @DueDateTo = :dueDateTo, @PurchaseFromDate = :purchaseFromDate, "
            + "@PurchaseToDate = :purchaseToDate, @ParentCategoryId = :parentCategoryId, @BalanceFrom = :balanceFrom, "
            + "@BalanceTo = :balanceTo, @CustomGroupId = :customGroupId, @PartyGroupId = :partyGroupId, @ActionId = :actionId, "
            + "@UserId = :userId, @TradeTypeId = :tradeTypeId, @ControlAccountIds = :controlAccountIds, "
            + "@BranchesIds = :branchesIds, @AgingDays = :agingDays",
            nativeQuery = true)
    List<PayablesPaymentScheduleRow> payablesAndPaymentScheduleExec(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("dueDateFrom") LocalDate dueDateFrom,
            @Param("dueDateTo") LocalDate dueDateTo,
            @Param("purchaseFromDate") LocalDate purchaseFromDate,
            @Param("purchaseToDate") LocalDate purchaseToDate,
            @Param("parentCategoryId") Integer parentCategoryId,
            @Param("balanceFrom") Double balanceFrom,
            @Param("balanceTo") Double balanceTo,
            @Param("customGroupId") Integer customGroupId,
            @Param("partyGroupId") Integer partyGroupId,
            @Param("actionId") Integer actionId,
            @Param("userId") Integer userId,
            @Param("tradeTypeId") Integer tradeTypeId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("branchesIds") String branchesIds,
            @Param("agingDays") Integer agingDays);

    // ------------------------------------------------------------------ usp_ReceivablesAndReceiptsSchedule

    /**
     * usp_ReceivablesAndReceiptsSchedule (single branch): customer accounts (AccountTypeId 3, AccountClass 2,
     * debit closing balances, Closing CAST to DECIMAL(18,0)) with Opening / CurrDebit / -CurrCredit /
     * Closing, DueBalance (opening + sale invoices by DueDate + other vouchers - receipts in the due window,
     * capped by Closing), NotYetDue (invoices due after @DueDateTo, capped by Closing - DueBalance), last bill
     * / last receipt date and amount, sale qty / weight / amount (InvSaleInvoiceDetail), order vs dispatched
     * quantities (SaleOrderDetail vs InvGdnDetail + un-GDN'd InvSaleInvoiceDetail), ordered by DueBalance DESC.
     * Procedure defaults reproduced in the P CTE: @FromDate NULL -> Start_Period of the company's logged-in
     * FinancialYear (TOP 1 ORDER BY End_Period DESC); @DueDateFrom NULL -> the resolved @FromDate.
     * Native CTE rewrite with the same structure as the payables schedule (Final0, MyCredits, MyDebits,
     * TradeParties, OnlySaleParties, TmpSaleOrder, TotalSaleOrder, DispatchData, TotalDispatchData,
     * PartyGroup, Final, TmpResult, TempIds, AllData, DueBalance, NotYetDueT, Res2, Res3).
     * The PartyGroupId lookup picks the lowest SupplierCustomer.Id where the procedure picked an arbitrary one.
     */
    @Query(value = "WITH P AS ( "
            + "SELECT CAST(COALESCE(:fromDate, (SELECT TOP 1 Start_Period FROM FinancialYear WHERE CompanyId = :companyId AND LoginStatus = 1 ORDER BY End_Period DESC)) AS DATE) AS FromDate, "
            + "CAST(COALESCE(:dueDateFrom, :fromDate, (SELECT TOP 1 Start_Period FROM FinancialYear WHERE CompanyId = :companyId AND LoginStatus = 1 ORDER BY End_Period DESC)) AS DATE) AS DueDateFrom "
            + "), AccStatus AS ( "
            + "SELECT CASE WHEN EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) "
            + "THEN (SELECT TOP 1 AccountStatus FROM TmpShowHideStatusWithAccounts WHERE UserId = :userId) ELSE 0 END AS AccountStatus "
            + "), BrancheIds AS ( "
            + "SELECT data AS BranchId FROM dbo.fnSplitString(:branchesIds, ',') "
            + "), Final0 AS ( "
            + "SELECT f.AccountId, SUM(f.Opening) AS Opening, SUM(f.CurrDebit) AS CurrDebit, SUM(f.CurrCredit) AS CurrCredit, "
            + "CAST(SUM(f.Opening) + SUM(f.CurrDebit) - SUM(f.CurrCredit) AS DECIMAL(18,0)) AS Closing, "
            + "f.AccountTypeId, f.AccountClass, C.ParentCodeId, f.BranchesId "
            + "FROM ( "
            + "SELECT a.AccountId, a.BranchesId, SUM(a.Debit) - SUM(a.Credit) AS Opening, 0 AS CurrDebit, 0 AS CurrCredit, 0 AS Closing, a.AccountTypeId, a.AccountClass "
            + "FROM ( "
            + "SELECT o.ChartOfAccountId AS AccountId, 0 AS BranchesId, ISNULL(SUM(o.YearObDebit), 0) AS Debit, ISNULL(SUM(o.YearObCredit), 0) AS Credit, c.AccountTypeId, c.AccountClass "
            + "FROM AccountsOpeningBalances o INNER JOIN ChartofAccount c ON o.ChartOfAccountId = c.Id "
            + "WHERE o.OrganizationId = :organizationId AND o.CompanyId = :companyId AND c.AccountGroup = 'DETAIL' AND c.AccountTypeId = 3 AND c.AccountClass = 2 "
            + "GROUP BY o.ChartOfAccountId, c.AccountTypeId, c.AccountClass "
            + "UNION ALL "
            + "SELECT d.AccountId, CASE WHEN :branchesIds IS NULL THEN 0 ELSE d.BranchesId END AS BranchesId, "
            + "ISNULL(SUM(d.DebitAmount), 0), ISNULL(SUM(d.CreditAmount), 0), c.AccountTypeId, c.AccountClass "
            + "FROM VoucherHead h INNER JOIN VoucherDetail d ON h.Id = d.VoucherHeadId INNER JOIN ChartofAccount c ON d.AccountId = c.Id CROSS JOIN P "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId AND c.AccountGroup = 'DETAIL' AND c.AccountTypeId = 3 AND c.AccountClass = 2 "
            + "AND (P.FromDate IS NULL OR CONVERT(DATE, h.VoucherDate) < CONVERT(DATE, P.FromDate)) "
            + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM BrancheIds WHERE BranchId = d.BranchesId)) "
            + "GROUP BY d.AccountId, c.AccountTypeId, c.AccountClass, CASE WHEN :branchesIds IS NULL THEN 0 ELSE d.BranchesId END "
            + ") a GROUP BY a.AccountId, a.AccountTypeId, a.AccountClass, a.BranchesId "
            + "UNION ALL "
            + "SELECT vd.AccountId, CASE WHEN :branchesIds IS NULL THEN 0 ELSE vd.BranchesId END AS BranchesId, 0 AS Opening, "
            + "SUM(vd.DebitAmount) AS Debit, SUM(vd.CreditAmount) AS Credit, 0 AS Closing, ca.AccountTypeId, ca.AccountClass "
            + "FROM VoucherHead vh INNER JOIN VoucherDetail vd ON vh.Id = vd.VoucherHeadId INNER JOIN ChartofAccount ca ON vd.AccountId = ca.Id CROSS JOIN P "
            + "WHERE vh.OrganizationId = :organizationId AND vh.CompanyId = :companyId "
            + "AND (P.FromDate IS NULL OR CONVERT(DATE, vh.VoucherDate) >= CONVERT(DATE, P.FromDate)) "
            + "AND (:toDate IS NULL OR CONVERT(DATE, vh.VoucherDate) <= CONVERT(DATE, :toDate)) "
            + "AND ca.AccountTypeId = 3 AND ca.AccountClass = 2 "
            + "AND (:branchesIds IS NULL OR EXISTS (SELECT 1 FROM BrancheIds WHERE BranchId = vd.BranchesId)) "
            + "GROUP BY vd.AccountId, ca.AccountTypeId, ca.AccountClass, CASE WHEN :branchesIds IS NULL THEN 0 ELSE vd.BranchesId END "
            + ") f INNER JOIN ChartofAccount C ON f.AccountId = C.Id "
            + "GROUP BY f.AccountId, f.AccountTypeId, f.AccountClass, C.ParentCodeId, f.BranchesId "
            + "), MyCredits AS ( "
            + "SELECT a.row_num, a.AccountId, a.VoucherDate, a.CreditAmount FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY Vd.AccountId ORDER BY Maxdt.VoucherDate DESC, Vh.Id DESC) AS row_num, "
            + "Vd.AccountId AS AccountId, Maxdt.VoucherDate, ISNULL(SUM(Vd.CreditAmount), 0) AS CreditAmount "
            + "FROM VoucherDetail AS Vd INNER JOIN VoucherHead AS Vh ON Vd.VoucherHeadId = Vh.Id "
            + "INNER JOIN V_AcReceiablesMaxDateAndTotalCredit AS Maxdt ON Vd.AccountId = Maxdt.AccountId AND Vh.VoucherDate = Maxdt.VoucherDate "
            + "INNER JOIN Final0 T ON Maxdt.AccountId = T.AccountId "
            + "WHERE Vh.OrganizationId = :organizationId AND Vh.CompanyId = :companyId "
            + "AND Vh.DocumentTypeId IN (3,4,34) AND T.AccountTypeId = 3 AND T.AccountClass = 2 "
            + "GROUP BY Vd.AccountId, Maxdt.VoucherDate, Vh.Id "
            + ") a WHERE a.row_num = 1 "
            + "), MyDebits AS ( "
            + "SELECT a.row_num, a.AccountId, a.VoucherDate, a.DebitAmount FROM ( "
            + "SELECT ROW_NUMBER() OVER (PARTITION BY Maxdt.AccountId ORDER BY Maxdt.VoucherDate DESC, Vh.Id DESC) AS row_num, "
            + "Maxdt.AccountId AS AccountId, Maxdt.VoucherDate, ISNULL(SUM(Vd.DebitAmount), 0) AS DebitAmount "
            + "FROM VoucherDetail AS Vd INNER JOIN VoucherHead AS Vh ON Vd.VoucherHeadId = Vh.Id "
            + "INNER JOIN V_AcReceiablesMaxDateAndTotalDebit AS Maxdt ON Vd.AccountId = Maxdt.AccountId AND Vh.VoucherDate = Maxdt.VoucherDate "
            + "INNER JOIN Final0 T ON Maxdt.AccountId = T.AccountId "
            + "WHERE Vh.DocumentTypeId IN (95,99,1660,1661) AND T.AccountTypeId = 3 AND T.AccountClass = 2 "
            + "GROUP BY Maxdt.AccountId, Maxdt.VoucherDate, Vh.Id "
            + ") a WHERE a.row_num = 1 "
            + "), TradeParties AS ( "
            + "SELECT sp.GlAccountId, SUM(d.InvoiceQty) AS ItemQty, SUM(d.InvoiceWeight) AS BillWeight, SUM(d.SaleAmount) AS SaleAmount "
            + "FROM Final0 r INNER JOIN SupplierCustomer sp ON r.AccountId = sp.GlAccountId "
            + "INNER JOIN InvSaleInvoice h ON h.SupplierCustomerId = sp.Id "
            + "INNER JOIN (SELECT InvSaleInvoiceId, SUM(ItemQty) AS InvoiceQty, SUM(NetBillWeight) AS InvoiceWeight, SUM(BillAmount) AS SaleAmount "
            + "FROM InvSaleInvoiceDetail GROUP BY InvSaleInvoiceId) d ON h.Id = d.InvSaleInvoiceId "
            + "WHERE h.DocumentTypeId IN (95,99,1660,1661) "
            + "AND (:saleFromDate IS NULL OR CAST(h.DocDate AS DATE) >= CAST(:saleFromDate AS DATE)) "
            + "AND (:saleToDate IS NULL OR CAST(h.DocDate AS DATE) <= CAST(:saleToDate AS DATE)) "
            + "GROUP BY sp.GlAccountId "
            + "), OnlySaleParties AS ( "
            + "SELECT DISTINCT sp.GlAccountId, h.SupplierCustomerId "
            + "FROM Final0 r INNER JOIN SupplierCustomer sp ON r.AccountId = sp.GlAccountId "
            + "INNER JOIN InvSaleInvoice h ON h.SupplierCustomerId = sp.Id "
            + "WHERE h.DocumentTypeId IN (95,99,1660,1661) "
            + "GROUP BY sp.GlAccountId, h.SupplierCustomerId "
            + "), TmpSaleOrder AS ( "
            + "SELECT T.GlAccountId, T.SupplierCustomerId, H.Id AS OrderId, D.Id AS OrderDetailId, D.OrderItemQty AS OrderQty, D.NetWeight AS OrderWeight "
            + "FROM SaleOrderDetail D INNER JOIN SaleOrder H ON D.SaleOrderId = H.Id AND D.ActionTypeId <> 3 AND ISNULL(D.isCanceled, 0) = 0 "
            + "INNER JOIN OnlySaleParties T ON H.OrderSupCustId = T.SupplierCustomerId "
            + "WHERE H.OrganizationId = :organizationId AND H.CompanyId = :companyId "
            + "), TotalSaleOrder AS ( "
            + "SELECT R.GlAccountId, SUM(R.OrderQty) AS OrderQty, SUM(R.OrderWeight) AS OrderWeight FROM TmpSaleOrder R GROUP BY R.GlAccountId "
            + "), DispatchData AS ( "
            + "SELECT H.SupplierCustomerId, R.GlAccountId, R.OrderId, R.OrderDetailId, CAST(SUM(D.ItemQty) AS FLOAT) AS DispatchQty, CAST(SUM(D.NetBillWeight) AS FLOAT) AS DispatchWeight "
            + "FROM InvGdnDetail D INNER JOIN InvGdn H ON D.InvGdnId = H.Id "
            + "INNER JOIN TmpSaleOrder R ON D.SaleOrderId = R.OrderId AND D.SaleOrderDetailId = R.OrderDetailId "
            + "GROUP BY H.SupplierCustomerId, R.GlAccountId, R.OrderId, R.OrderDetailId "
            + "UNION ALL "
            + "SELECT H.SupplierCustomerId, R.GlAccountId, R.OrderId, R.OrderDetailId, CAST(SUM(D.ItemQty) AS FLOAT), CAST(SUM(D.NetBillWeight) AS FLOAT) "
            + "FROM InvSaleInvoiceDetail D INNER JOIN InvSaleInvoice H ON D.InvSaleInvoiceId = H.Id "
            + "INNER JOIN TmpSaleOrder R ON D.SaleOrderId = R.OrderId AND D.SaleOrderDetailId = R.OrderDetailId "
            + "WHERE ISNULL(D.InvGdnId, 0) = 0 "
            + "GROUP BY H.SupplierCustomerId, R.GlAccountId, R.OrderId, R.OrderDetailId "
            + "), TotalDispatchData AS ( "
            + "SELECT R.GlAccountId, SUM(R.DispatchQty) AS DispatchQty, SUM(R.DispatchWeight) AS DispatchWeight FROM DispatchData R GROUP BY R.GlAccountId "
            + "), PartyGroup AS ( "
            + "SELECT x.GlAccountId, x.CustomerGroupId FROM ( "
            + "SELECT SP.GlAccountId, SP.CustomerGroupId, ROW_NUMBER() OVER (PARTITION BY SP.GlAccountId ORDER BY SP.Id) AS rn FROM SupplierCustomer SP "
            + ") x WHERE x.rn = 1 "
            + "), Final AS ( "
            + "SELECT f.AccountId, f.Opening, f.CurrDebit, f.CurrCredit, f.Closing, f.AccountTypeId, f.AccountClass, f.ParentCodeId, f.BranchesId, "
            + "CASE WHEN mc.AccountId IS NULL THEN CONVERT(NVARCHAR(50), '') ELSE CONVERT(NVARCHAR(50), mc.VoucherDate) END AS LastReceiptDate, "
            + "CASE WHEN mc.AccountId IS NULL THEN 0 ELSE CAST(mc.CreditAmount AS INT) END AS LastReceiptAmount, "
            + "CASE WHEN md.AccountId IS NULL THEN CONVERT(NVARCHAR(50), '') ELSE CONVERT(NVARCHAR(50), md.VoucherDate) END AS LastBillDate, "
            + "CASE WHEN md.AccountId IS NULL THEN 0 ELSE CAST(md.DebitAmount AS INT) END AS LastBillsAmount, "
            + "CASE WHEN tp.GlAccountId IS NULL THEN 0 ELSE CAST(tp.ItemQty AS INT) END AS ItemQty, "
            + "CASE WHEN tp.GlAccountId IS NULL THEN 0 ELSE CAST(tp.BillWeight AS INT) END AS BillWeight, "
            + "CASE WHEN tp.GlAccountId IS NULL THEN 0 ELSE CAST(tp.SaleAmount AS INT) END AS SaleAmount, "
            + "CASE WHEN so.GlAccountId IS NULL THEN 0 ELSE CAST(so.OrderQty AS INT) END AS OrderQty, "
            + "CASE WHEN so.GlAccountId IS NULL THEN 0 ELSE CAST(so.OrderWeight AS INT) END AS OrderWeight, "
            + "CASE WHEN dd.GlAccountId IS NULL THEN 0 ELSE CAST(dd.DispatchQty AS INT) END AS DispatchQty, "
            + "CASE WHEN dd.GlAccountId IS NULL THEN 0 ELSE CAST(dd.DispatchWeight AS INT) END AS DispatchWeight, "
            + "CASE WHEN pg.GlAccountId IS NULL THEN 0 ELSE pg.CustomerGroupId END AS PartyGroupId "
            + "FROM Final0 f "
            + "LEFT JOIN MyCredits mc ON f.AccountId = mc.AccountId "
            + "LEFT JOIN MyDebits md ON f.AccountId = md.AccountId "
            + "LEFT JOIN TradeParties tp ON f.AccountId = tp.GlAccountId "
            + "LEFT JOIN TotalSaleOrder so ON f.AccountId = so.GlAccountId "
            + "LEFT JOIN TotalDispatchData dd ON f.AccountId = dd.GlAccountId "
            + "LEFT JOIN PartyGroup pg ON f.AccountId = pg.GlAccountId "
            + "), TmpResult AS ( "
            + "SELECT a.AccountId, a.AccountTypeId, TP.Abbreviation AS AccountType, AC.ClassName, Pac.AccountTitle AS ParentAccount, "
            + "c.AccountCode, c.AccountTitle, a.Opening, a.CurrDebit, a.CurrCredit, a.Closing, a.Closing - a.Opening AS ShortExcess, "
            + "a.LastBillDate, a.LastBillsAmount, "
            + "CASE WHEN ISNULL(a.LastBillDate, '') <> '' THEN DATEDIFF(DAY, CONVERT(DATE, a.LastBillDate), CONVERT(DATE, GETDATE())) ELSE 0 END AS BillDays, "
            + "a.LastReceiptDate, a.LastReceiptAmount, "
            + "CASE WHEN ISNULL(a.LastReceiptDate, '') <> '' THEN DATEDIFF(DAY, CONVERT(DATE, a.LastReceiptDate), CONVERT(DATE, GETDATE())) ELSE 0 END AS ReceiptsDays, "
            + "a.ItemQty, a.BillWeight, a.SaleAmount, a.OrderQty, a.DispatchQty, a.OrderQty - a.DispatchQty AS BalQty, "
            + "a.OrderWeight, a.DispatchWeight, a.OrderWeight - a.DispatchWeight AS BalWeight, a.BranchesId, BR.BranchName "
            + "FROM Final a "
            + "INNER JOIN ChartofAccount c ON a.AccountId = c.Id "
            + "INNER JOIN ChartofAccount Pac ON a.ParentCodeId = Pac.Id "
            + "INNER JOIN AccountTypes TP ON a.AccountTypeId = TP.Id "
            + "INNER JOIN vAccountClasses AC ON a.AccountClass = AC.ClassId "
            + "LEFT JOIN AccountsCustomGroups cg ON a.AccountId = cg.ChartOfAccountId "
            + "LEFT JOIN Branches BR ON a.BranchesId = BR.Id "
            + "WHERE (:customGroupId IS NULL OR cg.AcLookUpsId = :customGroupId) "
            + "AND (:balanceFrom IS NULL OR a.Closing >= :balanceFrom) "
            + "AND (:balanceTo IS NULL OR a.Closing <= :balanceTo) "
            + "AND (:partyGroupId IS NULL OR a.PartyGroupId = :partyGroupId) "
            + "AND (a.Closing > 0) "
            + "AND (:controlAccountIds IS NULL OR EXISTS (SELECT 1 FROM fnSplitString(:controlAccountIds, ',') AS PC WHERE PC.Data = a.ParentCodeId)) "
            + "), TempIds AS ( "
            + "SELECT DISTINCT AccountId AS GlAccountId FROM TmpResult "
            + "), AllData AS ( "
            + "SELECT o.ChartOfAccountId AS AccountId, CAST(SUM(YearObDebit) AS DECIMAL(18,4)) AS DebitAmount, CAST(SUM(YearObCredit) AS DECIMAL(18,4)) AS CreditAmount "
            + "FROM AccountsOpeningBalances o INNER JOIN ChartofAccount c ON o.ChartOfAccountId = c.Id INNER JOIN TempIds ti ON c.Id = ti.GlAccountId "
            + "WHERE o.OrganizationId = :organizationId AND o.CompanyId = :companyId "
            + "GROUP BY o.ChartOfAccountId "
            + "UNION ALL "
            + "SELECT d.AccountId, CAST(SUM(d.DebitAmount) AS DECIMAL(18,4)), CAST(0 AS DECIMAL(18,4)) "
            + "FROM VoucherDetail d INNER JOIN VoucherHead h ON d.VoucherHeadId = h.Id "
            + "INNER JOIN TempIds ti ON d.AccountId = ti.GlAccountId AND h.DocumentTypeId IN (95,99,1660,1661) CROSS JOIN P "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND (P.DueDateFrom IS NULL OR CAST(h.DueDate AS DATE) >= CAST(P.DueDateFrom AS DATE)) "
            + "AND (:dueDateTo IS NULL OR CAST(h.DueDate AS DATE) <= CAST(:dueDateTo AS DATE)) "
            + "GROUP BY d.AccountId "
            + "UNION ALL "
            + "SELECT d.AccountId, CAST(SUM(d.DebitAmount) AS DECIMAL(18,4)), CAST(0 AS DECIMAL(18,4)) "
            + "FROM VoucherDetail d INNER JOIN VoucherHead h ON d.VoucherHeadId = h.Id "
            + "INNER JOIN TempIds ti ON d.AccountId = ti.GlAccountId CROSS JOIN P "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND h.DocumentTypeId NOT IN (95,99,1660,1661) "
            + "AND (P.DueDateFrom IS NULL OR CAST(h.VoucherDate AS DATE) >= CAST(P.DueDateFrom AS DATE)) "
            + "AND (:dueDateTo IS NULL OR CAST(h.VoucherDate AS DATE) <= CAST(:dueDateTo AS DATE)) "
            + "GROUP BY d.AccountId "
            + "UNION ALL "
            + "SELECT d.AccountId, CAST(0 AS DECIMAL(18,4)), CAST(SUM(d.CreditAmount) AS DECIMAL(18,4)) "
            + "FROM VoucherDetail d INNER JOIN VoucherHead h ON d.VoucherHeadId = h.Id "
            + "INNER JOIN TempIds ti ON d.AccountId = ti.GlAccountId CROSS JOIN P "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND (P.DueDateFrom IS NULL OR CAST(h.VoucherDate AS DATE) >= CAST(P.DueDateFrom AS DATE)) "
            + "AND (:dueDateTo IS NULL OR CAST(h.VoucherDate AS DATE) <= CAST(:dueDateTo AS DATE)) "
            + "GROUP BY d.AccountId "
            + "), DueBalance AS ( "
            + "SELECT R.AccountId, SUM(R.DebitAmount) - SUM(R.CreditAmount) AS DueAmount FROM AllData R GROUP BY R.AccountId "
            + "), NotYetDueT AS ( "
            + "SELECT d.AccountId, SUM(d.DebitAmount) AS NotYetDue "
            + "FROM VoucherDetail d INNER JOIN VoucherHead h ON d.VoucherHeadId = h.Id "
            + "INNER JOIN TempIds ti ON d.AccountId = ti.GlAccountId AND h.DocumentTypeId IN (95,99,1660,1661) "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND (CAST(h.DueDate AS DATE) > CAST(:dueDateTo AS DATE)) "
            + "GROUP BY d.AccountId "
            + "), Res2 AS ( "
            + "SELECT R.AccountId, R.AccountTypeId, R.AccountType, R.ClassName, R.ParentAccount, R.AccountCode, R.AccountTitle, "
            + "R.Opening, R.CurrDebit, R.CurrCredit, R.Closing, R.ShortExcess, "
            + "CASE WHEN db.AccountId IS NULL THEN CAST(0 AS DECIMAL(18,2)) ELSE CAST(db.DueAmount AS DECIMAL(18,2)) END AS DueBalance1, "
            + "CASE WHEN ny.AccountId IS NULL THEN CAST(0 AS DECIMAL(18,2)) ELSE CAST(ny.NotYetDue AS DECIMAL(18,2)) END AS NotYetDue, "
            + "R.LastBillDate, R.LastBillsAmount, R.BillDays, R.LastReceiptDate, R.LastReceiptAmount, R.ReceiptsDays, "
            + "R.ItemQty, R.BillWeight, R.SaleAmount, R.OrderQty, R.DispatchQty, R.BalQty, R.OrderWeight, R.DispatchWeight, R.BalWeight, R.BranchesId, R.BranchName "
            + "FROM TmpResult R "
            + "LEFT JOIN DueBalance db ON R.AccountId = db.AccountId "
            + "LEFT JOIN NotYetDueT ny ON R.AccountId = ny.AccountId "
            + "), Res3 AS ( "
            + "SELECT R.AccountId, R.AccountTypeId, R.AccountType, R.ClassName, R.ParentAccount, R.AccountCode, R.AccountTitle, "
            + "R.Opening, R.CurrDebit, R.CurrCredit, R.Closing, R.ShortExcess, "
            + "CASE WHEN R.DueBalance1 > 0 AND R.Closing >= R.DueBalance1 THEN R.DueBalance1 ELSE CAST(0 AS DECIMAL(18,2)) END AS DueBalance, "
            + "R.NotYetDue, R.LastBillDate, R.LastBillsAmount, R.BillDays, R.LastReceiptDate, R.LastReceiptAmount, R.ReceiptsDays, "
            + "R.ItemQty, R.BillWeight, R.SaleAmount, R.OrderQty, R.DispatchQty, R.BalQty, R.OrderWeight, R.DispatchWeight, R.BalWeight, R.BranchesId, R.BranchName "
            + "FROM Res2 R "
            + ") "
            + "SELECT R.AccountId, R.AccountTypeId, R.AccountType, R.ClassName, R.ParentAccount, R.AccountCode, R.AccountTitle, "
            + "R.Opening, R.CurrDebit, -R.CurrCredit AS CurrCredit, R.Closing, R.ShortExcess AS [Short/Excess], R.DueBalance, "
            + "CASE WHEN R.NotYetDue > 0 AND (R.Closing - R.DueBalance) > R.NotYetDue THEN R.NotYetDue ELSE R.Closing - R.DueBalance END AS NotYetDue, "
            + "CAST(0 AS FLOAT) AS ReceiptsToday, "
            + "R.LastBillDate, R.LastBillsAmount, R.BillDays, R.LastReceiptDate, R.LastReceiptAmount, R.ReceiptsDays, "
            + "R.ItemQty AS SaleQty, R.BillWeight AS SaleWeight, R.SaleAmount, R.OrderQty, R.DispatchQty, "
            + "CASE WHEN R.BalQty < 0 THEN 0 ELSE R.BalQty END AS BalQty, "
            + "R.OrderWeight, R.DispatchWeight, R.BalWeight, R.BranchesId, R.BranchName, "
            + "P.FromDate AS FromDate, CAST(:toDate AS DATE) AS ToDate, "
            + "P.DueDateFrom AS DueDateFrom, CAST(:dueDateTo AS DATE) AS DueDateTo, "
            + "CAST(:saleFromDate AS DATE) AS SaleFromDate, CAST(:saleToDate AS DATE) AS SaleToDate "
            + "FROM Res3 R CROSS JOIN AccStatus st CROSS JOIN P "
            + "WHERE ( "
            + "(st.AccountStatus = 1 AND NOT EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = R.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 2 AND EXISTS (SELECT 1 FROM TmpShowHideStatusWithAccounts t WHERE t.ChartofAccountId = R.AccountId AND UserId = :userId)) "
            + "OR (st.AccountStatus = 0) ) "
            + "ORDER BY R.DueBalance DESC",
            nativeQuery = true)
    List<ReceivablesReceiptsScheduleRow> receivablesAndReceiptsSchedule(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("dueDateFrom") LocalDate dueDateFrom,
            @Param("dueDateTo") LocalDate dueDateTo,
            @Param("saleFromDate") LocalDate saleFromDate,
            @Param("saleToDate") LocalDate saleToDate,
            @Param("balanceFrom") Double balanceFrom,
            @Param("balanceTo") Double balanceTo,
            @Param("customGroupId") Integer customGroupId,
            @Param("partyGroupId") Integer partyGroupId,
            @Param("userId") Integer userId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("branchesIds") String branchesIds);

    /**
     * usp_ReceivablesAndReceiptsSchedule -- KEEP-EXEC fallback: the original procedure call with all 18
     * parameters (@ActionId and @TradeTypeId are overwritten to 1 inside; @ParentCategoryId unused).
     * Same projection.
     */
    @Query(value = "EXEC dbo.usp_ReceivablesAndReceiptsSchedule "
            + "@OrganizationId = :organizationId, @CompanyId = :companyId, @FromDate = :fromDate, @ToDate = :toDate, "
            + "@DueDateFrom = :dueDateFrom, @DueDateTo = :dueDateTo, @SaleFromDate = :saleFromDate, @SaleToDate = :saleToDate, "
            + "@ParentCategoryId = :parentCategoryId, @BalanceFrom = :balanceFrom, @BalanceTo = :balanceTo, "
            + "@CustomGroupId = :customGroupId, @PartyGroupId = :partyGroupId, @ActionId = :actionId, @UserId = :userId, "
            + "@TradeTypeId = :tradeTypeId, @ControlAccountIds = :controlAccountIds, @BranchesIds = :branchesIds",
            nativeQuery = true)
    List<ReceivablesReceiptsScheduleRow> receivablesAndReceiptsScheduleExec(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("dueDateFrom") LocalDate dueDateFrom,
            @Param("dueDateTo") LocalDate dueDateTo,
            @Param("saleFromDate") LocalDate saleFromDate,
            @Param("saleToDate") LocalDate saleToDate,
            @Param("parentCategoryId") Integer parentCategoryId,
            @Param("balanceFrom") Double balanceFrom,
            @Param("balanceTo") Double balanceTo,
            @Param("customGroupId") Integer customGroupId,
            @Param("partyGroupId") Integer partyGroupId,
            @Param("actionId") Integer actionId,
            @Param("userId") Integer userId,
            @Param("tradeTypeId") Integer tradeTypeId,
            @Param("controlAccountIds") String controlAccountIds,
            @Param("branchesIds") String branchesIds);
}
