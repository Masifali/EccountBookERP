package com.mst.services;

import com.mst.models.ConfigrationsAllocation;
import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.JournalNoOffsetVoucherDto;
import com.mst.repositories.DesktopVoucherWriter;
import com.mst.repositories.support.DesktopProc;
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
import java.util.Comparator;
import java.util.Date;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Screen 914 JournalVoucher_New - "Journal Voucher (without Offset)".
 *
 * Desktop: Architecture.WinApp.Account_Definition.VouchersWithTax.JournalVoucher_New (DocumentTypeId 17,
 * base.Name "JournalVoucher_New" - the ScreenName the rights are read under). One grid row = one ledger line
 * (a Dr OR a Cr amount, no offset line is generated); the totals must agree within 1.0.
 *
 * Persistence is the BLL VoucherHead.Save chain (DesktopVoucherWriter), exactly the call the desktop's Insert()
 * makes. The ledger lines, the cost-centre rows and VoucherAmount are derived here from the posted rows.
 */
@Service
public class JournalNoOffsetVoucherService {

    private static final Logger LOG = LoggerFactory.getLogger(JournalNoOffsetVoucherService.class);
    public static final int DOCUMENT_TYPE_ID = 17;
    public static final String SCREEN_NAME = "JournalVoucher_New";

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?";

    @Autowired private DesktopVoucherWriter writer;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private VoucherDesktopConfigService config;
    @Autowired private DesktopVoucherScreenService screens;

    /** A desktop Yes/No the operator has to answer before the save can proceed. */
    public static class DuplicateConfirmation extends ContraVoucherService.ConfirmationRequiredException {
        public final int accountId;
        public DuplicateConfirmation(String message, int accountId) {
            super(message, "duplicate");
            this.accountId = accountId;
        }
    }

    // ================================================================================ lookups

    /** Everything DayBookVoucher_Load binds, each from the desktop's own call. */
    public Map<String, Object> lookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("accounts", accounts());                         // DebitAccountTitleFill
        r.put("projects", screens.projects());                 // CombProjectFill
        r.put("locationTypes", screens.locationTypes());       // LocationType
        r.put("currencies", screens.currencies());             // CurrencyFill - MultiCurrency.GetAll
        r.put("jobLots", screens.jobLots());                   // combojoblotfill
        Map<String, Object> flags = flags();
        r.put("flags", flags);
        r.put("branches", Boolean.TRUE.equals(flags.get("branchFeature")) ? screens.branches() : new ArrayList<>());
        r.put("costCenters", Boolean.TRUE.equals(flags.get("isBookingOffice")) ? screens.costCenters() : new ArrayList<>());
        r.put("subsidiaries", Boolean.TRUE.equals(flags.get("subsidiaryFeature")) ? subsidiaries() : new ArrayList<>());
        r.put("nextCode", nextCode());
        return r;
    }

    /**
     * DebitAccountTitleFill: CommonServices.Accounts_GetAccountTitleByAccountTypeIds() with no type list ->
     * [dbo].[USP_Accounts_GetAccountTitleByAccountTypeIds] @OrganizationId @CompanyId @AppId (@UserId when non-zero).
     */
    public List<Map<String, Object>> accounts() {
        int user = currentUserContext.currentUserId();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : DesktopProc.rows(jdbcTemplate, "[dbo].[USP_Accounts_GetAccountTitleByAccountTypeIds]",
                DesktopProc.params("OrganizationId", currentUserContext.currentOrganizationId(),
                        "CompanyId", currentUserContext.currentCompanyId(), "AppId", screens.appId(),
                        "UserId", user != 0 ? user : null))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(row, "Id"));
            o.put("AccountTitle", ci(row, "AccountTitle"));
            o.put("AccountCode", ci(row, "AccountCode"));
            o.put("ParentAccountTitle", ci(row, "ParentAccountTitle"));
            o.put("AccountClass", ci(row, "AccountClass"));
            o.put("AccountTypeId", ci(row, "AccountTypeId"));
            out.add(o);
        }
        return out;
    }

    /**
     * BindAllSubsidiaryaccounts: VoucherHead.BindSubsidiaryAccount(CompanyId, 0, null) - every row. The desktop reads
     * the cells by position (Cells[2] = type, Cells[4] = the parent GL account); both are exposed by name here.
     */
    public List<Map<String, Object>> subsidiaries() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : screens.subsidiaryAccounts(0, null)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(row, "Id"));
            o.put("SubsidiaryAccount", ci(row, "SubsidiaryAccount"));
            Object type = ci(row, "SubsidiaryTypeId");
            o.put("SubsidiaryTypeId", type != null ? type : pos(row, 2));
            Object parent = ci(row, "AccountId");
            o.put("AccountId", parent != null ? parent : pos(row, 4));
            out.add(o);
        }
        return out;
    }

    /** The Load-time switches plus the defaults DefaultConfigurations() applies. */
    public Map<String, Object> flags() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> f = new LinkedHashMap<>();
        Map<String, ConfigrationsAllocation> m = config.configMap();
        f.put("defaultJobLotId", toInt(VoucherDesktopConfigService.key(m, "Job/Lot")));
        f.put("baseCurrencyId", toInt(VoucherDesktopConfigService.key(m, "Base Currency")));
        String rate = VoucherDesktopConfigService.key(m, "BaseCurrencyRate");
        f.put("baseCurrencyRate", rate == null ? null : VoucherDesktopConfigService.toDouble(rate));
        f.put("amountDecimals", toInt(VoucherDesktopConfigService.key(m, "Default NoofDecimal Points For Amount")));
        f.put("rateDecimals", toInt(VoucherDesktopConfigService.key(m, "Default NoofDecimal Points For Rate")));
        f.put("defaultBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        f.put("multiCurrencyFeature", config.erpFeature(6));      // MultiCurrencyFeatureVisibilty
        f.put("subsidiaryFeature", config.erpFeature(4));         // SubsidiaryAccountAllowOnVouchers
        f.put("branchFeature", config.erpFeature(17));            // BranchFeature
        f.put("isBookingOffice", screens.isBookingOffice());
        f.put("screenName", SCREEN_NAME);
        f.put("documentTypeId", DOCUMENT_TYPE_ID);
        f.put("rights", rights());
        return f;
    }

    /** CommonServices.GenerateVoucherCode(17). */
    public int nextCode() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return writer.nextVoucherCode(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID,
                currentUserContext.currentFinancialYearId(), u.getBranchesId() == null ? 0 : u.getBranchesId());
    }

    /** CmbDr_Leave: ReadByCurrentBalanceByDateAndAccountId, Balance column. */
    public double balance(int accountId, String date) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return writer.accountBalance(u.getOrganizationId(), u.getCompanyId(),
                currentUserContext.currentFinancialYearId(), isoDay(date), accountId);
    }

    /** CmbDrSubsidiaryAccount_ValueChanged: GetSubsiadiaryBalance. */
    public double subsidiaryBalance(int glAccountId, int subsidiaryId, String date) {
        return screens.subsidiaryBalance(glAccountId, subsidiaryId, isoDay(date));
    }

    /** cmbCurrency_Leave: GetLastExchangeRateAndCurrencyOfVoucher with DocumentTypeIds "17"; no row -> 0. */
    public double lastRate(int currencyId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbcTemplate, "Sp_Vouchers_GetMethods",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "DocumentTypeIds", String.valueOf(DOCUMENT_TYPE_ID),
                        "DMultiCurrencyIds", currencyId != 0 ? String.valueOf(currencyId) : null,
                        "Activity", "GetMultiCurrencyAndLastRate"));
        return rows.isEmpty() ? 0d : toDouble(ci(rows.get(0), "LastExchRate"));
    }

    // ================================================================================ history

    /**
     * HistoryGridFill -> VoucherHead.VoucherFormHistory -> USP_VoucherFormHistory with DocumentTypeName "17",
     * CanViewAllRecord = the screen's right, EntryUser only when that right is off, IsApproved unless "All".
     * This form passes neither AppId nor an account.
     */
    public Map<String, Object> history(String dateType, String fromDate, String toDate,
                                       Integer fromDocNo, Integer toDocNo, String approvedStatus) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean viewAll = hasRight("CanView AllRecord");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_VoucherFormHistory @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeName=?, @CanViewAllRecord=?");
        List<Object> a = new ArrayList<>();
        a.add(u.getOrganizationId()); a.add(u.getCompanyId()); a.add(u.getId()); a.add(String.valueOf(DOCUMENT_TYPE_ID)); a.add(viewAll);
        int year = currentUserContext.currentFinancialYearId();
        if (year != 0) { sql.append(", @FinancialYearId=?"); a.add(year); }
        String from = isoDay(fromDate), to = isoDay(toDate);
        String fp, tp;
        if ("entrydate".equalsIgnoreCase(dateType)) { fp = "@EntryFromDate"; tp = "@EntryToDate"; }
        else if ("modifydate".equalsIgnoreCase(dateType)) { fp = "@ModifyFromDate"; tp = "@ModifyToDate"; }
        else if ("approveddate".equalsIgnoreCase(dateType)) { fp = "@ApprovedFromDate"; tp = "@ApprovedToDate"; }
        else { fp = "@FromDate"; tp = "@ToDate"; }
        if (from != null) { sql.append(", ").append(fp).append("=?"); a.add(from); }
        if (to != null) { sql.append(", ").append(tp).append("=?"); a.add(to); }
        if (nz(fromDocNo) != 0) { sql.append(", @DocNoFrom=?"); a.add(fromDocNo); }
        if (nz(toDocNo) != 0) { sql.append(", @DocNoTo=?"); a.add(toDocNo); }
        if (!viewAll) { sql.append(", @EntryUser=?"); a.add(u.getId()); }
        if (!"all".equalsIgnoreCase(approvedStatus)) { sql.append(", @IsApproved=?"); a.add("approved".equalsIgnoreCase(approvedStatus)); }

        List<Map<String, Object>> raw = jdbcTemplate.queryForList(sql.toString(), a.toArray());
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", ci(r, "Id"));
            o.put("documentTypeId", ci(r, "DocumentTypeId"));
            o.put("voucherDate", dateStr(ci(r, "VoucherDate")));
            o.put("voucherCode", ci(r, "VoucherCode"));
            o.put("documentType", ci(r, "DocumentTypeCode"));
            o.put("voucherAmount", ci(r, "VoucherAmount"));
            o.put("fcyCode", ci(r, "Currencycode"));
            o.put("exchangeRate", ci(r, "ExchangeCurrencyRate"));
            o.put("fcyAmount", ci(r, "FcAmount"));
            o.put("remarks", ci(r, "Remarks"));
            o.put("entryUser", ci(r, "UserName"));
            o.put("entryDate", dateTimeStr(ci(r, "EntryDate")));
            o.put("modifyUser", ci(r, "ModifyUserName"));
            o.put("modifyDate", dateTimeStr(ci(r, "ModifyDate")));
            o.put("approvedUser", ci(r, "ApprovedUserName"));
            o.put("approvedDate", dateTimeStr(ci(r, "PostDate")));
            o.put("attachment", ci(r, "NoOfAttachments"));
            rows.add(o);
        }
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("rows", rows);
        res.put("totalVouchers", raw.isEmpty() ? 0 : toInt(ci(raw.get(0), "TotalVouchers")));
        res.put("totalApprovedVoucher", raw.isEmpty() ? 0 : toInt(ci(raw.get(0), "TotalApprovedVoucher")));
        res.put("totalUnApprovedVoucher", raw.isEmpty() ? 0 : toInt(ci(raw.get(0), "TotalUnApprovedVoucher")));
        res.put("multiCurrencyFeature", config.erpFeature(6));
        return res;
    }

    // ================================================================================ load

    /** The head row when it is this tenant's document type 17 voucher, otherwise null. */
    private Map<String, Object> ownHead(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> h = screens.readHead(id);
        if (h == null) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId() || toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        return h;
    }

    /**
     * ReadById(): the head fields and one grid row per stored line (ReadById table: FcyDr = DCurrencyAmount when
     * debit > 0, FcyCr when credit > 0, branch columns only with the Branch feature).
     */
    public Map<String, Object> load(int id) {
        Map<String, Object> h = ownHead(id);
        if (h == null) return null;
        boolean branchFeature = config.erpFeature(17);
        List<Map<String, Object>> details = screens.readDetails(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        out.put("voucherCode", ci(h, "VoucherCode"));
        out.put("voucherDate", dateStr(ci(h, "VoucherDate")));
        out.put("projectId", ci(h, "ProjectId"));
        out.put("chequeNo", ci(h, "ChequeNo"));
        out.put("remarks", str(ci(h, "Remarks")));
        out.put("multiCurrencyId", ci(h, "MultiCurrencyId"));
        out.put("exchangeCurrencyRate", ci(h, "ExchangeCurrencyRate"));
        out.put("fcAmount", ci(h, "FcAmount"));
        out.put("voucherAmount", ci(h, "VoucherAmount"));
        out.put("isApproved", truthy(ci(h, "IsApproved")));
        out.put("customAccounts", truthy(ci(h, "CustomAccounts")));
        out.put("locationTypeId", details.isEmpty() ? 0 : toInt(ci(details.get(0), "LocationTypeId")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details) {
            Map<String, Object> r = new LinkedHashMap<>();
            double dr = toDouble(ci(d, "DebitAmount")), cr = toDouble(ci(d, "CreditAmount"));
            r.put("accountCode", ci(d, "AccountCode"));
            r.put("accountId", ci(d, "AccountId"));
            r.put("accountTitle", ci(d, "AccountTitle"));
            r.put("subsidiaryAccountId", ci(d, "SubsidiaryAccountId"));
            r.put("subsidiaryAccount", ci(d, "SubsidiaryAccountTitle"));
            r.put("subsidiaryAccountTypeId", ci(d, "SubsidiaryTypeId"));
            r.put("remarks", ci(d, "Comments"));
            r.put("jobLotId", ci(d, "JobLotId"));
            r.put("jobLot", ci(d, "JobLotDescription"));
            r.put("cheqNo", ci(d, "CheqNoDetail"));
            r.put("fcyAmountDr", dr > 0 ? toDouble(ci(d, "DCurrencyAmount")) : 0d);
            r.put("amountDr", dr);
            r.put("fcyAmountCr", cr > 0 ? toDouble(ci(d, "DCurrencyAmount")) : 0d);
            r.put("amountCr", cr);
            r.put("branchId", branchFeature ? ci(d, "BranchesId") : 0);
            r.put("branchName", branchFeature ? ci(d, "BranchName") : "");
            r.put("costCenterId", ci(d, "CostCenterId"));
            r.put("costCenter", ci(d, "CostCenterName"));
            rows.add(r);
        }
        out.put("rows", rows);
        return out;
    }

    /** DataGridHistory_SelectionChanged: the detail grid, ordered by SubNo. */
    public Map<String, Object> lines(int id) {
        Map<String, Object> h = ownHead(id);
        if (h == null) return null;
        List<Map<String, Object>> list = new ArrayList<>(screens.readDetails(id));
        list.sort(Comparator.comparingInt(d -> toInt(ci(d, "SubNo"))));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : list) {
            double dr = toDouble(ci(d, "DebitAmount")), cr = toDouble(ci(d, "CreditAmount"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("accountCode", ci(d, "AccountCode"));
            o.put("accountId", ci(d, "AccountId"));
            o.put("accountTitle", ci(d, "AccountTitle"));
            o.put("subsidiaryAccount", ci(d, "SubsidiaryAccountTitle"));
            o.put("jobLot", ci(d, "JobLotDescription"));
            o.put("remarks", ci(d, "Comments"));
            o.put("cheqNo", ci(d, "CheqNoDetail"));
            o.put("debitFcyAmount", dr > 0 ? toDouble(ci(d, "DCurrencyAmount")) : 0d);
            o.put("amountDr", dr);
            o.put("creditFcyAmount", cr > 0 ? toDouble(ci(d, "DCurrencyAmount")) : 0d);
            o.put("amountCr", cr);
            o.put("branchName", ci(d, "BranchName"));
            o.put("costCenter", ci(d, "CostCenterName"));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ save

    @Transactional
    public Map<String, Object> save(JournalNoOffsetVoucherDto dto) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean insert = dto.Id == null || dto.Id <= 0;
        if (insert && !hasRight("Save")) throw new SecurityException("You don't have Save right on this screen.");
        if (!insert && !hasRight("Update")) throw new SecurityException("You don't have Update right on this screen.");
        Map<String, Object> existing = null;
        if (!insert) {
            existing = ownHead(dto.Id);
            if (existing == null) throw new IllegalArgumentException("Record Not Update  " + dto.Id);
        }
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        int userBranch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        Map<String, Object> f = flags();
        boolean multiCurrency = Boolean.TRUE.equals(f.get("multiCurrencyFeature"));
        boolean subsidiaryFeature = Boolean.TRUE.equals(f.get("subsidiaryFeature"));
        boolean branchFeature = Boolean.TRUE.equals(f.get("branchFeature"));

        // ---- Insert() preamble
        if (nz(dto.ProjectId) == 0) throw new IllegalArgumentException("Cost Center Field is Required");
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid Fields Required");
        if (isoDay(dto.VoucherDate) == null) throw new IllegalArgumentException("Voucher Date Field is Required");
        if (multiCurrency) {
            if (nz(dto.MultiCurrencyId) == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (nzd(dto.ExchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Exchange Rate Field is Required");
            if (nzd(dto.FcAmount) == 0d) throw new IllegalArgumentException("Fcy Amount Field is Required");
        } else {
            if (nz(dto.MultiCurrencyId) == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (nzd(dto.ExchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }
        validateRows(dto, subsidiaryFeature, branchFeature);

        List<JournalNoOffsetVoucherDto.Row> rows = dto.rows;
        for (int i = 0; i < rows.size(); i++) {
            if (nzd(rows.get(i).AmountDr) > 0d && nz(rows.get(i).AccountId) == 0) {
                throw new IllegalArgumentException("Please Select Account Title First in Row#" + (i + 1));
            }
        }
        for (int i = 0; i < rows.size(); i++) {
            if (nzd(rows.get(i).AmountCr) > 0d && nz(rows.get(i).AccountId) == 0) {
                throw new IllegalArgumentException("Please Select Account Title First in Row#" + (i + 1));
            }
        }

        // ---- head
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        int voucherCode = nz(dto.VoucherCode);
        if (insert && voucherCode <= 0) voucherCode = nextCode();
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.Id = insert ? 0 : dto.Id;
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = 0;
        vh.VoucherCode = voucherCode;
        vh.ProjectId = nz(dto.ProjectId);
        vh.VoucherDate = isoDay(dto.VoucherDate);
        vh.RemarksOtherLingo = "";
        vh.FinancialYearId = currentUserContext.currentFinancialYearId();
        vh.ChequeNo = nzs(dto.ChequeNo).trim();
        vh.Remarks = nzs(dto.Remarks);                          // Conversion.ToString(txtVoucherremarks.Text), not trimmed
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.EntryUser = u.getId();
        vh.ModifyUser = u.getId();
        vh.OrganizationId = org;
        vh.CompanyId = comp;
        vh.BranchId = userBranch;
        vh.MultiCurrencyId = nz(dto.MultiCurrencyId);
        vh.ExchangeCurrencyRate = nzd(dto.ExchangeCurrencyRate);
        vh.FcAmount = nzd(dto.FcAmount);
        vh.CustomAccounts = Boolean.TRUE.equals(dto.CustomAccounts);
        if (existing != null) {
            Object av = ci(existing, "AttachmentsValues");
            vh.AttachmentsValues = av == null ? null : String.valueOf(av);
            vh.CustomAttachmentsValues = str(ci(existing, "CustomAttachmentsValues"));
        } else {
            vh.CustomAttachmentsValues = "";
        }

        // ---- detail: one ledger line per grid row, in grid order
        int locationId = nz(dto.LocationTypeId);
        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        int lineId = 1;
        double totalDr = 0d, totalCr = 0d;
        for (int i = 0; i < rows.size(); i++) {
            JournalNoOffsetVoucherDto.Row r = rows.get(i);
            ContraVoucherDto.Detail vd = new ContraVoucherDto.Detail();
            vd.AccountId = nz(r.AccountId);
            vd.AgainstAccountId = vd.AccountId;
            vd.SubsidiaryTypeId = nz(r.SubsidiaryAccountTypeId);
            int sub = nz(r.SubsidiaryAccountId);
            if (sub > 0) {
                vd.SupplierCustomerId = vd.SubsidiaryTypeId == 1 ? sub : 0;
                vd.EmployeeId = vd.SubsidiaryTypeId == 2 ? sub : 0;
                vd.SubsidiaryAccountId = sub;
            }
            vd.Comments = nzs(r.Remarks);
            vd.CheqNoDetail = nzs(r.CheqNo);
            vd.DebitAmount = nzd(r.AmountDr);
            vd.CreditAmount = nzd(r.AmountCr);
            vd.JobLotId = nz(r.JobLotId);
            vd.LineId = lineId++;
            vd.SortNo = i + 1;
            vd.DMultiCurrencyId = nz(dto.MultiCurrencyId);
            vd.DExchangeCurrencyRate = nzd(dto.ExchangeCurrencyRate);
            vd.DCurrencyAmount = vd.CreditAmount > 0d ? nzd(r.FcyAmountCr) : nzd(r.FcyAmountDr);
            vd.ActionId = insert ? 1 : 2;
            vd.BranchesId = branchFeature ? nz(r.BranchId) : userBranch;
            vd.CostCenterId = nz(r.CostCenterId);
            vd.LocationTypeId = locationId;
            totalDr += vd.DebitAmount;
            totalCr += vd.CreditAmount;
            details.add(vd);
        }
        if (Math.abs(totalDr - totalCr) >= 1.0d) throw new IllegalArgumentException("Debit & Credit side not equal");

        // ---- cost centres: every row with a cost centre, first its Dr amount, then its Cr amount, stable by SortNo
        List<ContraVoucherDto.CostCentre> costs = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) {
            if (nz(rows.get(i).CostCenterId) > 0) costs.add(cost(i + 1, rows.get(i).CostCenterId, nzd(rows.get(i).AmountDr)));
        }
        for (int i = 0; i < rows.size(); i++) {
            if (nz(rows.get(i).CostCenterId) > 0) costs.add(cost(i + 1, rows.get(i).CostCenterId, nzd(rows.get(i).AmountCr)));
        }
        costs.sort(Comparator.comparingInt(c -> c.SortNo));
        vh.VoucherAmount = totalDr;

        // ---- VoucherHead.VoucherExistWithSameAmountInSameDate: one question per debit account (ActionId is set here)
        Set<Integer> acked = new LinkedHashSet<>();
        if (dto.duplicateAcknowledgedAccounts != null) {
            for (Integer a : dto.duplicateAcknowledgedAccounts) if (a != null) acked.add(a);
        }
        Set<Integer> seen = new LinkedHashSet<>();
        for (ContraVoucherDto.Detail d : details) {
            if (nzd(d.DebitAmount) <= 0d || !seen.add(nz(d.AccountId)) || acked.contains(nz(d.AccountId))) continue;
            String title = writer.duplicateVoucherTitle(org, comp, vh.VoucherDate, nz(d.AccountId), nzd(d.DebitAmount), nz(d.ActionId));
            if (title != null) {
                throw new DuplicateConfirmation("Voucher against '" + title
                        + "' with same Debit Amount already exists on this date. Do you want to continue?", nz(d.AccountId));
            }
        }

        int id = writer.save(vh, details, costs);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("voucherCode", vh.VoucherCode);
        res.put("voucherAmount", vh.VoucherAmount);
        res.put("message", (insert ? "Record Save Successfully...[" : "Record Update Successfully...[") + vh.VoucherCode + "]");
        return res;
    }

    /** FormValidation (btnplus_Click / btnUpdateDetail_Click) for every posted row: a row that skips it was not built by the screen. */
    private void validateRows(JournalNoOffsetVoucherDto dto, boolean subsidiaryFeature, boolean branchFeature) {
        Set<Integer> accounts = new HashSet<>();
        for (Map<String, Object> a : accounts()) accounts.add(toInt(ci(a, "Id")));
        Set<Integer> withSubsidiary = new HashSet<>();
        if (subsidiaryFeature) {
            for (Map<String, Object> s : subsidiaries()) withSubsidiary.add(toInt(ci(s, "AccountId")));
        }
        for (JournalNoOffsetVoucherDto.Row r : dto.rows) {
            if (nz(r.AccountId) == 0 || !accounts.contains(nz(r.AccountId))) throw new IllegalArgumentException("Account Field is Required");
            if (subsidiaryFeature && withSubsidiary.contains(nz(r.AccountId)) && nz(r.SubsidiaryAccountId) == 0) {
                throw new IllegalArgumentException("Subsidiary Account Field Required");
            }
            if (nzs(r.Remarks).trim().isEmpty()) throw new IllegalArgumentException("Remarks Field is Required");
            if (nz(r.JobLotId) == 0) throw new IllegalArgumentException("JobLot Field is Required");
            if (nzd(r.AmountDr) == 0d && nzd(r.AmountCr) == 0d) throw new IllegalArgumentException("Debit and Credit cannot be equal to zero....!");
            if (branchFeature && nz(r.BranchId) == 0) throw new IllegalArgumentException("Branch Field is Required");
        }
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

    // ================================================================================ rights

    /** CommonServices.SetRightsValueInRightsObject("JournalVoucher_New") with its Admin short-circuit. */
    public Map<String, Boolean> rights() {
        Map<String, Boolean> r = new LinkedHashMap<>();
        for (String n : new String[] {"Save", "Update", "Print", "Delete"}) r.put(n.toLowerCase(), hasRight(n));
        return r;
    }

    private boolean hasRight(String rightName) {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equals(role)) return true;
        try {
            for (Map<String, Object> row : jdbcTemplate.queryForList(SQL_USER_RIGHTS, currentUserContext.currentUserId(),
                    SCREEN_NAME, role == null ? "" : role, currentUserContext.currentCompanyId(), "GetByUserId")) {
                Object n = ci(row, "RightName");
                if (n != null && rightName.equals(String.valueOf(n).trim())) return truthy(ci(row, "Value"));
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, SCREEN_NAME, e);
        }
        return false;
    }

    // ================================================================================ helpers

    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }
    private static String nzs(String v) { return v == null ? "" : v; }
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static Object pos(Map<String, Object> row, int index) {
        int i = 0;
        for (Object v : row.values()) { if (i++ == index) return v; }
        return null;
    }

    private static String dateTimeStr(Object v) {
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        return v == null ? null : String.valueOf(v);
    }

    private static String dateStr(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new SimpleDateFormat("yyyy-MM-dd").format((java.util.Date) v);
        return isoDay(v);
    }

    private static String isoDay(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return (int) Double.parseDouble(v.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static double toDouble(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        if (v == null) return 0d;
        try { return Double.parseDouble(v.toString().replace(",", "").trim()); } catch (NumberFormatException e) { return 0d; }
    }

    private static boolean truthy(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = v.toString().trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
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
