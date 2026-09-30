package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

/**
 * Screen 738 "Party Limits" = Architecture.WinApp.SupfrmSupplierLimits (base.Name frmInvSupCustLimits, caption
 * "Supplier / Customer Limits"), served on the Account Reports tile "Party Limits &amp; Balances" (R4, 2026-09-30).
 * BLL Architecture.BLL.Inventory.SupCustCreditLimitSchedule / DAL of the same name:
 *   Getall   -> Sp_SupCustCreditLimitSchedule_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadByOrganizationCompanyId'
 *   GetByID  -> Sp_SupCustCreditLimitSchedule_GetAllMethod @Id, @Activity='ReadById'
 *   Save     -> Id == 0 ? Sp_SupCustCreditLimitSchedule_Insert : Sp_SupCustCreditLimitSchedule_Update, every model
 *               property sent (GenericProvider.SetProc), one transaction (DAL SetData).
 * cmbsupplierfill -> CommonServices.SupplierCustomerGetforComboServiceBind -> SupplierCustomer.GetforComboBinding:
 *   Sp_SupplierCustomer_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadByOrganizationIdCompanyIdForBinding'.
 */
@Repository
public class PartyLimitsRepository {
    private final JdbcTemplate jdbc;

    public PartyLimitsRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public Map<String, Object> lookups(UserAccount u) {
        Map<String, Object> data = new LinkedHashMap<>();
        data.put("amountDecimals", ReportValueSupport.amountDecimals(jdbc, u));
        List<Map<String, Object>> parties = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationIdCompanyIdForBinding"))) {
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("Id", r.get("Id"));
            p.put("CompanyName", r.get("CompanyName"));
            parties.add(p);
        }
        data.put("parties", parties);
        return data;
    }

    /** datagridviewform(): the desktop copies Id, CompanyName, CreditLimit, DebitLimit, EffectedDate into its own table. */
    public List<Map<String, Object>> history(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_SupCustCreditLimitSchedule_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> row = new LinkedHashMap<>();
            row.put("Id", r.get("Id"));
            row.put("CompanyName", r.get("CompanyName"));
            row.put("CreditLimits", number(r.get("CreditLimit")));
            row.put("DebitLimits", number(r.get("DebitLimit")));
            row.put("EffectedDate", date(r.get("EffectedDate")));
            out.add(row);
        }
        return out;
    }

    /** datagridview_DoubleClick -> SupCustCreditLimitSchedule.GetByID(RecId). */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_SupCustCreditLimitSchedule_GetAllMethod",
                DesktopProc.params("Id", id, "Activity", "ReadById"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> r = rows.get(0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", r.get("Id"));
        out.put("SuplierCustomerId", r.get("SuplierCustomerId"));
        out.put("CreditLimit", number(r.get("CreditLimit")));
        out.put("DebitLimit", number(r.get("DebitLimit")));
        out.put("EffectedDate", date(r.get("EffectedDate")));
        out.put("OrganizationId", r.get("OrganizationId"));
        out.put("CompanyId", r.get("CompanyId"));
        return out;
    }

    /**
     * Insert(): obj carries Id (RecId, 0 for Save), SuplierCustomerId, DebitLimit, CreditLimit, EffectedDate
     * (txteffecteddate.Value - the picker keeps the time of day it was opened with), OrganizationId, CompanyId,
     * EntryUser = ModifyUser = UserAccount.ID, EntryDate = ModifyDate = DateTime.Now. All eleven are sent.
     */
    @Transactional
    public int save(UserAccount u, int id, int partyId, double debit, double credit, LocalDate effected) {
        LocalDateTime now = LocalDateTime.now();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("CompanyId", u.getCompanyId());
        p.put("CreditLimit", credit);
        p.put("DebitLimit", debit);
        p.put("EffectedDate", Timestamp.valueOf(LocalDateTime.of(effected, LocalTime.now())));
        p.put("EntryDate", Timestamp.valueOf(now));
        p.put("EntryUser", u.getId());
        p.put("Id", id);
        p.put("ModifyDate", Timestamp.valueOf(now));
        p.put("ModifyUser", u.getId());
        p.put("OrganizationId", u.getOrganizationId());
        p.put("SuplierCustomerId", partyId);
        int n = DesktopProc.setProc(jdbc, id == 0 ? "Sp_SupCustCreditLimitSchedule_Insert" : "Sp_SupCustCreditLimitSchedule_Update", p);
        return n > 0 ? n : id;
    }

    private static Object number(Object v) {
        return v instanceof java.math.BigDecimal ? ((java.math.BigDecimal) v).toPlainString() : v;
    }

    private static String date(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        String s = String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
