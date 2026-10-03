package com.mst.services;

import com.mst.models.ConfigrationsAllocation;
import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.PaymentReceiptVoucherDto;
import com.mst.repositories.DesktopVoucherWriter;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Screens 28 Cash Payment (DocumentTypeId 1), 29 Bank Payment (2), 30 Cash Receipt (3) and
 * 31 Bank Receipt Voucher (4).
 *
 * Desktop: Architecture.WinApp.Account_Definition.PaymentVoucherNew (1/2, Tag frmCashPaymentVoucher /
 * frmBankPaymentVoucher) and Architecture.WinApp.Account_Definition.ReceiptsVoucherNew (3/4, Tag
 * frmCashReceiptVoucher / frmBankReceiptVoucher) - the NON-tax forms (ScreenDefinition / FavoriteScreens
 * TargetUrl; CommonServices.EditMethodFromLinked with BaseDocumentTypeId 0).
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS EXISTS BESIDE VoucherService.saveVoucher
 * ---------------------------------------------------------------------------------------------
 * The shared saveVoucher persists with JPA repositories. Both desktop forms call
 * BLL VoucherHead.Save -> DAL 0586 SetData: Sp_VoucherHead_Insert|Update, Sp_VoucherDetail_Insert per
 * line, USP_VoucherBalanceCheck, USP_voucherCostCenterDetail_Insert, Sp_VoucherHead_H_Insert,
 * Sp_VoucherDetail_H_Insert, [DAW].[USp_DocumentApprovalDetail_Insert], one transaction. That chain is
 * already reproduced by DesktopVoucherWriter (built for Contra); these four vouchers now use it too.
 * saveVoucher is untouched, so every other voucher keeps its behaviour.
 *
 * ---------------------------------------------------------------------------------------------
 * DEBIT / CREDIT DIRECTION - read from each form's own Insert(), not assumed
 * ---------------------------------------------------------------------------------------------
 *   PaymentVoucherNew.Insert():872-958   vd  = grid account  DEBIT,  AgainstAccountId = CmbCreditAccount
 *                                        vd2 = CmbCreditAccount CREDIT, AgainstAccountId = grid account
 *   ReceiptsVoucherNew.Insert():666-734  vd  = CmbDebitAccount DEBIT, AgainstAccountId = grid account
 *                                        vd2 = grid account  CREDIT, AgainstAccountId = CmbDebitAccount
 * i.e. on a receipt the cash/bank header account is the DEBIT side - the opposite of a payment.
 * WHT pair (vd3/vd4, once per voucher, both LineId = last LineId + 1):
 *   Payment: vd3 RefDocNoId (CmbAgainstAc) DEBIT / vd4 AgainstAccountId (CmbWithHoldingAc) CREDIT
 *   Receipt: vd3 AgainstAccountId (CmbWithHoldingAc) DEBIT / vd4 RefDocNoId (CmbAgainstAc) CREDIT
 */
@Service
public class PaymentReceiptVoucherService {

    private static final Logger LOG = LoggerFactory.getLogger(PaymentReceiptVoucherService.class);

    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?";

    @Autowired private DesktopVoucherWriter writer;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private VoucherDesktopConfigService config;

    // ================================================================================ helpers: form

    public static boolean isPayment(int doc) { return doc == 1 || doc == 2; }

    public static void requireDoc(int doc) {
        if (doc < 1 || doc > 4) throw new IllegalArgumentException("Unknown voucher type " + doc);
    }

    /** base.Tag of the form, which is the ScreenName its rights are read under (Load: SetRightsValueInRightsObject). */
    public static String screenName(int doc) {
        switch (doc) {
            case 1: return "frmCashPaymentVoucher";
            case 2: return "frmBankPaymentVoucher";
            case 3: return "frmCashReceiptVoucher";
            default: return "frmBankReceiptVoucher";
        }
    }

    // ================================================================================ lookups

    /**
     * Everything the form binds on Load, each from the desktop's own call:
     *   headerAccounts  AccountTitleFill -> COAAllocation.GetDetailAccountByDocumentTypeId ->
     *                   Sp_COAAllocation_GetAllMethod @OrganizationId @CompanyId @DocumentTypeId
     *                   @Activity='GetDetailAccountsByDocumentTypeId' (type 2 for 1/3, 15 for 2/4)
     *   detailAccounts  DetailAccountFill (VoucherDesktopConfigService "payment"/"receipt")
     *   whtAccounts     AccountsComboBind -> GetAccountsFromGlobalByTypeIds(null, {2,11,15}) - EXCLUDES 2,11,15
     *   paymentTypes    lookUp.GetDataPaymentTypeForPayments | GetDataPaymentTypeForReceipts
     *   jobLots         jobLot.GetList -> SP_JobLot_ReadMethod @OrganizationId @CompanyId @Activity='GetAll'
     *   projects        CommonServices.ProjectServiceBind -> Sp_Projects_GetAllMethod ... @MethodType='GetAll'
     *   currencies      MultiCurrency.GetAll -> Sp_MultiCurrency_GetAllMethod ... @Activity='ReadAll'
     *   taxTypes        TaxesTypes.GetForComboBind(Type=1) -> Sp_TaxesTypes_GetAllMethod ... @Type=1 @Activity='ReadByCombo'
     *   branches        BranchesAllocationToUser.GetBranchsAllocatedToUser -> USP_GetBranchsAllocatedToUser
     *   costCenters     Projects.GetSubCostCenters(org, comp, user, appId, 0) -> usp_getCostCenters (@AppId when != 0)
     *   instrumentTypes VoucherHead.GetInstrumentTypes -> usp_getInstrumentType   (payment form only)
     *   chequeTypes     ChequeType.GetChequeType -> [dbo].[sp_ChequeType]         (payment form only)
     */
    public Map<String, Object> lookups(int doc) {
        requireDoc(doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        Map<String, Object> r = new LinkedHashMap<>();

        List<Map<String, Object>> header = new ArrayList<>();
        for (Map<String, Object> row : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Activity=?",
                org, comp, doc, "GetDetailAccountsByDocumentTypeId")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", ci(row, "Id"));
            o.put("accountTitle", ci(row, "AccountTitle"));
            o.put("accountCode", ci(row, "AccountCode"));
            header.add(o);
        }
        r.put("headerAccounts", header);
        r.put("detailAccounts", config.detailAccounts(isPayment(doc) ? "payment" : "receipt"));
        r.put("whtAccounts", globalAccountsWithout(org, comp, new int[] {2, 11, 15}));
        r.put("paymentTypes", jdbcTemplate.queryForList("EXEC [Account].[USP_lookUp_GetAllMethod] @Activity=?",
                isPayment(doc) ? "GetDataPaymentTypeForPayments" : "GetDataPaymentTypeForReceipts"));
        r.put("jobLots", jdbcTemplate.queryForList(
                "EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?, @CompanyId=?, @Activity=?", org, comp, "GetAll"));
        r.put("projects", jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Projects_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType=?", org, comp, "GetAll"));
        r.put("currencies", jdbcTemplate.queryForList(
                "EXEC dbo.Sp_MultiCurrency_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?", org, comp, "ReadAll"));
        r.put("taxTypes", jdbcTemplate.queryForList(
                "EXEC dbo.Sp_TaxesTypes_GetAllMethod @OrganizationId=?, @CompanyId=?, @Type=?, @Activity=?",
                org, comp, 1, "ReadByCombo"));

        Map<String, Object> flags = flags(doc);
        r.put("flags", flags);
        if (Boolean.TRUE.equals(flags.get("branchFeature"))) {
            r.put("branches", jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId=?, @CompanyId=?, @UserId=?",
                    org, comp, u.getId()));
        } else {
            r.put("branches", new ArrayList<>());
        }
        if (Boolean.TRUE.equals(flags.get("isBookingOffice"))) {
            int appId = u.getAppId() == null ? 0 : u.getAppId();
            r.put("costCenters", appId != 0
                    ? jdbcTemplate.queryForList("EXEC dbo.usp_getCostCenters @OrganizationId=?, @CompanyId=?, @UserId=?, @AppId=?",
                            org, comp, u.getId(), appId)
                    : jdbcTemplate.queryForList("EXEC dbo.usp_getCostCenters @OrganizationId=?, @CompanyId=?, @UserId=?",
                            org, comp, u.getId()));
        } else {
            r.put("costCenters", new ArrayList<>());
        }
        if (isPayment(doc)) {
            r.put("instrumentTypes", jdbcTemplate.queryForList("EXEC dbo.usp_getInstrumentType"));
            /* sp_ChequeType is SELECT * - CheqTypeImage (an image column) is not sent to the page. */
            List<Map<String, Object>> ct = new ArrayList<>();
            for (Map<String, Object> row : jdbcTemplate.queryForList("EXEC [dbo].[sp_ChequeType]")) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("id", ci(row, "id"));
                o.put("CheqType", ci(row, "CheqType"));
                ct.add(o);
            }
            r.put("chequeTypes", ct);
        }
        r.put("nextCode", nextCode(doc));
        return r;
    }

    /** The Load-time switches of the form, plus the defaults DefaultConfigurations() applies. */
    public Map<String, Object> flags(int doc) {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> f = new LinkedHashMap<>(config.voucherFlags());
        Map<String, ConfigrationsAllocation> m = config.configMap();
        f.put("preventNegativeBalanceEntry", VoucherDesktopConfigService.toBool(VoucherDesktopConfigService.key(m, "is Minus balance Allowed")));
        f.put("displayWarningforNegativeBalance", VoucherDesktopConfigService.toBool(VoucherDesktopConfigService.key(m, "DisplayWarningforNegativeBalance")));
        f.put("disableBothNegativeBalanceRestrictions", VoucherDesktopConfigService.toBool(VoucherDesktopConfigService.key(m, "DisableBothNegativeBalanceRestrictions")));
        f.put("defaultJobLotId", toInt(VoucherDesktopConfigService.key(m, "Job/Lot")));
        f.put("baseCurrencyId", toInt(VoucherDesktopConfigService.key(m, "Base Currency")));
        String rate = VoucherDesktopConfigService.key(m, "BaseCurrencyRate");
        f.put("baseCurrencyRate", rate == null ? null : VoucherDesktopConfigService.toDouble(rate));
        f.put("amountDecimals", toInt(VoucherDesktopConfigService.key(m, "Default NoofDecimal Points For Amount")));
        /* CommonServices.GetDecimalConfiguration: DecimalRateFormate (txtExchangeRate_Leave / DefaultConfigurations). */
        f.put("rateDecimals", toInt(VoucherDesktopConfigService.key(m, "Default NoofDecimal Points For Rate")));
        /* BranchesFill: cmbBranchName.Text = UserAccount.BranchName - the signed-in branch is the default row. */
        f.put("defaultBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        // SubsidiaryAccountAllownOnVouchers = GetERPFeatureById(4)
        f.put("subsidiaryFeature", config.erpFeature(4));
        f.put("isBookingOffice", isBookingOffice(u));
        f.put("rights", rights(doc));
        return f;
    }

    /** Load: IsBookingOffice = GetApplicationsAllocateToCompanyList has AppId 5 for this company, IsActive. */
    private boolean isBookingOffice(UserAccount u) {
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList("EXEC [dbo].[USP_GetApplicationsAllocateToCompany]")) {
                if (toInt(ci(r, "AppId")) == 5 && toInt(ci(r, "CompanyId")) == u.getCompanyId()) {
                    return truthy(ci(r, "IsActive"));
                }
            }
        } catch (Exception e) {
            LOG.warn("USP_GetApplicationsAllocateToCompany failed; treating as not a booking office", e);
        }
        return false;
    }

    private List<Map<String, Object>> globalAccountsWithout(int org, int comp, int[] without) {
        Set<Integer> wo = new HashSet<>();
        for (int i : without) wo.add(i);
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?", org, comp)) {
            if (wo.contains(toInt(ci(r, "AccountTypeId")))) continue;
            int id = toInt(ci(r, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", id);
            o.put("accountTitle", ci(r, "AccountTitle"));
            o.put("accountCode", ci(r, "AccountCode"));
            o.put("parentAccountTitle", ci(r, "ParentAccountTitle"));
            o.put("accountClass", ci(r, "AccountClassName"));
            out.add(o);
        }
        return out;
    }

    /** CommonServices.GenerateVoucherCode(doc). */
    public int nextCode(int doc) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return writer.nextVoucherCode(u.getOrganizationId(), u.getCompanyId(), doc,
                currentUserContext.currentFinancialYearId(), u.getBranchesId() == null ? 0 : u.getBranchesId());
    }

    /** GetAccountBalance / AccountCurrentBalance: ReadByCurrentBalanceByDateAndAccountId, Balance column. */
    public double balance(int accountId, String date) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return writer.accountBalance(u.getOrganizationId(), u.getCompanyId(),
                currentUserContext.currentFinancialYearId(), isoDay(date), accountId);
    }

    /**
     * checkBox1_CheckedChanged: TaxScheduleMain.ReadTaxSchedule -> Sp_TaxSchedule_GetAllMehtod
     * @OrganizationId @CompanyId @EffectedDate @TaxNameId @Activity='GetTaxPercentInTaxSchedule'.
     * Returns the first row (TaxPercent, TaxGLAccountId, AccountTitle) or null when there is none.
     */
    public Map<String, Object> taxSchedule(int taxTypeId, String date) {
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_TaxSchedule_GetAllMehtod @OrganizationId=?, @CompanyId=?, @EffectedDate=?, @TaxNameId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), isoDay(date), taxTypeId, "GetTaxPercentInTaxSchedule");
        if (rows.isEmpty()) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("taxPercent", ci(rows.get(0), "TaxPercent"));
        o.put("taxGLAccountId", ci(rows.get(0), "TaxGLAccountId"));
        o.put("accountTitle", ci(rows.get(0), "AccountTitle"));
        return o;
    }

    // ================================================================================ history

    /**
     * HistoryFillCpv/Bpv (PaymentVoucherNew) and HistoryFillCrv/Brv (ReceiptsVoucherNew) ->
     * BLL VoucherHead.VoucherFormHistory -> USP_VoucherFormHistory. Each form passes its OWN single
     * DocumentTypeName (vh.Ids = "1" / "2" / "3" / "4"), CanViewAllRecord = the "CanView AllRecord"
     * right (EntryUser only when that right is off), AppId = UserAccount.AppId (only when != 0), and
     * IsApproved unless the status combo reads "All". Every other parameter only when set.
     * No fallback query: a failure is reported, never replaced with rows from another source.
     */
    public Map<String, Object> history(int doc, String dateType, String fromDate, String toDate,
                                       Integer fromDocNo, Integer toDocNo, Integer accountId, String approvedStatus) {
        requireDoc(doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean viewAll = hasRight(doc, "CanView AllRecord");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_VoucherFormHistory @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeName=?, @CanViewAllRecord=?");
        List<Object> a = new ArrayList<>();
        a.add(u.getOrganizationId()); a.add(u.getCompanyId()); a.add(u.getId()); a.add(String.valueOf(doc)); a.add(viewAll);
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
        if (nz(accountId) != 0) { sql.append(", @AccountId=?"); a.add(accountId); }
        if (!viewAll) { sql.append(", @EntryUser=?"); a.add(u.getId()); }
        if (!"all".equalsIgnoreCase(approvedStatus)) { sql.append(", @IsApproved=?"); a.add("approved".equalsIgnoreCase(approvedStatus)); }
        int appId = u.getAppId() == null ? 0 : u.getAppId();
        if (appId != 0) { sql.append(", @AppId=?"); a.add(appId); }

        List<Map<String, Object>> raw = jdbcTemplate.queryForList(sql.toString(), a.toArray());
        boolean bank = doc == 2 || doc == 4;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", ci(r, "Id"));
            o.put("documentTypeId", ci(r, "DocumentTypeId"));
            o.put("voucherDate", dateStr(ci(r, "VoucherDate")));
            o.put("voucherCode", ci(r, "VoucherCode"));
            o.put("documentType", ci(r, "DocumentTypeCode"));
            if (bank) o.put("chequeNo", ci(r, "ChequeNo"));
            o.put("accountId", ci(r, "RefAccountId"));
            o.put("accountTitle", ci(r, "AccountTitle"));
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

    /**
     * ComboBindForCpvHistory / Bpv / Crv / Brv -> VoucherReports.LedgerByJobLotDropDownAndLists ->
     * Sp_Vouchers_LedgerByJobLot_DropDownAndLists @OrganizationId @CompanyId @AppId @UserId
     * @DocumentTypeIds (no @ActivityType - the form keeps the rows whose ActivityType is
     * "Account Header"). The desktop passes UserAccount.OrganizationId as CompanyId here (a
     * copy-paste slip); the signed-in CompanyId is sent instead, as every other voucher port does.
     */
    public List<Map<String, Object>> historyAccounts(int doc) {
        requireDoc(doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_LedgerByJobLot_DropDownAndLists @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @DocumentTypeIds=?",
                u.getOrganizationId(), u.getCompanyId(), u.getAppId() == null ? 0 : u.getAppId(), u.getId(), String.valueOf(doc))) {
            if (!"Account Header".equals(str(ci(r, "ActivityType")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("name", ci(r, "name"));
            out.add(o);
        }
        return out;
    }

    // ================================================================================ load

    /**
     * ReadById(): VoucherHead.GetByID -> Sp_Vouchers_GetMethods @Id @Activity='ReadByID', details
     * @Activity='VoucherDetail_ReadByVoucherHeadID'. The grid is rebuilt from the lines with
     * IsTaxable == "False" and AccountId != the header account (so the mirror of each pair drops out),
     * the WHT controls from the IsTaxable == "True" lines - each exactly as the form's own ReadById does.
     */
    public Map<String, Object> load(int doc, int id) {
        return load(doc, id, false);
    }

    /**
     * ReadById(ID, SaveAs). PaymentVoucherNew with SaveAs: header Remarks and line Comments as stored (no
     * RemarksOtherLingo choice), ChequeDate = DateTime.Now, ChequeId 0, ChequeNo "", PayeeTitle "".
     * ReceiptsVoucherNew.ReadById ignores the flag; its SaveAs handler clears CheqDate / CmbCheqNo /
     * txtPayTitle afterwards, which the page does.
     */
    public Map<String, Object> load(int doc, int id, boolean saveAs) {
        requireDoc(doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> heads = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "ReadByID");
        if (heads.isEmpty()) return null;
        Map<String, Object> h = heads.get(0);
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId() || toInt(ci(h, "CompanyId")) != u.getCompanyId()) {
            return null;                                      // never another tenant's document
        }
        int hDoc = toInt(ci(h, "DocumentTypeId"));
        List<Map<String, Object>> details = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "VoucherDetail_ReadByVoucherHeadID");

        Map<String, Object> flags = config.voucherFlags();
        boolean autoPay = Boolean.TRUE.equals(flags.get("autoRemarksForPaymentThroughBank"));
        int refAccountId = toInt(ci(h, "RefAccountId"));
        boolean inclusive = truthy(ci(h, "InclusiveTax"));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("documentTypeId", hDoc);
        out.put("voucherCode", ci(h, "VoucherCode"));
        out.put("voucherDate", dateStr(ci(h, "VoucherDate")));
        out.put("refAccountId", refAccountId);
        out.put("refAccountTitle", ci(h, "AccountTitle"));
        out.put("projectId", ci(h, "ProjectId"));
        out.put("multiCurrencyId", ci(h, "MultiCurrencyId"));
        out.put("exchangeCurrencyRate", ci(h, "ExchangeCurrencyRate"));
        out.put("fcAmount", ci(h, "FcAmount"));
        out.put("voucherAmount", ci(h, "VoucherAmount"));
        out.put("isApproved", truthy(ci(h, "IsApproved")));
        out.put("inclusiveTax", inclusive);
        out.put("refDocNoId", ci(h, "RefDocNoId"));
        out.put("againstAccountId", ci(h, "AgainstAccountId"));
        String hRemarks = str(ci(h, "Remarks")), hOther = (String) ci(h, "RemarksOtherLingo");
        if (isPayment(hDoc)) {
            // ReadById:2574 - BPV shows RemarksOtherLingo (the operator's own text) when auto-remarks rewrote Remarks.
            out.put("remarks", (saveAs || hDoc != 2) ? hRemarks : (autoPay ? hOther : (hOther != null ? hOther : hRemarks)));
        } else {
            out.put("remarks", hRemarks);
            out.put("payTitle", ci(h, "PayTitle"));
            out.put("chequeNo", ci(h, "ChequeNo"));
            out.put("chequeDate", dateStr(ci(h, "ChequeDate")));
        }

        List<Map<String, Object>> rows = new ArrayList<>();
        boolean wht = false;
        Map<String, Object> whtOut = new LinkedHashMap<>();
        for (Map<String, Object> d : details) {
            String taxable = str(ci(d, "IsTaxable"));
            int acc = toInt(ci(d, "AccountId"));
            double debit = toDouble(ci(d, "DebitAmount")), credit = toDouble(ci(d, "CreditAmount"));
            if ("False".equals(taxable) && acc != refAccountId) {
                Map<String, Object> r = new LinkedHashMap<>();
                r.put("paymentType", ci(d, "PaymentType"));
                r.put("paymentTypeId", ci(d, "PaymentTypeId"));
                r.put("accountCode", ci(d, "AccountCode"));
                r.put("accountId", acc);
                r.put("accountTitle", ci(d, "AccountTitle"));
                r.put("jobLotId", ci(d, "JobLotId"));
                r.put("jobLot", ci(d, "JobLotDescription"));
                r.put("fcyAmount", ci(d, "DCurrencyAmount"));
                r.put("branchId", ci(d, "BranchesId"));
                r.put("branchName", ci(d, "BranchName"));
                r.put("costCenterId", ci(d, "CostCenterId"));
                r.put("costCenterName", ci(d, "CostCenterName"));
                if (isPayment(hDoc)) {
                    String c = (String) ci(d, "Comments"), co = (String) ci(d, "CommentsOtherLingo");
                    r.put("remarks", (saveAs || hDoc != 2) ? c : (!autoPay ? (co != null ? co : c) : co));
                    double tax = toDouble(ci(d, "TaxAmount"));
                    r.put("amount", (inclusive && debit > 0) ? debit + tax : debit);
                    r.put("taxAmount", tax);
                    r.put("financialInstrumentId", ci(d, "InstrumentTypeId"));
                    r.put("financialInstrument", ci(d, "InstrumentType"));
                    r.put("chequeDate", saveAs ? java.time.LocalDate.now().toString() : dateStr(ci(d, "DCheqDate")));
                    r.put("chequeId", saveAs ? 0 : ci(d, "InvoiceNoRefId"));
                    r.put("chequeNo", saveAs ? "" : ci(d, "CheqNoDetail"));
                    r.put("payeeTitle", saveAs ? "" : ci(d, "PayeeTitle"));
                    r.put("chequeTypeId", ci(d, "ChequeTypeId"));
                } else {
                    r.put("remarks", ci(d, "Comments"));
                    r.put("amount", credit);
                }
                rows.add(r);
            } else if ("True".equals(taxable)) {
                wht = true;
                if (isPayment(hDoc)) {
                    whtOut.put("againstAcId", ci(h, "RefDocNoId"));
                    if (credit > 0) {
                        whtOut.put("withHoldingAcId", acc);
                        whtOut.put("withHoldingAcTitle", ci(d, "AccountTitle"));
                        whtOut.put("taxTypeId", ci(d, "TaxTypeId"));
                        whtOut.put("taxPercent", ci(d, "TaxPrcnt"));
                    }
                } else {
                    if (credit > 0) {
                        whtOut.put("againstAcId", acc);
                        whtOut.put("againstAcTitle", ci(d, "AccountTitle"));
                    }
                    if (debit > 0) {
                        whtOut.put("withHoldingAcId", acc);
                        whtOut.put("withHoldingAcTitle", ci(d, "AccountTitle"));
                        whtOut.put("taxTypeId", ci(d, "TaxTypeId"));
                        whtOut.put("taxPercent", ci(d, "TaxPrcnt"));
                        whtOut.put("taxAmount", ci(d, "TaxesTotalAmount"));
                    }
                }
            }
        }
        out.put("rows", rows);
        out.put("includeWHT", wht);
        out.put("wht", whtOut);
        return out;
    }

    /**
     * History detail grid - VoucherDetailByHeaderId / VoucherDetailBPVByHeaderId (PaymentVoucherNew),
     * VoucherDetailByHeaderId / VoucherDetailBRVByHeaderId (ReceiptsVoucherNew): VoucherHead.GetByID ->
     * Sp_Vouchers_GetMethods 'ReadByID' + 'VoucherDetail_ReadByVoucherHeadID'. Every line is returned as
     * stored; the page applies each form's own column set, order and Inclusive-tax add-back.
     */
    public Map<String, Object> lines(int doc, int id) {
        requireDoc(doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        List<Map<String, Object>> heads = jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "ReadByID");
        if (heads.isEmpty()) return null;
        Map<String, Object> h = heads.get(0);
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId() || toInt(ci(h, "CompanyId")) != u.getCompanyId()) {
            return null;                                      // never another tenant's document
        }
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @Id=?, @Activity=?", id, "VoucherDetail_ReadByVoucherHeadID")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("id", toInt(ci(d, "Id")));
            o.put("accountCode", ci(d, "AccountCode"));
            o.put("accountTitle", ci(d, "AccountTitle"));
            o.put("subsidiaryAccount", ci(d, "SubsidiaryAccountTitle"));
            o.put("jobLot", ci(d, "JobLotDescription"));
            o.put("remarks", ci(d, "Comments"));
            o.put("debit", toDouble(ci(d, "DebitAmount")));
            o.put("credit", toDouble(ci(d, "CreditAmount")));
            o.put("fcy", toDouble(ci(d, "DCurrencyAmount")));
            o.put("branchName", ci(d, "BranchName"));
            o.put("costCenter", ci(d, "CostCenterName"));
            o.put("taxAmount", toDouble(ci(d, "TaxAmount")));
            o.put("isTaxable", str(ci(d, "IsTaxable")));
            o.put("chequeDate", dateStr(ci(d, "DCheqDate")));
            o.put("chequeNo", ci(d, "CheqNoDetail"));
            o.put("payeeTitle", ci(d, "PayeeTitle"));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("documentTypeId", toInt(ci(h, "DocumentTypeId")));
        out.put("inclusiveTax", truthy(ci(h, "InclusiveTax")));
        out.put("rows", rows);
        return out;
    }

    /**
     * cmbCurrency_Leave: a currency other than the base one -> VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher
     * (BLL 0654) -> Sp_Vouchers_GetMethods @OrganizationId @CompanyId @DocumentTypeIds (CmbVoucherType.Value)
     * @DMultiCurrencyIds (cmbCurrency.Value) @Activity='GetMultiCurrencyAndLastRate', column LastExchRate;
     * no row -> "0". The base currency itself takes BaseRate on the page.
     */
    public double lastRate(int doc, int currencyId) {
        requireDoc(doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?");
        List<Object> a = new ArrayList<>();
        a.add(u.getOrganizationId()); a.add(u.getCompanyId()); a.add(String.valueOf(doc));
        if (currencyId != 0) { sql.append(", @DMultiCurrencyIds=?"); a.add(String.valueOf(currencyId)); }
        sql.append(", @Activity=?"); a.add("GetMultiCurrencyAndLastRate");
        List<Map<String, Object>> rows = jdbcTemplate.queryForList(sql.toString(), a.toArray());
        return rows.isEmpty() ? 0d : toDouble(ci(rows.get(0), "LastExchRate"));
    }

    // ================================================================================ save

    @Transactional
    public Map<String, Object> save(int doc, PaymentReceiptVoucherDto dto) {
        requireDoc(doc);
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean insert = dto.Id == null || dto.Id <= 0;
        /* Load: Save.Enabled / Update.Enabled = the screen rights. A disabled button is not a check. */
        if (insert && !hasRight(doc, "Save")) throw new SecurityException("You don't have Save right on this screen.");
        if (!insert && !hasRight(doc, "Update")) throw new SecurityException("You don't have Update right on this screen.");
        if (!insert) {
            Map<String, Object> existing = load(doc, dto.Id);
            if (existing == null || toInt(existing.get("documentTypeId")) != doc) {
                throw new IllegalArgumentException("Record Not Update  " + dto.Id);
            }
        }

        int org = u.getOrganizationId(), comp = u.getCompanyId();
        int branchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        int year = currentUserContext.currentFinancialYearId();
        Map<String, Object> f = flags(doc);
        boolean multiCurrency = Boolean.TRUE.equals(f.get("multiCurrencyFeature"));
        boolean branchFeature = Boolean.TRUE.equals(f.get("branchFeature"));
        int decimals = toInt(f.get("amountDecimals"));
        Set<String> ack = new HashSet<>(dto.acknowledged == null ? new ArrayList<>() : dto.acknowledged);

        // lookups the desktop reads from its bound combos (display text, cheque/branch flags)
        Map<Integer, String> headerTitles = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(
                "EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Activity=?",
                org, comp, doc, "GetDetailAccountsByDocumentTypeId")) {
            headerTitles.put(toInt(ci(r, "Id")), str(ci(r, "AccountTitle")));
        }
        Map<Integer, String> detailTitles = new LinkedHashMap<>();
        for (Map<String, Object> r : config.detailAccounts(isPayment(doc) ? "payment" : "receipt")) {
            detailTitles.put(toInt(r.get("id")), str(r.get("accountTitle")));
        }
        Map<Integer, String> paymentTypes = new LinkedHashMap<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList("EXEC [Account].[USP_lookUp_GetAllMethod] @Activity=?",
                isPayment(doc) ? "GetDataPaymentTypeForPayments" : "GetDataPaymentTypeForReceipts")) {
            paymentTypes.put(toInt(ci(r, "Id")), str(ci(r, "PaymentType")));
        }

        validate(doc, dto, f, multiCurrency, branchFeature, headerTitles, detailTitles, paymentTypes);

        String voucherDate = isoDay(dto.VoucherDate);
        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        int voucherCode = nz(dto.VoucherCode);
        if (insert && voucherCode <= 0) voucherCode = nextCode(doc);

        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.Id = insert ? 0 : dto.Id;
        vh.DocumentTypeId = doc;
        vh.ProjectId = nz(dto.ProjectId);
        vh.VoucherCode = voucherCode;
        vh.VoucherDate = voucherDate;
        vh.RefAccountId = nz(dto.RefAccountId);
        vh.AgainstAccountId = nz(dto.WithHoldingAcId);    // CmbWithHoldingAc
        vh.RefDocNoId = nz(dto.AgainstAcId);              // CmbAgainstAc
        vh.IncludeWHT = Boolean.TRUE.equals(dto.IncludeWHT);
        vh.BranchId = branchId;
        vh.OrganizationId = org;
        vh.CompanyId = comp;
        vh.FinancialYearId = year;
        vh.EntryUser = u.getId();
        vh.ModifyUser = u.getId();
        vh.EntryDate = now;                                // BLL 0654 Save(): EntryDate = ModifyDate = Now
        vh.ModifyDate = now;
        vh.MultiCurrencyId = nz(dto.MultiCurrencyId);
        vh.ExchangeCurrencyRate = nzd(dto.ExchangeCurrencyRate);
        vh.FcAmount = nzd(dto.FcAmount);

        List<ContraVoucherDto.Detail> details = new ArrayList<>();
        List<ContraVoucherDto.CostCentre> costCentres = new ArrayList<>();
        List<PaymentReceiptVoucherDto.Row> rows = dto.rows;
        int count = rows.size();
        String headerTitle = headerTitles.getOrDefault(nz(dto.RefAccountId), "");
        double rate = nzd(dto.ExchangeCurrencyRate);
        double taxAmount = vh.IncludeWHT ? nzd(dto.TaxAmount) : 0d;
        double voucherAmount;

        if (isPayment(doc)) {
            boolean inclusive = Boolean.TRUE.equals(dto.InclusiveTax);
            vh.InclusiveTax = inclusive;                  // radioButton1.Checked
            String remarks = dto.Remarks == null ? "" : dto.Remarks;   // vh.Remarks = txtremarksmain.Text
            vh.Remarks = remarks;
            vh.PayTitle = "";                                // txtPayTitle is reset after each Add; header PayTitle comes from the row
            /* Total()/TaxAmountProportion(): row TaxAmount only in Included mode. */
            double totalDetail = 0d;
            for (PaymentReceiptVoucherDto.Row r : rows) totalDetail += nzd(r.Amount);
            double totalTax = round(taxAmount, decimals);
            double[] rowTax = new double[count];
            for (int i = 0; i < count; i++) {
                rowTax[i] = (inclusive && totalTax > 0 && totalDetail > 0)
                        ? round(totalTax / totalDetail * nzd(rows.get(i).Amount), decimals) : 0d;
            }
            /* txtTotalAmount (Total()): Excluded = base + tax, Included = the grid sum. */
            voucherAmount = inclusive ? totalDetail : totalDetail + taxAmount;

            Map<String, Boolean> autoFlags = new LinkedHashMap<>();
            for (String k : new String[] {"autoRemarksForPaymentThroughBank", "autoRemarksIncludeHeaderRemarks",
                    "autoRemarksIncludeChequeDateAndNumber", "autoRemarksIncludeChequeNumber",
                    "autoRemarksIncludePayeeTitle", "autoRemarksIncludeDetailRemarks"}) {
                autoFlags.put(k, Boolean.TRUE.equals(f.get(k)));
            }
            int lineId = 1;
            double debitSum = 0d;
            for (int i = 0; i < count; i++) {
                PaymentReceiptVoucherDto.Row r = rows.get(i);
                double amount = nzd(r.Amount);
                String rowRemarks = r.Remarks == null ? "" : r.Remarks;
                String rowTitle = detailTitles.getOrDefault(nz(r.AccountId), "");
                String ptText = paymentTypes.getOrDefault(nz(r.PaymentTypeId), "");
                String chequeDate = isoDay(r.ChequeDate);
                int branch = branchFeature ? nz(r.BranchId) : branchId;

                ContraVoucherDto.Detail vd = new ContraVoucherDto.Detail();
                String[] br = buildRemarks(autoFlags, vh.Remarks, r, headerTitle);
                if (doc == 2) {
                    if (count == 1) {
                        vh.CheqId = nz(r.ChequeId);
                        vh.ChequeNo = r.ChequeNo;
                        vh.ChequeDate = chequeDate;
                        vh.PayTitle = r.PayeeTitle;
                    }
                    vd.Comments = br[0];
                    vd.CommentsOtherLingo = rowRemarks;
                } else {
                    vd.Comments = rowRemarks;
                }
                if (ptText.contains("Advance")) vd.AdvanceAmount = amount;
                vd.LineId = lineId;
                vd.SortNo = i + 1;
                vd.PaymentTypeId = nz(r.PaymentTypeId);
                vd.PaymentType = ptText;
                vd.AccountId = nz(r.AccountId);
                vd.AgainstAccountId = nz(dto.RefAccountId);
                vd.JobLotId = nz(r.JobLotId);
                vd.TaxAmount = rowTax[i];
                vd.DebitAmount = inclusive ? amount - rowTax[i] : amount;
                debitSum += vd.DebitAmount;
                vd.IsTaxable = "False";
                vd.InvoiceNoRefId = nz(r.ChequeId);
                vd.InstrumentTypeId = nz(r.FinancialInstrumentId);
                vd.CheqNoDetail = r.ChequeNo;
                vd.DCheqDate = chequeDate;
                vd.PayeeTitle = r.PayeeTitle;
                vd.ChequeTypeId = nz(r.ChequeTypeId);
                vd.BranchesId = branch;
                vd.DMultiCurrencyId = nz(dto.MultiCurrencyId);
                vd.DExchangeCurrencyRate = rate;
                vd.DCurrencyAmount = rate > 0 ? vd.DebitAmount / rate : 0d;
                vd.CostCenterId = nz(r.CostCenterId);
                if (count == 1 && vh.Remarks.trim().isEmpty()) {
                    vh.RemarksOtherLingo = null;
                    vh.Remarks = vd.Comments == null ? "" : vd.Comments;
                }
                if (!insert) vd.ActionId = 2;
                details.add(vd);
                lineId++;

                ContraVoucherDto.Detail vd2 = new ContraVoucherDto.Detail();
                if (doc == 2) {
                    vd2.Comments = br[1] + " PAID TO: " + rowTitle;
                    vd2.CommentsOtherLingo = rowRemarks;
                } else {
                    vd2.Comments = vh.Remarks.trim().isEmpty() ? rowRemarks : vh.Remarks.trim();
                }
                if ("Advance".equals(ptText)) vd2.AdvanceAmount = amount;
                vd2.LineId = lineId;
                vd2.SortNo = i + 1;
                vd2.PaymentTypeId = nz(r.PaymentTypeId);
                vd2.PaymentType = ptText;
                vd2.AccountId = nz(dto.RefAccountId);
                vd2.AgainstAccountId = nz(r.AccountId);
                vd2.JobLotId = nz(r.JobLotId);
                vd2.TaxAmount = rowTax[i];
                vd2.CreditAmount = inclusive ? amount - rowTax[i] : amount;
                vd2.DebitAmount = 0d;
                vd2.IsTaxable = "False";
                vd2.InvoiceNoRefId = nz(r.ChequeId);
                vd2.InstrumentTypeId = nz(r.FinancialInstrumentId);
                vd2.ChequeTypeId = nz(r.ChequeTypeId);
                vd2.BranchesId = branch;
                vd2.CheqNoDetail = r.ChequeNo;
                vd2.DCheqDate = chequeDate;
                vd2.PayeeTitle = r.PayeeTitle;
                vd2.DMultiCurrencyId = nz(dto.MultiCurrencyId);
                vd2.DExchangeCurrencyRate = rate;
                vd2.DCurrencyAmount = vd.DCurrencyAmount;   // computed from vd, as the desktop does
                vd2.CostCenterId = nz(r.CostCenterId);
                details.add(vd2);
                lineId++;

                if (nz(r.CostCenterId) > 0) {
                    ContraVoucherDto.CostCentre c = new ContraVoucherDto.CostCentre();
                    c.SortNo = i + 1;
                    c.CostCenterId = nz(r.CostCenterId);
                    c.costPrcent = new BigDecimal("100");
                    c.costAmount = inclusive ? amount - rowTax[i] : amount;
                    costCentres.add(c);
                }
            }
            if (vh.IncludeWHT) {
                String comments = "Tax Name    " + nzs(dto.TaxTypeName) + "   Tax %   " + nzs(dto.TaxPercent).trim();
                ContraVoucherDto.Detail vd3 = whtLine(lineId + 1, vh.RefDocNoId, vh.AgainstAccountId, dto, comments);
                vd3.DebitAmount = taxAmount;
                details.add(vd3);
                ContraVoucherDto.Detail vd4 = whtLine(lineId + 1, vh.AgainstAccountId, vh.RefDocNoId, dto, comments);
                vd4.CreditAmount = taxAmount;
                details.add(vd4);
            }

            /* Insert(): negative balance (Acc = "Bank" for 2 else "Cash"), then the duplicate check. */
            if (!Boolean.TRUE.equals(f.get("disableBothNegativeBalanceRestrictions"))) {
                double closing = Math.round(writer.accountBalance(org, comp, year, voucherDate, nz(dto.RefAccountId)));
                String acc = doc == 2 ? "Bank" : "Cash";
                if (debitSum > closing) {
                    String msg = "Debit Amount cannot be greater than " + acc + " Balance.\n" + acc + " Balance is "
                            + num(closing) + " and Total Debit Amount is " + num(debitSum) + ".";
                    if (Boolean.TRUE.equals(f.get("preventNegativeBalanceEntry"))) throw new IllegalArgumentException(msg);
                    if (Boolean.TRUE.equals(f.get("displayWarningforNegativeBalance")) && !ack.contains("negativeBalance")) {
                        throw new ConfirmationRequiredException(msg, "negativeBalance");
                    }
                }
            }
            duplicateGuard(details, ack, org, comp, voucherDate);
            vh.VoucherAmount = voucherAmount;
            /* GetGLBalanceByAccountTypeIdClassIdAndAccountId(org, comp, AcId, date, 3, 3) per first line of each account with a debit. */
            if (!ack.contains("glBalance")) {
                Set<Integer> unique = new HashSet<>();
                for (ContraVoucherDto.Detail d : details) {
                    if (unique.add(nz(d.AccountId)) && nzd(d.DebitAmount) > 0 && !ack.contains("glBalance:" + nz(d.AccountId))) {
                        List<Map<String, Object>> g = jdbcTemplate.queryForList(
                                "EXEC [dbo].[USP_GetGLBalanceByAccountTypeIdClassIdAndAccountId] @OrganizationId=?, @CompanyId=?, @AccountId=?, @ToDate=?, @AccountTypeId=?, @AccountClass=?",
                                org, comp, nz(d.AccountId), voucherDate, 3, 3);
                        double gl = g.isEmpty() || g.get(0).isEmpty() ? 0d : toDouble(g.get(0).values().iterator().next());
                        if (gl > 0) {
                            /* vd carries the grid AccountTitle; the WHT line vd3 has none (''). */
                            String title = "False".equals(d.IsTaxable) ? detailTitles.getOrDefault(nz(d.AccountId), "") : "";
                            throw new ConfirmationRequiredException("The account '" + title
                                    + "' already has a debit ledger balance with an amount of " + num(gl)
                                    + ". Do you want to add the payment again?", "glBalance:" + nz(d.AccountId));
                        }
                    }
                }
            }
        } else {
            // ReceiptsVoucherNew.Insert()
            vh.Remarks = dto.Remarks == null ? "" : dto.Remarks.trim();
            vh.RemarksOtherLingo = "";
            vh.ChequeDate = isoDay(dto.ChequeDate);        // CheqDate.Value - set on CRV too (hidden picker)
            vh.ChequeNo = dto.ChequeNo == null ? "" : dto.ChequeNo;
            vh.PayTitle = dto.PayTitle == null ? "" : dto.PayTitle.trim();
            vh.CheqId = 0;                                  // never assigned by this form
            boolean auto = doc == 4 && Boolean.TRUE.equals(f.get("autoRemarksForReceiptsThroughBank"));
            int lineId = 1;
            voucherAmount = 0d;
            for (int i = 0; i < count; i++) {
                PaymentReceiptVoucherDto.Row r = rows.get(i);
                double amount = nzd(r.Amount);
                String rowRemarks = r.Remarks == null ? "" : r.Remarks;
                String acTitle = detailTitles.getOrDefault(nz(r.AccountId), "");
                String ptText = paymentTypes.getOrDefault(nz(r.PaymentTypeId), "");
                int branch = branchFeature ? nz(r.BranchId) : branchId;
                double fcy = (rate > 0 && amount > 0) ? amount / rate : 0d;   // Add_Click_1 FcyAmount cell
                String remarksAuto = "";

                ContraVoucherDto.Detail vd = new ContraVoucherDto.Detail();
                if (doc == 4) {
                    if (auto) {
                        remarksAuto = vh.Remarks + "  CHEQUE NO: " + vh.ChequeNo + "   RECEIVED FROM   " + acTitle
                                + "     DEPOSITED INTO   " + headerTitle.trim();
                        vd.Comments = remarksAuto;
                        if (count == 1) vh.Remarks = remarksAuto;
                    } else if (!vh.Remarks.isEmpty()) {
                        vd.Comments = vh.Remarks;
                    } else {
                        vd.Comments = rowRemarks;
                    }
                } else if (!vh.Remarks.isEmpty()) {
                    vd.Comments = vh.Remarks;
                } else {
                    vd.Comments = rowRemarks;
                }
                vd.LineId = lineId;
                vd.SortNo = i + 1;
                vd.PaymentTypeId = nz(r.PaymentTypeId);
                vd.PaymentType = ptText;
                vd.AccountId = nz(dto.RefAccountId);
                vd.AgainstAccountId = nz(r.AccountId);
                vd.JobLotId = nz(r.JobLotId);
                vd.DebitAmount = amount;
                vd.IsTaxable = "False";
                vd.InvoiceNoRefId = vh.CheqId;
                vd.CheqNoDetail = vh.ChequeNo;
                vd.DCheqDate = vh.ChequeDate;
                vd.DMultiCurrencyId = nz(dto.MultiCurrencyId);
                vd.BranchesId = branch;
                vd.CostCenterId = nz(r.CostCenterId);
                vd.DExchangeCurrencyRate = rate;
                vd.DCurrencyAmount = fcy;
                voucherAmount += amount;
                if (count == 1 && vh.Remarks.isEmpty()) vh.Remarks = vd.Comments;
                details.add(vd);
                lineId++;

                ContraVoucherDto.Detail vd2 = new ContraVoucherDto.Detail();
                vd2.Comments = (doc == 4 && auto) ? remarksAuto : rowRemarks;
                vd2.LineId = lineId;
                vd2.SortNo = i + 1;
                vd2.PaymentTypeId = nz(r.PaymentTypeId);
                vd2.PaymentType = ptText;
                vd2.AccountId = nz(r.AccountId);
                vd2.AgainstAccountId = nz(dto.RefAccountId);
                vd2.JobLotId = nz(r.JobLotId);
                vd2.CreditAmount = amount;
                vd2.IsTaxable = "False";
                vd2.InvoiceNoRefId = vh.CheqId;
                vd2.CheqNoDetail = vh.ChequeNo;
                vd2.DCheqDate = vh.ChequeDate;
                vd2.DMultiCurrencyId = nz(dto.MultiCurrencyId);
                vd2.DExchangeCurrencyRate = rate;
                vd2.DCurrencyAmount = fcy;
                vd2.BranchesId = branch;
                vd2.CostCenterId = nz(r.CostCenterId);
                details.add(vd2);
                lineId++;

                if (nz(r.CostCenterId) > 0) {
                    ContraVoucherDto.CostCentre c = new ContraVoucherDto.CostCentre();
                    c.SortNo = i + 1;
                    c.CostCenterId = nz(r.CostCenterId);
                    c.costPrcent = new BigDecimal("100");
                    c.costAmount = amount;
                    costCentres.add(c);
                }
            }
            if (vh.IncludeWHT) {
                String comments = "Tax Name    " + nzs(dto.TaxTypeName) + "   Tax %   " + nzs(dto.TaxPercent).trim();
                ContraVoucherDto.Detail vd3 = whtLine(lineId + 1, vh.AgainstAccountId, vh.RefDocNoId, dto, comments);
                vd3.DebitAmount = taxAmount;
                details.add(vd3);
                ContraVoucherDto.Detail vd4 = whtLine(lineId + 1, vh.RefDocNoId, vh.AgainstAccountId, dto, comments);
                vd4.CreditAmount = taxAmount;
                details.add(vd4);
            }
            vh.VoucherAmount = voucherAmount;
            /* ReceiptsVoucherNew.Insert() has no negative-balance, duplicate or GL-balance check. */
        }

        int newId = writer.save(vh, details, costCentres);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", newId);
        res.put("voucherCode", vh.VoucherCode);
        res.put("voucherAmount", vh.VoucherAmount);
        res.put("detailLines", details.size());
        res.put("costCentreLines", costCentres.size());
        res.put("message", (insert ? "Voucher Save Successfully...[" : "Voucher Update Successfully...[") + vh.VoucherCode + "]");
        return res;
    }

    private static ContraVoucherDto.Detail whtLine(int lineId, int accountId, int againstId,
                                                   PaymentReceiptVoucherDto dto, String comments) {
        ContraVoucherDto.Detail d = new ContraVoucherDto.Detail();
        d.LineId = lineId;
        d.AccountId = accountId;
        d.AgainstAccountId = againstId;
        d.TaxTypeId = nz(dto.TaxTypeId);
        d.IsTaxable = "True";
        d.Comments = comments;
        d.TaxPrcnt = toDouble(dto.TaxPercent);
        d.TaxesTotalAmount = nzd(dto.TaxAmount);
        return d;
    }

    /** PaymentVoucherNew.BuildRemarks(): {DebitRemarks, CreditRemarks}. */
    private static String[] buildRemarks(Map<String, Boolean> f, String vhRemarks,
                                         PaymentReceiptVoucherDto.Row row, String creditAccount) {
        String chequeNo = display(row.ChequeNo);
        String detail = row.Remarks == null ? "" : row.Remarks.trim();
        if (!f.get("autoRemarksForPaymentThroughBank")) {
            String o = !"None".equals(chequeNo) ? ("CHEQUE: (" + chequeNo + "), " + detail).trim() : detail;
            return new String[] {o, o};
        }
        String cd = isoDay(row.ChequeDate);
        String chequeDate = "None";
        if (cd != null && cd.length() == 10) {       // ToShortDateString, en-US: M/d/yyyy
            chequeDate = Integer.parseInt(cd.substring(5, 7)) + "/" + Integer.parseInt(cd.substring(8, 10)) + "/" + cd.substring(0, 4);
        }
        String payTitle = display(row.PayeeTitle);
        String account = creditAccount == null || creditAccount.trim().isEmpty() ? "None" : creditAccount.trim();
        List<String> debit = new ArrayList<>(), credit = new ArrayList<>();
        if (f.get("autoRemarksIncludeChequeDateAndNumber")) {
            String t = "CHEQUE: (" + chequeNo + ", " + chequeDate + ")"; debit.add(t); credit.add(t);
        } else if (f.get("autoRemarksIncludeChequeNumber")) {
            String t = "CHEQUE: (" + chequeNo + ")"; debit.add(t); credit.add(t);
        }
        if (f.get("autoRemarksIncludePayeeTitle")) {
            String t = "PayTitle: (" + payTitle + ")"; debit.add(t); credit.add(t);
        }
        debit.add("Credit Account: " + account);
        String da = String.join(", ", debit), ca = String.join(", ", credit);
        String header = vhRemarks == null ? "" : vhRemarks.trim();
        String hl = header.toLowerCase();
        if (hl.contains("cheque:") || hl.contains("paytitle:") || hl.contains("credit account:")) header = "";
        String d = f.get("autoRemarksIncludeHeaderRemarks") ? (header + " " + da).trim() : da;
        String c = f.get("autoRemarksIncludeHeaderRemarks") ? (header + " " + ca).trim() : ca;
        if (f.get("autoRemarksIncludeDetailRemarks") && !detail.isEmpty()) {
            d = (detail + " " + d).trim();
            c = (detail + " " + c).trim();
        }
        return new String[] {d, c};
    }

    private static String display(String v) {
        String t = v == null ? "" : v.trim();
        return t.isEmpty() ? "None" : t;
    }

    /** BLL VoucherExistWithSameAmountInSameDate: first line of each AccountId among lines with a debit. */
    private void duplicateGuard(List<ContraVoucherDto.Detail> details, Set<String> ack, int org, int comp, String date) {
        if (ack.contains("duplicate")) return;
        Set<Integer> seen = new LinkedHashSet<>();
        for (ContraVoucherDto.Detail d : details) {
            if (nzd(d.DebitAmount) <= 0) continue;
            if (!seen.add(nz(d.AccountId))) continue;
            /* The BLL asks once per account (Yes moves on to the next one): the answer is per account. */
            if (ack.contains("duplicate:" + nz(d.AccountId))) continue;
            String title = writer.duplicateVoucherTitle(org, comp, date, nz(d.AccountId), nzd(d.DebitAmount), nz(d.ActionId));
            if (title != null) {
                throw new ConfirmationRequiredException("Voucher against '" + title
                        + "' with same Debit Amount already exists on this date. Do you want to continue?", "duplicate:" + nz(d.AccountId));
            }
        }
    }

    // ================================================================================ validation

    private void validate(int doc, PaymentReceiptVoucherDto dto, Map<String, Object> f, boolean multiCurrency,
                          boolean branchFeature, Map<Integer, String> headerTitles, Map<Integer, String> detailTitles,
                          Map<Integer, String> paymentTypes) {
        boolean pay = isPayment(doc);
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");

        // FormValidation()
        if (nz(dto.ProjectId) == 0) throw new IllegalArgumentException("Cost Center Field is Required");
        if (nz(dto.RefAccountId) == 0 || !headerTitles.containsKey(nz(dto.RefAccountId))) {
            throw new IllegalArgumentException(pay ? "Credit Account Field is Required" : "Account Field is Required");
        }
        if (multiCurrency) {
            if (nz(dto.MultiCurrencyId) == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (nzd(dto.ExchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Exchange Rate Field is Required");
            if (nzd(dto.FcAmount) == 0d) throw new IllegalArgumentException("Fcy Amount Field is Required");
        } else {
            if (nz(dto.MultiCurrencyId) == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (nzd(dto.ExchangeCurrencyRate) == 0d) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }

        // WHT checks inside Insert()
        if (Boolean.TRUE.equals(dto.IncludeWHT)) {
            if (nz(dto.TaxTypeId) == 0) throw new IllegalArgumentException(pay ? "TaxType Field is Required" : "TaxType Account Field is Required");
            if (toDouble(dto.TaxPercent) == 0d) throw new IllegalArgumentException("Tax Percent Field is Required");
            if (nz(dto.AgainstAcId) == 0) {
                throw new IllegalArgumentException(pay ? "Withholding Debit Account Field is Required" : "Withholding Credit Account Field is Required");
            }
            if (nz(dto.WithHoldingAcId) == 0) {
                throw new IllegalArgumentException(pay ? "Withholding Credit Account Field is Required" : "Withholding Debit Account Field is Required");
            }
        }
        for (PaymentReceiptVoucherDto.Row r : dto.rows) {
            if (nzd(r.Amount) > 0 && nz(r.AccountId) == 0) throw new IllegalArgumentException("Please Select Account Title First");
        }

        // The row-add checks (FormValidationDetail / Add_Click_1): a row that lacks them was not built by the screen.
        boolean compulsory = Boolean.TRUE.equals(f.get("chequeNoCompulsoryOnBpv"));
        for (PaymentReceiptVoucherDto.Row r : dto.rows) {
            if (!paymentTypes.containsKey(nz(r.PaymentTypeId))) {
                throw new IllegalArgumentException(pay ? "PaymentType Field is Required" : "PaymentType Field Required");
            }
            if (nz(r.AccountId) == 0 || !detailTitles.containsKey(nz(r.AccountId))) {
                throw new IllegalArgumentException(pay ? "Debit Account Field is Required" : "Credit Account Field Required");
            }
            if (nz(r.JobLotId) == 0) throw new IllegalArgumentException(pay ? "Job/Lot Field is Required" : "Job/Lot Field Required");
            if (nzd(r.Amount) == 0d) throw new IllegalArgumentException(pay ? "Amount Field is Required" : "Amount Field Required");
            if (doc == 2) {
                if (nz(r.FinancialInstrumentId) == 0) throw new IllegalArgumentException("Financial Instrument Field is Required");
                if (nz(r.FinancialInstrumentId) == 1 && compulsory && nz(r.ChequeId) == 0) {
                    throw new IllegalArgumentException("Cheque_number Field is Required");
                }
                if (nz(r.FinancialInstrumentId) == 1 && nz(r.ChequeTypeId) == 0) {
                    throw new IllegalArgumentException("Cheque Type Field is Required");
                }
            }
            if (branchFeature && nz(r.BranchId) == 0) throw new IllegalArgumentException("BranchName Field is Required");
        }

        if (pay && Boolean.TRUE.equals(dto.IncludeWHT)) {
            Set<Integer> accounts = new HashSet<>();
            for (PaymentReceiptVoucherDto.Row r : dto.rows) accounts.add(nz(r.AccountId));
            if (accounts.size() > 1) {
                throw new IllegalArgumentException("There are multi accounts in detail so can't apply Wht.\n You can only Apply Wht On Single Account in Detail");
            }
            if (nz(dto.rows.get(0).AccountId) != nz(dto.AgainstAcId)
                    && (dto.acknowledged == null || !dto.acknowledged.contains("whtMismatch"))) {
                throw new ConfirmationRequiredException("WHT Debit A/C not match with detail Debit A/C. Are You Sure To Proceed?", "whtMismatch");
            }
        }
    }

    // ================================================================================ rights

    /** CommonServices.SetRightsValueInRightsObject(ScreenName) with its Admin short-circuit. */
    public Map<String, Boolean> rights(int doc) {
        Map<String, Boolean> r = new LinkedHashMap<>();
        for (String n : new String[] {"Save", "Update", "Print", "Delete"}) r.put(n.toLowerCase(), hasRight(doc, n));
        return r;
    }

    private boolean hasRight(int doc, String rightName) {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equals(role)) return true;
        /* SetRightsValueInRightsObject maps "Delete" to DoHaveCanDelete from RightName "Delete". */
        try {
            for (Map<String, Object> row : jdbcTemplate.queryForList(SQL_USER_RIGHTS, currentUserContext.currentUserId(),
                    screenName(doc), role == null ? "" : role, currentUserContext.currentCompanyId(), "GetByUserId")) {
                Object n = ci(row, "RightName");
                if (n != null && rightName.equals(String.valueOf(n).trim())) return truthy(ci(row, "Value"));
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, screenName(doc), e);
        }
        return false;
    }

    // ================================================================================ small helpers

    /** A desktop Yes/No the operator has to answer before the save can proceed. */
    public static class ConfirmationRequiredException extends RuntimeException {
        public final String kind;
        public ConfirmationRequiredException(String message, String kind) { super(message); this.kind = kind; }
    }

    private static double round(double v, int decimals) {
        return BigDecimal.valueOf(v).setScale(Math.max(decimals, 0), RoundingMode.HALF_UP).doubleValue();
    }

    /** C# default double.ToString(): no trailing zeros. */
    private static String num(double d) {
        if (d == 0d) return "0";
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }
    private static String nzs(String v) { return v == null ? "" : v; }
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static String dateTimeStr(Object v) {
        if (v instanceof java.sql.Timestamp) {
            return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        }
        return v == null ? null : String.valueOf(v);
    }

    /** A DATETIME column as yyyy-MM-dd (Jackson would otherwise send epoch millis). */
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
