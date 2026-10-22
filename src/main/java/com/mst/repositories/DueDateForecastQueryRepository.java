package com.mst.repositories;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.ChartofAccount;
import com.mst.projections.DueDateForecastBucketRow;
import com.mst.projections.DueDateForecastDailyRow;

/**
 * Query-only repository (bound to the existing ChartofAccount entity) for
 * usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast. The procedure fills #AllData (opening balances
 * + vouchers of AccountTypeId 3 due up to @ToDate), computes the net / overdue totals, generates one row per
 * day between @FromDate and @ToDate (recursive CTE, OPTION (MAXRECURSION 0)) with the sale / export
 * receivables and purchase / import payables due that day into #ReceivablesPayablesSummary, and returns
 * TWO result sets. Each set is one native statement here that rebuilds the same CTE chain; the shared
 * P CTE resolves the procedure defaults (@DaysInterval NULL -> 8, @ToDate NULL -> MAX(DueDate) of #AllData).
 * The procedure's literal CAST('12:00:00 AM' AS DATETIME) is written as CAST('1900-01-01' AS DATETIME)
 * (same value) because Hibernate reads ':00' as a named parameter.
 */
@Repository
public interface DueDateForecastQueryRepository extends org.springframework.data.repository.Repository<ChartofAccount, Integer> {

    /**
     * usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast -- FIRST result set: one row per calendar
     * day with daily and running receivables / payables, the net / overdue / not-yet-due totals and MaxDueDate,
     * optionally restricted to one aging bucket by @SortNo (1 = up to @FromDate, 2 = next @DaysInterval days,
     * 3 / 4 = following intervals, 5 = beyond 3 intervals).
     * Native CTE rewrite: AllData, Totals, P, DateRange (recursive), Receivables, Payables, Summary.
     */
    @Query(value = "WITH AllData AS ( "
            + "SELECT CAST('1900-01-01' AS DATE) AS VoucherDate, CAST('1900-01-01' AS DATETIME) AS VoucherTime, CAST('1900-01-01' AS DATE) AS DueDate, "
            + "o.ChartOfAccountId AS AccountId, CAST(ISNULL(o.YearObDebit, 0) AS FLOAT) AS DebitAmount, CAST(ISNULL(o.YearObCredit, 0) AS FLOAT) AS CreditAmount, "
            + "0 AS Id, 0 AS DocumentTypeId "
            + "FROM AccountsOpeningBalances o INNER JOIN ChartofAccount c ON o.ChartOfAccountId = c.Id "
            + "WHERE o.OrganizationId = :organizationId AND o.CompanyId = :companyId AND c.AccountTypeId = 3 "
            + "UNION ALL "
            + "SELECT CAST(vh.VoucherDate AS DATE), CAST(vh.EntryDate AS DATETIME), CAST(ISNULL(vh.DueDate, vh.VoucherDate) AS DATE), "
            + "vd.AccountId, CAST(ISNULL(vd.DebitAmount, 0) AS FLOAT), CAST(ISNULL(vd.CreditAmount, 0) AS FLOAT), vh.Id, vh.DocumentTypeId "
            + "FROM VoucherHead vh INNER JOIN VoucherDetail vd ON vh.Id = vd.VoucherHeadId INNER JOIN ChartofAccount c ON vd.AccountId = c.Id "
            + "WHERE vh.OrganizationId = :organizationId AND vh.CompanyId = :companyId "
            + "AND (:toDate IS NULL OR CAST(vh.DueDate AS DATE) <= CAST(:toDate AS DATE)) "
            + "AND c.AccountTypeId = 3 "
            + "), Totals AS ( "
            + "SELECT CAST(SUM(CASE WHEN c.AccountClass = 2 THEN ROUND(T.DebitAmount - T.CreditAmount, 0) ELSE 0 END) AS DECIMAL(18,2)) AS NetReceivables, "
            + "CAST(SUM(CASE WHEN c.AccountClass = 2 AND T.DueDate < :fromDate THEN ROUND(T.DebitAmount - T.CreditAmount, 0) ELSE 0 END) AS DECIMAL(18,2)) AS ReceivablesOverDue, "
            + "CAST(SUM(CASE WHEN c.AccountClass = 3 THEN ROUND(T.CreditAmount - T.DebitAmount, 0) ELSE 0 END) AS DECIMAL(18,2)) AS NetPayables, "
            + "CAST(SUM(CASE WHEN c.AccountClass = 3 AND T.DueDate < :fromDate THEN ROUND(T.CreditAmount - T.DebitAmount, 0) ELSE 0 END) AS DECIMAL(18,2)) AS PayablesOverDue, "
            + "MAX(T.DueDate) AS MaxDueDate "
            + "FROM AllData T INNER JOIN ChartofAccount c ON T.AccountId = c.Id "
            + "), P AS ( "
            + "SELECT CAST(:fromDate AS DATE) AS FromDate, CAST(COALESCE(:toDate, Totals.MaxDueDate) AS DATE) AS ToDate, COALESCE(:daysInterval, 8) AS DaysInterval "
            + "FROM Totals "
            + "), DateRange AS ( "
            + "SELECT P.FromDate AS DueDate FROM P "
            + "UNION ALL "
            + "SELECT DATEADD(DAY, 1, d.DueDate) FROM DateRange d CROSS JOIN P WHERE DATEADD(DAY, 1, d.DueDate) <= P.ToDate "
            + "), Receivables AS ( "
            + "SELECT h.DueDate, "
            + "SUM(CASE WHEN h.DocumentTypeId IN (95,99,1660,1661) THEN h.BillAmount ELSE 0 END) AS SaleReceivable, "
            + "SUM(CASE WHEN h.DocumentTypeId = 27 THEN h.BillAmount ELSE 0 END) AS ExportReceivable "
            + "FROM VoucherHead h CROSS JOIN P "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND h.DueDate BETWEEN P.FromDate AND P.ToDate AND h.DocumentTypeId IN (95,99,27,1660,1661) "
            + "GROUP BY h.DueDate "
            + "), Payables AS ( "
            + "SELECT h.DueDate, "
            + "SUM(CASE WHEN h.DocumentTypeId IN (56,57,702,64,1603,1604,1653,1654) THEN h.BillAmount ELSE 0 END) AS PurchasePayable, "
            + "SUM(CASE WHEN h.DocumentTypeId = 1205 THEN h.BillAmount ELSE 0 END) AS ImportPayable "
            + "FROM VoucherHead h CROSS JOIN P "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND h.DueDate BETWEEN P.FromDate AND P.ToDate AND h.DocumentTypeId IN (56,57,702,64,1205,1604,1603,1653,1654) "
            + "GROUP BY h.DueDate "
            + "), Summary AS ( "
            + "SELECT dr.DueDate, "
            + "CAST(ISNULL(r.SaleReceivable, 0) AS DECIMAL(18,2)) AS SaleReceivable, "
            + "CAST(ISNULL(r.ExportReceivable, 0) AS DECIMAL(18,2)) AS ExportReceivable, "
            + "CAST((ISNULL(r.SaleReceivable, 0) + ISNULL(r.ExportReceivable, 0)) AS DECIMAL(18,2)) AS TotalReceivables, "
            + "CAST(SUM(ISNULL(r.SaleReceivable, 0) + ISNULL(r.ExportReceivable, 0)) OVER (ORDER BY dr.DueDate) AS DECIMAL(18,2)) AS RunningTotalReceivables, "
            + "CAST(ISNULL(p.PurchasePayable, 0) AS DECIMAL(18,2)) AS PurchasePayable, "
            + "CAST(ISNULL(p.ImportPayable, 0) AS DECIMAL(18,2)) AS ImportPayable, "
            + "CAST((ISNULL(p.PurchasePayable, 0) + ISNULL(p.ImportPayable, 0)) AS DECIMAL(18,2)) AS TotalPayables, "
            + "CAST(SUM(ISNULL(p.PurchasePayable, 0) + ISNULL(p.ImportPayable, 0)) OVER (ORDER BY dr.DueDate) AS DECIMAL(18,2)) AS RunningTotalPayables, "
            + "CAST((ISNULL(r.SaleReceivable, 0) + ISNULL(r.ExportReceivable, 0)) - (ISNULL(p.PurchasePayable, 0) + ISNULL(p.ImportPayable, 0)) AS DECIMAL(18,2)) AS ReceivablesMinusPayables, "
            + "CAST(SUM((ISNULL(r.SaleReceivable, 0) + ISNULL(r.ExportReceivable, 0)) - (ISNULL(p.PurchasePayable, 0) + ISNULL(p.ImportPayable, 0))) OVER (ORDER BY dr.DueDate) AS DECIMAL(18,2)) AS RunningReceivables "
            + "FROM DateRange dr "
            + "LEFT JOIN Receivables r ON dr.DueDate = r.DueDate "
            + "LEFT JOIN Payables p ON dr.DueDate = p.DueDate "
            + ") "
            + "SELECT R.DueDate, R.SaleReceivable, R.ExportReceivable, R.TotalReceivables, R.RunningTotalReceivables, "
            + "R.PurchasePayable, R.ImportPayable, R.TotalPayables, R.RunningTotalPayables, R.ReceivablesMinusPayables, R.RunningReceivables, "
            + "tt.NetReceivables AS NetReceivables, "
            + "tt.ReceivablesOverDue AS ReceivablesOverDue, "
            + "CASE WHEN tt.NetReceivables > tt.ReceivablesOverDue THEN tt.NetReceivables - tt.ReceivablesOverDue ELSE SUM(R.TotalReceivables) OVER() END AS ReceivablesNotYetDue, "
            + "tt.NetPayables AS NetPayables, "
            + "tt.PayablesOverDue AS PayablesOverDue, "
            + "CASE WHEN tt.NetPayables > tt.PayablesOverDue THEN tt.NetPayables - tt.PayablesOverDue ELSE SUM(R.TotalPayables) OVER() END AS PayablesNotYetDue, "
            + "tt.MaxDueDate AS MaxDueDate "
            + "FROM Summary R CROSS JOIN Totals tt CROSS JOIN P "
            + "WHERE (:sortNo IS NULL "
            + "OR (:sortNo = 1 AND R.DueDate <= P.FromDate) "
            + "OR (:sortNo = 2 AND R.DueDate BETWEEN DATEADD(DAY, 1, P.FromDate) AND DATEADD(DAY, P.DaysInterval, P.FromDate)) "
            + "OR (:sortNo = 3 AND R.DueDate BETWEEN DATEADD(DAY, P.DaysInterval + 1, P.FromDate) AND DATEADD(DAY, P.DaysInterval * 2, P.FromDate)) "
            + "OR (:sortNo = 4 AND R.DueDate BETWEEN DATEADD(DAY, (P.DaysInterval * 2) + 1, P.FromDate) AND DATEADD(DAY, P.DaysInterval * 3, P.FromDate)) "
            + "OR (:sortNo = 5 AND (R.DueDate >= DATEADD(DAY, (P.DaysInterval * 3) + 1, P.FromDate) OR R.DueDate IS NULL))) "
            + "OPTION (MAXRECURSION 0)",
            nativeQuery = true)
    List<DueDateForecastDailyRow> dueDateForecastDaily(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("sortNo") Integer sortNo,
            @Param("daysInterval") Integer daysInterval);

    /**
     * usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast -- SECOND result set: the five aging buckets
     * ('Due Upto Today (...)', 'Due in Next n Days (...)', ..., 'Due After 3n+1 Days (...)') with bucket sums
     * and cumulative receivables / payables / balance over the same daily summary (not filtered by @SortNo).
     * Same CTE chain as {@link #dueDateForecastDaily}; AgingCategories built from the P CTE.
     */
    @Query(value = "WITH AllData AS ( "
            + "SELECT CAST('1900-01-01' AS DATE) AS VoucherDate, CAST('1900-01-01' AS DATETIME) AS VoucherTime, CAST('1900-01-01' AS DATE) AS DueDate, "
            + "o.ChartOfAccountId AS AccountId, CAST(ISNULL(o.YearObDebit, 0) AS FLOAT) AS DebitAmount, CAST(ISNULL(o.YearObCredit, 0) AS FLOAT) AS CreditAmount, "
            + "0 AS Id, 0 AS DocumentTypeId "
            + "FROM AccountsOpeningBalances o INNER JOIN ChartofAccount c ON o.ChartOfAccountId = c.Id "
            + "WHERE o.OrganizationId = :organizationId AND o.CompanyId = :companyId AND c.AccountTypeId = 3 "
            + "UNION ALL "
            + "SELECT CAST(vh.VoucherDate AS DATE), CAST(vh.EntryDate AS DATETIME), CAST(ISNULL(vh.DueDate, vh.VoucherDate) AS DATE), "
            + "vd.AccountId, CAST(ISNULL(vd.DebitAmount, 0) AS FLOAT), CAST(ISNULL(vd.CreditAmount, 0) AS FLOAT), vh.Id, vh.DocumentTypeId "
            + "FROM VoucherHead vh INNER JOIN VoucherDetail vd ON vh.Id = vd.VoucherHeadId INNER JOIN ChartofAccount c ON vd.AccountId = c.Id "
            + "WHERE vh.OrganizationId = :organizationId AND vh.CompanyId = :companyId "
            + "AND (:toDate IS NULL OR CAST(vh.DueDate AS DATE) <= CAST(:toDate AS DATE)) "
            + "AND c.AccountTypeId = 3 "
            + "), Totals AS ( "
            + "SELECT MAX(T.DueDate) AS MaxDueDate FROM AllData T INNER JOIN ChartofAccount c ON T.AccountId = c.Id "
            + "), P AS ( "
            + "SELECT CAST(:fromDate AS DATE) AS FromDate, CAST(COALESCE(:toDate, Totals.MaxDueDate) AS DATE) AS ToDate, COALESCE(:daysInterval, 8) AS DaysInterval "
            + "FROM Totals "
            + "), DateRange AS ( "
            + "SELECT P.FromDate AS DueDate FROM P "
            + "UNION ALL "
            + "SELECT DATEADD(DAY, 1, d.DueDate) FROM DateRange d CROSS JOIN P WHERE DATEADD(DAY, 1, d.DueDate) <= P.ToDate "
            + "), Receivables AS ( "
            + "SELECT h.DueDate, "
            + "SUM(CASE WHEN h.DocumentTypeId IN (95,99,1660,1661) THEN h.BillAmount ELSE 0 END) AS SaleReceivable, "
            + "SUM(CASE WHEN h.DocumentTypeId = 27 THEN h.BillAmount ELSE 0 END) AS ExportReceivable "
            + "FROM VoucherHead h CROSS JOIN P "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND h.DueDate BETWEEN P.FromDate AND P.ToDate AND h.DocumentTypeId IN (95,99,27,1660,1661) "
            + "GROUP BY h.DueDate "
            + "), Payables AS ( "
            + "SELECT h.DueDate, "
            + "SUM(CASE WHEN h.DocumentTypeId IN (56,57,702,64,1603,1604,1653,1654) THEN h.BillAmount ELSE 0 END) AS PurchasePayable, "
            + "SUM(CASE WHEN h.DocumentTypeId = 1205 THEN h.BillAmount ELSE 0 END) AS ImportPayable "
            + "FROM VoucherHead h CROSS JOIN P "
            + "WHERE h.OrganizationId = :organizationId AND h.CompanyId = :companyId "
            + "AND h.DueDate BETWEEN P.FromDate AND P.ToDate AND h.DocumentTypeId IN (56,57,702,64,1205,1604,1603,1653,1654) "
            + "GROUP BY h.DueDate "
            + "), Summary AS ( "
            + "SELECT dr.DueDate, "
            + "CAST(ISNULL(r.SaleReceivable, 0) AS DECIMAL(18,2)) AS SaleReceivable, "
            + "CAST(ISNULL(r.ExportReceivable, 0) AS DECIMAL(18,2)) AS ExportReceivable, "
            + "CAST((ISNULL(r.SaleReceivable, 0) + ISNULL(r.ExportReceivable, 0)) AS DECIMAL(18,2)) AS TotalReceivables, "
            + "CAST(ISNULL(p.PurchasePayable, 0) AS DECIMAL(18,2)) AS PurchasePayable, "
            + "CAST(ISNULL(p.ImportPayable, 0) AS DECIMAL(18,2)) AS ImportPayable, "
            + "CAST((ISNULL(p.PurchasePayable, 0) + ISNULL(p.ImportPayable, 0)) AS DECIMAL(18,2)) AS TotalPayables, "
            + "CAST((ISNULL(r.SaleReceivable, 0) + ISNULL(r.ExportReceivable, 0)) - (ISNULL(p.PurchasePayable, 0) + ISNULL(p.ImportPayable, 0)) AS DECIMAL(18,2)) AS ReceivablesMinusPayables "
            + "FROM DateRange dr "
            + "LEFT JOIN Receivables r ON dr.DueDate = r.DueDate "
            + "LEFT JOIN Payables p ON dr.DueDate = p.DueDate "
            + "), AgingCategories AS ( "
            + "SELECT 1 AS SortNo, 'Due Upto Today (' + FORMAT(P.FromDate, 'MMM dd, yyyy') + ')' AS AgingBucket, P.FromDate AS StartDate, P.FromDate AS EndDate FROM P "
            + "UNION ALL "
            + "SELECT 2, 'Due in Next ' + CAST(P.DaysInterval AS VARCHAR(10)) + ' Days (' "
            + "+ FORMAT(DATEADD(DAY, 1, P.FromDate), 'MMM d') + ' - ' + FORMAT(DATEADD(DAY, P.DaysInterval, P.FromDate), 'MMM d, yyyy') + ')', "
            + "DATEADD(DAY, 1, P.FromDate), DATEADD(DAY, P.DaysInterval, P.FromDate) FROM P "
            + "UNION ALL "
            + "SELECT 3, 'Due in Next ' + CAST(P.DaysInterval + 1 AS VARCHAR(10)) + ' to ' + CAST(P.DaysInterval * 2 AS VARCHAR(10)) + ' Days (' "
            + "+ FORMAT(DATEADD(DAY, P.DaysInterval + 1, P.FromDate), 'MMM d') + ' - ' + FORMAT(DATEADD(DAY, P.DaysInterval * 2, P.FromDate), 'MMM d, yyyy') + ')', "
            + "DATEADD(DAY, P.DaysInterval + 1, P.FromDate), DATEADD(DAY, P.DaysInterval * 2, P.FromDate) FROM P "
            + "UNION ALL "
            + "SELECT 4, 'Due in Next ' + CAST((P.DaysInterval * 2) + 1 AS VARCHAR(10)) + ' to ' + CAST(P.DaysInterval * 3 AS VARCHAR(10)) + ' Days (' "
            + "+ FORMAT(DATEADD(DAY, (P.DaysInterval * 2) + 1, P.FromDate), 'MMM d') + ' - ' + FORMAT(DATEADD(DAY, P.DaysInterval * 3, P.FromDate), 'MMM d, yyyy') + ')', "
            + "DATEADD(DAY, (P.DaysInterval * 2) + 1, P.FromDate), DATEADD(DAY, P.DaysInterval * 3, P.FromDate) FROM P "
            + "UNION ALL "
            + "SELECT 5, 'Due After ' + CAST((P.DaysInterval * 3) + 1 AS VARCHAR(10)) + ' Days (' "
            + "+ FORMAT(DATEADD(DAY, (P.DaysInterval * 3) + 1, P.FromDate), 'MMM d, yyyy') + ' and beyond)', "
            + "DATEADD(DAY, (P.DaysInterval * 3) + 1, P.FromDate), NULL FROM P "
            + ") "
            + "SELECT A.SortNo, A.AgingBucket AS [Description], "
            + "COALESCE(SUM(R.SaleReceivable), 0) AS SaleReceivable, "
            + "COALESCE(SUM(R.ExportReceivable), 0) AS ExportReceivable, "
            + "COALESCE(SUM(R.TotalReceivables), 0) AS TotalReceivables, "
            + "SUM(SUM(COALESCE(R.TotalReceivables, 0))) OVER (ORDER BY A.SortNo ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS ComulativeReceivables, "
            + "COALESCE(SUM(R.PurchasePayable), 0) AS PurchasePayable, "
            + "COALESCE(SUM(R.ImportPayable), 0) AS ImportPayable, "
            + "COALESCE(SUM(R.TotalPayables), 0) AS TotalPayables, "
            + "SUM(SUM(COALESCE(R.TotalPayables, 0))) OVER (ORDER BY A.SortNo ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS ComulativePayables, "
            + "COALESCE(SUM(R.ReceivablesMinusPayables), 0) AS NetBalance, "
            + "SUM(SUM(COALESCE(R.ReceivablesMinusPayables, 0))) OVER (ORDER BY A.SortNo ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS ComulativeBalance "
            + "FROM AgingCategories A "
            + "LEFT JOIN Summary R ON (R.DueDate BETWEEN A.StartDate AND COALESCE(A.EndDate, '9999-12-31')) "
            + "GROUP BY A.SortNo, A.AgingBucket "
            + "ORDER BY A.SortNo "
            + "OPTION (MAXRECURSION 0)",
            nativeQuery = true)
    List<DueDateForecastBucketRow> dueDateForecastBuckets(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("daysInterval") Integer daysInterval);

    /**
     * usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast -- KEEP-EXEC fallback: the original
     * procedure call with all 6 parameters. JPA returns the FIRST result set only (daily rows); the bucket
     * set is available through {@link #dueDateForecastBuckets}.
     */
    @Query(value = "EXEC dbo.usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast "
            + "@OrganizationId = :organizationId, @CompanyId = :companyId, @FromDate = :fromDate, @ToDate = :toDate, "
            + "@SortNo = :sortNo, @DaysInterval = :daysInterval",
            nativeQuery = true)
    List<DueDateForecastDailyRow> dueDateForecastDailyExec(
            @Param("organizationId") Integer organizationId,
            @Param("companyId") Integer companyId,
            @Param("fromDate") LocalDate fromDate,
            @Param("toDate") LocalDate toDate,
            @Param("sortNo") Integer sortNo,
            @Param("daysInterval") Integer daysInterval);
}
