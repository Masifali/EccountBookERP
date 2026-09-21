package com.mst.repositories;

import com.mst.models.dto.ContraVoucherDto;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Types;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * The desktop's voucher write path, reproduced.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS EXISTS BESIDE VoucherService
 * ---------------------------------------------------------------------------------------------
 * {@code VoucherService.saveVoucher} persists vouchers with JPA repositories. That is not what
 * the desktop does: {@code Architecture.DAL.Accounts.VoucherHead.SetData} (DAL 0586) runs a chain
 * of stored procedures inside ONE transaction, and several of them do work no JPA save can
 * reproduce — a debit/credit balance check that RAISERRORs, a history mirror, a document-approval
 * row, and (on update) a set of child deletes plus BankReconciliation and
 * VoucherInvoicesAdjustment housekeeping.
 *
 * This class reproduces DAL 0586 step for step. It is deliberately NOT wired into
 * {@code VoucherService}: the nine other voucher screens keep the path they have until each one's
 * own desktop contract has been traced, so correcting Contra cannot quietly change what a Bank
 * Payment Voucher writes.
 *
 * ---------------------------------------------------------------------------------------------
 * THE SEQUENCE (DAL 0586 SetData)
 * ---------------------------------------------------------------------------------------------
 *   0  head.CostCenterAmount = SUM(costAmount)            set BEFORE the header insert
 *   1  Sp_VoucherHead_Insert | Sp_VoucherHead_Update      -> id; if (n > 0) id = n else id = head.Id
 *   2  no details                                         -> throw "Voucher Detail Not Found"
 *   3  per detail: VoucherHeadId = id, limitAmount += DebitAmount,
 *      Sp_VoucherDetail_Insert -> the returned id is written back onto the detail
 *   4  USP_VoucherBalanceCheck @OrganizationId @CompanyId @Id
 *   5  per cost centre: VoucherHeaderId = id, VoucherDetailId = the FIRST detail with the same
 *      SortNo, USP_voucherCostCenterDetail_Insert
 *   6  Sp_VoucherHead_H_Insert -> documentTypeIdRef
 *   7  per detail: DocumentTypeIdRef = documentTypeIdRef, Sp_VoucherDetail_H_Insert
 *   8  [DAW].[USp_DocumentApprovalDetail_Insert] with LimitAmount = SUM(DebitAmount)
 *
 * Steps the desktop also has and Contra never triggers — Sp_PaymentDueSchedule_Insert,
 * USP_VoucherDetailMultiCurrency_Insert, the attachment procedures, and the DocumentTypeId == 5
 * SubNo pairing — are absent here rather than stubbed, so this class cannot write rows the
 * screen it serves cannot produce.
 *
 * ---------------------------------------------------------------------------------------------
 * ATOMICITY
 * ---------------------------------------------------------------------------------------------
 * The desktop opens one SqlTransaction and rolls the whole thing back on any exception. Here the
 * caller's Spring transaction plays that part: every statement below runs on the same connection,
 * and anything that throws — including a RAISERROR out of USP_VoucherBalanceCheck — propagates,
 * so nothing is left half-posted. This method is REQUIRED, never REQUIRES_NEW: a new transaction
 * would be able to commit a voucher whose caller then failed.
 */
@Repository
public class DesktopVoucherWriter {

    private static final Logger LOG = LoggerFactory.getLogger(DesktopVoucherWriter.class);

    private static final String P_HEAD_INS   = "Sp_VoucherHead_Insert";
    private static final String P_HEAD_UPD   = "Sp_VoucherHead_Update";
    private static final String P_DET_INS    = "Sp_VoucherDetail_Insert";
    private static final String P_BALANCE    = "USP_VoucherBalanceCheck";
    private static final String P_COST_INS   = "USP_voucherCostCenterDetail_Insert";
    private static final String P_HEAD_H     = "Sp_VoucherHead_H_Insert";
    private static final String P_DET_H      = "Sp_VoucherDetail_H_Insert";
    private static final String P_APPROVAL   = "[DAW].[USp_DocumentApprovalDetail_Insert]";

    private final JdbcTemplate jdbc;
    public DesktopVoucherWriter(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /**
     * @return the voucher head id, as {@code SetData} returns it.
     */
    @Transactional(propagation = Propagation.REQUIRED)
    public int save(ContraVoucherDto.Head head,
                    List<ContraVoucherDto.Detail> details,
                    List<ContraVoucherDto.CostCentre> costCentres) {

        if (details == null || details.isEmpty()) {
            /* DAL 0586:208 — the desktop's own words, thrown before anything is committed. */
            throw new IllegalStateException("Voucher Detail Not Found");
        }

        /* Step 0 — DAL 0586:30. The header carries the cost-centre total, so it has to be
           computed before the header row is written, not after the children. */
        double costTotal = 0d;
        if (costCentres != null) {
            for (ContraVoucherDto.CostCentre c : costCentres) {
                costTotal += (c.costAmount == null ? 0d : c.costAmount);
            }
        }
        head.CostCenterAmount = costTotal;

        boolean insert = head.Id == null || head.Id <= 0;
        head.ActionId = insert ? 1 : 2;                       // BLL 0654

        /* Step 1 */
        Integer returned = execReturningId(insert ? P_HEAD_INS : P_HEAD_UPD, head);
        int id = (returned != null && returned > 0) ? returned : (head.Id == null ? 0 : head.Id);
        if (id <= 0) throw new IllegalStateException("Voucher save returned no Id");
        head.Id = id;

        /* Step 3 */
        BigDecimal limitAmount = BigDecimal.ZERO;
        for (ContraVoucherDto.Detail d : details) {
            d.VoucherHeadId = id;
            limitAmount = limitAmount.add(BigDecimal.valueOf(d.DebitAmount == null ? 0d : d.DebitAmount));
            Integer detailId = execReturningId(P_DET_INS, d);
            /* The returned id is written back because step 5 resolves VoucherDetailId from it. */
            d.Id = (detailId == null) ? 0 : detailId;
        }

        /* Step 4 — the debit/credit enforcement. It RAISERRORs
           "Debit Amount not equal to Credit Amount. Debit is X and Credit is Y" when
           ABS(Debit - Credit) >= 1, which is precisely why a Contra voucher must be written as
           a debit/credit PAIR per grid row rather than one line. */
        exec("EXEC dbo." + P_BALANCE + " @OrganizationId=?, @CompanyId=?, @Id=?",
                head.OrganizationId, head.CompanyId, id);

        /* Step 5 */
        if (costCentres != null) {
            for (ContraVoucherDto.CostCentre c : costCentres) {
                c.VoucherHeaderId = id;
                /* DAL 0586:111 — .Find returns the FIRST detail with this SortNo. Both lines of a
                   Contra pair carry the same SortNo, so the cost centre attaches to the DEBIT
                   line. Reproduced with the same first-match rule rather than "the debit one",
                   so a screen whose pair is ordered differently still behaves as the DAL does. */
                for (ContraVoucherDto.Detail d : details) {
                    if (d.SortNo != null && c.SortNo != null && d.SortNo.intValue() == c.SortNo.intValue()) {
                        c.VoucherDetailId = d.Id;
                        break;
                    }
                }
                execReturningId(P_COST_INS, c);
            }
        }

        /* Steps 6 and 7 — the history mirror. There is NO trigger on VoucherHead or VoucherDetail
           (checked against the schema dump), so these rows exist only because the application
           writes them, on insert AND on update. */
        Integer documentTypeIdRef = execReturningId(P_HEAD_H, head);
        for (ContraVoucherDto.Detail d : details) {
            d.VoucherHeadId = id;
            d.DocumentTypeIdRef = (documentTypeIdRef == null) ? 0 : documentTypeIdRef;
            execReturningId(P_DET_H, d);
        }

        /* Step 8 */
        ContraVoucherDto.ApprovalDetail appr = new ContraVoucherDto.ApprovalDetail();
        appr.OrganizationId = head.OrganizationId;
        appr.CompanyId      = head.CompanyId;
        appr.DocumentTypeId = head.DocumentTypeId;
        appr.Id             = id;
        appr.LimitAmount    = limitAmount;
        execReturningId(P_APPROVAL, appr);

        return id;
    }

    // =========================================================================== balance read

    /**
     * {@code CommonServices.GetAccountBalance} →
     * {@code VoucherHead.ReadByCurrentBalanceByDateAndAccountId}, reading the Balance column.
     * Used by the negative-balance guard, which runs BEFORE the save on the desktop.
     */
    public double accountBalance(int organizationId, int companyId, int financialYearId,
                                 String voucherDate, int refAccountId) {
        List<Map<String, Object>> rows = exec(
                "EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, "
              + "@FinancialYearId=?, @VoucherDate=?, @RefAccountId=?, @Activity=?",
                organizationId, companyId, financialYearId, voucherDate, refAccountId,
                "ReadByCurrentBalanceByDateAndAccountId");
        if (rows.isEmpty()) return 0d;
        Object v = ci(rows.get(0), "Balance");
        if (v instanceof Number) return ((Number) v).doubleValue();
        if (v == null) return 0d;
        try { return Double.parseDouble(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0d; }
    }

    /**
     * {@code VoucherHead.VoucherExistWithSameAmountInSameDate} — one call per distinct AccountId
     * among the lines with a debit. A hit is a CONFIRMATION on the desktop, not a refusal, so
     * this returns the matched title rather than throwing.
     *
     * @return the Title of the first clash, or null when there is none.
     */
    public String duplicateVoucherTitle(int organizationId, int companyId, String voucherDate,
                                        int accountId, double debitAmount, int actionId) {
        List<Map<String, Object>> rows = exec(
                "EXEC [dbo].[USP_VoucherExistWithSameAmountInSameDate] @OrganizationId=?, "
              + "@CompanyId=?, @AccountId=?, @VoucherDate=?, @DebitAmount=?, @ActionId=?",
                organizationId, companyId, accountId, voucherDate, debitAmount, actionId);
        if (rows.isEmpty()) return null;
        Object t = ci(rows.get(0), "Title");
        return t == null ? "" : String.valueOf(t);
    }

    /** {@code CommonServices.GenerateVoucherCode(documentTypeId)} — BLL 0654. */
    public int nextVoucherCode(int organizationId, int companyId, int documentTypeId,
                               int financialYearId, int branchesId) {
        List<Map<String, Object>> rows = exec(
                "EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, "
              + "@DocumentTypeId=?, @FinancialYearId=?, @BranchesId=?, @Activity=?",
                organizationId, companyId, documentTypeId, financialYearId, branchesId,
                "GenerateVoucherCodeByDocumentTypeId");
        if (rows.isEmpty()) return 0;
        Object v = ci(rows.get(0), "VoucherCode");
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    // =============================================================================== plumbing

    /**
     * A null with no type attached is sent by the SQL Server driver as an INTEGER null, and a
     * procedure that declares the parameter as a date then fails with
     * "Operand type clash: int is incompatible with date" before it runs at all. Every null
     * therefore carries the type of the field it came from.
     */
    private static SqlParameterValue typedNull(Class<?> type) {
        if (type == Integer.class || type == int.class
         || type == Long.class    || type == long.class
         || type == Short.class   || type == short.class)   return new SqlParameterValue(Types.INTEGER, null);
        if (type == Double.class  || type == double.class
         || type == Float.class   || type == float.class)   return new SqlParameterValue(Types.DOUBLE, null);
        if (type == BigDecimal.class)                       return new SqlParameterValue(Types.NUMERIC, null);
        if (type == Boolean.class || type == boolean.class) return new SqlParameterValue(Types.BIT, null);
        /* Dates travel as strings on these DTOs, and SQL Server converts a NULL varchar to a
           NULL date without complaint — unlike a NULL int. */
        return new SqlParameterValue(Types.VARCHAR, null);
    }

    /**
     * {@code EXEC <proc> @Field=?, ...} over every public field of the object, in declaration
     * order, mirroring what GenericProvider.SetProc does by reflection. ExecuteScalar's value is
     * the new id, so the statement runs as a query and the first column of the first row is read.
     *
     * Every field name was checked against the procedure's own declared parameters before this
     * was written — see the DTO's class note.
     */
    private Integer execReturningId(String proc, Object obj) {
        List<String> names = new ArrayList<>();
        List<Object> values = new ArrayList<>();
        for (java.lang.reflect.Field f : obj.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            Object v;
            try { v = f.get(obj); } catch (IllegalAccessException e) { continue; }
            names.add(f.getName());
            values.add(v == null ? typedNull(f.getType()) : v);
        }
        StringBuilder sql = new StringBuilder("EXEC ");
        sql.append(proc.startsWith("[") ? proc : "dbo." + proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append('@').append(names.get(i)).append("=?");
        }
        try {
            List<Map<String, Object>> rows = exec(sql.toString(), values.toArray());
            if (rows.isEmpty()) return 0;
            Map<String, Object> first = rows.get(0);
            if (first.isEmpty()) return 0;
            Object v = first.values().iterator().next();
            return (v instanceof Number) ? ((Number) v).intValue() : 0;
        } catch (RuntimeException e) {
            /* Named, never swallowed: a failure here rolls the whole voucher back, and the log
               has to say which procedure refused it. */
            LOG.warn("{} failed", proc, e);
            throw e;
        }
    }

    /**
     * Execute a procedure that MAY or MAY NOT return a result set, and return its first result
     * set as rows (empty when there is none).
     *
     * {@code JdbcTemplate.queryForList} insists on a result set and throws
     * "The statement did not return a result set." otherwise. Several of these procedures return
     * nothing on the happy path:
     *
     *   - USP_VoucherBalanceCheck returns rows only when it RAISERRORs; a balanced voucher
     *     produces no result set at all, so queryForList failed on every SUCCESSFUL save.
     *   - USP_VoucherExistWithSameAmountInSameDate returns rows only when there IS a clash.
     *
     * The desktop never had this problem: GenericProvider fills a DataTable, and a procedure that
     * returns nothing simply yields zero rows. This is the equivalent — execute(), then read a
     * result set only if one is actually there.
     */
    private List<Map<String, Object>> exec(String sql, Object... args) {
        return jdbc.execute(sql, (java.sql.PreparedStatement ps) -> {
            for (int i = 0; i < args.length; i++) {
                Object a = args[i];
                if (a instanceof SqlParameterValue) {
                    SqlParameterValue v = (SqlParameterValue) a;
                    if (v.getValue() == null) ps.setNull(i + 1, v.getSqlType());
                    else ps.setObject(i + 1, v.getValue(), v.getSqlType());
                } else {
                    ps.setObject(i + 1, a);
                }
            }
            boolean hasResultSet = ps.execute();
            /* Step past any update counts to the first real result set, the way ExecuteScalar
               and a DataTable fill both do. */
            while (!hasResultSet && ps.getUpdateCount() != -1) {
                hasResultSet = ps.getMoreResults();
            }
            List<Map<String, Object>> out = new ArrayList<>();
            if (!hasResultSet) return out;
            try (java.sql.ResultSet rs = ps.getResultSet()) {
                if (rs == null) return out;
                java.sql.ResultSetMetaData md = rs.getMetaData();
                int n = md.getColumnCount();
                while (rs.next()) {
                    Map<String, Object> row = new java.util.LinkedHashMap<>();
                    for (int c = 1; c <= n; c++) {
                        String label = md.getColumnLabel(c);
                        if (label == null || label.isEmpty()) label = "col" + c;
                        row.put(label, rs.getObject(c));
                    }
                    out.add(row);
                }
            }
            return out;
        });
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }
}
