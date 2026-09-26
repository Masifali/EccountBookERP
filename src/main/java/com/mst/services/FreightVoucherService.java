package com.mst.services;

import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.FreightVoucherDto;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Types;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Screen "Freight Payment Voucher" — {@code Architecture.WinApp.Account_Definition.FreightVoucher}
 * (FreightVoucher.cs, 4482 lines), DocumentTypeId 28, ScreenName "FreightVoucher".
 *
 * <p>Every read is the procedure the desktop's BLL calls, with the same parameters sent under
 * the same conditions. The write is {@code BLL.Inventory.InvFreightVoucher.Save} (BLL 0538) →
 * {@code DAL.Inventory.InvFreightVoucher.SetData} (DAL 0393), step for step:
 *
 * <pre>
 *   0  driverBiodataId == 0      -> [dbo].[USP_DriverBiodata_InsertIfNotExists]  (returns id)
 *   1  Sp_InvFreightVoucher_Insert | Sp_InvFreightVoucher_Update               (Update deletes
 *                                   the old detail and break-up rows itself)
 *   2  per grid row               -> Sp_InvFreightVoucherDetail_Insert
 *   3  per break-up row           -> USP_InvFreightVoucherPaymentBreakup_Insert
 *   4  SUM(TotalPayableAmount) > 0:
 *        Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId'
 *        Sp_VoucherHead_Insert | Sp_VoucherHead_Update (Update deletes the old lines)
 *        Sp_VoucherDetail_Insert per line
 *        Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert per line
 * </pre>
 *
 * <p>Unlike the generic voucher DAL (0586) this chain has NO USP_VoucherBalanceCheck and NO
 * document-approval row. That is DAL 0393 as written, and it is reproduced, not "completed".
 *
 * <p>Attachments ({@code FormHelper.UpdateAttachmentsForObject}, Proc_DMSAttachments_Insert) are
 * not part of this port: the page has no attachment store, so it sends none, and with an empty
 * AttachmentsList the DAL skips that block entirely — exactly the path taken here.
 */
@Service
public class FreightVoucherService {

    private static final Logger LOG = LoggerFactory.getLogger(FreightVoucherService.class);

    /** FreightVoucher.cs:351 */
    public static final int DOCUMENT_TYPE_ID = 28;
    /** FreightVoucher.cs:352 */
    private static final String SCREEN_NAME = "FreightVoucher";

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;

    /** A refusal in the desktop's own words; the controller answers 400. */
    public static class Refusal extends RuntimeException {
        public Refusal(String m) { super(m); }
    }

    // ============================================================================ page load

    /**
     * InitializeComponentMethod (:415) + GetConfigurationsFromGlobal (:468): rights, the five
     * configuration values, the next document number, the pending gate passes and the Register
     * combos, in one round trip. The account, city and driver lists are the desktop's global
     * caches (clsGlobalVariables) and are served by their own endpoints.
     */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights());
        out.put("config", config());
        out.put("documentNo", generateCode());
        out.put("pending", pending());
        out.put("historyCombos", historyCombos());
        return out;
    }

    /** CommonServices.SetRightsValueInRightsObject("FreightVoucher") — Admin short-circuit :17581. */
    public Map<String, Object> rights() {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("canSave", hasRight("Save"));
        r.put("canUpdate", hasRight("Update"));
        r.put("canPrint", hasRight("Print"));
        return r;
    }

    /** GetConfigurationsFromGlobal (:468) — every value read by ConfigDescription. */
    public Map<String, Object> config() {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("DefaultFreightVoucherCreditAccountId", toInt(configValue("DefaultFreightVoucherCreditAccountId")));
        c.put("DefaultDaysToLessFromHistoryFromDate", toInt(configValue("DefaultDaysToLessFromHistoryFromDate")));
        c.put("TolerancePercentForDiscountonFreightVoucher", toDouble(configValue("TolerancePercentForDiscountonFreightVoucher")));
        c.put("IncludeBankAccountsInFreightVoucherCreditAccount", toBool(configValue("IncludeBankAccountsInFreightVoucherCreditAccount")));
        c.put("ChequeBookEnableStatus", toBool(configValue("CheqBook Enabled")));
        return c;
    }

    /** VoucherNoDbCall (:526) -> InvFreightVoucher.GenerateCode -> @Activity='GenerateCode'. */
    public int generateCode() {
        List<Map<String, Object>> rows = query(
                "EXEC dbo.Sp_InvFreightVoucher_GetAllMethod @OrganizationId=?, @CompanyId=?, "
              + "@DocumentTypeId=?, @FinancialYearId=?, @Activity=?",
                org(), comp(), DOCUMENT_TYPE_ID, fy(), "GenerateCode");
        return rows.isEmpty() ? 0 : toInt(col(rows.get(0), "DocumentNo"));
    }

    /** PendingRecordDbCall (:967) -> GatePassInward.GetGpDataForFreightVoucher. */
    public List<Map<String, Object>> pending() {
        return query("EXEC dbo.usp_getGpDataForFreightVoucher @OrganizationId=?, @CompanyId=?", org(), comp());
    }

    /** HistoryComboBind (:492) <- InvFreightVoucher.GetDataForDropDownFromFreightVoucher(org, comp, null). */
    public List<Map<String, Object>> historyCombos() {
        return query("EXEC [dbo].[USP_GetDataForDropDownFromFreightVoucher] @OrganizationId=?, @CompanyId=?",
                org(), comp());
    }

    /**
     * clsGlobalVariables.AllAccountsWithCustomGroupId <- GlobalServicesMethods
     * .GetGlobalAllAccountsWithCustomGroup(org, comp) (BLL 0379:609) — PageSize / PageNumber are
     * 0, so neither is sent. Projected to the columns the three binders read
     * (CashAccountFillFromGlobal, DebitAccountFillFromGlobal, BreakUp.BindAccounts). Rows are NOT
     * de-duplicated here: each binder de-duplicates by ChartOfAccountId after its own filter,
     * and DebitAccountFillFromGlobal filters on CustomGroupId, which a per-account dedupe would lose.
     */
    public List<Map<String, Object>> globalAccounts() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : query("EXEC [dbo].[USP_GETAllAccountsFromCustomGroups] @OrganizationId=?, @CompanyId=?",
                org(), comp())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(col(r, "ChartOfAccountId")));
            m.put("AccountTitle", str(col(r, "AccountTitle")));
            m.put("AccountCode", str(col(r, "AccountCode")));
            m.put("ParentAccountTitle", str(col(r, "ParentAccountTitle")));
            m.put("AccountTypeId", toInt(col(r, "AccountTypeId")));
            m.put("AccountType", str(col(r, "AccountType")));
            m.put("AccountClassName", str(col(r, "AccountClassName")));
            m.put("CustomGroupId", toInt(col(r, "CustomGroupId")));
            out.add(m);
        }
        return out;
    }

    /** CityDtFillFromGlobalAndBind (:619) <- getGlobalAllCity -> USP_City_GetAllWithCountryAndTehsil (Id, CityName). */
    public List<Map<String, Object>> cities() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : query("EXEC [dbo].[USP_City_GetAllWithCountryAndTehsil] @OrganizationId=?, @CompanyId=?",
                org(), comp())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(col(r, "Id")));
            m.put("Description", str(col(r, "CityName")));
            out.add(m);
        }
        return out;
    }

    /** clsGlobalVariables.globalAllDriverBioInfo <- driverBiodata.ReadAll (BLL 0378:252). */
    public List<Map<String, Object>> drivers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : query("EXEC [dbo].[USP_driverBiodata_GetAllMethod] @OrganizationId=?, @CompanyId=?, @Activity=?",
                org(), comp(), "ReadAll")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(col(r, "driverBiodataId")));
            m.put("DriverName", str(col(r, "driverName")));
            m.put("CnicNo", str(col(r, "cnicNo")));
            m.put("DriverCellNo", str(col(r, "cellNo")));
            m.put("WhatsappNo", str(col(r, "whatsappNo")));
            m.put("AlternateCellNo", str(col(r, "alternateCellNo")));
            m.put("FatherName", str(col(r, "fatherName")));
            m.put("FatherCnicNo", str(col(r, "fatherCnicNo")));
            out.add(m);
        }
        return out;
    }

    /**
     * AccountCurrentBalance (:645) — VoucherHead.ReadByCurrentBalanceByDateAndAccountId with
     * VoucherDate = DateTime.Now; the Balance column of the first row.
     */
    public Map<String, Object> balance(int accountId) {
        List<Map<String, Object>> rows = query(
                "EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, "
              + "@VoucherDate=?, @RefAccountId=?, @Activity=?",
                org(), comp(), fy(), now(), accountId, "ReadByCurrentBalanceByDateAndAccountId");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("found", !rows.isEmpty());
        m.put("balance", rows.isEmpty() ? 0d : toDouble(col(rows.get(0), "Balance")));
        return m;
    }

    /** PreviousCityDataBind (:865) — getLastFreightInfoByCityId(companyId, cityId, 5, RecId). */
    public List<Map<String, Object>> previousCity(int cityId, int recId) {
        List<String> n = new ArrayList<>(Arrays.asList("@CompanyId", "@CityId", "@LastRecordCounter"));
        List<Object> v = new ArrayList<>(Arrays.asList(comp(), cityId, 5));
        if (recId != 0) { n.add("@FreightRecId"); v.add(recId); }
        return query(exec("[dbo].[usp_getLastFreightInfoByCityId]", n), v.toArray());
    }

    /** FreightVoucherPaymentBreakUp.GetInstrumentTypes -> VoucherHead.GetInstrumentTypes. */
    public List<Map<String, Object>> instrumentTypes() {
        return query("EXEC dbo.usp_getInstrumentType");
    }

    /**
     * FreightVoucherPaymentBreakUp.CheqNoFill (:498) -> CheqBookHeader.OutstandingCheqNo
     * (BLL 0647:61): @RecId only when the voucher id is non-zero; ActionId is never set by the
     * caller, so @Id is never sent.
     */
    public List<Map<String, Object>> cheques(int bankId, int voucherId) {
        List<String> n = new ArrayList<>(Arrays.asList("@OrganizationId", "@CompanyId", "@BankId"));
        List<Object> v = new ArrayList<>(Arrays.asList(org(), comp(), bankId));
        if (voucherId != 0) { n.add("@RecId"); v.add(voucherId); }
        n.add("@MethodType"); v.add("OutstandingCheqNo");
        return query(exec("SP_CheqBookHeader_GetAllMethod", n), v.toArray());
    }

    // ============================================================================ register

    /** btnshow_Click (:1716) -> InvFreightVoucher.FreightVoucherSlipAndRegister (BLL 0538). */
    public List<Map<String, Object>> register(FreightVoucherDto.RegisterFilter f) {
        List<String> n = new ArrayList<>(Arrays.asList("@OrganizationId", "@CompanyId", "@DocumentTypeId", "@FinancialYearId"));
        List<Object> v = new ArrayList<>(Arrays.asList(org(), comp(), DOCUMENT_TYPE_ID, fy()));
        if (notBlank(f.fromDate)) { n.add("@FromDate"); v.add(f.fromDate); }
        if (notBlank(f.toDate))   { n.add("@ToDate");   v.add(f.toDate); }
        if (nz(f.gpNoFrom) != 0)  { n.add("@GpSrNoF"); v.add(f.gpNoFrom); }
        if (nz(f.gpNoTo) != 0)    { n.add("@GpSrNoT"); v.add(f.gpNoTo); }
        if (nz(f.supplierCustomerId) != 0) { n.add("@SupplierCustomerId"); v.add(f.supplierCustomerId); }
        if (nz(f.creditAccountId) != 0)    { n.add("@CreditAccountId"); v.add(f.creditAccountId); }
        if (nz(f.debitAccountId) != 0)     { n.add("@DebitAccountId"); v.add(f.debitAccountId); }
        if (notBlank(f.vehicleNo)) { n.add("@VehicleNo"); v.add(f.vehicleNo); }
        if (Boolean.TRUE.equals(f.onlyDiscountedRows)) { n.add("@FreightDiscountAmount"); v.add(1.0d); }
        if (Boolean.TRUE.equals(f.freightAuditByCity)) { n.add("@ActionId"); v.add(1); }
        return dropBinary(query(exec("SP_FreightVoucherSlipAndRegister", n), v.toArray()));
    }

    /** CommonServices.FreightVoucherSlip241(PrintId) (:5602) — DocumentTypeId 28, Id = PrintId. */
    public List<Map<String, Object>> slip241(int id) {
        return dropBinary(query(
                "EXEC SP_FreightVoucherSlipAndRegister @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, "
              + "@FinancialYearId=?, @Id=?",
                org(), comp(), DOCUMENT_TYPE_ID, fy(), id));
    }

    /**
     * CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(VoucherHeadId) (:5841) ->
     * VoucherReports.NewVoucherReprot (BLL 0141:201).
     */
    public List<Map<String, Object>> voucher102(int voucherHeadId) {
        if (voucherHeadId == 0) throw new Refusal("VoucherId Not Found");
        return dropBinary(query(
                "EXEC SpVouchers_PaymentReceiptVoucherSlipNew_Rpt @OrganizationId=?, @CompanyId=?, @UserId=?, @Id=?",
                org(), comp(), ctx.currentUserId(), voucherHeadId));
    }

    // ============================================================================ read by id

    /**
     * ReadById (:1485) <- InvFreightVoucher.GetByID -> DAL.GetDate: ReadById, then ReadByHeaderId
     * and PaymentBreakup_ReadByHeaderId for the details; plus CommonServices.VoucherHeadIdGet.
     *
     * Decimal columns go out as their plain text (12500.00, not 12500) because the form puts
     * {@code Conversion.ToString(obj.SupplierWeight)} into the textbox and later builds the
     * detail remarks from that text.
     */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> head = query(
                "EXEC dbo.Sp_InvFreightVoucher_GetAllMethod @Id=?, @Activity=?", id, "ReadById");
        if (head.isEmpty()) throw new Refusal("Record not found.");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", textify(head.get(0)));
        out.put("details", query("EXEC dbo.Sp_InvFreightVoucher_GetAllMethod @Id=?, @Activity=?", id, "ReadByHeaderId"));
        out.put("breakUps", query("EXEC dbo.Sp_InvFreightVoucher_GetAllMethod @Id=?, @Activity=?", id, "PaymentBreakup_ReadByHeaderId"));
        out.put("voucherHeadId", voucherHeadId(id));
        return out;
    }

    private int voucherHeadId(int documentId) {
        List<Map<String, Object>> rows = query(
                "EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, "
              + "@DocumentTypeId=?, @DocumentTypeSrNo=?",
                "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                org(), comp(), DOCUMENT_TYPE_ID, documentId);
        return rows.isEmpty() ? 0 : toInt(col(rows.get(0), "Id"));
    }

    // ============================================================================ save

    /**
     * Save_Click / Update_Click -> Insert() (:1258). The page asks the operator's Yes/No
     * questions (the save confirmation and the freight-per-MTon warning) before posting; every
     * check that REFUSES is repeated here in the same order and the same words.
     */
    @Transactional
    public Map<String, Object> save(FreightVoucherDto d) {
        int recId = nz(d.id);
        if (recId == 0 && !hasRight("Save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !hasRight("Update")) throw new IllegalStateException("You do not have the Update right for this screen.");

        List<FreightVoucherDto.Row> rows = d.rows == null ? new ArrayList<>() : d.rows;
        List<FreightVoucherDto.BreakUp> breakUps = d.breakUps == null ? new ArrayList<>() : d.breakUps;

        /* :1270 */
        if (rows.isEmpty()) throw new Refusal("Grid Record Not Found");
        /* :1274 */
        if (str(d.driverCellNo).trim().isEmpty() || !Boolean.TRUE.equals(d.driverCellMaskFull))
            throw new Refusal("Cell No. Field Required");
        /* :1279 — the desktop's own double space */
        if (str(d.driverName).trim().isEmpty()) throw new Refusal("Driver Name  Field Required");
        /* :1284 */
        if (breakUps.isEmpty() && nz(d.cashAccountId) == 0) throw new Refusal("Cash Account Field Required");
        /* :1291 FormHelper.ValidateControls */
        requireNonZeroInt(d.gpNo, "GpNo");
        requireNonZeroInt(d.biltyFreight, "BiltyFreight");
        requireNonZeroDouble(d.supplierWeight, "SupplierWeight");
        requireNonZeroDouble(d.factoryWeight, "FactoryWeight");

        Map<String, Object> cfg = config();
        double tolerancePct = (Double) cfg.get("TolerancePercentForDiscountonFreightVoucher");
        boolean chequeBook = (Boolean) cfg.get("ChequeBookEnableStatus");

        /* :1301 */
        double sumDiscount = 0d, shortageTotal = 0d, sumPaid = 0d;
        for (FreightVoucherDto.Row r : rows) {
            sumDiscount += nzd(r.discountAmount);
            shortageTotal += nzd(r.shortageAmount);
            sumPaid += nzd(r.paidAmount);
        }
        double toleranceAmount = shortageTotal * tolerancePct / 100.0;
        if (sumDiscount > toleranceAmount)
            throw new Refusal("Total Discount Amount " + cs(sumDiscount) + " cannot be greater than Tolerance Amount "
                    + cs(toleranceAmount) + " for Shortage Amount " + cs(shortageTotal) + ".");
        /* :1308 */
        double allowShortage = toDouble(d.allowShortage);
        double shortWeight = toDouble(d.shortWeight);
        double toleranceWeight = shortWeight * tolerancePct / 100.0;
        if (allowShortage > toleranceWeight)
            throw new Refusal("Total Allow shortage " + cs(allowShortage) + " cannot be greater than Tolerance Weight "
                    + cs(toleranceWeight) + " for Short weight " + cs(shortWeight) + ".");

        int userId = ctx.currentUserId();
        String now = now();

        /* :1340 — CashAccountId: the combo when there is no break-up, the break-up's account
           when every break-up row shares one, otherwise 0. */
        Set<Integer> distinctBreakUpAccounts = new LinkedHashSet<>();
        for (FreightVoucherDto.BreakUp b : breakUps) distinctBreakUpAccounts.add(nz(b.accountTitleId));
        int cashAccountId = breakUps.isEmpty() ? nz(d.cashAccountId)
                : (distinctBreakUpAccounts.size() <= 1 ? nz(breakUps.get(0).accountTitleId) : 0);

        BigDecimal totalPaidAmount = BigDecimal.valueOf(sumPaid);
        BigDecimal differenceWeight = dec(d.shortWeight);

        /* :1369 — the detail list and its per-row refusals */
        List<Map<String, Object>> details = new ArrayList<>();
        for (int i = 0; i < rows.size(); i++) {
            FreightVoucherDto.Row r = rows.get(i);
            Map<String, Object> vd = new LinkedHashMap<>();
            int biltyFreightInt = toIntRounded(r.biltyFreight);
            double addLess = nzd(r.discountAmount);
            BigDecimal chargeToParty = BigDecimal.valueOf(nzd(r.chargeToParty));
            BigDecimal deducted = BigDecimal.valueOf(nzd(r.deductionAmount));
            BigDecimal shortage = BigDecimal.valueOf(nzd(r.shortageAmount));
            BigDecimal paid = BigDecimal.valueOf(nzd(r.paidAmount));
            vd.put("Id", recId != 0 ? nz(r.id) : 0);
            vd.put("FreightVoucherId", 0);
            vd.put("SortNo", i + 1);
            vd.put("ActionId", 0);
            vd.put("BiltyFreight", biltyFreightInt);
            vd.put("ScaleCharges", BigDecimal.valueOf(nzd(r.wbCharges)));
            vd.put("TotalPayableAmount", paid);
            vd.put("LessAny", BigDecimal.ZERO);      // never assigned by the form
            vd.put("RemarksDetail", remarksDetail(d, r, addLess, paid));
            vd.put("DebitAccountId", nz(r.debitAccountId));
            vd.put("AddLessAmount", addLess);
            vd.put("ShortageAmount", shortage);
            vd.put("DeductedAmount", deducted);
            vd.put("ChargeToParty", chargeToParty);
            vd.put("RateForShortage", BigDecimal.valueOf(nzd(r.rateForShortage)));
            vd.put("AdvanceByParty", nzd(r.advanceByParty));
            vd.put("AdvanceByFactory", nzd(r.advanceByFactory));
            vd.put("OtherDeduction", nzd(r.otherDeduction));
            vd.put("CustomGroupId", nz(r.freightCustomAccountsGroupId));

            /* :1376-1385 */
            if (nz(r.debitAccountId) == 0) throw new Refusal("Debit Account is required in Detail Grid at row No: " + (i + 1));
            if (biltyFreightInt == 0) throw new Refusal("BiltyFreight is required in Detail Grid at row No: " + (i + 1));
            double lhs = Math.rint(addLess) + Math.rint(chargeToParty.doubleValue()) + Math.rint(deducted.doubleValue());
            if (lhs > Math.rint(shortage.doubleValue()))
                throw new Refusal("Sum of 'DiscountAmount,ChargeToParty and DeductionAmount' cannot exceed 'ShortageAmount'. in Row No : " + (i + 1));
            if (paid.signum() < 0)
                throw new Refusal("TotalPayable Amount can not less than 0 in Row No : " + (i + 1));
            details.add(vd);
        }

        /* :1388 — the break-up list */
        List<Map<String, Object>> breakUpRows = new ArrayList<>();
        if (!breakUps.isEmpty()) {
            BigDecimal breakUpAmount = BigDecimal.ZERO;
            for (FreightVoucherDto.BreakUp b : breakUps) {
                Map<String, Object> m = new LinkedHashMap<>();
                BigDecimal amount = BigDecimal.valueOf(nzd(b.amount));
                m.put("Id", 0);
                m.put("InvFreightVoucherId", 0);
                m.put("TransTypeId", nz(b.transactionTypeId));
                m.put("InstrumentTypeId", nz(b.instrumentTypeId));
                m.put("AccountId", nz(b.accountTitleId));
                m.put("CheqId", chequeBook ? nz(b.chequeId) : 0);
                m.put("CheqDate", notBlank(b.chequeDate) ? dayStart(b.chequeDate) : now);
                m.put("CheqNo", str(b.chequeNo));
                m.put("PayeeTitle", str(b.payeeTitle));
                m.put("Amount", amount);
                m.put("Remarks", str(b.remarks));
                breakUpAmount = breakUpAmount.add(amount);
                breakUpRows.add(m);
            }
            /* :1406 — decimal equality. Both sides are rounded to the column's 4 places first so
               that binary double noise in the page's sums cannot fail an equal amount. */
            if (breakUpAmount.setScale(4, RoundingMode.HALF_EVEN).compareTo(totalPaidAmount.setScale(4, RoundingMode.HALF_EVEN)) != 0)
                throw new Refusal("BreakUp Amount:" + plain(breakUpAmount) + " not equal to TotalPaidAmount:" + plain(totalPaidAmount));
        } else {
            /* :1413 — one row from the credit account's own type */
            int accountTypeId = accountTypeId(nz(d.cashAccountId));
            boolean otherParty = accountTypeId == 3 || accountTypeId == 6 || accountTypeId == 8;
            int transTypeId = accountTypeId == 15 ? 2 : accountTypeId == 2 ? 1 : (otherParty ? 3 : 0);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", 0);
            m.put("InvFreightVoucherId", 0);
            m.put("TransTypeId", transTypeId);
            m.put("InstrumentTypeId", accountTypeId == 15 ? 3 : 0);
            m.put("AccountId", cashAccountId);
            m.put("CheqId", 0);
            m.put("CheqDate", now);
            m.put("CheqNo", "");
            m.put("PayeeTitle", null);
            m.put("Amount", totalPaidAmount);
            m.put("Remarks", null);
            breakUpRows.add(m);
        }

        /* ---------------------------------------------------------------- DAL 0393 SetData */

        int driverId = nz(d.driverBiodataId);
        if (driverId == 0) {
            Map<String, Object> drv = new LinkedHashMap<>();
            drv.put("driverBiodataId", 0);
            drv.put("driverName", d.driverName);
            drv.put("cnicNo", d.cnicNo);
            drv.put("cellNo", d.driverCellNo);
            drv.put("whatsappNo", d.whatsappNo);
            drv.put("alternateCellNo", d.alternateCellNo);
            drv.put("fatherName", d.fatherName);
            drv.put("fatherCnicNo", d.fatherCnicNo);
            drv.put("OrganizationId", org());
            drv.put("CompanyId", comp());
            drv.put("BranchId", branch());
            drv.put("EntryUserId", userId);
            drv.put("EntryDate", now);
            drv.put("ModifyDate", now);
            drv.put("ApprovedDate", now);
            driverId = scalar("[dbo].[USP_DriverBiodata_InsertIfNotExists]", drv);
        }

        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId);
        h.put("OrganizationId", org());
        h.put("CompanyId", comp());
        h.put("FinancialYearId", fy());
        h.put("BranchId", branch());
        h.put("ProjectId", branch());          // :1328 — ProjectId = BranchesId, as the form sets it
        h.put("EntryUserId", userId);
        h.put("EntryDate", now);
        h.put("ModifyUserId", userId);
        h.put("ModifyDate", now);
        h.put("IsApproved", false);
        h.put("ApprovedUserId", 0);
        h.put("ApprovedDate", now);
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("DocumentNo", toInt(d.documentNo));
        h.put("DocDate", docDateWithTime(d.docDate));
        h.put("GatePassNo", toInt(d.gpNo));
        h.put("GatePassDate", dayStart(d.gpDate));
        h.put("VehicleNo", str(d.vehicleNo));
        h.put("BiltyNo", str(d.biltyNo));
        h.put("SupplierWeight", dec(d.supplierWeight));
        h.put("FactoryWeight", dec(d.factoryWeight));
        h.put("DifferenceWeight", differenceWeight);
        h.put("ToleranceWeight", dec(d.toleranceWeight));
        h.put("RemarksHeader", str(d.remarksHeader).trim());
        h.put("CashAccountId", cashAccountId);
        h.put("GatePassId", toInt(d.gpId));
        h.put("TotalPaidAmount", totalPaidAmount);
        h.put("CityId", nz(d.cityId));
        h.put("AllowShortage", differenceWeight.signum() > 0 ? toDouble(d.allowShortage) : 0d);   // :1354
        h.put("ShortageApply", toDouble(d.shortageApply));
        h.put("driverBiodataId", driverId);
        h.put("ManualNo", str(d.manualNo));
        h.put("FreightOn", Boolean.TRUE.equals(d.creditAccount) ? 1 : 0);
        h.put("UserLogId", 0L);

        int returned = scalar(recId == 0 ? "Sp_InvFreightVoucher_Insert" : "Sp_InvFreightVoucher_Update", h);
        int id = returned > 0 ? returned : recId;

        for (Map<String, Object> vd : details) {
            vd.put("FreightVoucherId", id);
            scalar("Sp_InvFreightVoucherDetail_Insert", vd);
        }
        for (Map<String, Object> b : breakUpRows) {
            b.put("InvFreightVoucherId", id);
            scalar("USP_InvFreightVoucherPaymentBreakup_Insert", b);
        }

        /* BLL 0538 Save: MakeVoucher only when SUM(TotalPayableAmount) > 0, and the DAL writes
           the voucher under the same condition. */
        int voucherId = 0;
        if (totalPaidAmount.signum() > 0) {
            voucherId = writeVoucher(id, d, details, breakUpRows, cashAccountId, userId, now);
        }

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("voucherHeadId", voucherId);
        res.put("documentNo", toInt(d.documentNo));
        /* :1436 / :1440 — the form's own number in the message, not the one the proc assigned */
        res.put("message", (recId == 0 ? "Record Save Successfully...[" : "Record Update Successfully...[")
                + toInt(d.documentNo) + "]");
        return res;
    }

    /** BLL 0538 MakeVoucher + DAL 0393's voucher block. */
    private int writeVoucher(int id, FreightVoucherDto d, List<Map<String, Object>> details,
                             List<Map<String, Object>> breakUps, int cashAccountId, int userId, String now) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = id;
        vh.RefDocNoId = id;
        vh.RefAccountId = cashAccountId;
        vh.VoucherCode = toInt(d.documentNo);
        vh.VoucherDate = docDateWithTime(d.docDate);
        vh.Remarks = str(d.remarksHeader).trim();
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = today();
        vh.IncludeWHT = false;
        vh.BranchId = 0;
        vh.ProjectId = 0;
        vh.ManualBillNo = "";
        vh.DueDate = now;
        vh.OrganizationId = org();
        vh.CompanyId = comp();
        vh.FinancialYearId = fy();
        vh.EntryUser = userId;
        vh.EntryDate = now;
        vh.ModifyUser = userId;
        vh.ModifyDate = now;
        vh.ActionId = 0;       // MakeVoucher never sets it

        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
        double total = 0d;
        int lastDebitAccount = 0;
        String lastText = "";
        int breakUpCount = breakUps.size();
        for (Map<String, Object> vd : details) {
            lastDebitAccount = toInt(vd.get("DebitAccountId"));
            lastText = str(vd.get("RemarksDetail"));
            double paid = ((BigDecimal) vd.get("TotalPayableAmount")).doubleValue();
            double lessAny = ((BigDecimal) vd.get("LessAny")).doubleValue();
            double itemAmount = toInt(vd.get("BiltyFreight"));
            ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
            dr.AccountId = lastDebitAccount;
            dr.AgainstAccountId = cashAccountId;
            dr.Comments = lastText;
            dr.DebitAmount = paid;
            dr.RateCutAmount = lessAny;
            dr.ItemAmount = itemAmount;
            lines.add(dr);
            if (breakUpCount == 0) {
                ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
                cr.AccountId = cashAccountId;
                cr.AgainstAccountId = lastDebitAccount;
                cr.Comments = lastText;
                cr.CreditAmount = paid;
                cr.RateCutAmount = lessAny;
                cr.ItemAmount = itemAmount;
                lines.add(cr);
            }
            total += paid;
        }
        for (Map<String, Object> b : breakUps) {
            String remarks = str(b.get("Remarks"));
            ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
            cr.AccountId = toInt(b.get("AccountId"));
            cr.AgainstAccountId = lastDebitAccount;
            cr.Comments = remarks.trim().isEmpty() ? lastText : remarks;
            cr.CreditAmount = ((BigDecimal) b.get("Amount")).doubleValue();
            cr.InstrumentTypeId = toInt(b.get("InstrumentTypeId"));
            cr.InvoiceNoRefId = toInt(b.get("CheqId"));
            cr.CheqNoDetail = str(b.get("CheqNo"));
            cr.DCheqDate = str(b.get("CheqDate"));
            cr.PayeeTitle = str(b.get("PayeeTitle"));
            lines.add(cr);
        }
        vh.AgainstAccountId = lastDebitAccount;
        vh.VoucherAmount = total;
        vh.BillAmount = total;

        int existing = voucherHeadId(id);
        vh.Id = existing;
        int n2 = scalarObj(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        vh.Id = n2 > 0 ? n2 : existing;
        if (lines.isEmpty()) throw new Refusal("VoucherDetail List Not Found");
        for (ContraVoucherDto.Detail l : lines) {
            l.VoucherHeadId = vh.Id;
            scalarObj("Sp_VoucherDetail_Insert", l);
        }
        int documentTypeIdRef = scalarObj("Sp_VoucherHead_H_Insert", vh);
        for (ContraVoucherDto.Detail l : lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = documentTypeIdRef;
            scalarObj("Sp_VoucherDetail_H_Insert", l);
        }
        return vh.Id;
    }

    /**
     * FillDetailListCommonForInsertAndDelete (:1179) — the remark every detail row and voucher
     * line carries. detail.LessAny is never assigned by the form, so its "ShortageAmount" clause
     * can never print; it is left out for that reason, not by oversight.
     */
    private static String remarksDetail(FreightVoucherDto d, FreightVoucherDto.Row r, double addLess, BigDecimal paid) {
        StringBuilder s = new StringBuilder();
        s.append("Freight of ").append(str(r.itemQty)).append(" Bags of ").append(str(r.varietyName))
         .append(" GpNo ").append(str(d.gpNo))
         .append(" VehicleNo ").append(str(d.vehicleNo))
         .append(" BiltyNo ").append(str(d.biltyNo));
        if (toIntRounded(toDouble(d.biltyFreight)) > 0) s.append(" BiltyFreight ").append(str(d.biltyFreight));
        if (toDouble(d.supplierWeight) > 0) {
            s.append(" SupplierWeight ").append(str(d.supplierWeight)).append(" WeighBridgeWeight ").append(str(d.factoryWeight));
        }
        double sw = toDouble(d.shortWeight);
        if (sw > 0) s.append(" ShortWeight ").append(str(d.shortWeight)).append(" KG");
        else if (sw < 0) s.append(" ExcessWeight ").append(str(d.shortWeight)).append(" KG");
        double other = nzd(r.otherDeduction);
        if (toIntRounded(other) > 0) s.append(" OtherDeduction ").append(cs(other));
        if (toIntRounded(addLess) > 0) s.append(" AddLessAmount ").append(cs(addLess)).append(" PaidAmount ").append(plain(paid));
        if (!str(d.partyName).isEmpty()) s.append("\nFor Party ").append(d.partyName);
        if (!str(d.remarksHeader).trim().isEmpty()) s.append("\n ").append(d.remarksHeader);
        return s.toString();
    }

    // ============================================================================ helpers

    private int accountTypeId(int accountId) {
        for (Map<String, Object> a : globalAccounts()) {
            if (toInt(a.get("Id")) == accountId) return toInt(a.get("AccountTypeId"));
        }
        return 0;
    }

    private boolean hasRight(String rightName) {
        String role = ctx.currentRoleName();
        if ("Admin".equals(role)) return true;
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, @CompanyId=?, @Activity=?",
                    ctx.currentUserId(), SCREEN_NAME, role == null ? "" : role, ctx.currentCompanyId(), "GetByUserId")) {
                Object name = col(r, "RightName");
                if (name != null && rightName.equals(String.valueOf(name).trim())) return toBool(col(r, "Value"));
            }
        } catch (Exception e) {
            LOG.warn("Could not read the '{}' right for {}; denying", rightName, SCREEN_NAME, e);
        }
        return false;
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal — matched on ConfigDescription, value is ConfigKey. */
    private String configValue(String description) {
        try {
            List<Map<String, Object>> rows = jdbc.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                  + "@ConfigDescription=?, @Activity=?",
                    org(), comp(), description, "GetConfigurationByOrgCompandConfigDescription");
            if (rows.isEmpty()) return "";
            Object v = col(rows.get(0), "ConfigKey");
            return v == null ? "" : String.valueOf(v).trim();
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read; treated as empty", description, e);
            return "";
        }
    }

    private void requireNonZeroInt(String text, String field) {
        try {
            int v = Integer.parseInt(str(text).trim());
            if (v != 0) return;
        } catch (NumberFormatException ignored) { }
        throw new Refusal(field + " must be a non-zero number");
    }

    private void requireNonZeroDouble(String text, String field) {
        try {
            double v = Double.parseDouble(str(text).trim().replace(",", ""));
            if (v != 0d) return;
        } catch (NumberFormatException ignored) { }
        throw new Refusal(field + " must be a non-zero number");
    }

    private int org()    { return ctx.currentOrganizationId(); }
    private int comp()   { return ctx.currentCompanyId(); }
    private int fy()     { return ctx.currentFinancialYearId(); }
    private int branch() { return ctx.currentBranchId(); }

    /* ISO 8601 with the 'T': the one datetime literal SQL Server reads the same under every
       SET LANGUAGE / DATEFORMAT, so a British-language login cannot swap day and month. */
    private static String now()   { return new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").format(new Date()); }
    private static String today() { return new SimpleDateFormat("yyyy-MM-dd").format(new Date()) + "T00:00:00"; }
    private static String dayStart(String day) { return notBlank(day) ? day.trim() + "T00:00:00" : null; }

    /** A DateTimePicker's Value carries the time it was opened with; the date is the operator's. */
    private static String docDateWithTime(String day) {
        if (!notBlank(day)) return now();
        return day.trim() + "T" + new SimpleDateFormat("HH:mm:ss").format(new Date());
    }

    private static String exec(String proc, List<String> names) {
        StringBuilder b = new StringBuilder("EXEC ").append(proc.startsWith("[") ? proc : "dbo." + proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) b.append(", ");
            b.append(names.get(i)).append("=?");
        }
        return b.toString();
    }

    /** GenericProvider.SetProc: every property as a named parameter, ExecuteScalar's value back. */
    private int scalar(String proc, Map<String, Object> params) {
        List<String> names = new ArrayList<>();
        List<Object> values = new ArrayList<>();
        for (Map.Entry<String, Object> e : params.entrySet()) {
            names.add("@" + e.getKey());
            values.add(e.getValue() == null ? new SqlParameterValue(Types.VARCHAR, null) : e.getValue());
        }
        return firstInt(runProc(exec(proc, names), values.toArray(), proc));
    }

    /** The same over a ContraVoucherDto shape (its public fields were checked against the procedures). */
    private int scalarObj(String proc, Object obj) {
        List<String> names = new ArrayList<>();
        List<Object> values = new ArrayList<>();
        for (java.lang.reflect.Field f : obj.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            Object v;
            try { v = f.get(obj); } catch (IllegalAccessException e) { continue; }
            names.add("@" + f.getName());
            values.add(v == null ? typedNull(f.getType()) : v);
        }
        return firstInt(runProc(exec(proc, names), values.toArray(), proc));
    }

    private static SqlParameterValue typedNull(Class<?> type) {
        if (type == Integer.class || type == Long.class) return new SqlParameterValue(Types.INTEGER, null);
        if (type == Double.class) return new SqlParameterValue(Types.DOUBLE, null);
        if (type == BigDecimal.class) return new SqlParameterValue(Types.NUMERIC, null);
        if (type == Boolean.class) return new SqlParameterValue(Types.BIT, null);
        return new SqlParameterValue(Types.VARCHAR, null);
    }

    private static int firstInt(List<Map<String, Object>> rows) {
        if (rows.isEmpty() || rows.get(0).isEmpty()) return 0;
        Object v = rows.get(0).values().iterator().next();
        return v instanceof Number ? ((Number) v).intValue() : toInt(v);
    }

    private List<Map<String, Object>> runProc(String sql, Object[] args, String proc) {
        try {
            return query(sql, args);
        } catch (RuntimeException e) {
            LOG.warn("{} failed", proc, e);
            throw e;
        }
    }

    /**
     * Execute and return the first result set, or nothing when the procedure returns none —
     * the DataTable-fill behaviour (Sp_InvFreightVoucher_Update returns no rows at all).
     */
    private List<Map<String, Object>> query(String sql, Object... args) {
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
            boolean has = ps.execute();
            while (!has && ps.getUpdateCount() != -1) has = ps.getMoreResults();
            List<Map<String, Object>> out = new ArrayList<>();
            if (!has) return out;
            try (java.sql.ResultSet rs = ps.getResultSet()) {
                if (rs == null) return out;
                java.sql.ResultSetMetaData md = rs.getMetaData();
                int n = md.getColumnCount();
                while (rs.next()) {
                    Map<String, Object> row = new LinkedHashMap<>();
                    for (int c = 1; c <= n; c++) {
                        String label = md.getColumnLabel(c);
                        if (label == null || label.isEmpty()) label = "col" + c;
                        Object v = rs.getObject(c);
                        if (v instanceof java.sql.Timestamp) v = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss").format((java.sql.Timestamp) v);
                        else if (v instanceof java.sql.Date) v = new SimpleDateFormat("yyyy-MM-dd").format((java.sql.Date) v);
                        row.put(label, v);
                    }
                    out.add(row);
                }
            }
            return out;
        });
    }

    /** CompLogoImage and other binaries are not printable on this page; they are dropped. */
    private static List<Map<String, Object>> dropBinary(List<Map<String, Object>> rows) {
        for (Map<String, Object> r : rows) r.values().removeIf(v -> v instanceof byte[]);
        return rows;
    }

    private static Map<String, Object> textify(Map<String, Object> row) {
        Map<String, Object> out = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : row.entrySet()) {
            Object v = e.getValue();
            if (v instanceof BigDecimal) v = ((BigDecimal) v).toPlainString();
            if (v instanceof byte[]) continue;
            out.put(e.getKey(), v);
        }
        return out;
    }

    static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static boolean notBlank(String s) { return s != null && !s.trim().isEmpty(); }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o); }
    private static int nz(Integer v) { return v == null ? 0 : v; }
    private static double nzd(Double v) { return v == null ? 0d : v; }

    static int toInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) {
            try { return (int) Math.rint(Double.parseDouble(s)); } catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToInt(double) — Convert.ToInt32, i.e. round half to even. */
    private static int toIntRounded(Double v) { return v == null ? 0 : (int) Math.rint(v); }
    private static int toIntRounded(double v) { return (int) Math.rint(v); }

    static double toDouble(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0d; }
    }

    private static BigDecimal dec(String s) { return BigDecimal.valueOf(toDouble(s)); }

    static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim().toLowerCase(Locale.ROOT);
        return s.equals("true") || s.equals("1") || s.equals("yes");
    }

    /** C# double.ToString(): integers without a decimal point, otherwise the shortest form. */
    private static String cs(double d) {
        if (d == Math.rint(d) && !Double.isInfinite(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return BigDecimal.valueOf(d).stripTrailingZeros().toPlainString();
    }

    private static String plain(BigDecimal b) {
        String s = b.stripTrailingZeros().toPlainString();
        return s;
    }
}
