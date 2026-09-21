package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.DesktopVoucherWriter;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

/**
 * Screen 22 "Contra Voucher" — {@code Architecture.WinApp.Account_Definition.ContraVoucher},
 * DocumentTypeId 10.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS IS A SCREEN OF ITS OWN AND NOT A CALL INTO VoucherService
 * ---------------------------------------------------------------------------------------------
 * The generic {@code VoucherService.saveVoucher} writes vouchers with JPA and one detail line per
 * grid row. The desktop writes a debit/credit PAIR per row through a chain of stored procedures,
 * one of which — {@code USP_VoucherBalanceCheck} — RAISERRORs unless debits equal credits. The
 * two behaviours cannot both be right.
 *
 * Correcting that inside the shared service would silently change nine other voucher screens
 * whose own desktop contracts have not been traced yet. So Contra gets its own service and its
 * own writer ({@link DesktopVoucherWriter}), and the shared path is left exactly as it is.
 *
 * ---------------------------------------------------------------------------------------------
 * ORDER OF OPERATIONS — ContraVoucher.Insert() (line 458)
 * ---------------------------------------------------------------------------------------------
 *   1  grid empty                     -> "Grid Record not found"
 *   2  FormValidation()               -> the seven header checks, in the desktop's order
 *   3  row with Amount > 0, no account-> "Please Select Account Title First"
 *   4  build header, pair, cost centres
 *   5  negative-balance guard         -> refuse, or warn and require an acknowledgement
 *   6  duplicate-voucher guard        -> warn and require an acknowledgement
 *   7  DesktopVoucherWriter.save()    -> the procedure chain, one transaction
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT THE CLIENT CANNOT DECIDE
 * ---------------------------------------------------------------------------------------------
 * DocumentTypeId is fixed at 10. Organization, Company, Branch, FinancialYear, EntryUser and
 * ModifyUser come from the signed-in user. The client posts GRID ROWS, never detail lines: which
 * account is debited and which is credited is derived here, because letting a caller choose that
 * is letting them write any journal they like.
 */
@Service
public class ContraVoucherService {

    private static final Logger LOG = LoggerFactory.getLogger(ContraVoucherService.class);

    /** ContraVoucher.cs:505 — vh.DocumentTypeId = 10. */
    public static final int DOCUMENT_TYPE_ID = 10;

    /** The desktop screen name, for the per-screen grant lookup. */
    private static final String SCREEN_NAME = "ContraVoucher";

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
          + "@CompanyId=?, @Activity=?";

    private static final String SQL_CONFIG =
            "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
          + "@ConfigDescription=?, @Activity=?";

    @Autowired private DesktopVoucherWriter writer;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;

    // =============================================================================== save

    @Transactional
    public Map<String, Object> save(ContraVoucherDto dto) {
        UserAccount u = currentUserContext.requireAccountingUser();

        boolean insert = dto.Id == null || dto.Id <= 0;
        /* AcfrmPaymentVoucher_Load:821 only disables the buttons. A disabled button is not a
           permission check, so the rule is enforced here too. */
        if (insert && !hasRight("Save")) {
            throw new IllegalStateException("You do not have the Save right for this screen.");
        }
        if (!insert && !hasRight("Update")) {
            throw new IllegalStateException("You do not have the Update right for this screen.");
        }

        int orgId    = u.getOrganizationId();
        int compId   = u.getCompanyId();
        int branchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        int userId   = u.getId();
        int yearId   = currentUserContext.currentFinancialYearId();

        /* MultiCurrencyFeature():974 -> GetERPFeatureById(6); the Branch feature is 17
           (AcfrmPaymentVoucher_Load:866). Both ids are the desktop's, not a guess. */
        boolean multiCurrency = feature(orgId, compId, 6);
        boolean branchFeature = feature(orgId, compId, 17);

        validate(dto, multiCurrency);

        String voucherDate = isoDay(dto.VoucherDate);

        // ---------------------------------------------------------------- header (§3 of the trace)

        ContraVoucherDto.Head head = new ContraVoucherDto.Head();
        head.Id                   = insert ? 0 : dto.Id;
        head.DocumentTypeId       = DOCUMENT_TYPE_ID;
        head.VoucherCode          = dto.VoucherCode == null ? 0 : dto.VoucherCode;
        head.VoucherDate          = voucherDate;
        head.ProjectId            = nz(dto.ProjectId);
        head.RefAccountId         = nz(dto.RefAccountId);
        head.ChequeDate           = isoDay(dto.ChequeDate);
        head.ChequeNo             = dto.ChequeNo;
        head.CheqId               = nz(dto.CheqId);
        head.PayTitle             = dto.PayTitle;
        head.Remarks              = dto.Remarks == null ? "" : dto.Remarks.trim();
        head.MultiCurrencyId      = nz(dto.MultiCurrencyId);
        head.ExchangeCurrencyRate = nzd(dto.ExchangeCurrencyRate);
        head.FcAmount             = nzd(dto.FcAmount);
        head.IncludeWHT           = Boolean.FALSE;            // :521
        head.IsApproved           = Boolean.FALSE;
        head.OrganizationId       = orgId;
        head.CompanyId            = compId;
        head.BranchId             = branchId;
        head.FinancialYearId      = yearId;
        head.EntryUser            = userId;
        head.ModifyUser           = userId;

        /* BLL 0654 sets both on every save, insert and update alike. */
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        head.EntryDate  = now;
        head.ModifyDate = now;

        /* AgainstAccountId starts as the credit account (:508) and is then overwritten inside the
           row loop (:604) with each row's account, so it ends as the LAST row's. That is what the
           desktop stores; it is reproduced rather than "corrected". */
        head.AgainstAccountId = nz(dto.RefAccountId);

        // ------------------------------------------------- the debit/credit pair (§4 of the trace)

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        List<ContraVoucherDto.CostCentre> costCentres = new ArrayList<>();
        double voucherAmount = 0d;
        int lineId = 1;

        for (int i = 0; i < dto.rows.size(); i++) {
            ContraVoucherDto.Row r = dto.rows.get(i);
            int sortNo = i + 1;                                // :546 / :601 — row index + 1
            double amount = nzd(r.Amount);
            String rowRemarks = r.Remarks == null ? "" : r.Remarks;

            /* :576 — with a cheque number and auto-remarks off, the narration is prefixed. */
            String comments = (head.ChequeNo != null && !head.ChequeNo.trim().isEmpty())
                    ? "CHEQUE NO: " + head.ChequeNo + ", " + rowRemarks
                    : rowRemarks;

            ContraVoucherDto.Detail debit = new ContraVoucherDto.Detail();
            debit.LineId                = lineId++;
            debit.SortNo                = sortNo;
            debit.AccountId             = nz(r.AccountId);
            debit.AgainstAccountId      = nz(dto.RefAccountId);
            debit.JobLotId              = nz(r.JobLotId);
            debit.Comments              = comments;
            debit.CommentsOtherLingo    = rowRemarks;
            debit.DebitAmount           = amount;
            debit.InvoiceNoRefId        = nz(dto.CheqId);
            debit.CheqNoDetail          = head.ChequeNo;
            debit.DCheqDate             = head.ChequeDate;
            debit.IsTaxable             = "False";
            debit.DMultiCurrencyId      = nz(dto.MultiCurrencyId);
            debit.DExchangeCurrencyRate = nzd(dto.ExchangeCurrencyRate);
            debit.DCurrencyAmount       = nzd(r.FcyAmount);
            debit.BranchesId            = branchFeature ? nz(r.BranchId) : branchId;
            debit.CostCenterId          = nz(r.CostCenterId);
            details.add(debit);
            voucherAmount += amount;

            ContraVoucherDto.Detail credit = new ContraVoucherDto.Detail();
            credit.LineId                = lineId++;
            credit.SortNo                = sortNo;              // deliberately the SAME SortNo
            credit.AccountId             = nz(dto.RefAccountId);
            credit.AgainstAccountId      = nz(r.AccountId);
            credit.JobLotId              = nz(r.JobLotId);
            /* :611 — with exactly one row and a non-empty header remarks, the credit line takes
               the header's text; otherwise it copies the debit line's. */
            credit.Comments              = (dto.rows.size() == 1 && !head.Remarks.isEmpty())
                                            ? head.Remarks : debit.Comments;
            credit.CommentsOtherLingo    = rowRemarks;
            credit.CreditAmount          = amount;
            credit.InvoiceNoRefId        = nz(dto.CheqId);
            credit.CheqNoDetail          = head.ChequeNo;
            credit.DCheqDate             = head.ChequeDate;
            credit.IsTaxable             = "False";
            credit.DMultiCurrencyId      = nz(dto.MultiCurrencyId);
            credit.DExchangeCurrencyRate = nzd(dto.ExchangeCurrencyRate);
            credit.DCurrencyAmount       = nzd(r.FcyAmount);
            credit.BranchesId            = branchFeature ? nz(r.BranchId) : branchId;
            /* :617 sets no CostCenterId on the credit line. */
            details.add(credit);

            /* :604 — the header's AgainstAccountId follows the last row processed. */
            head.AgainstAccountId = nz(r.AccountId);

            /* :630 — one row, empty header remarks: the header takes the detail's comments. */
            if (dto.rows.size() == 1 && head.Remarks.isEmpty()) {
                head.Remarks = debit.Comments;
            }

            /* §5 — a cost-centre line only for rows that name one. */
            if (nz(r.CostCenterId) > 0) {
                ContraVoucherDto.CostCentre c = new ContraVoucherDto.CostCentre();
                c.Id           = 0;
                c.SortNo       = sortNo;
                c.CostCenterId = nz(r.CostCenterId);
                c.costPrcent   = new BigDecimal("100");        // :673 — costPrcent = 100m
                c.costAmount   = amount;
                costCentres.add(c);
            }
        }

        head.VoucherAmount = voucherAmount;

        // ------------------------------------------------------- guards (§6 and §7 of the trace)

        negativeBalanceGuard(dto, head, voucherAmount, orgId, compId, yearId);
        duplicateGuard(dto, details, head, orgId, compId, voucherDate, insert ? 1 : 2);

        // ------------------------------------------------------------------------------ write

        int id = writer.save(head, details, costCentres);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("voucherCode", head.VoucherCode);
        res.put("voucherAmount", voucherAmount);
        res.put("detailLines", details.size());
        res.put("costCentreLines", costCentres.size());
        /* The desktop's own confirmation text (:692 / :696). */
        res.put("message", (insert ? "Voucher Save Successfully...[" : "Voucher Update Successfully...[")
                         + head.VoucherCode + "]");
        return res;
    }

    // =========================================================================== validation

    /** FormValidation():763 plus the two checks Insert() makes around it. */
    private void validate(ContraVoucherDto dto, boolean multiCurrency) {
        if (dto.rows == null || dto.rows.isEmpty()) {
            throw new IllegalArgumentException("Grid Record not found");
        }
        if (nz(dto.ProjectId) == 0) {
            throw new IllegalArgumentException("Cost Center Field is Required");
        }
        if (nz(dto.RefAccountId) == 0) {
            throw new IllegalArgumentException("Credit Account Field is Required");
        }
        if (multiCurrency) {
            if (nz(dto.MultiCurrencyId) == 0) {
                throw new IllegalArgumentException("Fcy Code Field is Required");
            }
            if (nzd(dto.ExchangeCurrencyRate) == 0d) {
                throw new IllegalArgumentException("Exchange Rate Field is Required");
            }
            if (nzd(dto.FcAmount) == 0d) {
                throw new IllegalArgumentException("Fcy Amount Field is Required");
            }
        } else {
            if (nz(dto.MultiCurrencyId) == 0) {
                throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            }
            if (nzd(dto.ExchangeCurrencyRate) == 0d) {
                throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
            }
        }
        /* :494 — checked after the confirm dialog on the desktop, before anything is built. */
        for (ContraVoucherDto.Row r : dto.rows) {
            if (nzd(r.Amount) > 0d && nz(r.AccountId) == 0) {
                throw new IllegalArgumentException("Please Select Account Title First");
            }
        }
        /* Add_Click_1():413 — the desktop refuses these at row-add time, so a row that reaches
           the server without them was not built by the screen. */
        for (ContraVoucherDto.Row r : dto.rows) {
            if (nz(r.AccountId) == 0)   throw new IllegalArgumentException("Debit Account Field Required");
            if (nz(r.JobLotId) == 0)    throw new IllegalArgumentException("Job/Lot Field Required");
            if (nzd(r.Amount) == 0d)    throw new IllegalArgumentException("Amount Field Required");
        }
    }

    // =============================================================================== guards

    /**
     * Insert():673. Three configuration switches decide whether an overdraw is refused, merely
     * warned about, or ignored. They are read from the company's configuration, never from the
     * request — a client-supplied "allow negative" flag would be an authorization hole.
     */
    private void negativeBalanceGuard(ContraVoucherDto dto, ContraVoucherDto.Head head,
                                      double voucherAmount, int orgId, int compId, int yearId) {
        if (configFlag(orgId, compId, "DisableBothNegativeBalanceRestrictions")) return;

        double closing = Math.round(writer.accountBalance(
                orgId, compId, yearId, head.VoucherDate, nz(dto.RefAccountId)));
        if (voucherAmount <= closing) return;

        String message = "Debit Amount cannot be greater than Account Balance.\n"
                + "Account Balance is " + trim(closing)
                + " and Total Debit Amount is " + trim(voucherAmount) + ".";

        if (configFlag(orgId, compId, "is Minus balance Allowed")) {
            /* PreventNegativeBalanceEntry — an outright refusal, not a question. */
            throw new IllegalArgumentException(message);
        }
        if (configFlag(orgId, compId, "DisplayWarningforNegativeBalance")
                && !Boolean.TRUE.equals(dto.negativeBalanceAcknowledged)) {
            throw new ConfirmationRequiredException(message + "\nDo you want to continue?",
                                                    "negativeBalance");
        }
    }

    /**
     * VoucherHead.VoucherExistWithSameAmountInSameDate — one call per distinct AccountId among
     * the DEBIT lines. On the desktop a hit is a Yes/No, and No abandons the save, so this raises
     * a confirmation rather than refusing.
     */
    private void duplicateGuard(ContraVoucherDto dto, List<ContraVoucherDto.Detail> details,
                                ContraVoucherDto.Head head, int orgId, int compId,
                                String voucherDate, int actionId) {
        if (Boolean.TRUE.equals(dto.duplicateAcknowledged)) return;

        Set<Integer> seen = new LinkedHashSet<>();
        for (ContraVoucherDto.Detail d : details) {
            if (nzd(d.DebitAmount) <= 0d) continue;
            if (!seen.add(nz(d.AccountId))) continue;          // group by AccountId, first only
            String title = writer.duplicateVoucherTitle(
                    orgId, compId, voucherDate, nz(d.AccountId), nzd(d.DebitAmount), actionId);
            if (title != null) {
                throw new ConfirmationRequiredException(
                        "Voucher against '" + title + "' with same Debit Amount already exists on "
                      + "this date. Do you want to continue?", "duplicate");
            }
        }
    }

    /** A desktop Yes/No that the operator has to answer before the save can proceed. */
    public static class ConfirmationRequiredException extends RuntimeException {
        public final String kind;
        public ConfirmationRequiredException(String message, String kind) {
            super(message);
            this.kind = kind;
        }
    }

    // ============================================================================== helpers

    /** CommonServices.GenerateVoucherCode(10). */
    public int nextVoucherCode() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return writer.nextVoucherCode(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID,
                currentUserContext.currentFinancialYearId(),
                u.getBranchesId() == null ? 0 : u.getBranchesId());
    }

    /**
     * {@code CommonServices.GetERPFeatureById(id)} tests whether the id appears in
     * {@code clsGlobalVariables.ErpFeaturesList}, a login-time cache of
     * {@code CompanyFeatures.GetERPFeaturesByCompanyId} →
     * {@code USP_GetERPFeaturesByCompanyId @OrganizationId @CompanyId}. Read here rather than
     * cached, from that same procedure — the pattern already used elsewhere in this port.
     *
     * A failure is reported and treated as off, which is what the desktop does with a missing
     * row: the feature simply is not in the list.
     */
    private boolean feature(int orgId, int compId, int featureId) {
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",
                    orgId, compId)) {
                Object id = ci(r, "Id");
                if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
            }
        } catch (Exception e) {
            LOG.warn("ERP feature {} could not be read; treating as off", featureId, e);
        }
        return false;
    }

    private boolean configFlag(int orgId, int compId, String name) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    SQL_CONFIG, orgId, compId, name,
                    "GetConfigurationByOrgCompandConfigDescription");
            return !rows.isEmpty() && toBool(ci(rows.get(0), "ConfigKey"));
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read; treating as off", name, e);
            return false;
        }
    }

    /**
     * CommonServices.SetRightsValueInRightsObject:17565, with the desktop's own Admin
     * short-circuit (:17582).
     */
    private boolean hasRight(String rightName) {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equals(role)) return true;
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS, currentUserContext.currentUserId(), SCREEN_NAME,
                    role == null ? "" : role, currentUserContext.currentCompanyId(), "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null && rightName.equals(String.valueOf(name).trim())) {
                    return toBool(ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, SCREEN_NAME, e);
        }
        return false;
    }

    private static int nz(Integer v)   { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }

    private static String trim(double d) {
        String s = String.format(Locale.ENGLISH, "%.2f", d);
        return s.endsWith(".00") ? s.substring(0, s.length() - 3) : s;
    }

    private static String isoDay(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number)  return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s) || "yes".equalsIgnoreCase(s);
    }
}
