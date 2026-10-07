package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.ExportPreInvoiceRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import com.mst.services.desktopvoucher.DesktopVoucherSupport;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Base64;
import java.util.Collection;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 * 40 "Export Voucher" - Architecture.WinApp.Account_Definition.frmExportInvoiceVoucher (form Name / ScreenName
 * "frmExportInvoiceVoucher", DocumentTypeId 27). ":NNN" = line in frmExportInvoiceVoucher.cs; BLL 0456 = Architecture.BLL.Export.ExportVoucher,
 * DAL 0508 = Architecture.DAL.Export.ExportVoucher.
 *
 * Save (Insert :1445 -> ExportVoucher.Save, BLL 0456 :428): MakeVoucherForExportVoucher builds the accounting voucher, then DAL SetDate
 * (one transaction): USP_ExportVoucher_Insert / _Update, USP_ExportVoucherDetail_Insert per row (LineId 1..n), USP_exImInvoiceSaleManCommission_Insert,
 * attachments (DeleteById + Proc_DMSAttachments_Insert), FIFO stock evaluation (ERP feature 5) or usp_StockEvaluationUpdateForExportForwarding,
 * usp_StockInTransit_VoucherDelete_ByExportVoucherId, Sp_VoucherHead_Insert / _Update, Sp_VoucherDetail_Insert, USP_VoucherBalanceCheck,
 * Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert. Everything runs in this service's single Spring transaction.
 *
 * Tenancy (organisation, company, branch, year, user) always comes from the session. The invoice, voucher, detail and commission ids that the
 * stored procedures do not check themselves are verified here before any procedure is called.
 */
@Service
public class ExportInvoiceVoucherService {
    public static final String SCREEN_NAME = "frmExportInvoiceVoucher";
    public static final int DOCUMENT_TYPE_ID = 27;
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");
    private static final DateTimeFormatter SQLSTAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopVoucherSupport support;
    private final DesktopAttachmentStore attachmentStore;
    private final ExportPreInvoiceRepository lookups;

    public ExportInvoiceVoucherService(JdbcTemplate jdbc, CurrentUserContext context, DesktopVoucherSupport support,
                                       DesktopAttachmentStore attachmentStore, ExportPreInvoiceRepository lookups) {
        this.jdbc = jdbc; this.context = context; this.support = support; this.attachmentStore = attachmentStore; this.lookups = lookups;
    }

    private UserAccount user() { return context.requireAccountingUser(); }

    // ================================================================================ load (AcfrmPaymentVoucher_Load :230)

    /**
     * Rightsobjects of the screen, FIFOCGS (ERP feature 5), SaleCommissionTabFeatureWise (feature 15), CGSEntryAllow, SaleCostingJobOrderWise,
     * ToleranceWeightForDifferenceInDispatchedAndInvoice (abs), the combos (Buyer = customer group 7, Account Title = types 3,6,8,10,
     * Commission Expense = 11,20,21, currencies with exchange rate, sales persons = group 9), the next voucher code and the pending list.
     */
    public Map<String, Object> init() {
        UserAccount u = user();
        Map<String, Boolean> r = support.rights(SCREEN_NAME);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("canSave", r.get("save")); m.put("canUpdate", r.get("update")); m.put("canDelete", r.get("delete"));
        m.put("canPrint", r.get("print")); m.put("canViewAll", r.get("canViewAllRecord"));
        boolean fifo = support.feature(5);
        boolean commissionTab = support.feature(15);
        m.put("fifo", fifo); m.put("commissionTab", commissionTab);
        m.put("cgsEntryAllow", DesktopVoucherSupport.toBool(support.config("CGSEntryAllow")));
        m.put("saleCostingJobOrderWise", DesktopVoucherSupport.toBool(support.config("SaleCostingJobOrderWise")));
        m.put("tolerance", tolerance());
        m.put("decimals", support.decimals());
        m.put("defaultDays", DesktopVoucherSupport.toInt(support.config("DefaultDaysToLessFromHistoryFromDate")));
        m.put("now", STAMP.format(LocalDateTime.now()));
        m.put("voucherCode", support.generateVoucherCode(DOCUMENT_TYPE_ID));
        m.put("buyers", buyers(u));
        m.put("creditAccounts", accounts("3,6,8,10"));
        m.put("commissionAccounts", accounts("11,20,21"));
        m.put("currencies", currencies(u));
        m.put("salesPersons", commissionTab ? salesPersons(u) : new ArrayList<Map<String, Object>>());
        m.put("pending", pending());
        m.put("historyCustomers", historyCustomers());
        return m;
    }

    /** btnRefresh_Click :1397 - the global lists again (suppliers, accounts, currencies) and the config flags. */
    public Map<String, Object> refresh() {
        UserAccount u = user();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("cgsEntryAllow", DesktopVoucherSupport.toBool(support.config("CGSEntryAllow")));
        m.put("saleCostingJobOrderWise", DesktopVoucherSupport.toBool(support.config("SaleCostingJobOrderWise")));
        m.put("tolerance", tolerance());
        m.put("buyers", buyers(u));
        m.put("creditAccounts", accounts("3,6,8,10"));
        m.put("commissionAccounts", accounts("11,20,21"));
        m.put("currencies", currencies(u));
        return m;
    }

    private double tolerance() { return Math.abs(DesktopVoucherSupport.toDouble(support.config("ToleranceWeightForDifferenceInDispatchedAndInvoice"))); }

    private List<Map<String, Object>> buyers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : lookups.customersByGroup(u, "7")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id"))); m.put("CompanyName", str(r.get("CompanyName")));
            m.put("PartyCode", str(r.get("PartyCode"))); m.put("GlAccountId", toInt(r.get("GlAccountId")));
            out.add(m);
        }
        return out;
    }

    private List<Map<String, Object>> salesPersons(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : lookups.customersByGroup(u, "9")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id"))); m.put("CompanyName", str(r.get("CompanyName")));
            out.add(m);
        }
        return out;
    }

    private List<Map<String, Object>> accounts(String typeIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : support.accountsGetAccountTitleByAccountTypeIds(typeIds, 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id"))); m.put("AccountTitle", str(r.get("AccountTitle"))); m.put("AccountCode", str(r.get("AccountCode")));
            out.add(m);
        }
        return out;
    }

    /** MultiCurrency.GetMultiCurrencywithExchangeRate(CompanyId) -> usp_getMultiCurrencywithExchangeRate. */
    private List<Map<String, Object>> currencies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[usp_getMultiCurrencywithExchangeRate]", DesktopProc.params("CompanyId", u.getCompanyId()))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id"))); m.put("CurrencyCode", str(r.get("CurrencyCode")));
            m.put("ExchangeRate", plain(r.get("ExchangeRate")));
            out.add(m);
        }
        return out;
    }

    /** ComboBindForHistory :2230 - USP_GetDataForDropDownFromExportInvoiceVoucher (org, company), rows with ActivityType == "Customer". */
    public List<Map<String, Object>> historyCustomers() {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoiceVoucher]",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (!"Customer".equals(str(r.get("ActivityType")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id"))); m.put("name", str(r.get("name")));
            out.add(m);
        }
        return out;
    }

    public int nextCode() { user(); return support.generateVoucherCode(DOCUMENT_TYPE_ID); }

    // ================================================================================ pending list (PendingForAccountsGLEntry :719)

    /** VoucherHead.PendingForAccountsGLEntry -> Sp_ExImInvoice_PendingForAccountsGLEntry(@OrginzationId, @CompanyId), columns renamed as :757. */
    public List<Map<String, Object>> pending() {
        UserAccount u = user();
        return pendingRows(u, 0);
    }

    private List<Map<String, Object>> pendingRows(UserAccount u, int invoiceId) {
        Map<String, Object> p = DesktopProc.params("OrginzationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (invoiceId > 0) p.put("EximInvoiceId", invoiceId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : DesktopProc.rows(jdbc, "[dbo].[Sp_ExImInvoice_PendingForAccountsGLEntry]", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(d.get("Id"))); m.put("SupplierCustomerId", toInt(d.get("SupplierCustomerId")));
            m.put("Customer", str(d.get("BuyerName"))); m.put("Consignee", str(d.get("Consignee")));
            m.put("ContractNo", str(d.get("ContractNo"))); m.put("InvoiceNo", str(d.get("InvoiceNo")));
            m.put("InvoiceDate", plain(d.get("DocDate"))); m.put("InvoiceStatusId", toInt(d.get("InvoiceStatusId")));
            m.put("InvoiceDispatchStatus", str(d.get("InvoiceStatus"))); m.put("FcurrencyId", toInt(d.get("FcurrencyId")));
            m.put("FcyCode", str(d.get("FcyCode"))); m.put("ExchangeRate", plain(d.get("ExchangeRate")));
            m.put("FcyAmount", plain(d.get("FCurrencyAmount"))); m.put("DeliveryTerm", str(d.get("DeliveryTermCode")));
            m.put("NoOfContainers", plain(d.get("NoOfContainers"))); m.put("InvoiceNetWeight", plain(d.get("NetWeight")));
            m.put("DispatchWeight", plain(d.get("DispatchWeight"))); m.put("DoNetWeight", plain(d.get("DoWeight")));
            m.put("WbGrossWeight", plain(d.get("WbGrossWeight"))); m.put("ChartOfAccountId", toInt(d.get("ChartOfAccountId")));
            m.put("CreditAccountId", toInt(d.get("CreditAccountId"))); m.put("FI_Number", str(d.get("FI_Number")));
            m.put("FI_Date", plain(d.get("FI_Date"))); m.put("SpecialApprovedStatus", str(d.get("SpecialApprovedStatus")));
            out.add(m);
        }
        return out;
    }

    /**
     * PendingRecordbind :803 - the Edit button of a pending row. InvoiceStatusId 0 refuses with the desktop's text; otherwise the form is filled
     * from the row and the invoice: grid (usp_getexportInvoiceDetailByIdForExportVoucher with the voucher date), payment terms, other charges
     * and commission (Sp_ExImInvoice_GetAllMethod). Only an invoice that is still pending in this company can be bound.
     */
    public Map<String, Object> bindInvoice(int invoiceId, String voucherDate) {
        UserAccount u = user();
        if (!support.rights(SCREEN_NAME).get("save")) throw new AccessDeniedException("Record Not bind in grid");
        List<Map<String, Object>> rows = pendingRows(u, invoiceId);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not bind in grid");
        Map<String, Object> row = rows.get(0);
        if (toInt(row.get("InvoiceStatusId")) == 0) {
            throw new IllegalArgumentException((tolerance() == 0d
                    ? "The invoice status is pending because the dispatch has not been completed against the invoice"
                    : "The invoice status is pending because the dispatch has not been completed against the invoice or Special Approval is Pending") + ". Please check");
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("header", row);
        m.put("rows", invoiceRows(u, invoiceId, voucherDate));
        m.put("paymentTerms", paymentTerms(u, invoiceId));
        m.put("otherCharges", otherCharges(u, invoiceId));
        m.put("commission", commission(u, invoiceId));
        return m;
    }

    private List<Map<String, Object>> invoiceRows(UserAccount u, int invoiceId, String voucherDate) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : DesktopProc.rows(jdbc, "[dbo].[usp_getexportInvoiceDetailByIdForExportVoucher]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocDate", date(voucherDate), "Id", invoiceId))) {
            out.add(gridRow(d.get("EntryTypeId"), d.get("Id"), d.get("ExImInvoiceDetailId"), d.get("ExImInvoiceId"), d.get("ContractId"), d.get("ContractNo"),
                    d.get("InvoiceNo"), d.get("ItemId"), d.get("ItemName"), d.get("WarehouseId"), d.get("CropYearId"), d.get("CropYear"), d.get("LotJobId"),
                    d.get("JobLotCode"), d.get("PackingMaterialId"), d.get("PackMaterilaType"), d.get("Qty"), d.get("OuterQtyUomId"), d.get("ItemUOM"),
                    d.get("NetWeight"), d.get("RatePrice"), d.get("RateUomId"), d.get("RateUOM"), d.get("FcAmount"), d.get("StockWeight"), d.get("CostRate"),
                    d.get("StockValue"), 0));
        }
        return out;
    }

    /** The 28 columns of the desktop's grd DataTable, in order. */
    private static Map<String, Object> gridRow(Object... v) {
        String[] k = { "EntryTypeId", "Id", "ExImInvoiceDetailId", "ExImInvoiceId", "ContractId", "ContractNo", "InvoiceNo", "ItemId", "ItemName", "WarehouseId",
                "CropYearId", "CropYear", "LotJobId", "JobLotCode", "PackingMaterialId", "PackMaterilaType", "Qty", "OuterQtyUomId", "ItemUOM", "NetWeight",
                "RatePrice", "RateUomId", "RateUOM", "FcAmount", "StockWeight", "CostRate", "StockValue", "AddLessAmount" };
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i < k.length; i++) m.put(k[i], plain(v[i]));
        return m;
    }

    // ================================================================================ invoice side reads

    private void requireOwnInvoice(UserAccount u, int invoiceId) {
        if (invoiceId <= 0) throw new IllegalArgumentException("Invoice Not Found");
        List<Integer> hit = jdbc.queryForList("SELECT 1 FROM dbo.ExImInvoice WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?",
                Integer.class, invoiceId, u.getOrganizationId(), u.getCompanyId());
        if (hit.isEmpty()) throw new AccessDeniedException("The selected invoice does not belong to this company");
    }

    /** FillPaymentDetailFromInvoice :1781 - ReadExImInvoicePaymentTermsDetailByHeaderId. */
    private List<Map<String, Object>> paymentTerms(UserAccount u, int invoiceId) {
        requireOwnInvoice(u, invoiceId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", DesktopProc.params("Id", invoiceId,
                "Activity", "ReadExImInvoicePaymentTermsDetailByHeaderId"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", plain(r.get("Id"))); m.put("PaymentTermId", toInt(r.get("PaymentTermId"))); m.put("PaymentTerm", str(r.get("PaymentTerm")));
            m.put("DocumentTypeId", toInt(r.get("DocumentTypeId"))); m.put("ExImEFormRegistrationId", toInt(r.get("ExImEFormRegistrationId")));
            m.put("FinancialInstrumentNo", str(r.get("FinancialInstrumentNo"))); m.put("PrcntOfTotal", plain(r.get("PrcntOfTotal")));
            m.put("FcyAmount", plain(r.get("FcyAmount"))); m.put("DueDays", toInt(r.get("DueDays"))); m.put("Remarks", str(r.get("PaymentRemarks")));
            out.add(m);
        }
        return out;
    }

    /** FillOtherChargesDetailFromInvoice :1793 - ExImInvoiceOtherChargesDetail_ReadByHeaderId. */
    private List<Map<String, Object>> otherCharges(UserAccount u, int invoiceId) {
        requireOwnInvoice(u, invoiceId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", DesktopProc.params("Id", invoiceId,
                "Activity", "ExImInvoiceOtherChargesDetail_ReadByHeaderId"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", plain(r.get("Id"))); m.put("ChargesItemId", toInt(r.get("ChargesItemId"))); m.put("ChargesItem", str(r.get("ChargesItemName")));
            m.put("AddAmount", plain(r.get("AddAmount"))); m.put("LessAmount", plain(r.get("LessAmount")));
            m.put("ChargesAccountId", toInt(r.get("ChargesAccountId"))); m.put("ChargesAccount", str(r.get("ChargesAccount")));
            m.put("Remarks", str(r.get("Remarks")));
            out.add(m);
        }
        return out;
    }

    /** FillCommissionDetailFromInvoice :1805 - EximInvoiceCommissionInfoByReadByHeaderId. */
    private List<Map<String, Object>> commission(UserAccount u, int invoiceId) {
        requireOwnInvoice(u, invoiceId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", DesktopProc.params("Id", invoiceId,
                "Activity", "EximInvoiceCommissionInfoByReadByHeaderId"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("exImInvoiceSaleManCommissionId"))); m.put("SalesPersonId", toInt(r.get("SalesPersonId")));
            m.put("SalesPerson", str(r.get("SalesPerson"))); m.put("CommissionTypeId", toInt(r.get("commissionTypeId")));
            m.put("CommissionType", str(r.get("CommissionType"))); m.put("Rate", plain(r.get("commissionRate")));
            m.put("RateUom", str(r.get("rateUom"))); m.put("FcyAmount", plain(r.get("fcyAmount")));
            m.put("ExchangeRate", plain(r.get("exchangeRate"))); m.put("LcyAmount", plain(r.get("lcyAmount")));
            m.put("Remarks", str(r.get("Remarks")));
            out.add(m);
        }
        return out;
    }

    /** GetInvoiceDate (:ReadById) - Sp_ExImInvoice_GetAllMethod(org, company, Id, 'GetInvoiceDate'): InvoiceDate, NetWeight. */
    private Map<String, Object> invoiceDate(UserAccount u, int invoiceId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("InvoiceDate", null); m.put("NetWeight", 0d);
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", DesktopProc.params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Id", invoiceId, "Activity", "GetInvoiceDate"))) {
            m.put("InvoiceDate", plain(r.get("InvoiceDate"))); m.put("NetWeight", plain(r.get("NetWeight")));
            break;
        }
        return m;
    }

    // ================================================================================ ReadById (:1690) and History

    /** ExportVoucher.GetByID: Sp_ExportVoucher_GetAllMethod 'ReadById' (no tenant filter in the procedure, so organisation / company / type are checked here). */
    private Map<String, Object> ownVoucher(UserAccount u, int id) {
        if (id <= 0) throw new IllegalArgumentException("RecordId Not Found.....");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ExportVoucher_GetAllMethod", DesktopProc.params("Id", id, "Activity", "ReadById"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Voucher Not Found");
        Map<String, Object> h = rows.get(0);
        if (toInt(h.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(h.get("CompanyId")) != toInt(u.getCompanyId())
                || toInt(h.get("DocumentTypeId")) != DOCUMENT_TYPE_ID) throw new AccessDeniedException("The selected voucher does not belong to this company");
        return h;
    }

    private List<Map<String, Object>> voucherDetails(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExportVoucher_GetAllMethod", DesktopProc.params("Id", id, "Activity", "ReadDetailByHeaderId"));
    }

    /**
     * ReadById :1690 - the saved voucher. The grid is the voucher's rows; payment terms, other charges and commission are re-read from the invoice
     * (as the desktop does), the invoice date / weight come from GetInvoiceDate and the attachments from DMSAttachments.GetByID(RecId, base.Name).
     */
    public Map<String, Object> read(int id) {
        UserAccount u = user();
        Map<String, Object> h = ownVoucher(u, id);
        int invoiceId = toInt(h.get("ExImInvoiceId"));
        Map<String, Object> m = new LinkedHashMap<>();
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id); o.put("DocNo", toInt(h.get("DocNo"))); o.put("DocDate", plain(h.get("DocDate")));
        o.put("SupplierCustomerId", toInt(h.get("SupplierCustomerId"))); o.put("CreditAccountId", toInt(h.get("CreditAccounId")));
        o.put("CommissionExpenseAccountId", toInt(h.get("CommissionExpenseAccountId"))); o.put("CurrencyId", toInt(h.get("CurrencyId")));
        o.put("ExImInvoiceId", invoiceId); o.put("InvoiceNo", str(h.get("InvoiceNo")));
        double without = toDouble(h.get("FcyAmountWithoutAddLess"));
        o.put("FcyAmountWithoutAddLess", without > 0 ? without : toDouble(h.get("FcyAmount")));
        o.put("AddLessAmount", toDouble(h.get("AddLessAmount"))); o.put("FcyAmount", toDouble(h.get("FcyAmount")));
        o.put("ExchangeRate", toDouble(h.get("ExchangeRate"))); o.put("LcyAmount", plain(h.get("LcyAmount")));
        o.put("DueDays", toInt(h.get("DueDays"))); o.put("DueDate", plain(h.get("DueDate"))); o.put("Remarks", str(h.get("Remarks")));
        o.put("IsApproved", DesktopVoucherSupport.toBool(h.get("IsApproved")));
        o.put("IsSpecialApproved", DesktopVoucherSupport.toBool(h.get("IsSpecialApproved")));
        m.put("header", o);
        m.put("invoice", invoiceDate(u, invoiceId));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : voucherDetails(id)) {
            rows.add(gridRow(d.get("EntryTypeId"), d.get("Id"), d.get("ExImInvoiceDetailId"), invoiceId, d.get("ContractId"), d.get("ContractNo"), str(h.get("InvoiceNo")),
                    d.get("ItemId"), d.get("ItemName"), d.get("WarehouseId"), d.get("CropYearId"), d.get("CropYear"), d.get("LotJobId"), d.get("JobLotDescription"),
                    d.get("PackingTypeId"), d.get("PackTypeDesc"), d.get("Qty"), d.get("ItemUomId"), d.get("PackUom"), d.get("NetWeight"), d.get("RatePrice"),
                    d.get("RateUomId"), d.get("RateUom"), d.get("FcyAmount"), d.get("StockWeight"), d.get("CostRate"), d.get("StockValue"), d.get("AddLessAmount")));
        }
        m.put("rows", rows);
        m.put("paymentTerms", paymentTerms(u, invoiceId));
        m.put("otherCharges", otherCharges(u, invoiceId));
        m.put("commission", commission(u, invoiceId));
        m.put("attachments", attachments(id));
        return m;
    }

    /** DMSAttachments.GetByID(id, base.Name) -> Sp_DMSAttachments_GetAllMethod 'ReadById'; other tenants dropped. */
    public List<Map<String, Object>> attachments(int id) {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<>();
        if (id <= 0) return out;
        ownVoucher(u, id);
        for (Map<String, Object> a : DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params("ScreenName", SCREEN_NAME, "Id", id, "Activity", "ReadById"))) {
            if (toInt(a.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(a.get("CompanyId")) != toInt(u.getCompanyId())) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(a.get("Id"))); o.put("Attachment", str(a.get("Attachment")));
            o.put("CustomName", str(a.get("UploadedFileCustomName"))); o.put("EntryDate", plain(a.get("EntryDate")));
            out.add(o);
        }
        return out;
    }

    public static final class Download {
        private final String name; private final byte[] bytes;
        Download(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
        public String name() { return name; }
        public byte[] bytes() { return bytes; }
    }

    /** NoOfAttachments link / attachment card: one stored file of a voucher of this company. */
    public Download attachment(int id, int attachmentId) {
        UserAccount u = user();
        for (Map<String, Object> a : attachments(id)) {
            if (toInt(a.get("Id")) != attachmentId) continue;
            String original = basename(str(a.get("Attachment")));
            String stored = str(a.get("CustomName"));
            return new Download(original, attachmentStore.read(u, basename(stored.trim().isEmpty() ? original : stored)));
        }
        throw new IllegalArgumentException("Attachment not found");
    }

    private static String basename(String name) {
        String v = name == null ? "" : name.replace('\\', '/');
        v = v.substring(v.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(v);
        return v;
    }

    /** HistoryFill :1915 -> ExportVoucher.FormHistoryNew (BLL 0456 :544): USP_ExportInvoiceVoucher_FormHistory with the desktop's guarded parameters. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = user();
        Map<String, Boolean> rights = support.rights(SCREEN_NAME);
        boolean all = Boolean.TRUE.equals(rights.get("canViewAllRecord"));
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", context.currentFinancialYearId(), "BranchesId", u.getBranchesId(), "DocumentTypeId", DOCUMENT_TYPE_ID,
                "CanViewAllRecord", all);
        String type = str(b.get("dateType"));
        java.sql.Date from = date(b.get("fromDate")), to = date(b.get("toDate"));
        if ("entry".equals(type)) { put(p, "EntryFromDate", from); put(p, "EntryToDate", to); }
        else if ("modify".equals(type)) { put(p, "ModifyFromDate", from); put(p, "ModifyToDate", to); }
        else if ("approved".equals(type)) { put(p, "ApprovedFromDate", from); put(p, "ApprovedToDate", to); }
        else { put(p, "FromDate", from); put(p, "ToDate", to); }
        int fromNo = toInt(b.get("fromNo")), toNo = toInt(b.get("toNo")), cust = toInt(b.get("supplierCustomerId"));
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        if (cust != 0) p.put("SupplierCustomerId", cust);
        if (!all) p.put("EntryUser", context.currentUserId());
        String status = str(b.get("approved"));
        if (!"All".equals(status)) p.put("IsApproved", "Approved".equals(status));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_ExportInvoiceVoucher_FormHistory", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id"))); m.put("VoucherHeadId", toInt(r.get("VoucherHeadId"))); m.put("DocumentTypeId", toInt(r.get("DocumentTypeId")));
            m.put("VoucherCode", toInt(r.get("DocNo"))); m.put("VoucherDate", dMonY(r.get("DocDate"))); m.put("InvoiceNo", str(r.get("InvoiceNo")));
            m.put("CustomerName", str(r.get("CustomerName"))); m.put("Remarks", str(r.get("Remarks"))); m.put("NoOfContainers", toInt(r.get("NoOfContainers")));
            m.put("NetWeight", toDouble(r.get("NetWeight"))); m.put("CurrencyCode", str(r.get("CurrencyCode")));
            m.put("ExchangeRate", toDouble(r.get("ExchangeRate"))); m.put("FcyAmount", toDouble(r.get("FcyAmount")));
            m.put("VoucherAmount", toDouble(r.get("LcyAmount"))); m.put("EntryDate", plain(r.get("EntryDate")));
            m.put("EntryUser", str(r.get("EntryUserName"))); m.put("ModifyDate", plain(r.get("ModifyDate")));
            m.put("ModifyUser", str(r.get("ModifyUserName"))); m.put("ApprovalStatus", str(r.get("ApprovalStatus")));
            m.put("NoOfAttachments", plain(r.get("NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    private static void put(Map<String, Object> p, String k, Object v) { if (v != null) p.put(k, v); }

    private static String dMonY(Object v) {
        Object t = plain(v);
        if (t == null) return "";
        String s = String.valueOf(t);
        if (s.length() < 10) return s;
        LocalDate d = LocalDate.parse(s.substring(0, 10));
        return String.format("%02d-%s-%d", d.getDayOfMonth(), new String[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }[d.getMonthValue() - 1], d.getYear());
    }

    /** History row selection :GetDeailByHeaderId - the voucher's detail rows (ExportVoucher.GetByID), columns renamed as the history detail grid. */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = user();
        ownVoucher(u, id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : voucherDetails(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ContractNo", str(d.get("ContractNo"))); m.put("InvoiceNo", ""); m.put("ItemName", str(d.get("ItemName")));
            m.put("CropYear", str(d.get("CropYear"))); m.put("JobLotCode", str(d.get("JobLotDescription")));
            m.put("PackMaterilaType", str(d.get("PackTypeDesc"))); m.put("Qty", toDouble(d.get("Qty"))); m.put("ItemUOM", str(d.get("PackUom")));
            m.put("NetWeight", toDouble(d.get("NetWeight"))); m.put("RatePrice", toDouble(d.get("RatePrice"))); m.put("RateUOM", str(d.get("RateUom")));
            m.put("FcAmount", toDouble(d.get("FcyAmount"))); m.put("StockWeight", toDouble(d.get("StockWeight")));
            m.put("CostRate", toDouble(d.get("CostRate"))); m.put("StockValue", toDouble(d.get("StockValue")));
            m.put("AddLessAmount", toDouble(d.get("AddLessAmount")));
            out.add(m);
        }
        return out;
    }

    // ================================================================================ datVoucherDate_Leave (:2512)

    /**
     * Re-prices the grid for a voucher date: SaleCostingJobOrderWise -> usp_getAvgRateByJobOrder (DocDate, JobLotId, ItemId, plus DocumentTypeId 27 /
     * RecId when editing); otherwise AvgRateOnlyForCGS -> Sp_GetAvgRatesAndStockInHand_GetAllMethod 'GetOnlyAvgRateForCGS' (zero optional ids omitted).
     */
    public List<Map<String, Object>> rates(Map<String, Object> b) {
        UserAccount u = user();
        int recId = toInt(b.get("recId"));
        if (recId > 0) ownVoucher(u, recId);
        Timestamp docDate = ts(b.get("voucherDate"));
        boolean jobWise = DesktopVoucherSupport.toBool(support.config("SaleCostingJobOrderWise"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : maps(b.get("rows"))) {
            out.add(DesktopProc.params("rate", avgRate(u, jobWise, docDate, jobWise ? (recId > 0 ? DOCUMENT_TYPE_ID : 0) : DOCUMENT_TYPE_ID, recId,
                    toInt(r.get("itemId")), toInt(r.get("lotJobId")), toInt(r.get("cropYearId")), toInt(r.get("warehouseId")))));
        }
        return out;
    }

    /** btnRecordsUpdate_Click :Ctrl+Shift+F - USP_GetExportVoucherIdsForAutoUpdation(org, company, year, 27); empty -> "Ids Not Found For Update". */
    public List<Integer> autoUpdateIds() {
        UserAccount u = user();
        if (!Boolean.TRUE.equals(support.rights(SCREEN_NAME).get("update"))) throw new AccessDeniedException("You do not have the Update right on this screen");
        List<Integer> ids = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_GetExportVoucherIdsForAutoUpdation]", DesktopProc.params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "FinancialYearId", context.currentFinancialYearId(), "DocumentTypeId", DOCUMENT_TYPE_ID))) {
            Object v = r.values().isEmpty() ? null : r.values().iterator().next();
            ids.add(toInt(r.containsKey("Id") ? r.get("Id") : v));
        }
        if (ids.isEmpty()) throw new IllegalArgumentException("Ids Not Found For Update");
        return ids;
    }

    // ================================================================================ Save (Insert :1445 -> ExportVoucher.Save -> DAL SetDate)

    /** ExportVoucherDetail model rows as the form builds them (:1506-1546). */
    private static final class Det {
        int entryTypeId, id, exImInvoiceDetailId, contractId, cropYearId, packingTypeId, itemUomId, itemId, lotJobId, warehouseId, rateUomId, currencyId, lineId, exportVoucherId;
        BigDecimal costRate = BigDecimal.ZERO, qty = BigDecimal.ZERO, stockWeight = BigDecimal.ZERO, netWeight = BigDecimal.ZERO, stockValue = BigDecimal.ZERO,
                mton = BigDecimal.ZERO, ratePrice = BigDecimal.ZERO, exchangeRate = BigDecimal.ZERO, fcyAmount = BigDecimal.ZERO, addLess = BigDecimal.ZERO, lcyAmount = BigDecimal.ZERO;
        String remarks = "", contractNo = "", itemName = "";
    }

    /** ExportVoucher model header (the 34 procedure parameters). */
    private static final class Hdr {
        int id, exImInvoiceId, docNo, supplierCustomerId, creditAccountId, commissionExpenseAccountId, currencyId, dueDays, branchesId, organizationId, companyId, financialYearId,
                entryUserId, modifyUserId, autoUpdate;
        Timestamp docDate, dueDate, entryDate, modifyDate;
        BigDecimal exchangeRate = BigDecimal.ZERO, fcyAmount = BigDecimal.ZERO, lcyAmount = BigDecimal.ZERO, netWeight = BigDecimal.ZERO;
        double fcyAmountWithoutAddLess, addLessAmount;
        String remarks = "", invoiceNo = "";
    }

    /**
     * Insert() :1445 - FormValidation, the confirmations (client side), the Commission Expense Account rule, the grid / weight / add-less checks, then
     * ExportVoucher.Save. recId 0 inserts (USP_ExportVoucher_Insert), recId > 0 updates (USP_ExportVoucher_Update); autoUpdate is the
     * Ctrl+Shift+F "Records Update" re-save (obj.AutoUpdate = 1). One transaction; any failure rolls everything back.
     */
    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = user();
        final int org = u.getOrganizationId(), company = u.getCompanyId();
        final int userId = context.currentUserId();
        int recId = toInt(b.get("recId"));
        boolean auto = DesktopVoucherSupport.toBool(b.get("autoUpdate"));
        if (auto && recId <= 0) throw new IllegalArgumentException("Rec Id Not found");
        Map<String, Boolean> rights = support.rights(SCREEN_NAME);
        if (recId > 0) {
            if (!Boolean.TRUE.equals(rights.get("update"))) throw new AccessDeniedException("You do not have the Update right on this screen");
        } else if (!Boolean.TRUE.equals(rights.get("save"))) throw new AccessDeniedException("You do not have the Save right on this screen");

        // ---- what the bound invoice / saved voucher fixes (read-only controls on the desktop)
        int invoiceId, supplier, currency;
        String invoiceNo;
        double compareWeight;
        Map<String, Map<String, Object>> serverRows = new LinkedHashMap<>();
        Map<String, Object> head = null;
        int docNo;
        if (recId > 0) {
            head = ownVoucher(u, recId);
            invoiceId = toInt(head.get("ExImInvoiceId")); supplier = toInt(head.get("SupplierCustomerId")); currency = toInt(head.get("CurrencyId"));
            invoiceNo = str(head.get("InvoiceNo")); docNo = toInt(head.get("DocNo"));
            compareWeight = toDouble(invoiceDate(u, invoiceId).get("NetWeight"));
            for (Map<String, Object> d : voucherDetails(recId)) serverRows.put("id:" + toInt(d.get("Id")), d);
        } else {
            invoiceId = toInt(b.get("exImInvoiceId"));
            if (invoiceId <= 0) throw new IllegalArgumentException("Account Field is Required");
            List<Map<String, Object>> pr = pendingRows(u, invoiceId);
            if (pr.isEmpty()) throw new IllegalArgumentException("The selected invoice is not pending for the Export Voucher");
            Map<String, Object> row = pr.get(0);
            if (toInt(row.get("InvoiceStatusId")) == 0) throw new IllegalArgumentException("The invoice status is pending because the dispatch has not been completed against the invoice"
                    + (tolerance() == 0d ? "" : " or Special Approval is Pending") + ". Please check");
            supplier = toInt(row.get("SupplierCustomerId")); currency = toInt(row.get("FcurrencyId")); invoiceNo = str(row.get("InvoiceNo"));
            compareWeight = toDouble(row.get("InvoiceNetWeight"));
            docNo = toInt(b.get("voucherCode")) > 0 ? toInt(b.get("voucherCode")) : support.generateVoucherCode(DOCUMENT_TYPE_ID);
            for (Map<String, Object> d : DesktopProc.rows(jdbc, "[dbo].[usp_getexportInvoiceDetailByIdForExportVoucher]", DesktopProc.params("OrganizationId", org,
                    "CompanyId", company, "DocDate", date(b.get("voucherDate")), "Id", invoiceId)))
                serverRows.put("k:" + toInt(d.get("EntryTypeId")) + ":" + toInt(d.get("ExImInvoiceDetailId")), d);
        }

        // ---- FormValidation :482 (on the values the read-only controls hold)
        String buyerText = null, currencyText = null;
        for (Map<String, Object> m : buyers(u)) if (toInt(m.get("Id")) == supplier) buyerText = str(m.get("CompanyName"));
        if (supplier == 0 || buyerText == null) throw new IllegalArgumentException("Account Field is Required");
        for (Map<String, Object> m : currencies(u)) if (toInt(m.get("Id")) == currency) currencyText = str(m.get("CurrencyCode"));
        if (currency == 0 || currencyText == null) throw new IllegalArgumentException("Currency Code Field is Required");
        if (invoiceNo.trim().isEmpty() || "0".equals(invoiceNo.trim())) throw new IllegalArgumentException("Invoice No Field is Required");
        BigDecimal exchangeRate = dec(b.get("exchangeRate")), fcyAmount = dec(b.get("fcyAmount")), lcyAmount = dec(b.get("rsAmount"));
        if (exchangeRate.signum() == 0) throw new IllegalArgumentException("ExchangeRate Field is Required");
        if (fcyAmount.signum() == 0) throw new IllegalArgumentException("FcyAmount Field is Required");
        if (lcyAmount.signum() == 0) throw new IllegalArgumentException("RsAmount Field is Required");

        int creditAccount = toInt(b.get("creditAccountId")), commissionAccount = toInt(b.get("commissionExpenseAccountId"));
        if (creditAccount > 0 && !accountAllowed("3,6,8,10", creditAccount)) throw new IllegalArgumentException("Account Field is Required");
        if (commissionAccount > 0 && !accountAllowed("11,20,21", commissionAccount)) throw new IllegalArgumentException("Commission Expense Account required because commission is given in commission info tab!");
        List<Map<String, Object>> commissionIn = maps(b.get("commission"));
        if (commissionAccount == 0 && !commissionIn.isEmpty()) throw new IllegalArgumentException("Commission Expense Account required because commission is given in commission info tab!");

        Hdr o = new Hdr();
        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        o.organizationId = org; o.companyId = company; o.branchesId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        o.financialYearId = context.currentFinancialYearId(); o.entryUserId = userId; o.modifyUserId = userId; o.entryDate = now; o.modifyDate = now;
        o.id = recId; o.autoUpdate = auto ? 1 : 0;
        o.exImInvoiceId = invoiceId; o.invoiceNo = invoiceNo.trim(); o.docNo = docNo; o.docDate = ts(b.get("voucherDate"));
        o.supplierCustomerId = supplier; o.creditAccountId = creditAccount; o.commissionExpenseAccountId = commissionAccount; o.currencyId = currency;
        o.fcyAmountWithoutAddLess = toDouble(b.get("fcyAmountWithoutAddLess")); o.addLessAmount = toDouble(b.get("addLessAmount"));
        o.fcyAmount = fcyAmount; o.exchangeRate = exchangeRate; o.lcyAmount = lcyAmount;
        o.dueDate = ts(b.get("dueDate")); o.dueDays = toInt(b.get("dueDays")); o.remarks = str(b.get("remarks")).trim();

        // ---- grid rows :1498
        List<Map<String, Object>> rowsIn = maps(b.get("rows"));
        if (rowsIn.isEmpty()) throw new IllegalArgumentException("Grid Fields Required");
        boolean cgsFlag = DesktopVoucherSupport.toBool(support.config("CGSEntryAllow"));
        double tolerance = tolerance();
        List<Det> dets = new ArrayList<>();
        Set<String> seen = new HashSet<>();
        BigDecimal detailAddLess = BigDecimal.ZERO;
        double totalNetWeight = 0d;
        for (Map<String, Object> r : rowsIn) {
            Map<String, Object> s;
            if (recId > 0) {
                s = serverRows.get("id:" + toInt(r.get("Id")));
                if (toInt(r.get("Id")) <= 0 || s == null) throw new IllegalArgumentException("The grid row does not belong to this voucher");
                if (!seen.add("id:" + toInt(r.get("Id")))) throw new IllegalArgumentException("The grid row is repeated");
            } else {
                String key = "k:" + toInt(r.get("EntryTypeId")) + ":" + toInt(r.get("ExImInvoiceDetailId"));
                s = serverRows.get(key);
                if (s == null) throw new IllegalArgumentException("The grid row does not belong to the selected invoice");
                if (!seen.add(key)) throw new IllegalArgumentException("The grid row is repeated");
            }
            Det it = new Det();
            it.entryTypeId = toInt(s.get("EntryTypeId")); it.id = recId > 0 ? toInt(s.get("Id")) : 0; it.exImInvoiceDetailId = toInt(s.get("ExImInvoiceDetailId"));
            it.contractId = toInt(s.get("ContractId")); it.cropYearId = toInt(s.get("CropYearId"));
            it.packingTypeId = toInt(recId > 0 ? s.get("PackingTypeId") : s.get("PackingMaterialId"));
            it.itemUomId = toInt(recId > 0 ? s.get("ItemUomId") : s.get("OuterQtyUomId"));
            it.costRate = dec(r.get("CostRate"));
            if (cgsFlag && it.costRate.signum() <= 0) throw new IllegalArgumentException("CostRate Not Found Please Check");
            it.qty = dec(s.get("Qty")); it.stockWeight = dec(s.get("StockWeight")); it.netWeight = dec(s.get("NetWeight"));
            it.stockValue = it.costRate.multiply(it.stockWeight);
            it.itemId = toInt(s.get("ItemId")); it.lotJobId = toInt(s.get("LotJobId")); it.warehouseId = toInt(s.get("WarehouseId"));
            it.mton = it.netWeight.divide(BigDecimal.valueOf(1000));
            it.ratePrice = dec(s.get("RatePrice")); it.rateUomId = toInt(s.get("RateUomId")); it.currencyId = currency; it.exchangeRate = exchangeRate;
            it.fcyAmount = dec(recId > 0 ? s.get("FcyAmount") : s.get("FcAmount")); it.addLess = dec(r.get("AddLessAmount"));
            it.lcyAmount = it.fcyAmount.multiply(it.exchangeRate);
            it.itemName = str(s.get("ItemName")); it.contractNo = str(s.get("ContractNo"));
            String rate = rawDecimal(s.get("RatePrice"));
            if (it.entryTypeId != 1) {
                it.remarks = buyerText + " : " + it.itemName + " : " + it.mton.stripTrailingZeros().toPlainString() + "/MTon : @" + currencyText + " " + rate
                        + " : Invoice No: " + o.invoiceNo + " : Contract No: " + it.contractNo;
            } else {
                it.remarks = buyerText + " : " + it.itemName + " @ " + currencyText + " " + rate + " Invoice No: " + o.invoiceNo;
            }
            dets.add(it);
            detailAddLess = detailAddLess.add(it.addLess);
            totalNetWeight += it.netWeight.doubleValue();
        }
        if (tolerance > 0d) {
            if (Math.floor(compareWeight) > Math.floor(totalNetWeight) + tolerance) throw new IllegalArgumentException("TotalInvoiceWeight should be equal to SumOfGridNetWeight please Check");
        } else if (Math.abs(Math.floor(compareWeight) - Math.floor(totalNetWeight)) > 0.5) {
            throw new IllegalArgumentException("TotalInvoiceWeight not equal to SumOfGridNetWeight please Check");
        }
        o.netWeight = BigDecimal.valueOf(totalNetWeight);
        if (BigDecimal.valueOf(o.addLessAmount).subtract(detailAddLess).abs().compareTo(new BigDecimal("0.3")) > 0)
            throw new IllegalArgumentException("Header AddLess:" + clr(o.addLessAmount) + " not equal to detail AddLess:" + detailAddLess.stripTrailingZeros().toPlainString() + ". Please Check!");

        // ---- payment terms and other charges (the grids hold the invoice's rows) and commission (:1626-1660)
        List<Map<String, Object>> terms = paymentTerms(u, invoiceId);
        List<Map<String, Object>> charges = otherCharges(u, invoiceId);
        Set<Integer> invoiceCommissionIds = new HashSet<>();
        for (Map<String, Object> c : commission(u, invoiceId)) invoiceCommissionIds.add(toInt(c.get("Id")));
        List<Map<String, Object>> commissionOut = new ArrayList<>();
        for (Map<String, Object> c : commissionIn) {
            int cid = toInt(c.get("Id"));
            if (cid > 0 && !invoiceCommissionIds.contains(cid)) throw new AccessDeniedException("The commission row does not belong to this invoice");
            Map<String, Object> m = commissionMap(c, cid, cid <= 0 ? 1 : 2);
            commissionOut.add(m);
        }
        if (recId > 0) {
            for (Map<String, Object> c : maps(b.get("removedCommission"))) {
                int cid = toInt(c.get("Id"));
                if (cid <= 0) continue;
                if (!invoiceCommissionIds.contains(cid)) throw new AccessDeniedException("The commission row does not belong to this invoice");
                Map<String, Object> m = commissionMap(c, cid, 3);
                commissionOut.add(m);
            }
        }

        // ---- ExportVoucher.Save -> MakeVoucherForExportVoucher (before the user ids are zeroed, BLL 0456 :428)
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
        ContraVoucherDto.Head vh = makeVoucher(u, o, dets, terms, charges, commissionOut, lines);
        String proc;
        if (o.id == 0) { o.modifyUserId = 0; proc = "USP_ExportVoucher_Insert"; }
        else { o.entryUserId = 0; proc = "USP_ExportVoucher_Update"; }
        int id = saveChain(u, o, proc, dets, commissionOut, vh, lines, b, recId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id); out.put("docNo", o.docNo); out.put("updated", recId > 0); out.put("auto", auto);
        out.put("message", auto ? "" : (recId == 0 ? "Voucher Save Successfully...[" + o.docNo + "]" : "Voucher Update Successfully...[" + o.docNo + "]"));
        return out;
    }

    /** ExImInvoiceSaleManCommission model from a grid row (:1626 and the delete handler :1090). */
    private static Map<String, Object> commissionMap(Map<String, Object> c, int cid, int action) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("exImInvoiceSaleManCommissionId", cid); m.put("ActionTypeId", action); m.put("SalesPersonId", toInt(c.get("SalesPersonId")));
        m.put("SalesPerson", str(c.get("SalesPerson"))); m.put("commissionTypeId", toInt(c.get("CommissionTypeId"))); m.put("CommissionType", str(c.get("CommissionType")));
        m.put("commissionRate", toDouble(c.get("Rate"))); m.put("rateUom", str(c.get("RateUom"))); m.put("fcyAmount", toDouble(c.get("FcyAmount")));
        m.put("exchangeRate", toDouble(c.get("ExchangeRate"))); m.put("lcyAmount", toDouble(c.get("LcyAmount"))); m.put("Remarks", str(c.get("Remarks")));
        return m;
    }

    private boolean accountAllowed(String typeIds, int accountId) {
        for (Map<String, Object> m : accounts(typeIds)) if (toInt(m.get("Id")) == accountId) return true;
        return false;
    }

    // ================================================================================ MakeVoucherForExportVoucher (BLL 0456 :20)

    private ContraVoucherDto.Head makeVoucher(UserAccount u, Hdr o, List<Det> dets, List<Map<String, Object>> terms, List<Map<String, Object>> charges,
                                              List<Map<String, Object>> commissions, List<ContraVoucherDto.Detail> lines) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.DocumentTypeId = DOCUMENT_TYPE_ID; vh.DocumentTypeSrNo = o.id; vh.RefDocNoId = o.exImInvoiceId; vh.VoucherCode = o.docNo;
        vh.VoucherDate = sql(o.docDate); vh.Remarks = o.remarks; vh.RemarksOtherLingo = ""; vh.ManualBillNo = o.invoiceNo;
        vh.VoucherAmount = o.lcyAmount.doubleValue(); vh.FcAmount = o.fcyAmount.doubleValue(); vh.ExchangeCurrencyRate = o.exchangeRate.doubleValue();
        vh.MultiCurrencyId = o.currencyId; vh.ChequeDate = LocalDate.now() + " 00:00:00"; vh.IncludeWHT = false; vh.BranchId = o.branchesId; vh.ProjectId = 0;
        vh.BillAmount = o.lcyAmount.doubleValue(); vh.DueDate = sql(o.dueDate); vh.DueDays = o.dueDays;
        vh.OrganizationId = o.organizationId; vh.CompanyId = o.companyId; vh.FinancialYearId = o.financialYearId; vh.EntryUser = o.entryUserId;
        String now = SQLSTAMP.format(LocalDateTime.now());
        vh.EntryDate = now; vh.ModifyDate = now; vh.ModifyUser = o.modifyUserId; vh.ActionId = 1;

        Map<Integer, Map<String, Object>> items = itemGl(u);
        List<Map<String, Object>> customers = lookups.customerGlAccounts(u);
        if (!customers.isEmpty()) {
            Map<String, Object> hit = null;
            for (Map<String, Object> c : customers) if (toInt(c.get("Id")) == o.supplierCustomerId) { hit = c; break; }
            if (hit == null) throw new IllegalArgumentException("Customer GLAccountId not Found");
            vh.RefAccountId = toInt(hit.get("GlAccountId"));
        }
        final int ref = vh.RefAccountId;
        boolean flag = DesktopVoucherSupport.toBool(support.config("FinancialisActiveonCommercialInvoice"));
        boolean flag2 = DesktopVoucherSupport.toBool(support.config("ExportAddlessAmountChargeToSalesAccount"));
        boolean flag3 = DesktopVoucherSupport.toBool(support.config("CGSEntryAllow"));
        boolean fifo = support.feature(5);
        boolean flag4 = DesktopVoucherSupport.toBool(support.config("SaleCostingJobOrderWise"));
        int num = 1;
        for (Det item : dets) {
            Map<String, Object> gl = items.get(item.itemId);
            if (gl != null) {
                int sale = toInt(gl.get("SaleGLAC")), cogs = toInt(gl.get("COGSGLAC")), purchase = toInt(gl.get("PurchaseGLAC"));
                if (!flag) {
                    BigDecimal num2 = item.addLess.multiply(o.exchangeRate);
                    item.lcyAmount = flag2 ? item.lcyAmount.add(num2) : item.lcyAmount;
                    item.lineId = num;
                    int target = o.creditAccountId > 0 ? o.creditAccountId : sale;
                    ContraVoucherDto.Detail d1 = new ContraVoucherDto.Detail();
                    d1.AccountId = ref; d1.AgainstAccountId = target; d1.DebitAmount = item.lcyAmount.doubleValue();
                    ContraVoucherDto.Detail d2 = new ContraVoucherDto.Detail();
                    d2.AccountId = target; d2.AgainstAccountId = ref; d2.CreditAmount = item.lcyAmount.doubleValue();
                    for (ContraVoucherDto.Detail d : new ContraVoucherDto.Detail[] { d1, d2 }) {
                        d.JobLotId = item.lotJobId; d.Comments = item.remarks; d.IsTaxable = "False"; d.QtyOut = item.qty.doubleValue();
                        d.WeightOut = item.netWeight.doubleValue(); d.ItemRate = item.ratePrice.doubleValue(); d.ItemId = item.itemId;
                        d.ItemAmount = item.lcyAmount.doubleValue(); d.DMultiCurrencyId = item.currencyId; d.DExchangeCurrencyRate = item.exchangeRate.doubleValue();
                        d.DCurrencyAmount = item.fcyAmount.doubleValue() + item.addLess.doubleValue(); d.SupplierCustomerId = o.supplierCustomerId; d.LineId = num;
                        lines.add(d);
                    }
                }
                double num3 = 0d;
                String text = "";
                if (flag3 || flag4) {
                    if (flag4) {
                        item.costRate = BigDecimal.valueOf(avgRate(u, true, o.docDate, o.id > 0 ? DOCUMENT_TYPE_ID : 0, o.id, item.itemId, item.lotJobId, 0, 0));
                        if (item.costRate.signum() <= 0) item.costRate = BigDecimal.ONE;
                    } else {
                        item.costRate = BigDecimal.valueOf(avgRate(u, false, o.docDate, o.id > 0 ? DOCUMENT_TYPE_ID : 0, o.id, item.itemId, item.lotJobId, item.cropYearId, item.warehouseId));
                    }
                }
                if (item.costRate.signum() > 0 && !fifo) {
                    num3 = item.stockWeight.doubleValue() * item.costRate.doubleValue();
                    item.stockValue = BigDecimal.valueOf(num3);
                    text = "Item: " + str(gl.get("ItemName")) + ", Qty: " + item.qty.toPlainString() + ", Weight: " + item.stockWeight.toPlainString() + ", Rate:"
                            + item.costRate.toPlainString() + " Item Amount: " + clr(num3);
                    int dAcc = (flag3 || flag4) ? cogs : (o.creditAccountId > 0 ? o.creditAccountId : cogs);
                    int cAgainst = dAcc;
                    ContraVoucherDto.Detail d3 = new ContraVoucherDto.Detail();
                    d3.AccountId = dAcc; d3.AgainstAccountId = purchase; d3.DebitAmount = num3;
                    ContraVoucherDto.Detail d4 = new ContraVoucherDto.Detail();
                    d4.AccountId = purchase; d4.AgainstAccountId = cAgainst; d4.CreditAmount = num3;
                    for (ContraVoucherDto.Detail d : new ContraVoucherDto.Detail[] { d3, d4 }) {
                        d.Comments = text; d.ItemId = item.itemId; d.JobLotId = item.lotJobId; d.QtyOut = item.qty.doubleValue(); d.OrderNo = item.contractId;
                        d.WeightOut = item.stockWeight.doubleValue(); d.ItemRate = item.costRate.doubleValue(); d.ItemCgsRate = item.costRate.doubleValue();
                        d.ItemAmount = num3; d.SupplierCustomerId = o.supplierCustomerId; d.RefDocSubIdNo = item.id; d.IsCGS = 1; d.LineId = num;
                        lines.add(d);
                    }
                }
            }
            num++;
        }
        if (!flag) {
            if (!flag2) otherChargesFinancial(o, ref, charges, lines);
            paymentDetailFinancial(u, o, ref, terms, lines);
            commissionFinancial(o, commissions, lines);
        }
        return vh;
    }

    /** CommonServies.GetItemListForFinancialEffects - Sp_Item_GetAllMethod 'GetItemGlIdsandItemName' (Id, ItemName, SaleGLAC, COGSGLAC, PurchaseGLAC). */
    private Map<Integer, Map<String, Object>> itemGl(UserAccount u) {
        Map<Integer, Map<String, Object>> m = new HashMap<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", DesktopProc.params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Activity", "GetItemGlIdsandItemName"))) m.putIfAbsent(toInt(r.get("Id")), r);
        return m;
    }

    private double avgRate(UserAccount u, boolean jobWise, Timestamp docDate, int docTypeId, int recId, int itemId, int lotJobId, int cropYearId, int warehouseId) {
        if (jobWise) {
            Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocDate", docDate,
                    "JobLotId", lotJobId, "ItemId", itemId);
            if (docTypeId != 0) p.put("DocumentTypeId", docTypeId);
            if (recId != 0) p.put("RecId", recId);
            List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "usp_getAvgRateByJobOrder", p);
            return rows.isEmpty() ? 0d : toDouble(rows.get(0).get("AvgRate"));
        }
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId, "DocDate", docDate);
        if (docTypeId != 0) p.put("DocumentTypeId", docTypeId);
        if (recId != 0) p.put("RecId", recId);
        if (lotJobId != 0) p.put("JobLotId", lotJobId);
        if (cropYearId != 0) p.put("CropYearId", cropYearId);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        p.put("Activity", "GetOnlyAvgRateForCGS");
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        return rows.isEmpty() ? 0d : toDouble(rows.get(0).get("AvgRate"));
    }

    /** ExportOtherChargesFinancial (BLL 0456 :267). */
    private void otherChargesFinancial(Hdr o, int ref, List<Map<String, Object>> charges, List<ContraVoucherDto.Detail> lines) {
        for (Map<String, Object> c : charges) {
            double add = toDouble(c.get("AddAmount")), less = toDouble(c.get("LessAmount"));
            if (add == 0d && less == 0d) continue;
            int acct = toInt(c.get("ChargesAccountId"));
            if (acct == 0) throw new IllegalArgumentException("Charges Account not found");
            double amt = (add > 0d ? add : less);
            double num = amt * o.exchangeRate.doubleValue();
            String remarks = str(c.get("Remarks"));
            String comments = !remarks.isEmpty() ? remarks + " " + str(c.get("ChargesAccount")) : "Export Charges " + str(c.get("ChargesAccount"));
            ContraVoucherDto.Detail d1 = new ContraVoucherDto.Detail();
            d1.AccountId = add > 0d ? ref : acct; d1.AgainstAccountId = add > 0d ? acct : ref; d1.DebitAmount = num;
            ContraVoucherDto.Detail d2 = new ContraVoucherDto.Detail();
            d2.AccountId = add > 0d ? acct : ref; d2.AgainstAccountId = add > 0d ? ref : acct; d2.CreditAmount = num;
            for (ContraVoucherDto.Detail d : new ContraVoucherDto.Detail[] { d1, d2 }) {
                d.Comments = comments; d.DMultiCurrencyId = o.currencyId; d.DExchangeCurrencyRate = o.exchangeRate.doubleValue(); d.DCurrencyAmount = amt;
                d.SubsidiaryTypeId = 1; d.SubsidiaryAccountId = o.supplierCustomerId; d.SupplierCustomerId = o.supplierCustomerId; d.BranchesId = o.branchesId;
                lines.add(d);
            }
        }
    }

    /** ExportPaymentDetailFinancial (BLL 0456 :312) - exchange rate gain / loss against the FCY receipt behind each PaymentTermId 1 row. */
    private void paymentDetailFinancial(UserAccount u, Hdr o, int ref, List<Map<String, Object>> terms, List<ContraVoucherDto.Detail> lines) {
        for (Map<String, Object> t : terms) {
            if (toInt(t.get("PaymentTermId")) != 1) continue;
            String fi = str(t.get("FinancialInstrumentNo"));
            List<Map<String, Object>> r = DesktopProc.rows(jdbc, "[dbo].[usp_getExchangeRateAndGainLossAccountFcyReceipts]", DesktopProc.params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", toInt(t.get("DocumentTypeId")),
                    "FcyReceiptsId", toInt(t.get("ExImEFormRegistrationId"))));
            if (r.isEmpty()) throw new IllegalArgumentException("ExchangeRate or GainLossAccountId not found Against This FI " + fi);
            int gainLoss = toInt(r.get(0).get("GainLossAccountId"));
            BigDecimal rate = dec(r.get(0).get("ExchangeRate"));
            if (rate.signum() == 0) throw new IllegalArgumentException("ExchangeRate not found Against This FI " + fi);
            if (gainLoss == 0) throw new IllegalArgumentException("GainLossAccountId not found Against This FI " + fi);
            BigDecimal diff = o.exchangeRate.subtract(rate);
            if (diff.signum() == 0) continue;
            double amount = diff.doubleValue() * toDouble(t.get("FcyAmount"));
            String remarks = "Export Invoice ExchangeRate is " + o.exchangeRate.toPlainString() + " and Fcy Receipts ExchangeRate is " + rate.toPlainString() + " " + str(t.get("Remarks"));
            if (diff.signum() > 0) addEntry(lines, gainLoss, ref, amount, remarks, o.supplierCustomerId, o.branchesId);
            else addEntry(lines, ref, gainLoss, amount, remarks, o.supplierCustomerId, o.branchesId);
        }
    }

    private static void addEntry(List<ContraVoucherDto.Detail> lines, int debit, int credit, double amount, String remarks, int subsidiary, int branch) {
        ContraVoucherDto.Detail d1 = new ContraVoucherDto.Detail();
        d1.AccountId = debit; d1.AgainstAccountId = credit; d1.DebitAmount = amount;
        ContraVoucherDto.Detail d2 = new ContraVoucherDto.Detail();
        d2.AccountId = credit; d2.AgainstAccountId = debit; d2.CreditAmount = amount;
        for (ContraVoucherDto.Detail d : new ContraVoucherDto.Detail[] { d1, d2 }) {
            d.Comments = remarks; d.SubsidiaryTypeId = 1; d.SubsidiaryAccountId = subsidiary; d.SupplierCustomerId = subsidiary; d.BranchesId = branch;
            lines.add(d);
        }
    }

    /** ExportCommissionAgentFinancial (BLL 0456 :351) - over every row of the commission list, deleted rows included, as the desktop does. */
    private void commissionFinancial(Hdr o, List<Map<String, Object>> commissions, List<ContraVoucherDto.Detail> lines) {
        UserAccount u = user();
        List<Map<String, Object>> customers = lookups.customerGlAccounts(u);
        for (Map<String, Object> c : commissions) {
            int person = toInt(c.get("SalesPersonId"));
            if (person == 0) throw new IllegalArgumentException("Sales person Account not found");
            if (o.commissionExpenseAccountId == 0) throw new IllegalArgumentException("Commission Expenses Account not found");
            Map<String, Object> hit = null;
            for (Map<String, Object> g : customers) if (toInt(g.get("Id")) == person) { hit = g; break; }
            if (hit == null) throw new IllegalArgumentException("Sales person GLAccountId not Found");
            int gl = toInt(hit.get("GlAccountId"));
            double fcy = toDouble(c.get("fcyAmount"));
            double num2 = fcy * o.exchangeRate.doubleValue();
            String remarks = str(c.get("Remarks"));
            String comments = !remarks.isEmpty() ? remarks + " " + str(c.get("SalesPerson"))
                    : "CommissionType " + str(c.get("CommissionType")) + " Commission Rate " + clr(toDouble(c.get("commissionRate"))) + " " + str(c.get("SalesPerson"));
            ContraVoucherDto.Detail d1 = new ContraVoucherDto.Detail();
            d1.AccountId = o.commissionExpenseAccountId; d1.AgainstAccountId = gl; d1.DebitAmount = num2;
            ContraVoucherDto.Detail d2 = new ContraVoucherDto.Detail();
            d2.AccountId = gl; d2.AgainstAccountId = o.commissionExpenseAccountId; d2.CreditAmount = num2;
            d2.SubsidiaryTypeId = 1; d2.SubsidiaryAccountId = person; d2.SupplierCustomerId = person;
            for (ContraVoucherDto.Detail d : new ContraVoucherDto.Detail[] { d1, d2 }) {
                d.Comments = comments; d.DMultiCurrencyId = o.currencyId; d.DExchangeCurrencyRate = o.exchangeRate.doubleValue(); d.DCurrencyAmount = fcy;
                d.BranchesId = o.branchesId;
                lines.add(d);
            }
        }
    }

    // ================================================================================ DAL ExportVoucher.SetDate (DAL 0508 :19)

    private static final Set<String> STOCK_PARAMS = new HashSet<>(java.util.Arrays.asList(("id refdocumenttypeid refdocidno refrefdocumenttypeid refrefdocidno doccodeno docdate "
            + "suppliercustomerid refrefdocsubidno refdocsubidno orderno warehouseid prdjoborderno vehicleno gpnodcno tranremarks itemid itemuom cropbatch joblotid invpackingtypeid "
            + "qtyin qtyout billweightin billweightout stockweightin stockweightout calctype itemrate rateuom amountin expenseamountin amountout cgsrate cgsamount isapproved "
            + "organizationid companyid branchesid projectsid entrydate entryuser modifydate modifyuser lineid cityid otherdocumenttypeid otherdocnoid othersubdocnoid invoiceid "
            + "invoicedetailid varientid refwarehouseid biltyno validforsale itemconditionid").split(" ")));
    private static final Set<String> STOCK_NUMERIC = new HashSet<>(java.util.Arrays.asList(("id refdocumenttypeid refdocidno refrefdocumenttypeid refrefdocidno suppliercustomerid "
            + "refrefdocsubidno refdocsubidno orderno warehouseid itemid itemuom joblotid invpackingtypeid qtyin qtyout billweightin billweightout stockweightin stockweightout "
            + "itemrate rateuom amountin expenseamountin amountout cgsrate cgsamount organizationid companyid branchesid projectsid entryuser modifyuser lineid cityid "
            + "otherdocumenttypeid otherdocnoid othersubdocnoid invoiceid invoicedetailid varientid refwarehouseid itemconditionid").split(" ")));

    private int saveChain(UserAccount u, Hdr o, String proc, List<Det> dets, List<Map<String, Object>> commissions, ContraVoucherDto.Head vh,
                          List<ContraVoucherDto.Detail> lines, Map<String, Object> body, int recId) {
        // attachments: validate and store the new files first (their rollback hook is registered by the store)
        Map<String, Object> att = body.get("attachments") instanceof Map ? castMap(body.get("attachments")) : new HashMap<String, Object>();
        List<Map<String, Object>> keep = new ArrayList<>();
        if (recId > 0) {
            Set<Integer> want = new HashSet<>();
            Object kv = att.get("keep");
            if (kv instanceof Collection) for (Object k : (Collection<?>) kv) want.add(toInt(k));
            if (!want.isEmpty()) for (Map<String, Object> a : attachments(recId)) if (want.contains(toInt(a.get("Id")))) keep.add(a);
        }
        List<Map<String, Object>> add = maps(att.get("add"));
        if (add.size() > 10) throw new IllegalArgumentException("Select at most ten files at once");
        int removedCount = toInt(att.get("removed"));

        int num3 = DesktopProc.setProc(jdbc, proc, headerParams(o));
        if (num3 > 0) o.id = num3; else num3 = o.id;
        int line = 1;
        for (Det d : dets) {
            d.lineId = line; d.exportVoucherId = o.id;
            d.id = DesktopProc.setProc(jdbc, "USP_ExportVoucherDetail_Insert", detailParams(d));
            line++;
        }
        for (Map<String, Object> c : commissions) {
            Map<String, Object> p = new LinkedHashMap<>(c);
            p.remove("SalesPerson"); p.remove("CommissionType");
            p.put("exImInvoiceId", o.exImInvoiceId);
            DesktopProc.setProc(jdbc, "USP_exImInvoiceSaleManCommission_Insert", p);
        }
        if (!keep.isEmpty() || !add.isEmpty()) {
            DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params("ScreenName", SCREEN_NAME, "Id", num3, "Activity", "DeleteById"));
            Timestamp now = Timestamp.valueOf(LocalDateTime.now());
            for (Map<String, Object> a : keep) insertAttachment(o, num3, now, str(a.get("Attachment")), str(a.get("CustomName")), 0d);
            for (Map<String, Object> up : add) {
                String name = str(up.get("name"));
                DesktopAttachmentStore.validateName(name);
                byte[] bytes = decode(str(up.get("base64")));
                String stored = attachmentStore.store(u, name, bytes);
                insertAttachment(o, num3, now, name, stored, bytes.length / 1048576d);
            }
        }

        Map<Integer, Map<String, Object>> items = itemGl(u);
        if (support.feature(5)) {
            List<Map<String, Object>> stock = new ArrayList<>();
            for (Det item2 : dets) {
                if (item2.entryTypeId == 1) continue;
                Map<String, Object> p = DesktopProc.params("OrganizationId", o.organizationId, "CompanyId", o.companyId, "OtherDocumentTypeId", 205,
                        "InvoiceId", o.exImInvoiceId, "InvoiceDetailId", item2.exImInvoiceDetailId, "ItemId", item2.itemId, "ItemUomId", item2.itemUomId,
                        "JobLotId", item2.lotJobId, "PackingTypeId", item2.packingTypeId, "WarehouseId", item2.warehouseId);
                if (item2.cropYearId > 0) p.put("CropYearId", item2.cropYearId);
                List<Map<String, Object>> list2 = DesktopProc.rows(jdbc, "[dbo].[USP_GetInventoryStockEvalautionDetailByOtherIdsAndStockIdsForExportInoice]", p);
                if (list2.isEmpty()) throw new IllegalArgumentException("InventoryStockEvalautionDetailslist not found against CGS other reference");
                for (Map<String, Object> row : list2) {
                    Map<String, Object> gl = items.get(item2.itemId);
                    if (gl == null) throw new IllegalArgumentException("ItemId Not Found Against CGS Transaction");
                    double qtyOut = toDouble(row.get("QtyOut")), wOut = toDouble(row.get("StockWeightOut")), cgsRate = toDouble(row.get("CgsRate")), cgsAmount = toDouble(row.get("CgsAmount"));
                    String text2 = "ItemQty: " + clr(qtyOut) + " " + str(gl.get("ItemName")) + " Net Weight: " + clr(wOut) + " CGS Rate:" + clr(cgsRate);
                    ContraVoucherDto.Detail v1 = new ContraVoucherDto.Detail();
                    v1.AccountId = toInt(gl.get("COGSGLAC")); v1.AgainstAccountId = toInt(gl.get("PurchaseGLAC")); v1.DebitAmount = cgsAmount; v1.ItemAmount = toDouble(row.get("AmountOut"));
                    ContraVoucherDto.Detail v2 = new ContraVoucherDto.Detail();
                    v2.AccountId = toInt(gl.get("PurchaseGLAC")); v2.AgainstAccountId = toInt(gl.get("COGSGLAC")); v2.CreditAmount = cgsAmount;
                    for (ContraVoucherDto.Detail v : new ContraVoucherDto.Detail[] { v1, v2 }) {
                        v.LineId = item2.lineId; v.IsCGS = 1; v.Comments = text2; v.ItemId = item2.itemId; v.QtyOut = qtyOut; v.WeightOut = wOut; v.ItemCgsRate = cgsRate;
                        v.JobLotId = item2.lotJobId; v.SupplierCustomerId = o.supplierCustomerId; v.RefDocumentTypeId = toInt(row.get("RefRefDocumentTypeId"));
                        v.RefDocNoId = toInt(row.get("RefRefDocIdNo")); v.RefDocNoDetailId = toInt(row.get("RefRefDocSubIdNo"));
                        lines.add(v);
                    }
                    Map<String, Object> sd = stockParams(row);
                    sd.put("OrganizationId", o.organizationId); sd.put("CompanyId", o.companyId); sd.put("SupplierCustomerId", o.supplierCustomerId);
                    sd.put("BranchesId", o.branchesId); sd.put("RefDocumentTypeId", DOCUMENT_TYPE_ID); sd.put("EntryUser", o.entryUserId); sd.put("ModifyUser", o.modifyUserId);
                    sd.put("CalcType", "Weight");
                    Det found = null;
                    for (Det x : dets) if (x.exImInvoiceDetailId == toInt(row.get("InvoiceDetailId")) && toInt(row.get("OtherDocumentTypeId")) == 205) { found = x; break; }
                    if (found != null) {
                        double eq = equivalent(u, item2.itemId, found.rateUomId);
                        if (toDouble(row.get("BillWeightOut")) > 0d && found.ratePrice.signum() > 0 && eq > 0d) {
                            double rate = found.ratePrice.doubleValue() / eq * o.exchangeRate.doubleValue();
                            sd.put("AmountOut", toDouble(row.get("BillWeightOut")) * rate);
                            sd.put("ItemRate", rate);
                        }
                        sd.put("RefDocIdNo", found.exportVoucherId); sd.put("RefDocSubIdNo", found.id);
                    }
                    stock.add(sd);
                }
            }
            for (Map<String, Object> sd : stock) DesktopProc.setProc(jdbc, "USP_InventoryStockEvalautionDetailGdnReferences_Update", sd);
        } else {
            DesktopProc.scalar(jdbc, "usp_StockEvaluationUpdateForExportForwarding", DesktopProc.params("OrganizationId", o.organizationId, "CompanyId", o.companyId,
                    "RefDocumentTypeId", DOCUMENT_TYPE_ID, "RefDocIdNo", o.id, "ActionId", 2));
        }
        DesktopProc.scalar(jdbc, "usp_StockInTransit_VoucherDelete_ByExportVoucherId", DesktopProc.params("Id", o.id));

        if (!lines.isEmpty()) {
            Map<String, Object> head = model(vh);
            int existing = lookups.voucherHeadId(u, DOCUMENT_TYPE_ID, o.id);
            if (existing > 0) head.put("Id", existing);
            head.put("DocumentTypeSrNo", o.id);
            int n = DesktopProc.setProc(jdbc, existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", head);
            if (n > 0) head.put("Id", n);
            int vhId = toInt(head.get("Id"));
            List<Map<String, Object>> detailMaps = new ArrayList<>();
            for (ContraVoucherDto.Detail d : lines) {
                Map<String, Object> m = model(d);
                if (d.LineId != null && d.LineId > 0) for (Det x : dets) if (x.lineId == d.LineId) { m.put("RefDocSubIdNo", x.id); break; }
                m.put("VoucherHeadId", vhId); m.put("BranchesId", o.branchesId);
                DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", m);
                detailMaps.add(m);
            }
            if (vhId > 0) DesktopProc.scalar(jdbc, "USP_VoucherBalanceCheck", DesktopProc.params("OrganizationId", o.organizationId, "CompanyId", o.companyId, "Id", vhId));
            int ref = DesktopProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", head);
            for (Map<String, Object> m : detailMaps) {
                m.put("VoucherHeadId", vhId); m.put("DocumentTypeIdRef", ref);
                DesktopProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", m);
            }
        }
        if (recId > 0 && removedCount > 0 && keep.isEmpty() && add.isEmpty()) {
            DesktopProc.rows(jdbc, "[dbo].[Sp_DMSAttachments_GetAllMethod]", DesktopProc.params("Id", recId, "ScreenName", SCREEN_NAME, "RefDocumentTypeId", DOCUMENT_TYPE_ID,
                    "Activity", "RemoveByIdAndName"));
        }
        return num3;
    }

    private void insertAttachment(Hdr o, int docId, Timestamp now, String name, String stored, double mb) {
        Map<String, Object> p = DesktopProc.params("Id", 0, "RefAccountId", o.supplierCustomerId, "DMSFoldersLabelsId", 0, "RefDocumentTypeId", DOCUMENT_TYPE_ID,
                "RefDocumentNo", docId, "Attachment", name, "EntryDate", now, "EntryUser", o.entryUserId, "ModifyDate", now, "ModifyUser", o.modifyUserId,
                "OrganizationId", o.organizationId, "CompanyId", o.companyId, "BranchId", 0, "ScreenName", SCREEN_NAME, "DetailWiseAttachment", Boolean.FALSE,
                "UploadedFileCustomName", stored == null || stored.isEmpty() ? null : stored, "UploadedFileSizeMb", mb, "LineId", 0);
        DesktopProc.setProc(jdbc, "Proc_DMSAttachments_Insert", p);
    }

    private static byte[] decode(String base64) {
        if (base64.isEmpty() || base64.length() > 7 * 1024 * 1024) throw new IllegalArgumentException("Attachment exceeds 5 MB");
        byte[] bytes;
        try { bytes = Base64.getDecoder().decode(base64); } catch (IllegalArgumentException e) { throw new IllegalArgumentException("Invalid attachment content"); }
        if (bytes.length == 0 || bytes.length > DesktopAttachmentStore.MAX_BYTES) throw new IllegalArgumentException("Attachment must be between 1 byte and 5 MB");
        return bytes;
    }

    /** CommonServices.GetEqvilentByItemIdAndUomScheduleId - Sp_Item_GetAllMethod 'GetEqvilentByItemIdAndUomScheduleId', the Equivalent column. */
    private double equivalent(UserAccount u, int itemId, int scheduleId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "ScheduleId", scheduleId, "Activity", "GetEqvilentByItemIdAndUomScheduleId"));
        return rows.isEmpty() ? 0d : toDouble(rows.get(0).get("Equivalent"));
    }

    /** The InventoryStockEvalautionDetail model: only its 56 procedure parameters, NULL numerics as the model's 0. */
    private static Map<String, Object> stockParams(Map<String, Object> row) {
        Map<String, Object> m = new java.util.TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            String k = e.getKey().toLowerCase();
            if (!STOCK_PARAMS.contains(k)) continue;
            Object v = e.getValue();
            if (v == null && STOCK_NUMERIC.contains(k)) v = 0;
            if (v != null) m.put(e.getKey(), v);
        }
        return m;
    }

    private Map<String, Object> headerParams(Hdr o) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", o.id); m.put("ExImInvoiceId", o.exImInvoiceId); m.put("DocNo", o.docNo); m.put("DocDate", o.docDate); m.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        m.put("Remarks", o.remarks); m.put("SupplierCustomerId", o.supplierCustomerId); m.put("CreditAccounId", o.creditAccountId); m.put("CurrencyId", o.currencyId);
        m.put("ExchangeRate", o.exchangeRate); m.put("FcyAmount", o.fcyAmount); m.put("LcyAmount", o.lcyAmount); m.put("NetWeight", o.netWeight);
        m.put("EntryDate", o.entryDate); m.put("EntryUserId", o.entryUserId); m.put("ModifyDate", o.modifyDate); m.put("ModifyUserId", o.modifyUserId);
        m.put("IsApproved", Boolean.FALSE); m.put("OrganizationId", o.organizationId); m.put("CompanyId", o.companyId); m.put("BranchesId", o.branchesId);
        m.put("FinancialYearId", o.financialYearId); m.put("InvoiceNo", o.invoiceNo); m.put("DueDays", o.dueDays); m.put("DueDate", o.dueDate);
        m.put("FcyAmountWithoutAddLess", o.fcyAmountWithoutAddLess); m.put("AddLessAmount", o.addLessAmount);
        m.put("CommissionExpenseAccountId", o.commissionExpenseAccountId); m.put("IsUploaded", Boolean.FALSE); m.put("IsSpecialApproved", Boolean.FALSE);
        m.put("IsSpecialApprovalRequired", 0); m.put("SpecialApprovalUserId", 0); m.put("AutoUpdate", o.autoUpdate);
        return m;
    }

    private Map<String, Object> detailParams(Det d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", d.id); m.put("ExportVoucherId", d.exportVoucherId); m.put("ExImInvoiceDetailId", d.exImInvoiceDetailId); m.put("ContractId", d.contractId);
        m.put("ItemId", d.itemId); m.put("WarehouseId", d.warehouseId); m.put("CropYearId", d.cropYearId); m.put("LotJobId", d.lotJobId); m.put("PackingTypeId", d.packingTypeId);
        m.put("Qty", d.qty); m.put("ItemUomId", d.itemUomId); m.put("NetWeight", d.netWeight); m.put("Mton", d.mton); m.put("RatePrice", d.ratePrice);
        m.put("RateUomId", d.rateUomId); m.put("CurrencyId", d.currencyId); m.put("FcyAmount", d.fcyAmount); m.put("ExchangeRate", d.exchangeRate);
        m.put("LcyAmount", d.lcyAmount); m.put("StockWeight", d.stockWeight); m.put("CostRate", d.costRate); m.put("StockValue", d.stockValue); m.put("LineId", d.lineId);
        m.put("Remarks", d.remarks); m.put("EntryTypeId", d.entryTypeId); m.put("AddLessAmount", d.addLess);
        return m;
    }

    // ================================================================================ Delete (BtnDelete_Click :1830)

    /**
     * Approved -> "Record cannot be Delete, because ots approved"; RecId 0 -> "RecordId Not Found....."; otherwise InvPurchaseInvoice.RemoveByID ->
     * Sp_InvoicesVouchersandStocksDelete(org, company, DocumentTypeId 27, Id, user). The approved state is read from the database, not the request.
     */
    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> delete(Map<String, Object> b) {
        UserAccount u = user();
        if (!Boolean.TRUE.equals(support.rights(SCREEN_NAME).get("delete"))) throw new AccessDeniedException("You do not have the Delete right on this screen");
        int id = toInt(b.get("recId"));
        if (id <= 0) throw new IllegalArgumentException("RecordId Not Found.....");
        Map<String, Object> h = ownVoucher(u, id);
        if (DesktopVoucherSupport.toBool(h.get("IsApproved"))) throw new IllegalArgumentException("Record cannot be Delete, because ots approved");
        DesktopProc.scalar(jdbc, "Sp_InvoicesVouchersandStocksDelete", DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOCUMENT_TYPE_ID, "Id", id, "UserId", context.currentUserId()));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("message", "Delete Record Successfully");
        return out;
    }

    // ================================================================================ helpers

    /** Public fields in declaration order (GenericProvider.SetProc): the voucher DTOs; date strings become timestamps. */
    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>(DesktopVoucherSupport.fields(o));
        for (String n : new String[] { "VoucherDate", "ChequeDate", "DueDate", "EntryDate", "ModifyDate", "PostDate", "DCheqDate", "GpDate" }) {
            Object v = m.get(n);
            if (v != null) { String s = String.valueOf(v); m.put(n, Timestamp.valueOf(s.length() == 10 ? s + " 00:00:00" : s)); }
        }
        return m;
    }

    @SuppressWarnings("unchecked")
    private static Map<String, Object> castMap(Object v) { return (Map<String, Object>) v; }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> maps(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof Collection) for (Object o : (Collection<?>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    private static String sql(Timestamp t) { return t == null ? null : SQLSTAMP.format(t.toLocalDateTime().withNano(0)); }

    private static java.sql.Date date(Object v) {
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Invalid date: " + s); }
    }

    /** A DateTimePicker value: the chosen date with the current time of day. */
    private static Timestamp ts(Object v) {
        java.sql.Date d = date(v);
        LocalDate day = d == null ? LocalDate.now() : d.toLocalDate();
        return Timestamp.valueOf(LocalDateTime.of(day, LocalTime.now().withNano(0)));
    }

    /** Conversion.ToDecimal: blank / non-numeric -> 0. */
    private static BigDecimal dec(Object v) {
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Number) return new BigDecimal(String.valueOf(v));
        String s = v == null ? "" : String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(s); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    private static String rawDecimal(Object v) {
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Double || v instanceof Float) return clr(((Number) v).doubleValue());
        return str(v);
    }

    private static int toInt(Object v) { return DesktopVoucherSupport.toInt(v); }
    private static double toDouble(Object v) { return DesktopVoucherSupport.toDouble(v); }
    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** double.ToString(): integral values without a decimal part, otherwise up to 15 significant digits. */
    private static String clr(double d) {
        if (Double.isNaN(d) || Double.isInfinite(d)) return "0";
        if (d == Math.rint(d) && Math.abs(d) < 1e15) return String.valueOf((long) d);
        return new BigDecimal(d).round(new java.math.MathContext(15, RoundingMode.HALF_EVEN)).stripTrailingZeros().toPlainString();
    }

    private static Object plain(Object v) {
        if (v instanceof Timestamp) return STAMP.format(((Timestamp) v).toLocalDateTime());
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + "T00:00:00";
        if (v instanceof LocalDateTime) return STAMP.format((LocalDateTime) v);
        if (v instanceof BigDecimal) return ((BigDecimal) v).doubleValue();
        return v;
    }
}
