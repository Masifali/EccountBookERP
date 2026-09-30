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
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

/**
 * Screen 855 "Contra Voucher New" - {@code Architecture.WinApp.Account_Definition.VouchersWithTax.ContraVoucher}
 * (ScreenName ContraVoucherTax), DocumentTypeId 10 with BaseDocumentTypeId 3. A port of its Insert()
 * (:650) onto the desktop's procedure chain ({@link DesktopVoucherWriter}).
 *
 * DOUBLE ENTRY (:747-903): every grid row writes a PAIR - vd debits the row's account
 * (AgainstAccountId = the Credit Account), vd2 credits the Credit Account (AgainstAccountId = the
 * row's account), both with SortNo = row index + 1; the header's AgainstAccountId ends as the last
 * row's account. Cheque details are per ROW and are used only when the Credit Account's
 * AccountTypeId is 15 (bank); the header takes the first row's cheque.
 *
 * NOT PORTED: the multi-currency breakup grid (grdMultiCurrency / VoucherDetailMultiCurrency and the
 * ThirdCurrency / BaseFcy columns) that the form uses when ERP feature 6 is on. The dump has no
 * feature 6 for any company (ERPFeatures holds ids 1-5, 11, 12, 20), so the desktop never takes that
 * branch here; a save with feature 6 on is refused rather than written half-way.
 */
@Service
public class ContraVoucherTaxService {

    private static final DesktopVoucherScreenService.Screen SCREEN = DesktopVoucherScreenService.Screen.CONTRA_TAX;

    @Autowired private DesktopVoucherWriter writer;
    @Autowired private DesktopVoucherScreenService screens;
    @Autowired private CurrentUserContext ctx;

    @Transactional
    public Map<String, Object> save(DesktopVoucherDtos.ContraTax dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.id == null ? 0 : dto.id;
        boolean insert = recId <= 0;
        if (insert && !screens.hasRight(SCREEN, "save")) throw new IllegalStateException("You don't have right");
        if (!insert && !screens.hasRight(SCREEN, "update")) throw new IllegalStateException("You don't have right");
        Map<String, Object> existing = insert ? null : screens.requireOwnHead(SCREEN, recId);

        boolean multiCurrencyGrid = screens.feature(6);          // MultiCurrencyGridWorking (:1122)
        boolean branchFeature = screens.feature(17);
        List<DesktopVoucherDtos.ContraTaxRow> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        int count = rows.size();

        /* dtcoalst (AccountsComboBind :1446) - titles and AccountTypeId of the credit/debit accounts. */
        Map<Integer, Map<String, Object>> coa = new HashMap<>();
        for (Map<String, Object> a : screens.contraAccounts()) coa.put(DesktopVoucherScreenService.toInt(a.get("Id")), a);
        Map<String, Object> credit = coa.get(nz(dto.refAccountId));
        String creditText = credit == null ? "" : DesktopVoucherScreenService.str(credit.get("AccountTitle")).trim();
        boolean creditIsBank = credit != null && DesktopVoucherScreenService.toInt(credit.get("AccountTypeId")) == 15;

        // ---------------------------------------------------------------- Insert() :664 + FormValidation :1030
        if (count == 0) throw new IllegalArgumentException("Grid Record not found");
        if (nz(dto.projectId) == 0) throw new IllegalArgumentException("Cost Center Field is Required");
        if (nz(dto.refAccountId) == 0) throw new IllegalArgumentException("Credit Account Field is Required");
        if (nz(dto.locationTypeId) == 0) throw new IllegalArgumentException("Location Type Field is Required");
        /* MultiCurrencyFeatureVisibilty is hard-set true on this form (:1304), so these three always apply. */
        if (nz(dto.multiCurrencyId) == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
        if (nzd(dto.exchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Exchange Rate Field is Required");
        if (nzd(dto.fcAmount) == 0d) throw new IllegalArgumentException("Fcy Amount Field is Required");
        if (multiCurrencyGrid) {
            throw new IllegalArgumentException("The multi-currency information grid of this screen (ERP feature 6) is not ported to the web; save this voucher on the desktop.");
        }
        for (DesktopVoucherDtos.ContraTaxRow r : rows) {
            if (nzd(r.amount) > 0d && nz(r.accountId) == 0) throw new IllegalArgumentException("Please Select Account Title First");
        }

        // ------------------------------------------------------------------------------ header :706-724
        ContraVoucherDto.Head head = new ContraVoucherDto.Head();
        head.Id = insert ? 0 : recId;
        head.BaseDocumentTypeId = 3;
        head.DocumentTypeId = 10;
        head.VoucherCode = nz(dto.voucherCode);
        head.ProjectId = nz(dto.projectId);
        head.VoucherDate = day(dto.voucherDate);
        head.RefAccountId = nz(dto.refAccountId);
        head.AgainstAccountId = nz(dto.refAccountId);
        head.Remarks = dto.remarks == null ? "" : dto.remarks.trim();
        head.OrganizationId = u.getOrganizationId();
        head.CompanyId = u.getCompanyId();
        head.FinancialYearId = ctx.currentFinancialYearId();
        head.EntryUser = u.getId();
        head.ModifyUser = u.getId();
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        head.EntryDate = now;
        head.ModifyDate = now;
        head.IncludeWHT = Boolean.FALSE;
        head.IsApproved = Boolean.FALSE;
        head.MultiCurrencyId = nz(dto.multiCurrencyId);
        head.ExchangeCurrencyRate = nzd(dto.exchangeCurrencyRate);
        head.FcAmount = nzd(dto.fcAmount);
        head.CustomAccounts = Boolean.TRUE.equals(dto.customAccounts);
        head.BranchId = 0;                                        // never assigned by this Insert()
        /* ChequeNo / CheqId / ChequeDate / PayTitle stay unset unless a bank row fills them below. */
        head.ChequeNo = null;
        head.CheqId = 0;
        /* :947-952 */
        String att = existing == null ? "" : DesktopVoucherScreenService.str(existing.get("AttachmentsValues"));
        head.AttachmentsValues = att;
        head.CustomAttachmentsValues = existing == null ? "" : DesktopVoucherScreenService.str(existing.get("CustomAttachmentsValues"));

        boolean autoRemarks = screens.configBool("AutoRemarksForPaymentThroughBank");
        boolean autoHeader = screens.configBool("AutoRemarksIncludeHeaderRemarks");
        boolean autoChqDateNum = screens.configBool("AutoRemarksIncludeChequeDateAndNumber");
        boolean autoChqNum = screens.configBool("AutoRemarksIncludeChequeNumber");
        boolean autoPayee = screens.configBool("AutoRemarksIncludePayeeTitle");
        boolean autoDetail = screens.configBool("AutoRemarksIncludeDetailRemarks");

        // ------------------------------------------------------------------------------ detail :728-904
        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        int userBranch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        String acTitle = "";
        int lineId = 1;
        double voucherAmt = 0d;
        for (int i = 0; i < count; i++) {
            DesktopVoucherDtos.ContraTaxRow r = rows.get(i);
            String rowRemarks = r.remarks == null ? "" : r.remarks;
            String remarksAuto = "";
            ContraVoucherDto.Detail vd = new ContraVoucherDto.Detail();
            if (autoRemarks && coa.containsKey(nz(r.accountId))) {
                acTitle = DesktopVoucherScreenService.str(coa.get(nz(r.accountId)).get("AccountTitle"));
            }
            vd.LineId = lineId;
            vd.SortNo = i + 1;
            vd.AccountId = nz(r.accountId);
            vd.AgainstAccountId = nz(dto.refAccountId);
            vd.JobLotId = nz(r.jobLotId);
            if (creditIsBank) {                                   // :752
                vd.InvoiceNoRefId = nz(r.chequeId);
                vd.DCheqDate = day(r.chequeDate);
                vd.CheqNoDetail = r.chequeNo == null ? "" : r.chequeNo;
                vd.PayeeTitle = r.payTitle == null ? "" : r.payTitle;
                if (head.ChequeNo == null || head.ChequeNo.trim().isEmpty() || nz(head.CheqId) == 0) {
                    head.CheqId = nz(r.chequeId);
                    head.ChequeDate = day(r.chequeDate);
                    head.ChequeNo = r.chequeNo == null ? "" : r.chequeNo;
                    head.PayTitle = r.payTitle == null ? "" : r.payTitle;
                }
            }
            if (autoRemarks) {                                    // :766-789
                String cheqNo = vd.CheqNoDetail == null ? "" : vd.CheqNoDetail;
                String payee = vd.PayeeTitle == null ? "" : vd.PayeeTitle;
                String cheqDate = shortDate(vd.DCheqDate);
                String tail = " Credit Account: " + creditText + " TRANSFER TO: " + acTitle;
                if (autoChqDateNum && autoPayee) remarksAuto = " CHEQUE DATE: " + cheqDate + " CHEQUE NO: " + cheqNo + " PayTitle: " + payee + tail;
                else if (autoChqDateNum) remarksAuto = " CHEQUE DATE: " + cheqDate + " CHEQUE NO: " + cheqNo + tail;
                else if (autoChqNum && autoPayee) remarksAuto = " CHEQUE NO: " + cheqNo + " PayTitle: " + payee + tail;
                else if (!autoPayee) remarksAuto = tail;
                else remarksAuto = " PayTitle: " + payee + tail;
                String hr = head.Remarks == null ? "" : head.Remarks;
                boolean contains = hr.contains("CHEQUE DATE") || hr.contains("CHEQUE NO") || hr.contains("PayTitle") || hr.contains("Credit Account");
                if (autoHeader && autoDetail) remarksAuto = (contains ? "" : hr) + " " + remarksAuto + " " + rowRemarks;
                else if (autoHeader) remarksAuto = (contains ? "" : hr) + " " + remarksAuto;
                else if (autoDetail) remarksAuto = remarksAuto + " " + rowRemarks;
                vd.Comments = remarksAuto;
                vd.CommentsOtherLingo = rowRemarks;
                if (count == 1) {
                    head.RemarksOtherLingo = !"".equals(hr) ? hr : null;
                    head.Remarks = remarksAuto;
                }
            } else if (!"".equals(vd.CheqNoDetail)) {
                /* :790 - vd.CheqNoDetail is null for a non-bank credit account, and null != "" is
                   true, so the desktop writes "CHEQUE NO: , <remarks>" there. Reproduced. */
                vd.Comments = "CHEQUE NO: " + (vd.CheqNoDetail == null ? "" : vd.CheqNoDetail) + ", " + rowRemarks;
                vd.CommentsOtherLingo = rowRemarks;
            } else {
                vd.Comments = rowRemarks;
                vd.CommentsOtherLingo = rowRemarks;
            }
            vd.DebitAmount = nzd(r.amount);
            if (vd.DebitAmount == 0d) throw new IllegalArgumentException("Amount is Required in Row# " + (i + 1));
            voucherAmt += vd.DebitAmount;
            vd.IsTaxable = "False";
            vd.DMultiCurrencyId = nz(r.tcyCodeId);
            vd.DExchangeCurrencyRate = nzd(r.tcyExchangeRate);
            vd.DCurrencyAmount = nzd(r.fcyAmount);
            vd.BranchesId = branchFeature ? nz(r.branchId) : userBranch;
            vd.CostCenterId = nz(r.costCenterId);
            vd.ReferenceAccountId = nz(r.referenceAccountId);
            vd.LocationTypeId = nz(dto.locationTypeId);
            details.add(vd);
            lineId++;

            ContraVoucherDto.Detail vd2 = new ContraVoucherDto.Detail();
            vd2.LineId = lineId;
            vd2.SortNo = i + 1;
            vd2.AccountId = nz(dto.refAccountId);
            vd2.AgainstAccountId = nz(r.accountId);
            head.AgainstAccountId = vd2.AgainstAccountId;         // :850
            vd2.JobLotId = nz(r.jobLotId);
            if (autoRemarks) {
                vd2.Comments = remarksAuto;
                vd2.CommentsOtherLingo = rowRemarks;
            } else if (count == 1 && !head.Remarks.trim().isEmpty()) {
                vd2.Comments = head.Remarks.trim();
                vd2.CommentsOtherLingo = vd.CommentsOtherLingo;
            } else {
                vd2.Comments = vd.Comments;
                vd2.CommentsOtherLingo = rowRemarks;
            }
            vd2.CreditAmount = nzd(r.amount);
            vd2.IsTaxable = "False";
            vd2.DMultiCurrencyId = nz(r.tcyCodeId);
            vd2.DExchangeCurrencyRate = nzd(r.tcyExchangeRate);
            vd2.DCurrencyAmount = nzd(r.fcyAmount);
            vd2.BranchesId = branchFeature ? nz(r.branchId) : userBranch;
            vd2.ReferenceAccountId = nz(r.referenceAccountId);
            if (count == 1 && "".equals(head.Remarks)) head.Remarks = vd2.Comments;
            vd2.LocationTypeId = nz(dto.locationTypeId);
            details.add(vd2);
            lineId++;
        }

        /* :905-930 - one 100% cost-centre line per row that names one, SortNo = DataTable index + 1. */
        List<ContraVoucherDto.CostCentre> costs = new ArrayList<>();
        for (int i = 0; i < count; i++) {
            DesktopVoucherDtos.ContraTaxRow r = rows.get(i);
            if (nz(r.costCenterId) > 0) {
                ContraVoucherDto.CostCentre c = new ContraVoucherDto.CostCentre();
                c.Id = 0;
                c.SortNo = i + 1;
                c.CostCenterId = nz(r.costCenterId);
                c.costPrcent = new BigDecimal("100");
                c.costAmount = nzd(r.amount);
                costs.add(c);
            }
        }

        // ---------------------------------------------------------------- negative balance :953-969
        if (!screens.configBool("DisableBothNegativeBalanceRestrictions")) {
            double closing = Math.rint(screens.accountBalance(head.RefAccountId, head.VoucherDate));
            if (voucherAmt > closing) {
                String msg = "Debit Amount cannot be greater than Account Balance.\nAccount '" + creditText + "' Balance is "
                        + ExpenseVoucherPlainService.net(closing) + " and Total Debit Amount is " + ExpenseVoucherPlainService.net(voucherAmt) + ".";
                if (screens.configBool("is Minus balance Allowed")) throw new IllegalArgumentException(msg);
                if (screens.configBool("DisplayWarningforNegativeBalance") && !Boolean.TRUE.equals(dto.negativeBalanceAcknowledged)) {
                    throw new ContraVoucherService.ConfirmationRequiredException(msg, "negativeBalance");
                }
            }
        }

        /* :970 - vd.ActionId is never set, so @ActionId = 0 and the procedure never returns a row. */
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

        head.VoucherAmount = voucherAmt;
        int id = writer.save(head, details, costs);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("voucherCode", head.VoucherCode);
        res.put("message", (insert ? "Voucher Save Successfully...[" : "Voucher Update Successfully...[") + head.VoucherCode + "]");
        return res;
    }

    /** ToShortDateString() under the desktop's dd-MMM-yy pattern; a missing date is DateTime.MinValue. */
    private static String shortDate(String isoDay) {
        if (isoDay == null || isoDay.length() < 10) return "01-Jan-01";
        try {
            return java.time.LocalDate.parse(isoDay.substring(0, 10))
                    .format(java.time.format.DateTimeFormatter.ofPattern("dd-MMM-yy", Locale.ENGLISH));
        } catch (Exception e) {
            return isoDay;
        }
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }
    private static String day(String v) {
        if (v == null || v.trim().isEmpty()) return null;
        String s = v.trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
