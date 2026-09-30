package com.mst.services.desktopvoucher;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Propagation;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Shared desktop plumbing for the Accounts Transaction screens rebuilt by recheck group C
 * (16 frmPartyPaymentVoucher, 17 frmPartyReceiptVoucher, 25 AccountsPrematurePaymentsReceipts,
 * 38 frmBillsPayables, 39 frmBillsReceivables).
 *
 * Every call below is the procedure the desktop calls, with the parameter set the desktop's BLL
 * builds (a parameter the BLL only adds when non-zero / non-empty is only added here under the
 * same condition). Nulls follow ADO.NET AddWithValue semantics through {@link DesktopProc}: a
 * CLR null is OMITTED, so the procedure's own default applies.
 *
 * Tenancy (organization, company, branch, financial year, user, app) always comes from the
 * signed-in user via {@link CurrentUserContext}; nothing here accepts it from a request.
 *
 * The voucher writer {@link #saveVoucher} is DAL 0586 {@code VoucherHead.SetData} step for step.
 * It is deliberately separate from {@code repositories.DesktopVoucherWriter} (Contra), which
 * binds nulls as typed NULL instead of omitting them, and from {@code VoucherService} (JPA).
 */
@Service
public class DesktopVoucherSupport {

    private static final Logger LOG = LoggerFactory.getLogger(DesktopVoucherSupport.class);

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;

    // ================================================================================ tenancy

    public UserAccount user()  { return ctx.requireAccountingUser(); }
    public int orgId()         { return ctx.currentOrganizationId(); }
    public int companyId()     { return ctx.currentCompanyId(); }
    public int userId()        { return ctx.currentUserId(); }
    public int yearId()        { return ctx.currentFinancialYearId(); }
    public int branchId() {
        Integer b = user().getBranchesId();
        return b == null ? 0 : b;
    }
    /** UserAccount.AppId. The desktop reads it off the login row; 0 when it cannot be resolved. */
    public int appId() {
        try { return ctx.currentAppId(); }
        catch (Exception e) {
            Integer a = user().getAppId();
            return a == null ? 0 : a;
        }
    }

    // ================================================================================= rights

    /**
     * CommonServices.SetRightsValueInRightsObject(screenName) (:17565) -
     * tblUserRights.GetByUserId -> Sp_tblUserRights_GetAllMethod @UserId @ScreenName
     * @RightName(=RoleName) @CompanyId @Activity='GetByUserId', with the desktop's Admin
     * short-circuit. Keys are the Rightsobjects the screens read.
     */
    public Map<String, Boolean> rights(String screenName) {
        Map<String, Boolean> r = new LinkedHashMap<>();
        String role = ctx.currentRoleName();
        boolean admin = "Admin".equals(role);
        r.put("save", admin);
        r.put("update", admin);
        r.put("delete", admin);
        r.put("print", admin);
        r.put("canViewAllRecord", admin);
        r.put("canChangeOrderStatusToComplete", admin);
        r.put("canChangeOrderStatusToCancel", admin);
        try {
            List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_tblUserRights_GetAllMethod",
                    DesktopProc.params("UserId", userId(), "ScreenName", screenName,
                            "RightName", role == null ? "" : role, "CompanyId", companyId(),
                            "Activity", "GetByUserId"));
            for (Map<String, Object> row : rows) {
                String name = row.get("RightName") == null ? "" : String.valueOf(row.get("RightName"));
                boolean v = toBool(row.get("Value"));
                switch (name) {
                    case "Save":              if (!admin) r.put("save", v); break;
                    case "Update":            if (!admin) r.put("update", v); break;
                    case "Print":             if (!admin) r.put("print", v); break;
                    case "CanView AllRecord": if (!admin) r.put("canViewAllRecord", v); break;
                    case "Delete":            r.put("delete", v); break;
                    case "CanChangeOrderStatusToComplete": if (!admin) r.put("canChangeOrderStatusToComplete", v); break;
                    case "CanChangeOrderStatusToCancel":   if (!admin) r.put("canChangeOrderStatusToCancel", v); break;
                    default: break;
                }
            }
        } catch (Exception e) {
            LOG.warn("Rights for {} could not be read; treating as not granted", screenName, e);
        }
        return r;
    }

    public void requireRight(String screenName, String key, String message) {
        if (!Boolean.TRUE.equals(rights(screenName).get(key))) throw new SecurityException(message);
    }

    // ======================================================================= features, config

    /** CommonServices.GetERPFeatureById(id): the id is in USP_GetERPFeaturesByCompanyId. */
    public boolean feature(int featureId) {
        try {
            for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                    DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId()))) {
                Object id = r.get("Id");
                if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
            }
        } catch (Exception e) {
            LOG.warn("ERP feature {} could not be read; treating as off", featureId, e);
        }
        return false;
    }

    /** ConfigrationsAllocation ConfigKey for a ConfigDescription, or null. */
    public String config(String description) {
        try {
            List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod",
                    DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId(),
                            "ConfigDescription", description,
                            "Activity", "GetConfigurationByOrgCompandConfigDescription"));
            if (rows.isEmpty()) return null;
            Object v = rows.get(0).get("ConfigKey");
            return v == null ? null : String.valueOf(v);
        } catch (Exception e) {
            LOG.warn("Configuration '{}' could not be read", description, e);
            return null;
        }
    }

    /**
     * CommonServices.GetDecimalConfiguration(): DefaultNoofDecimalPointsForAmount,
     * DefaultNoofDecimalPointsForRate (2 when unset) and DefaultNoofDecimalPointsForFcyAmount.
     */
    public Map<String, Object> decimals() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("amount", toInt(config("Default NoofDecimal Points For Amount")));
        int rate = toInt(config("Default NoofDecimal Points For Rate"));
        m.put("rate", rate > 0 ? rate : 2);
        m.put("fcyAmount", toInt(config("DefaultNoOfDecimalPointsForFcyAmount")));
        return m;
    }

    // ================================================================================ lookups

    /** CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll (BLL 0648:154). */
    public List<Map<String, Object>> coaAllocationSearch() {
        int uid = userId();
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod",
                DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId(),
                        "UserId", uid != 0 ? uid : null, "Activity", "COAAllocationSearch"));
    }

    /**
     * CommonServices.Accounts_GetAccountTitleByAccountTypeIds(types, typesNot, costCenter) ->
     * COAAllocation.Accounts_GetAccountTitleByAccountTypeIds (BLL 0648:520).
     */
    public List<Map<String, Object>> accountsGetAccountTitleByAccountTypeIds(String typeIds, int costCenterId) {
        int uid = userId();
        return DesktopProc.rows(jdbc, "[dbo].[USP_Accounts_GetAccountTitleByAccountTypeIds]",
                DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId(), "AppId", appId(),
                        "AccountTypeIds", empty(typeIds) ? null : typeIds,
                        "UserId", uid != 0 ? uid : null,
                        "CostCenterId", costCenterId != 0 ? costCenterId : null));
    }

    /**
     * CommonServices.CoaAllocationAccountTitleByAccountTypeIds(types, null, costCenter) ->
     * COAAllocation.GetAccountTitleByAccountTypeIds (BLL 0648:423).
     */
    public List<Map<String, Object>> coaAccountTitleByAccountTypeIds(String typeIds, int costCenterId) {
        int uid = userId();
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod",
                DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId(), "AppId", appId(),
                        "AccountTypeIds", empty(typeIds) ? null : typeIds,
                        "UserId", uid != 0 ? uid : null,
                        "CostCenterId", costCenterId != 0 ? costCenterId : null,
                        "Activity", "GetAccountTitleByAccountTypeIds"));
    }

    /** jobLot.GetList / jobLot.GetAll (BLL 0594) -> SP_JobLot_ReadMethod @Activity='GetAll'. */
    public List<Map<String, Object>> jobLots() {
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod",
                DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId(), "Activity", "GetAll"));
    }

    /** MultiCurrency.GetAll (BLL 0076) -> Sp_MultiCurrency_GetAllMethod @Activity='ReadAll'. */
    public List<Map<String, Object>> currencies() {
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod",
                DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId(), "Activity", "ReadAll"));
    }

    /** CommonServices.MultiLanguagesGetAll -> Sp_MultiLanguages_GetAll @MethodType='ReadAll'. */
    public List<Map<String, Object>> languages() {
        return DesktopProc.rows(jdbc, "Sp_MultiLanguages_GetAll",
                DesktopProc.params("MethodType", "ReadAll", "OrganizationId", orgId(), "CompanyId", companyId()));
    }

    /** VoucherHead.BindSubsidiaryAccount(CompanyId, 0, null) -> USP_GetSubsidiaryAccountsByParentAccount @CompanyId. */
    public List<Map<String, Object>> subsidiaries() {
        return DesktopProc.rows(jdbc, "USP_GetSubsidiaryAccountsByParentAccount",
                DesktopProc.params("CompanyId", companyId()));
    }

    /** clsGlobalVariables.globalBranchesAllocateToUser -> [dbo].[USP_GetBranchsAllocatedToUser]. */
    public List<Map<String, Object>> branchesAllocatedToUser() {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUser]",
                DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId(), "UserId", userId()));
    }

    /** CheqBookHeader.OutstandingCheqNo (BLL 0647:61). */
    public List<Map<String, Object>> outstandingCheques(int bankId, int recId) {
        return DesktopProc.rows(jdbc, "SP_CheqBookHeader_GetAllMethod",
                DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId(), "BankId", bankId,
                        "RecId", recId != 0 ? recId : null, "MethodType", "OutstandingCheqNo"));
    }

    /** VoucherHead.ReadByCurrentBalanceByDateAndAccountId (BLL 0654:498). */
    public List<Map<String, Object>> currentBalance(String voucherDate, int accountId) {
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Activity", "ReadByCurrentBalanceByDateAndAccountId",
                        "OrganizationId", orgId(), "CompanyId", companyId(), "FinancialYearId", yearId(),
                        "RefAccountId", accountId, "VoucherDate", voucherDate));
    }

    /** CommonServices.GenerateVoucherCode(documentTypeId) -> GenerateVoucherCodeByDocumentTypeId. */
    public int generateVoucherCode(int documentTypeId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyId(),
                        "DocumentTypeId", documentTypeId, "FinancialYearId", yearId(),
                        "BranchesId", branchId(), "Activity", "GenerateVoucherCodeByDocumentTypeId"));
        return rows.isEmpty() ? 0 : toInt(rows.get(0).get("VoucherCode"));
    }

    /**
     * VoucherReports.LedgerByJobLotDropDownAndLists (BLL 0141:3834), filtered to
     * ActivityType == "Account Header" the way every ComboBindForHistory does. @AppId and
     * @UserId are always sent (0 when the form leaves them unset); the caller passes the
     * CompanyId the form passes - several forms pass UserAccount.OrganizationId there.
     */
    public List<Map<String, Object>> historyAccountHeaders(int companyIdAsSent, int appIdAsSent,
                                                           int userIdAsSent, String documentTypeIds) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_Vouchers_LedgerByJobLot_DropDownAndLists",
                DesktopProc.params("OrganizationId", orgId(), "CompanyId", companyIdAsSent,
                        "AppId", appIdAsSent, "UserId", userIdAsSent,
                        "DocumentTypeIds", empty(documentTypeIds) ? null : documentTypeIds));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if ("Account Header".equals(r.get("ActivityType") == null ? null : String.valueOf(r.get("ActivityType")))) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", r.get("Id"));
                m.put("AccountTitle", r.get("name"));
                out.add(m);
            }
        }
        return out;
    }

    /**
     * VoucherHead.VoucherFormHistory (BLL 0654:617), parameter for parameter.
     *
     * @param dateType   "doc" | "entry" | "modify" | "approved" (the radio buttons)
     * @param approvedFilter the combo text: "Approved", "All", or the not-approved caption
     * @param appIdAsSent 0 when the form does not set ReportsParameters.AppId (omitted then)
     */
    public List<Map<String, Object>> voucherFormHistory(String documentTypeName, boolean canViewAllRecord,
            String dateType, String fromDate, String toDate, int fromDocNo, int toDocNo, int accountId,
            String approvedFilter, boolean approvedTrue, int appIdAsSent) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", orgId());
        p.put("CompanyId", companyId());
        p.put("UserId", userId());
        p.put("DocumentTypeName", documentTypeName);
        p.put("CanViewAllRecord", canViewAllRecord);
        int y = yearId();
        if (y != 0) p.put("FinancialYearId", y);
        String d = dateType == null ? "doc" : dateType;
        if ("entry".equals(d)) {
            p.put("EntryFromDate", nullIfEmpty(fromDate)); p.put("EntryToDate", nullIfEmpty(toDate));
        } else if ("modify".equals(d)) {
            p.put("ModifyFromDate", nullIfEmpty(fromDate)); p.put("ModifyToDate", nullIfEmpty(toDate));
        } else if ("approved".equals(d)) {
            p.put("ApprovedFromDate", nullIfEmpty(fromDate)); p.put("ApprovedToDate", nullIfEmpty(toDate));
        } else {
            p.put("FromDate", nullIfEmpty(fromDate)); p.put("ToDate", nullIfEmpty(toDate));
        }
        if (fromDocNo != 0) p.put("DocNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("DocNoTo", toDocNo);
        if (accountId != 0) p.put("AccountId", accountId);
        if (!canViewAllRecord) p.put("EntryUser", userId());
        if (!"All".equals(approvedFilter)) p.put("IsApproved", approvedTrue);
        if (appIdAsSent != 0) p.put("AppId", appIdAsSent);
        return DesktopProc.rows(jdbc, "USP_VoucherFormHistory", p);
    }

    // ============================================================================ read by id

    /**
     * VoucherHead.GetByID (BLL 0654:380) -> DAL 0586 GetData: the header (ReadByID), its details
     * (VoucherDetail_ReadByVoucherHeadID) and its payment schedule (Sp_PaymentDueSchedule_ReadAll).
     * The desktop never filters this by tenancy; the web refuses a voucher of another
     * organization/company or another document type, which the desktop's own history cannot list.
     */
    public Map<String, Object> readById(int id, int expectedDocumentTypeId) {
        List<Map<String, Object>> heads = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Id", id, "Activity", "ReadByID"));
        if (heads.isEmpty()) return null;
        Map<String, Object> head = heads.get(0);
        if (toInt(head.get("OrganizationId")) != orgId() || toInt(head.get("CompanyId")) != companyId()) return null;
        if (expectedDocumentTypeId != 0 && toInt(head.get("DocumentTypeId")) != expectedDocumentTypeId) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", head);
        out.put("details", DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods",
                DesktopProc.params("Id", id, "Activity", "VoucherDetail_ReadByVoucherHeadID")));
        out.put("schedules", DesktopProc.rows(jdbc, "Sp_PaymentDueSchedule_ReadAll",
                DesktopProc.params("DocumentTypeId", toInt(head.get("DocumentTypeId")), "RefDocNoId", id)));
        return out;
    }

    // ================================================================= DAL 0586 SetData writer

    /** Architecture.Model.PaymentDueSchedule, non-virtual properties. */
    public static class DueSchedule {
        public String  DueDate;
        public Double  Amount = 0d;
        public Double  DuePercentage = 0d;
        public Integer DueDays = 0;
        public Integer RefdocNoId = 0;
        public Integer refDocumentTypeId = 0;
        public Long    PaymentDueScheduleId = 0L;
    }

    /**
     * DAL 0586 {@code VoucherHead.SetData}: header insert/update, details, balance check,
     * payment schedule, history mirror, approval row - one transaction (the caller's).
     * BLL 0654 sets ActionId 1 (insert) / 2 (update) and EntryDate/ModifyDate = now before it.
     */
    @Transactional(propagation = Propagation.REQUIRED)
    public int saveVoucher(ContraVoucherDto.Head head, List<ContraVoucherDto.Detail> details,
                           List<DueSchedule> schedules) {
        boolean insert = head.Id == null || head.Id == 0;
        head.ActionId = insert ? 1 : 2;
        int n = DesktopProc.setProc(jdbc, insert ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", fields(head));
        int id = n > 0 ? n : (head.Id == null ? 0 : head.Id);
        head.Id = id;
        if (details == null || details.isEmpty()) {
            throw new IllegalStateException("Voucher Detail Not Found");
        }
        BigDecimal limit = BigDecimal.ZERO;
        for (ContraVoucherDto.Detail d : details) {
            d.VoucherHeadId = id;
            limit = limit.add(BigDecimal.valueOf(d.DebitAmount == null ? 0d : d.DebitAmount));
            d.Id = DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", fields(d));
        }
        DesktopProc.scalar(jdbc, "USP_VoucherBalanceCheck",
                DesktopProc.params("OrganizationId", head.OrganizationId, "CompanyId", head.CompanyId, "Id", id));
        if (schedules != null) {
            for (DueSchedule s : schedules) {
                s.RefdocNoId = id;
                s.refDocumentTypeId = head.DocumentTypeId;
                DesktopProc.setProc(jdbc, "Sp_PaymentDueSchedule_Insert", fields(s));
            }
        }
        int documentTypeIdRef = DesktopProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", fields(head));
        for (ContraVoucherDto.Detail d : details) {
            d.VoucherHeadId = id;
            d.DocumentTypeIdRef = documentTypeIdRef;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", fields(d));
        }
        DesktopProc.setProc(jdbc, "[DAW].[USp_DocumentApprovalDetail_Insert]",
                DesktopProc.params("OrganizationId", head.OrganizationId, "CompanyId", head.CompanyId,
                        "DocumentTypeId", head.DocumentTypeId, "Id", id, "LimitAmount", limit));
        return id;
    }

    /** GenericProvider.SetProc: one parameter per non-virtual property; null => not sent. */
    public static Map<String, Object> fields(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (java.lang.reflect.Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try { m.put(f.getName(), f.get(o)); } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    // ================================================================================ helpers

    public static boolean empty(String s) { return s == null || s.trim().isEmpty(); }
    public static String nullIfEmpty(String s) { return empty(s) ? null : s.trim(); }

    public static int toInt(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null) return 0;
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return (int) Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }

    public static double toDouble(Object o) {
        if (o instanceof Number) return ((Number) o).doubleValue();
        if (o == null) return 0d;
        String s = String.valueOf(o).trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0d; }
    }

    /** Conversion.ToBool: "1"/"true"/non-zero number is true. */
    public static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    public static String str(Object o) { return o == null ? "" : String.valueOf(o); }

    /** "yyyy-MM-dd" from whatever the page sent; null when empty. */
    public static String isoDay(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    public static String now() {
        return new java.text.SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new java.util.Date());
    }
}
