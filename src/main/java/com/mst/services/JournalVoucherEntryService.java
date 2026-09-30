package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.DesktopVoucherDtos;
import com.mst.repositories.DesktopVoucherWriter;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Screen 19 "Jounal Voucher" - {@code Architecture.WinApp.Account_Definition.VoucherEntry},
 * DocumentTypeId 5, save and update. A port of VoucherEntry.Insert() (:1625) onto the desktop's
 * own procedure chain ({@link DesktopVoucherWriter}: Sp_VoucherHead_Insert|Update ->
 * Sp_VoucherDetail_Insert -> USP_VoucherBalanceCheck -> USP_voucherCostCenterDetail_Insert ->
 * the two _H_ mirrors -> [DAW].[USp_DocumentApprovalDetail_Insert], one transaction).
 *
 * THE DOUBLE ENTRY IS THE OPERATOR'S, NOT A MIRROR. Every "+" on this form (btnplus_Click :754)
 * adds TWO grid rows - a credit row (odd LineId n, AmountCr) and a debit row (LineId n+1, AmountDr),
 * each naming the other's account as AgainstAccountId - and Insert() writes ONE VoucherDetail per
 * grid row. There is no mirrorCreditToRefAccount-style extra line on this screen; VoucherHead has
 * no RefAccountId at all. The debit/credit direction is therefore taken row by row from AmountDr /
 * AmountCr (:1755-1756), and Insert() refuses a grid whose two totals differ (:1771).
 *
 * The desktop has no Delete for this screen (no Delete button on its toolstrip) - there is none here.
 */
@Service
public class JournalVoucherEntryService {

    private static final DesktopVoucherScreenService.Screen SCREEN = DesktopVoucherScreenService.Screen.VOUCHER_ENTRY;

    @Autowired private DesktopVoucherWriter writer;
    @Autowired private DesktopVoucherScreenService screens;
    @Autowired private CurrentUserContext ctx;

    @Transactional
    public Map<String, Object> save(DesktopVoucherDtos.JournalEntry dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.id == null ? 0 : dto.id;
        boolean insert = recId <= 0;

        /* DayBookVoucher_Load :1381/:1383 - Save.Enabled / btnUpdate.Enabled follow the rights; a
           disabled button is not a server check, so it is enforced here. */
        if (insert && !screens.hasRight(SCREEN, "save")) {
            throw new IllegalStateException("You don't have right");
        }
        if (!insert && !screens.hasRight(SCREEN, "update")) {
            throw new IllegalStateException("You don't have right");
        }
        Map<String, Object> existing = insert ? null : screens.requireOwnHead(SCREEN, recId);

        boolean multiCurrency = screens.feature(6);
        boolean branchFeature = screens.feature(17);
        List<DesktopVoucherDtos.JournalRow> table = dto.rows == null ? new ArrayList<>() : dto.rows;

        // ------------------------------------------------------------ Insert() :1647-1718, in order
        if (nz(dto.projectId) == 0) throw new IllegalArgumentException("Cost Center Field is Required");
        if (table.isEmpty()) throw new IllegalArgumentException("Grid Fields Required");
        if (multiCurrency) {
            if (nz(dto.multiCurrencyId) == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (nzd(dto.exchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Exchange Rate Field is Required");
            if (nzd(dto.fcAmount) == 0d) throw new IllegalArgumentException("Fcy Amount Field is Required");
        } else {
            if (nz(dto.multiCurrencyId) == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (nzd(dto.exchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }

        /* The grid shows the DataTable through DefaultView.Sort = "SortIndex ASC" (:770 / :1958) and
           Insert() walks grd.GetRows() - the SORTED order - so SortNo is the displayed position. */
        List<DesktopVoucherDtos.JournalRow> shown = new ArrayList<>(table);
        shown.sort(Comparator.comparingInt(r -> nz(r.sortIndex)));   // List.sort is stable

        for (DesktopVoucherDtos.JournalRow r : shown) {
            if (nzd(r.amountDr) > 0d && nz(r.accountId) == 0) throw new IllegalArgumentException("Please Select Debit Account Title First");
        }
        for (DesktopVoucherDtos.JournalRow r : shown) {
            if (nzd(r.amountCr) > 0d && nz(r.accountId) == 0) throw new IllegalArgumentException("Please Select Credit Account Title First");
        }

        // ------------------------------------------------------------------------ header :1719-1736
        ContraVoucherDto.Head head = new ContraVoucherDto.Head();
        head.Id = insert ? 0 : recId;
        head.DocumentTypeId = 5;
        head.DocumentTypeSrNo = 0;
        head.VoucherCode = nz(dto.voucherCode);
        head.ProjectId = nz(dto.projectId);
        head.VoucherDate = day(dto.voucherDate);
        head.RemarksOtherLingo = "";
        head.FinancialYearId = ctx.currentFinancialYearId();
        head.Remarks = dto.remarks == null ? "" : dto.remarks;          // Conversion.ToString(txt.Text), not trimmed
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        head.EntryDate = now;                                            // BLL Save :23-24 overwrites both
        head.ModifyDate = now;
        head.EntryUser = u.getId();
        head.ModifyUser = u.getId();
        head.OrganizationId = u.getOrganizationId();
        head.CompanyId = u.getCompanyId();
        head.BranchId = u.getBranchesId() == null ? 0 : u.getBranchesId();   // :1733 - this form DOES set it
        head.MultiCurrencyId = nz(dto.multiCurrencyId);
        head.ExchangeCurrencyRate = nzd(dto.exchangeCurrencyRate);
        head.FcAmount = nzd(dto.fcAmount);
        /* Never assigned by VoucherEntry.Insert(): RefAccountId, AgainstAccountId, ChequeNo,
           ChequeDate, CheqId, PayTitle, IncludeWHT - they keep the model defaults (0 / null / false),
           and the DTO's defaults are the same. */
        head.IncludeWHT = Boolean.FALSE;
        head.IsApproved = Boolean.FALSE;
        /* :1830-1831 - AttachmentsValues keeps what ReadById loaded; CustomAttachmentsValues is the
           loaded value on an update and "" on a new voucher. Attachments themselves are not ported. */
        head.AttachmentsValues = existing == null ? null : strOrNull(existing.get("AttachmentsValues"));
        head.CustomAttachmentsValues = existing == null ? "" : str(existing.get("CustomAttachmentsValues"));

        // ------------------------------------------------------------------------ detail :1738-1767
        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        int userBranch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        for (int i = 0; i < shown.size(); i++) {
            DesktopVoucherDtos.JournalRow r = shown.get(i);
            ContraVoucherDto.Detail vd = new ContraVoucherDto.Detail();
            vd.AccountId = nz(r.accountId);
            vd.AgainstAccountId = nz(r.againstAccountId);
            vd.SubsidiaryTypeId = nz(r.subsidiaryAccountTypeId);
            int sub = nz(r.subsidiaryAccountId);
            if (sub > 0) {
                vd.SupplierCustomerId = vd.SubsidiaryTypeId == 1 ? sub : 0;
                vd.EmployeeId = vd.SubsidiaryTypeId == 2 ? sub : 0;
                vd.SubsidiaryAccountId = sub;
            }
            vd.Comments = r.remarks == null ? "" : r.remarks;
            vd.CheqNoDetail = r.cheqNo == null ? "" : r.cheqNo;
            vd.DebitAmount = nzd(r.amountDr);
            vd.CreditAmount = nzd(r.amountCr);
            vd.JobLotId = nz(r.jobLotId);
            vd.LineId = nz(r.lineId);
            vd.SortNo = i + 1;                                             // r3.RowIndex + 1
            vd.DMultiCurrencyId = nz(dto.multiCurrencyId);
            vd.DExchangeCurrencyRate = nzd(dto.exchangeCurrencyRate);
            vd.DCurrencyAmount = nzd(r.fcyAmount);
            vd.ActionId = insert ? 1 : 2;                                  // :1763
            vd.BranchesId = branchFeature ? nz(r.branchId) : userBranch;
            vd.CostCenterId = nz(r.costCenterId);
            /* IsTaxable and CommentsOtherLingo are never set by this form: left null (not sent). */
            details.add(vd);
        }

        double totalDr = 0d, totalCr = 0d;
        for (DesktopVoucherDtos.JournalRow r : shown) { totalDr += nzd(r.amountDr); totalCr += nzd(r.amountCr); }
        if (totalCr != totalDr) throw new IllegalArgumentException("Debit & Credit side not equal");   // :1771

        /* :1776-1825 - cost centres come from the grid's DataSource in TABLE order (not the sorted
           view): every row with a CostCenterId gives one entry with its AmountDr, then every such row
           again with its AmountCr, SortNo = table index + 1, then List.Sort by SortNo. Reproduced
           as written, including the zero-amount entry each row contributes on its empty side. */
        List<ContraVoucherDto.CostCentre> costs = new ArrayList<>();
        for (int i = 0; i < table.size(); i++) {
            DesktopVoucherDtos.JournalRow r = table.get(i);
            if (nz(r.costCenterId) > 0) costs.add(cost(i + 1, nz(r.costCenterId), nzd(r.amountDr)));
        }
        for (int i = 0; i < table.size(); i++) {
            DesktopVoucherDtos.JournalRow r = table.get(i);
            if (nz(r.costCenterId) > 0) costs.add(cost(i + 1, nz(r.costCenterId), nzd(r.amountCr)));
        }
        costs.sort(Comparator.comparingInt(c -> c.SortNo));
        head.VoucherAmount = totalDr;                                      // :1826

        // ------------------------------------ VoucherHead.VoucherExistWithSameAmountInSameDate :1832
        if (!Boolean.TRUE.equals(dto.duplicateAcknowledged)) {
            Set<Integer> seen = new LinkedHashSet<>();
            for (ContraVoucherDto.Detail d : details) {
                if (nzd(d.DebitAmount) <= 0d || !seen.add(nz(d.AccountId))) continue;
                String title = writer.duplicateVoucherTitle(u.getOrganizationId(), u.getCompanyId(),
                        head.VoucherDate, nz(d.AccountId), nzd(d.DebitAmount), nz(d.ActionId));
                if (title != null) {
                    throw new ContraVoucherService.ConfirmationRequiredException(
                            "Voucher against '" + title + "' with same Debit Amount already exists on this date. Do you want to continue?",
                            "duplicate");
                }
            }
        }

        int id = writer.save(head, details, costs);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("voucherCode", head.VoucherCode);
        res.put("message", (insert ? "Record Save Successfully...[" : "Record Update Successfully...[") + head.VoucherCode + "]");
        return res;
    }

    private static ContraVoucherDto.CostCentre cost(int sortNo, int costCenterId, double amount) {
        ContraVoucherDto.CostCentre c = new ContraVoucherDto.CostCentre();
        c.Id = 0;
        c.SortNo = sortNo;
        c.CostCenterId = costCenterId;
        c.costPrcent = new BigDecimal("100");
        c.costAmount = amount;
        return c;
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }
    private static String strOrNull(Object v) { return v == null ? null : String.valueOf(v); }
    private static String day(String v) {
        if (v == null || v.trim().isEmpty()) return null;
        String s = v.trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
